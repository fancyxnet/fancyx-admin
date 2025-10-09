import { useState } from 'react';
import { Button, Drawer, Radio, Typography } from 'antd';
import { loadThemeFromStorage, switchTheme } from '@/utils/themeUtils';
import { themeMap } from '@/theme';
import { setTheme, setSize, setSidebarMode, selectSize, selectSidebarMode } from '@/store/themeStore';
import { useDispatch, useSelector } from 'react-redux';
import ProIcon from '@/components/ProIcon';
import './index.scss';
import type { ThemeType } from '@/theme';

interface InterfaceSettingsProps {
  className?: string;
  size?: 'small' | 'middle' | 'large';
}

const { Title, Text } = Typography;

const InterfaceSettings: React.FC<InterfaceSettingsProps> = ({ size = 'small' }) => {
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [currentTheme, setCurrentTheme] = useState<ThemeType>(loadThemeFromStorage());
  const currentSize = useSelector(selectSize);
  const currentSidebarMode = useSelector(selectSidebarMode);
  const dispatch = useDispatch();

  // 处理尺寸选择
  const handleSizeSelect = (e: any) => {
    dispatch(setSize(e.target.value));
  };

  // 处理侧边栏主题模式选择
  const handleSidebarModeSelect = (e: any) => {
    dispatch(setSidebarMode(e.target.value));
  };

  // 处理主题选择
  const handleThemeSelect = (themeName: ThemeType) => {
    setCurrentTheme(themeName);

    // 更新Redux store中的主题状态
    dispatch(setTheme(themeName));

    // 使用themeUtils中的switchTheme函数应用主题
    switchTheme(themeName, () => {
      // 为了确保更改生效，触发一次重绘
      const body = document.body;
      const display = body.style.display;
      body.style.display = 'none';
      void body.offsetHeight; // 触发重绘
      body.style.display = display;
    });
  };

  return (
    <>
      <Button
        type="text" className="navbar-btn"
        size={size}
        onClick={() => setIsDrawerOpen(true)}
      >
        <ProIcon icon="antd:SettingOutlined" />
      </Button>

      <Drawer
        title="界面设置"
        placement="right"
        onClose={() => setIsDrawerOpen(false)}
        open={isDrawerOpen}
        width={300}
      >
        <div className="theme-drawer-content">
          <Title level={5}>主题</Title>
          <div className="color-schemes">
            {Object.values(themeMap).map((theme) => (
              <div
                key={theme.name}
                className={`color-scheme-item ${currentTheme === theme.name ? 'active' : ''}`}
                onClick={() => handleThemeSelect(theme.name)}
                title={theme.description}
              >
                <div
                  className="color-scheme-preview"
                  style={{
                    backgroundColor: theme.config?.token?.colorPrimary || '#7E57C2'
                  }}
                />
                <Text>{theme.displayName}</Text>
              </div>
            ))}
          </div>

          <Title level={5} style={{ marginTop: '24px' }}>尺寸</Title>
          <Radio.Group
            value={currentSize}
            onChange={handleSizeSelect}
            buttonStyle="solid"
            className="theme-radio-group"
          >
            <Radio.Button value="large">大</Radio.Button>
            <Radio.Button value="middle">中</Radio.Button>
            <Radio.Button value="small">小</Radio.Button>
          </Radio.Group>

          <Title level={5} style={{ marginTop: '24px' }}>侧边栏</Title>
          <Radio.Group
            value={currentSidebarMode}
            onChange={handleSidebarModeSelect}
            buttonStyle="solid"
            className="theme-radio-group"
          >
            <Radio.Button value="light">亮色</Radio.Button>
            <Radio.Button value="dark">暗色</Radio.Button>
          </Radio.Group>
        </div>
      </Drawer>
    </>
  );
};

export default InterfaceSettings;