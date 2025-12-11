import { Space, Button, Tag, Dropdown } from 'antd';
import { useRef } from 'react';
import {
  PlusOutlined,
  ExclamationCircleFilled,
  EditOutlined,
  HddOutlined,
  DeleteOutlined,
  DoubleRightOutlined,
  PieChartOutlined,
} from '@ant-design/icons';
import { deleteRole, getRoleList, type GetRoleListRequest, type RoleItem } from '@/api/system/role';
import RoleForm, { type ModalRef } from '@/pages/system/components/RoleForm.tsx';
import AssignMenuForm, { type AssignMenuModalRef } from '@/pages/system/components/AssignMenuForm.tsx';
import useApp from 'antd/es/app/useApp';
import Permission from '@/components/Permission';
import { useAuthProvider } from '@/components/AuthProvider';
import AssignDataScopeForm, { type AssignDataScopeModalRef } from './components/AssignDataScopeForm';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Role: React.FC = () => {
  const modalRef = useRef<ModalRef>(null);
  const assignMenuForRef = useRef<AssignMenuModalRef>(null);
  const assignDataScopeForRef = useRef<AssignDataScopeModalRef>(null);
  const actionRef = useRef<ActionType>();
  const { message, modal } = useApp();
  const { hasPermission } = useAuthProvider();
  const columns: ProColumnType<RoleItem>[] = [
    {
      title: '角色名',
      dataIndex: 'roleName',
      key: 'roleName',
    },
    {
      title: '状态',
      dataIndex: 'isEnabled',
      search: false,
      render: (_: any, record: RoleItem) => {
        if (record.isEnabled) {
          return <Tag color="success">启用</Tag>;
        }
        return <Tag>禁用</Tag>;
      },
    },
    {
      title: '备注',
      dataIndex: 'remark',
      key: 'remark',
      search: false,
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
      search: false,
    },
    {
      title: '操作',
      key: 'action',
      width: 210,
      fixed: 'right',
      search: false,
      render: (_: any, record: RoleItem) => {
        const curDropdownItems = [];
        if (hasPermission!('Sys.Role.AssignMenu')) {
          curDropdownItems.push({
            key: 'assignMenu',
            label: (
              <a onClick={() => openAssignModal(record)}>
                <HddOutlined className="mr-4" />
                功能权限
              </a>
            ),
            onClick: () => { },
          });
        }
        if (hasPermission!('Sys.Role.AssignDataScope')) {
          curDropdownItems.push({
            key: 'assignDataScope',
            label: (
              <a onClick={() => openAssignDataScopeModal(record)}>
                <PieChartOutlined className="mr-4" />
                数据权限
              </a>
            ),
            onClick: () => { },
          });
        }
        return (
          <Space>
            <Permission permissions={'Sys.Role.Update'}>
              <Button type="link" icon={<EditOutlined />} onClick={() => rowEdit(record)}>
                编辑
              </Button>
            </Permission>
            <Permission permissions={'Sys.Role.Delete'}>
              <Button type="link" icon={<DeleteOutlined />} danger onClick={() => rowDelete(record.id)}>
                删除
              </Button>
            </Permission>
            <Permission permissions={['Sys.Role.AssignMenu', 'Sys.Role.DataScope']} mode="some">
              <Dropdown
                placement="bottom"
                menu={{
                  items: curDropdownItems,
                }}
              >
                <Button type="link" icon={<DoubleRightOutlined />}>
                  更多
                </Button>
              </Dropdown>
            </Permission>
          </Space>
        );
      },
    },
  ];

  const openAssignModal = (row: RoleItem) => {
    assignMenuForRef?.current?.openModal(row);
  };
  const openAssignDataScopeModal = (row: RoleItem) => {
    assignDataScopeForRef?.current?.openModal(row);
  };

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
        deleteRole(id).then(() => {
          message.success('删除成功');
          actionRef?.current?.reload();
        });
      },
    });
  };
  const rowEdit = (record: RoleItem) => {
    modalRef.current?.openModal(record);
  };

  return (<div className='fancyx-table-wrapper'>
    <ProTable<RoleItem, GetRoleListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetRoleListRequest
      ) => {
        const res = await getRoleList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      toolBarRender={
        () => [
          <Permission permissions={'Sys.Role.Add'}>
            <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
              新增
            </Button>
          </Permission>
        ]
      }
    />
    {/* 角色新增/编辑弹窗 */}
    <RoleForm ref={modalRef} refresh={() => actionRef?.current?.reload()} />
    {/* 分配功能权限 */}
    <AssignMenuForm ref={assignMenuForRef} />
    {/* 分配数据权限 */}
    <AssignDataScopeForm ref={assignDataScopeForRef} />
  </div>)
}

export default Role;
