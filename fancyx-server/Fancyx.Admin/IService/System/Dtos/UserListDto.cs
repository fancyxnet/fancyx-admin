using MiniExcelLibs.Attributes;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class UserListDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [ExcelIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [ExcelColumn(Name = "用户名")]
        public string? UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [ExcelColumn(Name = "手机号")]
        public string? Phone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [ExcelIgnore]
        public string? Avatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [ExcelColumn(Name = "昵称")]
        public string? NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [ExcelColumn(Name = "性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [ExcelColumn(Name = "是否启用")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DeptName { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string? PostName { get; set; }
    }
}