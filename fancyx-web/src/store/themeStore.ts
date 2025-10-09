import { createSlice } from '@reduxjs/toolkit';
import type { ThemeType } from '@/theme';

export const themeSlice = createSlice({
  name: 'themeSettings',
  initialState: {
    collapsed: false,
    size: 'middle',
    currentTheme: 'default' as ThemeType,
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
  },
});

export default themeSlice.reducer;
export const { toggleCollapsed, setSize, setTheme } = themeSlice.actions;

export const selectCollapsed = (state: { themeSettings: { collapsed: any } }) => state.themeSettings.collapsed;
export const selectSize = (state: { themeSettings: { size: any } }) => state.themeSettings.size;
export const selectTheme = (state: { themeSettings: { currentTheme: any } }) => state.themeSettings.currentTheme;
