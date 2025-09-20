namespace Fancyx.Shared.Models
{
    public class MicroServiceOption
    {
        /// <summary>
        /// Direct or Consul
        /// </summary>
        public string? Mode { get; set; } = null!;

        public List<MicroServiceAddress>? Address { get; set; }
    }

    public class MicroServiceAddress
    {
        public string? Name { get; set; }
        public string? Http { get; set; }
        public string? Grpc { get; set; }
    }
}
