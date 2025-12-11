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
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [HasPermission("Org.Dept.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddDeptAsync([FromBody] AddOrUpdateDeptRequest req)
        {
            await _deptService.AddDeptAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 部门树形列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Org.Dept.List")]
        public async Task<AppResponse<List<DeptItem>>> GetDeptListAsync([FromQuery] GetDeptListRequest req)
        {
            var data = await _deptService.GetDeptListAsync(req);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [HasPermission("Org.Dept.Update")]
        public async Task<AppResponse<bool>> UpdateDeptAsync([FromBody] AddOrUpdateDeptRequest req)
        {
            await _deptService.UpdateDeptAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        [HasPermission("Org.Dept.Delete")]
        [ApiAccessLog(operateName: "删除部门", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteDeptAsync(long id)
        {
            await _deptService.DeleteDeptAsync(id);
            return Result.Ok();
        }

        [HttpGet("GetDeptSimpleInfos")]
        [HasPermission("Org.Dept.GetDeptSimpleInfos")]
        public async Task<AppResponse<List<DeptSimpleInfo>>> GetDeptSimpleInfosAsync([FromQuery] string? keyword)
        {
            var data = await _deptService.GetDeptSimpleInfosAsync(keyword);
            return Result.Data(data);
        }

        [HttpGet("{id}")]
        [HasPermission("Org.Dept.List")]
        public async Task<AppResponse<DeptItem>> GetDeptAsync(long id)
        {
            var data = await _deptService.GetDeptAsync(id);
            return Result.Data(data);
        }
    }
}