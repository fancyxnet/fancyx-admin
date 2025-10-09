import { createSlice } from '@reduxjs/toolkit';
import type { ThemeType } from '@/theme';

export const themeSlice = createSlice({
  name: 'themeSettings',
  initialState: {
    collapsed: false,
    size: 'middle',
    currentTheme: 'default' as ThemeType,
    sidebarMode: 'light' as 'light' | 'dark', // 侧边栏主题模式：明色(light)或暗色(dark)
  },
  reducers: {
    toggleCollapsed: (state) => {
      state.collapsed = !state.collapsed;
    },
    setSize: (state, action) => {
      const sizeOptions = ['large', 'middle', 'small'];
      if (sizeOptions.some((h) => h === action.payload)) {
        state.size = action.payload;
      } else {
        console.error('全局尺寸设置错误');
      }
    },
    setTheme: (state, action) => {
      state.currentTheme = action.payload;
    },
    setSidebarMode: (state, action) => {
      const modeOptions = ['light', 'dark'];
      if (modeOptions.some((mode) => mode === action.payload)) {
        state.sidebarMode = action.payload;
      } else {
        console.error('侧边栏主题模式设置错误');
      }
    },
  },
});

export default themeSlice.reducer;
export const { toggleCollapsed, setSize, setTheme, setSidebarMode } = themeSlice.actions;

export const selectCollapsed = (state: { themeSettings: { collapsed: any } }) => state.themeSettings.collapsed;
export const selectSize = (state: { themeSettings: { size: any } }) => state.themeSettings.size;
export const selectTheme = (state: { themeSettings: { currentTheme: any } }) => state.themeSettings.currentTheme;
export const selectSidebarMode = (state: { themeSettings: { sidebarMode: any } }) => state.themeSettings.sidebarMode;
