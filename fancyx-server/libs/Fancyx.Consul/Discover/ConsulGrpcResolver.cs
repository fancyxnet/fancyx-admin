using Grpc.Net.Client.Balancer;

using Microsoft.Extensions.Logging;

namespace Fancyx.Consul.Discover
{
    public class ConsulGrpcResolver : PollingResolver
    {
        private readonly Uri _uri;
        private readonly ConsulHelper _consulHelper;
        private readonly ILogger _logger;
        private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(30);
        private Timer? _timer;

        public ConsulGrpcResolver(Uri uri, ConsulHelper consulHelper, ILoggerFactory loggerFactory):base(loggerFactory)
        {
            _uri = uri;
            _consulHelper = consulHelper;
            _logger = loggerFactory.CreateLogger<ConsulGrpcResolver>();
        }

        protected override async Task ResolveAsync(CancellationToken cancellationToken)
        {
            var nodes = await _consulHelper.GetAllNodes(_uri.Host);
            var balancerAddresses = new List<BalancerAddress>();
            nodes.ForEach(node =>
            {
                balancerAddresses.Add(new BalancerAddress(node.Address, node.GrpcPort));
            });
            Listener(ResolverResult.ForResult(balancerAddresses));
        }

        protected override void OnStarted()
        {
            base.OnStarted();

            if (_refreshInterval != Timeout.InfiniteTimeSpan)
            {
                _timer = new Timer(OnTimerCallback, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                _timer.Change(_refreshInterval, _refreshInterval);
            }
        }

        private void OnTimerCallback(object? state)
        {
            try
            {
                Refresh();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ConsulGrpcResolver.OnTimerCallback");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _timer?.Dispose();
        }
    }
}
