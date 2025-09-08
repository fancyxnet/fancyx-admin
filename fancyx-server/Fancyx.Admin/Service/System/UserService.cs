using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Admin.SharedService;
using Fancyx.Core.Interfaces;
using Fancyx.Core.Utils;
using Fancyx.Logger;
using Fancyx.Repository;
using Fancyx.Repository.Entities.System;
using Fancyx.Repository.Enums;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Generated;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Service.System
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserDO> _userRepository;
        private readonly IRepository<UserRoleDO> _userRoleRepository;
        private readonly IdentitySharedService _identityDomainService;
        private readonly ICurrentUser _currentUser;

        public UserService(IRepository<UserDO> userRepository, IRepository<UserRoleDO> userRoleRepository
            , IdentitySharedService identityDomainService, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _identityDomainService = identityDomainService;
            _currentUser = currentUser;
        }

        public async Task<Guid> AddUserAsync(UserDto dto)
        {
            var isExist = await _userRepository.AnyAsync(x => x.UserName.ToLower() == dto.UserName.ToLower());
            if (isExist)
            {
                throw new BusinessException("账号已存在");
            }
            if (!RegexCodeGen.Password().IsMatch(dto.Password))
            {
                throw new BusinessException("密码格式不正确");
            }
            var user = new UserDO
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                PasswordSalt = EncryptionUtils.GetPasswordSalt(),
                Avatar = dto.Avatar,
                NickName = dto.NickName ?? dto.UserName,
                Sex = dto.Sex,
                Phone = dto.Phone,
                IsEnabled = true
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
                var items = new List<UserRoleDO>();
                foreach (var item in dto.RoleIds)
                {
                    items.Add(new UserRoleDO
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
            var resp = await _userRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.UserName), x => x.UserName.Contains(dto.UserName!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new UserListDto { Id = x.Id, Avatar = x.Avatar , UserName = x.UserName, Sex = x.Sex.GetHashCode(), IsEnabled = x.IsEnabled, NickName = x.NickName, Phone = x.Phone })
                .PagedAsync(dto.Current, dto.PageSize);

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

        [AsyncLogRecord(LogRecordConsts.SysUser, LogRecordConsts.SysUserResetPwdSubType, "{{id}}", LogRecordConsts.SysUserResetPwdContent)]
        public async Task ResetUserPasswordAsync(ResetUserPwdDto dto)
        {
            var user = await _userRepository.GetAsync(x => x.Id == dto.UserId) ?? throw new EntityNotFoundException();
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
    }
}