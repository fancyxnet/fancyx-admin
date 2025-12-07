import Permission from '@/components/Permission';
import {
  deleteNotifications,
  getNotificationList,
  type AddOrUpdateNotificationRequest,
  type NotificationItem,
} from '@/api/organization/notification.ts';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Select, Space, Tag } from 'antd';
import React, { useRef } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import NotificationForm, { type ModalRef } from './components/NotificationForm.tsx';
import useApp from 'antd/es/app/useApp';

const NotificationList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const modalRef = useRef<ModalRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '通知标题',
      dataIndex: 'title',
    },
    {
      title: '通知用户',
      dataIndex: 'nickName',
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
      width: 210,
      fixed: 'right',
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
                  tableRef.current?.reload();
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

  return (
    <>
      <SmartTable
        columns={columns}
        ref={tableRef}
        rowKey="id"
        request={async (params) => {
          const { data } = await getNotificationList(params);
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
          <Space size="middle">
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
          </Space>
        }
      />
      {/** 新增/编辑通知弹窗 */}
      <NotificationForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
    </>
  );
};

export default NotificationList;
