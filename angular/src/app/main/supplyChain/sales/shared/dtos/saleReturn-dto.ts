import * as moment from 'moment';
import { CreateOrEditOERETHeaderDto } from './oeretHeader-dto';
import { CreateOrEditOERETDetailDto } from './oeretDetails-dto';
import { ISaleReturnDto } from '../interfaces/saleReturn-interface';

export class SaleReturnDto implements ISaleReturnDto {
    oeretHeader!: CreateOrEditOERETHeaderDto | undefined;
    oeretDetail!: CreateOrEditOERETDetailDto[] | undefined;

    constructor(data?: ISaleReturnDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.oeretHeader = data["oeretHeader"] ? CreateOrEditOERETHeaderDto.fromJS(data["oeretHeader"]) : <any>undefined;
            if (data["oeretDetail"] && data["oeretDetail"].constructor === Array) {
                this.oeretDetail = [] as any;
                for (let item of data["oeretDetail"])
                    this.oeretDetail!.push(CreateOrEditOERETDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): SaleReturnDto {
        data = typeof data === 'object' ? data : {};
        let result = new SaleReturnDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["oeretHeader"] = this.oeretHeader ? this.oeretHeader : <any>undefined;
        if (this.oeretDetail && this.oeretDetail.constructor === Array) {
            data["oeretDetail"] = [];
            for (let item of this.oeretDetail)
                data["oeretDetail"].push(item);
        }
        return data; 
    }
}