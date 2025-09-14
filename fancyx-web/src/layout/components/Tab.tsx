import { Tabs } from 'antd';
import { selectTabs, selectActiveKey, remove, setActiveKey } from '@/store/tabStore.ts';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router';
import UserStore from '@/store/userStore.ts';
import styled from 'styled-components';
import { useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { Menu, Item, Separator, useContextMenu } from 'react-contexify';
import 'react-contexify/dist/ReactContexify.css';
import '../style/Tab.scss';
import {
  CloseCircleOutlined,
  CloseOutlined,
  DoubleLeftOutlined,
  DoubleRightOutlined,
  ReloadOutlined,
} from '@ant-design/icons';

const StyledTabs = styled(Tabs)`
  .ant-tabs-nav {
    margin-bottom: 0 !important;
  }

  .ant-tabs-nav-wrap,
  .ant-tabs-nav-list,
  .ant-tabs-tab {
    border: none !important;
    border-radius: 0 !important;
  }

  .ant-tabs-tab {
    background: #e5e5e5 !important;
    margin: 6px 6px !important;
    padding: 4px 10px !important;
    border-radius: 4px !important;
    font-size: 12px !important;
  }

  .ant-tabs-nav::before {
    border-bottom: none !important;
  }

  .ant-tabs-tab-active {
    background: #ecf5ff !important;
  }
`;

const Tab = () => {
  const tabs: any[] = useSelector(selectTabs);
  const activeKey = useSelector(selectActiveKey);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const location = useLocation();
  const menuId = 'my-menu';
  const { show } = useContextMenu({ id: menuId });

  useEffect(() => {
    if (location.pathname) {
      onChange(location.pathname);
    }
  }, [location.pathname]);

  /**
   * 切换面板回调
   * @param {*} newActiveKey
   */
  const onChange = (newActiveKey: string): void => {
    if (newActiveKey === '' || newActiveKey === '/') {
      navigate('/');
      dispatch(setActiveKey(newActiveKey));
    }
    const tmp = UserStore.menuList.find((h) => h.path === newActiveKey);
    if (!tmp) return;

    navigate(tmp.path);
    dispatch(setActiveKey(newActiveKey));
  };
  const onEdit = (targetKey: string, action: string) => {
    if (action === 'remove') {
      const change = activeKey === targetKey;
      dispatch(remove({ key: targetKey, change: change }));
      if (change) {
        //找到要跳转/指定的活动标签
        const index = tabs.findIndex((h) => h.key === targetKey && targetKey !== '');
        if (index > 0) {
          onChange(tabs[index - 1].key);
        }
      }
    }
  };
  const handleContextMenu = (event: React.MouseEvent<HTMLDivElement>, key: string) => {
    event.preventDefault();
    show({ event, props: { key: key } }); // 将event包装成对象传递
  };
  const exeContextMenuCommand = (option: number, key: string) => {
    // 1刷新当前页面2关闭当前3关闭左侧4关闭右侧5关闭其它
    const i = tabs.findIndex((x) => x.key === key);
    const activeIndex = tabs.findIndex((x) => x.key === activeKey);
    const len = tabs.length;
    switch (option) {
      case 1:
        window.location.reload();
        break;
      case 2:
        onEdit(key, 'remove');
        break;
      case 3:
        for (let j = 1; j < i; j++) {
          dispatch(remove({ key: tabs[j].key, change: false }));
        }
        if (activeIndex < i) {
          onChange(key);
        }
        break;
      case 4:
        for (let j = i + 1; j < len; j++) {
          dispatch(remove({ key: tabs[j].key, change: false }));
        }
        if (activeIndex > i) {
          onChange(key);
        }
        break;
      case 5:
        for (let j = 1; j < len; j++) {
          if (j === i) continue;
          dispatch(remove({ key: tabs[j].key, change: false }));
        }
        // 当前不是活动页需要标记活动页
        if (activeKey !== key) {
          onChange(key);
        }
        break;
    }
  };

  return (
    <div className="fancyx-tabs">
      <StyledTabs
        hideAdd
        type="editable-card"
        onChange={onChange}
        activeKey={activeKey}
        onEdit={(e, action) => onEdit(e as string, action)}
        items={tabs.map((x) => ({
          key: x.key,
          label: <div onContextMenu={(e) => handleContextMenu(e, x.key)}>{x.label}</div>,
          closable: x.closable,
        }))}
      />
      <Menu id={menuId} className="mini-context-menu" animation="scale">
        <Item onClick={({ props }) => exeContextMenuCommand(1, props.key)}>
          <ReloadOutlined />
          <span className="ml-4">刷新当前页</span>
        </Item>
        <Item onClick={({ props }) => exeContextMenuCommand(2, props.key)}>
          <CloseOutlined />
          <span className="ml-4">关闭当前</span>
        </Item>
        <Item onClick={({ props }) => exeContextMenuCommand(3, props.key)}>
          <DoubleLeftOutlined />
          <span className="ml-4">关闭左侧</span>
        </Item>
        <Item onClick={({ props }) => exeContextMenuCommand(4, props.key)}>
          <DoubleRightOutlined />
          <span className="ml-4">关闭右侧</span>
        </Item>
        <Separator />
        <Item onClick={({ props }) => exeContextMenuCommand(5, props.key)}>
          <CloseCircleOutlined />
          <span className="ml-4">关闭其它</span>
        </Item>
      </Menu>
    </div>
  );
};
export default Tab;
