import { useRoutes } from 'react-router-dom';
import { routes } from '@/router';
import zhCN from 'antd/locale/zh_CN';
import { ConfigProvider } from 'antd';
import { Suspense, useEffect, useState } from 'react';
import { generateDynamicRoutes } from './router/dynamic';
import UserStore from './store/userStore';
import { useSelector } from 'react-redux';
import { selectSize, selectTheme } from '@/store/themeStore.ts';
import { AuthProvider } from '@/components/AuthProvider';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import Application from '@/components/Application';
import { loadThemeFromStorage } from './utils/themeUtils';
import type { ThemeType } from './theme';
import { themeMap, getThemeConfig } from './theme';
import Fallback from './components/Fallback';

function App() {
  const size = useSelector(selectSize);
  const reduxTheme = useSelector(selectTheme);
  const renderRoutes = routes;
  const [currentTheme, setCurrentTheme] = useState<ThemeType>('default');

  // 初始化主题 - 在组件内部直接初始化，避免静态函数调用
  useEffect(() => {
    const initializeTheme = () => {
      try {
        // 直接在组件内部初始化主题，而不是通过静态函数
        const storedTheme = loadThemeFromStorage();
        setCurrentTheme(storedTheme);
        
        // 手动更新CSS变量
        const theme = themeMap[storedTheme];
        if (theme && theme.config && theme.config.token) {
          const root = document.documentElement;
          const { token } = theme.config;
          
          // 设置主要CSS变量
          if (token.colorPrimary) {
            root.style.setProperty('--primary-color', token.colorPrimary);
          }
          if (token.colorBgBase) {
            root.style.setProperty('--bg-base', token.colorBgBase);
          }
          // 设置其他需要的变量...
        }
      } catch (error) {
        console.error('Failed to initialize theme:', error);
      }
    };
    
    initializeTheme();
  }, []);

  // 当 Redux 中的主题变化时更新应用主题
  useEffect(() => {
    if (reduxTheme && reduxTheme !== currentTheme) {
      setCurrentTheme(reduxTheme);
    }
  }, [reduxTheme, currentTheme]);

  if (UserStore.isAuthenticated()) {
    const menus = UserStore.userInfo?.menus;
    if (Array.isArray(menus) && menus.length > 0) {
      const dyRoutes = generateDynamicRoutes(menus);
      dyRoutes.forEach((dy) => {
        renderRoutes[1].children?.push(dy);
      });
    }
  }

  // 创建 QueryClient 实例（管理缓存和请求）
  const queryClient = new QueryClient();

  return (
    <>
      <AuthProvider>
        <ConfigProvider locale={zhCN} componentSize={size} theme={getThemeConfig(currentTheme)}>
          <Application>
            <QueryClientProvider client={queryClient}>
              <Suspense fallback={<Fallback/>}>{useRoutes(renderRoutes)}</Suspense>
            </QueryClientProvider>
          </Application>
        </ConfigProvider>
      </AuthProvider>
    </>
  );
}

export default App;
