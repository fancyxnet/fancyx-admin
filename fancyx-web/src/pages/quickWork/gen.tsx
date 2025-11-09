import Permission from '@/components/Permission';
import {
    genCode,
    importTable,
    getGenTableList,
    deleteGenTable,
    getTableList,
    genSyncFromDb,
    type GenTableListDto,
    type GenCodeResultDto,
} from '@/api/system/gen.ts';
import { DeleteOutlined, EditOutlined, EyeOutlined, ImportOutlined } from '@ant-design/icons';
import { Button, Form, Input, message, Modal, Popconfirm, Space, Tabs, type TabsProps } from 'antd';
import React, { useRef, useState } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import useApp from 'antd/es/app/useApp';
import { useNavigate } from 'react-router-dom';
import { open } from '@/store/tabStore.ts';
import { useDispatch } from 'react-redux';
import ProIcon from '@/components/ProIcon';

const { TextArea } = Input;

const GenList: React.FC = () => {
    const tableRef = useRef<SmartTableRef>(null);
    const { message } = useApp();
    const [importModalVisible, setImportModalVisible] = useState<boolean>(false);
    const [perviewModalVisible, setPreviewModalVisible] = useState<boolean>(false);
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const [code, setCode] = useState<GenCodeResultDto>();
    const columns: SmartTableColumnType[] = [
        {
            title: '表名',
            dataIndex: 'tableName',
        },
        {
            title: '表描述',
            dataIndex: 'tableComment',
        },
        {
            title: '类名',
            dataIndex: 'className',
        },
        {
            title: '命名空间',
            dataIndex: 'namespaceName',
        },
        {
            title: '模块',
            dataIndex: 'moduleName',
        },
        {
            title: '业务名',
            dataIndex: 'businessName',
        },
        {
            title: '操作',
            dataIndex: 'option',
            width: 210,
            fixed: 'right',
            render: (_: any, record: GenTableListDto) => (
                <Space>
                    <Permission mode="some" permissions={['Sys.Gen.SaveGenTableInfo', 'Sys.Gen.SaveGenColumnInfo']}>
                        <Button
                            type="link"
                            icon={<EditOutlined />}
                            key="edit"
                            onClick={() => {
                                const path = `/quickWork/genEdit/${record.tableId}`;
                                navigate(path);
                                dispatch(open({ name: `修改${record.tableName}生成配置`, path: path }));
                            }}
                        >
                            编辑
                        </Button>
                    </Permission>
                    <Permission permissions={'Sys.Gen.GenCode'}>
                        <Button
                            type="link"
                            icon={<EyeOutlined />}
                            key="preview"
                            onClick={() => {
                                genCode(record.tableId).then((res) => {
                                    setCode(res.data);
                                    setPreviewModalVisible(true);
                                });
                            }}
                        >
                            预览
                        </Button>
                    </Permission>
                    <Permission permissions={'Sys.Gen.GenSyncFromDb'}>
                        <Popconfirm
                            key="sync"
                            title="确定同步吗？"
                            onConfirm={() => {
                                genSyncFromDb(record.tableId).then(() => {
                                    message.success('同步成功');
                                    tableRef.current?.reload();
                                });
                            }}
                        >
                            <Button type="link" key="sync">
                                <ProIcon icon="iconify:mage:reload" /> 同步
                            </Button>
                        </Popconfirm>
                    </Permission>
                    <Permission permissions={'Sys.Gen.DeleteGenTable'}>
                        <Popconfirm
                            key="delete"
                            title="确定删除吗？"
                            description="删除后无法撤销"
                            onConfirm={() => {
                                deleteGenTable(record.tableId).then(() => {
                                    message.success('删除成功');
                                    tableRef.current?.reload();
                                });
                            }}
                        >
                            <Button type="link" danger icon={<DeleteOutlined />}>
                                删除
                            </Button>
                        </Popconfirm>
                    </Permission>
                </Space>
            ),
        },
    ];

    return (
        <>
            <SmartTable
                columns={columns}
                ref={tableRef}
                rowKey="id"
                request={async (params) => {
                    const { data } = await getGenTableList(params);
                    return data;
                }}
                searchItems={[
                    <Form.Item label="表名" name="tableName" key="tableName">
                        <Input placeholder="请输入表名" />
                    </Form.Item>,
                ]}
                toolbar={
                    <Space size="middle">
                        <Permission permissions={'Sys.Gen.ImportTable'}>
                            <Button
                                type="primary"
                                key="primary"
                                onClick={() => {
                                    setImportModalVisible(true);
                                }}
                            >
                                <ImportOutlined /> 导入
                            </Button>
                        </Permission>
                    </Space>
                }
            />
            {/** 导入弹窗 */}
            <ImportModal
                show={importModalVisible}
                onOk={() => {
                    tableRef?.current?.reload();
                    setImportModalVisible(false);
                }}
                onCancel={() => {
                    setImportModalVisible(false);
                }}
            />
            {/** 预览弹窗 */}
            <PerviewModal
                code={code}
                show={perviewModalVisible}
                onOk={() => {
                    setPreviewModalVisible(false);
                }}
                onCancel={() => {
                    setPreviewModalVisible(false);
                }}
            />
        </>
    );
};

