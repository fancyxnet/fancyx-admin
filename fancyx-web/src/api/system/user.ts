import httpClient from '@/utils/httpClient';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增用户
 * @param dto
 */
export function addUser(dto: UserDto) {
  return httpClient.post<UserDto, AppResponse<boolean>>('/admin-api/user/add', dto);
}

/**
 * 用户分页列表
 * @param dto
 */
export function getUserList(dto: UserQueryDto) {
  return httpClient.get<UserQueryDto, AppResponse<PagedResult<UserListDto>>>('/admin-api/user/list', { params: dto });
}

/**
 * 删除用户
 * @param id
 */
export function deleteUser(id: string) {
  return httpClient.delete<UserDto, AppResponse<boolean>>('/admin-api/user/delete/' + id);
}

/**
 * 分配角色
 * @param dto
 */
export function assignRole(dto: AssignRoleDto) {
  return httpClient.post<AssignRoleDto, AppResponse<boolean>>('/admin-api/user/assignRole', dto);
}

/**
 * 切换用户启用状态
 * @param id
 */
export function switchUserEnabledStatus(id: string) {
  return httpClient.put<string, AppResponse<boolean>>('/admin-api/user/changeEnabled/' + id);
}

/**
 * 获取指定用户角色
 * @param uid
 */
export function getUserRoleIds(uid: string) {
  return httpClient.get<string, AppResponse<string[]>>('/admin-api/user/roles/' + uid);
}

/**
 * 重置用户密码
 * @param dto
 */
export function resetUserPwd(dto: ResetUserPwdDto) {
  return httpClient.put<string, AppResponse<boolean>>('/admin-api/user/resetPwd', dto);
}

/**
 * 用户简单信息查询
 * @param keyword 账号/昵称
 */
export function getUserSimpleInfos(keyword?: string) {
  return httpClient.get<string, AppResponse<UserSimpleInfoDto[]>>('/admin-api/user/simpleUserInfos', {
    params: {
      keyword,
    },
  });
}

/**
 * 获取用户编辑信息
 * @param id 用户ID
 * @returns
 */
export function getUserEditInfo(id: string) {
  return httpClient.get<string, AppResponse<UserEditInfoDto[]>>('/admin-api/user/EditInfo?id=' + id);
}

/**
 * 修改用户
 * @param dto
 * @returns
 */
export function updateUser(dto: UserEditDto) {
  return httpClient.put<UserEditDto, AppResponse<boolean>>('/admin-api/user/update', dto);
}

export interface UserEditInfoDto {
  id?: string | null;
  userName: string;
  nickName?: string | null;
  sex: number;
  phone?: string;
  deptId?: string | null;
  postId?: string | null;
}

export interface UserEditDto {
  id?: string | null;
  nickName?: string | null;
  sex: number;
  phone?: string;
  deptId?: string | null;
  postId?: string | null;
}

export interface UserDto {
  id?: string | null;
  userName: string;
  password: string;
  avatar?: string | null;
  nickName?: string | null;
  sex: number;
  isEnabled: boolean;
  phone?: string;
  deptId?: string | null;
  postId?: string | null;
}

export interface UserQueryDto extends PageSearch {
  userName?: string | null;
}

export interface UserListDto {
  id: string;
  userName: string | null;
  avatar: string | null;
  nickName: string | null;
  sex: number;
  isEnabled: boolean;
  deptName: string | null;
  postName: string | null;
}

export interface AssignRoleDto {
  userId: string;
  roleIds: string[] | null;
}

export interface ResetUserPwdDto {
  userId: string;
  password: string;
}

export interface UserSimpleInfoDto {
  id: string;
  userName: string;
  nickName: string;
}
