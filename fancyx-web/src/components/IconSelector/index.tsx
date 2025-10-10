import { Button, Input, Modal, Select, Space, Tooltip, Tabs } from 'antd';
import { SearchOutlined, DeleteOutlined, InfoCircleOutlined } from '@ant-design/icons';
import * as antdIcons from '@ant-design/icons';
import React, { useState, useEffect } from 'react';
import ProIcon from '../ProIcon';
import './index.scss';

interface IconSelectorProps {
  value?: string;
  onChange?: (value: string) => void;
  placeholder?: string;
  disabled?: boolean;
}

const IconSelector: React.FC<IconSelectorProps> = ({
  value,
  onChange,
  placeholder = '请选择图标',
  disabled = false
}) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [filteredIcons, setFilteredIcons] = useState<string[]>([]);
  const [selectedType, setSelectedType] = useState<'antd' | 'iconify'>('antd');
  const [selectedStyle, setSelectedStyle] = useState<'outlined' | 'filled' | 'twoTone'>('outlined');
  const [loading, setLoading] = useState(false);

  // 初始化antd图标列表 - 从@ant-design/icons包中获取所有图标名称
  useEffect(() => {
    if (selectedType === 'antd') {
      // 根据选中的风格类型过滤图标
      let iconNames: string[] = [];
      if (selectedStyle === 'outlined') {
        iconNames = Object.keys(antdIcons).filter(key => key.endsWith('Outlined'));
      } else if (selectedStyle === 'filled') {
        iconNames = Object.keys(antdIcons).filter(key => key.endsWith('Filled'));
      } else if (selectedStyle === 'twoTone') {
        iconNames = Object.keys(antdIcons).filter(key => key.endsWith('TwoTone'));
      }
      setFilteredIcons(iconNames);
    }
  }, [selectedType, selectedStyle]);

  // 搜索图标
  useEffect(() => {
    if (selectedType === 'antd') {
        // 从@ant-design/icons包中获取所有图标名称，并根据选中的风格类型过滤
        let allIcons: string[] = [];
        if (selectedStyle === 'outlined') {
          allIcons = Object.keys(antdIcons).filter(key => key.endsWith('Outlined'));
        } else if (selectedStyle === 'filled') {
          allIcons = Object.keys(antdIcons).filter(key => key.endsWith('Filled'));
        } else if (selectedStyle === 'twoTone') {
          allIcons = Object.keys(antdIcons).filter(key => key.endsWith('TwoTone'));
        }
        
        if (searchText.trim() === '') {
          setFilteredIcons(allIcons);
        } else {
          const filtered = allIcons.filter((icon: string) =>
            icon.toLowerCase().includes(searchText.toLowerCase())
          );
          setFilteredIcons(filtered);
        }
      } else if (selectedType === 'iconify') {
      // 使用api搜索iconify图标
      if (searchText.trim() !== '') {
        searchIconifyIcons(searchText);
      } else {
        setFilteredIcons([]);
      }
    }
  }, [searchText, selectedType, selectedStyle]);

  // 搜索iconify图标
  const searchIconifyIcons = async (query: string) => {
    setLoading(true);
    try {
      // 这里使用iconify的API搜索图标
      const response = await fetch(`https://api.iconify.design/search?query=${encodeURIComponent(query)}&limit=50`);
      const data = await response.json();
      if (data.icons && Array.isArray(data.icons)) {
        const icons = data.icons.map((icon: string) => icon);
        setFilteredIcons(icons);
      }
    } catch (error) {
      console.error('搜索iconify图标失败:', error);
    } finally {
      setLoading(false);
    }
  };

  // 打开图标选择模态框
  const showModal = () => {
    setIsModalOpen(true);
  };

  // 关闭图标选择模态框
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  // 选择图标
  const handleSelectIcon = (iconName: string) => {
    const iconValue = `${selectedType}:${iconName}`;
    if (onChange) {
      onChange(iconValue);
    }
    setIsModalOpen(false);
  };

  // 清除图标
  const handleClearIcon = () => {
    if (onChange) {
      onChange('');
    }
  };

  // 渲染当前选择的图标
  const renderCurrentIcon = () => {
    if (!value) return placeholder;
    
    return (
      <Space>
        <ProIcon icon={value} />
        <span>{value}</span>
      </Space>
    );
  };

  // 渲染图标网格
  const renderIconGrid = () => {
    return (
      <div className="icon-grid">
        {filteredIcons.map((iconName) => (
          <Tooltip key={iconName} title={iconName}>
            <div
              className={`icon-item ${value === `${selectedType}:${iconName}` ? 'selected' : ''}`}
              onClick={() => handleSelectIcon(iconName)}
            >
              <ProIcon icon={`${selectedType}:${iconName}`} />
            </div>
          </Tooltip>
        ))}
      </div>
    );
  };

  return (
    <Space>
      <Button
        type="default"
        onClick={showModal}
        disabled={disabled}
        className="icon-selector-button"
      >
        {renderCurrentIcon()}
      </Button>
      {value && (
        <Button
          type="text"
          danger
          icon={<DeleteOutlined />}
          onClick={handleClearIcon}
          disabled={disabled}
        />
      )}

      <Modal
        title="选择图标"
        open={isModalOpen}
        onCancel={handleCancel}
        footer={null}
        width={600}
        className="icon-selector-modal"
      >
        <div className="icon-selector-content">
          <Space className="mb-4" style={{ width: '100%' }}>
            <Select
              value={selectedType}
              onChange={setSelectedType}
              className="flex-grow"
              style={{ width: '120px' }}
              options={[
                { value: 'antd', label: 'Ant Design 图标' },
                { value: 'iconify', label: 'Iconify 图标' }
              ]}
            />
            <Input
              placeholder="搜索图标..."
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              prefix={<SearchOutlined />}
              className="flex-grow"
              disabled={loading}
            />
          </Space>
          
          {selectedType === 'antd' && (
            <div className="icon-style-tabs mb-2">
              <Tabs
                activeKey={selectedStyle}
                onChange={(key) => setSelectedStyle(key as 'outlined' | 'filled' | 'twoTone')}
                centered
                items={[
                  {
                    key: 'outlined',
                    label: '线框 (Outlined)'
                  },
                  {
                    key: 'filled',
                    label: '实底 (Filled)'
                  },
                  {
                    key: 'twoTone',
                    label: '双色 (TwoTone)'
                  }
                ]}
              />
            </div>
          )}

          {loading ? (
            <div className="loading">加载中...</div>
          ) : filteredIcons.length > 0 ? (
            renderIconGrid()
          ) : (
            <div className="no-result">
              <InfoCircleOutlined />
              {searchText ? '未找到匹配的图标' : '请输入搜索词'}
            </div>
          )}
        </div>
      </Modal>
    </Space>
  );
};

export default IconSelector;