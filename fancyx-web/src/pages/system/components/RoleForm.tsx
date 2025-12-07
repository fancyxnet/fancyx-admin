import { Form, Input, Modal, Switch } from 'antd';
import { forwardRef, useImperativeHandle, useState } from 'react';
import { addRole, type AddOrUpdateRoleRequest, updateRole } from '@/api/system/role';
import type { AppResponse } from '@/types/api';
import useApp from 'antd/es/app/useApp';

interface ModalProps {
  refresh?: () => void;
}

export interface ModalRef {
  openModal: (row?: AddOrUpdateRoleRequest) => void;
}

const RoleForm = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<AddOrUpdateRoleRequest | null>();
  const { message } = useApp();

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  const openModal = (row?: AddOrUpdateRoleRequest) => {
    setIsOpenModal(true);
    if (row) {
      setRow(row);
      form.setFieldsValue(row);
    } else {
      setRow(null);
      form.resetFields();
      form.setFieldValue('isEnabled', true);
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
    values: AddOrUpdateRoleRequest,
    apiAction: (params: AddOrUpdateRoleRequest) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: AddOrUpdateRoleRequest) => {
    const isEdit = !!row?.id;

    execute(values, isEdit ? updateRole : addRole, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      title={row?.id ? '编辑角色' : '新增角色'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
    >
      <Form<AddOrUpdateRoleRequest>
        name="wrap"
        labelCol={{ flex: '80px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="角色名" name="roleName" rules={[{ required: true }, { max: 64 }]}>
          <Input placeholder="请输入角色名" />
        </Form.Item>
        <Form.Item label="状态" name="isEnabled" rules={[{ required: true, message: '请选择角色状态' }]}>
          <Switch />
        </Form.Item>
        <Form.Item label="备注" name="remark" rules={[{ max: 512 }]}>
          <Input placeholder="请输入备注" allowClear />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default RoleForm;
