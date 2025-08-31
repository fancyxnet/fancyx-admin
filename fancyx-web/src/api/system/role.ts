import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch, AppOption } from '@/types/api';

/**
 * 新增角色
 * @param dto
 */
export function addRole(dto: RoleDto) {
  return httpClient.post<RoleDto, AppResponse<boolean>>('/api/role/add', dto);
}

/**
 * 角色分页列表
 * @param dto
 */
export function getRoleList(dto: RoleQueryDto) {
  return httpClient.get<RoleQueryDto, AppResponse<PagedResult<RoleListDto>>>('/api/role/list', { params: dto });
}

/**
 * 修改角色
 * @param dto
 */
export function updateRole(dto: RoleDto) {
  return httpClient.put<RoleDto, AppResponse<boolean>>('/api/role/update', dto);
}

/**
 * 删除角色
 * @param id
 */
export function deleteRole(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>(`/api/role/delete/${id}`);
}

/**
 * 分配菜单
 * @param dto
 */
export function assignMenu(dto: AssignMenuDto) {
  return httpClient.post<AssignMenuDto, AppResponse<boolean>>('/api/role/assignMenu', dto);
}

/**
 * 获取角色
 */
export function getRoleOptions() {
  return httpClient.get<unknown, AppResponse<AppOption[]>>('/api/role/options');
}

/**
 * 获取指定角色菜单
 * @param id
 */
export function getRoleMenuIds(id: string) {
  return httpClient.get<string, AppResponse<string[]>>(`/api/role/menus/${id}`);
}

/**
 * 分配数据
 * @param dto
 */
export function assignData(dto: AssignDataDto) {
  return httpClient.post<AssignDataDto, AppResponse<boolean>>('/api/role/assignData', dto);
}

/**
 * 获取角色部门权限编码
 * @param roleId
 * @returns
 */
export function getRoleDeptPowerInfo(roleId: string) {
  return httpClient.get<string, AppResponse<RoleDeptPowerInfo>>('/api/role/GetRoleDeptPowerInfo?roleId=' + roleId);
}

/**
 * 分配角色数据权限
 * @param dto
 * @returns
 */
export function assignDataScope(dto: AssignDataScopeDto) {
  return httpClient.post<AssignDataScopeDto, AppResponse<boolean>>('/api/role/assignDataScope', dto);
}

export interface RoleDto {
  id?: string | null;
  roleName: string;
  remark?: string | null;
  isEnabled: boolean;
}

export interface RoleListDto {
  id: string;
  roleName: string;
  remark: string | null;
  isEnabled: boolean;
  creationTime: string;
}

export interface RoleQueryDto extends PageSearch {
  roleName?: string | null;
}

export interface AssignMenuDto {
  menuIds: string[] | null;
  roleId: string;
}

export interface AssignDataDto {
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
