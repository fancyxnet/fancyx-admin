namespace Fancyx.Storage.S3
{
    public class S3StoreageOptions
    {
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string ServiceURL { get; set; } = null!;
    }
}
