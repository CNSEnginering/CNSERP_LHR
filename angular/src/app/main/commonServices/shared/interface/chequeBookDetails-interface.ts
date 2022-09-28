import * as moment from 'moment';
import { GetChequeBookDetailForViewDto, ChequeBookDetailDto, CreateOrEditChequeBookDetailDto } from '../dto/chequeBookDetails-dto';

export interface IPagedResultDtoOfGetChequeBookDetailForViewDto {
    totalCount: number | undefined;
    items: GetChequeBookDetailForViewDto[] | undefined;
}

export interface IGetChequeBookDetailForViewDto {
    chequeBookDetail: ChequeBookDetailDto | undefined;
}

export interface IChequeBookDetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    bankid: string | undefined;
    bankAccNo: string | undefined;
    fromChNo: string | undefined;
    toChNo: string | undefined;
    booKID: string | undefined;
    voucherNo: number | undefined;
    voucherDate: moment.Moment | undefined;
    narration: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetChequeBookDetailForEditOutput {
    chequeBookDetail: CreateOrEditChequeBookDetailDto | undefined;
}

export interface ICreateOrEditChequeBookDetailDto {
    detID: number | undefined;
    docNo: number | undefined;
    bankid: string;
    bankAccNo: string | undefined;
    fromChNo: string | undefined;
    toChNo: string | undefined;
    booKID: string | undefined;
    voucherNo: number | undefined;
    voucherDate: moment.Moment | undefined;
    narration: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}
