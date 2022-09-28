import * as moment from 'moment';
import { GLINVHeaderDto } from '../dto/glinvHeader-dto';

export interface IPagedResultDtoOfGLINVHeaderDto {
    totalCount: number | undefined;
    items: GLINVHeaderDto[] | undefined;
}

export interface IGLINVHeaderDto {
    docNo:number | undefined;
    typeID: string | undefined;
    bankID:string | undefined;
    accountID:string | undefined;
    configID:number | undefined;
    docDate: moment.Moment | undefined;
    postDate: moment.Moment | undefined;
    narration: string | undefined;
    curRate:number | undefined;
    creditLimit:number | undefined;
    closingBalance:number | undefined;
    chequeNo:string | undefined;
    refNo:string | undefined;
    payReason:string | undefined;
    partyInvNo:string | undefined;
    partyInvDate:moment.Moment | undefined;
    postedStock:boolean;
    postedStockBy:string | undefined;
    postedStockDate:moment.Moment | undefined;
    cprID:number | undefined;
    cprNo:string | undefined;
    cprDate:moment.Moment | undefined;
    posted:boolean;
    postedBy:string | undefined;
    postedDate:moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
    arClass:number | undefined;
    arRate:number | undefined;
    arAccID: string;
    arAmount:number | undefined;
    icTaxAuth: string;
    icTaxClass:number | undefined;
    icTaxRate:number | undefined;
    icTaxAccID: string;
    icTaxAmount:number | undefined;
}

export interface ICreateOrEditGLINVHeaderDto {
    docNo:number | undefined;
    typeID: string | undefined;
    bankID:string | undefined;
    accountID:string | undefined;
    configID:number | undefined;
    docDate: moment.Moment | undefined;
    postDate: moment.Moment | undefined;
    narration: string | undefined;
    curRate:number | undefined;
    creditLimit:number | undefined;
    closingBalance:number | undefined;
    chequeNo:string | undefined;
    refNo:string | undefined;
    payReason:string | undefined;
    partyInvNo:string | undefined;
    partyInvDate:moment.Moment | undefined;
    postedStock:boolean;
    postedStockBy:string | undefined;
    postedStockDate:moment.Moment | undefined;
    cprID:number | undefined;
    cprNo:string | undefined;
    cprDate:moment.Moment | undefined;
    posted:boolean;
    postedBy:string | undefined;
    postedDate:moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
    arClass:number | undefined;
    arRate:number | undefined;
    arAccID: string;
    arAmount:number | undefined;
    icTaxAuth: string;
    icTaxClass:number | undefined;
    icTaxRate:number | undefined;
    icTaxAccID: string;
    icTaxAmount:number | undefined;
}