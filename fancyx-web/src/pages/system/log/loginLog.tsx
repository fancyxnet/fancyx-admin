import { getLoginLogList, type GetLoginLogListRequest, type LoginLogItem } from '@/api/system/log/loginLog';
import { Tag } from 'antd';
import React from 'react';
import { ProTable, type ProColumnType } from '@ant-design/pro-components';

const LoginLog: React.FC = () => {
  const columns: ProColumnType<LoginLogItem>[] = [
    {
      title: '账号',
      dataIndex: 'userName',
    },
    {
      title: 'IP',
      dataIndex: 'ip',
      search: false,
    },
    {
      title: '地址',
      dataIndex: 'address',
      search: false,
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
      search: false,
    },
    {
      title: '结果',
      dataIndex: 'isSuccess',
      search: false,
      render: (_: any, record: LoginLogItem) => {
        return record.isSuccess ? <Tag color="green">成功</Tag> : <Tag color="red">失败</Tag>;
      },
    },
    {
      title: '结果消息',
      search: false,
      dataIndex: 'operationMsg',
    },
    {
      title: '登录时间',
      search: false,
      dataIndex: 'creationTime',
    },
  ];

  return <div className='fancyx-table-wrapper'>
    <ProTable<LoginLogItem, GetLoginLogListRequest>
      rowKey="id"
      columns={columns}
      request={async (
        params: GetLoginLogListRequest
      ) => {
        const res = await getLoginLogList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
    />
  </div>
}

export default LoginLog;
