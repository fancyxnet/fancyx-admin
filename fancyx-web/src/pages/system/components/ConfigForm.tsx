import { Form } from 'antd';
import { addConfig, type AddOrUpdateConfigRequest, updateConfig, getConfig } from '@/api/system/config.ts';
import useApp from 'antd/es/app/useApp';
import { ModalForm, ProFormText, ProFormTextArea } from '@ant-design/pro-components';

const ConfigModal: React.FC<{
  id: string | null;
  modalVisit: boolean;
  onOpenChange: (show: boolean) => void;
  callback: () => void;
}> = ({ id, modalVisit, onOpenChange, callback }) => {
  const isUpdate = id && id.length > 0;
  const [form] = Form.useForm<AddOrUpdateConfigRequest>();
  const { message } = useApp();
  return (
    <ModalForm<AddOrUpdateConfigRequest>
      layout="horizontal"
      labelCol={{ flex: '90px' }}
      labelWrap
      wrapperCol={{ flex: 1 }}
      title={`${isUpdate ? '编辑' : '新增'}配置`}
      open={modalVisit}
      form={form}
      onOpenChange={(show: boolean) => {
        if (show) {
          if (isUpdate) {
            getConfig(id).then((res) => {
              form.setFieldsValue(res.data as AddOrUpdateConfigRequest);
            });
          }
        } else {
          form.resetFields();
        }
        onOpenChange(show);
      }}
      onFinish={async (values: AddOrUpdateConfigRequest) => {
        const apiFunc = isUpdate ? updateConfig : addConfig;
        await apiFunc(values);
        message.success('操作成功');
        onOpenChange(false);
        callback();
      }}
    >
      <ProFormText label="组别" name="groupKey" placeholder="请输入组别" />
      <ProFormText
        label="配置名称"
        name="name"
        placeholder="请输入配置名称"
        rules={[{ required: true }, { max: 256 }]}
      />
      <ProFormText
        label="配置键名"
        name="key"
        placeholder="请输入配置键名"
        rules={[{ required: true }, { max: 128 }]}
      />
      <ProFormText label="配置值" name="value" placeholder="请输入配置值" rules={[{ required: true }, { max: 1024 }]} />
      <ProFormTextArea label="备注" name="remark" placeholder="请输入备注" rules={[{ max: 64 }]} />
    </ModalForm>
  );
};

export default ConfigModal;
