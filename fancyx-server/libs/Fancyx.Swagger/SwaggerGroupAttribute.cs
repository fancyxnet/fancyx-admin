using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Swagger
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SwaggerGroupAttribute : ApiExplorerSettingsAttribute
    {
        public string Name { get; init; }
        public string Version { get; init; }

        public SwaggerGroupAttribute(string name, string version = "v1")
        {
            Name = name;
            GroupName = name;
            Version = version;
        }
    }
}