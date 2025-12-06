namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class DeptTreeOption
    {
        public long Key { get; set; }

        public string? Title { get; set; }
        
        public List<DeptTreeOption>? Children { get; set; }
    }
}
