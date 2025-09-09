import dayjs from 'dayjs';
import type { FrontendMenu } from '@/api/auth';
import { refreshToken } from '@/api/auth.ts';
import { makeAutoObservable, runInAction } from 'mobx';

export type TokenInfo = {
  sessionId: string;
  accessToken: string;
  refreshToken: string;
  expiredTime: Date;
};

export type UserAuthInfo = {
  userId: string;
  userName: string;
  nickName?: string;
  avatar: string;
  sex: number;
  permissions: string[];
  menus: FrontendMenu[];
  phone?: string | null;
};

const USER_TOKEN_KEY = 'user_token';
const USER_INFO_KEY = 'user_info';
const MENU_LIST_KEY = 'menu_list';

class UserStore {
  constructor() {
    makeAutoObservable(this, {}, { autoBind: true });
    this.initState();
  }

  userInfo: UserAuthInfo | null = null;
  menuList: FrontendMenu[] = [];
  private readonly CHECK_INTERVAL = 5 * 60 * 1000;
  private timer: NodeJS.Timeout | null = null;

  private initState() {
    const userInfoStorage = localStorage.getItem(USER_INFO_KEY);
    if (userInfoStorage) {
      this.userInfo = JSON.parse(userInfoStorage);
    }
    const menuListStorage = localStorage.getItem(MENU_LIST_KEY);
    if (menuListStorage) {
      this.menuList = JSON.parse(menuListStorage);
    }
  }

  get token(): TokenInfo | null {
    const tokenStorage = localStorage.getItem(USER_TOKEN_KEY);
    if (tokenStorage) {
      return JSON.parse(tokenStorage);
    }
    return null;
  }

  /**
   * 是否登录
   */
  isAuthenticated(): boolean {
    const tokenInfo = this.token;
    if (!tokenInfo || !tokenInfo.accessToken) return false;
    return dayjs(tokenInfo.expiredTime).isAfter(new Date());
  }

  /**
   * 设置token
   */
  setToken(token: TokenInfo) {
    localStorage.setItem(USER_TOKEN_KEY, JSON.stringify(token));
  }

  /**
   * 刷新token
   * @param accessToken
   * @param refreshToken
   * @param expiredTime
   */
  refreshToken(accessToken: string, refreshToken: string, expiredTime: Date) {
    const tokenInfo = this.token;
    if (tokenInfo) {
      tokenInfo.accessToken = accessToken;
      tokenInfo.refreshToken = refreshToken;
      tokenInfo.expiredTime = expiredTime;
      this.setToken(tokenInfo);
    }
  }

  setUserInfo(userInfo: UserAuthInfo) {
    if (userInfo) {
      const _menuList = this.flattenTreeDFS(userInfo.menus);
      localStorage.setItem(USER_INFO_KEY, JSON.stringify(userInfo));
      localStorage.setItem(MENU_LIST_KEY, JSON.stringify(_menuList));
      runInAction(() => {
        this.userInfo = userInfo;
        this.menuList = _menuList;
      });
    }
  }

  logout() {
    localStorage.removeItem(USER_INFO_KEY);
    localStorage.removeItem(USER_TOKEN_KEY);
    localStorage.removeItem(MENU_LIST_KEY);
    runInAction(() => {
      this.userInfo = null;
      this.menuList = [];
    });
  }

  flattenTreeDFS(menus: FrontendMenu[]): FrontendMenu[] {
    const result: FrontendMenu[] = [];

    function traverse(node: FrontendMenu) {
      if (!node) return;

      result.push(node); // 前序遍历
      // 遍历所有子节点
      node.children?.forEach((child) => traverse(child));
    }

    menus.forEach((item) => {
      traverse(item);
    });
    return result;
  }

  startTokenChecker() {
    this.stopTokenChecker();
    this.scheduleNextTokenCheck();
  }

  stopTokenChecker() {
    if (this.timer) {
      clearTimeout(this.timer);
      this.timer = null;
    }
  }

  private scheduleNextTokenCheck() {
    this.timer = setTimeout(() => {
      this.checkTokenIsExpired().finally(() => {
        this.scheduleNextTokenCheck();
      });
    }, this.CHECK_INTERVAL);
  }

  private async checkTokenIsExpired() {
    const token = this.token?.accessToken;
    const expired = this.token?.expiredTime;

    if (token && expired) {
      //过期时间小于10分钟进行刷新token
      const isAboutToExpire = dayjs(expired).subtract(10, 'minute').isBefore(new Date());
      if (isAboutToExpire) {
        const refreshTokenValue = this.token?.refreshToken;
        if (refreshTokenValue) {
          const refreshTokenRes = await refreshToken(refreshTokenValue);
          if (refreshTokenRes.data) {
            const refreshTokenData = refreshTokenRes.data;
            this.refreshToken(
              refreshTokenData.accessToken,
              refreshTokenData.refreshToken,
              refreshTokenData.expiredTime,
            );
          }
        }
      }
    }
  }
}

export default new UserStore();
