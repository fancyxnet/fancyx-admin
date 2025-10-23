import httpClient from '@/utils/httpClient.ts';
import type { AppResponse } from '@/types/api';

/**
 * 文件上传
 * @param file
 */
export function uploadFile(file: File) {
  const formData = new FormData();
  formData.append('file', file);
  return httpClient.post<File, AppResponse<string>>('/admin-api/Oss/Upload', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
}

/**
 * 删除文件
 * @param key
 */
export function deleteFile(key: string) {
  return httpClient.delete<string, AppResponse<boolean>>('/admin-api/Oss/Delete', {
    params: { key },
  });
}
