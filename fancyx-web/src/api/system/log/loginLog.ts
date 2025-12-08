import httpClient from '@/utils/httpClient';
import type { AppResponse, PageSearch, PagedResult } from '@/types/api';

/**
 * 登录日志分页列表
 * @param req
 */
export function getLoginLogList(req: GetLoginLogListRequest) {
  return httpClient.get<GetLoginLogListRequest, AppResponse<PagedResult<LoginLogItem>>>('/admin-api/LoginLog/GetLoginLogList', {
    params: req,
  });
}

export interface GetLoginLogListRequest extends PageSearch {
  userName?: string | null;
  status?: number;
  address?: string | null;
  os?: string | null;
}

export interface LoginLogItem {
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
