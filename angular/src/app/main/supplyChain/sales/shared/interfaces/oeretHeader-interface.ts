import * as moment from 'moment';
import { OERETHeaderDto } from '../dtos/oeretHeader-dto';

export interface IPagedResultDtoOfOERETHeaderDto {
    totalCount: number | undefined;
    items: OERETHeaderDto[] | undefined;
}

export interface IOERETHeaderDto {
    locID: number | undefined;
    locDesc: string | undefined; 
    docNo: number | undefined;
    docDate: moment.Moment | undefined;
    paymentDate: moment.Moment | undefined;
    typeID: string | undefined; 
    typeDesc: string | undefined; 
    custID: number | undefined;
    customerName: string | undefined; 
    priceList: string | undefined; 
    narration: string | undefined;
    ogp: string | undefined;
    totalQty: number | undefined;
    amount: number | undefined;
    tax: number | undefined;
    addTax: number | undefined;
    disc: number | undefined; 
    tradeDisc: number | undefined; 
    margin: number | undefined; 
    freight: number | undefined; 
    ordNo: number | undefined;
    totAmt: number | undefined;
    posted: boolean;
    linkDetID: number | undefined;
    active:boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditOERETHeaderDto {
    locID: number | undefined;
    docNo: number | undefined;
    docDate: moment.Moment | undefined;
    paymentDate: moment.Moment | undefined;
    typeID: string | undefined; 
    custID: number | undefined;
    priceList: string | undefined; 
    narration: string | undefined;
    ogp: string | undefined;
    totalQty: number | undefined;
    amount: number | undefined;
    tax: number | undefined;
    addTax: number | undefined;
    disc: number | undefined; 
    tradeDisc: number | undefined; 
    margin: number | undefined; 
    freight: number | undefined;  
    ordNo: number | undefined;
    totAmt: number | undefined;
    posted: boolean;
    linkDetID: number | undefined;
    active:boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}