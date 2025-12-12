import { useRef } from 'react';
import { Editor } from '@tinymce/tinymce-react';
import './index.scss';
import { useApplication } from '../Application';
import { uploadFile } from '@/api/oss';

// 定义组件的Props接口
interface RichEditorProps {
  // onChange回调函数，当编辑器内容改变时触发
  onChange?: (content: string) => void;
  // 初始内容
  initialValue?: string;
}

// 定义TinyMCE编辑器实例的类型
type TinyMCEInstance = any;

export default function RichEditor({ onChange, initialValue = '' }: RichEditorProps) {
  const editorRef = useRef<TinyMCEInstance>(null);

  // 处理编辑器内容变化的函数
  const handleEditorChange = (content: string) => {
    // 如果提供了onChange回调，则调用它并传递当前内容
    if (onChange) {
      onChange(content);
    }
  };

  const ImagesUploadHandler = async (blobInfo: any) => {
    const { data } = await uploadFile(blobInfo.blob());
    return data;
  };

  return (
    <>
      <Editor
        tinymceScriptSrc="/tinymce/tinymce.min.js"
        licenseKey="gpl"
        onInit={(_evt, editor) => {
          editorRef.current = editor as unknown as TinyMCEInstance;
        }}
        // 使用传入的initialValue或默认值
        initialValue={initialValue || ''}
        // 配置内容变化事件监听
        onEditorChange={(content) => handleEditorChange(content)}
        init={{
          height: 500,
          menubar: false,
          language: 'zh_CN',
          plugins: [
            'advlist',
            'autolink',
            'lists',
            'link',
            'image',
            'charmap',
            'anchor',
            'searchreplace',
            'visualblocks',
            'code',
            'fullscreen',
            'insertdatetime',
            'media',
            'table',
            'preview',
            'help',
            'wordcount',
            'image',
          ],
          toolbar:
            'undo redo | blocks | ' +
            'bold italic forecolor | alignleft aligncenter ' +
            'alignright alignjustify | bullist numlist outdent indent | ' +
            ' image | ' +
            'removeformat | help',
          content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }',
          paste_data_images: true,
          // 启用实时内容变化事件
          setup: (editor: any) => {
            editor.on('input change', () => {
              const content = editor.getContent();
              handleEditorChange(content);
            });
          },
          images_upload_handler: ImagesUploadHandler,
        }}
      />
    </>
  );
}
