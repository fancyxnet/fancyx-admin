import { createSlice } from '@reduxjs/toolkit';
import UserStore from '@/store/userStore.ts';
import { StaticRoutes } from '@/utils/globalValue';
import { needDisplayRoutes } from '@/router/index'

const homeTab = { key: '/', label: '首页', closable: false };

export const tabSlice = createSlice({
  name: 'tab',
  initialState: {
    tabs: [homeTab],
    activeKey: '/',
  },
  reducers: {
    open: (state, action) => {
      const menus = UserStore.menuList;
      //外链特殊处理，截取external/后的路径
      if (action.payload.startsWith(StaticRoutes.External)) {
        action.payload = action.payload.replace(StaticRoutes.External, '');
      }
      let currentTitle = menus.find((h) => h.path === action.payload)?.title;
      if (!currentTitle) {
        const findStaticRoute = needDisplayRoutes.find(x => x.path === action.payload);
        if (!findStaticRoute) {
          return;
        }
        currentTitle = findStaticRoute.title;
      }

      const exist = state.tabs.some((h) => h.key === action.payload);
      //存在不添加，只设置活动标签key
      if (exist) {
        state.activeKey = action.payload;
        return;
      }
      state.tabs.push({
        key: action.payload,
        label: currentTitle,
        closable: true,
      });
      state.activeKey = action.payload;
    },
    remove: (state, action) => {
      //不能移除home页
      const key = action.payload.key as string;
      const change = action.payload.change as boolean;
      if (key === '' || key === '/') return;
      const index = state.tabs.findIndex((h) => h.key === key);
      if (index < 0) return;

      if (state.tabs.length > 0) {
        state.tabs.splice(index, 1);
        if (change) {
          state.activeKey = state.tabs[index - 1].key;
        }
      }
    },
    setActiveKey: (state, action) => {
      state.activeKey = action.payload;
    },
    clearTabs: (state) => {
      state.tabs = [homeTab];
      state.activeKey = '/';
    },
  },
});

export default tabSlice.reducer;
export const { open, remove, setActiveKey, clearTabs } = tabSlice.actions;

export const selectTabs = (state: { tab: { tabs: any } }) => state.tab.tabs;
export const selectActiveKey = (state: { tab: { activeKey: any } }) => state.tab.activeKey;
