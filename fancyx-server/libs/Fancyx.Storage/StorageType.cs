namespace Fancyx.Storage
{
    public enum StorageType
    {
        /// <summary>
        /// 本地文件系统
        /// </summary>
        Local = 1,

        /// <summary>
        /// 阿里云OSS
        /// </summary>
        AliyunOss = 2,
        
        /// <summary>
        /// S3协议
        /// </summary>
        S3 = 3
    }
}