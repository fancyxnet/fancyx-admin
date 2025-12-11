import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增通知
 * @param req
 */
export function addNotification(req: AddOrUpdateNotificationRequest) {
  return httpClient.post<AddOrUpdateNotificationRequest, AppResponse<boolean>>('/admin-api/Notification/add', req);
}

/**
 * 通知分页列表
 * @param req
 */
export function getNotificationList(req: GetNotificationListRequest) {
  return httpClient.get<GetNotificationListRequest, AppResponse<PagedResult<NotificationItem>>>('/admin-api/Notification/list', {
    params: req,
  });
}

/**
 * 修改通知
 * @param req
 */
export function updateNotification(req: AddOrUpdateNotificationRequest) {
  return httpClient.put<AddOrUpdateNotificationRequest, AppResponse<boolean>>('/admin-api/Notification/update', req);
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

/**
 * 通知详情
 * @param id 
 * @returns 
 */
export function getNotification(id: string) {
  return httpClient.get<string, AppResponse<NotificationItem>>(`/admin-api/Notification/${id}`)
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
