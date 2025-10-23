import { Avatar, Breadcrumb, Button, Dropdown, type MenuProps } from 'antd';
import ProIcon from '@/components/ProIcon';
import { signOut } from '@/api/auth.ts';
import { useLocation, useNavigate } from 'react-router-dom';
import UserStore from '@/store/userStore.ts';
import { selectCollapsed, toggleCollapsed } from '@/store/themeStore.ts';
import { useDispatch, useSelector } from 'react-redux';
import { open } from '@/store/tabStore.ts';
import { useMemo, useRef } from 'react';
import { LogoutOutlined, UserOutlined } from '@ant-design/icons';
import SearchModal, { type SearchModalRef } from '@/layout/components/SearchModal.tsx';
import { useApplication } from '@/components/Application';
import { StaticRoutes } from '@/utils/globalValue.ts';
import { useAuthProvider } from '@/components/AuthProvider';
import { observer } from 'mobx-react-lite';
import NotificationPopover from '@/layout/components/NotificationPopover.tsx';
import InterfaceSettings from '@/components/InterfaceSettings';

const Navbar = observer(() => {
  const collapsed = useSelector(selectCollapsed);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const searchModalRef = useRef<SearchModalRef>(null);
  const { clearToken } = useAuthProvider();
  const userItems: MenuProps['items'] = [
    {
      key: 'profile',
      label: '个人信息',
      icon: <UserOutlined />,
    },
    {
      type: 'divider',
    },
    {
      key: 'logout',
      label: '退出登录',
      icon: <LogoutOutlined />,
    },
  ];
  const { ossDomain } = useApplication();

  const location = useLocation();
  const setCollapsed = () => {
    dispatch(toggleCollapsed());
  };

  const onClick = async ({ key }: { key: string }) => {
    if (key === 'logout') {
      clearToken();
      navigate(StaticRoutes.Login);
      await signOut();
    } else if (key === 'profile') {
      navigate('/profile');
      dispatch(open({ path: '/profile' }));
    }
  };

  const breadcrumbItems = useMemo((): { title: string }[] => {
    const curMenu = UserStore.menuList.find((x) => x.path === location.pathname);
    if (curMenu) {
      return curMenu.layerName.split('/').map((x) => {
        return { title: x };
      });
    }
    return [];
  }, [location.pathname]);

  return (
    <>
      <div className="flex w-full fancyx-navbar">
        <div>
          <Button type="text" onClick={setCollapsed} className="navbar-btn">
            <ProIcon icon={collapsed ? 'antd:MenuUnfoldOutlined' : 'antd:MenuFoldOutlined'} />
          </Button>
        </div>
        <div className="ml-10 fancyx-navbar-breadcrumb-wrapper">
          <Breadcrumb items={breadcrumbItems} />
        </div>
        {/* 右侧菜单 */}
        <div className="grow flex flex-row-reverse fancyx-navbar-right-wrapper">
          {/* 头像 */}
          <div>
            <Dropdown
              menu={{
                items: userItems,
                selectable: true,
                onClick,
              }}
              trigger={['click']}
            >
              <Button type="text" className="navbar-btn">
                {UserStore.userInfo?.avatar ? (
                  <Avatar size={28} src={ossDomain + UserStore.userInfo?.avatar} alt="头像" />
                ) : (
                  <Avatar size={28} icon={<UserOutlined />} />
                )}
                {UserStore.userInfo?.nickName ?? UserStore.userInfo?.userName}
              </Button>
            </Dropdown>
          </div>

          {/** 主题 */}
          <div>
            <InterfaceSettings size="small" />
          </div>

          {/** 通知 */}
          <div>
            <NotificationPopover />
          </div>

          {/** 搜索 */}
          <div>
            <Button type="text" className="navbar-btn" onClick={searchModalRef?.current?.openModal}>
              <ProIcon icon="antd:SearchOutlined" />
            </Button>
            {/* 搜索框 */}
            <SearchModal ref={searchModalRef} />
          </div>
        </div>
      </div>
    </>
  );
});

export default Navbar;
