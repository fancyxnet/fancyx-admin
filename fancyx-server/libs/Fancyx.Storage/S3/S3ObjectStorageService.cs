using System.Threading;

using Amazon.S3;
using Amazon.S3.Model;

namespace Fancyx.Storage.S3
{
    public class S3ObjectStorageService : IObjectStorageService
    {
        AmazonS3Client? _client;
        private readonly Lock _lockObj = new();
        public S3ObjectStorageService(S3StoreageOptions options)
        {
            Options = options;
        }

        public AmazonS3Client S3Client
        {
            get
            {
                if (_client == null)
                {
                    using (_lockObj.EnterScope())
                    {
                        _client = new AmazonS3Client(Options.AccessKey, Options.SecretKey, new AmazonS3Config
                        {
                            ServiceURL = Options.ServiceURL,
                            ForcePathStyle = true
                        });
                    }
                    return _client;
                }
                return _client;
            }
        }

        public S3StoreageOptions Options { get; }

        public Task DeleteAsync(string filePathOrKey)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(string filePathOrKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string filePathOrKey)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(Stream stream, string fileName)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = Options.Region,
                Key = fileName,
                InputStream = stream
            };

            var response = await S3Client.PutObjectAsync(putRequest);
            return $"{putRequest.BucketName}/{fileName}";
        }
    }
}
