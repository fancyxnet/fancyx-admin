import { Form } from 'antd';
import { addDictType, type AddOrUpdateDictTypeRequest, getDictType, updateDictType } from '@/api/system/dictType.ts';
import useApp from 'antd/es/app/useApp';
import { ModalForm, ProFormSwitch, ProFormText, ProFormTextArea } from '@ant-design/pro-components';

const DictTypeModal: React.FC<{
  id: string | null;
  modalVisit: boolean;
  onOpenChange: (show: boolean) => void;
  callback: () => void;
}> = ({ id, modalVisit, onOpenChange, callback }) => {
  const isUpdate = id && id.length > 0;
  const [form] = Form.useForm<AddOrUpdateDictTypeRequest>();
  const { message } = useApp();
  return (
    <ModalForm<AddOrUpdateDictTypeRequest>
      layout="horizontal"
      labelCol={{ flex: '90px' }}
      labelWrap
      wrapperCol={{ flex: 1 }}
      title={`${isUpdate ? '编辑' : '新增'}字典`}
      open={modalVisit}
      form={form}
      onOpenChange={(show: boolean) => {
        if (show) {
          if (isUpdate) {
            getDictType(id).then((res) => {
              form.setFieldsValue(res.data as AddOrUpdateDictTypeRequest);
            });
          }
        } else {
          form.resetFields();
        }
        onOpenChange(show);
      }}
      onFinish={async (values: AddOrUpdateDictTypeRequest) => {
        if (isUpdate) {
          values.id = id;
        }
        const apiFunc = isUpdate ? updateDictType : addDictType;
        await apiFunc(values);
        message.success('操作成功');
        onOpenChange(false);
        callback();
      }}
    >
      <ProFormText
        label="字典名称"
        name="name"
        placeholder="请输入字典名称"
        rules={[{ required: true }, { max: 128 }]}
      />
      <ProFormText
        label="字典类型"
        name="dictType"
        placeholder="请输入字典类型"
        rules={[{ required: true }, { max: 128 }]}
      />
      <ProFormSwitch label="状态" name="isEnabled" rules={[{ required: true }]} />
      <ProFormTextArea label="备注" name="remark" placeholder="请输入备注" rules={[{ max: 64 }]} />
    </ModalForm>
  );
};

export default DictTypeModal;
