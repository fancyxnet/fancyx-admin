import httpClient from '@/utils/httpClient';
import type { AppResponse, PageSearch, PagedResult, AppOption } from '@/types/api';

/**
 * 业务日志分页列表
 * @param dto
 */
export function getBusinessLogList(dto: BusinessLogQueryDto) {
  return httpClient.get<BusinessLogQueryDto, AppResponse<PagedResult<BusinessLogListDto>>>('/admin-api/businessLog/list', {
    params: dto,
  });
}

/**
 * 获取所有业务类型选项
 * @param type
 */
export function getBusinessTypeOptions(type?: string | null) {
  return httpClient.get<string, AppResponse<AppOption[]>>('/admin-api/businessLog/TypeOptions', {
    params: type,
  });
}

export interface BusinessLogQueryDto extends PageSearch {
  type?: string;
  subType?: string;
  content?: string;
}

export interface BusinessLogListDto {
  id: string;
  userName: string;
  type: string;
  subType: string;
  content: string;
  bizNo: string;
  browser: string;
  ip: string;
  creationTime: string;
}
