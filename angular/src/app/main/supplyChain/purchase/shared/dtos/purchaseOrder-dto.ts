import * as moment from 'moment';
import { CreateOrEditPOPOHeaderDto } from './popoHeader-dto';
import { IPurchaseOrderDto } from '../interfaces/purchaseOrder-interface';
import { CreateOrEditPOPODetailDto } from './popoDetails-dto';

export class PurchaseOrderDto implements IPurchaseOrderDto {
    popoHeader!: CreateOrEditPOPOHeaderDto | undefined;
    popoDetail!: CreateOrEditPOPODetailDto[] | undefined;

    constructor(data?: IPurchaseOrderDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.popoHeader = data["popoHeader"] ? CreateOrEditPOPOHeaderDto.fromJS(data["popoHeader"]) : <any>undefined;
            if (data["popoDetail"] && data["popoDetail"].constructor === Array) {
                this.popoDetail = [] as any;
                for (let item of data["popoDetail"])
                    this.popoDetail!.push(CreateOrEditPOPODetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PurchaseOrderDto {
        data = typeof data === 'object' ? data : {};
        let result = new PurchaseOrderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["popoHeader"] = this.popoHeader ? this.popoHeader : <any>undefined;
        if (this.popoDetail && this.popoDetail.constructor === Array) {
            data["popoDetail"] = [];
            for (let item of this.popoDetail)
                data["popoDetail"].push(item);
        }
        return data; 
    }
}