import httpClient from '@/utils/httpClient';
import type { AppResponse } from '@/types/api';

/**
 * 登录
 * @param req
 */
export function login(req: LoginRequest) {
  return httpClient.post<LoginRequest, AppResponse<LoginRespone>>('/admin-api/Account/Login', req);
}

/**
 * 短信登录
 * @param req
 */
export function smsLogin(req: SmsLoginRequest) {
  return httpClient.post<SmsLoginRequest, AppResponse<LoginRespone>>('/admin-api/Account/SmsLogin', req);
}

/**
 * 获取短信验证码
 * @param phone
 */
export function sendLoginSmsCode(phone: string) {
  return httpClient.post<string, AppResponse<string>>('/admin-api/Account/SendLoginSmsCode?phone=' + phone);
}

/**
 * 刷新token
 * @param refreshToken
 * @returns
 */
export function refreshToken(refreshToken: string) {
  return httpClient.post<string, AppResponse<TokenResponse>>('/admin-api/Account/RefreshToken?refreshToken=' + refreshToken);
}

/**
 * 修改个人基本信息
 * @param info
 */
export function updateInfo(info: UpdateUserInfoRequest) {
  return httpClient.put<UpdateUserInfoRequest, AppResponse<boolean>>('/admin-api/Account/UpdateInfo', info);
}

/**
 * 修改个人密码
 * @param req
 */
export function updatePwd(req: UpdateUserPwdRequest) {
  return httpClient.put<UpdateUserPwdRequest, AppResponse<boolean>>('/admin-api/Account/UpdatePwd', req);
}

/**
 * 注销
 */
export function signOut() {
  return httpClient.post<AppResponse<boolean>>('/admin-api/Account/SignOut');
}

/**
 * 用户权限信息
 */
export function getUserAuth() {
  return httpClient.get<unknown, AppResponse<GetUserAuthInfoResponse>>('/admin-api/Account/UserAuth');
}

export interface LoginRequest {
  userName: string;
  password: string;
}

interface TokenResponse {
  accessToken: string;
  refreshToken: string;
  expiredTime: Date;
}

interface LoginRespone extends TokenResponse {
  sessionId: string;
  userId: string;
  userName: string;
}

export interface UpdateUserInfoRequest {
  avatar?: string;
  nickName?: string;
  sex?: number;
  phone?: string;
}

export interface UpdateUserPwdRequest {
  oldPwd: string;
  newPwd: string;
}

interface CurrentUserInfo {
  userId: string;
  userName: string;
  avatar: string;
  nickName: string;
  sex: number;
  phone?: string | null;
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
  isExternal: boolean;
  keepAlive: boolean;
}

interface GetUserAuthInfoResponse {
  user: CurrentUserInfo;
  permissions: string[];
  menus: FrontendMenu[];
}

export interface SmsLoginRequest {
  phone: string;
  code: string;
}
