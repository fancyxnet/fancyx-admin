namespace Fancyx.Storage.Local
{
    /// <summary>
    /// 本地服务器文件存储
    /// </summary>
    internal class LocalObjectStorageService : IObjectStorageService
    {
        public LocalObjectStorageService(LocalStorageOptions options)
        {
            Options = options;
        }

        public LocalStorageOptions Options { get; }

        public async Task<string> UploadAsync(Stream fileStream, string fileName)
        {
            var rootPath = GetOssRootPath();
            var dir = Path.GetDirectoryName(fileName) ?? "/";
            var name = Path.GetFileName(fileName);
            var path = Path.Combine(rootPath, dir);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var fullPath = Path.Combine(path, name);
            if (!File.Exists(fullPath) || Options.IsCovered)
            {
                using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fs);
            }
            if (!string.IsNullOrEmpty(dir)) return dir + "/" + name;
            return name;
        }

        public Task<Stream> DownloadAsync(string filePathOrKey)
        {
            var rootPath = GetOssRootPath();
            var path = Path.Combine(rootPath, filePathOrKey);
            if (!File.Exists(path)) throw new FileNotFoundException();

            return Task.FromResult((Stream)new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        public Task DeleteAsync(string filePathOrKey)
        {
            var rootPath = GetOssRootPath();
            var path = Path.Combine(rootPath, filePathOrKey);
            if (File.Exists(path)) File.Delete(path);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string filePathOrKey)
        {
            var rootPath = GetOssRootPath();
            var path = Path.Combine(rootPath, filePathOrKey);
            return Task.FromResult(File.Exists(path));
        }

        private string GetOssRootPath()
        {
            var path = Options.Bucket;
            if (Directory.Exists(path)) return path;
            try
            {
                Directory.CreateDirectory(path!);
                return path!;
            }
            catch (Exception)
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "oss");
            }
        }
    }
}