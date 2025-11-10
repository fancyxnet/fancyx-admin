using Fancyx.Gateway.Ocelot;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

var serviceMode = builder.Configuration["ServiceMode"];
var configurationBuilder = new ConfigurationBuilder();
// OcelotÕ¯πÿ≈‰÷√
if (serviceMode == "Direct")
{
    configurationBuilder.AddJsonFile("ocelot.direct.json");
    builder.Services.AddOcelot(configurationBuilder.Build())
        .AddDelegatingHandler<TenantDelegatingHandler>(true);
}
else
{
    configurationBuilder.AddJsonFile("ocelot.consul.json");
    builder.Services.AddOcelot(configurationBuilder.Build()).AddConsul()
        .AddDelegatingHandler<TenantDelegatingHandler>(true);
}
// Õ¯πÿøÁ”Ú
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