import Permission from '@/components/Permission';
import { deleteWarehouse, getWarehouseList, addWarehouse, updateWarehouse, getWarehouse, type Warehouse, type GetWarehouseListRequest } from '@/api/erp/warehouse.ts';
import { DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Modal, Popconfirm, Space } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
import useApp from 'antd/es/app/useApp';
import TextArea from 'antd/es/input/TextArea';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const Warehouse: React.FC = () => {
  const { message } = useApp();
  const actionRef = useRef<ActionType>();
  const [isOpenModal, setIsOpenModal] = useState<boolean>(false);
  const [rowId, setRowId] = useState<string>('');
  const columns: ProColumnType<Warehouse>[] = [
    {
      title: '编码',
      dataIndex: 'code',
      search: false,
    },
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '备注',
      dataIndex: 'remark',
      search: false,
    },
    {
      title: '是否启用',
      dataIndex: 'isEnabled',
      search: false,
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 210,
      fixed: 'right',
      search: false,
      render: (_: any, record: Warehouse) => (
        <Space>
          <Permission permissions={'Erp.Warehouse.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                setRowId(record.id);
                setIsOpenModal(true)
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Erp.Warehouse.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteWarehouse(record.id!).then(() => {
                  message.success('删除成功');
                  actionRef.current?.reload();
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

  return (
    <div className='fancyx-table-wrapper' >
      <ProTable<Warehouse, GetWarehouseListRequest>
        actionRef={actionRef}
        rowKey="id"
        columns={columns}
        request={async (
          params: GetWarehouseListRequest
        ) => {
          const res = await getWarehouseList(params);
          return {
            data: res.data.items,
            success: true,
            total: res.data.totalCount
          };
        }}
        toolBarRender={
          () => [<Permission permissions={'Erp.Warehouse.Add'}>
            <Button
              type="primary"
              key="primary"
              onClick={() => {
                setIsOpenModal(true);
              }}
            >
              <PlusOutlined /> 新增
            </Button>
          </Permission>]
        }
      />
      {/** 新增/编辑弹窗 */}
      <WarehouseModal id={rowId}
        show={isOpenModal}
        onOk={() => {
          setRowId('');
          setIsOpenModal(false);
          actionRef?.current?.reload();
        }}
        onCancel={() => {
          setRowId('');
          setIsOpenModal(false);
        }} />
    </div>)
}

const WarehouseModal: React.FC<{
  id: string;
  show: boolean;
  onOk: () => void;
  onCancel: () => void;
}> = ({ id, show, onOk, onCancel }) => {
  const isEdit = id && id.length > 0;
  const [form] = Form.useForm();
  const { message } = useApp();

  useEffect(() => {
    if (isEdit) {
      getWarehouse(id).then((res) => {
        form.setFieldsValue(res.data)
      })
    }
  }, [id])

  const onFinish = (values: Warehouse) => {
    try {
      if (isEdit) {
        updateWarehouse({ ...values, id }).then(() => {
          message.success('编辑成功')
          onOk();
        })

      } else {
        addWarehouse(values).then(() => {
          message.success('新增成功')
          onOk();
        })
      }
    }
    finally {
      form.resetFields();
    }
  };

  return (
    <Modal
      title={(isEdit ? '编辑' : '新增') + '仓库'}
      width="60%"
      open={show}
      onCancel={() => {
        form.resetFields();
        onCancel();
      }}
      onOk={() => {
        form.submit();
      }}
      maskClosable={false}
    >
      <Form<Warehouse>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="编码" name="code" rules={[{ required: true }, { max: 256 }]}>
          <Input placeholder="请输入编码" />
        </Form.Item>
        <Form.Item label="名称" name="name" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入名称" />
        </Form.Item>
        <Form.Item label="备注" name="remark" rules={[{ max: 64 }]}>
          <TextArea placeholder="请输入备注" />
        </Form.Item>
      </Form>
    </Modal>
  );
}

export default Warehouse;
