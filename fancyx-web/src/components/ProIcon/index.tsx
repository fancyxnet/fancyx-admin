import * as icons from '@ant-design/icons';
import { Icon } from '@iconify/react';
import React from 'react';

const ProIcon = ({
  icon,
  width,
  height,
  className,
}: {
  icon: string;
  width?: number;
  height?: number;
  className?: string;
}) => {
  const arr = icon.split(':');
  const type = arr[0];
  const name = icon.substring(type.length + 1);
  if (type === 'antd') {
    const AntdIcon = icons[name as keyof typeof icons] as React.FC;
    return AntdIcon ? (
      <span className={className}>
        <AntdIcon />
      </span>
    ) : null;
  } else if (type == 'iconify') {
    return <Icon icon={name} width={width} height={height} className={className} />;
  } else {
    return <span>icon-type not found</span>;
  }
};

export default ProIcon;
