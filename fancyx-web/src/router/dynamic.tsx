import type { RouteObject } from 'react-router-dom';
import React, { lazy, Suspense } from 'react';
import type { FrontendMenu } from '@/api/auth';
import KeepAlive from 'react-activation';
import Fallback from '@/components/Fallback';

// 获取所有页面文件
const PageKeys = Object.keys(import.meta.glob(['@/pages/**/index.tsx', '@/pages/**/*.tsx'], { eager: true }));
const PagesList = import.meta.glob(['@/pages/**/index.tsx', '@/pages/**/*.tsx']);

const createElementFromPath = (routePath: string, keepAlive: boolean, componentPath: string) => {
  if (!PageKeys.includes(componentPath)) return undefined;

  const LazyComponent = lazy(() => PagesList[componentPath]() as Promise<{ default: React.ComponentType<any> }>);
  const key = routePath; //TODO: 后面改成hash值
  const WrappedComponent = () => {
    if (keepAlive) {
      return (
        <KeepAlive id={key} cacheKey={key} when={keepAlive}>
          <Suspense fallback={<Fallback />}>
            <LazyComponent />
          </Suspense>
        </KeepAlive>
      );
    }
    return (
      <Suspense fallback={<Fallback />}>
        <LazyComponent />
      </Suspense>
    );
  };
  return React.createElement(WrappedComponent);
};

const convertMenuToRoutes = (menus: FrontendMenu[]): RouteObject[] => {
  return menus.map((menu) => {
    const componentPath = `/src/pages/${menu.component}.tsx`;
    const route: RouteObject = {
      path: menu.path,
      element: !menu.isExternal ? createElementFromPath(menu.path, menu.keepAlive, componentPath) : undefined,
    };

    if (menu.children?.length) {
      route.children = convertMenuToRoutes(menu.children);
    }

    return route;
  });
};

export const generateDynamicRoutes = (apiRoutes: FrontendMenu[]) => {
  if (apiRoutes.length <= 0) return [];
  return convertMenuToRoutes(apiRoutes);
};
