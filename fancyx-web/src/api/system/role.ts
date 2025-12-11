import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch, AppOption } from '@/types/api';

/**
 * 新增角色
 * @param req
 */
export function addRole(req: AddOrUpdateRoleRequest) {
  return httpClient.post<AddOrUpdateRoleRequest, AppResponse<boolean>>('/admin-api/Role/Add', req);
}

/**
 * 角色分页列表
 * @param req
 */
export function getRoleList(req: GetRoleListRequest) {
  return httpClient.get<GetRoleListRequest, AppResponse<PagedResult<RoleItem>>>('/admin-api/Role/List', { params: req });
}

/**
 * 修改角色
 * @param req
 */
export function updateRole(req: AddOrUpdateRoleRequest) {
  return httpClient.put<AddOrUpdateRoleRequest, AppResponse<boolean>>('/admin-api/Role/Update', req);
}

/**
 * 删除角色
 * @param id
 */
export function deleteRole(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>(`/admin-api/Role/Delete/${id}`);
}

/**
 * 分配菜单
 * @param req
 */
export function assignMenu(req: AssignMenuRequest) {
  return httpClient.post<AssignMenuRequest, AppResponse<boolean>>('/admin-api/Role/AssignMenu', req);
}

/**
 * 获取角色
 */
export function getRoleOptions() {
  return httpClient.get<unknown, AppResponse<AppOption[]>>('/admin-api/Role/Options');
}

/**
 * 获取指定角色菜单
 * @param id
 */
export function getRoleMenuIds(id: string) {
  return httpClient.get<string, AppResponse<string[]>>(`/admin-api/Role/Menus/${id}`);
}

/**
 * 分配数据
 * @param req
 */
export function assignData(req: AssignDataScopeRequest) {
  return httpClient.post<AssignDataScopeRequest, AppResponse<boolean>>('/admin-api/Role/AssignData', req);
}

/**
 * 获取角色部门权限编码
 * @param roleId
 * @returns
 */
export function getRoleDeptPowerInfo(roleId: string) {
  return httpClient.get<string, AppResponse<RoleDeptPowerInfo>>('/admin-api/Role/GetRoleDeptPowerInfo?roleId=' + roleId);
}

/**
 * 分配角色数据权限
 * @param req
 * @returns
 */
export function assignDataScope(req: AssignDataScopeDto) {
  return httpClient.post<AssignDataScopeDto, AppResponse<boolean>>('/admin-api/Role/AssignDataScope', req);
}

/**
 * 角色详情
 * @param id 
 * @returns 
 */
export function getRole(id: string) {
  return httpClient.get<string, AppResponse<RoleDetails>>(`/admin-api/Role/${id}`)
}

export interface AddOrUpdateRoleRequest {
  id?: string | null;
  roleName: string;
  remark?: string | null;
  isEnabled: boolean;
}

export interface RoleItem {
  id: string;
  roleName: string;
  remark: string | null;
  isEnabled: boolean;
  creationTime: string;
}

export interface GetRoleListRequest extends PageSearch {
  roleName?: string | null;
}

export interface AssignMenuRequest {
  menuIds: string[] | null;
  roleId: string;
}

export interface AssignDataScopeRequest {
  roleId: string;
  powerDataType: number;
  deptIds: string[] | null;
}

export interface RolePowerInfoDto {
  deptIds: string[];
  deptPowerType: number;
  allDeptIds: string[];
}

export interface DeptTreeOptionDto {
  key: string;
  title: string;
  children?: DeptTreeOptionDto[];
}
export interface RoleDeptPowerInfo {
  powerInfo: RolePowerInfoDto;
  deptOptions: DeptTreeOptionDto[];
}

export interface AssignDataScopeDto {
  roleId: string;
  deptPowerType: number;
  deptIds: string[] | null;
}

export interface RoleDetails extends RoleItem {
  menuIds: string[] | null;
  deptPowerType: number;
  deptIds: string[] | null;
}