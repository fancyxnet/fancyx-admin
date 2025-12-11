import { readed, getMyNotificationList, type UserNotificationItem, type GetMyNotificationListRequest } from '@/api/organization/myNotification.ts';
import { CheckOutlined } from '@ant-design/icons';
import { Button, Tag } from 'antd';
import React, { useRef, useState } from 'react';
import useApp from 'antd/es/app/useApp';
import { ErrorCode } from '@/utils/globalValue';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const MyNotification: React.FC = () => {
  const { message } = useApp();
  const actionRef = useRef<ActionType>();
  const [selectedKeys, setSelectedKeys] = useState<string[]>([])
  const columns: ProColumnType<UserNotificationItem>[] = [
    {
      title: '通知标题',
      dataIndex: 'title',
    },
    {
      title: '通知内容',
      dataIndex: 'content',
      search: false,
    },
    {
      title: '状态',
      dataIndex: 'isReaded',
      render: (_: any, record: UserNotificationItem) => {
        return record.isReaded ? <Tag color="green">已读</Tag> : <Tag color="red">未读</Tag>;
      },
      valueEnum: {
        false: { text: '未读' },
        true: { text: '已读' },
      },
      fieldProps: {
        placeholder: '请选择处理状态',
      },
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
      search: false,
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 70,
      fixed: 'right',
      search: false,
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
        actionRef?.current?.reload();
      }
    });
  };

  return (<div className='fancyx-table-wrapper'>
    <ProTable<UserNotificationItem, GetMyNotificationListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetMyNotificationListRequest
      ) => {
        const res = await getMyNotificationList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      rowSelection={{
        onChange: (selectedRowKeys) => {
          setSelectedKeys(selectedRowKeys as string[]);
        },
      }}
      toolBarRender={
        () => [
          <Button
            type="primary"
            icon={<CheckOutlined />}
            onClick={() => {
              if (selectedKeys.length <= 0) {
                message.warning('请选择一条记录进行操作');
                return;
              }
              batchReaded(selectedKeys);
            }}
          >
            批量已读
          </Button>
        ]
      }
    />
  </div>)
}

export default MyNotification;
