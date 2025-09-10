using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Fancyx.Core.Helpers;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.Payment;
using Fancyx.DataAccess.Enums;
using Fancyx.Payment.Exceptions;
using Fancyx.Payment.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RedLockNet.SERedis;

namespace Fancyx.Payment.Services
{
    internal class AlipayService : IPayNormalize
    {
        private readonly IRepository<PayProvider> _payProviderRepository;
        private readonly IRepository<PaymentOrder> _paymentOrderRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AlipayService> _logger;
        private readonly RedLockFactory _redLockFactory;

        public PaymentType PayType => PaymentType.AliPay;

        public AlipayService(IRepository<PayProvider> payProviderRepository, IRepository<PaymentOrder> paymentOrderRepository, IConfiguration configuration, ILogger<AlipayService> logger, RedLockFactory redLockFactory)
        {
            _payProviderRepository = payProviderRepository;
            _paymentOrderRepository = paymentOrderRepository;
            _configuration = configuration;
            _logger = logger;
            _redLockFactory = redLockFactory;
        }

        public async Task<PayResult> CreateOrderAsync(CreateOrderRequest req)
        {
            var expiry = TimeSpan.FromSeconds(10);
            var wait = TimeSpan.FromSeconds(3);
            var retry = TimeSpan.FromSeconds(1);

            using var redLock = await _redLockFactory.CreateLockAsync($"CreateOrder_User_{req.UserId}", expiry, wait, retry);
            if (!redLock.IsAcquired)
            {
                throw new PaymentFailureException("用户操作太快");
            }

            var provider = await _payProviderRepository.GetAsync(x => x.IsEnabled && x.Type == this.PayType)
                ?? throw new NotFoundPayProviderException(this.PayType);
            //创建付款单
            var paymentOrder = new PaymentOrder()
            {
                ProviderId = provider.Id,
                Type = this.PayType,
                OrderNo = $"M{SnowflakeHelper.Instance.NextId()}",
                PayStatus = PayStatus.Processing.GetStatus(),
                InitiationTime = DateTime.Now,
                OrderAmount = req.TotalAmount,
                UserId = req.UserId,
                PayDesc = req.Subject
            };
            await _paymentOrderRepository.InsertAsync(paymentOrder);

            var client = await GetAlipayClientAsync(provider);
            AlipayTradePagePayRequest request = new();
            request.SetNotifyUrl(provider.NotifyUrl);

            AlipayTradePagePayModel model = new()
            {
                OutTradeNo = paymentOrder.OrderNo,
                TotalAmount = paymentOrder.OrderAmount.ToString("F2"),
                Subject = req.Subject,
                ProductCode = "FAST_INSTANT_TRADE_PAY"
            };
            request.SetBizModel(model);

            AlipayTradePagePayResponse response = client.pageExecute(request, null, "POST");
            var result = new PayResult
            {
                OrderNo = paymentOrder.OrderNo,
                RedirectUrl = response.Body
            };
            if (!EnsureSuccess(response))
            {
                _logger.LogError("支付宝下单失败: {@result}", result);
                return result;
            }
            result.IsSuccess = true;
            return result;
        }

        public async Task<TradeQueryResult> QueryTradeStatusAsync(string orderNo)
        {
            var paymentOrder = await _paymentOrderRepository.GetAsync(x => x.OrderNo == orderNo)
                ?? throw new PaymentFailureException($"找不到单号{orderNo}的付款单");
            var payProvider = await _payProviderRepository.FindAsync(paymentOrder.ProviderId)
                ?? throw new PaymentFailureException($"找不到{paymentOrder.ProviderId}的支付渠道");

            var client = await GetAlipayClientAsync(payProvider);
            var request = new AlipayTradeQueryRequest();
            var model = new AlipayTradeQueryModel
            {
                OutTradeNo = paymentOrder.OrderNo
            };
            request.SetBizModel(model);
            AlipayTradeQueryResponse response = client.Execute(request);
            var result = new TradeQueryResult()
            {
                OrderNo = paymentOrder.OrderNo,
                PayStatus = paymentOrder.PayStatus
            };
            if (EnsureSuccess(response))
            {
                if (result.PayStatus != PayStatus.Processing.GetStatus())
                {
                    if (response.TradeStatus == "TRADE_SUCCESS" || response.TradeStatus == "TRADE_FINISHED")
                    {
                        result.PayStatus = PayStatus.Success.GetStatus();
                    }
                    else if (response.TradeStatus == "WAIT_BUYER_PAY")
                    {
                        result.PayStatus = PayStatus.Processing.GetStatus();
                    }
                    else
                    {
                        result.PayStatus = PayStatus.Failed.GetStatus();
                    }
                }

                result.TradeNo = response.TradeNo;
                result.OutTradeNo = response.OutTradeNo;
                result.TradeStatus = response.TradeStatus;
                result.TotalAmount = decimal.Parse(response.TotalAmount);
                result.BuyerUserId = response.BuyerUserId;
            }
            else
            {
                if (result.PayStatus != PayStatus.Processing.GetStatus())
                {
                    result.PayStatus = PayStatus.Failed.GetStatus();
                }
            }

            return result;
        }

