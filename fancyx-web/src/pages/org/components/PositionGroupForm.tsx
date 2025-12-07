import { Form, Input, InputNumber, Modal, TreeSelect } from 'antd';
import { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import {
  addPositionGroup,
  getPositionGroupList,
  type AddOrUpdatePositionGroupRequest,
  type PositionGroupItem,
  updatePositionGroup,
} from '@/api/organization/positionGroup';
import type { AppResponse } from '@/types/api';
import useApp from 'antd/es/app/useApp';
import TextArea from 'antd/es/input/TextArea';

interface ModalProps {
  refresh?: () => void;
}

export interface ModalRef {
  openModal: (row?: AddOrUpdatePositionGroupRequest) => void;
}

const RoleForm = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<AddOrUpdatePositionGroupRequest | null>();
  const [treeData, setTreeData] = useState<PositionGroupItem[]>([]);
  const { message } = useApp();

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  useEffect(() => {
    if (isOpenModal) {
      fetchTreeData();
    }
  }, [isOpenModal]);

  const fetchTreeData = (groupName?: string) => {
    getPositionGroupList({ groupName }).then((res) => {
      setTreeData(res.data!);
    });
  };

  const openModal = (row?: AddOrUpdatePositionGroupRequest) => {
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
    values: AddOrUpdatePositionGroupRequest,
    apiAction: (params: AddOrUpdatePositionGroupRequest) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: AddOrUpdatePositionGroupRequest) => {
    const isEdit = !!row?.id;

    execute(values, isEdit ? updatePositionGroup : addPositionGroup, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      title={row?.id ? '编辑职位分组' : '新增职位分组'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
    >
      <Form<AddOrUpdatePositionGroupRequest>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="分组名称" name="groupName" rules={[{ required: true }, { max: 64 }]}>
          <Input placeholder="请输入分组名称" />
        </Form.Item>
        <Form.Item label="上级分组" name="parentId">
          <TreeSelect
            showSearch
            style={{ width: '100%' }}
            styles={{
              popup: {
                root: { maxHeight: 400, overflow: 'auto' },
              },
            }}
            placeholder="请选择上级分组"
            allowClear
            treeDefaultExpandAll
            treeData={treeData}
            fieldNames={{
              label: 'groupName',
              value: 'id',
              children: 'children',
            }}
            filterTreeNode={false}
            onSearch={(value) => {
              fetchTreeData(value ? value : undefined);
            }}
          />
        </Form.Item>
        <Form.Item label="显示排序" name="sort">
          <InputNumber min={1} max={999} placeholder="排序值" />
        </Form.Item>
        <Form.Item label="备注" name="remark" rules={[{ max: 500 }]}>
          <TextArea placeholder="请输入备注" />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default RoleForm;
