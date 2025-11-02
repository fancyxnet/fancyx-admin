using Fancyx.Storage.Aliyun;
using Fancyx.Storage.Local;
using Fancyx.Storage.S3;

using Microsoft.Extensions.Configuration;

namespace Fancyx.Storage
{
    internal class ObjectStorageFactory : IObjectStorageFactory
    {
        private readonly IConfiguration _configuration;

        public ObjectStorageFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IObjectStorageService GetService(StorageType storageType, StorageOptions? options = null)
        {
            return storageType switch
            {
                StorageType.Local => new LocalObjectStorageService((LocalStorageOptions?)options ?? new LocalStorageOptions { Bucket = _configuration["Oss:Bucket"] }),
                StorageType.AliyunOss => new AliyunObjectStorageService((AliyunStorageOptions?)options ?? new AliyunStorageOptions
                {
                    AccessKey = _configuration["Oss:Aliyun:AccessKey"],
                    AccessKeySecret = _configuration["Oss:Aliyun:AccessKeySecret"],
                    Endpoint = _configuration["Oss:Aliyun:Endpoint"],
                    Bucket = _configuration["Oss:Aliyun:Bucket"],
                    Timeout = int.TryParse(_configuration["Oss:Aliyun:Timeout"], out var timeout) ? timeout : 10000
                }),
                StorageType.S3 => new S3ObjectStorageService(new S3StoreageOptions
                {
                    AccessKey = _configuration["Oss:S3:AccessKey"]!,
                    SecretKey = _configuration["Oss:S3:SecretKey"]!,
                    ServiceURL = _configuration["Oss:S3:ServiceURL"]!,
                    Region = _configuration["Oss:S3:Region"]!,
                }),
                _ => throw new NotSupportedException($"Storage type '{storageType}' is not supported.")
            };
        }
    }
}