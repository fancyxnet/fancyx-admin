namespace Fancyx.Storage.Aliyun
{
    public class AliyunStorageOptions : StorageOptions
    {
        public string? Endpoint { get; set; }
        public string? AccessKey { get; set; }
        public string? AccessKeySecret { get; set; }
        public string? Bucket { get; set; }
        public int Timeout { get; set; }
    }
}