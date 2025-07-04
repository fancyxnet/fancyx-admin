import dayjs from 'dayjs';
import type { FrontendMenu } from '@/api/auth';

export type TokenInfo = {
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
};

const USER_TOKEN_KEY = 'user_token';
const USER_INFO_KEY = 'user_info';
const MENU_LIST_KEY = 'menu_list';

const UserStore = {
  /**
   * 是否登录
   */
  isAuthenticated(): boolean {
    const tokenInfo = this.token;
    if (!tokenInfo || !tokenInfo.accessToken) return false;
    return dayjs(tokenInfo.expiredTime).isAfter(new Date());
  },

  /**
   * 设置token
   */
  setToken(token: TokenInfo) {
    localStorage.setItem(USER_TOKEN_KEY, JSON.stringify(token));
  },

  get token(): TokenInfo | null {
    const item = localStorage.getItem(USER_TOKEN_KEY);
    if (item) {
      return JSON.parse(item);
    }
    return null;
  },

  setUserInfo(userInfo: UserAuthInfo) {
    localStorage.setItem(USER_INFO_KEY, JSON.stringify(userInfo));
    localStorage.setItem(MENU_LIST_KEY, JSON.stringify(this.flattenTreeDFS(userInfo.menus)));
  },

  get userInfo(): UserAuthInfo | null {
    const item = localStorage.getItem(USER_INFO_KEY);
    if (item) {
      return JSON.parse(item);
    }
    return null;
  },

  logout() {
    localStorage.removeItem(USER_INFO_KEY);
    localStorage.removeItem(USER_TOKEN_KEY);
    localStorage.removeItem(MENU_LIST_KEY);
  },

  get menuList(): FrontendMenu[] {
    const item = localStorage.getItem(MENU_LIST_KEY);
    if (item) {
      return JSON.parse(item);
    }
    return [];
  },

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
  },
};

export default UserStore;
