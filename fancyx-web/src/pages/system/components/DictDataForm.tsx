import { Form } from 'antd';
import { addDictData, type AddOrUpdateDictDataRequest, getDictData, updateDictData } from '@/api/system/dictData.ts';
import { useParams } from 'react-router-dom';
import useApp from 'antd/es/app/useApp';
import { ModalForm, ProFormDigit, ProFormSwitch, ProFormText, ProFormTextArea } from '@ant-design/pro-components';

const DictDataModal: React.FC<{
  id: string | null;
  modalVisit: boolean;
  isCopy?: boolean;
  onOpenChange: (show: boolean) => void;
  callback: () => void;
}> = ({ id, modalVisit, isCopy = false, onOpenChange, callback }) => {
  const isQuery = id && id.length > 0;
  const isUpdate = isQuery && !isCopy;
  const [form] = Form.useForm<AddOrUpdateDictDataRequest>();
  const { message } = useApp();
  const urlParams = useParams();
  return (
    <ModalForm<AddOrUpdateDictDataRequest>
      layout="horizontal"
      labelCol={{ flex: '90px' }}
      labelWrap
      wrapperCol={{ flex: 1 }}
      title={`${isUpdate ? '编辑' : '新增'}字典项`}
      open={modalVisit}
      form={form}
      onOpenChange={(show: boolean) => {
        if (show) {
          if (isQuery) {
            getDictData(id).then((res) => {
              form.setFieldsValue(res.data as AddOrUpdateDictDataRequest);
            });
          }
        } else {
          form.resetFields();
        }
        onOpenChange(show);
      }}
      onFinish={async (values: AddOrUpdateDictDataRequest) => {
        if (!urlParams?.dictType) {
          message.error('字典类型不能为空');
          return;
        }
        if (isUpdate) {
          values.id = id;
        }
        values.dictType = urlParams!.dictType;

        const apiFunc = isUpdate ? updateDictData : addDictData;
        await apiFunc(values);
        message.success('操作成功');
        form.resetFields();
        onOpenChange(false);
        callback();
      }}
    >
      <ProFormText
        label="字典标签"
        name="label"
        placeholder="请输入字典标签"
        rules={[{ required: true }, { max: 256 }]}
      />
      <ProFormText
        label="字典键值"
        name="value"
        placeholder="请输入字典键值"
        rules={[{ required: true }, { max: 128 }]}
      />
      <ProFormSwitch label="状态" name="isEnabled" rules={[{ required: true }]} />
      <ProFormDigit label="显示排序" name="sort" min={1} max={999} placeholder="排序值" rules={[{ required: true }]} />
      <ProFormTextArea label="备注" name="remark" placeholder="请输入备注" rules={[{ max: 512 }]} />
    </ModalForm>
  );
};

export default DictDataModal;
