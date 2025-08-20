import { Form, Input, InputNumber, Modal, Radio, Switch, TreeSelect } from 'antd';
import { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import type { AppResponse } from '@/types/api';
import {
  addMenu,
  getMenuOptions,
  type MenuDto,
  type MenuListDto,
  type MenuOptionTreeDto,
  updateMenu,
} from '@/api/system/menu';
import { MenuType } from '@/utils/globalValue.ts';
import useApp from 'antd/es/app/useApp';

interface ModalProps {
  refresh?: () => void;
}

export interface ModalRef {
  openModal: (row?: MenuListDto, isAddSub?: boolean) => void;
}

const MenuForm = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<MenuListDto | null>();
  const [treeData, setTreeData] = useState<MenuOptionTreeDto[]>([]);
  const [menuType, setMenuType] = useState<number>(1);
  const { message } = useApp();

  useImperativeHandle(ref, () => ({
    openModal,
  }));
  const fetchMenuOptions = (keywords?: string) => {
    getMenuOptions(true, keywords).then((res) => {
      if (res.data) {
        setTreeData(res.data.tree);
      }
    });
  };
  useEffect(() => {
    if (isOpenModal) {
      fetchMenuOptions();
    }
  }, [isOpenModal]);

  const openModal = (row?: MenuListDto, isAddSub?: boolean) => {
    setIsOpenModal(true);
    if (row && !isAddSub) {
      setRow(row);
      setMenuType(row.menuType);
      form.setFieldsValue(row);
    } else {
      setRow(null);
      const _menuType = isAddSub
        ? row?.menuType === MenuType.Folder
          ? MenuType.Menu
          : MenuType.Button
        : MenuType.Folder;
      setMenuType(_menuType);
      form.setFieldsValue({
        menuType: _menuType,
        sort: 0,
        display: true,
        parentId: isAddSub ? row?.id : null,
      });
    }
  };

  const onCancel = () => {
    form.resetFields();
    setIsOpenModal(false);
  };

  const onOk = () => {
    form.submit();
  };

  const execute = (
    values: MenuDto,
    apiAction: (params: MenuDto) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: MenuDto) => {
    const isEdit = !!row?.id;

    execute(values, isEdit ? updateMenu : addMenu, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      title={row?.id ? '编辑菜单' : '新增菜单'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      width="50%"
      maskClosable={false}
    >
      <Form<MenuDto>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="上级菜单" name="parentId">
          <TreeSelect
            showSearch
            style={{ width: '100%' }}
            styles={{
              popup: {
                root: { maxHeight: 400, overflow: 'auto' },
              },
            }}
            placeholder="请选择上级菜单"
            allowClear
            treeData={treeData}
            fieldNames={{
              label: 'title',
              value: 'key',
            }}
            filterTreeNode={false}
            onSearch={(value) => {
              fetchMenuOptions(value ? value : undefined);
            }}
          />
        </Form.Item>
        <Form.Item label="菜单名称" name="title" rules={[{ required: true }, { max: 32 }]}>
          <Input placeholder="请输入菜单名称" />
        </Form.Item>
        <Form.Item label="菜单类型" name="menuType" rules={[{ required: true }]}>
          <Radio.Group
            buttonStyle="solid"
            onChange={(e) => {
              setMenuType(e.target.value);
            }}
          >
            <Radio.Button value={1}>目录</Radio.Button>
            <Radio.Button value={2}>菜单</Radio.Button>
            <Radio.Button value={3}>按钮</Radio.Button>
          </Radio.Group>
        </Form.Item>
        {(menuType === MenuType.Folder || menuType === MenuType.Menu) && (
          <>
            <Form.Item label="菜单图标" name="icon" rules={[{ max: 64 }]}>
              <Input placeholder="请输入菜单图标" />
            </Form.Item>
            <Form.Item label="路由地址" name="path" rules={[{ required: true }, { max: 256 }]}>
              <Input placeholder="请输入路由地址" />
            </Form.Item>
            {menuType === MenuType.Menu && (
              <Form.Item label="组件地址" name="component" rules={[{ required: true }, { max: 256 }]}>
                <Input placeholder="请输入组件地址" />
              </Form.Item>
            )}
          </>
        )}
        {menuType === MenuType.Button && (
          <Form.Item label="权限标识" name="permission" rules={[{ max: 128 }]}>
            <Input placeholder="请输入权限标识" />
          </Form.Item>
        )}
        <Form.Item label="显示排序" name="sort" rules={[{ required: true }]}>
          <InputNumber min={1} max={999} placeholder="排序值" />
        </Form.Item>
        {menuType === MenuType.Menu && (
          <Form.Item label="是否外链" name="isExternal">
            <Switch checkedChildren="外链" unCheckedChildren="内部" />
          </Form.Item>
        )}
        <Form.Item label="显示状态" name="display">
          <Switch checkedChildren="显示" unCheckedChildren="隐藏" />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default MenuForm;
