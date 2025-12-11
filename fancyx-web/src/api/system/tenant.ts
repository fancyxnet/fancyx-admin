import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';
import type { GetMenuOptionsResponse } from './menu';

/**
 * 新增租户
 * @param req
 */
export function addTenant(req: AddOrUpdateTenantRequest) {
  return httpClient.post<AddOrUpdateTenantRequest, AppResponse<boolean>>('/admin-api/Tenant/Add', req);
}

/**
 * 租户分页列表
 * @param req
 */
export function getTenantList(req: GetTenantListRequest) {
  return httpClient.get<GetTenantListRequest, AppResponse<PagedResult<TenantItem>>>('/admin-api/Tenant/List', {
    params: req,
  });
}

/**
 * 修改租户
 * @param req
 */
export function updateTenant(req: AddOrUpdateTenantRequest) {
  return httpClient.put<AddOrUpdateTenantRequest, AppResponse<boolean>>('/admin-api/Tenant/Update', req);
}

/**
 * 删除租户
 * @param id
 */
export function deleteTenant(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>(`/admin-api/Tenant/Delete/${id}`);
}

/**
 * 分配租户菜单
 * @param req
 */
export function assignTenantMenu(req: AssignTenantMenuRequest) {
  return httpClient.post<AssignTenantMenuRequest, AppResponse<boolean>>('/admin-api/Tenant/AssignTenantMenu', req);
}

/**
 * 租户已有菜单ID列表
 * @param req
 */
export function getTenantMenuIds(id: string) {
  return httpClient.get<string, AppResponse<string[]>>('/admin-api/Tenant/GetTenantMenuIds/' + id);
}

/**
 * 租户详情
 * @param id 
 * @returns 
 */
export function getTenant(id: string) {
  return httpClient.get<string, AppResponse<TenantDetails>>(`/admin-api/Tenant/${id}`)
}

/**
 * 初始管理员账号
 * @param req
 * @returns
 */
export function createTenantAccount(req: CreateTenantAccountRequest) {
  return httpClient.post<CreateTenantAccountRequest, AppResponse<TenantAccountInfo>>('/admin-api/Tenant/CreateTenantAccount', req);
}

/**
 * 获取菜单组成的选项树（全部，不含租户菜单过滤）
 * @param onlyMenu
 * @param keyword
 */
export function getMenuOptions(onlyMenu: boolean, keyword?: string) {
  return httpClient.get<number, AppResponse<GetMenuOptionsResponse>>('/admin-api/Tenant/MenuOptions', {
    params: {
      onlyMenu: onlyMenu,
      keyword: keyword,
    },
  });
}

export interface TenantAccountInfo {
  roleName: string;
  userName: string;
  password: string;
}

export interface CreateTenantAccountRequest {
  tenantId: string;
}

export interface AssignTenantMenuRequest {
  tenantId: string;
  menuIds: string[] | null;
}

export interface AddOrUpdateTenantRequest {
  name: string;
  tenantId?: string;
  remark?: string;
  domain: string;
  isEnabled: boolean;
}

export interface TenantItem {
  name: string;
  tenantId: string;
  remark?: string;
  domain?: string;
  lastModificationTime: string;
  isEnabled: boolean;
}

export interface GetTenantListRequest extends PageSearch {
  keyword?: string;
}

export interface TenantDetails extends TenantItem {
  menuIds: string[] | null
}