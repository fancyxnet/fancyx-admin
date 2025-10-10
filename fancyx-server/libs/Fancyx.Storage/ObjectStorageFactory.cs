using Fancyx.Storage.Aliyun;
using Fancyx.Storage.Local;
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
                _ => throw new NotSupportedException($"Storage type '{storageType}' is not supported.")
            };
        }
    }
}