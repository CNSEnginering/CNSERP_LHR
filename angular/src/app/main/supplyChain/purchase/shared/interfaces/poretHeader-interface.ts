import * as moment from 'moment';
import { PORETHeaderDto } from '../dtos/poretHeader-dto';

export interface IPagedResultDtoOfPORETHeaderDto {
    totalCount: number | undefined;
    items: PORETHeaderDto[] | undefined;
}

export interface IPORETHeaderDto {
    locID: number | undefined;
    docNo: number | undefined;
    docDate: moment.Moment | undefined;
    accountID: string | undefined; 
    accDesc: string | undefined; 
    subAccID: number | undefined;
    subAccDesc: string | undefined; 
    narration: string | undefined;
    igpNo: string | undefined;
    billNo: string | undefined;
    billDate: moment.Moment | undefined;
    billAmt: number | undefined;
    totalQty: number | undefined;
    totalAmt:number | undefined;
    posted: boolean;
    linkDetID: number | undefined;
    recDocNo: number | undefined;
    ordNo: string | undefined;
    freight: number | undefined;
    addExp: number | undefined;
    ccid: string | undefined;
    ccDesc: string | undefined; 
    addDisc: number | undefined;
    addLeak: number | undefined;
    addFreight: number | undefined;
    onHold: boolean;
    active:boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditPORETHeaderDto {
    locID:number | undefined;
    docNo:number | undefined;
    docDate: moment.Moment | undefined;
    accountID: string | undefined; 
    subAccID: number | undefined;
    narration: string | undefined;
    igpNo: string | undefined;
    billNo: string | undefined;
    billDate: moment.Moment | undefined;
    billAmt: number | undefined;
    totalQty: number | undefined;
    totalAmt:number | undefined;
    posted: boolean;
    linkDetID: number | undefined;
    recDocNo: number | undefined;
    ordNo: string | undefined;
    freight: number | undefined;
    addExp: number | undefined;
    ccid: string | undefined;
    addDisc: number | undefined;
    addLeak: number | undefined;
    addFreight: number | undefined;
    onHold: boolean;
    active:boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}