import * as moment from 'moment';
import { GetEmployeeArrearsForViewDto, EmployeeArrearsDto, CreateOrEditEmployeeArrearsDto } from '../dto/employeeArrears-dto';


export interface IPagedResultDtoOfGetEmployeeArrearsForViewDto {
    totalCount: number | undefined;
    items: GetEmployeeArrearsForViewDto[] | undefined;
}

export interface IGetEmployeeArrearsForViewDto {
    employeeArrears: EmployeeArrearsDto | undefined;
}

export interface IEmployeeArrearsDto {
    arrearID: number | undefined;
    employeeID: number | undefined;
    employeeName: string | undefined;
    salaryYear: number | undefined;
    salaryMonth: number | undefined;
    arrearDate: moment.Moment | undefined;
    amount: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEmployeeArrearsForEditOutput {
    employeeArrears: CreateOrEditEmployeeArrearsDto | undefined;
}

export interface ICreateOrEditEmployeeArrearsDto {
    arrearID: number;
    employeeID: number;
    employeeName: string | undefined;
    salaryYear: number;
    salaryMonth: number;
    arrearDate: moment.Moment | undefined;
    amount: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}