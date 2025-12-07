import { deleteDept, getDeptList, type DeptItem } from '@/api/organization/dept';
import { DeleteOutlined, EditOutlined, ExclamationCircleFilled, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Select, Space } from 'antd';
import React, { useRef } from 'react';
import Permission from '@/components/Permission';
import DeptForm, { type DeptModalRef } from '@/pages/org/components/DeptForm.tsx';
import type { SmartTableColumnType, SmartTableRef } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import useApp from 'antd/es/app/useApp';

const DepartmentList: React.FC = () => {
  const modalRef = useRef<DeptModalRef>(null);
  const tableRef = useRef<SmartTableRef>(null);
  const { message, modal } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '部门名称',
      dataIndex: 'name',
    },
    {
      title: '部门编号',
      dataIndex: 'code',
    },
    {
      title: '部门邮箱',
      dataIndex: 'email',
    },
    {
      title: '部门电话',
      dataIndex: 'phone',
    },
    {
      title: '负责人',
      dataIndex: 'curatorName',
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 140,
      fixed: 'right',
      render: (_: any, record: DeptItem) => (
        <Space>
          <Permission permissions={'Org.Dept.Update'}>
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
          <Permission permissions={'Org.Dept.Delete'}>
            <Button type="link" icon={<DeleteOutlined />} danger onClick={() => rowDelete(record.id)}>
              删除
            </Button>
          </Permission>
        </Space>
      ),
    },
  ];

  const rowDelete = (id: string) => {
    modal.confirm({
      title: '确认删除？',
      icon: <ExclamationCircleFilled />,
      onOk() {
        deleteDept(id).then(() => {
          message.success('删除成功');
          tableRef?.current?.reload();
        });
      },
    });
  };
  const rowEdit = (record: DeptItem) => {
    modalRef.current?.openModal(record);
  };
  const handleOpenModal = () => {
    if (modalRef.current) {
      modalRef.current.openModal();
    }
  };

  return (
    <>
      <SmartTable
        ref={tableRef}
        columns={columns}
        rowKey="id"
        request={async (params) => {
          const { data } = await getDeptList(params);
          return {
            items: data,
            totalCount: data.length,
          };
        }}
        searchItems={
          <>
            <Form.Item label="部门编码" name="code">
              <Input placeholder="请输入部门编码" />
            </Form.Item>
            <Form.Item label="部门名称" name="name">
              <Input placeholder="请输入部门名称" />
            </Form.Item>
            <Form.Item label="状态" name="status">
              <Select
                allowClear
                placeholder="请选择部门状态"
                options={[
                  { label: '正常', value: 1 },
                  { label: '停用', value: 2 },
                ]}
              />
            </Form.Item>
          </>
        }
        toolbar={
          <Permission permissions={'Org.Dept.Add'}>
            <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
              新增
            </Button>
          </Permission>
        }
      />
      {/* 部门新增/编辑弹窗 */}
      <DeptForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
    </>
  );
};

export default DepartmentList;
