import Permission from '@/components/Permission';
import { deleteConfig, getConfigList, type ConfigItem, type GetConfigListRequest } from '@/api/system/config.ts';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Popconfirm, Space } from 'antd';
import React, { useRef, useState } from 'react';
import ConfigForm from '@/pages/system/components/ConfigForm.tsx';
import useApp from 'antd/es/app/useApp';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Config: React.FC = () => {
  const { message } = useApp();
  const [rowId, setRowId] = useState<string | null>(null);
  const [modalVisit, setModalVisit] = useState<boolean>(false);
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<ConfigItem>[] = [
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
      search: false,
    },
    {
      title: '组别',
      dataIndex: 'groupKey',
      search: false,
    },
    {
      title: '备注',
      dataIndex: 'Remark',
      search: false,
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
      search: false,
    },
    {
      title: '上次修改时间',
      dataIndex: 'lastModificationTime',
      search: false,
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 210,
      fixed: 'right',
      search: false,
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
                  actionRef.current?.reload();
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

  return (<div className='fancyx-table-wrapper'>
    <ProTable<ConfigItem, GetConfigListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetConfigListRequest
      ) => {
        const res = await getConfigList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      toolBarRender={
        () => [
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
        ]
      }
    />
    {/** 新增/编辑配置弹窗 */}
    <ConfigForm modalVisit={modalVisit} id={rowId} callback={() => actionRef?.current?.reload()} onOpenChange={setModalVisit} />
  </div>)
}

export default Config;
