using Fancyx.Admin.EfCore.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.Account.Dtos
{
    public class UpdateUserInfoRequest
    {
        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(256)]
        public string? Avatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(64)]
        public string? NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexType Sex { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Phone]
        public string? Phone { get; set; }
    }
}