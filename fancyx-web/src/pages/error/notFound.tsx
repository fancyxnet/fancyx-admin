import React from "react";
import { Button, Result } from "antd";
import { useNavigate } from "react-router";

export const NotFoundTip: React.FC = () => {
  return (
    <Result
      status="404"
      title="404"
      subTitle="抱歉，您访问的页面不存在。"
    />
  );
};

const NotFound: React.FC = () => {
  const navigate = useNavigate();
  return (
    <Result
      status="404"
      title="404"
      subTitle="抱歉，您访问的页面不存在。"
      extra={
        <Button type="primary" onClick={() => navigate(-1)}>
          返回上一页
        </Button>
      }
    />
  );
};

export default NotFound;
