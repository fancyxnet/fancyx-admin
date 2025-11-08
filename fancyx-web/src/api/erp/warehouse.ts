import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增
 * @param dto
 */
export function addWarehouse(dto: Warehouse) {
  return httpClient.post<Warehouse, AppResponse<boolean>>('/erp-api/Warehouse/Add', dto);
}

/**
 * 分页列表
 * @param dto
 */
export function getWarehouseList(dto: WarehouseQueryDto) {
  return httpClient.get<WarehouseQueryDto, AppResponse<PagedResult<Warehouse>>>('/erp-api/Warehouse/List', { params: dto });
}

/**
 * 修改
 * @param dto
 */
export function updateWarehouse(dto: Warehouse) {
  return httpClient.put<Warehouse, AppResponse<boolean>>('/erp-api/Warehouse/Update', dto);
}

/**
 * 删除
 * @param id
 */
export function deleteWarehouse(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>('/erp-api/Warehouse/Delete/'+id);
}

export interface Warehouse {
  /**  */
  id: string;
  /** 编码 */
  code: string;
  /** 名称 */
  name: string;
  /** 备注 */
  remark: string | null;
  /** 是否启用 */
  isEnabled: boolean;
  /** 租户ID */
  tenantId: string | null;
  /**  */
  creatorId: string | null;
  /**  */
  creationTime: Date;
  /**  */
  lastModificationTime: Date | null;
  /**  */
  lastModifierId: string | null;
  /**  */
  isDeleted: number;
  /**  */
  deleterId: string | null;
  /**  */
  deletionTime: Date | null;
}

export interface WarehouseQueryDto extends PageSearch {

}
