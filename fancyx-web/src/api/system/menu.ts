import httpClient from '@/utils/httpClient';
import type { AppResponse } from '@/types/api';

/**
 * 新增菜单
 * @param dto
 */
export function addMenu(dto: MenuDto) {
  return httpClient.post<MenuDto, AppResponse<boolean>>('/api/menu/add', dto);
}

/**
 * 菜单树形列表
 * @param dto
 */
export function getMenuList(dto: MenuQueryDto) {
  return httpClient.get<MenuQueryDto, AppResponse<MenuListDto[]>>('/api/menu/list', { params: dto });
}

/**
 * 修改菜单
 * @param dto
 */
export function updateMenu(dto: MenuDto) {
  return httpClient.put<MenuDto, AppResponse<boolean>>('/api/menu/update', dto);
}

/**
 * 删除菜单
 * @param ids
 */
export function deleteMenu(ids: string[]) {
  return httpClient.delete<string[], AppResponse<boolean>>('/api/menu/delete', {
    data: ids,
  });
}

/**
 * 获取菜单组成的选项树
 * @param onlyMenu
 * @param keyword
 */
export function getMenuOptions(onlyMenu: boolean, keyword?: string) {
  return httpClient.get<number, AppResponse<MenuOptionResultDto>>('/api/menu/menuOptions', {
    params: {
      onlyMenu: onlyMenu,
      keyword: keyword,
    },
  });
}

export interface MenuDto {
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
}

export interface MenuQueryDto {
  title?: string | null;
  path?: string | null;
}

export interface MenuOptionResultDto {
  keys: string[];
  tree: MenuOptionTreeDto[];
}

export interface MenuOptionTreeDto {
  key: string;
  title?: string;
  extra?: never;
  children?: MenuOptionTreeDto[];
}

export interface MenuListDto {
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
  children: MenuListDto[];
}
