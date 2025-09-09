import React, { createContext, useContext } from 'react';
import UserStore from '@/store/userStore.ts';
import { getUserAuth, login, type LoginDto, smsLogin, type SmsLoginDto } from '@/api/auth.ts';
import { clearTabs } from '@/store/tabStore.ts';
import { useDispatch } from 'react-redux';

export interface AuthProviderType {
  pwdLogin?: (values: LoginDto) => Promise<void>;
  smsLogin?: (values: SmsLoginDto) => Promise<void>;
  clearToken: () => void;
  refreshUserAuthInfo?: () => Promise<void>;
  hasPermission?: (permission: string | string[], mode?: string) => boolean;
}

const AuthContext = createContext<AuthProviderType>({
  clearToken: () => {},
});

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const dispatch = useDispatch();

  const pwdLogin = async (values: LoginDto) => {
    const res = await login(values);
    if (res.data) {
      UserStore.setToken({
        sessionId: res.data.sessionId,
        accessToken: res.data.accessToken,
        refreshToken: res.data.refreshToken,
        expiredTime: res.data.expiredTime,
      });
      await setOrRefreshUserAuthInfo();
    }
  };
  const _smsLogin = async (values: SmsLoginDto) => {
    const res = await smsLogin(values);
    if (res.data) {
      UserStore.setToken({
        sessionId: res.data.sessionId,
        accessToken: res.data.accessToken,
        refreshToken: res.data.refreshToken,
        expiredTime: res.data.expiredTime,
      });
      await setOrRefreshUserAuthInfo();
    }
  };

  const clearToken = () => {
    UserStore.logout();
    dispatch(clearTabs());
  };

  const setOrRefreshUserAuthInfo = async () => {
    if (UserStore.isAuthenticated()) {
      const { data: authInfo } = await getUserAuth();
      const _authInfo = {
        userId: authInfo.user.userId,
        userName: authInfo.user.userName,
        nickName: authInfo.user.nickName,
        avatar: authInfo.user.avatar,
        sex: authInfo.user.sex,
        menus: authInfo.menus,
        permissions: authInfo.permissions,
        phone: authInfo.user.phone,
      };
      UserStore.setUserInfo(_authInfo);
    }
  };
  const hasPermission = (permissions: string | string[], mode = 'every'): boolean => {
    const powers: string[] = UserStore.userInfo?.permissions ?? [];

    // 检查操作权限
    const permissionList: string[] = Array.isArray(permissions) ? permissions : [permissions];
    if (mode === 'every') {
      return permissionList.every((p) => powers.includes(p));
    }
    return permissionList.some((p) => powers.includes(p));
  };

  return (
    <AuthContext.Provider
      value={{
        pwdLogin,
        smsLogin: _smsLogin,
        clearToken,
        refreshUserAuthInfo: setOrRefreshUserAuthInfo,
        hasPermission: hasPermission,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuthProvider = () => useContext(AuthContext);
