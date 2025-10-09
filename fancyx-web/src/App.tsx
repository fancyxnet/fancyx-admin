import { useRoutes } from 'react-router-dom';
import { routes } from '@/router';
import zhCN from 'antd/locale/zh_CN';
import { ConfigProvider, Spin } from 'antd';
import { Suspense, useEffect, useState } from 'react';
import { generateDynamicRoutes } from './router/dynamic';
import UserStore from './store/userStore';
import { useSelector } from 'react-redux';
import { selectSize, selectTheme } from '@/store/themeStore.ts';
import { AuthProvider } from '@/components/AuthProvider';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import Application from '@/components/Application';
import { initTheme, getTheme, loadThemeFromStorage } from './utils/themeUtils';
import type { ThemeType } from './theme';

function App() {
  const size = useSelector(selectSize);
  const reduxTheme = useSelector(selectTheme);
  const renderRoutes = routes;
  const [currentTheme, setCurrentTheme] = useState<ThemeType>('default');

  // 初始化主题
  useEffect(() => {
    initTheme();
    setCurrentTheme(loadThemeFromStorage());
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

  const fallback = (
    <div
      style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh',
      }}
    >
      <Spin />
    </div>
  );

  // 创建 QueryClient 实例（管理缓存和请求）
  const queryClient = new QueryClient();

  return (
    <>
      <AuthProvider>
        <ConfigProvider locale={zhCN} componentSize={size} theme={getTheme(currentTheme)}>
          <Application>
            <QueryClientProvider client={queryClient}>
              <Suspense fallback={fallback}>{useRoutes(renderRoutes)}</Suspense>
            </QueryClientProvider>
          </Application>
        </ConfigProvider>
      </AuthProvider>
    </>
  );
}

export default App;
