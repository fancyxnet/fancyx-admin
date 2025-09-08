using Fancyx.Admin.IService.Organization;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.Core.Attributes;
using Fancyx.Logger;
using Fancyx.Repository.Enums;
using Fancyx.Shared.Consts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Fancyx.Admin.Controllers.Organization
{
    [Authorize]
    [ApiController]
    [Route("api/dept")]
    public class DeptController : ControllerBase
    {
        private readonly IDeptService _deptService;

        public DeptController(IDeptService deptService)
        {
            _deptService = deptService;
        }

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [HasPermission("Org.Dept.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddDeptAsync([FromBody] DeptDto dto)
        {
            await _deptService.AddDeptAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 部门树形列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [HasPermission("Org.Dept.List")]
        public async Task<AppResponse<List<DeptListDto>>> GetDeptListAsync([FromQuery] DeptQueryDto dto)
        {
            var data = await _deptService.GetDeptListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [HasPermission("Org.Dept.Update")]
        public async Task<AppResponse<bool>> UpdateDeptAsync([FromBody] DeptDto dto)
        {
            await _deptService.UpdateDeptAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:guid}")]
        [HasPermission("Org.Dept.Delete")]
        [ApiAccessLog(operateName: "删除部门", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteDeptAsync(Guid id)
        {
            await _deptService.DeleteDeptAsync(id);
            return Result.Ok();
        }

        [HttpGet("GetDeptSimpleInfos")]
        [HasPermission("Org.Dept.GetDeptSimpleInfos")]
        public async Task<AppResponse<List<DeptSimpleInfoDto>>> GetDeptSimpleInfosAsync([FromQuery] string? keyword)
        {
            var data = await _deptService.GetDeptSimpleInfosAsync(keyword);
            return Result.Data(data);
        }
    }
}