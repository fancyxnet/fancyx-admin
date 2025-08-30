import { Form, Input, Radio, Modal, Row, Col, DatePicker, TreeSelect, Checkbox, Tooltip, Tag } from 'antd';
import { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import {
  addEmployee,
  type EmployeeDto,
  type EmployeeInfoDto,
  type EmployeeListDto,
  getEmployeeInfo,
  updateEmployee,
} from '@/api/organization/employee.ts';
import type { AppOptionTree, AppResponse } from '@/types/api';
import { type DeptListDto, getDeptList } from '@/api/organization/dept.ts';
import { getPositionOptions } from '@/api/organization/position.ts';
import { Patterns } from '@/utils/globalValue.ts';
import { InfoCircleOutlined } from '@ant-design/icons';
import useApp from 'antd/es/app/useApp';
import dayjs from 'dayjs';

interface ModalProps {
  refresh?: () => void;
}

export interface EmployeeModalRef {
  openModal: (row?: EmployeeListDto) => void;
}

const EmployeeForm = forwardRef<EmployeeModalRef, ModalProps>((props, ref) => {
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [form] = Form.useForm();
  const [row, setRow] = useState<EmployeeInfoDto | null>();
  const [employeeStatus, setEmployeeStatus] = useState<number>();
  const [deptData, setDeptData] = useState<DeptListDto[]>([]);
  const [positionData, setPositionData] = useState<AppOptionTree[]>([]);
  const [loading, setLoading] = useState(false);
  const [isAddUser, setIsAddUser] = useState(false);
  const { message } = useApp();

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

  const openModal = (row?: EmployeeListDto) => {
    setIsOpenModal(true);
    if (row) {
      setLoading(true);
      getEmployeeInfo(row.id).then((res) => {
        setLoading(false);
        setRow(res.data);
        setEmployeeStatus(res.data.status);
        form.setFieldsValue(res.data);
        if (res.data.inTime) {
          form.setFieldValue('inTime', dayjs(res.data.inTime));
        }
        if (res.data.outTime) {
          form.setFieldValue('outTime', dayjs(res.data.outTime));
        }
      });
    } else {
      setRow(null);
      form.resetFields();
      form.setFieldValue('status', 1);
      setEmployeeStatus(1);
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
    values: EmployeeDto,
    apiAction: (params: EmployeeDto) => Promise<AppResponse<boolean>>,
    successMsg: string,
  ) => {
    apiAction({ ...values, id: row?.id }).then(() => {
      message.success(successMsg);
      setIsOpenModal(false);
      form.resetFields();
      props?.refresh?.();
    });
  };
  const onFinish = (values: EmployeeDto) => {
    const isEdit = !!row?.id;
    let params = values;
    if (!isEdit) {
      params = { ...params, isAddUser: isAddUser };
    }

    execute(params, isEdit ? updateEmployee : addEmployee, isEdit ? '编辑成功' : '新增成功');
  };
  const pwdPatternValidateItem = {
    pattern: Patterns.LoginPassword,
    message: '密码至少有一个字母和数字，长度6-16位，特殊字符 "~`!@#$%^&*()_-+={[}]|\\:;\\"\'<,>.?/',
  };

  return (
    <Modal
      width="60%"
      title={row?.id ? '编辑员工' : '新增员工'}
      open={isOpenModal}
      onCancel={onCancel}
      onOk={onOk}
      maskClosable={false}
      loading={loading}
    >
      <Form<EmployeeDto>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
        onValuesChange={(changedValues) => {
          if (changedValues.status) {
            setEmployeeStatus(changedValues.status);
          }
        }}
      >
        <Row>
          <Col span={12}>
            <Form.Item label="姓名" name="name" rules={[{ required: true }, { max: 64 }]}>
              <Input placeholder="请输入姓名" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="工号" name="code" rules={[{ required: true }, { max: 64 }]}>
              <Input placeholder="请输入工号" />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col span={12}>
            <Form.Item label="性别" name="sex" rules={[{ required: true, message: '请选择性别' }]}>
              <Radio.Group
                options={[
                  { label: '男', value: 1 },
                  { label: '女', value: 2 },
                ]}
              />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="手机号" name="phone" rules={[{ required: true }, { max: 16 }]}>
              <Input placeholder="请输入手机号" />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col span={12}>
            <Form.Item label="状态" name="status" rules={[{ required: true }]}>
              <Radio.Group
                options={[
                  { label: '在职', value: 1 },
                  { label: '离职', value: 2 },
                ]}
              />
            </Form.Item>
          </Col>
          <Col span={12}>
            {employeeStatus === 1 && (
              <Form.Item label="入职时间" name="inTime">
                <DatePicker placeholder="请选择入职时间" />
              </Form.Item>
            )}
            {employeeStatus === 2 && (
              <Form.Item label="离职时间" name="outTime">
                <DatePicker placeholder="请选择离职时间" />
              </Form.Item>
            )}
          </Col>
        </Row>
        <Row>
          <Col span={12}>
            <Form.Item label="身份证号" name="idNo" rules={[{ max: 32 }]}>
              <Input placeholder="请输入身份证号" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="生日" name="birthday">
              <Input placeholder="输入身份证号后自动填入" disabled />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col span={12}>
            <Form.Item label="邮箱" name="email" rules={[{ max: 64 }]}>
              <Input placeholder="请输入邮箱" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item label="现住址" name="address" rules={[{ max: 512 }]}>
              <Input placeholder="请输入现住址" maxLength={512} />
            </Form.Item>
          </Col>
        </Row>
        <Row>
          <Col span={12}>
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
          </Col>
          <Col span={12}>
            <Form.Item label="担任职位" name="positionId">
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
          </Col>
        </Row>
        {!row || !row?.id ? (
          <Row>
            <Col span={12}>
              <Form.Item
                label={
                  <span>
                    添加用户{' '}
                    <Tooltip title="勾选后，会以手机号作为账号添加新的用户数据">
                      <InfoCircleOutlined className="icon-info" />
                    </Tooltip>
                  </span>
                }
                name="isAddUser"
              >
                <Checkbox
                  checked={isAddUser}
                  onChange={(v) => {
                    setIsAddUser(v.target.checked);
                  }}
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              {isAddUser && (
                <Form.Item label="用户密码" name="userPassword" rules={[{ required: true }, pwdPatternValidateItem]}>
                  <Input.Password placeholder="请输入用户密码" />
                </Form.Item>
              )}
            </Col>
          </Row>
        ) : (
          row?.userName && (
            <Row>
              <Col span={24}>
                <Form.Item label="已绑定用户">
                  <Tag color="magenta">账号：{row?.userName}</Tag>
                </Form.Item>
              </Col>
            </Row>
          )
        )}
      </Form>
    </Modal>
  );
});

export default EmployeeForm;
