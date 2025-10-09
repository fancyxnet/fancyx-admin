import { Button, Drawer, Radio, Typography, Divider, ColorPicker } from 'antd';
import type { Color } from 'antd/es/color-picker';
import { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import type { ThemeType } from '@/theme';
import { switchTheme, saveCustomThemeColor, applyCustomTheme, saveThemeToStorage } from '@/utils/themeUtils';
import { setTheme } from '@/store/themeStore';
import ProIcon from '@/components/ProIcon';
import './style.scss';

interface ThemeSwitcherProps {
  className?: string;
  size?: 'small' | 'middle' | 'large';
}

const { Title, Text } = Typography;

// 定义主题选项类型
interface ThemeOption {
  value: ThemeType;
  label: string;
  color?: string;
}

// 定义颜色方案接口
interface ColorScheme {
  primary: string;
  primaryHover: string;
  primaryActive: string;
  primaryLight: string;
}

// 定义颜色方案接口（包含显示名称）
interface ColorScheme {  
  primary: string;
  primaryHover: string;
  primaryActive: string;
  primaryLight: string;
  displayName: string;
}

// 预设的颜色方案
const colorSchemes: Record<string, ColorScheme> = {
  purple: {
    primary: '#7E57C2',
    primaryHover: '#9575CD',
    primaryActive: '#673AB7',
    primaryLight: '#EDE7F6',
    displayName: '紫罗兰'
  },
  blue: {
    primary: '#1890ff',
    primaryHover: '#40a9ff',
    primaryActive: '#096dd9',
    primaryLight: '#e6f7ff',
    displayName: '蓝色'
  },
  green: {
    primary: '#52c41a',
    primaryHover: '#73d13d',
    primaryActive: '#389e0d',
    primaryLight: '#f6ffed',
    displayName: '绿色'
  },
  red: {
    primary: '#ff4d4f',
    primaryHover: '#ff7875',
    primaryActive: '#cf1322',
    primaryLight: '#fff1f0',
    displayName: '红色'
  },
  orange: {
    primary: '#fa8c16',
    primaryHover: '#ffa940',
    primaryActive: '#d46b08',
    primaryLight: '#fffbe6',
    displayName: '橙色'
  },
  pink: {
    primary: '#eb2f96',
    primaryHover: '#f759ab',
    primaryActive: '#c41d7f',
    primaryLight: '#fff1f0',
    displayName: '粉色'
  },
};

const ThemeSwitcher: React.FC<ThemeSwitcherProps> = ({ className = '', size = 'small' }) => {
  const dispatch = useDispatch();
  const [currentTheme, setCurrentTheme] = useState<ThemeType>('default');
  const [customColor, setCustomColor] = useState('#7E57C2');
  const [isDrawerOpen, setIsDrawerOpen] = useState(false);
  const [selectedColorScheme, setSelectedColorScheme] = useState('purple');

  // 主题选项配置
  const themeOptions: ThemeOption[] = [
    {
      value: 'default',
      label: '紫罗兰',
      color: colorSchemes.purple.primary
    },
    {
      value: 'blue-light',
      label: '蓝色',
      color: colorSchemes.blue.primary
    },
    {
      value: 'custom',
      label: '自定义'
    }
  ];

  // 初始化当前主题
  useEffect(() => {
    // 尝试从 localStorage 中获取当前主题
    const savedTheme = localStorage.getItem('fancyx-admin-theme') as ThemeType || 'default';
    setCurrentTheme(savedTheme);
    
    // 尝试从 localStorage 中获取自定义颜色
    const savedCustomColor = localStorage.getItem('fancyx-admin-custom-color');
    if (savedCustomColor) {
      setCustomColor(savedCustomColor);
    }
    
    // 设置默认的颜色方案
    setSelectedColorScheme(currentTheme === 'default' ? 'purple' : 'blue');
  }, []);

  // 当主题改变时应用相应的主题
  useEffect(() => {
    if (currentTheme === 'custom') {
      applyCustomTheme();
    } else {
      switchTheme(currentTheme);
    }
  }, [currentTheme, customColor]);

  // 处理主题切换
  const handleThemeChange = (value: ThemeType) => {
    setCurrentTheme(value);
    // 更新 Redux store
    dispatch(setTheme(value));
    // 保存主题到 localStorage
    saveThemeToStorage(value);
    // 关闭抽屉
    setIsDrawerOpen(false);
  };

  // 处理自定义颜色切换
  const handleCustomColorChange = (color: Color) => {
    // 确保color是一个有效的颜色对象
    const colorHex = typeof color === 'string' ? color : color.toHexString();
    setCustomColor(colorHex);
    // 保存自定义颜色到 localStorage
    saveCustomThemeColor(colorHex);
    // 创建自定义主题并应用
    createAndApplyCustomTheme();
  };

  // 处理颜色方案选择
  const handleColorSchemeSelect = (scheme: string) => {
    setSelectedColorScheme(scheme);
    const color = colorSchemes[scheme].primary;
    setCustomColor(color);
    // 保存自定义颜色到 localStorage
    saveCustomThemeColor(color);
    // 创建自定义主题并应用
    createAndApplyCustomTheme();
  };

  // 创建并应用自定义主题
  const createAndApplyCustomTheme = () => {
    const customThemeName: ThemeType = 'custom';
    
    // 保存主题信息
    localStorage.setItem('fancyx-admin-theme', customThemeName);
    
    // 更新状态和 Redux store
    dispatch(setTheme(customThemeName));
    
    // 应用自定义主题
    applyCustomTheme();
  };

  return (
    <>
      <Button
        type="text"
        className={`theme-switcher-btn ${className}`}
        size={size}
        onClick={() => setIsDrawerOpen(true)}
        icon={<ProIcon icon="antd:BgColorsOutlined" />}
      >
        主题
      </Button>
      
      <Drawer
        title="主题设置"
        placement="right"
        onClose={() => setIsDrawerOpen(false)}
        open={isDrawerOpen}
        width={300}
      >
        <div className="theme-drawer-content">
          <Title level={5}>主题选择</Title>
          <Radio.Group 
            value={currentTheme} 
            onChange={(e) => handleThemeChange(e.target.value)}
            className="theme-radio-group"
          >
            {themeOptions.map((option) => (
              <Radio.Button 
                key={option.value} 
                value={option.value}
                style={option.color ? { 
                  position: 'relative',
                  paddingLeft: '30px'
                } : undefined}
              >
                {option.color && (
                  <span 
                    className="theme-color-indicator"
                    style={{ 
                      position: 'absolute',
                      left: '8px',
                      top: '50%',
                      transform: 'translateY(-50%)',
                      width: '12px',
                      height: '12px',
                      borderRadius: '50%',
                      backgroundColor: option.color,
                      border: '1px solid #d9d9d9'
                    }}
                  />
                )}
                {option.label}
              </Radio.Button>
            ))}
          </Radio.Group>
          
          {/* 只有在选择自定义主题时才显示颜色选择器 */}
          {currentTheme === 'custom' && (
            <>
              <Divider />
              <Title level={5}>主题选择</Title>
              <div className="color-schemes">
                {Object.entries(colorSchemes).map(([key, scheme]) => (
                  <div
                    key={key}
                    className={`color-scheme-item ${selectedColorScheme === key ? 'active' : ''}`}
                    onClick={() => handleColorSchemeSelect(key)}
                    title={scheme.displayName}
                  >
                    <div 
                      className="color-scheme-preview"
                      style={{ backgroundColor: scheme.primary }}
                    />
                    <Text>{scheme.displayName}</Text>
                  </div>
                ))}
                {/* 自定义颜色选项 */}
                <div
                  className="color-scheme-item custom-color-item"
                  onClick={() => {
                    const element = document.querySelector('.custom-color-picker .ant-color-picker-trigger');
                    if (element instanceof HTMLElement) {
                      element.click();
                    }
                  }}
                  title="自定义颜色"
                >
                  <div 
                    className="color-scheme-preview custom-preview"
                    style={{ 
                      background: `linear-gradient(45deg, ${customColor} 25%, #f0f0f0 25%, #f0f0f0 50%, ${customColor} 50%, ${customColor} 75%, #f0f0f0 75%, #f0f0f0 100%)`,
                      backgroundSize: '20px 20px'
                    }}
                  />
                  <Text>自定义</Text>
                </div>
              </div>
              
              <Divider />
              
              <Title level={5}>自定义配色</Title>
              <div className="custom-color-section">
                <Text>选择主色调：</Text>
                <ColorPicker
                  value={customColor}
                  onChange={handleCustomColorChange}
                  showText
                  className="custom-color-picker"
                />
              </div>
            </>
          )}
        </div>
      </Drawer>
    </>
  );
};

export default ThemeSwitcher;