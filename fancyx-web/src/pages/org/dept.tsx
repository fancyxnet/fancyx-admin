import { deleteDept, getDeptList, type DeptItem, type GetDeptListRequest } from '@/api/organization/dept';
import { DeleteOutlined, EditOutlined, ExclamationCircleFilled, PlusOutlined } from '@ant-design/icons';
import { Button, Space } from 'antd';
import React, { useRef } from 'react';
import Permission from '@/components/Permission';
import DeptForm, { type DeptModalRef } from '@/pages/org/components/DeptForm.tsx';
import useApp from 'antd/es/app/useApp';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Department: React.FC = () => {
  const { message, modal } = useApp();
  const modalRef = useRef<DeptModalRef>(null);
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<DeptItem>[] = [
    {
      title: '关键词',
      key: 'keyword',
      hideInTable: true,
      hideInForm: false,
      valueType: 'text',
      fieldProps: {
        placeholder: '请输入部门名称或编码',
      },
    },
    {
      title: '部门名称',
      dataIndex: 'name',
      search: false,
    },
    {
      title: '部门编号',
      dataIndex: 'code',
      search: false,
    },
    {
      title: '部门邮箱',
      dataIndex: 'email',
      search: false,
    },
    {
      title: '部门电话',
      dataIndex: 'phone',
      search: false,
    },
    {
      title: '负责人',
      dataIndex: 'curatorName',
      search: false,
    },
    {
      title: '状态',
      dataIndex: 'status',
      render: (_: any, record: DeptItem) => {
        return record.status === 1 ? '正常' : '停用';
      },
      valueEnum: {
        1: { text: '正常' },
        2: { text: '停用' },
      },
      fieldProps: {
        placeholder: '请选择部门状态',
      },
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 140,
      fixed: 'right',
      search: false,
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
          actionRef?.current?.reload();
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
    <div className="fancyx-table-wrapper">
      <ProTable<DeptItem, GetDeptListRequest>
        actionRef={actionRef}
        rowKey="id"
        columns={columns}
        request={async (params: GetDeptListRequest) => {
          const res = await getDeptList(params);
          return {
            data: res.data,
            success: true,
          };
        }}
        toolBarRender={() => [
          <Permission permissions={'Org.Dept.Add'}>
            <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
              新增
            </Button>
          </Permission>,
        ]}
      />
      {/* 部门新增/编辑弹窗 */}
      <DeptForm ref={modalRef} refresh={() => actionRef?.current?.reload()} />
    </div>
  );
};

export default Department;
