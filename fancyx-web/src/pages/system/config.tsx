import Permission from '@/components/Permission';
import { deleteConfig, getConfigList, type ConfigItem } from '@/api/system/config.ts';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space } from 'antd';
import React, { useRef, useState } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import ConfigForm from '@/pages/system/components/ConfigForm.tsx';
import useApp from 'antd/es/app/useApp';

const ConfigList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const { message } = useApp();
  const [rowId, setRowId] = useState<string | null>(null);
  const [modalVisit, setModalVisit] = useState<boolean>(false);
  const columns: SmartTableColumnType[] = [
    {
      title: '配置名称',
      dataIndex: 'name',
    },
    {
      title: '配置键名',
      dataIndex: 'key',
    },
    {
      title: '配置值',
      dataIndex: 'value',
    },
    {
      title: '组别',
      dataIndex: 'groupKey',
    },
    {
      title: '备注',
      dataIndex: 'Remark',
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
      render: (_: any, record: ConfigItem) => (
        <Space>
          <Permission permissions={'Sys.Config.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                setRowId(record.id);
                setModalVisit(true);
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
                deleteConfig(record.id!).then(() => {
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
          const { data } = await getConfigList(params);
          return data;
        }}
        searchItems={[
          <Form.Item label="配置名称" name="name">
            <Input placeholder="请输入配置名称" />
          </Form.Item>,
          <Form.Item label="配置键名" name="key">
            <Input placeholder="请输入配置键名" />
          </Form.Item>,
        ]}
        toolbar={
          <Space size="middle">
            <Permission permissions={'Sys.Config.Add'}>
              <Button
                type="primary"
                key="primary"
                onClick={() => {
                  setRowId(null);
                  setModalVisit(true);
                }}
              >
                <PlusOutlined /> 新增
              </Button>
            </Permission>
          </Space>
        }
      />
      {/** 新增/编辑配置弹窗 */}
      <ConfigForm modalVisit={modalVisit} id={rowId} callback={() => tableRef?.current?.reload()} onOpenChange={setModalVisit}/>
    </>
  );
};

export default ConfigList;
