import Permission from '@/components/Permission';
import { importTable, getGenTableList, deleteGenTable, getTableList, type GenTableListDto } from '@/api/system/gen.ts';
import { DeleteOutlined, EditOutlined, EyeOutlined, ImportOutlined } from '@ant-design/icons';
import { Button, Form, Input, message, Modal, Popconfirm, Space } from 'antd';
import React, { useRef, useState } from 'react';
import type { SmartTableRef, SmartTableColumnType } from '@/components/SmartTable/type.ts';
import SmartTable from '@/components/SmartTable';
import useApp from 'antd/es/app/useApp';
import { useNavigate } from 'react-router-dom';
import { open } from '@/store/tabStore.ts';
import { useDispatch } from 'react-redux';

const ConfigList: React.FC = () => {
    const tableRef = useRef<SmartTableRef>(null);
    const { message } = useApp();
    const [importModalVisible, setImportModalVisible] = useState<boolean>(false);
    const navigate = useNavigate();
    const dispatch = useDispatch();
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
                    <Permission mode='some' permissions={['Sys.Gen.SaveGenTableInfo', 'Sys.Gen.SaveGenColumnInfo']}>
                        <Button
                            type="link"
                            icon={<EditOutlined />}
                            key="edit"
                            onClick={() => {
                                const path = `/quickWork/genEdit/${record.tableId}`
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
                            key="edit"
                            onClick={() => {
                                //modalRef?.current?.openModal(record as ConfigDto);
                            }}
                        >
                            预览
                        </Button>
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
                    <Form.Item label="表名" name="tableName" key='tableName'>
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
        </>
    );
};

const ImportModal: React.FC<{
    show: boolean,
    onOk: () => void,
    onCancel: () => void
}> = ({ show, onOk, onCancel }) => {
    const tableRef = useRef<SmartTableRef>(null);
    const columns = [
        {
            title: '表名',
            dataIndex: 'tableName'
        },
        {
            title: '描述',
            dataIndex: 'tableComment'
        },
        {
            title: '创建时间',
            dataIndex: 'createTime'
        },
        {
            title: '修改时间',
            dataIndex: 'updateTime'
        }
    ]
    return (
        <Modal
            title="导入"
            width='60%'
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
                })
            }}
            maskClosable={false}>
            <SmartTable
                ref={tableRef}
                selection={true}
                selectionType='radio'
                rowKey='tableName'
                columns={columns}
                searchItems={[
                    <Form.Item label="表名" name="tableName" key='tableName'>
                        <Input placeholder="请输入表名" />
                    </Form.Item>,
                ]}
                request={async (params) => {
                    const { data } = await getTableList(params);
                    return data;
                }}></SmartTable>
        </Modal >
    )
}

export default ConfigList;
