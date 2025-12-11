import { getOnlineUsers, onlineUserLogout, type GetOnlineUserListRequest, type OnlineUserItem } from '@/api/monitor/onlineUser';
import { Button, Tag } from 'antd';
import React, { useRef } from 'react';
import Permission from '@/components/Permission';
import ProIcon from '@/components/ProIcon';
import useApp from 'antd/es/app/useApp';
import UserStore from '@/store/userStore.ts';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const OnlineUser: React.FC = () => {
  const { message } = useApp();
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<OnlineUserItem>[] = [
    {
      title: '账号',
      dataIndex: 'userName',
      render: (_: any, record: OnlineUserItem) => {
        if (UserStore.token && UserStore.token.sessionId === record.sessionId) {
          return (
            <div>
              {record.userName}
              <Tag color="magenta" className="ml-5">
                当前会话
              </Tag>
            </div>
          );
        }
        return record.userName;
      },
    },
    {
      title: 'IP',
      dataIndex: 'ip',
      search: false,
    },
    {
      title: '地址',
      dataIndex: 'address',
      search: false,
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
      search: false,
    },
    {
      title: '登录时间',
      dataIndex: 'creationTime',
      search: false,
    },
    {
      title: '操作',
      dataIndex: 'option',
      search: false,
      width: 80,
      fixed: 'right',
      render: (_: any, record: OnlineUserItem) => {
        return (
          <Permission permissions={'Monitor.Logout'}>
            <Button
              type="link"
              icon={<ProIcon icon="iconify:hugeicons:logout-04" />}
              onClick={() => {
                onlineUserLogout(record.userId + ':' + record.sessionId).then(() => {
                  message.success('注销成功');
                  actionRef.current?.reload();
                });
              }}
            >
              注销
            </Button>
          </Permission>
        );
      },
    },
  ]

  return <ProTable<OnlineUserItem, GetOnlineUserListRequest>
    className='fancyx-table-wrapper'
    actionRef={actionRef}
    rowKey="sessionId"
    columns={columns}
    request={async (
      params: GetOnlineUserListRequest
    ) => {
      const res = await getOnlineUsers(params);
      return {
        data: res.data,
        success: true,
      };
    }}
  />
}

export default OnlineUser;
