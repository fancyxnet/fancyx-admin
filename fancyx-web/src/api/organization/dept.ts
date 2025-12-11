import httpClient from '@/utils/httpClient';
import type { AppResponse } from '@/types/api';

/**
 * 新增部门
 * @param req
 */
export function addDept(req: AddOrUpdateDeptRequest) {
  return httpClient.post<AddOrUpdateDeptRequest, AppResponse<boolean>>('/admin-api/Dept/Add', req);
}

/**
 * 部门树形列表
 * @param req
 */
export function getDeptList(req: GetDeptListRequest) {
  return httpClient.get<GetDeptListRequest, AppResponse<DeptItem[]>>('/admin-api/Dept/List', { params: req });
}

/**
 * 修改部门
 * @param req
 */
export function updateDept(req: AddOrUpdateDeptRequest) {
  return httpClient.put<AddOrUpdateDeptRequest, AppResponse<boolean>>('/admin-api/Dept/Update', req);
}

/**
 * 删除部门
 * @param id 部门ID
 */
export function deleteDept(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>('/admin-api/Dept/Delete/' + id);
}

/**
 * 获取部门简单信息
 * @param keyword 部门名称/编码
 * @returns
 */
export function getDeptSimpleInfos(keyword?: string) {
  return httpClient.get<string, AppResponse<DeptSimpleInfo[]>>('/admin-api/Dept/GetDeptSimpleInfos', { params: { keyword } });
}

/**
 * 部门详情
 * @param id 
 * @returns 
 */
export function getDept(id: string) {
  return httpClient.get<string, AppResponse<DeptItem>>(`/admin-api/Dept/${id}`)
}

export interface AddOrUpdateDeptRequest {
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

export interface GetDeptListRequest {
  id?: string | null;
  code?: string | null;
  name?: string | null;
  status?: number;
}

export interface DeptItem {
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
  children?: DeptItem[];
}

export interface DeptSimpleInfo {
  id: string;
  name: string;
  code: string;
}
