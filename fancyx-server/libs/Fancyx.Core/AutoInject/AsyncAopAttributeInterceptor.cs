using Castle.DynamicProxy;

namespace Fancyx.Core.AutoInject
{
    public class AsyncAopAttributeInterceptor : AsyncInterceptorBase
    {
        private readonly IServiceProvider _serviceProvider;

        public AsyncAopAttributeInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            if (invocation.MethodInvocationTarget?.GetCustomAttributes(typeof(AsyncAopAttributeBase), false)
                .FirstOrDefault() is AsyncAopAttributeBase asyncAopAttribute)
            {
                asyncAopAttribute.SetServiceProvider(_serviceProvider);
                await asyncAopAttribute.OnBeforeAsync();
                try
                {
                    await proceed(invocation, proceedInfo);
                }
                catch (Exception)
                {
                    await asyncAopAttribute.OnExceptionAsync();
                    if (asyncAopAttribute.ThrowException)
                    {
                        throw;
                    }
                }

                await asyncAopAttribute.OnAfterAsync();
            }
            else
            {
                await proceed(invocation, proceedInfo);
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            if (invocation.MethodInvocationTarget?.GetCustomAttributes(typeof(AsyncAopAttributeBase), false)
                .FirstOrDefault() is AsyncAopAttributeBase asyncAopAttribute)
            {
                asyncAopAttribute.SetServiceProvider(_serviceProvider);
                await asyncAopAttribute.OnBeforeAsync();
                TResult? result = default;
                try
                {
                    result = await proceed(invocation, proceedInfo);
                }
                catch (Exception)
                {
                    await asyncAopAttribute.OnExceptionAsync();
                    if (asyncAopAttribute.ThrowException)
                    {
                        throw;
                    }
                }

                await asyncAopAttribute.OnAfterAsync();
                return result!;
            }
            else
            {
                return await proceed(invocation, proceedInfo);
            }
        }
    }
}