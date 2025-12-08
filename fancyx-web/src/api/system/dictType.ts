import httpClient from '@/utils/httpClient';
import type { AppResponse, PageSearch, AppOption, PagedResult } from '@/types/api';

/**
 * 新增字典类型
 * @param req
 */
export function addDictType(req: AddOrUpdateDictTypeRequest) {
  return httpClient.post<AddOrUpdateDictTypeRequest, AppResponse<boolean>>('/admin-api/DictType/AddDictType', req);
}

/**
 * 分页查询字典类型列表
 */
export function getDictTypeList(req: GetDictTypeListRequest) {
  return httpClient.get<GetDictTypeListRequest, AppResponse<PagedResult<DictTypeItem>>>(
    '/admin-api/dictType/getDictTypeList',
    { params: req },
  );
}

/**
 * 修改字典类型
 * @param req
 */
export function updateDictType(req: AddOrUpdateDictTypeRequest) {
  return httpClient.put<AddOrUpdateDictTypeRequest, AppResponse<boolean>>('/admin-api/DictType/UpdateDictType', req);
}

/**
 * 删除字典类型
 * @param dictType
 */
export function deleteDictType(dictType: string) {
  return httpClient.delete<string, AppResponse<boolean>>('/admin-api/DictType/DeleteDictType/' + dictType);
}

/**
 * 批量删除字典类型
 * @param ids
 */
export function deleteDictTypes(ids: string[]) {
  return httpClient.delete<string[], AppResponse<boolean>>('/admin-api/DictType/DeleteDictTypes', {
    data: ids,
  });
}

/**
 * 字典选项
 * @param type 字典类型
 * @returns
 */
export function getDictDataOptions(type: string) {
  return httpClient.get<string, AppResponse<AppOption[]>>('/admin-api/dictType/getDictDataOptions?type=' + type);
}

export interface AddOrUpdateDictTypeRequest {
  name: string;
  id?: string | null;
  isEnabled: boolean;
  dictType: string;
  remark?: string | null;
}

export interface GetDictTypeListRequest extends PageSearch {
  name?: string | null;
  dictType?: string | null;
}

export interface DictTypeItem {
  name: string;
  id: string;
  isEnabled: boolean;
  dictType?: string;
  remark?: string;
}
