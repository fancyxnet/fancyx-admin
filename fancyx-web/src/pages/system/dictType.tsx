import Permission from '@/components/Permission';
import {
  deleteDictType,
  getDictTypeList,
  type DictTypeItem,
  deleteDictTypes,
  type GetDictTypeListRequest,
} from '@/api/system/dictType';
import { DeleteOutlined, EditOutlined, ExclamationCircleFilled, PlusOutlined } from '@ant-design/icons';
import { Button, Popconfirm, Space, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import DictTypeForm from '@/pages/system/components/DictTypeForm.tsx';
import ProIcon from '@/components/ProIcon';
import useApp from 'antd/es/app/useApp';
import { useDispatch } from 'react-redux';
import { open } from '@/store/tabStore.ts';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const DictType: React.FC = () => {
  const actionRef = useRef<ActionType>();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { message, modal } = useApp();
  const [rowId, setRowId] = useState<string | null>(null);
  const [modalVisit, setModalVisit] = useState<boolean>(false);
  const [selectedKeys, setSelectedKeys] = useState<string[]>([])
  const columns: ProColumnType<DictTypeItem>[] = [
    {
      title: '字典名称',
      dataIndex: 'name',
    },
    {
      title: '字典类型',
      dataIndex: 'dictType',
    },
    {
      title: '备注',
      dataIndex: 'remark',
      search: false,
    },
    {
      title: '状态',
      dataIndex: 'isEnabled',
      render: (_: any, record: DictTypeItem) => {
        return <Switch checked={record.isEnabled} />;
      },
      search: false,
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
      search: false,
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 210,
      fixed: 'right',
      search: false,
      render: (_: any, record: DictTypeItem) => (
        <Space>
          <Permission permissions={'Sys.DictType.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                setRowId(record.id);
                setModalVisit(true);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Sys.DictData.List'}>
            <Button
              type="link"
              icon={<ProIcon icon="iconify:mi:database" />}
              key="data"
              onClick={() => {
                const dataPath = `/system/dictItem/${record.dictType}`;
                navigate(dataPath);
                dispatch(open({ name: `【${record.name}】字典数据`, path: dataPath }));
              }}
            >
              数据
            </Button>
          </Permission>
          <Permission permissions={'Sys.DictType.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteDictType(record.dictType!).then(() => {
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

  const batchDelete = () => {
    const ids = selectedKeys;
    if (!ids || !ids.length) {
      message.warning('请选择一条数据进行操作');
      return;
    }
    modal.confirm({
      title: `确认删除选中的${ids!.length}条数据？`,
      icon: <ExclamationCircleFilled />,
      onOk() {
        deleteDictTypes(ids as string[]).then(() => {
          message.success('删除成功');
          actionRef?.current?.reload();
        });
      },
    });
  };

  return <div className='fancyx-table-wrapper'>
    <ProTable<DictTypeItem, GetDictTypeListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetDictTypeListRequest
      ) => {
        const res = await getDictTypeList(params);
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      rowSelection={{
        onChange: (selectedRowKeys) => {
          setSelectedKeys(selectedRowKeys as string[]);
        },
      }}
      toolBarRender={
        () => [
          <Space size="middle">
            <Permission permissions={'Sys.DictType.Add'}>
              <Button
                type="primary"
                key="primary"
                onClick={() => {
                  setRowId(null);
                  setModalVisit(true);
                }}
              >
                <PlusOutlined /> 新增
              </Button>
            </Permission>
            <Permission permissions={'Sys.DictType.Delete'}>
              <Button color="danger" variant="solid" icon={<DeleteOutlined />} onClick={batchDelete}>
                删除
              </Button>
            </Permission>
          </Space>
        ]
      }
    />
    {/** 新增/编辑字典类型弹窗 */}
    <DictTypeForm
      id={rowId}
      modalVisit={modalVisit}
      onOpenChange={setModalVisit}
      callback={() => actionRef?.current?.reload()}
    />
  </div>
}

export default DictType;
