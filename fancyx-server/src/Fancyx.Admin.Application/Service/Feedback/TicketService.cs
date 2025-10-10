using AutoMapper;
using Fancyx.Admin.Application.IService.Feedback;
using Fancyx.Admin.Application.IService.Feedback.Dtos;
using Fancyx.Admin.EfCore.Entities.Feedback;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Admin.EfCore.Repositories;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.Feedback
{
    public class TicketService : ITicketService
    {
        private readonly TicketRepository _ticketRepository;
        private readonly IRepository<TicketReply> _ticketReplyRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TicketService(TicketRepository ticketRepository, IRepository<TicketReply> ticketReplyRepository, IMapper mapper, ICurrentUser currentUser
            , IUnitOfWorkManager unitOfWorkManager)
        {
            _ticketRepository = ticketRepository;
            _ticketReplyRepository = ticketReplyRepository;
            _mapper = mapper;
            _currentUser = currentUser;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public Task CloseTicketAsync(long id)
        {
            return _ticketRepository.Where(x => x.Id == id).ExecuteUpdateAsync(e => e
                .SetProperty(s => s.Status, TicketStatus.Closed)
                .SetProperty(s => s.LastModificationTime, DateTime.Now)
                .SetProperty(s => s.LastModifierId, _currentUser.Id));
        }

        public Task CreateTicketAsync(TicketCreateDto dto)
        {
            var ticket = new Ticket()
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = _currentUser.Id.GetValueOrDefault()
            };
            return _ticketRepository.InsertAsync(ticket);
        }

        public async Task EvaluationTicketAsync(EvaluationTicketDto dto)
        {
            var ticket = await _ticketRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            if (!_currentUser.Id.Equals(ticket.UserId))
            {
                throw new BusinessException("不是工单发起人，不能评价");
            }
            if (ticket.Rating > 0)
            {
                throw new BusinessException("工单已评价，不能再次操作");
            }
            if (!ticket.Status.Equals(TicketStatus.Closed))
            {
                throw new BusinessException("工单关闭后才能评价");
            }
            ticket.Rating = dto.Rating;
            ticket.RatingComment = dto.RatingComment;
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task<PagedResult<TicketListDto>> GetTicketListAsync(TicketQueryDto dto)
        {
            var data = await _ticketRepository.QueryListAsync(dto.Current, dto.PageSize);
            return new PagedResult<TicketListDto>(data.Total, _mapper.Map<List<TicketListDto>>(data.Items));
        }

        public async Task<PagedResult<UserTicketListDto>> GetUserTicketListAsync(UserTicketQueryDto dto)
        {
            var query = _ticketRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.Title), x => x.Title.StartsWith(dto.Title!));
            var data = await query.GroupJoin(_ticketReplyRepository.GetQueryable(), t => t.Id, g => g.TicketId,
                (t, g) => new UserTicketListDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    Rating = t.Rating,
                    RatingComment = t.RatingComment,
                    CreationTime = t.CreationTime,
                    ReplyCount = g.Where(gs => gs.SenderId != t.UserId).Count()
                }).PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<UserTicketListDto>(data.Total, data.Items);
        }

        public async Task ReplyTicketAsync(ReplyTicketDto dto)
        {
            var ticket = await _ticketRepository.FindAsync(dto.TicketId) ?? throw new EntityNotFoundException();
            if (ticket.Status.Equals(TicketStatus.Closed))
            {
                throw new BusinessException("工单已关闭，不能回复");
            }
            using var uow = await _unitOfWorkManager.BeginAsync();
            //工作人员回复了，工单标记进行中
            if (ticket.Status.Equals(TicketStatus.Open) && _currentUser.Id != ticket.UserId)
            {
                ticket.Status = TicketStatus.Processing;
                ticket.AssignedUserId = _currentUser.Id.GetValueOrDefault();
                await _ticketRepository.UpdateAsync(ticket, false);
            }
            var ticketReply = new TicketReply
            {
                TicketId = dto.TicketId,
                SenderId = _currentUser.Id.GetValueOrDefault(),
                Content = dto.Content
            };
            await _ticketReplyRepository.InsertAsync(ticketReply, false);
            await _unitOfWorkManager.SaveChangeAsync();
            await uow.CommitAsync();
        }
    }
}