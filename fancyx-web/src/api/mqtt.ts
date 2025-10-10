import httpClient from '@/utils/httpClient.ts';
import type { AppResponse } from '@/types/api';

/**
 * 获取MQTT访问令牌
 * @param code
 */
export function getMqttToken(code: string) {
  return httpClient.post<string, AppResponse<MqttToken>>('/admin-api/mqtt/getMqttToken?code=' + code);
}

export interface MqttToken {
  /** 过期时间戳，单位秒 */
  expired: number;
  /** mqtt访问令牌 */
  token: string;
}
