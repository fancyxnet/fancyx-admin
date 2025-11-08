import Permission from '@/components/Permission';
import { deleteWarehouse, getWarehouseList, addWarehouse, updateWarehouse, type Warehouse, type WarehouseQueryDto } from '@/api/erp/warehouse.ts';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space } from 'antd';
import React, { useRef } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import useApp from 'antd/es/app/useApp';

const ConfigList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '编码',
      dataIndex: 'code',
    },
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '备注',
      dataIndex: 'remark',
    },
    {
      title: '是否启用',
      dataIndex: 'isEnabled',
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 210,
      fixed: 'right',
      render: (_: any, record: Warehouse) => (
        <Space>
          <Permission permissions={'Sys.Config.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Sys.Config.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteWarehouse(record.id!).then(() => {
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
          const { data } = await getWarehouseList(params);
          return data;
        }}
        searchItems={[
          <Form.Item label="仓库名称" name="name">
            <Input placeholder="请输入仓库名称" />
          </Form.Item>,
        ]}
        toolbar={
          <Space size="middle">
            <Permission permissions={'Sys.Config.Add'}>
              <Button
                type="primary"
                key="primary"
                onClick={() => {
                  
                }}
              >
                <PlusOutlined /> 新增
              </Button>
            </Permission>
          </Space>
        }
      />
      {/** 新增/编辑弹窗 */}
      
    </>
  );
};

export default ConfigList;
