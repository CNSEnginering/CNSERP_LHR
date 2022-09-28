import * as moment from "moment";

export class ApprovalDto {
    fromDate: moment.Moment | undefined;
    toDate: moment.Moment | undefined;
    fromDoc: number | undefined;
    toDoc: number | undefined;
    options: string | undefined;
}
