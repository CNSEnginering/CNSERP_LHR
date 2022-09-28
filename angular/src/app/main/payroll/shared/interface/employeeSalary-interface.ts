import * as moment from 'moment';
import { GetEmployeeSalaryForViewDto, EmployeeSalaryDto, CreateOrEditEmployeeSalaryDto } from '../dto/employeeSalary-dto';


export interface IPagedResultDtoOfGetEmployeeSalaryForViewDto {
    totalCount: number | undefined;
    items: GetEmployeeSalaryForViewDto[] | undefined;
}

export interface IPagedResultDtoOfEmployeeSalaryDto{
    totalCount: number | undefined;
    items: EmployeeSalaryDto[] | undefined;
}

export interface IGetEmployeeSalaryForViewDto {
    employeeSalary: EmployeeSalaryDto | undefined;
}

export interface IEmployeeSalaryDto {
    employeeID: number | undefined;
    employeeName: string | undefined;
    bank_Amount: number | undefined;
    startDate: moment.Moment | undefined;
    gross_Salary: number | undefined;
    basic_Salary: number | undefined;
    tax: number | undefined;
    house_Rent: number | undefined;
    net_Salary: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEmployeeSalaryForEditOutput {
    employeeSalary: CreateOrEditEmployeeSalaryDto | undefined;
}

export interface ICreateOrEditEmployeeSalaryDto {
    employeeID: number | undefined;
    employeeName: string | undefined;
    bank_Amount: number | undefined;
    startDate: moment.Moment | undefined;
    gross_Salary: number | undefined;
    basic_Salary: number | undefined;
    tax: number | undefined;
    house_Rent: number | undefined;
    net_Salary: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}