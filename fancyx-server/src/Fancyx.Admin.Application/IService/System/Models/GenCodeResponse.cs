namespace Fancyx.Admin.Application.IService.System.Models
{
    public class GenCodeResponse
    {
        public AppOption Entity { get; set; } = null!;
        public AppOption IService { get; set; } = null!;
        public AppOption Service { get; set; } = null!;
        public AppOption Controller { get; set; } = null!;
        public AppOption QueryDto { get; set; } = null!;
        public AppOption Api { get; set; } = null!;
        public AppOption Page { get; set; } = null!;
    }
}
