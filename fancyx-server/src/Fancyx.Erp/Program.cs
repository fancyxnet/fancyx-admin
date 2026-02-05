using Cracker.AspNetCore;
using Cracker.AssemblyScanner;
using Cracker.Serilog;
using Fancyx.Erp;
using Fancyx.Shared;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi.Micro;

MultiTenancyVars.SetIsEnabled(true);
AssemblyManager.Instance.Initialize(options => { options.AssemblyPrefixes = ["Fancyx"]; });

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.ErpApi);
builder.Services.AddMicroService(builder.Configuration);

builder.AddApplication<FancyxErpModule>();

var app = builder.Build();

app.UseMicroDiscovery();
app.InitializeApplication();

app.Run();