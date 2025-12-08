import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增配置
 * @param req
 */
export function addConfig(req: AddOrUpdateConfigRequest) {
  return httpClient.post<AddOrUpdateConfigRequest, AppResponse<boolean>>('/admin-api/Config/Add', req);
}

/**
 * 配置分页列表
 * @param req
 */
export function getConfigList(req: GetConfigListRequest) {
  return httpClient.get<GetConfigListRequest, AppResponse<PagedResult<ConfigItem>>>('/admin-api/Config/List', { params: req });
}

/**
 * 修改配置
 * @param req
 */
export function updateConfig(req: AddOrUpdateConfigRequest) {
  return httpClient.put<AddOrUpdateConfigRequest, AppResponse<boolean>>('/admin-api/Config/Update', req);
}

/**
 * 删除配置
 * @param id
 */
export function deleteConfig(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>(`/admin-api/Config/Delete/${id}`);
}

export interface AddOrUpdateConfigRequest {
  id?: string;
  name: string;
  key: string;
  value: string;
  groupKey?: string;
  remark?: string;
}

export interface ConfigItem {
  id: string;
  name: string;
  key: string;
  value: string;
  groupKey?: string;
  remark?: string;
  creationTime: string;
  lastModificationTime: string;
}

export interface GetConfigListRequest extends PageSearch {
  key?: string;
}
