using AutoMapper;
using Fancyx.Admin.Application.IService.Feedback;
using Fancyx.Admin.Application.IService.Feedback.Models;
using Fancyx.Admin.EfCore.Entities.Feedback;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Admin.EfCore.Models;
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

        public Task CreateTicketAsync(TicketCreateRequest req)
        {
            var ticket = new Ticket()
            {
                Title = req.Title,
                Content = req.Content,
                UserId = _currentUser.Id.GetValueOrDefault()
            };
            return _ticketRepository.InsertAsync(ticket);
        }

        public async Task EvaluationTicketAsync(EvaluationTicketRequest req)
        {
            var ticket = await _ticketRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
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
            ticket.Rating = req.Rating;
            ticket.RatingComment = req.RatingComment;
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task<TicketDetails> GetTicketDetailsAsync(long id)
        {
            var ticket = await _ticketRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            var model = new TicketDetails
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Content = ticket.Content,
                Status = ticket.Status,
                Rating = ticket.Rating,
                RatingComment = ticket.RatingComment,
                CreationTime = ticket.CreationTime,
                ReplyList = _mapper.Map<List<TicketReplyListDto>>(await _ticketRepository.QueryReplyListAsync(id))
            };
            model.ReplyCount = model.ReplyList?.Count ?? 0;
            return model;
        }

        public async Task<PagedResult<TicketItem>> GetTicketListAsync(GetTicketListRequest req)
        {
            var data = await _ticketRepository.QueryListAsync(req.Current, req.PageSize);
            return new PagedResult<TicketItem>(data.Total, _mapper.Map<List<TicketItem>>(data.Items));
        }

        public async Task<PagedResult<UserTicketItem>> GetUserTicketListAsync(GetUserTicketListRequest req)
        {
            var query = _ticketRepository.GetQueryable().Where(x => x.UserId == _currentUser.Id).WhereIf(!string.IsNullOrEmpty(req.Title), x => x.Title.StartsWith(req.Title!));
            var data = await query.GroupJoin(_ticketReplyRepository.GetQueryable(), t => t.Id, g => g.TicketId,
                (t, g) => new UserTicketItem
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    Rating = t.Rating,
                    RatingComment = t.RatingComment,
                    CreationTime = t.CreationTime,
                    ReplyCount = g.Where(gs => gs.SenderId != t.UserId).Count()
                }).PagedAsync(req.Current, req.PageSize);
            return new PagedResult<UserTicketItem>(data.Total, data.Items);
        }

        public async Task ReplyTicketAsync(ReplyTicketRequest req)
        {
            var ticket = await _ticketRepository.FindAsync(req.TicketId) ?? throw new EntityNotFoundException();
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
                TicketId = req.TicketId,
                SenderId = _currentUser.Id.GetValueOrDefault(),
                Content = req.Content
            };
            await _ticketReplyRepository.InsertAsync(ticketReply, false);
            await uow.CommitAsync();
        }
    }
}