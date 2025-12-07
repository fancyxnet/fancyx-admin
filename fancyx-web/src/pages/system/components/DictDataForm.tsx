import { Form, Input, InputNumber, Modal, Switch } from 'antd';
import { forwardRef, useImperativeHandle, useState } from 'react';
import type { AppResponse } from '@/types/api';
import { addDictData, type AddOrUpdateDictDataRequest, updateDictData } from '@/api/system/dictData.ts';
import { useParams } from 'react-router-dom';
import useApp from 'antd/es/app/useApp';
import TextArea from 'antd/es/input/TextArea';

interface ModalProps {
  refresh?: () => void;
}

export interface ModalRef {
  openModal: (row?: AddOrUpdateDictDataRequest) => void;
}

const DictDataForm = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<AddOrUpdateDictDataRequest | null>();
  const urlParams = useParams();
  const { message } = useApp();

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  const openModal = (row?: AddOrUpdateDictDataRequest) => {
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
    values: AddOrUpdateDictDataRequest,
    apiAction: (params: AddOrUpdateDictDataRequest) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: AddOrUpdateDictDataRequest) => {
    if (!urlParams?.dictType) {
      message.error('字典类型不能为空');
      return;
    }
    const isEdit = !!row?.id;

    values.dictType = urlParams!.dictType;
    execute(values, isEdit ? updateDictData : addDictData, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      title={row?.id ? '编辑字典' : '新增字典'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
    >
      <Form<AddOrUpdateDictDataRequest>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="字典标签" name="label" rules={[{ required: true }, { max: 256 }]}>
          <Input placeholder="请输入字典标签" />
        </Form.Item>
        <Form.Item label="字典键值" name="value" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入字典键值" />
        </Form.Item>
        <Form.Item label="状态" name="isEnabled">
          <Switch />
        </Form.Item>
        <Form.Item label="显示排序" name="sort" rules={[{ required: true }]}>
          <InputNumber min={1} max={999} placeholder="排序值" />
        </Form.Item>
        <Form.Item label="备注" name="remark" rules={[{ max: 512 }]}>
          <TextArea placeholder="请输入备注" />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default DictDataForm;
