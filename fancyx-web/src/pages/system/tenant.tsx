import Permission from '@/components/Permission';
import { deleteTenant, getTenantList, type TenantDto, type TenantListDto } from '@/api/system/tenant.ts';
import { DeleteOutlined, EditOutlined, HddOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space } from 'antd';
import React, { useRef } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import TenantForm, { type ModalRef } from '@/pages/system/components/TenantForm.tsx';
import useApp from 'antd/es/app/useApp';
import AssignTenantForm, { type AssignTenantMenuFormModalRef } from './components/AssignTenantMenuForm';

const TenantList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const modalRef = useRef<ModalRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '租户名称',
      dataIndex: 'name',
    },
    {
      title: '租户标识',
      dataIndex: 'tenantId',
    },
    {
      title: '绑定域名',
      dataIndex: 'domain',
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
      render: (_: any, record: TenantListDto) => (
        <Space>
          <Permission permissions={'Sys.Tenant.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                modalRef?.current?.openModal(record as TenantDto);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Sys.Tenant.AssignTenantMenu'}>
            <Button
              type="link"
              icon={<HddOutlined />}
              key="assign"
              onClick={() => {
                assignTenantFormRef?.current?.openModal(record);
              }}
            >
              菜单
            </Button>
          </Permission>
          <Permission permissions={'Sys.Tenant.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteTenant(record.tenantId!).then(() => {
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
  const assignTenantFormRef = useRef<AssignTenantMenuFormModalRef>(null);

  return (
    <>
      <SmartTable
        columns={columns}
        ref={tableRef}
        rowKey="tenantId"
        request={async (params) => {
          const { data } = await getTenantList(params);
          return data;
        }}
        searchItems={[
          <Form.Item label="关键词" name="keyword">
            <Input placeholder="请输入租户名称/标识" />
          </Form.Item>,
        ]}
        toolbar={
          <Space size="middle">
            <Permission permissions={'Sys.Tenant.Add'}>
              <Button
                type="primary"
                key="primary"
                onClick={() => {
                  modalRef?.current?.openModal();
                }}
              >
                <PlusOutlined /> 新增
              </Button>
            </Permission>
          </Space>
        }
      />
      {/** 新增/编辑租户弹窗 */}
      <TenantForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
      {/* 分配功能权限 */}
      <AssignTenantForm ref={assignTenantFormRef} />
    </>
  );
};

export default TenantList;
