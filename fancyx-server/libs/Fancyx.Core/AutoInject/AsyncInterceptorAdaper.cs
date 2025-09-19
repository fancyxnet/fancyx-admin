using Castle.DynamicProxy;

namespace Fancyx.Core.AutoInject
{
    public class AsyncInterceptorAdaper : AsyncDeterminationInterceptor
    {
        private readonly IAsyncInterceptor asyncInterceptor;

        public AsyncInterceptorAdaper(IAsyncInterceptor asyncInterceptor) : base(asyncInterceptor)
        {
            this.asyncInterceptor = asyncInterceptor;
        }

        public override void Intercept(IInvocation invocation)
        {
            asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}