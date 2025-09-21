using Fancyx.Assemblies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Fancyx.Swagger
{
    public static class SwaggerExtension
    {
        public static Dictionary<string, string> Groups { get; private set; } = [];

        public static void AddSwaggerGenPro(this IServiceCollection services, string title, Action<SwaggerGenOptions>? setupAction = null)
        {
            var controllerType = typeof(ControllerBase);
            var types = AssemblyLoader.All.SelectMany(x => x.ExportedTypes.Where(t => !t.IsAbstract && !t.IsInterface && t.IsClass && t.IsAssignableTo(controllerType))).ToList();
            Groups.Add(title, "v1");
            foreach (var type in types)
            {
                var groupAttr = type.GetCustomAttribute<SwaggerGroupAttribute>();
                if (groupAttr == null) continue;
                Groups.TryAdd(groupAttr.Name, groupAttr.Version);
            }
            services.AddSwaggerGen(c =>
            {
                setupAction?.Invoke(c);
                foreach (var group in Groups)
                {
                    c.SwaggerDoc(group.Key, new OpenApiInfo { Title = group.Key, Version = group.Value });
                }
                c.DocInclusionPredicate((doc, api) =>
                {
                    if (string.IsNullOrEmpty(api.GroupName))
                    {
                        api.GroupName = title;
                        return true;
                    }
                    return doc == api.GroupName;
                });
            });
        }

        public static void UseSwaggerPro(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var group in Groups)
                {
                    c.SwaggerEndpoint($"/swagger/{group.Key}/swagger.json", group.Key);
                }
            });
        }
    }
}