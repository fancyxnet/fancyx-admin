using Fancyx.Core;
using Fancyx.Logger;
using Fancyx.Serilog;
using Fancyx.Shared.Consts;

MultiTenancyVars.SetIsEnabled(true);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.Logger);

builder.AddApplication<FancyxLoggerModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();