import * as moment from 'moment';
import { ISaleEntryDto } from '../interfaces/saleEntry-interface';
import { CreateOrEditOESALEDetailDto } from './oesaleDetails-dto';
import { CreateOrEditOESALEHeaderDto } from './oesaleHeader-dto';

export class SaleEntryDto implements ISaleEntryDto {
    oesaleHeader!: CreateOrEditOESALEHeaderDto | undefined;
    oesaleDetail!: CreateOrEditOESALEDetailDto[] | undefined;

    constructor(data?: ISaleEntryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.oesaleHeader = data["oesaleHeader"] ? CreateOrEditOESALEHeaderDto.fromJS(data["oesaleHeader"]) : <any>undefined;
            if (data["oesaleDetail"] && data["oesaleDetail"].constructor === Array) {
                this.oesaleDetail = [] as any;
                for (let item of data["oesaleDetail"])
                    this.oesaleDetail!.push(CreateOrEditOESALEDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): SaleEntryDto {
        data = typeof data === 'object' ? data : {};
        let result = new SaleEntryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["oesaleHeader"] = this.oesaleHeader ? this.oesaleHeader : <any>undefined;
        if (this.oesaleDetail && this.oesaleDetail.constructor === Array) {
            data["oesaleDetail"] = [];
            for (let item of this.oesaleDetail)
                data["oesaleDetail"].push(item);
        }
        return data; 
    }
}