import httpClient from '@/utils/httpClient';
import type { AppResponse } from '@/types/api';

/**
 * 新增部门
 * @param dto
 */
export function addDept(dto: DeptDto) {
  return httpClient.post<DeptDto, AppResponse<boolean>>('/api/dept/add', dto);
}

/**
 * 部门树形列表
 * @param dto
 */
export function getDeptList(dto: DeptQueryDto) {
  return httpClient.get<DeptQueryDto, AppResponse<DeptListDto[]>>('/api/dept/list', { params: dto });
}

/**
 * 修改部门
 * @param dto
 */
export function updateDept(dto: DeptDto) {
  return httpClient.put<DeptDto, AppResponse<boolean>>('/api/dept/update', dto);
}

/**
 * 删除部门
 * @param id 部门ID
 */
export function deleteDept(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>('/api/dept/delete/' + id);
}

/**
 * 获取部门简单信息
 * @param keyword 部门名称/编码
 * @returns
 */
export function getDeptSimpleInfos(keyword?: string) {
  return httpClient.get<string, AppResponse<DeptSimpleInfoDto[]>>('/api/dept/getDeptSimpleInfos', { params: { keyword } });
}

export interface DeptDto {
  id?: string | null;
  name: string;
  code: string;
  sort: number;
  description?: string | null;
  status: number;
  curatorId?: string | null;
  email?: string | null;
  phone?: string | null;
  parentId?: string | null;
}

export interface DeptQueryDto {
  id?: string | null;
  code?: string | null;
  name?: string | null;
  status?: number;
}

export interface DeptListDto {
  id: string;
  code: string;
  name: string;
  sort: number;
  description: string | null;
  status: number;
  curatorId: string | null;
  curatorName: string | null;
  email: string | null;
  phone: string | null;
  parentId: string | null;
  children?: DeptListDto[];
}

export interface DeptSimpleInfoDto {
  id: string;
  name: string;
  code: string;
}
