import { deletePosition, getPositionList } from '@/api/organization/position';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Popconfirm, Space } from 'antd';
import React, { useRef } from 'react';
import Permission from '@/components/Permission';
import type { GetPositionListRequest, PositionItem } from '@/api/organization/position';
import PositionForm, { type PositionModalRef } from '@/pages/org/components/PositionForm.tsx';
import SysDict from '@/components/SysDict';
import { DictType } from '@/utils/globalValue.ts';
import useApp from 'antd/es/app/useApp';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Position: React.FC = () => {
  const { message } = useApp();
  const modalRef = useRef<PositionModalRef>(null);
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<PositionItem>[] = [
    {
      title: '关键词',
      key: 'keyword',
      hideInTable: true,
      hideInForm: false,
      valueType: 'text',
      fieldProps: {
        placeholder: '请输入名称或编码',
      },
    },
    {
      title: '职位名称',
      dataIndex: 'name',
      search: false,
    },
    {
      title: '职位职级',
      dataIndex: 'level',
      render: (_: any, record: PositionItem) => {
        return <SysDict dictType={DictType.PositionLevel} value={record.level.toString()} isPlainText />;
      },
      renderFormItem: () => {
        return <SysDict
          dictType={DictType.PositionLevel}
          placeholder="请选择职位职级"
        />
      },
    },
    {
      title: '职位编码',
      dataIndex: 'code',
      search: false,
    },
    {
      title: '职位状态',
      dataIndex: 'status',
      render: (_: any, record: PositionItem) => {
        return record.status === 1 ? '正常' : '停用';
      },
      valueEnum: {
        1: { text: '正常' },
        0: { text: '停用' },
      },
      fieldProps: {
        placeholder: '请选择职位状态',
      },
    },
    {
      title: '所属层级',
      dataIndex: 'layerName',
      search: false,
    },
    {
      title: '备注',
      dataIndex: 'description',
      search: false,
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 140,
      fixed: 'right',
      search: false,
      render: (_: any, record: PositionItem) => (
        <Space>
          <Permission permissions={'Org.Position.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                rowEdit(record);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Org.Position.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                rowDelete(record.id);
              }}
            >
              <Button type="link" icon={<DeleteOutlined />} danger>
                删除
              </Button>
            </Popconfirm>
          </Permission>
        </Space>
      ),
    },
  ];

  const handleOpenModal = () => {
    if (modalRef.current) {
      modalRef.current.openModal();
    }
  };
  const rowDelete = (id: string) => {
    deletePosition(id).then(() => {
      message.success('删除成功');
      actionRef?.current?.reload();
    });
  };
  const rowEdit = (record: PositionItem) => {
    modalRef.current?.openModal(record);
  };

  return (<div className='fancyx-table-wrapper'>
    <ProTable<PositionItem, GetPositionListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetPositionListRequest
      ) => {
        const res = await getPositionList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      toolBarRender={
        () => [
          <Permission permissions={'Org.Position.Add'}>
            <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
              新增
            </Button>
          </Permission>
        ]
      }
    />
    {/* 职位新增/编辑弹窗 */}
    <PositionForm ref={modalRef} refresh={() => actionRef?.current?.reload()} />
  </div>)
}

export default Position;
