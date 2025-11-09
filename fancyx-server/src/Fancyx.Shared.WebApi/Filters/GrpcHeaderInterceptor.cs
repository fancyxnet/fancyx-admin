using Fancyx.Core.Authorization;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Fancyx.Shared.WebApi.Filters
{
    public class GrpcHeaderInterceptor : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var headers = context.Options.Headers ?? new Metadata();
            if (!string.IsNullOrEmpty(TenantManager.Current))
            {
                headers.Add("X-Tenant", TenantManager.Current);
            }

            var newOptions = context.Options.WithHeaders(headers);
            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method, context.Host, newOptions);

            return base.AsyncUnaryCall(request, newContext, continuation);
        }
    }
}
