namespace Fancyx.Core.AutoInject
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class AopAttributeBase : Attribute
    {
        private readonly bool _throwException;

        protected AopAttributeBase(bool throwException)
        {
            _throwException = throwException;
        }

        public bool ThrowException => _throwException;

        public abstract void OnBefore();

        public abstract void OnAfter();

        public virtual void OnException()
        { }
    }
}