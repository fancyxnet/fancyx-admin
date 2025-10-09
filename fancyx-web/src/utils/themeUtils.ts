import type { ThemeConfig } from 'antd';
import type { ThemeType } from '../theme';
import { themeMap, getThemeConfig } from '../theme';

// 主题键名（用于 localStorage 存储）
const THEME_STORAGE_KEY = 'fancyx-admin-theme';
// 自定义主题颜色键名
const CUSTOM_COLOR_STORAGE_KEY = 'fancyx-admin-custom-color';

/**
 * 保存主题到本地存储
 * @param themeType 主题类型
 */
export const saveThemeToStorage = (themeType: ThemeType): void => {
  try {
    localStorage.setItem(THEME_STORAGE_KEY, themeType);
  } catch (error) {
    console.error('Failed to save theme to localStorage:', error);
  }
};

/**
 * 从本地存储加载主题
 * @returns 主题类型，如果没有保存则返回默认主题
 */
export const loadThemeFromStorage = (): ThemeType => {
  try {
    const savedTheme = localStorage.getItem(THEME_STORAGE_KEY) as ThemeType;
    return savedTheme || 'default';
  } catch (error) {
    console.error('Failed to load theme from localStorage:', error);
    return 'default';
  }
};

/**
 * 获取主题配置
 * @param themeType 主题类型
 * @returns 主题配置对象
 */
export const getTheme = (themeType?: ThemeType): ThemeConfig => {
  return getThemeConfig(themeType || loadThemeFromStorage());
};

/**
 * 切换主题
 * @param themeType 新的主题类型
 * @param callback 切换主题后的回调函数
 */
export const switchTheme = (themeType: ThemeType, callback?: (theme: ThemeConfig) => void): void => {
  saveThemeToStorage(themeType);
  const themeConfig = getTheme(themeType);
  
  // 更新CSS变量（如果需要）
  updateCssVariables(themeType);
  
  // 执行回调
  if (callback) {
    callback(themeConfig);
  }
};

/**
 * 更新CSS变量
 * @param themeType 主题类型
 */
export const updateCssVariables = (themeType: ThemeType): void => {
  const root = document.documentElement;
  
  const theme = themeMap[themeType];
  if (!theme) return;
  
  const { token } = theme.config;
  
  // 设置CSS变量
  if (token) {
    // 主要颜色变量
    if (token.colorPrimary) {
      root.style.setProperty('--primary-color', token.colorPrimary);
      // 为react-contexify设置背景色变量
      if (token.colorPrimaryHover) {
        root.style.setProperty('--contexify-activeItem-bgColor', token.colorPrimaryHover);
      }
    }
    if (token.colorSuccess) {
      root.style.setProperty('--success-color', token.colorSuccess);
    }
    if (token.colorWarning) {
      root.style.setProperty('--warning-color', token.colorWarning);
    }
    if (token.colorError) {
      root.style.setProperty('--error-color', token.colorError);
    }
    if (token.colorInfo) {
      root.style.setProperty('--info-color', token.colorInfo);
    }
    
    // 文本颜色变量
      if ('colorTextPrimary' in token && token.colorTextPrimary) {
        root.style.setProperty('--text-primary', token.colorTextPrimary as string);
      }
      if (token.colorTextBase) {
        root.style.setProperty('--text-base', token.colorTextBase);
      }
      if (token.colorTextSecondary) {
        root.style.setProperty('--text-secondary', token.colorTextSecondary);
      }
    
    // 背景颜色变量
    if (token.colorBgBase) {
      root.style.setProperty('--bg-base', token.colorBgBase);
    }
    if (token.colorBgElevated) {
      root.style.setProperty('--bg-elevated', token.colorBgElevated);
    }
  }
};

/**
 * 应用自定义主题
 */
export const applyCustomTheme = (): void => {
  const root = document.documentElement;
  
  // 从 localStorage 获取自定义颜色
  const customColor = loadCustomThemeColor();
  
  if (customColor) {
    // 设置CSS变量 - 确保这些变量与Ant Design主题系统兼容
    root.style.setProperty('--primary-color', customColor);
    root.style.setProperty('--ant-primary-color', customColor);
    
    // 从颜色代码中提取RGB值
    const hexToRgb = (hex: string): [number, number, number] => {
      // 移除#符号
      const r = parseInt(hex.slice(1, 3), 16);
      const g = parseInt(hex.slice(3, 5), 16);
      const b = parseInt(hex.slice(5, 7), 16);
      return [r, g, b];
    };
    
    // 简单的颜色处理函数
    const [r, g, b] = hexToRgb(customColor);
    
    // 计算悬停颜色（稍微亮一点）
    const hoverColor = `rgb(${Math.min(r + 20, 255)}, ${Math.min(g + 20, 255)}, ${Math.min(b + 20, 255)})`;
    root.style.setProperty('--primary-color-hover', hoverColor);
    root.style.setProperty('--contexify-activeItem-bgColor', hoverColor);
    root.style.setProperty('--ant-primary-color-hover', hoverColor);
    
    // 计算激活颜色（稍微暗一点）
    const activeColor = `rgb(${Math.max(r - 20, 0)}, ${Math.max(g - 20, 0)}, ${Math.max(b - 20, 0)})`;
    root.style.setProperty('--primary-color-active', activeColor);
    root.style.setProperty('--ant-primary-color-active', activeColor);
    
    // 计算浅色（增加透明度）
    const lightColor = `rgba(${r}, ${g}, ${b}, 0.1)`;
    root.style.setProperty('--primary-color-light', lightColor);
    root.style.setProperty('--ant-primary-5', lightColor);
    
    // 设置轮廓颜色
    root.style.setProperty('--ant-primary-color-outline', `rgba(${r}, ${g}, ${b}, 0.2)`);
  } else {
    // 如果没有自定义颜色，使用默认主题配置
    switchTheme('default');
  }
  
  // 为了确保更改生效，触发一次重绘
  const body = document.body;
  const display = body.style.display;
  body.style.display = 'none';
  void body.offsetHeight; // 触发重绘
  body.style.display = display;
};

/**
 * 保存自定义主题颜色
 * @param color 主色调
 */
export const saveCustomThemeColor = (color: string): void => {
  try {
    localStorage.setItem(CUSTOM_COLOR_STORAGE_KEY, color);
  } catch (error) {
    console.error('Failed to save custom theme color to localStorage:', error);
  }
};

/**
 * 加载自定义主题颜色
 * @returns 自定义主色调，如果没有保存则返回null
 */
export const loadCustomThemeColor = (): string | null => {
  try {
    return localStorage.getItem(CUSTOM_COLOR_STORAGE_KEY);
  } catch (error) {
    console.error('Failed to load custom theme color from localStorage:', error);
    return null;
  }
};

/**
 * 初始化主题
 * @returns 初始主题配置
 */
export const initTheme = (): ThemeConfig => {
  const themeType = loadThemeFromStorage();
  updateCssVariables(themeType);
  return getTheme(themeType);
};

/**
 * 获取所有可用主题列表
 * @returns 主题列表
 */
export const getAvailableThemes = () => {
  return Object.values(themeMap);
};

/**
 * 检测是否支持主题切换
 * @returns 是否支持主题切换
 */
export const isThemeSupported = (): boolean => {
  try {
    // 测试 localStorage 是否可用
    const testKey = '__theme_test__';
    localStorage.setItem(testKey, testKey);
    localStorage.removeItem(testKey);
    return true;
  } catch (e) {
    return false;
  }
};