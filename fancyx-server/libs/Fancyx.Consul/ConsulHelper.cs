using Consul;

using Microsoft.Extensions.Caching.Memory;

namespace Fancyx.Consul
{
    public class ConsulHelper
    {
        private readonly Random random = new();
        private Timer? _timer = null;

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
            if (nodes.Count == 0)
            {
                throw new HttpRequestException($"{serviceName}服务节点宕机");
            }
            // 每30s检查一下节点
            _timer ??= new Timer(CheckNodes, serviceName, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            var index = random.Next(0, nodes.Count);
            return nodes[index];
        }

        private async void CheckNodes(object? state)
        {
            string serviceName = (string)state!;
            await GetAllNodes(serviceName, true);
        }

        private async Task<List<ConsulNode>> GetAllNodes(string serviceName, bool reload = false)
        {
            var key = $"ConsulNode:{serviceName}";
            if (!reload && MemoryCache.TryGetValue(key, out List<ConsulNode>? cacheData) && cacheData != null)
            {
                return cacheData;
            }
            var services = await ConsulClient.Health.Service(serviceName, null, true);
            if (services.Response.Length > 0)
            {
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
                MemoryCache.Set(key, nodes, TimeSpan.FromMinutes(1));
                return nodes;
            }
            MemoryCache.Set(key, new List<ConsulNode>(), TimeSpan.FromMinutes(1));
            return [];
        }
    }

    public class ConsulNode
    {
        public string? Address { get; set; }

        public int HttpPort { get; set; }

        public int GrpcPort { get; set; }
    }
}