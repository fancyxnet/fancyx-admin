using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace Fancyx.Serilog
{
    public static class LoggerExtension
    {
        public static void UseSerilogSetup(this ConfigureHostBuilder builder, string serviceName)
        {
            builder.UseSerilog((ctx, config) =>
            {
                config.Enrich.WithProperty("service.name", serviceName)
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext();

                var loggerCnf = ctx.Configuration.GetSection("Logger");

                if (loggerCnf["Output"] == "File")
                {
                    var logDir = loggerCnf["FilePath"] ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                    var logPath = Path.Combine(logDir, DateTime.Now.ToString("yyyy-MM"));
                    config.WriteTo.Async(c => c.File(Path.Combine(logPath, "log.txt"), rollingInterval: RollingInterval.Day))
                        .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Error)
                        .WriteTo.File(Path.Combine(logPath, "error.txt"), rollingInterval: RollingInterval.Day))
                        .WriteTo.Async(c => c.Console());
                }
                else
                {
                    config.WriteTo.Elasticsearch([new Uri(loggerCnf["Es:Url"]!)], opts =>
                    {
                        opts.DataStream = new DataStreamName("logs", loggerCnf["Es:DataSet"]!, loggerCnf["Es:Namespace"]!);
                        opts.BootstrapMethod = BootstrapMethod.Failure;
                        opts.ConfigureChannel = channelOpts =>
                        {
                            channelOpts.BufferOptions = new BufferOptions{};
                        };
                    }, transport =>
                    {
#if DEBUG
                        transport.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
#endif
                        transport.Authentication(new ApiKey(loggerCnf["Es:ApiKey"]!));
                    });
                }
            });
        }
    }
}