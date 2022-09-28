import * as moment from 'moment';
import { OESALEHeaderDto } from '../dtos/oesaleHeader-dto';

export interface IPagedResultDtoOfOESALEHeaderDto {
    totalCount: number | undefined;
    items: OESALEHeaderDto[] | undefined;
}

export interface IOESALEHeaderDto {
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
    routDesc: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
    routID: number | undefined;
    vehicleType: number | undefined;
    driverName: string | undefined;
    vehicleNo: string | undefined;
    
}

export interface ICreateOrEditOESALEHeaderDto {
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
    routID: number | undefined;
    vehicleType: number | undefined;
    driverName: string | undefined;
    vehicleNo: string | undefined;
}