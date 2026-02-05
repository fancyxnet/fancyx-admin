using Consul;
using Cracker.Consul;
using Cracker.Serilog;
using Elastic.CommonSchema;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Configuration.ConfigProvider;

var builder = WebApplication.CreateBuilder(args);

var isUseConsul = builder.Configuration["ServiceMode"] == "Consul";
if (isUseConsul)
{
    var consulConfigurationOptions = new ConsulConfigurationOptions
    {
        Address = builder.Configuration["Consul:Host"]!,
        Token = builder.Configuration["Consul:Token"]!,
    };
    builder.Configuration.AddConsulConfiguration($"{builder.Configuration["Consul:NodeName"]}/appsettings.json", consulConfigurationOptions);
    builder.Services.AddConsulDiscovery(new ConsulDiscoveryOptions
    {
        Address = consulConfigurationOptions.Address,
        Token = consulConfigurationOptions.Token,
        SetupNodeAction = o =>
        {
            o.Name = builder.Configuration["Consul:NodeName"]!;
            o.Server = builder.Configuration["Consul:NodeAddress"]!;
            o.HttpPort = int.Parse(builder.Configuration["Consul:HttpPort"]!);
            o.HttpPort = int.Parse(builder.Configuration["Consul:GrpcPort"]!);
        }
    });
    builder.Services.AddSingleton<IProxyConfigProvider, ConsulConfigurationConfigProvider>();
    builder.Services.AddReverseProxy();
}
else
{
    builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
}

builder.Host.UseSerilogSetup("yarp-gateway");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(builder.Configuration["CorsOrigins"]?
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToArray() ?? [])
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (isUseConsul)
{
    app.RegisterNode();
    app.Map(ConsulConstant.ConsulHealthUrl, () =>
    {
        return Results.Ok("yarp is ok");
    });
}

app.UseCors();
app.UseWebSockets();
app.MapReverseProxy();

app.Run();
