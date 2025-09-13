using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Admin.SharedService;
using Fancyx.Core.Extensions;
using Fancyx.Core.Interfaces;
using Fancyx.Core.Utils;
using Fancyx.DataAccess;
using Fancyx.DataAccess.Entities.System;
using Fancyx.DataAccess.Enums;
using Fancyx.Logger;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Generated;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Service.System
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IdentitySharedService _identityDomainService;
        private readonly ICurrentUser _currentUser;
        private readonly FancyxDbContext _context;

        public UserService(IRepository<User> userRepository, IRepository<UserRole> userRoleRepository
            , IdentitySharedService identityDomainService, ICurrentUser currentUser, FancyxDbContext context)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _identityDomainService = identityDomainService;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<Guid> AddUserAsync(UserDto dto)
        {
            var userNameIsExist = await _userRepository.AnyAsync(x => x.UserName.ToLower() == dto.UserName.ToLower());
            if (userNameIsExist)
            {
                throw new BusinessException("账号已存在");
            }
            if (!string.IsNullOrEmpty(dto.Phone) && await _userRepository.AnyAsync(x => x.Phone == dto.Phone))
            {
                throw new BusinessException("手机号已存在");
            }
            if (!RegexCodeGen.Password().IsMatch(dto.Password))
            {
                throw new BusinessException("密码格式不正确");
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                PasswordSalt = EncryptionUtils.GetPasswordSalt(),
                Avatar = dto.Avatar,
                NickName = dto.NickName ?? dto.UserName,
                Sex = dto.Sex,
                Phone = dto.Phone,
                IsEnabled = true,
                DeptId = dto.DeptId,
                PostId = dto.PostId
            };
            if (string.IsNullOrWhiteSpace(dto.Avatar))
            {
                user.Avatar = user.Sex == SexType.Male ? AdminConsts.AvatarMale : AdminConsts.AvatarFemale;
            }
            user.Password = EncryptionUtils.GenEncodingPassword(dto.Password, user.PasswordSalt);
            await _userRepository.InsertAsync(user);
            return user.Id;
        }

        public async Task<bool> AssignRoleAsync(AssignRoleDto dto)
        {
            await _userRoleRepository.DeleteAsync(x => x.UserId == dto.UserId);
            if (dto.RoleIds != null)
            {
                var items = new List<UserRole>();
                foreach (var item in dto.RoleIds)
                {
                    items.Add(new UserRole
                    {
                        UserId = dto.UserId,
                        RoleId = item
                    });
                }
                if (items.Count > 0)
                {
                    await _userRoleRepository.InsertManyAsync(items);
                }
            }
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            if (_currentUser.Id == id)
            {
                throw new BusinessException("不能删除自己");
            }
            await _userRepository.DeleteAsync(x => x.Id == id);
            await _identityDomainService.DelUserPermissionCacheByUserIdAsync(id);
            return true;
        }

        public async Task<PagedResult<UserListDto>> GetUserListAsync(UserQueryDto dto)
        {
            var resp = await _context.User.PowerFilter(_currentUser).GroupJoin(_context.Dept, u => u.DeptId, d => d.Id, (u, d) => new { u, d })
                .SelectMany(x => x.d.DefaultIfEmpty(), (x, d) => new { x.u, d })
                .GroupJoin(_context.Position, m => m.u.PostId, p => p.Id, (m, p) => new { m, p })
                .WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.m.u.UserName.Contains(dto.UserName!))
                .WhereIf(dto.DeptId.HasValue, x => x.m.u.DeptId == dto.DeptId!.Value)
                .OrderByDescending(x => x.m.u.CreationTime)
                .SelectMany(x => x.p.DefaultIfEmpty(), (x, p) => new UserListDto
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
                }).PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<UserListDto>(resp.Total, resp.Items);
        }

        public async Task<Guid[]> GetUserRoleIdsAsync(Guid uid)
        {
            return [.. await _userRoleRepository.Where(x => x.UserId == uid).SelectToListAsync(x => x.RoleId)];
        }

        public async Task<bool> SwitchUserEnabledStatusAsync(Guid id)
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

        [AsyncLogRecord(LogRecordConsts.User, LogRecordConsts.UserResetPwdSubType, "{{id}}", LogRecordConsts.UserResetPwdContent)]
        public async Task ResetUserPasswordAsync(ResetUserPwdDto dto)
        {
            var user = await _userRepository.FindAsync(dto.UserId) ?? throw new EntityNotFoundException();
            if (!RegexCodeGen.Password().IsMatch(dto.Password))
            {
                throw new BusinessException("密码格式不正确");
            }

            user.PasswordSalt = EncryptionUtils.GetPasswordSalt();
            user.Password = EncryptionUtils.GenEncodingPassword(dto.Password!, user.PasswordSalt);
            await _userRepository.UpdateAsync(user);

            LogRecordContext.PutVariable("id", user.Id);
            LogRecordContext.PutVariable("userName", user.UserName);
        }

        public Task<List<UserSimpleInfoDto>> GetUserSimpleInfosAsync(string? keyword)
        {
            return _userRepository.Where(x => x.IsEnabled)
                .WhereIf(!string.IsNullOrEmpty(keyword), x => x.UserName.Contains(keyword!) || x.NickName.Contains(keyword!))
                .OrderBy(x => x.NickName)
                .Select(x => new UserSimpleInfoDto { Id = x.Id.ToString(), NickName = x.NickName, UserName = x.UserName })
                .ToListAsync();
        }

        public Task<List<UserListDto>> ExportUserListAsync(UserQueryDto dto)
        {
            return _userRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName.Contains(dto.UserName!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new UserListDto { Id = x.Id, UserName = x.UserName, Phone = x.Phone, Avatar = x.Avatar, IsEnabled = x.IsEnabled, NickName = x.NickName, Sex = x.Sex.GetHashCode() })
                .ToListAsync();
        }

        public async Task UpdateUserAsync(UserEditDto dto)
        {
            var user = await _userRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            if (!string.IsNullOrEmpty(dto.Phone) && user.Phone != dto.Phone)
            {
                if (await _userRepository.AnyAsync(x => x.Phone == dto.Phone))
                {
                    throw new BusinessException("手机号已存在");
                }
            }
            user.NickName = dto.NickName;
            user.Phone = dto.Phone;
            user.Sex = dto.Sex;
            user.DeptId = dto.DeptId;
            user.PostId = dto.PostId;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserEditInfoDto> GetUserEditInfoAsync(Guid id)
        {
            var user = await _userRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return user.Mapper<User, UserEditInfoDto>();
        }
    }
}