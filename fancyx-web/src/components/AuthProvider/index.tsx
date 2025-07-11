import React, { createContext, useContext } from 'react';
import UserStore from '@/store/userStore.ts';
import { getUserAuth, login, type LoginDto } from '@/api/auth.ts';
import { clearTabs } from '@/store/tabStore.ts';
import { useDispatch } from 'react-redux';

export interface AuthProviderType {
  pwdLogin?: (values: LoginDto) => Promise<void>;
  clearToken: () => void;
  refreshUserAuthInfo?: () => Promise<void>;
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
      };
      UserStore.setUserInfo(_authInfo);
    }
  };

  return (
    <AuthContext.Provider
      value={{
        pwdLogin,
        clearToken,
        refreshUserAuthInfo: setOrRefreshUserAuthInfo,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuthProvider = () => useContext(AuthContext);
