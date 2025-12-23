using Fancyx.Admin;
using Fancyx.Core;
using Fancyx.Serilog;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi.Micro;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.AdminApi);
builder.Host.UseAutofac();
builder.Services.AddMicroService(builder.Configuration);

builder.AddApplication<FancyxAdminModule>();

var app = builder.Build();

app.UseMicroDiscovery();
app.InitializeApplication();

app.Run();