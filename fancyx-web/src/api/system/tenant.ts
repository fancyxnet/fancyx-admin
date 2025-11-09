import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';
import type { MenuOptionResultDto } from './menu';

/**
 * 新增租户
 * @param dto
 */
export function addTenant(dto: TenantDto) {
  return httpClient.post<TenantDto, AppResponse<boolean>>('/admin-api/Tenant/Add', dto);
}

/**
 * 租户分页列表
 * @param dto
 */
export function getTenantList(dto: TenantQueryDto) {
  return httpClient.get<TenantQueryDto, AppResponse<PagedResult<TenantListDto>>>('/admin-api/Tenant/List', {
    params: dto,
  });
}

/**
 * 修改租户
 * @param dto
 */
export function updateTenant(dto: TenantDto) {
  return httpClient.put<TenantDto, AppResponse<boolean>>('/admin-api/Tenant/Update', dto);
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
 * @param dto
 */
export function assignTenantMenu(dto: AssignTenantMenuDto) {
  return httpClient.post<AssignTenantMenuDto, AppResponse<boolean>>('/admin-api/Tenant/AssignTenantMenu', dto);
}

/**
 * 租户已有菜单ID列表
 * @param dto
 */
export function getTenantMenuIds(id: string) {
  return httpClient.get<string, AppResponse<string[]>>('/admin-api/Tenant/GetTenantMenuIds/' + id);
}

/**
 * 初始管理员账号
 * @param dto
 * @returns
 */
export function createTenantAccount(dto: CreateTenantAccountDto) {
  return httpClient.post<CreateTenantAccountDto, AppResponse<TenantAccountInfoDto>>('/admin-api/Tenant/CreateTenantAccount', dto);
}

/**
 * 获取菜单组成的选项树（全部，不含租户菜单过滤）
 * @param onlyMenu
 * @param keyword
 */
export function getMenuOptions(onlyMenu: boolean, keyword?: string) {
  return httpClient.get<number, AppResponse<MenuOptionResultDto>>('/admin-api/Tenant/MenuOptions', {
    params: {
      onlyMenu: onlyMenu,
      keyword: keyword,
    },
  });
}

export interface TenantAccountInfoDto {
  roleName: string;
  userName: string;
  password: string;
}

export interface CreateTenantAccountDto {
  tenantId: string;
}

export interface AssignTenantMenuDto {
  tenantId: string;
  menuIds: string[] | null;
}

export interface TenantDto {
  name: string;
  tenantId?: string;
  remark?: string;
  domain: string;
  isEnabled: boolean;
}

export interface TenantListDto {
  name: string;
  tenantId: string;
  remark?: string;
  domain?: string;
  lastModificationTime: string;
  isEnabled: boolean;
}

export interface TenantQueryDto extends PageSearch {
  keyword?: string;
}
