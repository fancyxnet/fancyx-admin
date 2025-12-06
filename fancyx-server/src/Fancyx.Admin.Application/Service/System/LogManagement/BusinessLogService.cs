using AutoMapper;

using Fancyx.Admin.Application.IService.System.LogManagement;
using Fancyx.Admin.Application.IService.System.LogManagement.Dtos;
using Fancyx.Admin.EfCore;
using Fancyx.EfCore;
using Fancyx.Shared.Logger.Entities;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.System.LogManagement
{
    public class BusinessLogService : IBusinessLogService
    {
        private readonly IRepository<LogRecord> _logRecordRepository;
        private readonly IMapper _mapper;

        public BusinessLogService(IRepository<LogRecord> logRecordRepository, IMapper mapper)
        {
            _logRecordRepository = logRecordRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<BusinessLogItem>> GetBusinessLogListAsync(GetBusinessLogListRequest dto)
        {
            var resp = await _logRecordRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Type), x => x.Type == dto.Type)
                .WhereIf(!string.IsNullOrEmpty(dto.SubType), x => x.SubType != null && x.SubType.Contains(dto.SubType!))
                .WhereIf(!string.IsNullOrEmpty(dto.Content), x => x.Content != null && x.Content.Contains(dto.Content!))
                .WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName != null && x.UserName.Contains(dto.UserName!))
                .OrderByDescending(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<BusinessLogItem>(dto, resp.Total, _mapper.Map<List<LogRecord>, List<BusinessLogItem>>(resp.Items));
        }

        public Task<List<AppOption>> GetBusinessTypeOptionsAsync(string? type)
        {
            return _logRecordRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(type), x => x.Type != null && x.Type.Contains(type!))
                .GroupBy(x => x.Type)
                .OrderBy(x => x.Key)
                .Select(x => new AppOption { Label = x.Key, Value = x.Key })
                .ToListAsync();
        }
    }
}