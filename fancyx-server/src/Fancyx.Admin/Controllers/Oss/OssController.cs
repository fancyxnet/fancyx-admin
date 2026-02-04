using Cracker.Storage;
using Cracker.Utils;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Admin.Controllers.Oss
{
    [Route("api/[controller]/[action]")]
    public class OssController : ControllerBase
    {
        private readonly IObjectStorageFactory _objectStorageFactory;
        private readonly IConfiguration _configuration;

        public OssController(IObjectStorageFactory objectStorageFactory, IConfiguration configuration)
        {
            _objectStorageFactory = objectStorageFactory;
            _configuration = configuration;
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
            var type = StorageType.Local;
            IObjectStorageService objectStorageService = _objectStorageFactory.GetService(type);
            var url = await objectStorageService.UploadAsync(stream, fileName);

            if (type == StorageType.Local)
            {
                url = $"File/{url}";
            }

            return Result.Data(new Uri(new Uri(_configuration["Oss:Domain"]!), url).ToString());
        }

        [HttpGet]
        [Route("/File/{*fileName}")]
        public async Task<IActionResult> ImageAsync([FromRoute] string fileName)
        {
            try
            {
                IObjectStorageService objectStorageService = _objectStorageFactory.GetService(StorageType.Local);
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