import { Button, Card, Dropdown, Form, type MenuProps, Space, Table, type TablePaginationConfig, Tooltip } from 'antd';
import React, { type ForwardedRef, forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import type { SmartTableColumnType, SmartTableProps, SmartTableRef } from './type';
import useDeepCompareEffect from 'use-deep-compare-effect';
import { ColumnHeightOutlined, ReloadOutlined } from '@ant-design/icons';
import { useSelector } from 'react-redux';
import { selectSize } from '@/store/themeStore.ts';
import type { TableProps } from 'antd';

type TableRowSelection<T extends object = object> = TableProps<T>['rowSelection'];

const SmartTable = forwardRef<SmartTableRef, SmartTableProps<any>>(
  <T extends object = any>(props: SmartTableProps<T>, ref: ForwardedRef<SmartTableRef>) => {
    const { selectionType, columns, selection = false, showPagination = true, ...restProps } = props;
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [queryParams, setQueryParams] = useState({
      current: 1,
      pageSize: 10,
    });
    const [total, setTotal] = useState<number>(0);
    const [dataSource, setDataSource] = useState<T[]>([]);
    const size = useSelector(selectSize);
    const [tableSize, setTableSize] = useState(props.size);
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

    useImperativeHandle(ref, () => ({
      reload: async () => await fetchData(),
      getSelectedKeys: () => selectedRowKeys,
      setQueryFormFieldValue: (field, value) => {
        form.setFieldValue(field, value);
      },
      getData: () => dataSource
    }));

    useEffect(() => {
      setTableSize(size);
    }, [size]);

    const fetchData = async () => {
      if (props.request) {
        setLoading(true);
        try {
          // 如果关闭分页，传递pageSize为-1表示获取全部数据
          const requestParams = showPagination ? queryParams : { ...queryParams, pageSize: -1 };
          const result = await props.request(requestParams);

          // 只有在显示分页时才需要处理分页逻辑
          if (showPagination && (result.items === null || result.items.length === 0)) {
            if (result.totalCount > 0) {
              setQueryParams({
                ...queryParams,
                current: 1,
              });
            }
          }
          setTotal(result.totalCount);
          setDataSource(result.items);
        } finally {
          setLoading(false);
        }
      } else {
        if (Array.isArray(props.dataSource)) {
          setDataSource(props.dataSource);
        }
      }
    };

    useDeepCompareEffect(() => {
      fetchData();
    }, [queryParams]);

    const columnWidthItems: MenuProps['items'] = [
      {
        key: 'large',
        label: '宽松',
      },
      {
        key: 'middle',
        label: '中等',
      },
      {
        key: 'small',
        label: '紧凑',
      },
    ];
    const columnWidthItemClick = ({ key }: { key: string }) => {
      setTableSize(key as 'large' | 'middle' | 'small');
    };

    const handleTableChange = (pagination: TablePaginationConfig) => {
      setQueryParams({
        ...queryParams,
        current: pagination.current ?? 1,
        pageSize: pagination.pageSize ?? 10,
      });
    };

    const onSearch = () => {
      setQueryParams({ ...queryParams, ...form.getFieldsValue() });
    };
    const onReset = () => {
      form.resetFields();
      onSearch();
    };
    const onSelectChange = (newSelectedRowKeys: React.Key[]) => {
      setSelectedRowKeys(newSelectedRowKeys);
    };
    const rowSelection: TableRowSelection<T> = {
      selectedRowKeys,
      onChange: onSelectChange,
    };
    const withEmptyValue = (columns: SmartTableColumnType[]) => {
      return columns.map((column) => {
        const originalRender = column.render;

        return {
          ...column,
          render: (value: any, record: any, index: number) => {
            // 如果原本有自定义渲染函数，先执行它
            if (originalRender) {
              const renderedValue = originalRender(value, record, index);
              // 如果渲染结果是假值，也显示占位符
              if (renderedValue === null || renderedValue === undefined || renderedValue === '') {
                return <span>--</span>;
              }
              return renderedValue;
            }
            // 如果没有值，显示默认占位符
            if (value === null || value === undefined || value === '') {
              value = '--';
            }

            return <span>{`${value}`}</span>;
          },
        };
      });
    };

    return (
      <div className="fancyx-table-wrapper">
        {props.searchItems && (
          <Card className="mb-1">
            <Form layout="inline" form={form} initialValues={{ layout: 'inline' }} style={{ maxWidth: 'none' }}>
              {Array.isArray(props.searchItems)
                ? React.Children.map(props.searchItems, (child, index) => {
                  if (React.isValidElement(child)) {
                    return React.cloneElement(child, {
                      key: child.key || `child-${index}`,
                    });
                  }
                  return child;
                })
                : props.searchItems}
              <Form.Item>
                <Space>
                  <Button type="primary" onClick={onSearch}>
                    查询
                  </Button>
                  <Button onClick={onReset}>重置</Button>
                </Space>
              </Form.Item>
            </Form>
          </Card>
        )}

        <Card>
          {/* 新增/编辑等操作栏 */}
          <div className="flex space-between">
            <div className="custom-toolbar mb-2">
              {Array.isArray(props.toolbar)
                ? React.Children.map(props.toolbar, (child, index) => {
                  if (React.isValidElement(child)) {
                    return React.cloneElement(child, {
                      key: child.key || `child-${index}`,
                    });
                  }
                  return child;
                })
                : props.toolbar}
            </div>
            <div className={'right-operation-toolbar ' + (props.toolbar ? 'mt-1' : 'mb-1')}>
              <Tooltip title="刷新">
                <Button color="default" variant="link" icon={<ReloadOutlined />} onClick={fetchData}></Button>
              </Tooltip>
              <Tooltip title="列宽">
                <Dropdown
                  menu={{
                    items: columnWidthItems,
                    onClick: columnWidthItemClick,
                    activeKey: tableSize,
                  }}
                  trigger={['click']}
                >
                  <Button color="default" variant="link" icon={<ColumnHeightOutlined />}></Button>
                </Dropdown>
              </Tooltip>
            </div>
          </div>
          <Table
            {...restProps}
            bordered
            dataSource={dataSource}
            columns={withEmptyValue(columns)}
            rowKey={props.rowKey ?? 'id'}
            size={tableSize}
            pagination={showPagination ? {
              current: queryParams.current,
              pageSize: queryParams.pageSize,
              total: total,
              showSizeChanger: true,
              showQuickJumper: true,
              pageSizeOptions: [10, 20, 50, 100].map(String),
              showTotal: (total: number) => `共 ${total} 条`,
            } : false}
            scroll={undefined}
            onChange={handleTableChange}
            loading={loading}
            rowSelection={selection ? { type: selectionType, ...rowSelection } : undefined}
          />
          {/* 当关闭分页但需要显示总条数时，在表格下方显示总条数 */}
          {!showPagination && (
            <div
              className="smart-table-total-count"
              style={{
                textAlign: 'right',
                padding: '12px 16px 0',
                fontSize: '14px',
                color: '#8c8c8c',
                borderTop: '1px solid #f0f0f0'
              }}
            >
              共 {total} 条数据
            </div>
          )}
        </Card>
      </div>
    );
  },
);

export default SmartTable;
