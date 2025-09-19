using Fancyx.Core.Interfaces;

namespace Fancyx.Core.AutoInject
{
    /// <summary>
    /// 自动注入特性，标记此特性的属性将自动注入（无需构造函数注入，减少代码） <br/>
    /// 1.对实现注册接口(<see cref="IScopedDependency"/>, <see cref="ISingletonDependency"/>,<see cref="ITransientDependency"/>)类生效 <br/>
    /// 2.对标记<see cref="DenpendencyInjectAttribute"/>类生效 <br/>
    /// 3.对于性能敏感的操作，建议使用构造函数注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AutowiredAttribute : Attribute
    {
    }
}