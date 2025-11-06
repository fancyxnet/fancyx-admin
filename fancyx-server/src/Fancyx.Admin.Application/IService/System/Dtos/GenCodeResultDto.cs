namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GenCodeResultDto
    {
        public AppOption Entity { get; set; } = null!;
        public AppOption IService { get; set; } = null!;
        public AppOption Service { get; set; } = null!;
        public AppOption Controller { get; set; } = null!;
        public AppOption BusinessAddDto { get; set; } = null!;
        public AppOption BusinessUpdateDto { get; set; } = null!;
        public AppOption BusinessListDto { get; set; } = null!;
        public AppOption BusinessDto { get; set; } = null!;
        public AppOption BusinessQueryDto { get; set; } = null!;
    }
}
