import httpClient from '@/utils/httpClient';
import type { AppResponse } from '@/types/api';

/**
 * 新增菜单
 * @param req
 */
export function addMenu(req: AddOrUpdateMenuRequest) {
  return httpClient.post<AddOrUpdateMenuRequest, AppResponse<boolean>>('/admin-api/Menu/Add', req);
}

/**
 * 菜单树形列表
 * @param req
 */
export function getMenuList(req: GetMenuListRequest) {
  return httpClient.get<GetMenuListRequest, AppResponse<MenuItem[]>>('/admin-api/Menu/List', { params: req });
}

/**
 * 修改菜单
 * @param req
 */
export function updateMenu(req: AddOrUpdateMenuRequest) {
  return httpClient.put<AddOrUpdateMenuRequest, AppResponse<boolean>>('/admin-api/Menu/Update', req);
}

/**
 * 删除菜单
 * @param ids
 */
export function deleteMenu(ids: string[]) {
  return httpClient.delete<string[], AppResponse<boolean>>('/admin-api/Menu/Delete', {
    data: ids,
  });
}

/**
 * 获取菜单组成的选项树
 * @param onlyMenu
 * @param keyword
 */
export function getMenuOptions(onlyMenu: boolean, keyword?: string) {
  return httpClient.get<number, AppResponse<GetMenuOptionsResponse>>('/admin-api/Menu/MenuOptions', {
    params: {
      onlyMenu: onlyMenu,
      keyword: keyword,
    },
  });
}

export interface AddOrUpdateMenuRequest {
  id?: string | null;
  title: string;
  name: string;
  icon?: string | null;
  path: string | null;
  functionType: number;
  permission: string;
  parentId: string;
  sort: number;
  display: boolean;
  component: string;
  isExternal: boolean;
  keepAlive: boolean;
}

export interface GetMenuListRequest {
  title?: string | null;
  path?: string | null;
}

export interface GetMenuOptionsResponse {
  keys: string[];
  tree: MenuOptionTree[];
}

export interface MenuOptionTree {
  key: string;
  title?: string;
  extra?: never;
  children?: MenuOptionTree[];
}

export interface MenuItem {
  id: string;
  title: string;
  icon: string | null;
  path: string | null;
  menuType: number;
  permission: string;
  parentId: string;
  sort: number;
  display: boolean;
  component: string;
  children: MenuItem[];
  isExternal: boolean;
  keepAlive: boolean;
}
