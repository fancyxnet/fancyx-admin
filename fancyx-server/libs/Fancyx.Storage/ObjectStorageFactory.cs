using Fancyx.Storage.Aliyun;
using Fancyx.Storage.Local;
using Microsoft.Extensions.Configuration;

namespace Fancyx.Storage
{
    internal class ObjectStorageFactory : IObjectStorageFactory
    {
        private readonly IConfiguration configuration;

        public ObjectStorageFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IObjectStorageService GetService(StorageType storageType, StorageOptions? options = null)
        {
            return storageType switch
            {
                StorageType.Local => new LocalObjectStorageService((LocalStorageOptions?)options ?? new LocalStorageOptions { Bucket = configuration["Oss:Bucket"] }),
                StorageType.AliyunOss => new AliyunObjectStorageService((AliyunStorageOptions?)options ?? new AliyunStorageOptions
                {
                    AccessKey = configuration["Oss:Aliyun:AccessKey"],
                    AccessKeySecret = configuration["Oss:Aliyun:AccessKeySecret"],
                    Endpoint = configuration["Oss:Aliyun:Endpoint"],
                    Bucket = configuration["Oss:Aliyun:Bucket"],
                    Timeout = int.TryParse(configuration["Oss:Aliyun:Timeout"], out var timeout) ? timeout : 10000
                }),
                _ => throw new NotSupportedException($"Storage type '{storageType}' is not supported.")
            };
        }
    }
}