import { type ApiAccessLogItem, getApiAccessLogList, type GetApiAccessLogListRequest } from '@/api/monitor/monitorLog.ts';
import { Button, Descriptions, Modal, Tag } from 'antd';
import React, { useMemo, useRef } from 'react';
import { OperateType } from '@/utils/globalValue.ts';
import { FileTextOutlined } from '@ant-design/icons';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const ApiAccessLog: React.FC = () => {
  const [isOpenModal, setIsOpenModal] = React.useState(false);
  const [details, setDetails] = React.useState<ApiAccessLogItem | null>();
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<ApiAccessLogItem>[] = [
    {
      title: '请求时间',
      dataIndex: 'requestTime',
      width: 180,
      search: false,
    },
    {
      title: '请求地址',
      dataIndex: 'path',
    },
    {
      title: '请求方法',
      dataIndex: 'method',
      width: 120,
      search: false,
    },
    {
      title: '操作用户',
      dataIndex: 'userName',
    },
    {
      title: '操作类型',
      dataIndex: 'operateType',
      search: false,
      render: (_: any, record: ApiAccessLogItem) => {
        return record.operateType?.map((x) => {
          return (
            <Tag bordered={false} color="magenta" key={x}>
              {/* eslint-disable-next-line @typescript-eslint/ban-ts-comment */}
              {/* @ts-expect-error */}
              {OperateType[x]}
            </Tag>
          );
        });
      },
    },
    {
      title: 'IP',
      dataIndex: 'ip',
      search: false,
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
      ellipsis: true,
      search: false
    },
    {
      title: '操作',
      dataIndex: 'operate',
      width: 70,
      fixed: 'right',
      search: false,
      render: (_: any, record: ApiAccessLogItem) => {
        return (
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
        );
      },
    },
  ];
  const items = useMemo(() => {
    return [
      {
        key: 'requestTime',
        label: '请求时间',
        children: <p>{details?.requestTime}</p>,
      },
      {
        key: 'path',
        label: '请求地址',
        children: <p>{details?.path}</p>,
      },
      {
        key: 'method',
        label: '请求方法',
        children: <p>{details?.method}</p>,
      },
      {
        key: 'userName',
        label: '操作用户',
        children: <p>{details?.userName}</p>,
      },
      {
        key: 'operateType',
        label: '操作类型',
        children: (
          <>
            {details?.operateType?.map((x) => {
              return (
                <Tag bordered={false} color="magenta" key={x}>
                  {/* eslint-disable-next-line @typescript-eslint/ban-ts-comment */}
                  {/* @ts-expect-error */}
                  {OperateType[x]}
                </Tag>
              );
            })}
          </>
        ),
      },
      {
        key: 'operateName',
        label: '操作名称',
        children: <p>{details?.operateName}</p>,
      },
      {
        key: 'IP',
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
        key: 'responseTime',
        label: '响应时间',
        children: <p>{details?.responseTime}</p>,
      },
      {
        key: 'duration',
        label: '耗时(毫秒)',
        children: <p>{details?.duration ? details?.duration?.toString() + 'ms' : ''}</p>,
      },
      {
        key: 'queryString',
        label: 'QueryString',
        children: <p>{details?.queryString}</p>,
      },
      {
        key: 'requestBody',
        label: '请求体',
        children: <p>{details?.requestBody}</p>,
      },
      {
        key: 'responseBody',
        label: '响应体',
        children: <p>{details?.responseBody}</p>,
      },
    ];
  }, [details]);

  return <div className='fancyx-table-wrapper'>
    <ProTable<ApiAccessLogItem, GetApiAccessLogListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetApiAccessLogListRequest
      ) => {
        const res = await getApiAccessLogList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
    />
    <Modal
      title="访问日志详情"
      open={isOpenModal}
      footer={null}
      width="60%"
      onCancel={() => {
        setIsOpenModal(false);
        setDetails(null);
      }}
    >
      <Descriptions items={items} column={2} size="small" />
    </Modal>
  </div>
}

export default ApiAccessLog;
