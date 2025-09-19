using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Builder;

namespace Fancyx.Logger
{
    public static class LoggerExtension
    {
        public static void UseFancyxLogger(this ConfigureHostBuilder builder)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", DateTime.Now.ToString("yyyy-MM"));
            Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File(Path.Combine(logPath, "log.txt"), rollingInterval: RollingInterval.Day))
            .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Error)
            .WriteTo.File(Path.Combine(logPath, "error.txt"), rollingInterval: RollingInterval.Day))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();
            builder.UseSerilog();
        }
    }
}