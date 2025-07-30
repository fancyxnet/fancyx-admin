using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Job;
using Fancyx.Logger;
using Fancyx.Payment.Enums;
using Fancyx.Payment.Services;
using Fancyx.Redis;
using Fancyx.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.Payment
{
    [DependsOn(
        typeof(FancyxRepositoryModule),
        typeof(FancyxLoggerModule),
        typeof(FancyxRedisModule),
        typeof(FancyxJobModule)
        )]
    public class FancyxPaymentModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddKeyedScoped<IPayNormalize, AlipayService>(PaymentType.AliPay);
        }

        public override void Configure(ApplicationInitializationContext context)
        {
        }
    }
}