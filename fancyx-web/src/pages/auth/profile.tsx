import { useState } from 'react';
import { Card, Form, Input, Upload, Avatar, Divider, Row, Col, Typography, Menu, Radio, Button } from 'antd';
import { UserOutlined, CameraOutlined, LockOutlined } from '@ant-design/icons';
import './style/profile.scss';
import { type UpdateUserInfoRequest, updateInfo, updatePwd, type UpdateUserPwdRequest } from '@/api/auth.ts';
import { ErrorCode, Patterns } from '@/utils/globalValue.ts';
import { useApplication } from '@/components/Application';
import { uploadFile } from '@/api/oss';
import useApp from 'antd/es/app/useApp';
import { useAuthProvider } from '@/components/AuthProvider';
import useDeepCompareEffect from 'use-deep-compare-effect';
import { observer } from 'mobx-react-lite';
import UserStore from '@/store/userStore.ts';

const { Title } = Typography;

const Profile = observer(() => {
  const userInfo = UserStore.userInfo;
  const { refreshUserAuthInfo } = useAuthProvider();
  const [baseInfoForm] = Form.useForm();
  const [pwdForm] = Form.useForm();
  const [avatar, setAvatar] = useState(userInfo?.avatar);
  const [activeKey, setActiveKey] = useState<string>('baseInfo');
  const [loading, setLoading] = useState<boolean>(false);
  const { ossDomain } = useApplication();
  const { message } = useApp();

  useDeepCompareEffect(() => {
    baseInfoForm.setFieldValue('nickName', userInfo?.nickName);
    baseInfoForm.setFieldValue('sex', userInfo?.sex);
    baseInfoForm.setFieldValue('phone', userInfo?.phone);
  }, [userInfo ?? {}]);

  const profileMenuItems = [
    {
      key: 'baseInfo',
      label: '基本信息',
      icon: <UserOutlined />,
    },
    {
      key: 'updatePwd',
      label: '修改密码',
      icon: <LockOutlined />,
    },
  ];

  const beforeUpload = (file: File) => {
    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
    if (!isJpgOrPng) {
      message.error('只能上传 JPG/PNG 格式的图片!');
    }
    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
      message.error('图片大小不能超过 2MB!');
    }
    return isJpgOrPng && isLt2M;
  };

  const updateUserInfo = (info: UpdateUserInfoRequest) => {
    setLoading(true);
    updateInfo(info)
      .then(() => {
        setLoading(false);
        message.success('修改成功');
        refreshUserAuthInfo?.();
      })
      .catch(() => setLoading(false));
  };

  const validateSurePassword = (_: any, value: string) => {
    const newPwd = pwdForm.getFieldValue('newPwd');
    if (newPwd && newPwd !== value) {
      return Promise.reject(new Error('两次密码不一致'));
    }
    return Promise.resolve();
  };

  const pwdPatternValidateItem = {
    pattern: Patterns.LoginPassword,
    message: '密码至少有一个字母和数字，长度6-16位，特殊字符 "~`!@#$%^&*()_-+={[}]|\\:;\\"\'<,>.?/',
  };

  return (
    <div className="profile-page">
      <Row gutter={24} className="full-height-row">
        {/* 左侧卡片 - 个人信息概览 */}
        <Col xs={24} md={6} className="card-col">
          <Card className="profile-card">
            <div className="avatar-section">
              <div className="avatar-upload">
                <Avatar size={128} src={ossDomain + avatar} icon={<UserOutlined />} />
                <Upload
                  name="avatar"
                  showUploadList={false}
                  beforeUpload={beforeUpload}
                  customRequest={({ file, onSuccess }) => {
                    uploadFile(file as File).then(async (res) => {
                      if (res.code === ErrorCode.Success) {
                        setAvatar(res.data);
                        const updateRes = await updateInfo({ avatar: res.data });
                        if (updateRes.code === ErrorCode.Success) {
                          message.success('新头像上传成功');
                          refreshUserAuthInfo?.();
                          onSuccess?.(res.data);
                        }
                      }
                    });
                  }}
                >
                  <div className="avatar-upload-mask">
                    <CameraOutlined style={{ fontSize: 24 }} />
                    <div>更换头像</div>
                  </div>
                </Upload>
              </div>
            </div>

            <Divider />

            <div className="profile-summary">
              <div className="nickname-section">
                <Title level={5} style={{ margin: 0 }}>
                  {userInfo?.nickName}
                </Title>
              </div>
            </div>

            <div>
              <Menu
                items={profileMenuItems}
                selectedKeys={[activeKey]}
                onClick={({ key }) => {
                  setActiveKey(key);
                }}
              />
            </div>
          </Card>
        </Col>

        {/* 右侧卡片 - 详细信息 */}
        <Col xs={24} md={18} className="card-col">
          {activeKey === 'baseInfo' && (
            <Card className="detail-card">
              <Title level={5} className="detail-title">
                基本信息
              </Title>

              <Form
                form={baseInfoForm}
                layout="vertical"
                onFinish={(values) => {
                  updateUserInfo(values);
                }}
              >
                <Form.Item label="账号" className="detail-form-item" rules={[{ required: true }]}>
                  <Input value={userInfo?.userName} disabled />
                </Form.Item>
                <Form.Item
                  label="昵称"
                  className="detail-form-item"
                  name="nickName"
                  rules={[{ required: true }, { max: 64 }]}
                >
                  <Input placeholder="请输入您的昵称" />
                </Form.Item>
                <Form.Item
                  label="手机号"
                  className="detail-form-item"
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
                <Form.Item label="性别" className="detail-form-item" name="sex" rules={[{ required: true }]}>
                  <Radio.Group
                    value={userInfo?.sex}
                    options={[
                      { label: '男', value: 1 },
                      { label: '女', value: 2 },
                    ]}
                  />
                </Form.Item>
                <Form.Item>
                  <Button type="primary" loading={loading} htmlType="submit">
                    确认修改
                  </Button>
                </Form.Item>
              </Form>
            </Card>
          )}
          {activeKey === 'updatePwd' && (
            <Card className="detail-card">
              <Title level={5} className="detail-title">
                修改密码
              </Title>

              <Form
                form={pwdForm}
                layout="vertical"
                onFinish={(values: UpdateUserPwdRequest) => {
                  setLoading(true);
                  updatePwd(values)
                    .then(() => {
                      setLoading(false);
                      message.success('修改成功');
                      pwdForm.resetFields();
                    })
                    .catch(() => {
                      setLoading(false);
                    });
                }}
              >
                <Form.Item label="旧密码" className="detail-form-item" name="oldPwd" rules={[{ required: true }]}>
                  <Input.Password placeholder="请输入旧密码" />
                </Form.Item>
                <Form.Item
                  label="新密码"
                  className="detail-form-item"
                  name="newPwd"
                  rules={[{ required: true }, pwdPatternValidateItem]}
                >
                  <Input.Password placeholder="请输入新密码" />
                </Form.Item>
                <Form.Item
                  label="确认新密码"
                  className="detail-form-item"
                  name="surePwd"
                  rules={[{ required: true }, { validator: validateSurePassword }, pwdPatternValidateItem]}
                >
                  <Input.Password placeholder="请再次输入新密码" />
                </Form.Item>
                <Form.Item>
                  <Button type="primary" loading={loading} htmlType="submit">
                    确认修改
                  </Button>
                </Form.Item>
              </Form>
            </Card>
          )}
        </Col>
      </Row>
    </div>
  );
});

export default Profile;
