import { useOutlet } from 'react-router-dom';
import Sidebar from '@/layout/components/Sidebar.tsx';
import Navbar from './components/Navbar.tsx';
import Tab from '@/layout/components/Tab.tsx';
import { FloatButton, Layout } from 'antd';
import { useDispatch, useSelector } from 'react-redux';
import { selectCollapsed, toggleCollapsed } from '@/store/themeStore.ts';
import { useMediaQuery } from 'react-responsive';
import ErrorFallback from '@/components/ErrorFallback';
import { ErrorBoundary } from 'react-error-boundary';
import { useEffect } from 'react';

const { Content, Sider } = Layout;

function Index() {
  const curOutlet = useOutlet();
  const collapsed = useSelector(selectCollapsed);
  const isMinScreen = useMediaQuery({ maxWidth: '1024px' });
  const dispatch = useDispatch();

  useEffect(() => {
    const needToggleCollapsed = (isMinScreen && !collapsed) || (!isMinScreen && collapsed);
    if (needToggleCollapsed) {
      dispatch(toggleCollapsed());
    }
  }, [isMinScreen]);

  return (
    <Layout hasSider className="fancyx-layout">
      <Sider trigger={null} collapsible collapsed={collapsed}>
        <Sidebar />
      </Sider>
      <Layout>
        <Navbar />
        <Tab />
        <ErrorBoundary FallbackComponent={ErrorFallback}>
          <Content>{curOutlet}</Content>
        </ErrorBoundary>
        <FloatButton.BackTop />
      </Layout>
    </Layout>
  );
}

export default Index;
