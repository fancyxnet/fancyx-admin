import Permission from '@/components/Permission';
import { deletePositionGroup, getPositionGroupList, type PositionGroupItem } from '@/api/organization/positionGroup';
import { DeleteOutlined, EditOutlined, ExclamationCircleFilled, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space } from 'antd';
import React, { useRef } from 'react';
import PositionGroupForm, { type ModalRef } from '@/pages/org/components/PositionGroupForm.tsx';
import SmartTable from '@/components/SmartTable';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import useApp from 'antd/es/app/useApp';

const PositionGroupList: React.FC = () => {
  const modalRef = useRef<ModalRef>(null);
  const tableRef = useRef<SmartTableRef>(null);
  const { message, modal } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '职位分组名称',
      dataIndex: 'groupName',
    },
    {
      title: '备注',
      dataIndex: 'remark',
    },
    {
      title: '排序值',
      dataIndex: 'sort',
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 140,
      fixed: 'right',
      render: (_: any, record: PositionGroupItem) => (
        <Space>
          <Permission permissions={'Org.PositionGroup.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              onClick={() => {
                rowEdit(record);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Org.PositionGroup.Delete'}>
            <Popconfirm
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                rowDelete(record.id);
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
  const handleOpenModal = () => {
    if (modalRef.current) {
      modalRef.current.openModal();
    }
  };

  const rowDelete = (id: string) => {
    modal.confirm({
      title: '确认删除？',
      icon: <ExclamationCircleFilled />,
      onOk() {
        deletePositionGroup(id).then(() => {
          message.success('删除成功');
          tableRef?.current?.reload();
        });
      },
    });
  };
  const rowEdit = (record: PositionGroupItem) => {
    modalRef.current?.openModal(record);
  };

  return (
    <>
      <SmartTable
        ref={tableRef}
        columns={columns}
        rowKey="id"
        request={async (params) => {
          const { data } = await getPositionGroupList(params);
          return {
            items: data,
            totalCount: data.length,
          };
        }}
        searchItems={
          <Form.Item label="职位分组名称" name="groupName">
            <Input placeholder="请输入职位分组名称" />
          </Form.Item>
        }
        toolbar={
          <Permission permissions={'Org.PositionGroup.Add'}>
            <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
              新增
            </Button>
          </Permission>
        }
      />
      {/* 职位分组新增/编辑弹窗 */}
      <PositionGroupForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
    </>
  );
};

export default PositionGroupList;
