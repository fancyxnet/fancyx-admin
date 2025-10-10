namespace Fancyx.Core.AutoInject
{
    /// <summary>
    /// 异步AOP
    /// 如果一个类同时被接口、本身服务注册，此特性将不生效
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class AsyncAopAttributeBase : Attribute
    {
        private readonly bool _throwException;

        protected AsyncAopAttributeBase(bool throwException)
        {
            _throwException = throwException;
        }

        public bool ThrowException => _throwException;

        public abstract Task OnBeforeAsync();

        public abstract Task OnAfterAsync();

        public virtual Task OnExceptionAsync()
        {
            return Task.CompletedTask;
        }

        protected IServiceProvider ServiceProvider { get; private set; } = null!;

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            if (ServiceProvider != null)
            {
                throw new InvalidOperationException("ServiceProvider has already been set.");
            }
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}