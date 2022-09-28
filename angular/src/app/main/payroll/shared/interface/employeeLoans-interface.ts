import * as moment from 'moment';
import { GetemployeeLoansForViewDto, employeeLoansDto, CreateOrEditemployeeLoansDto } from '../dto/employeeLoans-dto';

export interface IemployeeLoansDto {
    loanID: number | undefined;
    employeeID: number | undefined;
    loanDate: string| undefined;
    amount: string | undefined;
    noi: number | undefined;
    id: number | undefined;
}

export interface ICreateOrEditemployeeLoansDto {
    LoanID: number | undefined;
    EmployeeID: number | undefined;
    LoanDate:string | undefined;
    Amount: number | undefined;
    NOI: number | undefined;
    id: number | undefined;
}

export interface IGetemployeeLoansForViewDto {
    employeeLoans: employeeLoansDto | undefined;
}

export interface IPagedResultDtoOfGetemployeeLoansForViewDto {
    totalCount: number | undefined;
    items: GetemployeeLoansForViewDto[] | undefined;
}

export interface IGetemployeeLoansForEditOutput {
    employeeLoans: CreateOrEditemployeeLoansDto | undefined;
}

