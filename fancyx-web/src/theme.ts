import { theme } from 'antd';
import type { ThemeConfig } from 'antd';

// 主题类型定义
export type ThemeType = 'blue-light' | 'default' | 'custom';

export interface ThemeDefinition {
  name: ThemeType;
  config: ThemeConfig;
  displayName: string;
  description: string;
}

// 主题配置系统 - 所有颜色值与 src/styles/vars.scss 保持同步

// 基础主题配置生成函数
export const createBaseTheme = (
  primaryColor: string,
  primaryHover: string,
  primaryActive: string,
  primaryLight: string,
  algorithm: ThemeConfig['algorithm'] = theme.defaultAlgorithm,
): ThemeConfig => ({
  algorithm,
  token: {
    colorPrimary: primaryColor,
    colorPrimaryHover: primaryHover,
    colorPrimaryActive: primaryActive,
    colorLink: primaryColor,
    colorSuccess: '#66bb6a',
    colorWarning: '#ffa726',
    colorError: '#ff7043',
    colorInfo: '#29b6f6',
    colorBgBase: '#ffffff',
    colorBgElevated: '#ffffff',
    colorTextBase: '#4a4a4a',
    // 使用类型断言添加自定义属性
    ...({ colorTextPrimary: '#262626' } as any),
    colorTextSecondary: '#8c8c8c',
    colorBorder: '#d9d9d9',
    colorDivider: '#f0f0f0',
    borderRadius: 4,
    fontSize: 14,
  },
  components: {
    Layout: {
      siderBg: '#ffffff',
      headerBg: '#ffffff',
      bodyBg: '#fafafa',
    },
    Menu: {
      itemBg: '#ffffff',
      itemColor: '#4a4a4a',
      itemSelectedBg: primaryLight,
      itemSelectedColor: primaryColor,
      itemHoverBg: primaryLight,
      itemBorderRadius: 8,
    },
    Button: {
      colorPrimary: primaryColor,
      colorPrimaryHover: primaryHover,
      colorPrimaryActive: primaryActive,
    },
    Table: {
      headerBg: primaryLight,
      headerColor: '#4a4a4a',
      borderColor: '#ede7f6',
    },
    Input: {
      colorBgContainer: '#ffffff',
      colorBorder: '#d9d9d9',
    },
    Select: {
      colorBgContainer: '#ffffff',
      colorBorder: '#d9d9d9',
    },
    Card: {
      colorBgContainer: '#ffffff',
      colorBorder: '#f0f0f0',
    },
  },
});

// 紫色主题配置
export const PurpleLightTheme: ThemeDefinition = {
  name: 'default',
  config: createBaseTheme('#7E57C2', '#9575CD', '#673AB7', '#EDE7F6'),
  displayName: '紫色主题',
  description: '优雅的紫色主题，适合大多数应用场景',
};

// 蓝色主题配置
export const BlueLightTheme: ThemeDefinition = {
  name: 'blue-light',
  config: createBaseTheme('#1890ff', '#40a9ff', '#096dd9', '#e6f7ff'),
  displayName: '蓝色主题',
  description: '专业的蓝色主题，适合企业应用场景',
};

// 默认主题配置
export const DefaultTheme = PurpleLightTheme.config;

// 主题映射表
export const themeMap: Record<ThemeType, ThemeDefinition> = {
  'blue-light': BlueLightTheme,
  default: PurpleLightTheme,
  // 自定义主题（在运行时通过 localStorage 配置）
  custom: {
    name: 'custom',
    config: PurpleLightTheme.config, // 默认使用紫色主题的配置，实际使用时会通过 CSS 变量覆盖
    displayName: '自定义主题',
    description: '用户自定义的主题配色',
  },
};

// 获取主题配置
export const getThemeConfig = (themeType: ThemeType = 'default'): ThemeConfig => {
  return themeMap[themeType]?.config || DefaultTheme;
};

// 获取所有主题列表
export const getAllThemes = (): ThemeDefinition[] => {
  return Object.values(themeMap);
};

// 主题变量同步检查提示
// 注意：本文件中的颜色值应与 src/styles/vars.scss 文件中的颜色变量保持同步
// 如需新增主题，请同时更新 vars.scss 文件中的对应主题变量