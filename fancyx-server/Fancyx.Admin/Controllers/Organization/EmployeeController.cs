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
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [HasPermission("Org.Employee.Add")]
        [EnableRateLimiting(RateLimiterConsts.DebouncePolicy)]
        public async Task<AppResponse<bool>> AddEmployeeAsync([FromBody] EmployeeDto dto)
        {
            await _employeeService.AddEmployeeAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 员工列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        [HasPermission("Org.Employee.List")]
        public async Task<AppResponse<PagedResult<EmployeeListDto>>> GetEmployeePagedListAsync([FromQuery] EmployeeQueryDto dto)
        {
            var data = await _employeeService.GetEmployeePagedListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [HasPermission("Org.Employee.Update")]
        public async Task<AppResponse<bool>> UpdateEmployeeAsync([FromBody] EmployeeDto dto)
        {
            await _employeeService.UpdateEmployeeAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:guid}")]
        [HasPermission("Org.Employee.Delete")]
        [ApiAccessLog(operateName: "删除员工", operateType: [OperateType.Delete], reponseEnable: true)]
        public async Task<AppResponse<bool>> DeleteEmployeeAsync(Guid id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return Result.Ok();
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("bindUser")]
        [HasPermission("Org.Employee.BindUser")]
        public async Task<AppResponse<bool>> EmployeeBindUserAsync([FromBody] EmployeeBindUserDto dto)
        {
            await _employeeService.EmployeeBindUserAsync(dto);
            return Result.Ok();
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("info/{id:guid}")]
        [HasPermission("Org.Employee.List")]
        public async Task<AppResponse<EmployeeInfoDto>> GetEmployeeInfoAsync(Guid id)
        {
            var data = await _employeeService.GetEmployeeInfoAsync(id);
            return Result.Data(data);
        }

        /// <summary>
        /// 员工列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeList")]
        public async Task<AppResponse<List<EmployeeDto>>> GetEmployeeListAsync([FromQuery] EmployeeQueryDto dto)
        {
            var data = await _employeeService.GetEmployeeListAsync(dto);
            return Result.Data(data);
        }

        /// <summary>
        /// 部门+员工树形
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("GetDeptEmployeeTree")]
        public async Task<AppResponse<List<DeptEmployeeTreeDto>>> GetDeptEmployeeTreeAsync([FromQuery] DeptEmployeeTreeQueryDto dto)
        {
            var data = await _employeeService.GetDeptEmployeeTreeAsync(dto);
            return Result.Data(data);
        }
    }
}