        public async Task<PayCallBackResult> CallBackAsync(Dictionary<string, string> notifyMap)
        {
            if (!notifyMap.TryGetValue("notify_type", out string? notifyType)) throw new PayCallBackException(true, "notify_type");
            if (!notifyMap.TryGetValue("app_id", out string? appId)) throw new PayCallBackException(true, "app_id");
            if (!notifyMap.TryGetValue("trade_status", out string? tradeStatus)) throw new PayCallBackException(true, "trade_status");
            if (!notifyMap.TryGetValue("out_trade_no", out string? outTradeNo)) throw new PayCallBackException(true, "out_trade_no");
            if (!notifyMap.TryGetValue("total_amount", out string? totalAmount)) throw new PayCallBackException(true, "total_amount");

            if (notifyType != "trade_status_sync")
            {
                throw new PayCallBackException($"不支持的回调类型: {notifyType}");
            }

            //验签
            var provider = await _payProviderRepository.GetAsync(x => x.IsEnabled && x.Type == this.PayType && x.AppId == appId)
                ?? throw new NotFoundPayProviderException(this.PayType);
            string signKeyOrCert;
            if (provider.SignMode == "key")
            {
                signKeyOrCert = provider.ProviderPublicKey!;
                if (!AlipaySignature.RSACheckV1(notifyMap, signKeyOrCert, "utf-8", "RSA2", false))
                {
                    throw new PayCallBackException($"支付宝公钥模式验签失败，渠道ID:{provider.Id}");
                }
            }
            else
            {
                signKeyOrCert = provider.ProviderCertPath!;
                var crtDirPath = AppDomain.CurrentDomain.BaseDirectory;
                await DownloadCertificateAsync(signKeyOrCert);
                if (!AlipaySignature.RSACertCheckV1(notifyMap, crtDirPath + signKeyOrCert, "utf-8", "RSA2"))
                {
                    throw new PayCallBackException($"支付宝证书模式验签失败，渠道ID:{provider.Id}");
                }
            }

            var expiry = TimeSpan.FromSeconds(10);
            var wait = TimeSpan.FromSeconds(3);
            var retry = TimeSpan.FromSeconds(1);

            using var redLock = await _redLockFactory.CreateLockAsync($"CallBack_{outTradeNo}", expiry, wait, retry);
            if (!redLock.IsAcquired)
            {
                throw new PayCallBackException("回调频率过高");
            }

            //找到对应的付款单
            var paymentOrder = await _paymentOrderRepository.GetAsync(x => x.OrderNo == outTradeNo && x.PayStatus == PayStatus.Processing.GetStatus())
                ?? throw new PayCallBackException($"找不到单号{outTradeNo}的付款单");
            var result = new PayCallBackResult();
            if (tradeStatus == "TRADE_SUCCESS" || tradeStatus == "TRADE_FINISHED") //TRADE_SUCCESS:交易支付成功；TRADE_FINISHED:交易结束，不可退款
            {
                paymentOrder.PayStatus = PayStatus.Success.GetStatus();
                paymentOrder.RealAmount = decimal.Parse(totalAmount);
                if (notifyMap.TryGetValue("gmt_payment", out string? gmtPayment) && DateTime.TryParse(gmtPayment, out DateTime gmtPaymentTime))
                {
                    paymentOrder.PayedTime = gmtPaymentTime;
                }
                else
                {
                    paymentOrder.PayedTime = DateTime.Now;
                }
            }
            else if (tradeStatus == "WAIT_BUYER_PAY") //交易创建，等待买家付款
            {
                paymentOrder.PayStatus = PayStatus.Processing.GetStatus();
            }
            else if (tradeStatus == "TRADE_CLOSED") //未付款交易超时关闭，或支付完成后全额退款
            {
                if (notifyMap.TryGetValue("refund_fee", out string? refundFee) && decimal.TryParse(refundFee, out decimal refundAmount) && refundAmount > 0)
                {
                    //退款金额大于0，发生退款
                    paymentOrder.PayStatus = PayStatus.Refunded.GetStatus();
                    paymentOrder.RefundAmount = refundAmount * 100;
                    if (notifyMap.TryGetValue("gmt_refund", out string? gmtRefund) && DateTime.TryParse(gmtRefund, out DateTime gmtRefundTime))
                    {
                        paymentOrder.RefundTime = gmtRefundTime;
                    }
                    else
                    {
                        paymentOrder.RefundTime = DateTime.Now;
                    }
                }
                else
                {
                    paymentOrder.PayStatus = PayStatus.Timeout.GetStatus();
                    paymentOrder.TimeoutTime = DateTime.Now;
                }
            }
            else
            {
                throw new PayCallBackException("不支持的交易状态");
            }

            if (notifyMap.TryGetValue("trade_no", out string? tradeNo))
            {
                paymentOrder.TradeNo = tradeNo;
            }

            await _paymentOrderRepository.UpdateAsync(paymentOrder);
            result.PayStatus = paymentOrder.PayStatus;

            return result;
        }

