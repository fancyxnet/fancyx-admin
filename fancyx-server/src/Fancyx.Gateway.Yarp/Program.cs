using Fancyx.Consul;

var builder = WebApplication.CreateBuilder(args);

var isUseConsul = builder.Configuration["ServiceMode"] == "Consul";
if (isUseConsul)
{
    builder.Services.AddConsulSetup(builder.Configuration);
}

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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
    ConsulRegistration.Register(app.Services);
    app.Map(ConsulConstant.ConsulHealthUrl, () =>
    {
        return Results.Ok("yarp is ok");
    });
}

app.UseCors();
app.MapReverseProxy();

app.Run();
