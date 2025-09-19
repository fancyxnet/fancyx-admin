using Fancyx.Admin;
using Fancyx.Core;
using Fancyx.Logger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseFancyxLogger();
builder.Host.UseAutofac();

builder.AddApplication<FancyxAdminModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();