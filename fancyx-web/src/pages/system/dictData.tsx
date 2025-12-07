import Permission from '@/components/Permission';
import { deleteDictData, getDictDataList, type AddOrUpdateDictDataRequest, type DictDataItem } from '@/api/system/dictData';
import { CopyOutlined, DeleteOutlined, EditOutlined, PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, Popconfirm, Space, Switch } from 'antd';
import React, { useRef } from 'react';
import { useParams } from 'react-router-dom';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import DictDataForm, { type ModalRef } from '@/pages/system/components/DictDataForm.tsx';
import SmartTable from '@/components/SmartTable';
import useApp from 'antd/es/app/useApp';

const DictDataList: React.FC = () => {
  const tableRef = useRef<SmartTableRef>(null);
  const modalRef = useRef<ModalRef>(null);
  const { message } = useApp();
  const columns: SmartTableColumnType[] = [
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
    },
    {
      title: '备注',
      dataIndex: 'remark',
    },
    {
      title: '状态',
      dataIndex: 'isEnabled',
      search: false,
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
      width: 210,
      fixed: 'right',
      dataIndex: 'option',
      render: (_: any, record: DictDataItem) => (
        <Space>
          <Permission permissions={'Sys.DictData.Update'}>
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
          <Permission permissions={'Sys.DictData.Update'}>
            <Button
              key="copy"
              type="link"
              icon={<CopyOutlined />}
              onClick={() => {
                const row = record as AddOrUpdateDictDataRequest;
                row.id = undefined;
                modalRef?.current?.openModal(record);
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
  const urlParams = useParams();

  return (
    <>
      <SmartTable
        columns={columns}
        rowKey="id"
        ref={tableRef}
        request={async (params) => {
          const { data } = await getDictDataList({ ...params, dictType: urlParams?.dictType });
          return data;
        }}
        searchItems={[
          <Form.Item label="字典标签" name="label">
            <Input placeholder="请输入字典标签" />
          </Form.Item>,
        ]}
        toolbar={
          <Permission permissions={'Sys.DictData.Add'}>
            <Button
              type="primary"
              key="primary"
              onClick={() => {
                modalRef?.current?.openModal();
              }}
            >
              <PlusOutlined /> 新增
            </Button>
          </Permission>
        }
      />
      {/* 新增/编辑字典数据弹窗 */}
      <DictDataForm ref={modalRef} refresh={() => tableRef?.current?.reload()} />
    </>
  );
};

export default DictDataList;
