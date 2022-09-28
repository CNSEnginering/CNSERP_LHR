import * as moment from 'moment';
import { GetEmployeeLeavesForViewDto, EmployeeLeavesDto, CreateOrEditEmployeeLeavesDto } from '../dto/employeeLeaves-dto';


export interface IPagedResultDtoOfGetEmployeeLeavesForViewDto {
    totalCount: number | undefined;
    items: GetEmployeeLeavesForViewDto[] | undefined;
}

export interface IGetEmployeeLeavesForViewDto {
    employeeLeaves: EmployeeLeavesDto | undefined;
}

export interface IEmployeeLeavesDto {
    employeeID: number | undefined;
    leaveID: number | undefined;
    salaryYear: number | undefined;
    salaryMonth: number | undefined;
    startDate: moment.Moment | undefined;
    leaveType: number | undefined;
    casual: number | undefined;
    sick: number | undefined;
    annual: number | undefined;
    payType: string | undefined;
    remarks: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEmployeeLeavesForEditOutput {
    employeeLeaves: CreateOrEditEmployeeLeavesDto | undefined;
}

export interface ICreateOrEditEmployeeLeavesDto {
    employeeID: number;
    leaveID: number;
    salaryYear: number;
    salaryMonth: number | undefined;
    startDate: moment.Moment | undefined;
    leaveType: number | undefined;
    casual: number | undefined;
    sick: number | undefined;
    annual: number | undefined;
    payType: string | undefined;
    remarks: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}
