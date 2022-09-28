import * as moment from 'moment';
import { IVoucherPostingDto } from "../interface/voucher-posting-interface";


export class VoucherPostingDto implements IVoucherPostingDto {
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
    creditNote: boolean | undefined;
    debitNote: boolean | undefined;

    constructor(data?: IVoucherPostingDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            debugger;
            this.fromDate = data["fromDate"];
            this.toDate = data["toDate"];
            this.fromDoc = data["fromDoc"];
            this.toDoc = data["toDoc"];
            this.receipt = data["receipt"];
            this.sales = data["sales"];
            this.receiptReturn = data["receiptReturn"];
            this.salesReturn = data["salesReturn"];
            this.transfer = data["transfer"];
            this.consumption = data["consumption"];
            this.bankTransfer = data["bankTransfer"];
            this.assemblies = data["assemblies"];
            this.creditNote = data["creditNote"];
            this.debitNote = data["debitNote"];
        }
    }

    static fromJS(data: any): VoucherPostingDto {
        data = typeof data === 'object' ? data : {};
        let result = new VoucherPostingDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["fromDate"] = this.fromDate;
        data["toDate"] = this.toDate;
        data["fromDoc"] = this.fromDoc;
        data["toDoc"] = this.toDoc;
        data["receipt"] = this.receipt;
        data["sales"] = this.sales;
        data["receiptReturn"] = this.receiptReturn;
        data["transfer"] = this.transfer;
        data["consumption"] = this.consumption;
        data["bankTransfer"] = this.bankTransfer;
        data["assemblies"] = this.assemblies;
        data["creditNote"] = this.creditNote;
        data["debitNote"] = this.debitNote;
        return data;
    }
}
