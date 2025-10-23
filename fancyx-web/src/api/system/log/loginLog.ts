import httpClient from '@/utils/httpClient';
import type { AppResponse, PageSearch, PagedResult } from '@/types/api';

/**
 * 登录日志分页列表
 * @param dto
 */
export function getLoginLogList(dto: LoginLogQueryDto) {
  return httpClient.get<LoginLogQueryDto, AppResponse<PagedResult<LoginLogListDto>>>('/admin-api/LoginLog/GetLoginLogList', {
    params: dto,
  });
}

export interface LoginLogQueryDto extends PageSearch {
  userName?: string | null;
  status?: number;
  address?: string | null;
  os?: string | null;
}

export interface LoginLogListDto {
  id: number;
  userName: string;
  ip: string;
  address: string;
  os: string;
  browser?: string;
  isSuccess: boolean;
  operationMsg: string;
  creationTime: string;
}
