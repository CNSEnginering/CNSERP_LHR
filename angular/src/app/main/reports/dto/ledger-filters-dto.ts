import * as moment from "moment";

export class LedgerFiltersDto {
    fromDate: moment.Moment;
    toDate: moment.Moment;
    fromAccount: string;
	fromAccountName: string;
	fromLoc:string;
	toLoc:string;
	fromLocName:string;
	toLocName:string;
    toAccount: string;
    toAccountName: string;
    fromSubAccount: string;
    fromSubAccountName: string;
    toSubAccount: string;
    toSubAccountName: string;
    includeZero: boolean;
    reportType: string;
    status: string;
    bookSummary: boolean;
    location: number;
    slType: number;
    includeLevel3: boolean;
    includeZeroBalance: boolean;
}
