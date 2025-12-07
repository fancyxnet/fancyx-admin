import { Form, Input, Modal } from 'antd';
import { forwardRef, useImperativeHandle, useState } from 'react';
import type { AppResponse } from '@/types/api';
import { addConfig, type AddOrUpdateConfigRequest, updateConfig } from '@/api/system/config.ts';
import useApp from 'antd/es/app/useApp';
import TextArea from 'antd/es/input/TextArea';

interface ModalProps {
  refresh?: () => void;
}

export interface ModalRef {
  openModal: (row?: AddOrUpdateConfigRequest) => void;
}

const ConfigForm = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<AddOrUpdateConfigRequest | null>();
  const { message } = useApp();

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  const openModal = (row?: AddOrUpdateConfigRequest) => {
    setIsOpenModal(true);
    if (row) {
      setRow(row);
      form.setFieldsValue(row);
    } else {
      setRow(null);
      form.resetFields();
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
    values: AddOrUpdateConfigRequest,
    apiAction: (params: AddOrUpdateConfigRequest) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: AddOrUpdateConfigRequest) => {
    const isEdit = !!row?.id;

    execute(values, isEdit ? updateConfig : addConfig, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      title={row?.id ? '编辑配置' : '新增配置'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
    >
      <Form<AddOrUpdateConfigRequest>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="组别" name="groupKey">
          <Input placeholder="组别" />
        </Form.Item>
        <Form.Item label="配置名称" name="name" rules={[{ required: true }, { max: 256 }]}>
          <Input placeholder="请输入配置名称" />
        </Form.Item>
        <Form.Item label="配置键名" name="key" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入配置键名" />
        </Form.Item>
        <Form.Item label="配置值" name="value" rules={[{ required: true }, { max: 1024 }]}>
          <Input placeholder="请输入配置值" />
        </Form.Item>
        <Form.Item label="备注" name="remark" rules={[{ max: 64 }]}>
          <TextArea placeholder="请输入备注" />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default ConfigForm;
