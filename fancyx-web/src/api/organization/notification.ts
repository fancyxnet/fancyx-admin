import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增通知
 * @param dto
 */
export function addNotification(dto: NotificationDto) {
  return httpClient.post<NotificationDto, AppResponse<boolean>>('/api/notification/add', dto);
}

/**
 * 通知分页列表
 * @param dto
 */
export function getNotificationList(dto: NotificationQueryDto) {
  return httpClient.get<NotificationQueryDto, AppResponse<PagedResult<NotificationListDto>>>('/api/notification/list', {
    params: dto,
  });
}

/**
 * 修改通知
 * @param dto
 */
export function updateNotification(dto: NotificationDto) {
  return httpClient.put<NotificationDto, AppResponse<boolean>>('/api/notification/update', dto);
}

/**
 * 删除通知
 * @param ids
 */
export function deleteNotifications(ids: string[]) {
  return httpClient.delete<string, AppResponse<boolean>>(`/api/notification/BatchDelete`, {
    data: ids,
  });
}

export interface NotificationDto {
  id?: string;
  title: string;
  content: string | null;
  userId: string;
}

export interface NotificationListDto {
  id: string;
  title: string;
  content: string | null;
  userId: string;
  isReaded: boolean;
  creationTime: string;
  readedTime: string;
  nickName: string;
}

export interface NotificationQueryDto extends PageSearch {
  keyword?: string;
  isReaded?: boolean;
}
