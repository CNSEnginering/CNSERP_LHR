import * as moment from 'moment';
import { GetRecurringVoucherForViewDto, RecurringVoucherDto, CreateOrEditRecurringVoucherDto } from '../dto/recurringVouchers-dto';


export interface IPagedResultDtoOfGetRecurringVoucherForViewDto {
    totalCount: number | undefined;
    items: GetRecurringVoucherForViewDto[] | undefined;
}

export interface IGetRecurringVoucherForViewDto {
    recurringVoucher: RecurringVoucherDto | undefined;
}

export interface IRecurringVoucherDto {
    docNo: number | undefined;
    bookID: string | undefined;
    voucherNo: number | undefined;
    fmtVoucherNo: string | undefined;
    voucherDate: moment.Moment | undefined;
    voucherMonth: number | undefined;
    configID: number | undefined;
    reference: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetRecurringVoucherForEditOutput {
    recurringVoucher: CreateOrEditRecurringVoucherDto | undefined;
}

export interface ICreateOrEditRecurringVoucherDto {
    docNo: number | undefined;
    bookID: string | undefined;
    voucherNo: number | undefined;
    fmtVoucherNo: string | undefined;
    voucherDate: moment.Moment | undefined;
    voucherMonth: number | undefined;
    configID: number | undefined;
    reference: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}
