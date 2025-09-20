using Fancyx.Erp;
using Fancyx.Core;
using Fancyx.Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup();
builder.Host.UseAutofac();

builder.AddApplication<FancyxErpModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();