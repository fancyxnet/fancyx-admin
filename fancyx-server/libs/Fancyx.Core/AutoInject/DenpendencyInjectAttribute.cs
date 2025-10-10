namespace Fancyx.Core.AutoInject
{
    /// <summary>
    /// 自动注入，标记此特性的类会被自动注册到依赖注入容器中 <br/>
    /// 如果AsSelf为false且Interfaces为空，则不会注册
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DenpendencyInjectAttribute : Attribute
    {
        /// <summary>
        /// 注册方式
        /// </summary>
        public DenpendencyType Way { get; init; } = DenpendencyType.Scoped;

        /// <summary>
        /// 是否将当前类作为自身类型注入
        /// </summary>
        public bool AsSelf { get; init; }

        /// <summary>
        /// 实现的接口类型，如果不为空且AsSelf=false，则会将当前类注册为这些接口的实现；
        /// </summary>
        public Type[]? Interfaces { get; init; }
    }
}