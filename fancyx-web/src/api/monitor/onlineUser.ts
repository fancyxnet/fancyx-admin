import httpClient from '@/utils/httpClient';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 在线用户列表
 * @param dto
 */
export function getOnlineUsers(dto: OnlineUserSearchDto) {
  return httpClient.get<OnlineUserSearchDto, AppResponse<PagedResult<OnlineUserResultDto>>>(
    '/admin-api/OnlineUser/GetOnlineUserList',
    {
      params: dto,
    },
  );
}

/**
 * 注销当前会话
 * @param key
 */
export function onlineUserLogout(key: string) {
  return httpClient.post<string, AppResponse<boolean>>('/admin-api/OnlineUser/Logout?key=' + key);
}

export interface OnlineUserSearchDto extends PageSearch {
  userName?: string;
}

export interface OnlineUserResultDto {
  userId: string;
  userName: string;
  ip: string | null;
  address?: string;
  os: string | null;
  creationTime: string;
  sessionId: string;
}
