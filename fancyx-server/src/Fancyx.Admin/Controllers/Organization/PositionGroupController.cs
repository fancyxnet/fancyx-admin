using Fancyx.Shared.WebApi.Attributes;
using Fancyx.Shared.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Fancyx.Admin.Application.IService.Organization;
using Fancyx.Admin.Application.IService.Organization.Models;

namespace Fancyx.Admin.Controllers.Organization
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PositionGroupController : ControllerBase
    {
        private readonly IPositionGroupService _positionGroupService;

        public PositionGroupController(IPositionGroupService positionGroupService)
        {
            _positionGroupService = positionGroupService;
        }

        /// <summary>
        /// 新增职位分组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [HasPermission("Org.PositionGroup.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddPositionGroupAsync([FromBody] AddOrUpdatePositionGroupRequest req)
        {
            await _positionGroupService.AddPositionGroupAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 职位分组分页列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Org.PositionGroup.List")]
        public async Task<AppResponse<List<PositionGroupItem>>> GetPositionGroupListAsync([FromQuery] GetPositionGroupListRequest req)
        {
            var data = await _positionGroupService.GetPositionGroupListAsync(req);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改职位分组
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Org.PositionGroup.Update")]
        public async Task<AppResponse<bool>> UpdatePositionGroupAsync([FromBody] AddOrUpdatePositionGroupRequest req)
        {
            await _positionGroupService.UpdatePositionGroupAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 删除职位分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        [HasPermission("Org.PositionGroup.Delete")]
        [ApiAccessLog(operateName: "删除职位分组", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeletePositionGroupAsync(long id)
        {
            await _positionGroupService.DeletePositionGroupAsync(id);
            return Result.Ok();
        }
    }
}