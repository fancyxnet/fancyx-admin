import { Card, Divider, Form, message, Modal, Switch, Tag, Tree } from 'antd';
import { forwardRef, useImperativeHandle, useState } from 'react';
import { assignMenu, type AssignMenuDto, getRoleMenuIds, type RoleListDto } from '@/api/system/role.ts';
import { getMenuOptions, type MenuOptionTreeDto } from '@/api/system/menu.ts';

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface ModalProps {}

export interface AssignMenuModalRef {
  openModal: (row: RoleListDto) => void; // 定义 ref 的类型
}

const AssignMenuForm = forwardRef<AssignMenuModalRef, ModalProps>((_, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [menuOptions, setMenuOptions] = useState<MenuOptionTreeDto[]>([]);
  const [currentRow, setCurrentRow] = useState<RoleListDto>();
  const [roleMenuIds, setRoleMenuIds] = useState<string[] | null>();
  const [allKeys, setAllKeys] = useState<string[]>();
  const [expandKeys, setExpandKeys] = useState<string[]>();

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  const openModal = (row: RoleListDto) => {
    setCurrentRow(row);
    getMenuOptions(false).then(async (menuRes) => {
      setMenuOptions(menuRes.data.tree);
      setAllKeys(menuRes.data.keys);
      const { data } = await getRoleMenuIds(row!.id);
      setRoleMenuIds(data);
      setIsOpenModal(true);
    });
  };

  const onCancel = () => {
    form.resetFields();
    setIsOpenModal(false);
  };

  const onOk = () => {
    form.submit();
  };

  const onFinish = (values: AssignMenuDto) => {
    assignMenu({
      menuIds: values.menuIds,
      roleId: currentRow!.id!,
    }).then(() => {
      message.success('分配成功');
      setIsOpenModal(false);
      form.resetFields();
    });
  };
  const treeCheck = (checkKeys: string[]) => {
    form.setFieldValue('menuIds', checkKeys);
  };

  return (
    <Modal title="分配功能" open={isOpenModal} onCancel={onCancel} onOk={onOk} maskClosable={false} width="50%">
      <Form
        name="wrap"
        labelCol={{ flex: '80px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        style={{ maxWidth: 600 }}
        onFinish={onFinish}
      >
        <Form.Item label="角色名称" name="roleName">
          <Tag color="magenta">{currentRow?.roleName}</Tag>
        </Form.Item>
        <Form.Item<AssignMenuDto> label="菜单权限" name="menuIds">
          <Card size="small">
            <div className="flex align-center">
              <div className="mr-5">全部展开/折叠</div>
              <div>
                <Switch
                  checkedChildren="展开"
                  unCheckedChildren="折叠"
                  onClick={(checked: boolean) => {
                    if (checked) {
                      setExpandKeys(allKeys);
                    } else {
                      setExpandKeys([]);
                    }
                  }}
                />
              </div>
            </div>
            <Divider />
            <div
              style={{
                maxHeight: '400px',
                overflowY: 'auto',
              }}
            >
              <Tree
                checkable
                treeData={menuOptions}
                checkStrictly={true}
                expandedKeys={expandKeys}
                defaultCheckedKeys={roleMenuIds!}
                onCheck={({ checked }: any) => treeCheck(checked as string[])}
                onExpand={(expandKeys) => setExpandKeys(expandKeys as string[])}
              />
            </div>
          </Card>
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default AssignMenuForm;
