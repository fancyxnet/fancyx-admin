import httpClient from '@/utils/httpClient';
import type { AppResponse } from '@/types/api';

/**
 * 新增职位分组
 * @param dto
 */
export function addPositionGroup(dto: AddOrUpdatePositionGroupRequest) {
  return httpClient.post<AddOrUpdatePositionGroupRequest, AppResponse<boolean>>('/admin-api/PositionGroup/Add', dto);
}

/**
 * 职位分组分页列表
 * @param dto
 */
export function getPositionGroupList(dto?: GetPositionGroupListRequest) {
  return httpClient.get<GetPositionGroupListRequest, AppResponse<PositionGroupItem[]>>('/admin-api/PositionGroup/List', {
    params: dto,
  });
}

/**
 * 修改职位分组
 * @param dto
 */
export function updatePositionGroup(dto: AddOrUpdatePositionGroupRequest) {
  return httpClient.put<AddOrUpdatePositionGroupRequest, AppResponse<boolean>>('/admin-api/PositionGroup/Update', dto);
}

/**
 * 删除职位分组
 * @param id
 */
export function deletePositionGroup(id: string) {
  return httpClient.delete<AppResponse<boolean>>(`/admin-api/PositionGroup/Delete/${id}`);
}

export interface AddOrUpdatePositionGroupRequest {
  id?: string | null;
  parentId?: string | null;
  groupName: string;
  remark?: string | null;
  sort: number;
}

export interface GetPositionGroupListRequest {
  groupName?: string | null;
}

export interface PositionGroupItem {
  id: string;
  groupName: string;
  remark: string | null;
  parentId: string | null;
  sort: number;
  PositionGroupItem: AddOrUpdatePositionGroupRequest[];
}
