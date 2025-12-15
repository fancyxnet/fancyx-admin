using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Fancyx.Gateway.Yarp
{
    public class DynamicProxyConfig : IProxyConfig
    {
        public List<RouteConfig> Routes { get; internal set; } = new List<RouteConfig>();

        public List<ClusterConfig> Clusters { get; internal set; } = new List<ClusterConfig>();

        IReadOnlyList<RouteConfig> IProxyConfig.Routes => Routes;

        IReadOnlyList<ClusterConfig> IProxyConfig.Clusters => Clusters;

        // This field is required.
        public IChangeToken ChangeToken { get; internal set; } = default!;

        public void UpdateClusterConfig(ClusterConfig config)
        {
            var clusterIndex = Clusters.FindIndex(x => x.ClusterId == config.ClusterId);
            if (clusterIndex == -1)
            {
                Clusters.Add(config);
            }
            else
            {
                Clusters[clusterIndex] = config;
            }
        }
    }
}
