import * as moment from 'moment';
import {  CreateOrEditLoansTypeDto, loansTypeDto, GetLoansTypeForViewDto } from '../dto/loanTypes-dto';

export interface ILoansTypeDto {
    loanTypeID: number | undefined;
    loanTypeName:string|undefined;
    id: number | undefined;
}

export interface ICreateOrEditLoansTypeDto {
    LoanTypeID: number | undefined;
    LoanTypeName:string|undefined;
    id: number | undefined;
}

export interface IGetLoansTypeForViewDto {
    loansType: loansTypeDto | undefined;
}

export interface IPagedResultDtoOfGetLoansTypeForViewDto {
    totalCount: number | undefined;
    items: GetLoansTypeForViewDto[] | undefined;
}

export interface IGetLoansTypeForEditOutput {
    loansType: CreateOrEditLoansTypeDto | undefined;
}

