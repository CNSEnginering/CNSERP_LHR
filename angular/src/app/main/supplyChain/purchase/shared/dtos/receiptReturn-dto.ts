import * as moment from 'moment';
import { IReceiptReturnDto } from '../interfaces/receiptReturn-interface';
import { CreateOrEditPORETHeaderDto } from './poretHeader-dto';
import { CreateOrEditPORETDetailDto } from './poretDetails-dto';

export class ReceiptReturnDto implements IReceiptReturnDto {
    poretHeader!: CreateOrEditPORETHeaderDto | undefined;
    poretDetail!: CreateOrEditPORETDetailDto[] | undefined;

    constructor(data?: IReceiptReturnDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.poretHeader = data["poretHeader"] ? CreateOrEditPORETHeaderDto.fromJS(data["poretHeader"]) : <any>undefined;
            if (data["poretDetail"] && data["poretDetail"].constructor === Array) {
                this.poretDetail = [] as any;
                for (let item of data["poretDetail"])
                    this.poretDetail!.push(CreateOrEditPORETDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ReceiptReturnDto {
        data = typeof data === 'object' ? data : {};
        let result = new ReceiptReturnDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["poretHeader"] = this.poretHeader ? this.poretHeader : <any>undefined;
        if (this.poretDetail && this.poretDetail.constructor === Array) {
            data["poretDetail"] = [];
            for (let item of this.poretDetail)
                data["poretDetail"].push(item);
        }
        return data; 
    }
}