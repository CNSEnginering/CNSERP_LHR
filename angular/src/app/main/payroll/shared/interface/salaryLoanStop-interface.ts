import * as moment from 'moment';
import { GetSalaryLoanStopForViewDto, SalaryLoanStopDto, CreateOrEditSalaryLoanStopDto } from '../dto/SalaryLoanStop-dto';

export interface ISalaryLoanStopDto {
    typeID: number | undefined;
    employeeID: number | undefined;
    salaryYear: number | undefined;
    salaryMonth: number | undefined;
    active:boolean|undefined;
    id: number | undefined;
    include:boolean |undefined;
    remarks:string|undefined;
    loanID:number|undefined;
}

export interface ICreateOrEditSalaryLoanStopDto {
    typeID: number|undefined;
    employeeID: number | undefined;
    salaryYear: number| undefined;
    salaryMonth: number | undefined;
    id: number | undefined;
    active: boolean | undefined;
    remarks: string | undefined;
    include:boolean |undefined;
    loanID:number|undefined;
}

export interface IGetSalaryLoanStopForViewDto {
    SalaryLoanStop: SalaryLoanStopDto | undefined;
}

export interface IPagedResultDtoOfGetSalaryLoanStopForViewDto {
    totalCount: number | undefined;
    items: GetSalaryLoanStopForViewDto[] | undefined;
}

export interface IGetSalaryLoanStopForEditOutput {
    SalaryLoanStop: CreateOrEditSalaryLoanStopDto | undefined;
}

