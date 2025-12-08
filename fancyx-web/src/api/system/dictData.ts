import httpClient from '@/utils/httpClient';
import type { AppResponse, PageSearch, PagedResult } from '@/types/api';

/**
 * 新增字典数据
 */
export function addDictData(req: AddOrUpdateDictDataRequest) {
  return httpClient.post<AddOrUpdateDictDataRequest, AppResponse<boolean>>('/admin-api/DictData/Add', req);
}

/**
 * 字典数据分页列表
 * @param req
 * @returns
 */
export function getDictDataList(req: GetDictDataListRequest) {
  return httpClient.get<GetDictDataListRequest, AppResponse<PagedResult<DictDataItem>>>('/admin-api/DictData/list', {
    params: req,
  });
}

/**
 * 修改字典数据
 */
export function updateDictData(req: AddOrUpdateDictDataRequest) {
  return httpClient.put<AddOrUpdateDictDataRequest, AppResponse<boolean>>('/admin-api/DictData/Update', req);
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

export interface AddOrUpdateDictDataRequest {
  id?: string | null;
  values: string;
  label: string;
  dictType: string;
  remark?: string | null;
  sort: number;
  isEnabled: boolean;
}

export interface DictDataItem {
  id?: string;
  values: string;
  label: string;
  dictType: string;
  remark?: string | null;
  sort: number;
  isEnabled: boolean;
}

export interface GetDictDataListRequest extends PageSearch {
  key?: string | null;
  label?: string | null;
  dictType?: string | null;
}