        public async Task<RefundResult> RefundAsync(RefundRequest req)
        {
            /* 目前沙盒测试，支付宝只能退款3次，超过3次会提示退款金额无效 */

            var result = new RefundResult() { IsSuccess = false };
            var paymentOrder = await _paymentOrderRepository.GetAsync(x => x.OrderNo == req.OrderNo)
                ?? throw new RefundException($"找不到单号{req.OrderNo}的付款单");
            var payProvider = await _payProviderRepository.FindAsync(paymentOrder.ProviderId)
                ?? throw new RefundException($"找不到{paymentOrder.ProviderId}的支付渠道");
            if (paymentOrder.PayStatus != PayStatus.Success.GetStatus() && paymentOrder.PayStatus != PayStatus.Refunded.GetStatus())
            {
                throw new RefundException("付款单状态不正确，不能进行退款操作");
            }
            if (paymentOrder.PayStatus == PayStatus.Refunded.GetStatus() && paymentOrder.RefundAmount >= paymentOrder.RealAmount)
            {
                throw new RefundException("付款单已全部退款");
            }
            if (req.RefundAmount > paymentOrder.RealAmount - paymentOrder.RefundAmount)
            {
                throw new RefundException("退款金额不能大于实际支付金额");
            }

            var expiry = TimeSpan.FromSeconds(10);
            var wait = TimeSpan.FromSeconds(3);
            var retry = TimeSpan.FromSeconds(1);

            using var redLock = await _redLockFactory.CreateLockAsync($"Refund_{req.OrderNo}", expiry, wait, retry);
            if (!redLock.IsAcquired)
            {
                throw new RefundException("有一笔退款进行中，请稍候再试");
            }

            paymentOrder.RefundNo = $"R{SnowflakeHelper.Instance.NextId()}";
            paymentOrder.RefundTime = DateTime.Now;
            paymentOrder.RefundAmount += req.RefundAmount;
            paymentOrder.PayStatus = PayStatus.Refunded.GetStatus();
            paymentOrder.RefundReason += $"{req.Reason}；";

            var client = await GetAlipayClientAsync(payProvider);
            var request = new AlipayTradeRefundRequest();
            var model = new AlipayTradeRefundModel
            {
                RefundAmount = paymentOrder.RefundAmount.ToString("F2"),
                OutTradeNo = paymentOrder.OrderNo,
                OutRequestNo = paymentOrder.RefundNo,
                RefundReason = req.Reason
            };
            request.SetBizModel(model);

            AlipayTradeRefundResponse response = client.Execute(request);
            result.Code = response.Code;
            result.Msg = response.Msg;
            result.SubCode = response.SubCode;
            result.SubMsg = response.SubMsg;
            if (EnsureSuccess(response))
            {
                result.IsSuccess = true;
                await _paymentOrderRepository.UpdateAsync(paymentOrder);
            }
            return result;
        }

        /// <summary>
        /// 确保支付宝操作成功
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static bool EnsureSuccess(AopResponse response)
        {
            return !response.IsError && response.Code == "10000";
        }

        /// <summary>
        /// 获取支付宝客户端
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private async Task<IAopClient> GetAlipayClientAsync(PayProvider provider)
        {
            if (provider.SignMode == "key")
            {
                return new DefaultAopClient(provider.Gateway, provider.AppId, provider.AppPrivateKey, "json", "1.0", "RSA2", provider.ProviderPublicKey, "utf-8", false);
            }
            else
            {
                //下载证书文件
                await Task.WhenAll(DownloadCertificateAsync(provider.ProviderCertPath), DownloadCertificateAsync(provider.AppCertPath), DownloadCertificateAsync(provider.RootCertPath));

                var crtDirPath = AppDomain.CurrentDomain.BaseDirectory;
                CertParams certParams = new()
                {
                    AlipayPublicCertPath = crtDirPath + provider.ProviderCertPath,
                    AppCertPath = crtDirPath + provider.AppCertPath,
                    RootCertPath = crtDirPath + provider.RootCertPath
                };
                return new DefaultAopClient(provider.Gateway, provider.AppId, provider.AppPrivateKey, "json", "1.0", "RSA2", "utf-8", "false", certParams);
            }
        }

        /// <summary>
        /// 下载证书
        /// </summary>
        /// <param name="certPath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task DownloadCertificateAsync(string? certPath)
        {
            if (string.IsNullOrEmpty(certPath))
            {
                throw new ArgumentNullException(nameof(certPath), "证书路径不能为空");
            }
            var dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetDirectoryName(certPath) ?? "");
            var localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certPath);
            if (!File.Exists(localPath))
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(new Uri(this.OssBaseUri, certPath));
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsByteArrayAsync();
                    if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                    await File.WriteAllBytesAsync(localPath, content);
                }
            }
        }

        public Uri OssBaseUri => new(_configuration["Oss:Domain"]!);
    }
}