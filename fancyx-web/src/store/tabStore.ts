import { createSlice } from '@reduxjs/toolkit';
import { needDisplayRoutes } from '@/router/index'
import type { GlobalTabItem } from '@/types/global';

const homeTab: GlobalTabItem = { key: '/', label: '首页', closable: false };

export const tabSlice = createSlice({
  name: 'tab',
  initialState: {
    tabs: [homeTab],
    activeKey: '/',
  },
  reducers: {
    open: (state, action) => {
      let { name, path } = action.payload;
      if (!name) {
        const findStaticRoute = needDisplayRoutes.find(x => x.path === path);
        if (!findStaticRoute) {
          return;
        }
        name = findStaticRoute.title;
      }

      const exist = state.tabs.some((h) => h.key === path);
      //存在不添加，只设置活动标签key
      if (exist) {
        state.activeKey = path;
        return;
      }
      state.tabs.push({
        key: path,
        label: name ?? '未知',
        closable: true,
      });
      state.activeKey = path;
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
