namespace Fancyx.Orleans
{
    public class OrleansConfigOption
    {
        public string ClusterId { get; set; } = null!;
        public string ServiceId { get; set; } = null!;
        public string RedisEndPoints { get; set; } = null!;
        public string StorageName { get; set; } = null!;
    }
}