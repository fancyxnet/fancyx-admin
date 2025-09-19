using Fancyx.Admin.SharedService;
using Fancyx.Shared.Keys;
using Fancyx.Storage;
using Fancyx.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.Oss
{
    [Route("api/[controller]/[action]")]
    public class OssController : ControllerBase
    {
        private readonly IObjectStorageFactory _objectStorageFactory;
        private readonly ConfigSharedService _configSharedService;

        public OssController(IObjectStorageFactory objectStorageFactory, ConfigSharedService configSharedService)
        {
            _objectStorageFactory = objectStorageFactory;
            _configSharedService = configSharedService;
        }

        [HttpPost]
        [Authorize]
        public async Task<AppResponse<string>> UploadAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var fileName = file.FileName;
            if (HttpContext.Request.Headers.TryGetValue("dir", out var dir) && !string.IsNullOrWhiteSpace(dir))
            {
                fileName = dir + "/" + fileName;
            }
            var storageType = await _configSharedService.GetAsync(SystemConfigKey.StorageType);
            _ = Enum.TryParse(storageType, out StorageType type);
            IObjectStorageService objectStorageService = _objectStorageFactory.GetService(type);
            var url = await objectStorageService.UploadAsync(stream, fileName);

            if (type == StorageType.Local)
            {
                url = $"file/{url}";
            }

            return Result.Data(url);
        }

        [HttpGet]
        [Route("/file/{*fileName}")]
        public async Task<IActionResult> ImageAsync([FromRoute] string fileName)
        {
            try
            {
                var storageType = await _configSharedService.GetAsync(SystemConfigKey.StorageType);
                _ = Enum.TryParse(storageType, out StorageType type);
                IObjectStorageService objectStorageService = _objectStorageFactory.GetService(type);
                var stream = await objectStorageService.DownloadAsync(fileName);
                var name = Path.GetFileName(fileName);
                var mimeType = MimeTypesHelper.Instance.GetMimeType(name);
                return File(stream, mimeType, name);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<AppResponse<bool>> DeleteAsync([FromQuery] string key)
        {
            IObjectStorageService objectStorageService = _objectStorageFactory.GetService(StorageType.Local);
            await objectStorageService.DeleteAsync(key);
            return Result.Ok();
        }
    }
}