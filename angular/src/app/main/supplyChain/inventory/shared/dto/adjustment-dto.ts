import * as moment from 'moment';
import { CreateOrEditICADJHeaderDto } from './icadjHeader-dto';
import { CreateOrEditICADJDetailDto } from './icadjDetails-dto';
import { IAdjustmentDto } from '../interface/adjustment-interface';

export class AdjustmentDto implements IAdjustmentDto {
    icadjHeader!: CreateOrEditICADJHeaderDto | undefined;
    icadjDetail!: CreateOrEditICADJDetailDto[] | undefined;

    constructor(data?: IAdjustmentDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.icadjHeader = data["icadjHeader"] ? CreateOrEditICADJHeaderDto.fromJS(data["icadjHeader"]) : <any>undefined;
            if (data["icadjDetail"] && data["icadjDetail"].constructor === Array) {
                this.icadjDetail = [] as any;
                for (let item of data["icadjDetail"])
                    this.icadjDetail!.push(CreateOrEditICADJDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): AdjustmentDto {
        data = typeof data === 'object' ? data : {};
        let result = new AdjustmentDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icadjHeader"] = this.icadjHeader ? this.icadjHeader : <any>undefined;
        if (this.icadjDetail && this.icadjDetail.constructor === Array) {
            data["icadjDetail"] = [];
            for (let item of this.icadjDetail)
                data["icadjDetail"].push(item);
        }
        return data; 
    }
}