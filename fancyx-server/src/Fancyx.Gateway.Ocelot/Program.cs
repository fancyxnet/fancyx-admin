using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

var serviceMode = builder.Configuration["ServiceMode"];
var configurationBuilder = new ConfigurationBuilder();
// OcelotÍø¹ØÅäÖÃ
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
// Íø¹Ø¿çÓò
if (!string.IsNullOrEmpty(builder.Configuration["CorsOrigins"]))
{
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
}

if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddConsole();
}

var app = builder.Build();

app.UseCors();
await app.UseOcelot();

app.Run();