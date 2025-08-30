import Permission from '@/components/Permission';
import { DeleteOutlined, EditOutlined, PlusOutlined, UserAddOutlined } from '@ant-design/icons';
import { Button, Card, Col, Form, Input, List, Popconfirm, Row, Space, Tag } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
import { deleteEmployee, getEmployeePagedList, type EmployeeListDto } from '@/api/organization/employee';
import SmartTable from '@/components/SmartTable';
import EmployeeForm, { type EmployeeModalRef } from '@/pages/org/components/EmployeeForm.tsx';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import BindUserForm, { type BindUserFormRef } from '@/pages/org/components/BindUserForm.tsx';
import useApp from 'antd/es/app/useApp';
import './styles/employee.scss';
import Search from 'antd/es/input/Search';
import { type DeptSimpleInfoDto, getDeptSimpleInfos } from '@/api/organization/dept';

const EmployeeList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const modalRef = useRef<EmployeeModalRef>(null);
  const bindUserModalRef = useRef<BindUserFormRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
    {
      title: '姓名',
      dataIndex: 'name',
    },
    {
      title: '性别',
      dataIndex: 'sex',
      render: (text: number) => {
        if (text === 1) return '男';
        return '女';
      },
    },
    {
      title: '工号',
      dataIndex: 'code',
    },
    {
      title: '电话',
      dataIndex: 'phone',
    },
    {
      title: '部门',
      dataIndex: 'deptName',
    },
    {
      title: '职位',
      dataIndex: 'positionName',
    },
    {
      title: '状态',
      dataIndex: 'status',
      render: (text: number) => {
        if (text === 1) return <Tag color="green">正常</Tag>;
        return <Tag color="red">离职</Tag>;
      },
      width: 80,
    },
    {
      title: '操作',
      dataIndex: 'option',
      fixed: 'right',
      width: 210,
      render: (_: any, record: EmployeeListDto) => (
        <Space>
          <Permission permissions={'Org.Employee.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                modalRef?.current?.openModal(record);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Org.Employee.BindUser'}>
            <Button
              type="link"
              icon={<UserAddOutlined />}
              onClick={() => {
                bindUserModalRef?.current?.openModal(record);
              }}
            >
              绑定用户
            </Button>
          </Permission>
          <Permission permissions={'Org.Employee.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteEmployee(record.id!).then(() => {
                  message.success('删除成功');
                  tableRef.current?.reload();
                });
              }}
            >
              <Button type="link" danger icon={<DeleteOutlined />}>
                删除
              </Button>
            </Popconfirm>
          </Permission>
        </Space>
      ),
    },
  ];
  const [deptData, setDeptData] = useState<DeptSimpleInfoDto[]>([]);
  const [deptKeyword, setDeptKeyword] = useState<string>('');
  const [curDept, setCurDept] = useState<DeptSimpleInfoDto | null>(null);
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
            columns={columns}
            rowKey="id"
            ref={tableRef}
            request={async (params) => {
              const { data } = await getEmployeePagedList({ ...params, deptId: curDept?.id });
              return data;
            }}
            searchItems={[
              <Form.Item label="关键词" name="keyword">
                <Input placeholder="请输入姓名/手机号/工号" />
              </Form.Item>,
            ]}
            toolbar={
              <Permission permissions={'Org.Employee.Add'}>
                <Button
                  color="primary"
                  variant="solid"
                  icon={<PlusOutlined />}
                  onClick={() => {
                    modalRef?.current?.openModal();
                  }}
                >
                  新增
                </Button>
              </Permission>
            }
          />
          {/* 新增/编辑员工弹窗 */}
          <EmployeeForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
          {/* 绑定用户弹窗 */}
          <BindUserForm ref={bindUserModalRef} refresh={() => tableRef?.current?.reload()} />
        </Col>
      </Row>
    </>
  );
};

export default EmployeeList;
