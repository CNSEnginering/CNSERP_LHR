import * as moment from 'moment';
import { GetEmployeeEarningsForViewDto, EmployeeEarningsDto, CreateOrEditEmployeeEarningsDto } from '../dto/employeeEarnings-dto';


export interface IPagedResultDtoOfGetEmployeeEarningsForViewDto {
    totalCount: number | undefined;
    items: GetEmployeeEarningsForViewDto[] | undefined;
}

export interface IGetEmployeeEarningsForViewDto {
    employeeEarnings: EmployeeEarningsDto | undefined;
}

export interface IEmployeeEarningsDto {
    earningID: number | undefined;
    employeeID: number | undefined;
    employeeName: string | undefined;
    salaryYear: number | undefined;
    salaryMonth: number | undefined;
    earningDate: moment.Moment | undefined;
    amount: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEmployeeEarningsForEditOutput {
    employeeEarnings: CreateOrEditEmployeeEarningsDto | undefined;
}

export interface ICreateOrEditEmployeeEarningsDto {
    earningID: number;
    employeeID: number;
    employeeName: string | undefined;
    salaryYear: number;
    salaryMonth: number;
    earningDate: moment.Moment | undefined;
    amount: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}