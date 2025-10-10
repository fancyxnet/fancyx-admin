import httpClient from '@/utils/httpClient';
import type { AppResponse, PagedResult, PageSearch, AppOptionTree } from '@/types/api';

/**
 * 新增职位
 * @param dto
 */
export function addPosition(dto: PositionDto) {
  return httpClient.post<PositionDto, AppResponse<boolean>>('/admin-api/position/add', dto);
}

/**
 * 职位分页列表
 * @param dto
 */
export function getPositionList(dto: PositionQueryDto) {
  return httpClient.get<PositionQueryDto, AppResponse<PagedResult<PositionListDto>>>('/admin-api/position/list', {
    params: dto,
  });
}

/**
 * 编辑职位
 * @param dto
 */
export function updatePosition(dto: PositionDto) {
  return httpClient.put<PositionDto, AppResponse<boolean>>('/admin-api/position/update', dto);
}

/**
 * 删除职位
 * @param id
 */
export function deletePosition(id: string) {
  return httpClient.delete('/admin-api/position/delete/' + id);
}

/**
 * 职位分组+职位树
 */
export function getPositionOptions() {
  return httpClient.get<unknown, AppResponse<AppOptionTree[]>>('/admin-api/position/options');
}

export interface PositionDto {
  id?: string | null;
  name: string;
  code: string;
  level: number;
  status: number;
  description?: string | null;
  groupId?: string | null;
}

export interface PositionQueryDto extends PageSearch {
  keyword?: string | null;
  level?: string | null;
  status?: number | null;
  groupId?: number | null;
}

export interface PositionListDto {
  id: string;
  code: string | null;
  name: string | null;
  level: number;
  status: number;
  description: string;
  groupId: string | null;
  layerName: string | null;
}
