using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers.BaseInfo
{
    [Authorize]
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [HasPermission("Erp.Customer.Add")]
        public async Task<AppResponse<bool>> AddCustomerAsync([FromBody] AddOrUpdateCustomerRequest req)
        {
            await _customerService.AddCustomerAsync(req);
            return Result.Ok();
        }

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [HasPermission("Erp.Customer.List")]
        public async Task<AppResponse<PagedResult<CustomerItem>>> GetCustomerListAsync([FromQuery] GetCustomerListRequest req)
        {
            var data = await _customerService.GetCustomerListAsync(req);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Erp.Customer.Update")]
        public async Task<AppResponse<bool>> UpdateCustomerAsync([FromBody] AddOrUpdateCustomerRequest req)
        {
            await _customerService.UpdateCustomerAsync(req);
            return Result.Ok();
        }

        [HttpDelete("Delete/{id}")]
        [HasPermission("Erp.Customer.Delete")]
        public async Task<AppResponse<bool>> DeleteCustomerAsync(long id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return Result.Ok();
        }
    }
}
