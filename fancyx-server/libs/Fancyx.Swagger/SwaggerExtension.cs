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
            var sameNameGroups = new List<Tuple<string, string>>();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<SwaggerGroupAttribute>();
                if (attr == null) continue;

                if (Groups.TryGetValue(attr.Name, out var version) && !version.Equals(attr.Version, StringComparison.OrdinalIgnoreCase))
                {
                    Groups.Remove(attr.Name);
                    sameNameGroups.Add(new Tuple<string, string>(attr.Name, version));
                    sameNameGroups.Add(new Tuple<string, string>(attr.Name, attr.Version));
                    continue;
                }
                Groups.TryAdd(attr.Name, attr.Version);
            }
            foreach (var item in sameNameGroups.OrderBy(s => s.Item2))
            {
                Groups.Add($"{item.Item1}_{item.Item2}", item.Item2);
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
                    if (doc.Contains('_'))
                    {
                        var group = api.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is SwaggerGroupAttribute);
                        if (group is SwaggerGroupAttribute attr)
                        {
                            return doc == $"{attr.Name}_{attr.Version}";
                        }
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