import * as moment from 'moment';
import { POPOHeaderDto } from '../dtos/popoHeader-dto';

export interface IPagedResultDtoOfPOPOHeaderDto {
    totalCount: number | undefined;
    items: POPOHeaderDto[] | undefined;
}

export interface IPOPOHeaderDto {
    locID:number | undefined;
    docNo:number | undefined;
    docDate: Date;
    arrivalDate: Date;
    reqNo: number | undefined;
    accountID: string | undefined;
    subAccID: number | undefined;
    totalQty:number | undefined;
    totalAmt:number | undefined;
    ordNo: string | undefined;
    ccid: string | undefined;
    narration: string | undefined;
    whTermID: number | undefined;
    whRate: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmount: number | undefined;
    onHold: boolean;
    active:boolean;
    audtUser: string | undefined;
    audtDate: Date;
    createdBy: string | undefined;
    createDate: Date;
    completed: boolean | undefined;
    id: number | undefined;
    terms:string | undefined;
}

export interface ICreateOrEditPOPOHeaderDto {
    locID:number | undefined;
    docNo:number | undefined;
    docDate: Date ;
    arrivalDate: Date;
    reqNo: number | undefined;
    accountID: string | undefined;
    subAccID: number | undefined;
    totalQty:number | undefined;
    totalAmt:number | undefined;
    ordNo: string | undefined;
    ccid: string | undefined;
    narration: string | undefined;
    whTermID: number | undefined;
    whRate: number | undefined;
    taxAuth: string | undefined;
    taxClass: number | undefined;
    taxRate: number | undefined;
    taxAmount: number | undefined;
    onHold: boolean;
    active:boolean;
    audtUser: string | undefined;
    audtDate: Date;
    createdBy: string | undefined;
    createDate: Date;
    completed:boolean | undefined;
    id: number | undefined;
    terms:string | undefined;
}