import Permission from '@/components/Permission';
import {
  deleteScheduledTask,
  getScheduledTaskList,
  type ScheduledTaskDto,
  type ScheduledTaskListDto,
} from '@/api/system/scheduledTask.ts';
import { DeleteOutlined, EditOutlined, PlusOutlined, UnorderedListOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space, Tag } from 'antd';
import React, { useRef, useState } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import ScheduledTaskForm, { type ModalRef } from '@/pages/system/components/ScheduledTaskForm.tsx';
import useApp from 'antd/es/app/useApp';
import TaskExecutionLogModal from '@/pages/system/components/TaskExecutionLogModal.tsx';

const ScheduledTaskList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const modalRef = useRef<ModalRef>(null);
  const { message } = useApp();
  const [logModalVisible, setLogModalVisible] = useState<boolean>(false);
  const [currentTaskKey, setCurrentTaskKey] = useState<string>('');
  const columns: SmartTableColumnType[] = [
    {
      title: '任务KEY',
      dataIndex: 'taskKey',
    },
    {
      title: 'Cron表达式',
      dataIndex: 'cronExpression',
    },
    {
      title: '状态',
      dataIndex: 'isActive',
      render: (isActive: boolean) => {
        return isActive ? <Tag color="green">激活(Running)</Tag> : <Tag>未激活(Stop)</Tag>;
      },
    },
    {
      title: '任务描述',
      dataIndex: 'description',
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
    },
    {
      title: '上次修改时间',
      dataIndex: 'lastModificationTime',
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 210,
      fixed: 'right',
      render: (_: any, record: ScheduledTaskListDto) => (
        <Space>
          <Permission permissions={'Sys.ScheduledTask.Log'}>
            <Button
              type="link"
              icon={<UnorderedListOutlined />}
              onClick={() => {
                setLogModalVisible(true);
                setCurrentTaskKey(record.taskKey);
              }}
            >
              日志
            </Button>
          </Permission>
          <Permission permissions={'Sys.ScheduledTask.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                modalRef?.current?.openModal(record as ScheduledTaskDto);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Sys.ScheduledTask.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteScheduledTask(record.taskKey!).then(() => {
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
          const { data } = await getScheduledTaskList(params);
          return data;
        }}
        searchItems={[
          <Form.Item label="任务KEY" name="taskKey">
            <Input placeholder="请输入任务KEY" />
          </Form.Item>,
          <Form.Item label="任务描述" name="description">
            <Input placeholder="请输入任务描述" />
          </Form.Item>,
        ]}
        toolbar={
          <Space size="middle">
            <Permission permissions={'Sys.ScheduledTask.Add'}>
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
      {/** 新增/编辑任务弹窗 */}
      <ScheduledTaskForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
      {/** 执行日志弹窗 */}
      <TaskExecutionLogModal
        visible={logModalVisible}
        taskKey={currentTaskKey}
        onCancel={() => {
          setLogModalVisible(false);
        }}
      />
    </>
  );
};

export default ScheduledTaskList;
