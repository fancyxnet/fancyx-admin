import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch, AppOption } from '@/types/api';

/**
 * 预览生成的代码
 * @param req
 */
export function genCode(tableId: string) {
    return httpClient.post<string, AppResponse<GenCodeResponse>>('/admin-api/Gen/GenCode?tableId=' + tableId);
}

/**
 * 从数据库导入表
 * @param req
 */
export function importTable(table: string) {
    return httpClient.post<string, AppResponse<GenCodeResponse>>('/admin-api/Gen/ImportTable?table=' + table);
}

/**
 * 获取未生成过的表
 * @param req
 */
export function getTableList(req: GetTableListRequest) {
    return httpClient.get<GetTableListRequest, AppResponse<PagedResult<TableInfoDto>>>('/admin-api/Gen/GetTableList', { params: req });
}

/**
 * 同步
 * @param req
 */
export function genSyncFromDb(tableId: string) {
    return httpClient.post<string, AppResponse<boolean>>('/admin-api/Gen/GenSyncFromDb?tableId=' + tableId);
}

/**
 * 获取已经生成过的表
 * @param req
 */
export function getGenTableList(req: GetGenTableListRequest) {
    return httpClient.get<GetGenTableListRequest, AppResponse<PagedResult<GenTableListDto>>>('/admin-api/Gen/GetGenTableList', { params: req });
}

/**
 * 获取已经生成过的表列信息
 * @param req
 */
export function getGenTableColumnList(req: GenTableColumnRequest) {
    return httpClient.get<GenTableColumnRequest, AppResponse<PagedResult<GenTableColumnItem>>>('/admin-api/Gen/GetGenTableColumnList', { params: req });
}

/**
 * 删除生成表配置
 * @param id
 */
export function deleteGenTable(tableId: string) {
    return httpClient.delete<string, AppResponse<boolean>>(`/admin-api/Gen/DeleteGenTable/${tableId}`);
}

/**
 * 保存生成表配置
 * @param req
 */
export function saveGenTableInfo(req: SaveGenTableInfoRequest) {
    return httpClient.put<SaveGenTableInfoRequest, AppResponse<boolean>>('/admin-api/Gen/SaveGenTableInfo', req);
}

/**
 * 保存生成表列配置
 * @param req
 */
export function saveGenColumnInfo(dtos: SaveGenColumnInfoItem[]) {
    return httpClient.put<SaveGenColumnInfoItem[], AppResponse<boolean>>('/admin-api/Gen/SaveGenColumnInfo', dtos);
}

/**
 * 获取详细信息
 * @param req
 */
export function getGenDetailsInfo(tableId: string) {
    return httpClient.get<string, AppResponse<GenDetails>>('/admin-api/Gen/GetGenDetailsInfo?tableId=' + tableId);
}

export interface GetTableListRequest extends PageSearch {
    tableName?: string
}
export interface TableInfoDto {
    tableName?: string;
    tableComment?: string;
    createTime: string;
    updateTime: string;
}
export interface GenCodeResponse {
    entity: AppOption;
    iService: AppOption;
    service: AppOption;
    controller: AppOption;
    queryDto: AppOption;
    api: AppOption;
    page: AppOption;
}
export interface GetGenTableListRequest extends PageSearch {
    tableName?: string
}
export interface GenTableListDto {
    tableId: string;
    tableName?: string;
    tableComment?: string;
    className?: string;
    tplCategory?: string;
    namespaceName?: string;
    moduleName?: string;
    businessName?: string;
    genType?: string;
    genPath?: string;
    options?: string;
    remark?: string;
}
export interface GenTableColumnRequest extends PageSearch {
    tableId: string
}
export interface GenTableColumnItem {
    columnId: string;
    tableId: string;
    columnName?: string;
    columnComment?: string;
    columnType?: string;
    csharpType?: string;
    tsType?: string;
    csharpField?: string;
    isPk: boolean;
    isIncrement: boolean;
    isRequired: boolean;
    isInsert: boolean;
    isList: boolean;
    isQuery: boolean;
    queryType?: string;
    htmlType?: string;
    dictType?: string;
    sort: number;
}
export interface SaveGenTableInfoRequest {
    tableId: string;
    tableComment?: string;
    className?: string;
    tplCategory?: string;
    namespaceName?: string;
    moduleName?: string;
    businessName?: string;
    genType?: string;
    genPath?: string;
    options?: string;
    remark?: string;
}
export interface SaveGenColumnInfoItem {
    columnId: string;
    columnName?: string;
    columnComment?: string;
    columnType?: string;
    csharpType?: string;
    csharpField?: string;
    isPk: boolean;
    isIncrement: boolean;
    isRequired: boolean;
    isInsert: boolean;
    isList: boolean;
    isQuery: boolean;
    queryType?: string;
    htmlType?: string;
    dictType?: string;
    sort: number;
}
export interface GenDetails {
    tableId: string;
    tableName?: string;
    tableComment?: string;
    className?: string;
    tplCategory?: string;
    namespaceName?: string;
    moduleName?: string;
    businessName?: string;
    genType?: string;
    genPath?: string;
    options?: string;
    remark?: string;
}