const ImportModal: React.FC<{
    show: boolean;
    onOk: () => void;
    onCancel: () => void;
}> = ({ show, onOk, onCancel }) => {
    const tableRef = useRef<SmartTableRef>(null);
    const columns = [
        {
            title: '表名',
            dataIndex: 'tableName',
        },
        {
            title: '描述',
            dataIndex: 'tableComment',
        },
        {
            title: '创建时间',
            dataIndex: 'createTime',
        },
        {
            title: '修改时间',
            dataIndex: 'updateTime',
        },
    ];
    return (
        <Modal
            title="导入"
            width="60%"
            open={show}
            onCancel={() => {
                onCancel();
            }}
            onOk={() => {
                const keys = tableRef?.current?.getSelectedKeys();
                if (!keys?.length || keys?.length < 0) {
                    message.error('请选择一条数据');
                    return;
                }
                importTable(keys[0] as string).then(() => {
                    tableRef?.current?.reload();
                    onOk();
                });
            }}
            maskClosable={false}
        >
            <SmartTable
                ref={tableRef}
                selection={true}
                selectionType="radio"
                rowKey="tableName"
                columns={columns}
                searchItems={[
                    <Form.Item label="表名" name="tableName" key="tableName">
                        <Input placeholder="请输入表名" />
                    </Form.Item>,
                ]}
                request={async (params) => {
                    const { data } = await getTableList(params);
                    return data;
                }}
            ></SmartTable>
        </Modal>
    );
};

const PerviewModal: React.FC<{
    show: boolean;
    onOk: () => void;
    onCancel: () => void;
    code: GenCodeResultDto | undefined;
}> = ({ show, code, onCancel }) => {
    const [activeKey, setActiveKey] = useState<string>('entity');
    const renderCode = (name?: string, content?: string) => {
        if (!content || content.length < 0) return <p></p>
        const copy = async () => {
            await navigator.clipboard.writeText(content);
            message.success(`${name}已复制`);
        }
        return <div>
            <a type='link' className='mr-10 mb-1' style={{ float: 'right' }} onClick={copy}><ProIcon icon='antd:CopyOutlined' />复制</a>
            <TextArea className='w-full border-none' value={content} autoSize />
        </div>;
    };
    const items: TabsProps['items'] = [
        {
            key: 'entity',
            label: code?.entity?.label,
            children: renderCode(code?.entity?.label, code?.entity?.value),
        },
        {
            key: 'iService',
            label: code?.iService?.label,
            children: renderCode(code?.iService?.label, code?.iService?.value),
        },
        {
            key: 'service',
            label: code?.service?.label,
            children: renderCode(code?.service?.label, code?.service?.value),
        },
        {
            key: 'controller',
            label: code?.controller?.label,
            children: renderCode(code?.controller?.label, code?.controller?.value),
        },
        {
            key: 'queryDto',
            label: code?.queryDto?.label,
            children: renderCode(code?.queryDto?.label, code?.queryDto?.value),
        },
        {
            key: 'api',
            label: code?.api?.label,
            children: renderCode(code?.api?.label, code?.api?.value),
        }
    ];
    const onTabChange = (key: string) => {
        setActiveKey(key);
    };

    return (
        <Modal
            title="代码预览"
            width="80%"
            open={show}
            footer={null}
            maskClosable={false}
            onCancel={onCancel}
        >
            <div style={{ maxHeight: "550px", overflow: "auto" }}>
                <Tabs defaultActiveKey={activeKey} items={items} onChange={onTabChange} />
            </div>
        </Modal>
    );
};

export default GenList;
