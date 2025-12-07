import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch, AppOption } from '@/types/api';

/**
 * 新增角色
 * @param dto
 */
export function addRole(dto: AddOrUpdateRoleRequest) {
  return httpClient.post<AddOrUpdateRoleRequest, AppResponse<boolean>>('/admin-api/Role/Add', dto);
}

/**
 * 角色分页列表
 * @param dto
 */
export function getRoleList(dto: GetRoleListRequest) {
  return httpClient.get<GetRoleListRequest, AppResponse<PagedResult<RoleItem>>>('/admin-api/Role/List', { params: dto });
}

/**
 * 修改角色
 * @param dto
 */
export function updateRole(dto: AddOrUpdateRoleRequest) {
  return httpClient.put<AddOrUpdateRoleRequest, AppResponse<boolean>>('/admin-api/Role/Update', dto);
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
 * @param dto
 */
export function assignMenu(dto: AssignMenuRequest) {
  return httpClient.post<AssignMenuRequest, AppResponse<boolean>>('/admin-api/Role/AssignMenu', dto);
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
 * @param dto
 */
export function assignData(dto: AssignDataScopeRequest) {
  return httpClient.post<AssignDataScopeRequest, AppResponse<boolean>>('/admin-api/Role/AssignData', dto);
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
 * @param dto
 * @returns
 */
export function assignDataScope(dto: AssignDataScopeDto) {
  return httpClient.post<AssignDataScopeDto, AppResponse<boolean>>('/admin-api/Role/AssignDataScope', dto);
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
