import { Space, Button, Switch, Tag } from 'antd';
import { useRef, useState } from 'react';
import { PlusOutlined, ExclamationCircleFilled, DeleteOutlined, EditOutlined } from '@ant-design/icons';
import MenuForm, { type ModalRef } from '@/pages/system/components/MenuForm.tsx';
import { deleteMenu, getMenuList, type GetMenuListRequest, type MenuItem } from '@/api/system/menu.ts';
import { MenuType } from '@/utils/globalValue.ts';
import useApp from 'antd/es/app/useApp';
import Permission from '@/components/Permission';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Menu: React.FC = () => {
  const { message, modal } = useApp();
  const modalRef = useRef<ModalRef>(null);
  const actionRef = useRef<ActionType>();
  const [selectedKeys, setSelectedKeys] = useState<string[]>([])
  const columns: ProColumnType<MenuItem>[] = [
    {
      title: '菜单名称',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: '路由地址',
      dataIndex: 'path',
      key: 'path',
    },
    {
      title: '组件地址',
      dataIndex: 'component',
      key: 'component',
      search: false,
    },
    {
      title: '权限标识',
      dataIndex: 'permission',
      key: 'permission',
      search: false,
    },
    {
      title: '菜单类型',
      dataIndex: 'menuType',
      key: 'menuType',
      search: false,
      render: (_: any, record: MenuItem) => {
        const text = record.menuType
        if (text === MenuType.Folder) return <Tag>目录</Tag>;
        else if (text === MenuType.Menu) return <Tag color="magenta">菜单</Tag>;
        return <Tag color="blue">按钮</Tag>;
      },
    },
    {
      title: '显示状态',
      dataIndex: 'display',
      key: 'display',
      search: false,
      render: (_: any, record: MenuItem) => {
        return <Switch checked={record.display} />;
      },
    },
    {
      title: '操作',
      key: 'action',
      width: 140,
      fixed: 'right',
      search: false,
      render: (_: any, record: MenuItem) => (
        <Space>
          {(record.menuType === MenuType.Folder || record.menuType === MenuType.Menu) && (
            <Permission permissions={'Sys.Menu.Add'}>
              <Button type="link" icon={<PlusOutlined />} onClick={() => addSubItem(record)}>
                子级
              </Button>
            </Permission>
          )}
          <Permission permissions={'Sys.Menu.Update'}>
            <Button type="link" icon={<EditOutlined />} onClick={() => rowEdit(record)}>
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Sys.Menu.Delete'}>
            <Button type="link" icon={<DeleteOutlined />} danger onClick={() => dataDelete([record.id])}>
              删除
            </Button>
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
  const dataDelete = (ids: string[]) => {
    modal.confirm({
      title: '确认删除？',
      icon: <ExclamationCircleFilled />,
      onOk() {
        deleteMenu(ids).then(() => {
          message.success('删除成功');
          actionRef?.current?.reload();
        });
      },
    });
  };
  const rowEdit = (record: MenuItem) => {
    modalRef.current?.openModal(record);
  };
  const addSubItem = (record: MenuItem) => {
    modalRef.current?.openModal(record, true);
  };

  const batchDelete = () => {
    const ids = selectedKeys;
    if (!ids || !ids.length) {
      message.warning('请选择一条数据进行操作');
      return;
    }
    modal.confirm({
      title: `确认删除选中的${ids!.length}条数据？`,
      icon: <ExclamationCircleFilled />,
      onOk() {
        deleteMenu(ids as string[]).then(() => {
          message.success('删除成功');
          actionRef?.current?.reload();
        });
      },
    });
  };

  return (<div className='fancyx-table-wrapper'>
    <ProTable<MenuItem, GetMenuListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetMenuListRequest
      ) => {
        const res = await getMenuList(params);
        return {
          data: res.data,
          success: true,
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
            <Permission permissions={'Sys.Menu.Add'}>
              <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
                新增
              </Button>
            </Permission>
            <Permission permissions={'Sys.Menu.Delete'}>
              <Button color="danger" variant="solid" icon={<DeleteOutlined />} onClick={batchDelete}>
                删除
              </Button>
            </Permission>
          </Space>
        ]
      }
    />
    {/* 菜单新增/编辑弹窗 */}
    <MenuForm ref={modalRef} refresh={() => actionRef?.current?.reload()} />
  </div>)
}

export default Menu;
