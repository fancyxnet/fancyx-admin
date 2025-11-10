import { Menu } from 'antd';
import UserStore from '@/store/userStore.ts';
import type { FrontendMenu } from '@/api/auth.ts';
import { Link } from 'react-router-dom';
import React, { useMemo } from 'react';
import ProIcon from '@/components/ProIcon';
import { useDispatch, useSelector } from 'react-redux';
import { open, selectActiveKey } from '@/store/tabStore.ts';
import { selectCollapsed, selectSidebarMode } from '@/store/themeStore.ts';
import { HomeOutlined } from '@ant-design/icons';
import { observer } from 'mobx-react-lite';
import _ from 'lodash';
import type { ItemType, MenuItemType } from 'antd/es/menu/interface';

interface MenuItem {
  key: string;
  icon: React.ReactNode | null;
  label: React.ReactNode;
  children: MenuItem[] | null;
}

const Sidebar = observer(() => {
  const dispatch = useDispatch();
  const activeKey = useSelector(selectActiveKey);
  const collapsed = useSelector(selectCollapsed);
  const sidebarMode = useSelector(selectSidebarMode);

  const addTab = (menu: FrontendMenu) => {
    dispatch(open({ name: menu.title, path: menu.path }));
  };

  function convertToAntdMenuItems(menus: FrontendMenu[]) {
    return menus
      .filter((menu) => menu.display)
      .map((menu) => {
        const item: MenuItem = {
          key: menu.path ?? menu.id,
          icon: menu.icon ? <ProIcon icon={menu.icon} /> : null,
          label: menu.title,
          children: null,
        };

        if (menu.children && menu.children.length > 0) {
          item.children = convertToAntdMenuItems(menu.children);
        } else if (menu.path) {
          if (menu.isExternal) {
            menu.path = `/external/${menu.path}`;
          }
          item.label = (
            <Link to={menu.path} onClick={() => addTab(menu)}>
              <span>{menu.title}</span>
            </Link>
          );
        }

        return item;
      });
  }

  /* eslint-disable react-hooks/exhaustive-deps */
  const calcItems = useMemo(() => {
    const menus = _.cloneDeep(UserStore.userInfo?.menus ?? []);
    if (Array.isArray(menus) && menus.length > 0) {
      return convertToAntdMenuItems(menus);
    }
    return [];
  }, [UserStore.userInfo?.menus]);

  return (
    <div className={`fancyx-sidebar ${sidebarMode === 'dark' ? 'dark-sidebar' : ''}`}>
      <div className="sidebar-header">
        <h2>
          <HomeOutlined className={'header-icon' + (collapsed ? ' header-icon-center' : '')} />
          {!collapsed && <span>风汐管理系统</span>}
        </h2>
      </div>
      
      <div className="sidebar-content">
        <Menu mode="inline" items={calcItems as ItemType<MenuItemType>[]} selectedKeys={[activeKey]} inlineCollapsed={collapsed} />
      </div>
    </div>
  );
});

export default Sidebar;
