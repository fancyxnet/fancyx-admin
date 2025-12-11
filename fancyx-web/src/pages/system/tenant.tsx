import Permission from '@/components/Permission';
import {
  deleteTenant,
  getTenantList,
  createTenantAccount,
  type AddOrUpdateTenantRequest,
  type TenantItem,
  type TenantAccountInfo,
  type GetTenantListRequest,
} from '@/api/system/tenant.ts';
import { DeleteOutlined, EditOutlined, HddOutlined, PlusOutlined, UserAddOutlined } from '@ant-design/icons';
import { Button, Modal, Popconfirm, Space } from 'antd';
import React, { useRef, useState } from 'react';
import TenantForm, { type ModalRef } from '@/pages/system/components/TenantForm.tsx';
import useApp from 'antd/es/app/useApp';
import AssignTenantForm, { type AssignTenantMenuFormModalRef } from './components/AssignTenantMenuForm';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Tenant: React.FC = () => {
  const modalRef = useRef<ModalRef>(null);
  const { message } = useApp();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [account, setAccount] = useState<TenantAccountInfo>();
  const actionRef = useRef<ActionType>();
  const [selectedKeys, setSelectedKeys] = useState<string[]>([])
  const columns: ProColumnType<TenantItem>[] = [
    {
      title: '关键词',
      key: 'keyword',
      hideInTable: true,
      hideInForm: false,
      valueType: 'text',
      fieldProps: {
        placeholder: '请输入租户名称/标识',
      },
    },
    {
      title: '租户名称',
      dataIndex: 'name',
      search: false,
    },
    {
      title: '租户标识',
      dataIndex: 'tenantId',
      search: false,
    },
    {
      title: '绑定域名',
      dataIndex: 'domain',
      search: false,
    },
    {
      title: '状态',
      dataIndex: 'isEnabled',
      search: false,
      render: (_: any, record: TenantItem) => (record.isEnabled ? '启用' : '禁用'),
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
      render: (_: any, record: TenantItem) => (
        <Space>
          <Permission permissions={'Sys.Tenant.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                modalRef?.current?.openModal(record as AddOrUpdateTenantRequest);
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
  const assignTenantFormRef = useRef<AssignTenantMenuFormModalRef>(null);


  return (<div className='fancyx-table-wrapper'>
    <ProTable<TenantItem, GetTenantListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetTenantListRequest
      ) => {
        const res = await getTenantList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      rowSelection={{
        onChange: (selectedRowKeys) => {
          setSelectedKeys(selectedRowKeys as string[]);
        },
      }}
      toolBarRender={
        () => [
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
            <Permission permissions={'Sys.Tenant.CreateTenantAccount'}>
              <Button
                key="createTenantAccount"
                variant="outlined"
                onClick={() => {
                  const keys = selectedKeys;
                  if (!keys || keys?.length === 0) {
                    message.warning('请先选择租户');
                    return;
                  }
                  createTenantAccount({ tenantId: keys[0] }).then((res) => {
                    message.success('初始管理员账号创建成功');
                    setAccount(res.data);
                    setIsModalOpen(true);
                  });
                }}
              >
                <UserAddOutlined /> 初始管理员账号
              </Button>
            </Permission>
          </Space>
        ]
      }
    />
    {/** 新增/编辑租户弹窗 */}
    <TenantForm ref={modalRef} refresh={() => actionRef?.current?.reload()} />
    {/* 分配功能权限 */}
    <AssignTenantForm ref={assignTenantFormRef} />
    <Modal
      title="租户管理员账号"
      open={isModalOpen}
      footer={null}
      onCancel={() => {
        setIsModalOpen(false);
      }}
    >
      <p>密码只展示一次，请截图保存。</p>
      <p className='mt-1'>角色：{account?.roleName}</p>
      <p>账号：{account?.userName}</p>
      <p>密码：{account?.password}</p>
    </Modal>
  </div>)
}

export default Tenant;
