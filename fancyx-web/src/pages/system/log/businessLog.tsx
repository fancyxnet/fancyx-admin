import { getBusinessLogList, getBusinessTypeOptions, type BusinessLogItem, type GetBusinessLogListRequest } from '@/api/system/log/businessLog';
import { Tag } from 'antd';
import React from 'react';
import { ProTable, type ProColumnType } from '@ant-design/pro-components';

const BusinessLog: React.FC = () => {
  const columns: ProColumnType<BusinessLogItem>[] = [
    {
      title: '业务类型',
      dataIndex: 'type',
      render: (_: any, record: BusinessLogItem) => {
        return <Tag color="purple">{record.type}</Tag>;
      },
      request: async () => {
        const res = await getBusinessTypeOptions();
        return res.data
      },
      fieldProps: {
        placeholder: '请选择业务类型',
        allowClear: true,
      },
    },
    {
      title: '子类型',
      dataIndex: 'subType',
    },
    {
      title: '操作内容',
      dataIndex: 'content',
      minWidth: 180,
      ellipsis: true,
      search: false,
    },
    {
      title: '业务编号',
      dataIndex: 'bizNo',
      ellipsis: true,
      search: false,
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
      search: false,
      ellipsis: true
    },
    {
      title: '跟踪ID',
      dataIndex: 'traceId',
      ellipsis: true,
      search: false,
    },
    {
      title: '操作时间',
      dataIndex: 'creationTime',
      search: false,
    },
    {
      title: '操作用户',
      dataIndex: 'userName',
    },
  ];

  return <div className='fancyx-table-wrapper'>
    <ProTable<BusinessLogItem, GetBusinessLogListRequest>
      rowKey="id"
      columns={columns}
      request={async (
        params: GetBusinessLogListRequest
      ) => {
        const res = await getBusinessLogList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
    />
  </div>
}

export default BusinessLog;
