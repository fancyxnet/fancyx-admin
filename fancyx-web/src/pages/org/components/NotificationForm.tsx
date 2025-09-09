import { Form, Input, Modal, Select } from 'antd';
import { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import type { AppResponse } from '@/types/api';
import { addNotification, type NotificationDto, updateNotification } from '@/api/organization/notification.ts';
import useApp from 'antd/es/app/useApp';
import TextArea from 'antd/es/input/TextArea';
import { getUserSimpleInfos, type UserSimpleInfoDto } from '@/api/system/user';

interface ModalProps {
  refresh?: () => void;
}

export interface ModalRef {
  openModal: (row?: NotificationDto) => void;
}

const NotificationForm = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<NotificationDto | null>();
  const { message } = useApp();
  const [userOptions, setUserOptions] = useState<UserSimpleInfoDto[]>([]);

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  useEffect(() => {
    if (isOpenModal) {
      fetchTreeData();
    }
  }, [isOpenModal]);

  const fetchTreeData = (keyword?: string) => {
    getUserSimpleInfos(keyword).then((res) => {
      setUserOptions(res.data!);
    });
  };

  const openModal = (row?: NotificationDto) => {
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
    values: NotificationDto,
    apiAction: (params: NotificationDto) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: NotificationDto) => {
    const isEdit = !!row?.id;

    execute(values, isEdit ? updateNotification : addNotification, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      title={row?.id ? '编辑通知' : '新增通知'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
    >
      <Form<NotificationDto>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="通知标题" name="title" rules={[{ required: true }, { max: 100 }]}>
          <Input placeholder="请输入通知标题" />
        </Form.Item>
        <Form.Item label="通知用户" name="userId" rules={[{ required: true }, { max: 500 }]}>
          <Select
            placeholder="请选择通知用户"
            showSearch
            style={{ width: '100%' }}
            options={userOptions.map((x) => ({ label: x.nickName, value: x.id }))}
            onSearch={(value) => {
              fetchTreeData(value ? value : undefined);
            }}
          />
        </Form.Item>
        <Form.Item label="通知内容" name="content">
          <TextArea placeholder="请输入通知内容" autoSize={{ minRows: 2 }} maxLength={500} showCount />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default NotificationForm;
