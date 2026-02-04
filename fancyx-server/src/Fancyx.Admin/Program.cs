using Fancyx.Admin;
using Cracker.AspNetCore;
using Cracker.Serilog;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi.Micro;
using Fancyx.Shared;

MultiTenancyVars.SetIsEnabled(true);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.AdminApi);
builder.Services.AddMicroService(builder.Configuration);

builder.AddApplication<FancyxAdminModule>();

var app = builder.Build();

app.UseMicroDiscovery();
app.InitializeApplication();

app.Run();