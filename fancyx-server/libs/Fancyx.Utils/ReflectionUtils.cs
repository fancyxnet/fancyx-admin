using System.Reflection;
using System.Runtime.Loader;

using Microsoft.Extensions.DependencyModel;

namespace Fancyx.Utils
{
    public class ReflectionUtils
    {
        /// <summary>
        /// 获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> AllAssemblies
        {
            get;
            private set;
        }

        static ReflectionUtils()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            if (deps == null)
            {
                AllAssemblies = [];
                return;
            }
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");//排除所有的系统程序集、Nuget下载包
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            AllAssemblies = list;
        }
    }
}
