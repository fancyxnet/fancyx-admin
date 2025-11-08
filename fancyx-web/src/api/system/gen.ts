import httpClient from '@/utils/httpClient.ts';
import type { AppResponse, PagedResult, PageSearch, AppOption } from '@/types/api';

/**
 * 预览生成的代码
 * @param dto
 */
export function genCode(tableId: string) {
    return httpClient.post<string, AppResponse<GenCodeResultDto>>('/admin-api/Gen/GenCode?tableId=' + tableId);
}

/**
 * 从数据库导入表
 * @param dto
 */
export function importTable(table: string) {
    return httpClient.post<string, AppResponse<GenCodeResultDto>>('/admin-api/Gen/ImportTable?table=' + table);
}

/**
 * 获取未生成过的表
 * @param dto
 */
export function getTableList(dto: GetTableQueryDto) {
    return httpClient.get<GetTableQueryDto, AppResponse<PagedResult<TableInfoDto>>>('/admin-api/Gen/GetTableList', { params: dto });
}

/**
 * 同步
 * @param dto
 */
export function genSyncFromDb(tableId: string) {
    return httpClient.post<string, AppResponse<boolean>>('/admin-api/Gen/GenSyncFromDb?tableId=' + tableId);
}

/**
 * 获取已经生成过的表
 * @param dto
 */
export function getGenTableList(dto: GenTableQueryDto) {
    return httpClient.get<GenTableQueryDto, AppResponse<PagedResult<GenTableListDto>>>('/admin-api/Gen/GetGenTableList', { params: dto });
}

/**
 * 获取已经生成过的表列信息
 * @param dto
 */
export function getGenTableColumnList(dto: GenTableColumnQueryDto) {
    return httpClient.get<GenTableColumnQueryDto, AppResponse<PagedResult<GenTableColumnListDto>>>('/admin-api/Gen/GetGenTableColumnList', { params: dto });
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
 * @param dto
 */
export function saveGenTableInfo(dto: GenTableInfoDto) {
    return httpClient.put<GenTableInfoDto, AppResponse<boolean>>('/admin-api/Gen/SaveGenTableInfo', dto);
}

/**
 * 保存生成表列配置
 * @param dto
 */
export function saveGenColumnInfo(dtos: GenTableColumnDto[]) {
    return httpClient.put<GenTableColumnDto[], AppResponse<boolean>>('/admin-api/Gen/SaveGenColumnInfo', dtos);
}

/**
 * 获取详细信息
 * @param dto
 */
export function getGenDetailsInfo(tableId: string) {
    return httpClient.get<string, AppResponse<GenDetailsInfoDto>>('/admin-api/Gen/GetGenDetailsInfo?tableId=' + tableId);
}

export interface GetTableQueryDto extends PageSearch {
    tableName?: string
}
export interface TableInfoDto {
    tableName?: string;
    tableComment?: string;
    createTime: string;
    updateTime: string;
}
export interface GenCodeResultDto {
    entity: AppOption;
    iService: AppOption;
    service: AppOption;
    controller: AppOption;
    queryDto: AppOption;
    api: AppOption;
}
export interface GenTableQueryDto extends PageSearch {
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
export interface GenTableColumnQueryDto extends PageSearch {
    tableId: string
}
export interface GenTableColumnListDto {
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
export interface GenTableInfoDto {
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
export interface GenTableColumnDto {
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
export interface GenDetailsInfoDto {
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