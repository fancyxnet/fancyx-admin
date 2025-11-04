import RichEditor from '@/components/RichEditor';

const RichText = () => {
  return <RichEditor onChange={(content) => console.log(content)} />;
};

export default RichText;