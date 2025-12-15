using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Erp.EfCore.Entities;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.BaseInfo
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(IRepository<Customer> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task AddCustomerAsync(AddOrUpdateCustomerRequest req)
        {
            if (await _customerRepository.AnyAsync(x => x.Code == req.Code))
            {
                throw new BusinessException("客户编号已存在");
            }

            var entity = _mapper.Map<Customer>(req);
            await _customerRepository.InsertAsync(entity);
        }

        public Task DeleteCustomerAsync(long id)
        {
            return _customerRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<CustomerItem>> GetCustomerListAsync(GetCustomerListRequest req)
        {
            var resp = await _customerRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Code), x => x.Code.StartsWith(req.Code!))
                .Select(x => new CustomerItem
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    CodeSlim = x.CodeSlim,
                    ContactName = x.ContactName,
                    ContactPhone = x.ContactPhone
                }).PagedAsync(req.Current, req.PageSize);
            return new PagedResult<CustomerItem>(req, resp.Total, resp.Items);
        }

        public async Task UpdateCustomerAsync(AddOrUpdateCustomerRequest req)
        {
            var entity = await _customerRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = await _customerRepository.AnyAsync(x => x.Code == req.Code);
            if (codeIsExist && entity.Code != req.Code)
            {
                throw new BusinessException("客户编号已存在");
            }
            entity.Code = req.Code;
            entity.Name = req.Name;
            entity.Remark = req.Remark;
            entity.CodeSlim = req.CodeSlim;
            entity.ContactName = req.ContactName;
            entity.ContactPhone = req.ContactPhone;
            await _customerRepository.UpdateAsync(entity);
        }
    }
}