import { Card, Divider, Form, Modal, Switch, Tag, Tree } from 'antd';
import { forwardRef, useImperativeHandle, useState } from 'react';
import { assignMenu, type AssignMenuDto, getRoleMenuIds, type RoleListDto } from '@/api/system/role.ts';
import { getMenuOptions, type MenuOptionTreeDto } from '@/api/system/menu.ts';
import useApp from 'antd/es/app/useApp';

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
  const { message } = useApp();
  const [checkStrictly, setCheckStrictly] = useState<boolean>(true);

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

  const onFinish = () => {
    assignMenu({
      menuIds: roleMenuIds ?? [],
      roleId: currentRow!.id!,
    }).then(() => {
      message.success('分配成功');
      setIsOpenModal(false);
      form.resetFields();
    });
  };
  const treeCheck = (checkKeys: string[], info: any) => {
    if (!checkStrictly) {
      setRoleMenuIds(info.checkedNodes.map((node: any) => node.key));
      return;
    }
    setRoleMenuIds(checkKeys);
  };

  return (
    <Modal title="分配菜单权限" open={isOpenModal} onCancel={onCancel} onOk={onOk} maskClosable={false} width="50%">
      <Form
        labelCol={{ flex: '80px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
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
              <div className="ml-20 mr-5">父子关联</div>
              <div>
                <Switch
                  checkedChildren="是"
                  unCheckedChildren="否"
                  onClick={(checked: boolean) => {
                    setCheckStrictly(!checked);
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
                checkStrictly={checkStrictly}
                expandedKeys={expandKeys}
                checkedKeys={roleMenuIds ?? []}
                onCheck={({ checked }: any, info) => treeCheck(checked, info)}
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
