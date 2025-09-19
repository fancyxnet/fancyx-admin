namespace Fancyx.Shared.Models
{
    public class AppOptionTree : AppOption
    {
        /// <summary>
        /// 子集
        /// </summary>
        public virtual List<AppOptionTree>? Children { get; set; }
    }
}