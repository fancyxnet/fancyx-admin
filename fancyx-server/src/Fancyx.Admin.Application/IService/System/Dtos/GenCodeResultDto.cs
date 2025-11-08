namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GenCodeResultDto
    {
        public AppOption Entity { get; set; } = null!;
        public AppOption IService { get; set; } = null!;
        public AppOption Service { get; set; } = null!;
        public AppOption Controller { get; set; } = null!;
    }
}
