using Fancyx.Admin.IService.Organization;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.Core.Attributes;
using Fancyx.Logger;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Fancyx.Admin.Controllers.Organization
{
    [Authorize]
    [ApiController]
    [Route("api/positionGroup")]
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
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [HasPermission("Org.PositionGroup.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddPositionGroupAsync([FromBody] PositionGroupDto dto)
        {
            await _positionGroupService.AddPositionGroupAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 职位分组分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [HasPermission("Org.PositionGroup.List")]
        public async Task<AppResponse<List<PositionGroupListDto>>> GetPositionGroupListAsync([FromQuery] PositionGroupQueryDto dto)
        {
            var data = await _positionGroupService.GetPositionGroupListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改职位分组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [HasPermission("Org.PositionGroup.Update")]
        public async Task<AppResponse<bool>> UpdatePositionGroupAsync([FromBody] PositionGroupDto dto)
        {
            await _positionGroupService.UpdatePositionGroupAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除职位分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:guid}")]
        [HasPermission("Org.PositionGroup.Delete")]
        [ApiAccessLog(operateName: "删除职位分组", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeletePositionGroupAsync(Guid id)
        {
            await _positionGroupService.DeletePositionGroupAsync(id);
            return Result.Ok();
        }
    }
}