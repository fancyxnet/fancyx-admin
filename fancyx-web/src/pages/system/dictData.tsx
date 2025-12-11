import Permission from '@/components/Permission';
import { deleteDictData, getDictDataList, type DictDataItem, type GetDictDataListRequest } from '@/api/system/dictData';
import { CopyOutlined, DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Popconfirm, Space, Switch } from 'antd';
import React, { useRef, useState } from 'react';
import { useParams } from 'react-router-dom';
import DictDataForm from '@/pages/system/components/DictDataForm.tsx';
import useApp from 'antd/es/app/useApp';
import { ProTable, type ActionType, type ProColumnType } from '@ant-design/pro-components';

const DictData: React.FC = () => {
  const { message } = useApp();
  const [rowId, setRowId] = useState<string | null>(null);
  const [modalVisit, setModalVisit] = useState<boolean>(false);
  const [isCopy, setIsCopy] = useState<boolean>(false);
  const actionRef = useRef<ActionType>();
  const columns: ProColumnType<DictDataItem>[] = [
    {
      title: '字典标签',
      dataIndex: 'label',
    },
    {
      title: '字典键值',
      dataIndex: 'value',
      search: false,
    },
    {
      title: '显示排序',
      dataIndex: 'sort',
      search: false,
    },
    {
      title: '备注',
      dataIndex: 'remark',
      search: false,
    },
    {
      title: '状态',
      dataIndex: 'isEnabled',
      search: false,
      render: (_: any, record: DictDataItem) => {
        return <Switch checked={record.isEnabled} />;
      },
    },
    {
      title: '创建时间',
      dataIndex: 'creationTime',
      search: false,
    },
    {
      title: '操作',
      width: 210,
      fixed: 'right',
      dataIndex: 'option',
      search: false,
      render: (_: any, record: DictDataItem) => (
        <Space>
          <Permission permissions={'Sys.DictData.Update'}>
            <Button
              type="link"
              icon={<EditOutlined />}
              key="edit"
              onClick={() => {
                setIsCopy(false);
                setRowId(record.id);
                setModalVisit(true);
              }}
            >
              编辑
            </Button>
          </Permission>
          <Permission permissions={'Sys.DictData.Update'}>
            <Button
              key="copy"
              type="link"
              icon={<CopyOutlined />}
              onClick={() => {
                setIsCopy(true);
                setRowId(record.id);
                setModalVisit(true);
              }}
            >
              复制
            </Button>
          </Permission>
          <Permission permissions={'Sys.DictData.Delete'}>
            <Popconfirm
              key="delete"
              title="确定删除吗？"
              description="删除后无法撤销"
              onConfirm={() => {
                deleteDictData([record.id!]).then(() => {
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
  const urlParams = useParams();

  return <div className='fancyx-table-wrapper'>
    <ProTable<DictDataItem, GetDictDataListRequest>
      actionRef={actionRef}
      rowKey="id"
      columns={columns}
      request={async (
        params: GetDictDataListRequest
      ) => {
        const res = await getDictDataList({ ...params, dictType: urlParams?.dictType });
        return {
          data: res.data.items,
          success: true,
          total: res.data.totalCount
        };
      }}
      toolBarRender={
        () => [
          <Permission permissions={'Sys.DictData.Add'}>
            <Button
              type="primary"
              key="primary"
              onClick={() => {
                setIsCopy(false);
                setRowId(null);
                setModalVisit(true);
              }}
            >
              <PlusOutlined /> 新增
            </Button>
          </Permission>
        ]
      }
    />
    {/* 新增/编辑字典数据弹窗 */}
    <DictDataForm
      id={rowId}
      isCopy={isCopy}
      modalVisit={modalVisit}
      onOpenChange={setModalVisit}
      callback={() => actionRef?.current?.reload()}
    />
  </div>
}

export default DictData;
