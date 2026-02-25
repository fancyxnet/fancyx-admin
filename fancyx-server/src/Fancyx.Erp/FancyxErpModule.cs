using Cracker.AspNetCore.AutoInject;
using Cracker.AspNetCore.Context;
using Cracker.Caching;
using Cracker.IdentityServer;
using Cracker.IdentityServer.Abstractions;
using Cracker.Swagger;
using Fancyx.Erp.Application;
using Fancyx.Erp.Application.Remote;
using Fancyx.Internal.Grpc;
using Fancyx.Internal.Grpc.System;
using Fancyx.Shared;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi;
using Fancyx.Shared.WebApi.JsonConverters;
using Fancyx.Shared.WebApi.Micro;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;

namespace Fancyx.Erp
{
    [DependsOn(
        typeof(FancyxSharedWebApiModule),
        typeof(FancyxErpApplicationModule)
        )]
    public class FancyxErpModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new LongToStringConverter());
                options.JsonSerializerOptions.Converters.Add(new NullableLongToStringConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeToStringConverter());
                options.JsonSerializerOptions.Converters.Add(new NullableDateTimeToStringConverter());
            });

            context.Services.AddSwaggerGenPro("Fancyx Erp Api", c =>
            {
                // 添加 JWT 认证支持到 Swagger
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // 必须小写
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                var tenantScheme = new OpenApiSecurityScheme
                {
                    Name = "X-Tenant",
                    Description = "租户ID",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference
                    {
                        Id = "X-Tenant",
                        Type = ReferenceType.SecurityScheme,
                    },
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityDefinition(tenantScheme.Reference.Id, tenantScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()},
                    {tenantScheme, Array.Empty<string>()},
                });

                // 设置Swagger读取XML注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            context.Services.AddRemoteClient(context.Configuration, client =>
            {
                client.AddHttp<ITestApi>(MicroServiceConsts.AdminApi)
                      .AddGrpc<Test.TestClient>(MicroServiceConsts.AdminApi)
                      .AddGrpc<Dict.DictClient>(MicroServiceConsts.AdminApi);
            });
            var conn = ConnectionMultiplexer.Connect(context.Configuration["Redis:Connection"]!);
            RedisHelper.Instance.Initialize(conn);
            context.Services.AddCacheClient(options =>
            {
                options.GetCachingPrefix = (sp) =>
                {
                    var ctx = sp.GetRequiredService<IHttpContextAccessor>()?.HttpContext;
                    if (ctx != null)
                    {
                        var tenant = ctx.RequestServices.GetRequiredService<ICurrentTenant>();
                        if (tenant != null && !string.IsNullOrEmpty(tenant.TenantId))
                        {
                            return $"tenant:{tenant.TenantId}:";
                        }
                    }
                    return string.Empty;
                };
            });
            context.Services.AddIdentityServer(options =>
            {
                options.Jwt = new JwtOptions
                {
                    ClockSkew = Convert.ToInt32(context.Configuration.GetSection("Jwt")["ClockSkew"]),
                    ValidAudience = context.Configuration.GetSection("Jwt")["ValidAudience"]!,
                    ValidIssuer = context.Configuration.GetSection("Jwt")["ValidIssuer"]!,
                    IssuerSigningKey = context.Configuration.GetSection("Jwt")["IssuerSigningKey"]!
                };
            });
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            if (context.Application.Environment.IsDevelopment())
            {
                context.Application.UseSwaggerPro();
            }
            if (MultiTenancyVars.IsEnabled)
            {
                context.Application.UseMultiTenancy();
            }
            context.Application.UseAuthentication();
            context.Application.UseAuthorization();
            context.Application.UseCurrentUser();
        }
    }
}