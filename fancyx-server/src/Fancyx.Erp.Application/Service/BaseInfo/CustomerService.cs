using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Erp.EfCore.Entites;
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

        public async Task AddCustomerAsync(CustomerDto dto)
        {
            if (await _customerRepository.AnyAsync(x => x.Code == dto.Code))
            {
                throw new BusinessException("客户编号已存在");
            }

            var entity = _mapper.Map<Customer>(dto);
            await _customerRepository.InsertAsync(entity);
        }

        public async Task<PagedResult<CustomerListDto>> GetCustomerListAsync(CustomerQueryDto dto)
        {
            var resp = await _customerRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Code), x => x.Code.StartsWith(dto.Code!))
                .Select(x => new CustomerListDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    CodeSlim = x.CodeSlim,
                    ContactName = x.ContactName,
                    ContactPhone = x.ContactPhone
                }).PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<CustomerListDto>(dto, resp.Total, resp.Items);
        }
    }
}
