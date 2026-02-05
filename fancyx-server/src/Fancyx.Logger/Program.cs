using Cracker.AspNetCore;
using Cracker.AssemblyScanner;
using Cracker.Serilog;
using Fancyx.Logger;
using Fancyx.Shared;
using Fancyx.Shared.Consts;

MultiTenancyVars.SetIsEnabled(true);
AssemblyManager.Instance.Initialize(options => { options.AssemblyPrefixes = ["Fancyx"]; });

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilogSetup(MicroServiceConsts.Logger);

builder.AddApplication<FancyxLoggerModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();