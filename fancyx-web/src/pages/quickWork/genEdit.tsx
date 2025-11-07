import { Button, Card, Checkbox, Form, Input, Select, Tabs, type TabsProps } from 'antd';
import { useEffect, useRef, useState } from 'react';
import {
  getGenDetailsInfo,
  saveGenTableInfo,
  getGenTableColumnList,
  saveGenColumnInfo,
  type GenTableColumnDto,
  type GenTableColumnListDto,
  type GenTableInfoDto,
  type GenDetailsInfoDto,
} from '@/api/system/gen.ts';
import useApp from 'antd/es/app/useApp';
import TextArea from 'antd/es/input/TextArea';
import { useParams } from 'react-router-dom';
import SmartTable from '@/components/SmartTable';
import type { SmartTableRef } from '@/components/SmartTable/type';
import _ from 'lodash';

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
    getGenDetailsInfo(urlParams?.tableId as string).then((res) => {
      setGenTableInfo(res.data);
      form.setFieldsValue(res.data);
    });
  }, []);

  const onFinish = (values: GenTableInfoDto) => {
    saveGenTableInfo({ ...values, tableId: genTableInfo!.tableId }).then(() => {
      message.success('保存成功');
    });
  };
  const updateField = (field: string, value: any, columnId: string) => {
    const tableData = tableRef?.current?.getData();
    if (tableData && tableData.length > 0) {
      const i = tableData.findIndex((x) => x.columnId === columnId);
      tableData[i][field] = value;
      tableRef?.current?.updateData(_.cloneDeep(tableData));
    }
  };
  const columns = [
    {
      title: '字段列名',
      dataIndex: 'columnName',
    },
    {
      title: '字段描述',
      dataIndex: 'columnComment',
      render: (value: string, record: GenTableColumnListDto) => {
        return cellInput('columnComment', value, record.columnId);
      },
    },
    {
      title: '物理类型',
      dataIndex: 'columnType',
    },
    {
      title: 'CSharp类型',
      dataIndex: 'csharpType',
    },
    {
      title: 'CSharp属性',
      dataIndex: 'csharpField',
      render: (value: string, record: GenTableColumnListDto) => {
        return cellInput('csharpField', value, record.columnId);
      },
    },
    {
      title: '插入',
      dataIndex: 'isInsert',
      render: (value: boolean, record: GenTableColumnListDto) => {
        return booleanCheckbox('isInsert', value, record.columnId);
      },
    },
    {
      title: '编辑',
      dataIndex: 'isEdit',
      render: (value: boolean, record: GenTableColumnListDto) => {
        return booleanCheckbox('isEdit', value, record.columnId);
      },
    },
    {
      title: '列表',
      dataIndex: 'isList',
      render: (value: boolean, record: GenTableColumnListDto) => {
        return booleanCheckbox('isList', value, record.columnId);
      },
    },
    {
      title: '查询',
      dataIndex: 'isQuery',
      render: (value: boolean, record: GenTableColumnListDto) => {
        return booleanCheckbox('isQuery', value, record.columnId);
      },
    },
    {
      title: '查询方式',
      dataIndex: 'queryType',
      render: (value: string, record: GenTableColumnListDto) => {
        return queryTypeSelect('queryType', value, record.columnId);
      },
      minWidth: 90,
    },
    {
      title: '必填',
      dataIndex: 'isRequired',
      render: (value: boolean, record: GenTableColumnListDto) => {
        return booleanCheckbox('isRequired', value, record.columnId);
      },
    },
    {
      title: '显示类型',
      dataIndex: 'htmlType',
      render: (value: string, record: GenTableColumnListDto) => {
        return htmlTypeSelect('isQuery', value, record.columnId);
      },
    },
  ];
  const cellInput = (field: string, value: any, columnId: string) => {
    return (
      <Input
        value={value}
        onChange={(e) => {
          updateField(field, e.target.value, columnId);
        }}
      />
    );
  };
  const booleanCheckbox = (field: string, value: boolean, columnId: string) => {
    return (
      <Checkbox
        checked={value}
        onChange={(e) => {
          updateField(field, e.target.checked, columnId);
        }}
      />
    );
  };
  const htmlTypeSelect = (field: string, value: string, columnId: string) => {
    return (
      <Select
        value={value}
        style={{ width: '100%' }}
        onChange={(val) => {
          updateField(field, val, columnId);
        }}
        options={[
          { label: '文本框', value: 'text' },
          { label: '文本域', value: 'textarea' },
          { label: '下拉框', value: 'select' },
          { label: '单选框', value: 'radio' },
          { label: '复选框', value: 'checkbox' },
          { label: '日期控件', value: 'datetime' },
          { label: '图片上传', value: 'image' },
          { label: '文件上传', value: 'file' },
          { label: '富文本控件', value: 'richtext' },
        ]}
      />
    );
  };
  const queryTypeSelect = (field: string, value: string, columnId: string) => {
    return (
      <Select
        value={value}
        style={{ width: '100%' }}
        onChange={(val) => {
          updateField(field, val, columnId);
        }}
        options={[
          { label: '=', value: '=' },
          { label: '!=', value: '!=' },
          { label: '>', value: '>' },
          { label: '>=', value: '>=' },
          { label: '<', value: '<' },
          { label: '<=', value: '<=' },
          { label: 'LIKE', value: 'LIKE' },
          { label: 'BETWEEN', value: 'BETWEEN' },
        ]}
      />
    );
  };
  const items: TabsProps['items'] = [
    {
      key: 'genTable',
      label: '基本信息',
      children: (
        <Form<GenTableInfoDto>
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
      ),
    },
    {
      key: 'genTableColumn',
      label: '生成列配置',
      children: (
        <SmartTable
          ref={tableRef}
          rowKey="columnId"
          columns={columns}
          request={async (params) => {
            const { data } = await getGenTableColumnList({ ...params, tableId: genTableInfo?.tableId });
            return data;
          }}
        />
      ),
    },
  ];
  const onTabChange = (key: string) => {
    setActiveKey(key);
  };
  const onSave = () => {
    if (activeKey === 'genTable') {
      form.submit();
    } else {
      const arr = tableRef?.current?.getData() as GenTableColumnDto[];
      saveGenColumnInfo(arr).then(() => {
        message.success('保存成功');
      });
    }
  };

  return (
    <Card>
      <Tabs defaultActiveKey={activeKey} items={items} onChange={onTabChange} />
      <div style={{ margin: '15px auto', textAlign: 'center' }}>
        <Button type="primary" onClick={onSave}>
          保存
        </Button>
      </div>
    </Card>
  );
};

export default GenEdit;
