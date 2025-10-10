using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace Fancyx.Storage.Aliyun
{
    internal class AliyunObjectStorageService : IObjectStorageService
    {
        public AliyunObjectStorageService(AliyunStorageOptions options)
        {
            Options = options;
            var aliossConfiguration = new ClientConfiguration
            {
                ConnectionTimeout = options.Timeout
            };
            Client = new OssClient(options.Endpoint, options.AccessKey, options.AccessKeySecret, aliossConfiguration);
        }

        public AliyunStorageOptions Options { get; }

        public OssClient Client { get; }

        public Task DeleteAsync(string filePathOrKey)
        {
            Client.DeleteObject(Options.Bucket, filePathOrKey);
            return Task.CompletedTask;
        }

        public Task<Stream> DownloadAsync(string filePathOrKey)
        {
            OssObject ossObject = Client.GetObject(Options.Bucket, filePathOrKey);
            if (ossObject != null && ossObject.Content != null)
            {
                return Task.FromResult(ossObject.Content);
            }
            throw new FileNotFoundException("文件不存在");
        }

        public Task<bool> ExistsAsync(string filePathOrKey)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadAsync(Stream stream, string fileName)
        {
            stream.Seek(0, SeekOrigin.Begin);

            var result = Client.PutObject(Options.Bucket, fileName, stream);
            if (result.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"上传文件失败，状态码：{result.HttpStatusCode}");
            }
            return Task.FromResult(fileName);
        }
    }
}