import httpClient from '@/utils/httpClient';
import type { AppResponse, PageSearch, PagedResult } from '@/types/api';

/**
 * 新增字典数据
 */
export function addDictData(dto: DictDataDto) {
  return httpClient.post<DictDataDto, AppResponse<boolean>>('/admin-api/DictData/Add', dto);
}

/**
 * 字典数据分页列表
 * @param dto
 * @returns
 */
export function getDictDataList(dto: DictDataQueryDto) {
  return httpClient.get<DictDataQueryDto, AppResponse<PagedResult<DictDataListDto>>>('/admin-api/DictData/list', {
    params: dto,
  });
}

/**
 * 修改字典数据
 */
export function updateDictData(dto: DictDataDto) {
  return httpClient.put<DictDataDto, AppResponse<boolean>>('/admin-api/DictData/Update', dto);
}

/**
 * 删除字典数据
 * @param ids
 * @returns
 */
export function deleteDictData(ids: string[]) {
  return httpClient.delete<string[], AppResponse<boolean>>('/admin-api/DictData/Delete', {
    data: ids,
  });
}

export interface DictDataDto {
  id?: string | null;
  values: string;
  label: string;
  dictType: string;
  remark?: string | null;
  sort: number;
  isEnabled: boolean;
}

export interface DictDataListDto {
  id?: string;
  values: string;
  label: string;
  dictType: string;
  remark?: string | null;
  sort: number;
  isEnabled: boolean;
}

export interface DictDataQueryDto extends PageSearch {
  key?: string | null;
  label?: string | null;
  dictType?: string | null;
}
