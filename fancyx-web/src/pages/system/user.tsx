import { Button, Switch, Space, Form, Input, Avatar, Col, Card, List, Row, Tag, Dropdown } from 'antd';
import { useEffect, useRef, useState } from 'react';
import {
  DeleteOutlined,
  DoubleRightOutlined,
  EditOutlined,
  ExclamationCircleFilled,
  KeyOutlined,
  PlusOutlined,
} from '@ant-design/icons';
import useApp from 'antd/es/app/useApp';
import {
  deleteUser,
  getUserList,
  switchUserEnabledStatus,
  type UserItem,
  type GetUserListRequest,
} from '@/api/system/user';
import UserEditForm, { type ModalRef } from '@/pages/system/components/UserForm.tsx';
import AssignRoleForm, { type AssignRoleFormRef } from '@/pages/system/components/AssignRoleForm.tsx';
import SmartTable from '@/components/SmartTable';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import ResetUserPwdForm, { type ResetUserPwdFormRef } from '@/pages/system/components/ResetUserPwdForm.tsx';
import ProIcon from '@/components/ProIcon';
import { useApplication } from '@/components/Application';
import Permission from '@/components/Permission';
import { getDeptSimpleInfos, type DeptSimpleInfo } from '@/api/organization/dept';
import Search from 'antd/es/input/Search';
import './styles/user.scss';
import { useAuthProvider } from '@/components/AuthProvider';

