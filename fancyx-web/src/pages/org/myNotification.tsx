import { readed, getMyNotificationList, type UserNotificationItem } from '@/api/organization/myNotification.ts';
import { CheckOutlined } from '@ant-design/icons';
import { Button, Form, Input, Select, Tag } from 'antd';
import React, { useRef } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import useApp from 'antd/es/app/useApp';
import { ErrorCode } from '@/utils/globalValue';

const NotificationList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '通知标题',
      dataIndex: 'title',
    },
    {
      title: '通知内容',
      dataIndex: 'content',
    },
    {
      title: '状态',
      dataIndex: 'isReaded',
      render: (isReaded: boolean) => {
        return isReaded ? <Tag color="green">已读</Tag> : <Tag color="red">未读</Tag>;
      },
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 70,
      fixed: 'right',
      render: (_: any, record: UserNotificationItem) => {
        if (!record.isReaded) {
          return (
            <Button
              type="link"
              icon={<CheckOutlined />}
              onClick={() => {
                batchReaded([record.id]);
              }}
            >
              已读
            </Button>
          );
        }
      },
    },
  ];

  const batchReaded = (ids: string[]) => {
    readed(ids).then((res) => {
      if (res.code === ErrorCode.Success) {
        message.success('已读成功');
        tableRef?.current?.reload();
      }
    });
  };

  return (
    <>
      <SmartTable
        columns={columns}
        ref={tableRef}
        selection
        rowKey="id"
        request={async (params) => {
          const { data } = await getMyNotificationList(params);
          return data;
        }}
        searchItems={[
          <Form.Item label="通知标题" name="title">
            <Input placeholder="请输入通知标题" />
          </Form.Item>,
          <Form.Item label="通知状态" name="isReaded">
            <Select
              allowClear
              placeholder="请选择通知状态"
              options={[
                { label: '已读', value: true },
                { label: '未读', value: false },
              ]}
            />
          </Form.Item>,
        ]}
        toolbar={
          <Button
            type="primary"
            icon={<CheckOutlined />}
            onClick={() => {
              const ids = tableRef?.current?.getSelectedKeys() as string[];
              if (ids.length <= 0) {
                message.warning('请选择一条记录进行操作');
                return;
              }
              batchReaded(ids);
            }}
          >
            批量已读
          </Button>
        }
      />
    </>
  );
};

export default NotificationList;
