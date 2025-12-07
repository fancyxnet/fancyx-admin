import { getOnlineUsers, onlineUserLogout, type OnlineUserItem } from '@/api/monitor/onlineUser';
import { Button, Form, Input, Tag } from 'antd';
import React, { useRef } from 'react';
import Permission from '@/components/Permission';
import SmartTable from '@/components/SmartTable';
import type { SmartTableColumnType, SmartTableRef } from '@/components/SmartTable/type.ts';
import ProIcon from '@/components/ProIcon';
import useApp from 'antd/es/app/useApp';
import UserStore from '@/store/userStore.ts';

const OnlineUserList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '账号',
      dataIndex: 'userName',
      render: (userName: string, record: OnlineUserItem) => {
        if (UserStore.token && UserStore.token.sessionId === record.sessionId) {
          return (
            <div>
              {userName}
              <Tag color="magenta" className="ml-5">
                当前会话
              </Tag>
            </div>
          );
        }
        return userName;
      },
    },
    {
      title: 'IP',
      dataIndex: 'ip',
    },
    {
      title: '地址',
      dataIndex: 'address',
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
    },
    {
      title: '登录时间',
      dataIndex: 'creationTime',
    },
    {
      title: '操作',
      dataIndex: 'option',
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
                  tableRef.current?.reload();
                });
              }}
            >
              注销
            </Button>
          </Permission>
        );
      },
    },
  ];

  return (
    <SmartTable
      ref={tableRef}
      columns={columns}
      showPagination={false}
      rowKey="sessionId"
      request={async (params) => {
        const { data } = await getOnlineUsers(params);
        return {
          totalCount: data.length,
          items: data,
        };
      }}
      searchItems={
        <Form.Item label="账号" name="userName">
          <Input placeholder="请输入账号" />
        </Form.Item>
      }
    />
  );
};

export default OnlineUserList;