const UserTable = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const { message, modal } = useApp();
  const userEditModalRef = useRef<ModalRef>(null);
  const assignRoleRef = useRef<AssignRoleFormRef>(null);
  const resetUserPwdFormRef = useRef<ResetUserPwdFormRef>(null);
  const { ossDomain } = useApplication();
  const { hasPermission } = useAuthProvider();
  const columns: SmartTableColumnType[] = [
    {
      key: 'index',
      render: (_: any, __: any, index: number) => index + 1,
    },
    {
      title: '头像',
      dataIndex: 'avatar',
      render: (text: string) => {
        return <Avatar size={50} src={ossDomain + text} />;
      },
    },
    {
      title: '账号',
      dataIndex: 'userName',
      key: 'userName',
    },
    {
      title: '昵称',
      dataIndex: 'nickName',
    },
    {
      title: '性别',
      dataIndex: 'sex',
      key: 'sex',
      render: (text: number) => (text === 1 ? '男' : '女'),
    },
    {
      title: '状态',
      dataIndex: 'isEnabled',
      key: 'isEnabled',
      render: (text: boolean, record: UserItem) => (
        <Switch
          checked={text}
          checkedChildren="启用"
          unCheckedChildren="禁用"
          onChange={() => onUserStatusChange(record)}
        />
      ),
    },
    {
      title: '手机号',
      dataIndex: 'phone',
    },
    {
      title: '部门',
      dataIndex: 'deptName',
    },
    {
      title: '岗位',
      dataIndex: 'postName',
    },
    {
      title: '操作',
      key: 'action',
      width: 180,
      fixed: 'right',
      render: (_: any, record: UserItem) => {
        const curDropdownItems = [];
        if (hasPermission!('Sys.User.AssignRole')) {
          curDropdownItems.push({
            key: 'assignRole',
            label: (
              <a
                onClick={() => {
                  assignRoleRef?.current?.openModal(record);
                }}
              >
                <ProIcon icon="iconify:simple-line-icons:check" className="mr-4" />
                分配角色
              </a>
            ),
            onClick: () => {},
          });
        }
        if (hasPermission!('Sys.User.ResetPwd')) {
          curDropdownItems.push({
            key: 'resetPwd',
            label: (
              <a
                onClick={() => {
                  resetUserPwdFormRef?.current?.openModal(record);
                }}
              >
                <KeyOutlined className="mr-4" />
                重置密码
              </a>
            ),
            onClick: () => {},
          });
        }
        return (
          <Space>
            <Permission permissions={'Sys.User.Update'}>
              <Button
                type="link"
                icon={<EditOutlined />}
                key="edit"
                onClick={() => {
                  userEditModalRef?.current?.openModal(record.id);
                }}
              >
                编辑
              </Button>
            </Permission>
            <Permission permissions={'Sys.User.Delete'}>
              <Button type="link" icon={<DeleteOutlined />} danger onClick={() => rowDelete(record.id)}>
                删除
              </Button>
            </Permission>
            <Permission permissions={['Sys.User.AssignRole', 'Sys.User.ResetPwd']} mode="some">
              <Dropdown
                placement="bottom"
                menu={{
                  items: curDropdownItems,
                }}
              >
                <Button type="link" icon={<DoubleRightOutlined />}>
                  更多
                </Button>
              </Dropdown>
            </Permission>
          </Space>
        );
      },
    },
  ];
  const [deptData, setDeptData] = useState<DeptSimpleInfo[]>([]);
  const [deptKeyword, setDeptKeyword] = useState<string>('');
  const [curDept, setCurDept] = useState<DeptSimpleInfo | null>(null);
  const [deptLoading, setDeptLoading] = useState<boolean>(false);
  useEffect(() => {
    setDeptLoading(true);
    getDeptSimpleInfos(deptKeyword)
      .then((res) => {
        setDeptLoading(false);
        setDeptData(res.data);
      })
      .catch(() => {
        setDeptLoading(false);
      });
  }, [deptKeyword]);
  const onDeptSearch = (value: string) => {
    setDeptKeyword(value);
  };
  const preventDefault = (e: React.MouseEvent<HTMLElement>) => {
    e.preventDefault();
    setCurDept(null);
  };
  useEffect(() => {
    tableRef?.current?.reload();
  }, [curDept?.id]);

  const rowDelete = (id: string) => {
    modal.confirm({
      title: '确认删除？',
      icon: <ExclamationCircleFilled />,
      onOk() {
        deleteUser(id).then(() => {
          message.success('删除成功');
          tableRef?.current?.reload();
        });
      },
    });
  };
  const onUserStatusChange = (record: UserItem) => {
    switchUserEnabledStatus(record.id).then(() => {
      message.success('状态更改成功');
      tableRef?.current?.reload();
    });
  };

  return (
    <>
      <Row gutter={16}>
        <Col span={6}>
          <Card>
            {curDept?.id?.length && curDept?.id?.length > 0 && (
              <Tag closeIcon onClose={preventDefault} style={{ marginBottom: 12 }}>
                筛选部门：{curDept?.name}
              </Tag>
            )}
            <Search placeholder="请输入部门名称/编码" style={{ marginBottom: 12 }} allowClear onSearch={onDeptSearch} />
            <List
              loading={deptLoading}
              size="small"
              dataSource={deptData}
              className="dept-list"
              renderItem={(item) => (
                <List.Item>
                  <span
                    className="dept-text"
                    onClick={() => {
                      setCurDept(item);
                    }}
                  >
                    {item.name}({item.code})
                  </span>
                </List.Item>
              )}
            />
          </Card>
        </Col>
        <Col span={18}>
          <SmartTable
            rowKey="id"
            columns={columns}
            ref={tableRef}
            request={async (params) => {
              const { data } = await getUserList({ ...params, deptId: curDept?.id });
              return data;
            }}
            searchItems={
              <Form.Item<GetUserListRequest> label="账号" name="userName">
                <Input placeholder="请输入账号" />
              </Form.Item>
            }
            toolbar={
              <Permission permissions={'Sys.User.Add'}>
                <Button
                  color="primary"
                  variant="solid"
                  icon={<PlusOutlined />}
                  onClick={() => {
                    userEditModalRef?.current?.openModal();
                  }}
                >
                  新增
                </Button>
              </Permission>
            }
          />
        </Col>
      </Row>
      {/* 新增/编辑弹窗 */}
      <UserEditForm ref={userEditModalRef} refresh={() => tableRef?.current?.reload()} />
      {/* 分配角色弹窗 */}
      <AssignRoleForm ref={assignRoleRef} />
      {/* 重置密码弹窗 */}
      <ResetUserPwdForm ref={resetUserPwdFormRef} />
    </>
  );
};
export default UserTable;
