using Castle.DynamicProxy;

namespace Fancyx.Core.AutoInject
{
    public class AopAttributeInterceptor : AsyncInterceptorBase, IAsyncInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public AopAttributeInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            var attrs = invocation.MethodInvocationTarget?.GetCustomAttributes(typeof(AopAttributeBase), false);
            if (attrs != null)
            {
                var realAttrs = new List<AopAttributeBase>();
                foreach (var item in attrs)
                {
                    if (item is AopAttributeBase asyncAopAttribute)
                    {
                        realAttrs.Add(asyncAopAttribute);
                    }
                }

                var beforeTasks = realAttrs.Select(x =>
                {
                    x.SetServiceProvider(_serviceProvider);
                    return x.OnBeforeAsync();
                });
                await Task.WhenAll(beforeTasks);

                try
                {
                    await proceed(invocation, proceedInfo);
                }
                catch (Exception)
                {
                    var isThrow = false;
                    var exceptionTasks = realAttrs.Select(x =>
                    {
                        if (!isThrow && x.ThrowException)
                        {
                            isThrow = true;
                        }
                        return x.OnExceptionAsync();
                    });
                    await Task.WhenAll(exceptionTasks);
                    if (isThrow) throw;
                }
                await Task.WhenAll(realAttrs.Select(x => x.OnAfterAsync()));
            }
            else
            {
                await proceed(invocation, proceedInfo);
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var attrs = invocation.MethodInvocationTarget?.GetCustomAttributes(typeof(AopAttributeBase), false);
            if (attrs != null)
            {
                var realAttrs = new List<AopAttributeBase>();
                foreach (var item in attrs)
                {
                    if (item is AopAttributeBase asyncAopAttribute)
                    {
                        realAttrs.Add(asyncAopAttribute);
                    }
                }

                var beforeTasks = realAttrs.Select(x =>
                {
                    x.SetServiceProvider(_serviceProvider);
                    return x.OnBeforeAsync();
                });
                await Task.WhenAll(beforeTasks);

                TResult? result = default;
                try
                {
                    result = await proceed(invocation, proceedInfo);
                }
                catch (Exception)
                {
                    var isThrow = false;
                    var exceptionTasks = realAttrs.Select(x =>
                    {
                        if (!isThrow && x.ThrowException)
                        {
                            isThrow = true;
                        }
                        return x.OnExceptionAsync();
                    });
                    await Task.WhenAll(exceptionTasks);
                    if (isThrow) throw;
                }

                await Task.WhenAll(realAttrs.Select(x => x.OnAfterAsync()));
                return result!;
            }
            else
            {
                return await proceed(invocation, proceedInfo);
            }
        }
    }
}