import { Spin } from 'antd';

const Fallback = () => {
  return (
    <div
      style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh',
      }}
    >
      <Spin />
    </div>
  );
};

export default Fallback;
