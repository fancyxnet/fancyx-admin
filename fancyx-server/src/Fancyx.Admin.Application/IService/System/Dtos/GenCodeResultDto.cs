namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GenCodeResultDto
    {
        public AppOption Entity { get; set; } = null!;
        public AppOption IService { get; set; } = null!;
        public AppOption Service { get; set; } = null!;
        public AppOption Controller { get; set; } = null!;
        public AppOption QueryDto { get; set; } = null!;
        public AppOption Api { get; set; } = null!;
    }
}
