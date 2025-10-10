using System.Reflection;
using System.Threading.RateLimiting;
using Fancyx.Admin.Application;
using Fancyx.Core.AutoInject;
using Fancyx.Core.Context;
using Fancyx.Shared.Consts;
using Fancyx.Shared.WebApi;
using Fancyx.Shared.WebApi.JsonConverters;
using Fancyx.Storage;
using Fancyx.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;

namespace Fancyx.Admin
{
    [DependsOn(
        typeof(FancyxStorageModule),
        typeof(FancyxSharedWebApiModule),
        typeof(FancyxAdminApplicationModule)
        )]
    public class FancyxAdminModule : ModuleBase
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = context.Configuration;

            services.Configure<MvcNewtonsoftJsonOptions>(options =>
            {
                options.SerializerSettings.Converters.Add(new LongToStringConverter());
                options.SerializerSettings.Converters.Add(new NullableLongToStringConverter());
                options.SerializerSettings.Converters.Add(new DateTimeToStringConverter());
                options.SerializerSettings.Converters.Add(new NullableDateTimeToStringConverter());
            });

            //Swagger
            services.AddSwaggerGenPro("Fancyx Admin Api", c =>
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
            services.AddHostedService<PreparationHostService>();
            //限流
            services.AddRateLimiter(options =>
            {
                // 防抖1秒内1次
                options.AddFixedWindowLimiter(RateLimiterConsts.DebouncePolicy, opt =>
                {
                    opt.PermitLimit = 1;
                    opt.Window = TimeSpan.FromSeconds(1);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                // 滑动窗口限流
                options.AddSlidingWindowLimiter(RateLimiterConsts.SlidingPolicy, opt =>
                {
                    opt.PermitLimit = 10;
                    opt.Window = TimeSpan.FromSeconds(10);
                    opt.SegmentsPerWindow = 2; // 分2段统计
                });

                // 自定义被限流时的响应
                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                    context.HttpContext.Response.ContentType = "application/json";

                    await context.HttpContext.Response.WriteAsJsonAsync(new AppResponse<bool>(ErrorCode.ApiLimit, "操作频繁，请稍后再试").SetData(false), cancellationToken);
                };
            });
        }

        public override void Configure(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (context.Environment.IsDevelopment())
            {
                app.UseSwaggerPro();
            }

            app.UseStaticFiles();
            app.UseRateLimiter(); // 启用限流中间件
        }
    }
}