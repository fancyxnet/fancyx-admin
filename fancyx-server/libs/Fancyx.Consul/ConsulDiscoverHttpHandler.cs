using Consul;

using Microsoft.Extensions.Logging;

namespace Fancyx.Consul
{
    public class ConsulDiscoverHttpHandler: DelegatingHandler
    {
        private readonly ConsulHelper _consulHelper;
        private readonly ILogger<ConsulDiscoverHttpHandler> _logger;

        public ConsulDiscoverHttpHandler(ConsulHelper consulHelper, ILogger<ConsulDiscoverHttpHandler> logger)
        {
            _consulHelper = consulHelper;
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentUri = request.RequestUri ?? throw new InvalidDataException("RequestUri is null");
            var healthNode = await _consulHelper.GetNodeAsync(currentUri.Host);
            var baseUri = $"http://{healthNode.Address}:{healthNode.HttpPort}";
            if (string.IsNullOrWhiteSpace(baseUri))
            {
                throw new ArgumentNullException($"{currentUri.Host} does not contain helath service address!");
            }
            else
            {
                var realRequestUri = new Uri($"{currentUri.Scheme}://{baseUri}{currentUri.PathAndQuery}");
                request.RequestUri = realRequestUri;
                _logger.LogDebug("RequestUri:{realRequestUri}", realRequestUri);
            }

            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogDebug(ex, "Exception during SendAsync()");
                throw;
            }
            finally
            {
                request.RequestUri = currentUri;
            }
        }
    }
}
