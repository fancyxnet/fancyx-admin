import Permission from '@/components/Permission';
import {
  deleteDictType,
  getDictTypeList,
  type DictTypeItem,
  deleteDictTypes,
} from '@/api/system/dictType';
import { DeleteOutlined, EditOutlined, ExclamationCircleFilled, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import DictTypeForm from '@/pages/system/components/DictTypeForm.tsx';
import ProIcon from '@/components/ProIcon';
import useApp from 'antd/es/app/useApp';
import { useDispatch } from 'react-redux';
import { open } from '@/store/tabStore.ts';

const DictList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { message, modal } = useApp();
  const [rowId, setRowId] = useState<string | null>(null);
  const [modalVisit, setModalVisit] = useState<boolean>(false);
  const columns: SmartTableColumnType[] = [
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
    },
    {
      title: '状态',
      dataIndex: 'isEnabled',
      render: (text: boolean) => {
        return <Switch checked={text} />;
      },
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
    },
    {
      title: '操作',
      dataIndex: 'option',
      width: 210,
      fixed: 'right',
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

  const batchDelete = () => {
    const ids = tableRef?.current?.getSelectedKeys();
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
          tableRef?.current?.reload();
        });
      },
    });
  };

  return (
    <>
      <SmartTable
        columns={columns}
        ref={tableRef}
        rowKey="id"
        selection
        request={async (params) => {
          const { data } = await getDictTypeList(params);
          return data;
        }}
        searchItems={[
          <Form.Item label="字典名称" name="name">
            <Input placeholder="请输入字典名称" />
          </Form.Item>,
          <Form.Item label="字典类型" name="dictType">
            <Input placeholder="请输入字典类型" />
          </Form.Item>,
        ]}
        toolbar={
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
        }
      />
      {/** 新增/编辑字典类型弹窗 */}
      <DictTypeForm
        id={rowId}
        modalVisit={modalVisit}
        onOpenChange={setModalVisit}
        callback={() => tableRef?.current?.reload()}
      />
    </>
  );
};

export default DictList;
