import { Space, Form, Input, Button, Switch, Tag } from 'antd';
import { useRef } from 'react';
import { PlusOutlined, ExclamationCircleFilled, DeleteOutlined, EditOutlined } from '@ant-design/icons';
import MenuForm, { type ModalRef } from '@/pages/system/components/MenuForm.tsx';
import { deleteMenu, getMenuList, type MenuListDto } from '@/api/system/menu.ts';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type';
import SmartTable from '@/components/SmartTable';
import { MenuType } from '@/utils/globalValue.ts';
import useApp from 'antd/es/app/useApp';
import Permission from '@/components/Permission';

const MenuTable = () => {
  const modalRef = useRef<ModalRef>(null);
  const tableRef = useRef<SmartTableRef>(null);
  const { message, modal } = useApp();
  const columns: SmartTableColumnType[] = [
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
    },
    {
      title: '权限标识',
      dataIndex: 'permission',
      key: 'permission',
    },
    {
      title: '菜单类型',
      dataIndex: 'menuType',
      key: 'menuType',
      render: (text: number) => {
        if (text === MenuType.Folder) return <Tag>目录</Tag>;
        else if (text === MenuType.Menu) return <Tag color="magenta">菜单</Tag>;
        return <Tag color="blue">按钮</Tag>;
      },
    },
    {
      title: '显示状态',
      dataIndex: 'display',
      key: 'display',
      render: (text: boolean) => {
        return <Switch checked={text} />;
      },
    },
    {
      title: '操作',
      key: 'action',
      width: 140,
      fixed: 'right',
      render: (_: any, record: MenuListDto) => (
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
          tableRef?.current?.reload();
        });
      },
    });
  };
  const rowEdit = (record: MenuListDto) => {
    modalRef.current?.openModal(record);
  };
  const addSubItem = (record: MenuListDto) => {
    modalRef.current?.openModal(record, true);
  };

  const batchDelete = () => {
    const ids = tableRef?.current?.getSelectedKeys();
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
          tableRef?.current?.reload();
        });
      },
    });
  };

  return (
    <>
      <SmartTable
        columns={columns}
        ref={tableRef}
        rowKey="id"
        selection
        request={async (params) => {
          const { data } = await getMenuList(params);
          return {
            items: data,
            totalCount: data.length,
          };
        }}
        searchItems={[
          <Form.Item label="菜单名称" name="title">
            <Input placeholder="请输入菜单名称" />
          </Form.Item>,
          <Form.Item label="菜单路由" name="path">
            <Input placeholder="请输入菜单路由" />
          </Form.Item>,
        ]}
        toolbar={
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
        }
      />
      {/* 菜单新增/编辑弹窗 */}
      <MenuForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
    </>
  );
};
export default MenuTable;
