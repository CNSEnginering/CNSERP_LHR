import * as moment from 'moment';
import { GetEmployeeDeductionsForViewDto, EmployeeDeductionsDto, CreateOrEditEmployeeDeductionsDto } from '../dto/employeeDeductions-dto';


export interface IPagedResultDtoOfGetEmployeeDeductionsForViewDto {
    totalCount: number | undefined;
    items: GetEmployeeDeductionsForViewDto[] | undefined;
}

export interface IGetEmployeeDeductionsForViewDto {
    employeeDeductions: EmployeeDeductionsDto | undefined;
}

export interface IEmployeeDeductionsDto {
    deductionID: number | undefined;
    employeeID: number | undefined;
    employeeName: string | undefined;
    remarks: string | undefined;
    salaryYear: number | undefined;
    salaryMonth: number | undefined;
    deductionDate: moment.Moment | undefined;
    amount: number | undefined;
    deductionType: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEmployeeDeductionsForEditOutput {
    employeeDeductions: CreateOrEditEmployeeDeductionsDto | undefined;
}

export interface ICreateOrEditEmployeeDeductionsDto {
    deductionID: number;
    employeeID: number;
    employeeName: string | undefined;
    remarks: string | undefined;
    deductionType: number | undefined;
    salaryYear: number;
    salaryMonth: number;
    deductionDate:  Date | undefined;
    amount: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate:  Date | undefined;
    createdBy: string | undefined;
    createDate:  Date | undefined;
    id: number | undefined;
    detId:number |undefined;
}
