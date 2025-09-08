using EnumsNET;
using Fancyx.DataAccess.Enums;

namespace Fancyx.Payment.Exceptions
{
    public class NotFoundPayProviderException(PaymentType type) : Exception($"{type.AsString(EnumFormat.Description)}没有可用的支付渠道")
    {
    }
}