using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

var serviceMode = builder.Configuration["ServiceMode"];
var configurationBuilder = new ConfigurationBuilder();
if (serviceMode == "Direct")
{
    configurationBuilder.AddJsonFile("ocelot.direct.json");
    builder.Services.AddOcelot(configurationBuilder.Build());
}
else
{
    configurationBuilder.AddJsonFile("ocelot.consul.json");
    builder.Services.AddOcelot(configurationBuilder.Build()).AddConsul();
}

if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddConsole();
}

var app = builder.Build();

await app.UseOcelot();

app.Run();