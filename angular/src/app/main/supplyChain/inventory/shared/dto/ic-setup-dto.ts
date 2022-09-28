import { IPagedResultDtoOfICSetupDto, IICSetupDto } from "../interface/ic-setup-interface";
import * as moment from 'moment';

export class PagedResultDtoOfICSetupDto implements IPagedResultDtoOfICSetupDto {
    totalCount!: number | undefined;
    items!: ICSetupDto[] | undefined;

    constructor(data?: IPagedResultDtoOfICSetupDto) {
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
                    this.items!.push(ICSetupDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfICSetupDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfICSetupDto();
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

export class ICSetupDto implements IICSetupDto {
    segment1!: string | undefined;
    segment2!: string | undefined;
    segment3!: string | undefined;
    allowNegative!: boolean;
    errSrNo!: number | undefined;
    costingMethod!: number | undefined;
    inventoryPoint!: number | undefined;
    prBookID!: string | undefined;
    rtBookID!: string | undefined;
    cnsBookID!: string | undefined;
    slBookID!: string | undefined;
    srBookID!: string | undefined;
    trBookID!: string | undefined;
    prdBookID!: string | undefined;
    pyRecBookID!: string | undefined;
    adjBookID!: string | undefined;
    asmBookID!: string | undefined;
    wsBookID!: string | undefined;
    dsBookID!: string | undefined;
    salesReturnLinkOn!: boolean;
    salesLinkOn!: boolean;
    accLinkOn!: boolean;
    currentLocID!:  number | undefined;
    damageLocID!:  number | undefined;
    transType!:  number | undefined;
    glSegLink!:  number | undefined;
    allowLocID!: boolean;
    cDateOnly!: boolean;
    opt4!: string | undefined;
    opt5!: string | undefined;
    currency!:string|undefined;
    createdBy!: string | undefined;
    createadOn!: moment.Moment | undefined;
    id!: number | undefined;
    currentLocName:string | undefined;
    conType:number|undefined;

    constructor(data?: IICSetupDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.segment1 = data["segment1"];
            this.segment2 = data["segment2"];
            this.segment3 = data["segment3"];
            this.allowNegative = data["allowNegative"];
            this.errSrNo = data["errSrNo"];
            this.costingMethod = data["costingMethod"];
            this.prBookID = data["prBookID"];
            this.rtBookID = data["rtBookID"];
            this.cnsBookID = data["cnsBookID"];
            this.slBookID = data["slBookID"];
            this.srBookID = data["srBookID"];
            this.trBookID = data["trBookID"];
            this.currency=data["currency"];
            this.prdBookID = data["prdBookID"];
            this.pyRecBookID = data["pyRecBookID"];
            this.adjBookID = data["adjBookID"];
            this.asmBookID = data["asmBookID"];
            this.wsBookID = data["wsBookID"];
            this.dsBookID = data["dsBookID"];
            this.salesReturnLinkOn = data["salesReturnLinkOn"];
            this.salesLinkOn = data["salesLinkOn"];
            this.accLinkOn = data["accLinkOn"];
            this.currentLocID = data["currentLocID"];
            this.damageLocID=data["damageLocID"];
            this.glSegLink = data["glSegLink"];
            this.allowLocID = data["allowLocID"];
            this.cDateOnly = data["cDateOnly"];
            this.opt4 = data["opt4"];
            this.opt5 = data["opt5"];
            this.createdBy = data["createdBy"];
            this.transType=data["transType"];
            this.createadOn = data["createadOn"] ? moment(data["createadOn"].toString()) : <any>undefined;
            this.id = data["id"];
            this.currentLocName = data["currentLocName"];
            this.inventoryPoint=data["inventoryPoint"];
            this.conType = data["conType"];
            
            
        }
    }

    static fromJS(data: any): ICSetupDto {
        data = typeof data === 'object' ? data : {};
        let result = new ICSetupDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["segment1"] = this.segment1;
        data["segment2"] = this.segment2;
        data["segment3"] = this.segment3;
        data["allowNegative"] = this.allowNegative;
        data["errSrNo"] = this.errSrNo;
        data["costingMethod"] = this.costingMethod;
        data["prBookID"] = this.prBookID;
        data["rtBookID"] = this.rtBookID;
        data["cnsBookID"] = this.cnsBookID;
        data["slBookID"] = this.slBookID;
        data["srBookID"] = this.srBookID;
        data["trBookID"] = this.trBookID;
        data["prdBookID"] = this.prdBookID;
        data["currency"]=this.currency;
        data["pyRecBookID"] = this.pyRecBookID;
        data["adjBookID"] = this.adjBookID;
        data["asmBookID"] = this.asmBookID;
        data["wsBookID"] = this.wsBookID;
        data["dsBookID"] = this.dsBookID;
        data["salesReturnLinkOn"] = this.salesReturnLinkOn;
        data["salesLinkOn"] = this.salesLinkOn;
        data["accLinkOn"] = this.accLinkOn;
        data["currentLocID"] = this.currentLocID;
        data["damageLocID"]=this.damageLocID;
        data["allowLocID"] = this.allowLocID;
        data["transType"]=this.transType;
        data["glSegLink"] = this.glSegLink;
        data["cDateOnly"] = this.cDateOnly;
        data["opt4"] = this.opt4;
        data["opt5"] = this.opt5;
        data["createdBy"] = this.createdBy;
        data["createadOn"] = this.createadOn ? this.createadOn.toISOString() : <any>undefined;
        data["id"] = this.id;
        data["inventoryPoint"]=this.inventoryPoint;
        data["currentLocName"]=this.currentLocName;
        data["conType"] = this.conType;
        
        
        return data; 
    }
}