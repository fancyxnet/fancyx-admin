using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Erp.Application;
using Fancyx.Erp.Application.Remote;
using Fancyx.Internal.Grpc;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi;
using Fancyx.Shared.WebApi.Micro;
using Fancyx.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
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
                      .AddGrpc<Test.TestClient>(MicroServiceConsts.AdminApi);
            });
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (context.Environment.IsDevelopment())
            {
                app.UseSwaggerPro();
            }
        }
    }
}