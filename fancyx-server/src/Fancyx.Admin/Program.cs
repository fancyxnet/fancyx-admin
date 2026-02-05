using Cracker.AspNetCore;
using Cracker.AssemblyScanner;
using Cracker.Serilog;
using Fancyx.Admin;
using Fancyx.Shared;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi.Micro;

MultiTenancyVars.SetIsEnabled(true);
AssemblyManager.Instance.Initialize(options => { options.AssemblyPrefixes = ["Fancyx"]; });

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.AdminApi);
builder.Services.AddMicroService(builder.Configuration);

builder.AddApplication<FancyxAdminModule>();

var app = builder.Build();

app.UseMicroDiscovery();
app.InitializeApplication();

app.Run();