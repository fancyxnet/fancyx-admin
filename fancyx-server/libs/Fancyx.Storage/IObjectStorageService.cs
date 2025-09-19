namespace Fancyx.Storage
{
    public interface IObjectStorageService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名，可以带路径，如：/avatar/boy.jpg</param>
        /// <returns>文件访问路径</returns>
        Task<string> UploadAsync(Stream stream, string fileName);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePathOrKey">文件路径或存储键</param>
        /// <returns>文件流</returns>
        Task<Stream> DownloadAsync(string filePathOrKey);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePathOrKey">文件路径或存储键</param>
        Task DeleteAsync(string filePathOrKey);

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="filePathOrKey">文件路径或存储键</param>
        Task<bool> ExistsAsync(string filePathOrKey);
    }
}