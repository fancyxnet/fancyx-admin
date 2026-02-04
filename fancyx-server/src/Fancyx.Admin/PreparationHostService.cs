using Cracker.Storage;
using Cracker.Utils;

namespace Fancyx.Admin
{
    public class PreparationHostService : BackgroundService
    {
        private readonly IObjectStorageFactory _objectStorageFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PreparationHostService(IObjectStorageFactory objectStorageFactory, IWebHostEnvironment webHostEnvironment)
        {
            _objectStorageFactory = objectStorageFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //默认头像
            await SaveDefaultAvatarAsync(stoppingToken);
            //保存RSA公私钥
            //await SaveRSAKeysAsync();
        }

        private static async Task SaveRSAKeysAsync()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RSAKeys");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var publicKeysPath = Path.Combine(dir, "PublicKeys.txt");
            var privateKeysPath = Path.Combine(dir, "PrivateKeys.txt");
            if (!File.Exists(publicKeysPath) || !File.Exists(privateKeysPath))
            {
                var (publicKey, privateKey) = EncryptionUtils.GenerateRSAKeys();
                await File.WriteAllTextAsync(publicKeysPath, publicKey);
                await File.WriteAllTextAsync(privateKeysPath, privateKey);
            }
        }

        private async Task SaveDefaultAvatarAsync(CancellationToken stoppingToken)
        {
            var dir = Path.Combine(_webHostEnvironment.WebRootPath, "avatar");
            if (!Directory.Exists(dir)) return;
            var files = Directory.GetFiles(dir);
            if (files.Length > 0)
            {
                //默认头像写入oss
                foreach (var item in files)
                {
                    var fileName = Path.GetFileName(item);
                    using var fs = new FileStream(item, FileMode.Open, FileAccess.Read);
                    IObjectStorageService objectStorageService = _objectStorageFactory.GetService(StorageType.Local);
                    await objectStorageService.UploadAsync(fs, "avatar/" + fileName);
                }
            }
        }
    }
}