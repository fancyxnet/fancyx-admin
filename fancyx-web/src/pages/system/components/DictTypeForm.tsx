import { Form, Input, Modal, Switch } from 'antd';
import { forwardRef, useImperativeHandle, useState } from 'react';
import type { AppResponse } from '@/types/api';
import { addDictType, type AddOrUpdateDictTypeRequest, updateDictType } from '@/api/system/dictType.ts';
import useApp from 'antd/es/app/useApp';

interface ModalProps {
  refresh?: () => void;
}

export interface ModalRef {
  openModal: (row?: AddOrUpdateDictTypeRequest) => void;
}

const DictTypeForm = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<AddOrUpdateDictTypeRequest | null>();
  const { message } = useApp();

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  const openModal = (row?: AddOrUpdateDictTypeRequest) => {
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
    values: AddOrUpdateDictTypeRequest,
    apiAction: (params: AddOrUpdateDictTypeRequest) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: AddOrUpdateDictTypeRequest) => {
    const isEdit = !!row?.id;

    execute(values, isEdit ? updateDictType : addDictType, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      title={row?.id ? '编辑字典' : '新增字典'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
    >
      <Form<AddOrUpdateDictTypeRequest>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="字典名称" name="name" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入字典名称" />
        </Form.Item>
        <Form.Item label="字典类型" name="dictType" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入字典类型" />
        </Form.Item>
        <Form.Item label="状态" name="isEnabled" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Form.Item label="备注" name="remark" rules={[{ max: 512 }]}>
          <Input placeholder="请输入备注" />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default DictTypeForm;
