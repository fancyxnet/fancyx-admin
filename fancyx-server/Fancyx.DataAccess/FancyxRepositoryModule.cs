using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Fancyx.DataAccess
{
    public class FancyxRepositoryModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //context.Services.AddScoped<UnitOfWorkManager>(r => new UnitOfWorkManager(createFreeSql()));
            context.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
    }
}