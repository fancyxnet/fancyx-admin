import { type ApiAccessLogItem, getApiAccessLogList } from '@/api/monitor/monitorLog.ts';
import { Button, Descriptions, Form, Input, Modal, Tag, Tooltip } from 'antd';
import React, { useMemo } from 'react';
import type { SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import { OperateType } from '@/utils/globalValue.ts';
import { FileTextOutlined } from '@ant-design/icons';

const BusinessLogList: React.FC = () => {
  const [isOpenModal, setIsOpenModal] = React.useState(false);
  const [details, setDetails] = React.useState<ApiAccessLogItem | null>();
  const columns: SmartTableColumnType[] = [
    {
      title: '请求时间',
      dataIndex: 'requestTime',
      width: 180,
    },
    {
      title: '请求地址',
      dataIndex: 'path',
    },
    {
      title: '请求方法',
      dataIndex: 'method',
      width: 120,
    },
    {
      title: '操作用户',
      dataIndex: 'userName',
    },
    {
      title: '操作类型',
      dataIndex: 'operateType',
      render: (operateType: number[] | null) => {
        return operateType?.map((x) => {
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
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
      ellipsis: {
        showTitle: false,
      },
      render: (browser: string) => (
        <Tooltip placement="topLeft" title={browser}>
          {browser}
        </Tooltip>
      ),
    },
    {
      title: '操作',
      dataIndex: 'operate',
      width: 70,
      fixed: 'right',
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
  return (
    <>
      <SmartTable
        columns={columns}
        rowKey="id"
        request={async (params) => {
          const { data } = await getApiAccessLogList(params);
          return data;
        }}
        searchItems={
          <>
            <Form.Item label="API" name="path">
              <Input placeholder="请输入API地址" />
            </Form.Item>
            <Form.Item label="访问用户" name="userName">
              <Input placeholder="请输入访问账号" />
            </Form.Item>
          </>
        }
        toolbar={<h5>注意：API访问日志只记录操作成功的，发生异常的请查看异常日志</h5>}
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
    </>
  );
};

export default BusinessLogList;
