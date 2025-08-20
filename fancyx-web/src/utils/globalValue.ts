/**
 * API响应错误码
 */
export const ErrorCode = {
  Success: '0',
};

/**
 * 菜单类型
 */
export const MenuType = {
  Folder: 1,
  Menu: 2,
  Button: 3,
};

/**
 * 正则表达式
 */
export const Patterns = {
  LoginPassword: /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d~`!@#$%^&*()_\-+={[}\]|\\:;"'<,>.?\\/]{6,16}$/,
  Phone: /^1[3-9]\d{9}$/,
};

/**
 * 字典类型
 */
export const DictType = {
  PositionLevel: 'positionLevel',
};

/**
 * 权限相关常量
 */
export const PermissionConstant = {
  SuperAdmin: '系统管理员',
};

/**
 * 静态路由
 */
export const StaticRoutes = {
  Login: '/auth/login',
};

/**
 * 操作类型
 */
export const OperateType = {
  1: 'Create',
  2: 'Query',
  3: 'Update',
  4: 'Delete',
  5: 'Export',
  6: 'Import',
  7: 'Other',
};
