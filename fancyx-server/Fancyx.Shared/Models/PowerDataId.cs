using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Shared.Models
{
    public class PowerDataId
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [NotNull]
        public List<Guid>? DeptIds { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        [NotNull]
        public List<Guid>? UserIds { get; set; }
    }
}