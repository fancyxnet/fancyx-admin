import httpClient from '@/utils/httpClient';
import type { AppResponse, PagedResult, PageSearch } from '@/types/api';

/**
 * 新增员工
 * @param dto
 */
export function addEmployee(dto: EmployeeDto) {
  return httpClient.post<EmployeeDto, AppResponse<boolean>>('/api/employee/add', dto);
}

/**
 * 员工列表
 * @param dto
 */
export function getEmployeePagedList(dto: EmployeeQueryDto) {
  return httpClient.get<EmployeeQueryDto, AppResponse<PagedResult<EmployeeListDto>>>('/api/employee/list', {
    params: dto,
  });
}

/**
 * 员工列表
 * @param dto
 */
export function getEmployeeList(dto: EmployeePureQueryDto) {
  return httpClient.get<string, AppResponse<EmployeeDto[]>>('/api/employee/GetEmployeeList', {
    params: dto,
  });
}

/**
 * 修改员工
 * @param dto
 */
export function updateEmployee(dto: EmployeeDto) {
  return httpClient.put<EmployeeDto, AppResponse<boolean>>('/api/employee/update', dto);
}

/**
 * 删除员工
 * @param id
 */
export function deleteEmployee(id: string) {
  return httpClient.delete<AppResponse<boolean>>('/api/employee/delete/' + id);
}

/**
 * 绑定用户
 * @param dto
 */
export function bindUser(dto: EmployeeBindUserDto) {
  return httpClient.post<AppResponse<boolean>>('/api/employee/bindUser', dto);
}

/**
 * 获取员工信息
 * @param id
 */
export function getEmployeeInfo(id: string) {
  return httpClient.get<string, AppResponse<EmployeeInfoDto>>('/api/employee/info/' + id);
}

/**
 * 部门+员工树形
 * @param dto
 * @returns
 */
export function getDeptEmployeeTree(dto?: DeptEmployeeTreeQueryDto) {
  return httpClient.get<DeptEmployeeTreeQueryDto, AppResponse<DeptEmployeeTreeDto[]>>(
    '/api/employee/getDeptEmployeeTree',
    {
      params: dto,
    },
  );
}

export interface EmployeeDto {
  id?: string | null;
  name: string;
  code: string;
  sex: number;
  phone: string;
  idNo: string;
  frontIdNoUrl?: string | null;
  backIdNoUrl?: string | null;
  birthday: string;
  address?: string | null;
  email?: string | null;
  inTime?: string | null;
  status: number;
  userId: string;
  deptId: string;
  positionId: string;
  isAddUser?: boolean;
}

export interface EmployeeQueryDto extends PageSearch {
  keyword?: string | null;
  deptId?: string;
}

export interface EmployeePureQueryDto {
  keyword?: string | null;
  deptId?: string;
}

export interface EmployeeListDto {
  id: string;
  name: string;
  code: string;
  sex: number;
  phone: string;
  idNo: string;
  frontIdNoUrl: string | null;
  backIdNoUrl: string | null;
  birthday: string;
  address: string | null;
  email: string | null;
  inTime: string | null;
  outTime: string | null;
  status: number;
  userId: string;
  deptId: string;
  positionId: string;
  deptName: string;
  positionName: string;
}

export interface EmployeeBindUserDto {
  userId?: string;
  employeeId: string;
}

export interface EmployeeInfoDto extends EmployeeListDto {
  userName?: string | null;
  nickName?: string | null;
}

export interface DeptEmployeeTreeDto {
  label: string;
  value: string;
  type: number;
  children: DeptEmployeeTreeDto[];
  disabled: boolean;
}

export interface DeptEmployeeTreeQueryDto {
  employeeName?: string;
}
