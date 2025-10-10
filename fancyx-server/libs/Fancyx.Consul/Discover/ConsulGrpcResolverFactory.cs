using Grpc.Net.Client.Balancer;

namespace Fancyx.Consul.Discover
{
    public class ConsulGrpcResolverFactory(ConsulHelper consulHelper) : ResolverFactory
    {
        public override string Name => "consul";

        public override Resolver Create(ResolverOptions options) => new ConsulGrpcResolver(options.Address, consulHelper, options.LoggerFactory);
    }
}
