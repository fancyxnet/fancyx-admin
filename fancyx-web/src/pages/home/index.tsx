import { Card, Row, Col, Typography, Timeline, Button, Space, Tooltip, Tag, Divider, Image } from 'antd';
import {
  GithubOutlined,
  BookOutlined,
  CodeOutlined,
  HistoryOutlined,
  GiftOutlined,
  StarOutlined,
  ForkOutlined,
  EyeOutlined,
} from '@ant-design/icons';
import './index.scss';
import AliPay from '@/assets/alipay.jpg';
import { useEffect, useState } from 'react';

const { Title, Text, Paragraph } = Typography;

const HomePage = () => {
  // 更新日志数据
  const changelog = [
    {
      date: '2025-07-29',
      items: ['token定时检查、刷新'],
    },
    {
      date: '2025-07-18',
      items: ['通知管理、我的通知、动态管理定时任务'],
    },
    {
      date: '2025-07-15',
      items: ['租户管理、菜单支持外链'],
    },
    {
      date: '2025-07-14',
      items: ['创建sqlite分支，内置fancyx-admin.db文件、配置管理、修复已知BUG'],
    },
    {
      date: '2025-07-10',
      items: ['头像上传功能、使用mobx进行状态同步、修复已知BUG'],
    },
    {
      date: '2025-07-09',
      items: ['访问日志,异常日志增加详情、登录页样式优化，增加备案号'],
    },
    {
      date: '2025-07-05',
      items: ['增加员工绑定用户、样式调整、消除React警告'],
    },
    {
      date: '2025-07-02',
      items: ['系统雏形、建立仓库，代码开源'],
    },
  ];

  // 技术栈数据
  const techStack = [
    { name: '.NET Core' },
    { name: 'PostgreSQL' },
    { name: 'FreeSql' },
    { name: 'Aop' },
    { name: 'Redis' },
    { name: 'EventBus' },
    { name: 'AutoMapper' },
    { name: 'Serilog' },
    { name: 'React' },
    { name: 'Ant Design' },
    { name: 'Vite' },
    { name: 'Sass/SCSS' },
  ];

  // 仓库地址
  const repoLinks = [
    { name: 'GitHub', icon: <GithubOutlined />, url: 'https://github.com/fancyxnet/fancyx-admin' },
    { name: 'Gitee', icon: <GithubOutlined />, url: 'https://gitee.com/fancyxnet/fancyx-admin' },
    { name: '文档', icon: <BookOutlined />, url: 'https://crackerwork.com' },
  ];
  const [githubRepoInfo, setGithubRepoInfo] = useState<any>({
    starCount: 26,
    forkCount: 6,
    watchCount: 2,
  });

  useEffect(() => {
    fetch('https://api.github.com/repos/fancyxnet/fancyx-admin').then(async (res) => {
      const json = await res.json();
      if (json instanceof Object) {
        setGithubRepoInfo({
          starCount: json['stargazers_count'],
          forkCount: json['forks'],
          watchCount: json['subscribers_count'],
        });
      }
    });
  }, []);

  return (
    <div className="home-page">
      {/* 主要内容区域 */}
      <Row gutter={[24, 24]} className="main-content">
        {/* 左侧列 - 项目介绍和技术栈 */}
        <Col xs={24} md={16}>
          <Row gutter={[24, 24]}>
            {/* 开源项目介绍 */}
            <Col span={24}>
              <Card
                title={
                  <Space>
                    <BookOutlined />
                    <Title level={5} style={{ margin: 0 }}>
                      项目介绍
                    </Title>
                  </Space>
                }
                className="dashboard-card"
              >
                <Paragraph>
                  风汐管理系统，使用.NET9+React18构建的RBAC通用权限管理系统（支持按钮级别权限），支持多租户功能，简单易上手，不使用任何三方Admin框架，完全作者独立开发；旨在为个人、企业提供高效、美观的后台管理解决方案，为.NET+React后台方案添砖加瓦，
                  系统采用最新最稳定的技术栈，具有良好的扩展性和可维护性，支持快速定制开发。
                </Paragraph>
                <Paragraph>
                  <Text strong>核心特点：</Text>
                  <ul className="project-features">
                    <li>支持多租户</li>
                    <li>按钮级别权限控制</li>
                    <li>简洁高效的用户界面</li>
                    <li>模块化的系统架构</li>
                    <li>可读性高代码结构</li>
                  </ul>
                </Paragraph>

                <Divider dashed />

                <Space direction="vertical" style={{ width: '100%' }}>
                  <Text strong>仓库地址：</Text>
                  <div className="repo-buttons">
                    {repoLinks.map((link, index) => (
                      <Button
                        key={index}
                        icon={link.icon}
                        size="middle"
                        onClick={() => window.open(link.url, '_blank')}
                        className="repo-button"
                      >
                        {link.name}
                      </Button>
                    ))}
                  </div>
                  <div className="repo-stats">
                    <Tooltip title="Stars">
                      <Space>
                        <StarOutlined />
                        <Text strong>{githubRepoInfo.starCount}</Text>
                      </Space>
                    </Tooltip>
                    <Tooltip title="Forks">
                      <Space>
                        <ForkOutlined />
                        <Text strong>{githubRepoInfo.forkCount}</Text>
                      </Space>
                    </Tooltip>
                    <Tooltip title="Watchers">
                      <Space>
                        <EyeOutlined />
                        <Text strong>{githubRepoInfo.watchCount}</Text>
                      </Space>
                    </Tooltip>
                  </div>
                </Space>
              </Card>
            </Col>

            {/* 使用技术列表 */}
            <Col span={24}>
              <Card
                title={
                  <Space>
                    <CodeOutlined />
                    <Title level={5} style={{ margin: 0 }}>
                      使用技术列表
                    </Title>
                  </Space>
                }
                className="dashboard-card"
              >
                <div className="tech-stack">
                  {techStack.map((tech, index) => (
                    <Tag key={index} className="tech-tag">
                      {tech.name}
                    </Tag>
                  ))}
                </div>
                <Paragraph type="secondary" style={{ marginTop: 16 }}>
                  前端基于React和Ant Design在Vite下构建，后端使用.NETCore
                  WebAPI，数据库默认使用PostgreSQL（支持MySQL,SQLServer,Oracle等多种数据库）；
                  后端实现了服务自动注册、服务属性注入、模块化动态加载、Aop拦截特性特色功能，并且系统记录了API访问日志、异常日志、业务日志、登录日志。
                </Paragraph>
              </Card>
            </Col>
          </Row>
        </Col>

        {/* 右侧列 - 更新日志和捐赠 */}
        <Col xs={24} md={8}>
          <Row gutter={[24, 24]}>
            {/* 打赏二维码 */}
            <Col span={24}>
              <Card
                title={
                  <Space>
                    <GiftOutlined />
                    <Title level={5} style={{ margin: 0 }}>
                      支持作者
                    </Title>
                  </Space>
                }
                className="dashboard-card"
              >
                <div className="donation-container">
                  <div className="qrcode-placeholder">
                    <div className="qrcode-icon">
                      <Image src={AliPay} />
                    </div>
                  </div>
                  <div className="donation-text">
                    <Paragraph type="secondary" style={{ textAlign: 'center' }}>
                      请使用支付宝扫一扫
                    </Paragraph>
                    <Paragraph style={{ textAlign: 'center' }}>
                      <Text strong>如果您觉得这个项目对您有帮助，就请作者喝杯咖啡吧！</Text>
                    </Paragraph>
                  </div>
                </div>
              </Card>
            </Col>

            {/* 更新日志 */}
            <Col span={24}>
              <Card
                title={
                  <Space>
                    <HistoryOutlined />
                    <Title level={5} style={{ margin: 0 }}>
                      更新日志
                    </Title>
                  </Space>
                }
                className="dashboard-card"
              >
                <Timeline
                  mode="left"
                  items={changelog.map((log, index) => {
                    return {
                      color: '#7E57C2',
                      children: (
                        <div className="changelog-item" key={index}>
                          <Space>
                            <Text strong>{log.date}</Text>
                          </Space>
                          <ul className="changelog-list">
                            {log.items.map((item, i) => (
                              <li key={i}>{item}</li>
                            ))}
                          </ul>
                        </div>
                      ),
                    };
                  })}
                />
              </Card>
            </Col>
          </Row>
        </Col>
      </Row>
    </div>
  );
};

export default HomePage;
