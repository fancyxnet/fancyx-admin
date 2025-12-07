import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult } from '@/types/api';

/**
 * 标记已读
 * @param dto
 */
export function readed(ids: string[]) {
  return httpClient.put<string[], AppResponse<boolean>>('/admin-api/UserNotification/Readed', ids);
}

/**
 * 我的通知分页列表
 * @param dto
 */
export function getMyNotificationList(dto: GetMyNotificationListRequest) {
  return httpClient.get<GetMyNotificationListRequest, AppResponse<PagedResult<UserNotificationItem>>>(
    '/admin-api/UserNotification/MyNotificationList',
    {
      params: dto,
    },
  );
}

/**
 * 我的通知顶部导航信息
 * @param dto
 */
export function getMyNotificationNavbarInfo() {
  return httpClient.get<unknown, AppResponse<UserNotificationNavbarInfo>>(
    '/admin-api/UserNotification/MyNotificationNavbarInfo',
  );
}

export interface UserNotificationItem {
  id: string;
  title: string;
  content: string | null;
  isReaded: boolean;
  creationTime: string;
  readedTime: string;
}

export interface GetMyNotificationListRequest {
  title?: string;
  isReaded?: boolean;
}

export interface UserNotificationNavbarInfo {
  noReadedCount: number;
  items: UserNotificationNavbarItem[];
}

export interface UserNotificationNavbarItem {
  id: string;
  title?: string;
  content: string | null;
  isReaded: boolean;
  creationTime: string;
}
