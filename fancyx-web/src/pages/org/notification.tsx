import Permission from '@/components/Permission';
import {
  deleteNotifications,
  getNotificationList,
  type AddOrUpdateNotificationRequest,
  type GetNotificationListRequest,
  type NotificationItem,
} from '@/api/organization/notification.ts';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Popconfirm, Space, Tag } from 'antd';
import React, { useRef } from 'react';
import NotificationForm, { type ModalRef } from './components/NotificationForm.tsx';
import useApp from 'antd/es/app/useApp';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Notification: React.FC = () => {
  const { message } = useApp();
  const modalRef = useRef<ModalRef>(null);
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<NotificationItem>[] = [
    {
      title: '通知标题',
      dataIndex: 'title',
    },
    {
      title: '通知用户',
      dataIndex: 'nickName',
      search: false
    },
    {
      title: '通知内容',
      dataIndex: 'content',
      search: false
    },
    {
      title: '状态',
      dataIndex: 'isReaded',
      render: (_: any, record: NotificationItem) => {
        return record.isReaded ? <Tag color="green">已读</Tag> : <Tag color="red">未读</Tag>;
      },
      valueEnum: {
        false: { text: '待处理' },
        true: { text: '已处理' },
      },
      fieldProps: {
        placeholder: '请选择处理状态',
      },
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
      search: false
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 210,
      fixed: 'right',
      search: false,
      render: (_: any, record: NotificationItem) => (
        <Space>
          <Permission permissions={'Sys.Notification.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                modalRef?.current?.openModal(record as AddOrUpdateNotificationRequest);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Sys.Notification.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteNotifications([record.id!]).then(() => {
                  message.success('删除成功');
                  actionRef.current?.reload();
                });
              }}
            >
              <Button type="link" danger icon={<DeleteOutlined />}>
                删除
              </Button>
            </Popconfirm>
          </Permission>
        </Space>
      ),
    },
  ];

  return <div className='fancyx-table-wrapper'>
    <ProTable<NotificationItem, GetNotificationListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetNotificationListRequest
      ) => {
        const res = await getNotificationList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      toolBarRender={
        () => [
          <Permission permissions={'Sys.Notification.Add'}>
            <Button
              type="primary"
              key="primary"
              onClick={() => {
                modalRef?.current?.openModal();
              }}
            >
              <PlusOutlined /> 新增
            </Button>
          </Permission>
        ]
      }
    />
    {/** 新增/编辑通知弹窗 */}
    <NotificationForm ref={modalRef} refresh={() => actionRef?.current?.reload()} />
  </div>
}

export default Notification;
