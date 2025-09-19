using Fancyx.Admin;
using Fancyx.Core;
using Fancyx.Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup();
builder.Host.UseAutofac();

builder.AddApplication<FancyxAdminModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();