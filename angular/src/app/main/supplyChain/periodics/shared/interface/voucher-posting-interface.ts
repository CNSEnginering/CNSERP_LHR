import * as moment from 'moment';


export interface IVoucherPostingDto {
    fromDate: moment.Moment | undefined;
    toDate: moment.Moment | undefined;
    fromDoc: number | undefined;
    toDoc: number | undefined;
    receipt: boolean | undefined;
    sales: boolean | undefined;
    receiptReturn: boolean | undefined;
    salesReturn: boolean | undefined;
    transfer: boolean | undefined;
    consumption: boolean | undefined;
    bankTransfer: boolean | undefined;
    assemblies: boolean | undefined;
}


