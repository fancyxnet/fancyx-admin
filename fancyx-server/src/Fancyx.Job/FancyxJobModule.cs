using Fancyx.Admin.EfCore;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Job.Database;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace Fancyx.Job
{
    [DependsOn(
        typeof(FancyxAdminEfCoreModule)
        )]
    public class FancyxJobModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler().ConfigureAwait(true).GetAwaiter().GetResult();
            context.Services.AddSingleton<IScheduler>(r => scheduler);
            context.Services.AddHostedService<JobInitHostService>();
            context.Services.AddScoped<IJobControl, JobService>();
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            IScheduler? scheduler = context.ServiceProvider.GetService<IScheduler>();
            if (scheduler != null)
            {
                scheduler.JobFactory = new JobFactory(context.ServiceProvider);
                scheduler.ListenerManager.AddJobListener(new GlobalJobListener(context.ServiceProvider), EverythingMatcher<JobKey>.AllJobs());
                scheduler.Start().ConfigureAwait(false);
            }
        }
    }
}