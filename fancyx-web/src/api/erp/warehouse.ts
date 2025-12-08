import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增仓库
 * @param req
 */
export function addWarehouse(req: Warehouse) {
  return httpClient.post<Warehouse, AppResponse<boolean>>('/erp-api/Warehouse/Add', req);
}

/**
 * 仓库分页列表
 * @param req
 */
export function getWarehouseList(req: WarehouseQueryDto) {
  return httpClient.get<WarehouseQueryDto, AppResponse<PagedResult<Warehouse>>>('/erp-api/Warehouse/List', {
    params: req,
  });
}

/**
 * 仓库查询
 * @param req
 */
export function getWarehouse(id: string) {
  return httpClient.get<string, AppResponse<Warehouse>>('/erp-api/Warehouse/GetWarehouse/' + id);
}

/**
 * 修改仓库
 * @param req
 */
export function updateWarehouse(req: Warehouse) {
  return httpClient.put<Warehouse, AppResponse<boolean>>('/erp-api/Warehouse/Update', req);
}

/**
 * 删除仓库
 * @param id
 */
export function deleteWarehouse(id: string) {
  return httpClient.delete<string, AppResponse<boolean>>('/erp-api/Warehouse/Delete/' + id);
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
}

export interface WarehouseQueryDto extends PageSearch {
  name: string;
}
