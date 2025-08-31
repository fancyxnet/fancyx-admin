namespace Fancyx.Admin.IService.System.Dtos
{
    public class DeptTreeOptionDto
    {
        public Guid Key { get; set; }

        public string? Title { get; set; }
        
        public List<DeptTreeOptionDto>? Children { get; set; }
    }
}
