import { deletePosition, getPositionList } from '@/api/organization/position';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space } from 'antd';
import React, { useRef } from 'react';
import Permission from '@/components/Permission';
import type { PositionItem } from '@/api/organization/position';
import PositionForm, { type PositionModalRef } from '@/pages/org/components/PositionForm.tsx';
import SmartTable from '@/components/SmartTable';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SysDict from '@/components/SysDict';
import { DictType } from '@/utils/globalValue.ts';
import useApp from 'antd/es/app/useApp';

const Position: React.FC = () => {
  const modalRef = useRef<PositionModalRef>(null);
  const tableRef = useRef<SmartTableRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '职位名称',
      dataIndex: 'name',
    },
    {
      title: '职位职级',
      dataIndex: 'level',
      render: (text: number) => {
        return <SysDict dictType={DictType.PositionLevel} value={text.toString()} isPlainText />;
      },
    },
    {
      title: '职位编码',
      dataIndex: 'code',
    },
    {
      title: '职位状态',
      dataIndex: 'status',
      render: (_: number, record: PositionItem) => {
        return record.status === 1 ? '正常' : '停用';
      },
    },
    {
      title: '所属层级',
      dataIndex: 'layerName',
    },
    {
      title: '备注',
      dataIndex: 'description',
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 140,
      fixed: 'right',
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
      tableRef?.current?.reload();
    });
  };
  const rowEdit = (record: PositionItem) => {
    modalRef.current?.openModal(record);
  };

  return (
    <>
      <SmartTable
        ref={tableRef}
        columns={columns}
        rowKey="id"
        request={async (params) => {
          const { data } = await getPositionList(params);
          return data;
        }}
        searchItems={[
          <Form.Item label="关键词" name="keyword">
            <Input placeholder="请输入职位名称/编号" />
          </Form.Item>,
          <Form.Item label="职级" name="level">
            <SysDict
              dictType={DictType.PositionLevel}
              placeholder="请选择职位职级"
              style={{
                width: '150px',
              }}
            />
          </Form.Item>,
          <Form.Item label="状态" name="status">
            <Input placeholder="请输入状态" />
          </Form.Item>,
        ]}
        toolbar={
          <Permission permissions={'Org.Position.Add'}>
            <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
              新增
            </Button>
          </Permission>
        }
      />
      {/* 职位新增/编辑弹窗 */}
      <PositionForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
    </>
  );
};

export default Position;
