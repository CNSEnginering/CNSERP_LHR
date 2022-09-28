import * as moment from 'moment';
import { employeeLeaveBalanceDto, GetemployeeLeaveBalanceForViewDto, CreateOrEditemployeeLeaveBalanceDto } from '../dto/employeeLeaveBalance-dto';
export interface IPagedResultDtoOfGetemployeeLeaveBalanceForViewDto {
    totalCount: number | undefined;
    items: GetemployeeLeaveBalanceForViewDto[] | undefined;
}

export interface IGetemployeeLeaveBalanceForViewDto {
    employeeLeavesTotal: employeeLeaveBalanceDto | undefined;
}

export interface IGetemployeeLeaveBalanceForEditOutput {
    employeeLeavesTotal: CreateOrEditemployeeLeaveBalanceDto | undefined;
}

export interface ICreateOrEditemployeeLeaveBalanceDto {
    employeeID: number | undefined;
    salaryYear: number | undefined;
    leaves: number | undefined;
    casual:  number;
    sick:  number;
    annual:  number;
    cpl:  number;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IemployeeLeaveBalanceDto {
    employeeID: number | undefined;
    salaryYear: number | undefined;
    leaves: number | undefined;
    casual:  number;
    sick:  number;
    annual:  number;
    cpl:  number;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}