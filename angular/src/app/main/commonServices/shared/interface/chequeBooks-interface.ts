import * as moment from 'moment';
import { GetChequeBookForViewDto, ChequeBookDto, CreateOrEditChequeBookDto } from '../dto/chequeBooks-dto';


export interface IPagedResultDtoOfGetChequeBookForViewDto {
    totalCount: number | undefined;
    items: GetChequeBookForViewDto[] | undefined;
}

export interface IGetChequeBookForViewDto {
    chequeBook: ChequeBookDto | undefined;
}

export interface IChequeBookDto {
    docNo: number | undefined;
    docDate: moment.Moment | undefined;
    bankid: string | undefined;
    bankAccNo: string | undefined;
    fromChNo: string | undefined;
    toChNo: string | undefined;
    noofCh: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetChequeBookForEditOutput {
    chequeBook: CreateOrEditChequeBookDto | undefined;
}

export interface ICreateOrEditChequeBookDto {
    docNo: number | undefined;
    docDate: moment.Moment | undefined;
    bankid: string;
    bankAccNo: string | undefined;
    fromChNo: string | undefined;
    toChNo: string | undefined;
    noofCh: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}
