namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GenCodeResultDto
    {
        /// <summary>
        /// 实体
        /// </summary>
        public string? Entity { get; set; } = null!;

        /// <summary>
        /// 业务接口
        /// </summary>
        public string? IService { get; set; } = null!;

        /// <summary>
        /// 业务实现
        /// </summary>
        public string? Service { get; set; } = null!;

        /// <summary>
        /// 控制器
        /// </summary>
        public string? Controller {  get; set; } = null!;

        /// <summary>
        /// DTO
        /// </summary>
        public Dictionary<string, string>? Dtos { get; set; } = null!;
    }
}
