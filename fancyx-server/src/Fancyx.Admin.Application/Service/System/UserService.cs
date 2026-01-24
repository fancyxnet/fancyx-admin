using AutoMapper;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.EfCore.Aop;
using Fancyx.Shared.Consts;
using Fancyx.Shared.EfCore;
using Fancyx.Shared.Generated;
using Fancyx.Shared.Logger;
using Fancyx.Utils;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.System
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IdentitySharedService _identityDomainService;
        private readonly ICurrentUser _currentUser;
        private readonly FancyxDbContext _context;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IRepository<UserRole> userRoleRepository
            , IdentitySharedService identityDomainService, ICurrentUser currentUser, FancyxDbContext context
            , IMapper mapper)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _identityDomainService = identityDomainService;
            _currentUser = currentUser;
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddUserAsync(AddUserRequest req)
        {
            var userNameIsExist = await _userRepository.AnyAsync(x => x.UserName.ToLower() == req.UserName.ToLower());
            if (userNameIsExist)
            {
                throw new BusinessException("账号已存在");
            }
            if (!string.IsNullOrEmpty(req.Phone) && await _userRepository.AnyAsync(x => x.Phone == req.Phone))
            {
                throw new BusinessException("手机号已存在");
            }
            if (!RegexCodeGen.Password().IsMatch(req.Password))
            {
                throw new BusinessException("密码格式不正确");
            }
            var user = new User
            {
                UserName = req.UserName,
                PasswordSalt = EncryptionUtils.GetPasswordSalt(),
                Avatar = req.Avatar,
                NickName = req.NickName ?? req.UserName,
                Sex = req.Sex,
                Phone = req.Phone,
                IsEnabled = true,
                DeptId = req.DeptId,
                PostId = req.PostId
            };
            if (string.IsNullOrWhiteSpace(req.Avatar))
            {
                user.Avatar = user.Sex == SexType.Male ? AdminConsts.AvatarMale : AdminConsts.AvatarFemale;
            }
            user.Password = EncryptionUtils.GenEncodingPassword(req.Password, user.PasswordSalt);
            await _userRepository.InsertAsync(user);
            return user.Id;
        }

        [Transactional]
        public async Task<bool> AssignRoleAsync(AssignRoleRequest req)
        {
            await _userRoleRepository.DeleteAsync(x => x.UserId == req.UserId);
            if (req.RoleIds != null)
            {
                var items = new List<UserRole>();
                foreach (var item in req.RoleIds)
                {
                    items.Add(new UserRole
                    {
                        UserId = req.UserId,
                        RoleId = item
                    });
                }
                if (items.Count > 0)
                {
                    await _userRoleRepository.InsertManyAsync(items);
                }
            }
            await _identityDomainService.DelUserPermissionCacheByUserIdAsync(req.UserId);
            return true;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            if (_currentUser.Id == id)
            {
                throw new BusinessException("不能删除自己");
            }
            await _userRepository.DeleteAsync(x => x.Id == id);
            await _identityDomainService.DelUserPermissionCacheByUserIdAsync(id);
            return true;
        }

        public async Task<PagedResult<UserItem>> GetUserListAsync(GetUserListRequest req)
        {
            var resp = await _context.User.PowerFilter(_currentUser).GroupJoin(_context.Dept, u => u.DeptId, d => d.Id, (u, d) => new { u, d })
                .SelectMany(x => x.d.DefaultIfEmpty(), (x, d) => new { x.u, d })
                .GroupJoin(_context.Position, m => m.u.PostId, p => p.Id, (m, p) => new { m, p })
                .WhereIf(!string.IsNullOrEmpty(req.UserName), x => x.m.u.UserName.Contains(req.UserName!))
                .WhereIf(req.DeptId.HasValue, x => x.m.u.DeptId == req.DeptId!.Value)
                .OrderByDescending(x => x.m.u.CreationTime)
                .SelectMany(x => x.p.DefaultIfEmpty(), (x, p) => new UserItem
                {
                    Id = x.m.u.Id,
                    Avatar = x.m.u.Avatar,
                    UserName = x.m.u.UserName,
                    Sex = x.m.u.Sex.GetHashCode(),
                    IsEnabled = x.m.u.IsEnabled,
                    NickName = x.m.u.NickName,
                    Phone = x.m.u.Phone,
                    PostName = p != null ? p.Name : null,
                    DeptName = x.m.d != null ? x.m.d.Name : null
                }).PagedAsync(req.Current, req.PageSize);

            return new PagedResult<UserItem>(resp.Total, resp.Items);
        }

        public async Task<long[]> GetUserRoleIdsAsync(long uid)
        {
            return [.. await _userRoleRepository.Where(x => x.UserId == uid).SelectToListAsync(x => x.RoleId)];
        }

        public async Task<bool> SwitchUserEnabledStatusAsync(long id)
        {
            var entity = await _userRepository.Where(x => x.Id == id).FirstAsync()
                ?? throw new BusinessException("数据不存在");
            entity.IsEnabled = !entity.IsEnabled;
            await _userRepository.UpdateAsync(entity);

            if (!entity.IsEnabled)
            {
                await _identityDomainService.DelUserPermissionCacheByUserIdAsync(id);
            }
            return true;
        }

        [LogRecord(LogRecordConsts.User, LogRecordConsts.UserResetPwdSubType, "{{id}}", LogRecordConsts.UserResetPwdContent)]
        public async Task ResetUserPasswordAsync(ResetUserPwdRequest req)
        {
            var user = await _userRepository.FindAsync(req.UserId) ?? throw new EntityNotFoundException();
            if (!RegexCodeGen.Password().IsMatch(req.Password))
            {
                throw new BusinessException("密码格式不正确");
            }

            user.PasswordSalt = EncryptionUtils.GetPasswordSalt();
            user.Password = EncryptionUtils.GenEncodingPassword(req.Password!, user.PasswordSalt);
            await _userRepository.UpdateAsync(user);

            LogRecordContext.PutVariable("id", user.Id);
            LogRecordContext.PutVariable("userName", user.UserName);
        }

        public Task<List<UserSimpleInfo>> GetUserSimpleInfosAsync(string? keyword)
        {
            return _userRepository.Where(x => x.IsEnabled).PowerFilter(_currentUser)
                .WhereIf(!string.IsNullOrEmpty(keyword), x => x.UserName.Contains(keyword!) || x.NickName.Contains(keyword!))
                .OrderBy(x => x.NickName)
                .Select(x => new UserSimpleInfo { Id = x.Id.ToString(), NickName = x.NickName, UserName = x.UserName })
                .ToListAsync();
        }

        public Task<List<UserItem>> ExportUserListAsync(GetUserListRequest req)
        {
            return _userRepository.GetQueryable().PowerFilter(_currentUser)
                .WhereIf(!string.IsNullOrEmpty(req.UserName), x => x.UserName.Contains(req.UserName!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new UserItem { Id = x.Id, UserName = x.UserName, Phone = x.Phone, Avatar = x.Avatar, IsEnabled = x.IsEnabled, NickName = x.NickName, Sex = x.Sex.GetHashCode() })
                .ToListAsync();
        }

        public async Task UpdateUserAsync(UpdateUserRequest req)
        {
            var user = await _userRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            if (!string.IsNullOrEmpty(req.Phone) && user.Phone != req.Phone)
            {
                if (await _userRepository.AnyAsync(x => x.Phone == req.Phone))
                {
                    throw new BusinessException("手机号已存在");
                }
            }
            user.NickName = req.NickName;
            user.Phone = req.Phone;
            user.Sex = req.Sex;
            user.DeptId = req.DeptId;
            user.PostId = req.PostId;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserDetails> GetUserAsync(long id)
        {
            var user = await _userRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return _mapper.Map<User, UserDetails>(user);
        }
    }
}