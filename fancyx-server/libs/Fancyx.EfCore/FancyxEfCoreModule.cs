using Fancyx.Core;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Fancyx.EfCore
{
    public class FancyxEfCoreModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var baseContextType = typeof(EfCoreDbContextBase);
            var dbContextType = FrameConfiguration.LoadAssemblies.SelectMany(a => a.DefinedTypes)
                .FirstOrDefault(t => !t.IsAbstract && !t.IsSealed && !t.IsInterface && t.IsAssignableTo(baseContextType)) ?? throw new Exception("数据库上下文必须继承EfCoreDbContextBase.");
            context.Services.TryAddScoped<DbContext>(r =>
            {
                var contextOptionsBuilder = new DbContextOptionsBuilder()
                .UseNpgsql(context.Configuration.GetConnectionString("Default"))
                     .LogTo(Console.WriteLine, LogLevel.Information)
#if DEBUG
                     .EnableSensitiveDataLogging()
#endif
                     .EnableDetailedErrors();
                return (DbContext)Activator.CreateInstance(dbContextType, contextOptionsBuilder.Options, r)!;
            });
            context.Services.TryAddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            context.Services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
    }
}