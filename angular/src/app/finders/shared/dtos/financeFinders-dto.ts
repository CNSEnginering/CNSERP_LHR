import * as moment from 'moment';
import { IPagedResultDtoOfFinanceFindersDto, IFinanceFindersDto } from '../interfaces/finance-finders-interface';

export class PagedResultDtoOfFinanceFindersDto implements IPagedResultDtoOfFinanceFindersDto {
    totalCount!: number | undefined;
    items!: FinanceFindersDto[] | undefined;

    constructor(data?: IPagedResultDtoOfFinanceFindersDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(FinanceFindersDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfFinanceFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfFinanceFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class FinanceFindersDto implements IFinanceFindersDto {
    id!: string | undefined;
    displayName!: string | undefined;
    accountID!: string | undefined;
    subledger!: boolean | undefined;
    termRate!: number | undefined;
    amount!: number | undefined;
    slType!: number | undefined;
    docDate !: moment.Moment | undefined;
    docMonth!: number | undefined;
    configID !: number | undefined;
    bookId !: string | undefined;
    locName !: string | undefined;
    fmtDocNo !: number | undefined;
    docNo !: number | undefined;
    itemPriceID!:string|undefined;
 
    driverCtrlAcc!:string|undefined;
    driverSubAcc!:string|undefined;
    constructor(data?: IFinanceFindersDto) {
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
            this.slType = data["slType"];
            this.displayName = data["displayName"];
            this.accountID = data["accountID"];
            this.subledger = data["subledger"];
            this.termRate = data["termRate"];
            this.amount = data["amount"];
            this.docDate = data["docDate"];
            this.docMonth = data["docMonth"];
            this.configID = data["configID"];
            this.bookId = data["bookId"];
            this.fmtDocNo = data["fmtDocNo"];
            this.id = data["id"];
            this.docNo = data["docNo"];
            this.itemPriceID=data["itemPriceID"];
            this.driverSubAcc=data["driverSubAcc"];
            
            this.driverCtrlAcc=data["driverCtrlAcc"];
        }
    }

    static fromJS(data: any): FinanceFindersDto {
        data = typeof data === 'object' ? data : {};
        let result = new FinanceFindersDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["slType"] = this.slType;
        data["displayName"] = this.displayName;
        data["accountID"] = this.accountID;
        data["subledger"] = this.subledger;
        data["termRate"] = this.termRate;
        data["amount"] = this.amount;
        data["docDate"] = this.docDate ? moment(this.docDate).toISOString(true) : <any>undefined;;
        data["docMonth"] = this.docMonth;
        data["configID"] = this.configID;
        data["bookId"] = this.bookId ;
        data["fmtDocNo"] = this.fmtDocNo ;
        data["id"] = this.id;
        data["docNo"] = this.docNo;
        data["itemPriceID"]=this.itemPriceID;
        data["driverCtrlAcc"]=this.driverCtrlAcc;
        data["driverSubAcc"]=this.driverSubAcc;
        
    
        return data;
    }
}