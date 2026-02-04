using Cracker.AspNetCore;
using Fancyx.Logger;
using Cracker.Serilog;
using Fancyx.Shared.Consts;
using Fancyx.Shared;

MultiTenancyVars.SetIsEnabled(true);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.Logger);

builder.AddApplication<FancyxLoggerModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();