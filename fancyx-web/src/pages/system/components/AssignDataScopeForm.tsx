import { Card, Divider, Form, Modal, Select, Switch, Tag, Tree } from 'antd';
import { forwardRef, useImperativeHandle, useState } from 'react';
import { assignDataScope, type AssignDataScopeDto, type RoleListDto } from '@/api/system/role.ts';
import { getRoleDeptPowerInfo, type DeptTreeOptionDto } from '@/api/system/role.ts';
import useApp from 'antd/es/app/useApp';

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface ModalProps {}

export interface AssignDataScopeModalRef {
  openModal: (row: RoleListDto) => void; // 定义 ref 的类型
}

const AssignDataScopeForm = forwardRef<AssignDataScopeModalRef, ModalProps>((_, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [deptOptions, setDeptOptions] = useState<DeptTreeOptionDto[]>([]);
  const [currentRow, setCurrentRow] = useState<RoleListDto>();
  const [deptIds, setDeptIds] = useState<string[] | null>();
  const [allKeys, setAllKeys] = useState<string[]>();
  const [expandKeys, setExpandKeys] = useState<string[]>();
  const { message } = useApp();
  const [checkStrictly, setCheckStrictly] = useState<boolean>(true);
  const [deptPowerType, setDeptPowerType] = useState<number>(0);

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  const openModal = (row: RoleListDto) => {
    setCurrentRow(row);
    getRoleDeptPowerInfo(row.id).then(async (res) => {
      setDeptOptions(res.data.deptOptions);
      setAllKeys(res.data.powerInfo.allDeptIds);
      setDeptIds(res.data.powerInfo.deptIds);
      form.setFieldValue('deptPowerType', res.data.powerInfo.deptPowerType);
      setDeptPowerType(res.data.powerInfo.deptPowerType);
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
    assignDataScope({
      deptIds: deptIds ?? [],
      roleId: currentRow!.id!,
      deptPowerType: form.getFieldValue('deptPowerType'),
    }).then(() => {
      message.success('分配成功');
      setIsOpenModal(false);
      form.resetFields();
    });
  };
  const treeCheck = (checkKeys: string[], info: any) => {
    if (!checkStrictly) {
      setDeptIds(info.checkedNodes.map((node: any) => node.key));
      return;
    }
    setDeptIds(checkKeys);
  };

  return (
    <Modal title="分配数据权限" open={isOpenModal} onCancel={onCancel} onOk={onOk} maskClosable={false} width="50%">
      <Form
        labelCol={{ flex: '80px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
        onValuesChange={(changedValues) => {
          if (changedValues.deptPowerType) {
            setDeptPowerType(changedValues.deptPowerType);
          }
        }}
      >
        <Form.Item label="角色名称" name="roleName">
          <Tag color="magenta">{currentRow?.roleName}</Tag>
        </Form.Item>
        <Form.Item label="权限类型" name="deptPowerType">
          <Select
            placeholder="请选择权限类型"
            options={[
              { label: '全部', value: 0 },
              { label: '本部门', value: 1 },
              { label: '本部门及以下', value: 2 },
              { label: '指定部门', value: 3 },
              { label: '仅本人', value: 4 },
            ]}
          />
        </Form.Item>
        {deptPowerType === 3 && (
          <Form.Item<AssignDataScopeDto> label="数据权限" name="deptIds">
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
                  treeData={deptOptions}
                  checkStrictly={checkStrictly}
                  expandedKeys={expandKeys}
                  checkedKeys={deptIds ?? []}
                  onCheck={({ checked }: any, info) => treeCheck(checked, info)}
                  onExpand={(expandKeys) => setExpandKeys(expandKeys as string[])}
                />
              </div>
            </Card>
          </Form.Item>
        )}
      </Form>
    </Modal>
  );
});

export default AssignDataScopeForm;
