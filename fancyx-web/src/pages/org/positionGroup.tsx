import Permission from '@/components/Permission';
import { deletePositionGroup, getPositionGroupList, type GetPositionGroupListRequest, type PositionGroupItem } from '@/api/organization/positionGroup';
import { DeleteOutlined, EditOutlined, ExclamationCircleFilled, PlusOutlined } from '@ant-design/icons';
import { Button, Popconfirm, Space } from 'antd';
import React, { useRef } from 'react';
import PositionGroupForm, { type ModalRef } from '@/pages/org/components/PositionGroupForm.tsx';
import useApp from 'antd/es/app/useApp';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const PositionGroup: React.FC = () => {
  const { message, modal } = useApp();
  const modalRef = useRef<ModalRef>(null);
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<PositionGroupItem>[] = [
    {
      title: '分组名称',
      dataIndex: 'groupName',
    },
    {
      title: '备注',
      dataIndex: 'remark',
      search: false
    },
    {
      title: '排序值',
      dataIndex: 'sort',
      search: false
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 140,
      fixed: 'right',
      search: false,
      render: (_: any, record: PositionGroupItem) => (
        <Space>
          <Permission permissions={'Org.PositionGroup.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              onClick={() => {
                rowEdit(record);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Org.PositionGroup.Delete'}>
            <Popconfirm
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                rowDelete(record.id);
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
  const handleOpenModal = () => {
    if (modalRef.current) {
      modalRef.current.openModal();
    }
  };

  const rowDelete = (id: string) => {
    modal.confirm({
      title: '确认删除？',
      icon: <ExclamationCircleFilled />,
      onOk() {
        deletePositionGroup(id).then(() => {
          message.success('删除成功');
          actionRef?.current?.reload();
        });
      },
    });
  };
  const rowEdit = (record: PositionGroupItem) => {
    modalRef.current?.openModal(record);
  };

  return <div className='fancyx-table-wrapper'>
    <ProTable<PositionGroupItem, GetPositionGroupListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetPositionGroupListRequest
      ) => {
        const res = await getPositionGroupList(params);
        return {
          data: res.data,
          success: true,
        };
      }}
      toolBarRender={
        () => [
          <Permission permissions={'Org.PositionGroup.Add'}>
            <Button color="primary" variant="solid" icon={<PlusOutlined />} onClick={() => handleOpenModal()}>
              新增
            </Button>
          </Permission>
        ]
      }
    />
    {/* 职位分组新增/编辑弹窗 */}
    <PositionGroupForm ref={modalRef} refresh={() => actionRef?.current?.reload()} />
  </div>
}

export default PositionGroup;
