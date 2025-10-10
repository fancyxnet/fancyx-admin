import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult } from '@/types/api';

/**
 * 标记已读
 * @param dto
 */
export function readed(ids: string[]) {
  return httpClient.put<string[], AppResponse<boolean>>('/admin-api/UserNotification/readed', ids);
}

/**
 * 我的通知分页列表
 * @param dto
 */
export function getMyNotificationList(dto: UserNotificationQueryDto) {
  return httpClient.get<UserNotificationQueryDto, AppResponse<PagedResult<MyNotificationListDto>>>(
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
  return httpClient.get<unknown, AppResponse<UserNotificationNavbarDto>>(
    '/admin-api/UserNotification/MyNotificationNavbarInfo',
  );
}

export interface MyNotificationListDto {
  id: string;
  title: string;
  content: string | null;
  isReaded: boolean;
  creationTime: string;
  readedTime: string;
}

export interface UserNotificationQueryDto {
  title?: string;
  isReaded?: boolean;
}

export interface UserNotificationNavbarDto {
  noReadedCount: number;
  items: UserNotificationNavbarItemDto[];
}

export interface UserNotificationNavbarItemDto {
  id: string;
  title?: string;
  content: string | null;
  isReaded: boolean;
  creationTime: string;
}
