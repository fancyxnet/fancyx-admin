using System.Diagnostics.CodeAnalysis;

using Consul;

using Microsoft.Extensions.Caching.Memory;

namespace Fancyx.Consul
{
    public class ConsulHelper
    {
        private readonly Random _random = new();
        public const int CacheSeconds = 60;

        public ConsulHelper(IConsulClient consulClient, IMemoryCache memoryCache)
        {
            ConsulClient = consulClient;
            MemoryCache = memoryCache;
        }

        public IConsulClient ConsulClient { get; }
        public IMemoryCache MemoryCache { get; }

        public async Task<ConsulNode> GetNodeAsync(string serviceName)
        {
            var nodes = await GetAllNodes(serviceName);
            var index = _random.Next(0, nodes.Count);
            return nodes[index];
        }

        public async Task<List<ConsulNode>> GetAllNodes(string serviceName)
        {
            var key = $"ConsulNode:{serviceName}";
            if (MemoryCache.TryGetValue(key, out List<ConsulNode>? cacheData) && cacheData != null)
            {
                return cacheData;
            }
            var services = await ConsulClient.Health.Service(serviceName, null, true);
            if (services.Response.Length <= 0)
            {
                throw new HttpRequestException($"{serviceName}服务节点宕机");
            }
            var nodes = new List<ConsulNode>();
            foreach (var item in services.Response)
            {
                var node = new ConsulNode
                {
                    Address = item.Service.Address,
                    HttpPort = int.Parse(item.Service.Meta["HttpPort"]),
                    GrpcPort = int.Parse(item.Service.Meta["GrpcPort"])
                };
                nodes.Add(node);
            }
            MemoryCache.Set(key, nodes, TimeSpan.FromSeconds(CacheSeconds));
            return nodes;
        }
    }

    public class ConsulNode
    {
        [NotNull]
        public string? Address { get; set; } = null!;

        public int HttpPort { get; set; }

        public int GrpcPort { get; set; }
    }
}