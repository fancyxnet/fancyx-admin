import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增通知
 * @param dto
 */
export function addNotification(dto: AddOrUpdateNotificationRequest) {
  return httpClient.post<AddOrUpdateNotificationRequest, AppResponse<boolean>>('/admin-api/Notification/add', dto);
}

/**
 * 通知分页列表
 * @param dto
 */
export function getNotificationList(dto: GetNotificationListRequest) {
  return httpClient.get<GetNotificationListRequest, AppResponse<PagedResult<NotificationItem>>>('/admin-api/Notification/list', {
    params: dto,
  });
}

/**
 * 修改通知
 * @param dto
 */
export function updateNotification(dto: AddOrUpdateNotificationRequest) {
  return httpClient.put<AddOrUpdateNotificationRequest, AppResponse<boolean>>('/admin-api/Notification/update', dto);
}

/**
 * 删除通知
 * @param ids
 */
export function deleteNotifications(ids: string[]) {
  return httpClient.delete<string, AppResponse<boolean>>(`/admin-api/Notification/BatchDelete`, {
    data: ids,
  });
}

export interface AddOrUpdateNotificationRequest {
  id?: string;
  title: string;
  content: string | null;
  userId: string;
}

export interface NotificationItem {
  id: string;
  title: string;
  content: string | null;
  userId: string;
  isReaded: boolean;
  creationTime: string;
  readedTime: string;
  nickName: string;
}

export interface GetNotificationListRequest extends PageSearch {
  keyword?: string;
  isReaded?: boolean;
}
