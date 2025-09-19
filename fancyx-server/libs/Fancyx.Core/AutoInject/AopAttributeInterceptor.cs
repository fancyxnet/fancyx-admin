using Castle.DynamicProxy;

namespace Fancyx.Core.AutoInject
{
    public class AopAttributeInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget?.GetCustomAttributes(typeof(AopAttributeBase), false)
                .FirstOrDefault() is AopAttributeBase aopAttribute)
            {
                aopAttribute.OnBefore();
                try
                {
                    invocation.Proceed();
                }
                catch (Exception)
                {
                    aopAttribute.OnException();
                    if (aopAttribute.ThrowException)
                    {
                        throw;
                    }
                }

                aopAttribute.OnAfter();
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}