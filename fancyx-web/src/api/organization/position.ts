import httpClient from '@/utils/httpClient';
import type { AppResponse, PagedResult, PageSearch, AppOptionTree } from '@/types/api';

/**
 * 新增职位
 * @param req
 */
export function addPosition(req: AddOrUpdatePositionRequest) {
  return httpClient.post<AddOrUpdatePositionRequest, AppResponse<boolean>>('/admin-api/Position/Add', req);
}

/**
 * 职位分页列表
 * @param req
 */
export function getPositionList(req: GetPositionListRequest) {
  return httpClient.get<GetPositionListRequest, AppResponse<PagedResult<PositionItem>>>('/admin-api/Position/List', {
    params: req,
  });
}

/**
 * 编辑职位
 * @param req
 */
export function updatePosition(req: AddOrUpdatePositionRequest) {
  return httpClient.put<AddOrUpdatePositionRequest, AppResponse<boolean>>('/admin-api/Position/Update', req);
}

/**
 * 删除职位
 * @param id
 */
export function deletePosition(id: string) {
  return httpClient.delete('/admin-api/Position/Delete/' + id);
}

/**
 * 职位分组+职位树
 */
export function getPositionOptions() {
  return httpClient.get<unknown, AppResponse<AppOptionTree[]>>('/admin-api/Position/Options');
}

export interface AddOrUpdatePositionRequest {
  id?: string | null;
  name: string;
  code: string;
  level: number;
  status: number;
  description?: string | null;
  groupId?: string | null;
}

export interface GetPositionListRequest extends PageSearch {
  keyword?: string | null;
  level?: string | null;
  status?: number | null;
  groupId?: number | null;
}

export interface PositionItem {
  id: string;
  code: string | null;
  name: string | null;
  level: number;
  status: number;
  description: string;
  groupId: string | null;
  layerName: string | null;
}
