import { Form, Input, Modal, Radio, TreeSelect } from 'antd';
import { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import { addUser, getUserEditInfo, updateUser, type UserDto, type UserEditDto } from '@/api/system/user.ts';
import { Patterns } from '@/utils/globalValue.ts';
import useApp from 'antd/es/app/useApp';
import { type DeptListDto, getDeptList } from '@/api/organization/dept.ts';
import { getPositionOptions } from '@/api/organization/position.ts';
import type { AppOptionTree } from '@/types/api';

interface ModalProps {
  refresh?: () => void; // 定义 props 的类型
}

export interface ModalRef {
  openModal: (id?: string) => void; // 定义 ref 的类型
}

const UserModal = forwardRef<ModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const { message } = useApp();
  const [deptData, setDeptData] = useState<DeptListDto[]>([]);
  const [positionData, setPositionData] = useState<AppOptionTree[]>([]);
  const [id, setId] = useState<string>();

  useImperativeHandle(ref, () => ({
    openModal,
  }));
  useEffect(() => {
    if (isOpenModal) {
      fetchDeptData();
      fetchPositionData();
    }
  }, [isOpenModal]);

  const fetchDeptData = () => {
    getDeptList({}).then((res) => {
      setDeptData(res.data!);
    });
  };
  const fetchPositionData = () => {
    getPositionOptions().then((res) => {
      setPositionData(res.data!);
    });
  };

  const openModal = (id?: string) => {
    if (id && id.length > 0) {
      getUserEditInfo(id).then((res) => {
        form.setFieldsValue(res.data);
      });
    }
    setId(id);
    setIsOpenModal(true);
  };

  const onCancel = () => {
    form.resetFields();
    setIsOpenModal(false);
  };

  const onOk = () => {
    form.submit();
  };

  const onFinish = (values: UserDto | UserEditDto) => {
    if (id && id.length > 0) {
      const editDto = values as UserEditDto;
      editDto.id = id;
      updateUser(editDto).then(() => {
        message.success('编辑成功');
        setIsOpenModal(false);
        setId('');
        form.resetFields();
        props?.refresh?.();
      });
    } else {
      addUser(values as UserDto).then(() => {
        message.success('新增成功');
        setIsOpenModal(false);
        form.resetFields();
        props?.refresh?.();
      });
    }
  };
  const pwdPatternValidateItem = {
    pattern: Patterns.LoginPassword,
    message: '密码至少有一个字母和数字，长度6-16位，特殊字符 "~`!@#$%^&*()_-+={[}]|\\:;\\"\'<,>.?/',
  };

  return (
    <Modal
      title={id && id.length > 0 ? '编辑用户' : '新增用户'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      width={700}
      maskClosable={false}
    >
      <Form<UserDto>
        name="wrap"
        labelCol={{ flex: '80px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="账号" name="userName" rules={[{ required: true }, { max: 32 }, { min: 3 }]}>
          <Input placeholder="请输入账号" disabled={Boolean(id) && Boolean(id!.length)} />
        </Form.Item>
        {(id && id.length > 0) ?? (
          <Form.Item<UserDto> label="密码" name="password" rules={[{ required: true }, pwdPatternValidateItem]}>
            <Input.Password placeholder="请输入密码" />
          </Form.Item>
        )}
        <Form.Item label="昵称" name="nickName" rules={[{ required: true }, { max: 64 }]}>
          <Input placeholder="请输入昵称" />
        </Form.Item>
        <Form.Item label="性别" name="sex" rules={[{ required: true }]} initialValue={1}>
          <Radio.Group>
            <Radio value={1}> 男 </Radio>
            <Radio value={2}> 女 </Radio>
          </Radio.Group>
        </Form.Item>
        <Form.Item
          label="手机号"
          name="phone"
          rules={[
            {
              pattern: Patterns.Phone,
              message: '请输入正确的手机号格式',
            },
          ]}
        >
          <Input placeholder="请输入手机号" />
        </Form.Item>
        <Form.Item label="所属部门" name="deptId">
          <TreeSelect
            showSearch
            style={{ width: '100%' }}
            styles={{
              popup: {
                root: { maxHeight: 400, overflow: 'auto' },
              },
            }}
            placeholder="请选择所属部门"
            allowClear
            treeData={deptData}
            fieldNames={{
              label: 'name',
              value: 'id',
              children: 'children',
            }}
          />
        </Form.Item>
        <Form.Item label="担任职位" name="postId">
          <TreeSelect
            showSearch
            style={{ width: '100%' }}
            styles={{
              popup: {
                root: { maxHeight: 400, overflow: 'auto' },
              },
            }}
            placeholder="请选择担任职位"
            allowClear
            treeData={positionData}
          />
        </Form.Item>
      </Form>
    </Modal>
  );
});

export default UserModal;
