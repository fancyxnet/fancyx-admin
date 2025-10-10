import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增配置
 * @param dto
 */
export function addConfig(dto: ConfigDto) {
  return httpClient.post<ConfigDto, AppResponse<boolean>>('/admin-api/config/add', dto);
}

/**
 * 配置分页列表
 * @param dto
 */
export function getConfigList(dto: ConfigQueryDto) {
  return httpClient.get<ConfigQueryDto, AppResponse<PagedResult<ConfigListDto>>>('/admin-api/config/list', { params: dto });
}

/**
 * 修改配置
 * @param dto
 */
export function updateConfig(dto: ConfigDto) {
  return httpClient.put<ConfigDto, AppResponse<boolean>>('/admin-api/config/update', dto);
}

/**
 * 删除配置
 * @param id
 */
export function deleteConfig(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>(`/api/config/delete/${id}`);
}

export interface ConfigDto {
  id?: string;
  name: string;
  key: string;
  value: string;
  groupKey?: string;
  remark?: string;
}

export interface ConfigListDto {
  id: string;
  name: string;
  key: string;
  value: string;
  groupKey?: string;
  remark?: string;
  creationTime: string;
  lastModificationTime: string;
}

export interface ConfigQueryDto extends PageSearch {
  key?: string;
}
