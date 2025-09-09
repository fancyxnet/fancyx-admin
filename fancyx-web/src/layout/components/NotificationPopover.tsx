import { useEffect, useState } from 'react';
import { Badge, Popover, List, Divider, Button, notification } from 'antd';
import '../style/NotificationPopover.scss';
import ProIcon from '@/components/ProIcon';
import {
  getMyNotificationNavbarInfo,
  readed,
  type UserNotificationNavbarItemDto,
} from '@/api/organization/myNotification.ts';
import useApp from 'antd/es/app/useApp'; // 自定义样式
import { useNavigate } from 'react-router-dom';
import { CheckOutlined, EyeOutlined } from '@ant-design/icons';
import { observer } from 'mobx-react-lite';
import { useMqtt } from '@/hooks/useMqtt.ts';
import UserStore from '@/store/userStore.ts';

const NotificationPopover = observer(() => {
  const [visible, setVisible] = useState(false);
  const [notifications, setNotifications] = useState<UserNotificationNavbarItemDto[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const { message } = useApp();
  const navigate = useNavigate();
  const [api, contextHolder] = notification.useNotification();

  const fetchNotifications = () => {
    getMyNotificationNavbarInfo().then((res) => {
      setNotifications(res.data.items);
      setUnreadCount(res.data.noReadedCount);
    });
  };

  useEffect(() => {
    fetchNotifications();
  }, []);

  useMqtt('Notification:' + UserStore.userInfo?.userId, {
    onMessage: (json: string) => {
      const data = JSON.parse(json);
      if (data instanceof Object) {
        fetchNotifications();
        api['info']({
          message: data.title,
          description: data.content,
          duration: 3,
          placement: 'bottomRight',
        });
      }
    },
  });

  // 一键全部已读
  const markAllAsRead = () => {
    const ids = notifications.map((x) => x.id);
    if (ids.length <= 0) {
      return;
    }
    readed(ids).then(() => {
      message.success('一键已读成功', 1, () => {
        fetchNotifications();
        setVisible(false);
      });
    });
  };

  const content = (
    <div className="notification-popover">
      <List
        dataSource={notifications.slice(0, 5)} // 只显示5条
        renderItem={(item) => (
          <List.Item className={`notification-item ${item.isReaded ? '' : 'unread'}`}>
            <List.Item.Meta
              title={<div className="notification-title">{item.title}</div>}
              description={
                <>
                  <div className="notification-content">{item.content}</div>
                  <div className="notification-time">{item.creationTime}</div>
                </>
              }
            />
          </List.Item>
        )}
      />
      <Divider style={{ margin: '8px 0' }} />
      <div className="w-full notification-actions">
        {notifications.length > 0 && (
          <Button
            type="text"
            size="small"
            icon={<EyeOutlined />}
            onClick={() => {
              navigate('/org/myNotification');
              setVisible(false);
            }}
          >
            查看详情
          </Button>
        )}
        {unreadCount > 0 && (
          <Button type="primary" size="small" icon={<CheckOutlined />} onClick={markAllAsRead}>
            一键已读
          </Button>
        )}
      </div>
    </div>
  );

  return (
    <Popover
      content={content}
      title="通知中心"
      trigger="click"
      open={visible}
      onOpenChange={setVisible}
      placement="bottomRight"
    >
      {contextHolder}
      <Badge count={unreadCount} size="small" className="notification-badge">
        <Button type="text" className="navbar-btn">
          <ProIcon icon="antd:BellOutlined" />
        </Button>
      </Badge>
    </Popover>
  );
});

export default NotificationPopover;
