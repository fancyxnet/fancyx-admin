// 主题颜色常量 - 与 src/styles/vars.scss 文件中的颜色变量保持同步

// ========================================= 主色调 =========================================

// 紫色主题颜色
export const primaryColor = '#7e57c2';
export const primaryHover = '#9575cd';
export const primaryActive = '#673ab7';
export const primaryLight = '#f5f3ff';
export const primaryLighter = '#ede7f6';

// ========================================= 状态颜色 =========================================

export const successColor = '#66bb6a';
export const successHover = '#81c784';
export const successActive = '#43a047';

export const warningColor = '#ffa726';
export const warningHover = '#ffb74d';
export const warningActive = '#f57c00';

export const errorColor = '#ff7043';
export const errorHover = '#ff8a65';
export const errorActive = '#ef5350';

export const infoColor = '#29b6f6';
export const infoHover = '#4fc3f7';
export const infoActive = '#0288d1';

// ========================================= 文本颜色 =========================================

export const textPrimary = '#262626';
export const textBase = '#4a4a4a';
export const textSecondary = '#8c8c8c';
export const textPlaceholder = '#bfbfbf';
export const textDisabled = '#d9d9d9';

// ========================================= 背景颜色 =========================================

export const bgBase = '#ffffff';
export const bgLight = '#fafafa';
export const bgLighter = '#f5f5f5';
export const bgElevated = '#ffffff';

// ========================================= 边框颜色 =========================================

export const borderColor = '#d9d9d9';
export const borderColorLight = '#f0f0f0';
export const borderColorLighter = '#f5f5f5';

export const dividerColor = '#f0f0f0';

// ========================================= 阴影颜色 =========================================

export const shadowSm = 'rgba(0, 0, 0, 0.03)';
export const shadow = 'rgba(0, 0, 0, 0.05)';
export const shadowMd = 'rgba(0, 0, 0, 0.08)';
export const shadowLg = 'rgba(0, 0, 0, 0.12)';
export const shadowPrimary = 'rgba(126, 87, 194, 0.3)';
export const shadowPrimaryHover = 'rgba(126, 87, 194, 0.4)';
export const shadowPrimaryActive = 'rgba(126, 87, 194, 0.3)';

// ========================================= 主题色板对象 =========================================

export const themeColors = {
  primary: primaryColor,
  primaryHover,
  primaryActive,
  primaryLight,
  primaryLighter,
  success: successColor,
  successHover,
  successActive,
  warning: warningColor,
  warningHover,
  warningActive,
  error: errorColor,
  errorHover,
  errorActive,
  info: infoColor,
  infoHover,
  infoActive,
  text: {
    primary: textPrimary,
    base: textBase,
    secondary: textSecondary,
    placeholder: textPlaceholder,
    disabled: textDisabled,
  },
  bg: {
    base: bgBase,
    light: bgLight,
    lighter: bgLighter,
    elevated: bgElevated,
  },
  border: {
    color: borderColor,
    colorLight: borderColorLight,
    colorLighter: borderColorLighter,
    divider: dividerColor,
  },
  shadow: {
    sm: shadowSm,
    base: shadow,
    md: shadowMd,
    lg: shadowLg,
    primary: shadowPrimary,
    primaryHover,
    primaryActive,
  },
};

// 注意：本文件中的颜色值应与 src/styles/vars.scss 文件中的颜色变量保持同步