import { Form, Input, InputNumber, Modal, Switch, TreeSelect } from 'antd';
import { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import { addDept, getDeptList, type DeptDto, type DeptListDto, updateDept } from '@/api/organization/dept';
import type { AppResponse } from '@/types/api';
import useApp from 'antd/es/app/useApp';

interface ModalProps {
  refresh?: () => void;
}

export interface DeptModalRef {
  openModal: (row?: DeptDto) => void;
}

const DeptForm = forwardRef<DeptModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<DeptDto | null>();
  const [treeData, setTreeData] = useState<DeptListDto[]>([]);
  const { message } = useApp();

  useImperativeHandle(ref, () => ({
    openModal,
  }));

  useEffect(() => {
    if (isOpenModal) {
      fetchTreeData();
    }
  }, [isOpenModal]);

  const fetchTreeData = () => {
    getDeptList({}).then((res) => {
      setTreeData(res.data!);
    });
  };

  const openModal = (row?: DeptDto) => {
    setIsOpenModal(true);
    if (row) {
      setRow(row);
      form.setFieldsValue(row);
    } else {
      setRow(null);
      form.resetFields();
      form.setFieldValue('status', 1);
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
    values: DeptDto,
    apiAction: (params: DeptDto) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: DeptDto) => {
    const isEdit = !!row?.id;

    execute(values, isEdit ? updateDept : addDept, isEdit ? '编辑成功' : '新增成功');
  };

  return (
    <Modal
      width="50%"
      title={row?.id ? '编辑部门' : '新增部门'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
    >
      <Form<DeptDto>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="上级部门" name="parentId">
          <TreeSelect
            showSearch
            style={{ width: '100%' }}
            styles={{
              popup: {
                root: { maxHeight: 400, overflow: 'auto' },
              },
            }}
            placeholder="请选择上级部门"
            allowClear
            treeDefaultExpandAll
            treeData={treeData}
            fieldNames={{
              label: 'name',
              value: 'id',
              children: 'children',
            }}
          />
        </Form.Item>
        <Form.Item label="部门名称" name="name" rules={[{ required: true }]}>
          <Input placeholder="请输入部门名称" />
        </Form.Item>
        <Form.Item label="部门编号" name="code" rules={[{ required: true }]}>
          <Input placeholder="请输入部门编号" />
        </Form.Item>
        <Form.Item label="部门状态" name="status" rules={[{ required: true }]}>
          <Switch
            value={row?.status === 1}
            unCheckedChildren="停用"
            checkedChildren="正常"
            onChange={(val) => {
              form.setFieldValue('status', val ? 1 : 2);
            }}
          />
        </Form.Item>
        <Form.Item label="部门描述" name="description">
          <Input placeholder="请输入部门描述" />
        </Form.Item>
        <Form.Item label="显示排序" name="sort">
          <InputNumber min={1} max={999} placeholder="排序值" />
        </Form.Item>
        <Form.Item label="部门邮箱" name="email">
          <Input placeholder="请输入部门邮箱" />
        </Form.Item>
        <Form.Item label="部门电话" name="phone">
          <Input placeholder="请输入部门电话" />
        </Form.Item>
        <Form.Item label="部门负责人" name="curatorId">
          <Input placeholder="请输入部门负责人" />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default DeptForm;
