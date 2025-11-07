import { Button, Card, Form, Input, Select, Tabs, type TabsProps } from 'antd';
import { useEffect, useRef, useState } from 'react';
import { getGenDetailsInfo, saveGenTableInfo, getGenTableColumnList, saveGenColumnInfo, type GenTableColumnQueryDto, type GenTableColumnListDto, type GenTableInfoDto, type GenDetailsInfoDto } from '@/api/system/gen.ts';
import useApp from 'antd/es/app/useApp';
import TextArea from 'antd/es/input/TextArea';
import { useParams } from 'react-router-dom';
import SmartTable from '@/components/SmartTable';
import type { SmartTableRef } from '@/components/SmartTable/type';

export interface ModalRef {
  openModal: (tableId: string) => void;
}

const GenEdit = () => {
  const [form] = Form.useForm();
  const tableRef = useRef<SmartTableRef>(null);
  const [genTableInfo, setGenTableInfo] = useState<GenDetailsInfoDto | null>();
  const { message } = useApp();
  const urlParams = useParams();
  const [activeKey, setActiveKey] = useState<string>('genTable');

  useEffect(() => {
    getGenDetailsInfo(urlParams!.tableId).then(res => {
      setGenTableInfo(res.data);
      form.setFieldsValue(res.data)
    })
  }, [])

  const onFinish = (values: GenTableInfoDto) => {
    saveGenTableInfo({ ...values, tableId: genTableInfo!.tableId }).then(_ => {
      message.success('保存成功')
    })
  };
  const updateField = (field: string, value: any, columnId: string) => {
    const tableData = tableRef?.current?.getData();
    if (tableData && tableData.length > 0) {
      const i = tableData.findIndex(x => x.columnId === columnId);
      tableData[i][field] = value;
      tableRef?.current?.updateData(tableData);
    }
  }
  const columns = [
    {
      title: '列名称',
      dataIndex: 'columnName'
    },
    {
      title: '描述',
      dataIndex: 'columnComment'
    },
    {
      title: '类型',
      dataIndex: 'columnType'
    },
    {
      title: 'CSharp类型',
      dataIndex: 'csharpType'
    },
    {
      title: 'CSharp字段名',
      dataIndex: 'csharpField'
    },
    {
      title: '必填',
      dataIndex: 'isRequired',
      render: (value: boolean, record: GenTableColumnListDto) => {
        return <Select
          value={value}
          key={record.columnId}
          onChange={(val) => {
            updateField('isRequired', val, record.columnId);
          }}
          options={[
            { label: '是', value: true },
            { label: '否', value: false },
          ]} />
      }
    },
    {
      title: '插入',
      dataIndex: 'isInsert'
    },
    {
      title: '编辑',
      dataIndex: 'isEdit'
    },
    {
      title: '列表',
      dataIndex: 'isList'
    },
    {
      title: '查询',
      dataIndex: 'isQuery'
    },
    {
      title: '显示类型',
      dataIndex: 'htmlType'
    },
    {
      title: '排序',
      dataIndex: 'sort'
    }
  ]
  const items: TabsProps['items'] = [
    {
      key: 'genTable',
      label: '基本信息',
      children: <Form<GenTableInfoDto>
        name="wrap"
        labelCol={{ flex: '90px' }}
        labelWrap
        form={form}
        wrapperCol={{ flex: 1 }}
        colon={false}
        onFinish={onFinish}
      >
        <Form.Item label="表名" name="tableName">
          <Input disabled />
        </Form.Item>
        <Form.Item label="描述" name="tableComment" rules={[{ required: true }, { max: 256 }]}>
          <Input placeholder="请输入表描述" />
        </Form.Item>
        <Form.Item label="类名" name="className" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入类名" />
        </Form.Item>
        <Form.Item label="命名空间" name="namespaceName" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入命名空间" />
        </Form.Item>
        <Form.Item label="模块名" name="moduleName" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入模块名" />
        </Form.Item>
        <Form.Item label="业务名" name="businessName" rules={[{ required: true }, { max: 128 }]}>
          <Input placeholder="请输入业务名" />
        </Form.Item>
        <Form.Item label="备注" name="remark" rules={[{ max: 64 }]}>
          <TextArea placeholder="请输入备注" />
        </Form.Item>
      </Form>
    },
    {
      key: 'genTableColumn',
      label: '生成列配置',
      children: <SmartTable
        ref={tableRef}
        rowKey='columnId'
        columns={columns}
        request={async (params) => {
          const { data } = await getGenTableColumnList({ ...params, tableId: genTableInfo?.tableId });
          return data;
        }} />,
    },
  ];
  const onTabChange = (key: string) => {
    setActiveKey(key);
  };
  const onSave = () => {
    if (activeKey === 'genTable') {
      form.submit();
    } else {
      console.log(tableRef?.current?.getData())
    }
  }

  return (
    <Card
    >
      <Tabs defaultActiveKey={activeKey} items={items} onChange={onTabChange} />
      <div style={{ margin: "15px auto", textAlign: 'center' }}>
        <Button type="primary" onClick={onSave}>保存</Button>
        <Button className='ml-10'>返回</Button>
      </div>
    </Card>
  );
};

export default GenEdit;
