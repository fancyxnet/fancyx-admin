using Cracker.AspNetCore;
using Fancyx.Erp;
using Cracker.Serilog;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi.Micro;
using Fancyx.Shared;

MultiTenancyVars.SetIsEnabled(true);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.ErpApi);
builder.Services.AddMicroService(builder.Configuration);

builder.AddApplication<FancyxErpModule>();

var app = builder.Build();

app.UseMicroDiscovery();
app.InitializeApplication();

app.Run();