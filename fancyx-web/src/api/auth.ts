import httpClient from '@/utils/httpClient';
import type { AppResponse } from '@/types/api';

/**
 * 登录
 * @param dto
 */
export function login(dto: LoginDto) {
  return httpClient.post<LoginDto, AppResponse<LoginResultDto>>('/api/account/login', dto);
}

/**
 * 刷新token
 * @param refreshToken
 * @returns
 */
export function refreshToken(refreshToken: string) {
  return httpClient.post<string, AppResponse<TokenResultDto>>('/api/account/refreshToken?refreshToken=' + refreshToken);
}

/**
 * 修改个人基本信息
 * @param info
 */
export function updateInfo(info: PersonalInfoDto) {
  return httpClient.put<PersonalInfoDto, AppResponse<boolean>>('/api/account/updateInfo', info);
}

/**
 * 修改个人密码
 * @param dto
 */
export function updatePwd(dto: UserPwdDto) {
  return httpClient.put<UserPwdDto, AppResponse<boolean>>('/api/account/updatePwd', dto);
}

/**
 * 注销
 */
export function signOut() {
  return httpClient.post<AppResponse<boolean>>('/api/account/signOut');
}

/**
 * 用户权限信息
 */
export function getUserAuth() {
  return httpClient.get<unknown, AppResponse<UserAuthInfoDto>>('/api/account/userAuth');
}

export interface LoginDto {
  userName: string;
  password: string;
}

interface TokenResultDto {
  accessToken: string;
  refreshToken: string;
  expiredTime: Date;
}

interface LoginResultDto extends TokenResultDto {
  sessionId: string;
  userId: string;
  userName: string;
}

export interface PersonalInfoDto {
  avatar?: string;
  nickName?: string;
  sex?: number;
}

export interface UserPwdDto {
  oldPwd: string;
  newPwd: string;
}

interface UserInfoDto {
  userId: string;
  userName: string;
  avatar: string;
  nickName: string;
  sex: number;
}

export interface FrontendMenu {
  id: string;
  title: string;
  icon: string | null;
  display: boolean;
  path: string;
  component: string | null;
  children: FrontendMenu[] | null;
  layerName: string;
  menuType: number;
}

interface UserAuthInfoDto {
  user: UserInfoDto;
  permissions: string[];
  menus: FrontendMenu[];
}
