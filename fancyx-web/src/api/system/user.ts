import httpClient from '@/utils/httpClient';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增用户
 * @param req
 */
export function addUser(req: AddUserRequest) {
  return httpClient.post<AddUserRequest, AppResponse<boolean>>('/admin-api/User/Add', req);
}

/**
 * 用户分页列表
 * @param req
 */
export function getUserList(req: GetUserListRequest) {
  return httpClient.get<GetUserListRequest, AppResponse<PagedResult<UserItem>>>('/admin-api/User/List', { params: req });
}

/**
 * 删除用户
 * @param id
 */
export function deleteUser(id: string) {
  return httpClient.delete<AddUserRequest, AppResponse<boolean>>('/admin-api/User/Delete/' + id);
}

/**
 * 分配角色
 * @param req
 */
export function assignRole(req: AssignRoleRequest) {
  return httpClient.post<AssignRoleRequest, AppResponse<boolean>>('/admin-api/User/AssignRole', req);
}

/**
 * 切换用户启用状态
 * @param id
 */
export function switchUserEnabledStatus(id: string) {
  return httpClient.put<string, AppResponse<boolean>>('/admin-api/User/ChangeEnabled/' + id);
}

/**
 * 获取指定用户角色
 * @param uid
 */
export function getUserRoleIds(uid: string) {
  return httpClient.get<string, AppResponse<string[]>>('/admin-api/User/Roles/' + uid);
}

/**
 * 重置用户密码
 * @param req
 */
export function resetUserPwd(req: ResetUserPwdRequest) {
  return httpClient.put<string, AppResponse<boolean>>('/admin-api/User/ResetPwd', req);
}

/**
 * 用户简单信息查询
 * @param keyword 账号/昵称
 */
export function getUserSimpleInfos(keyword?: string) {
  return httpClient.get<string, AppResponse<UserSimpleInfo[]>>('/admin-api/User/SimpleUserInfos', {
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
  return httpClient.get<string, AppResponse<UserDetails[]>>('/admin-api/User/EditInfo?id=' + id);
}

/**
 * 修改用户
 * @param req
 * @returns
 */
export function updateUser(req: UpdateUserRequest) {
  return httpClient.put<UpdateUserRequest, AppResponse<boolean>>('/admin-api/User/Update', req);
}

export interface UserDetails {
  id?: string | null;
  userName: string;
  nickName?: string | null;
  sex: number;
  phone?: string;
  deptId?: string | null;
  postId?: string | null;
}

export interface UpdateUserRequest {
  id?: string | null;
  nickName?: string | null;
  sex: number;
  phone?: string;
  deptId?: string | null;
  postId?: string | null;
}

export interface AddUserRequest {
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

export interface GetUserListRequest extends PageSearch {
  userName?: string | null;
}

export interface UserItem {
  id: string;
  userName: string | null;
  avatar: string | null;
  nickName: string | null;
  sex: number;
  isEnabled: boolean;
  deptName: string | null;
  postName: string | null;
}

export interface AssignRoleRequest {
  userId: string;
  roleIds: string[] | null;
}

export interface ResetUserPwdRequest {
  userId: string;
  password: string;
}

export interface UserSimpleInfo {
  id: string;
  userName: string;
  nickName: string;
}
