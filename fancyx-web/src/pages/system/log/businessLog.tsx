﻿import { getBusinessLogList, getBusinessTypeOptions } from '@/api/system/log/businessLog';
import { Form, Input, Select, Tag, Tooltip } from 'antd';
import React, { useEffect, useState } from 'react';
import type { SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import type { AppOption } from '@/types/api';

const BusinessLogList: React.FC = () => {
  const [typeOptions, setTypeOptions] = useState<AppOption[]>();
  const columns: SmartTableColumnType[] = [
    {
      title: '业务类型',
      dataIndex: 'type',
      render: (text: string) => {
        return <Tag color="purple">{text}</Tag>;
      },
    },
    {
      title: '子类型',
      dataIndex: 'subType',
    },
    {
      title: '操作内容',
      dataIndex: 'content',
    },
    {
      title: '业务编号',
      dataIndex: 'bizNo',
      ellipsis: {
        showTitle: false,
      },
      render: (bizNo: string) => (
        <Tooltip placement="topLeft" title={bizNo}>
          {bizNo}
        </Tooltip>
      ),
    },
    {
      title: '操作用户',
      dataIndex: 'userName',
    },
    {
      title: '浏览器',
      dataIndex: 'browser',
    },
    {
      title: '操作时间',
      dataIndex: 'creationTime',
    },
  ];

  const fetchTypeOptions = (type?: string | null) => {
    getBusinessTypeOptions(type).then((res) => {
      setTypeOptions(res.data);
    });
  };

  useEffect(() => {
    fetchTypeOptions(null);
  }, []);

  return (
    <SmartTable
      columns={columns}
      rowKey="id"
      request={async (params) => {
        const { data } = await getBusinessLogList(params);
        return data;
      }}
      searchItems={
        <>
          <Form.Item label="业务类型" name="type">
            <Select placeholder="请选择业务类型" options={typeOptions} allowClear />
          </Form.Item>
          <Form.Item label="业务子类型" name="type">
            <Input placeholder="请输入业务子类型" />
          </Form.Item>
          <Form.Item label="操作用户" name="userName">
            <Input placeholder="请输入操作用户名" />
          </Form.Item>
        </>
      }
    />
  );
};

export default BusinessLogList;
