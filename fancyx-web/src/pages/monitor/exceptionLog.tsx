import { type ExceptionLogItem, getExceptionLogList, type GetExceptionLogListRequest, handleException } from '@/api/monitor/monitorLog.ts';
import { Button, Collapse, Descriptions, Modal, Space, Tag } from 'antd';
import React, { useMemo, useRef, useState } from 'react';
import { CheckCircleOutlined, ExclamationCircleFilled, FileTextOutlined } from '@ant-design/icons';
import useApp from 'antd/es/app/useApp';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const ExceptionLog: React.FC = () => {
  const [isOpenModal, setIsOpenModal] = useState(false);
  const { message, modal } = useApp();
  const actionRef = useRef<ActionType>();
  const [details, setDetails] = useState<ExceptionLogItem | null>();
  const columns: ProColumnType<ExceptionLogItem>[] = [
    {
      title: '发生时间',
      dataIndex: 'creationTime',
      search: false,
      minWidth: 154
    },
    {
      title: '请求地址',
      dataIndex: 'requestPath',
    },
    {
      title: '请求方法',
      dataIndex: 'requestMethod',
      search: false,
      width: 104
    },
    {
      title: '异常信息',
      dataIndex: 'message',
      search: false,
      ellipsis: true
    },
    {
      title: '异常名',
      dataIndex: 'exceptionType',
      search: false,
    },
    {
      title: '状态',
      dataIndex: 'isHandled',
      render: (_: any, record: ExceptionLogItem) => {
        if (record.isHandled) {
          return <Tag color="success">已处理</Tag>;
        }
        return <Tag color="red">待处理</Tag>;
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
      title: '操作',
      dataIndex: 'operate',
      width: 140,
      fixed: 'right',
      search: false,
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
                      actionRef?.current?.reload();
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
  return <div className='fancyx-table-wrapper'>
    <ProTable<ExceptionLogItem, GetExceptionLogListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetExceptionLogListRequest
      ) => {
        const res = await getExceptionLogList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
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
  </div>
}

export default ExceptionLog;
