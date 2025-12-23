using Fancyx.Core;
using Fancyx.Erp;
using Fancyx.Serilog;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi.Micro;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.ErpApi);
builder.Host.UseAutofac();
builder.Services.AddMicroService(builder.Configuration);

builder.AddApplication<FancyxErpModule>();

var app = builder.Build();

app.UseMicroDiscovery();
app.InitializeApplication();

app.Run();