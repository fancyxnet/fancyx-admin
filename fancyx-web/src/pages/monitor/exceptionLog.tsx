import { type ExceptionLogItem, getExceptionLogList, handleException } from '@/api/monitor/monitorLog.ts';
import { Button, Collapse, Descriptions, Form, Input, Modal, Select, Space, Tag } from 'antd';
import React, { useMemo, useRef, useState } from 'react';
import type { SmartTableColumnType, SmartTableRef } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import { CheckCircleOutlined, ExclamationCircleFilled, FileTextOutlined } from '@ant-design/icons';
import useApp from 'antd/es/app/useApp';

const BusinessLogList: React.FC = () => {
  const [isOpenModal, setIsOpenModal] = useState(false);
  const [details, setDetails] = useState<ExceptionLogItem | null>();
  const { message, modal } = useApp();
  const tableRef = useRef<SmartTableRef>(null);
  const columns: SmartTableColumnType[] = [
    {
      title: '发生时间',
      dataIndex: 'creationTime',
    },
    {
      title: '请求地址',
      dataIndex: 'requestPath',
    },
    {
      title: '请求方法',
      dataIndex: 'requestMethod',
    },
    {
      title: '异常信息',
      dataIndex: 'message',
    },
    {
      title: '异常名',
      dataIndex: 'exceptionType',
    },
    {
      title: '状态',
      dataIndex: 'isHandled',
      render: (isHandled: boolean) => {
        if (isHandled) {
          return <Tag color="success">已处理</Tag>;
        }
        return <Tag color="red">待处理</Tag>;
      },
    },
    {
      title: '操作',
      dataIndex: 'operate',
      width: 140,
      fixed: 'right',
      render: (_: any, record: ExceptionLogItem) => {
        return (
          <Space>
            <Button
              type="link"
              icon={<FileTextOutlined />}
              onClick={() => {
                setDetails(record);
                setIsOpenModal(true);
              }}
            >
              详情
            </Button>
            <Button
              type="link"
              icon={<CheckCircleOutlined />}
              onClick={() => {
                modal.confirm({
                  title: '确认已处理？',
                  icon: <ExclamationCircleFilled />,
                  onOk() {
                    handleException(record.id).then(() => {
                      message.success('处理成功');
                      tableRef?.current?.reload();
                    });
                  },
                });
              }}
            >
              已处理
            </Button>
          </Space>
        );
      },
    },
  ];

  const items = useMemo(() => {
    return [
      {
        key: 'creationTime',
        label: '发生时间',
        children: <p>{details?.creationTime}</p>,
      },
      {
        key: 'requestPath',
        label: '请求地址',
        children: <p>{details?.requestPath}</p>,
      },
      {
        key: 'requestMethod',
        label: '请求方法',
        children: <p>{details?.requestMethod}</p>,
      },
      {
        key: 'exceptionType',
        label: '异常名',
        children: <p>{details?.exceptionType}</p>,
      },
      {
        key: 'message',
        label: '异常信息',
        children: <p>{details?.message}</p>,
      },
      {
        key: 'userName',
        label: '操作用户',
        children: <p>{details?.userName}</p>,
      },
      {
        key: '用户IP',
        label: 'IP',
        children: <p>{details?.ip}</p>,
      },
      {
        key: 'browser',
        label: '浏览器',
        children: <p>{details?.browser}</p>,
      },
      {
        key: 'traceId',
        label: '跟踪ID',
        children: <p>{details?.traceId}</p>,
      },
      {
        key: 'isHandled',
        label: '状态',
        children: <>{details?.isHandled ? <Tag color="success">已处理</Tag> : <Tag color="red">待处理</Tag>}</>,
      },
      {
        key: 'handledBy',
        label: '处理人',
        children: <p>{details?.handledBy}</p>,
      },
      {
        key: 'handledTime',
        label: '处理时间',
        children: <p>{details?.handledTime}</p>,
      },
    ];
  }, [details]);
  return (
    <>
      <SmartTable
        columns={columns}
        rowKey="id"
        ref={tableRef}
        request={async (params) => {
          const { data } = await getExceptionLogList(params);
          return data;
        }}
        searchItems={
          <>
            <Form.Item label="API" name="path">
              <Input placeholder="请输入API地址" />
            </Form.Item>
            <Form.Item label="操作用户" name="userName">
              <Input placeholder="请输入操作账号" />
            </Form.Item>
            <Form.Item label="是否处理" name="isHandled">
              <Select
                placeholder="请选择处理状态"
                options={[
                  { label: '待处理', value: false },
                  { label: '已处理', value: true },
                ]}
                allowClear
              />
            </Form.Item>
          </>
        }
      />
      <Modal
        title="异常日志详情"
        open={isOpenModal}
        footer={null}
        width="60%"
        onCancel={() => {
          setIsOpenModal(false);
          setDetails(null);
        }}
      >
        <div style={{ padding: '16px' }}>
          <Descriptions items={items} column={2} size="small" />
        </div>
        <Collapse
          ghost
          items={[
            {
              key: '1',
              label: '异常堆栈信息',
              children: <p>{details?.stackTrace}</p>,
            },
          ]}
        />
      </Modal>
    </>
  );
};

export default BusinessLogList;
