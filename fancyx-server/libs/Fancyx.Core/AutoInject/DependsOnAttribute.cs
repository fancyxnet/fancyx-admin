namespace Fancyx.Core.AutoInject
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DependsOnAttribute : Attribute
    {
        public Type[] DependedModuleTypes { get; }

        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }
    }
}