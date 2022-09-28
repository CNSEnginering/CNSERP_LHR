import * as moment from 'moment';
import { CreateOrEditICCNSHeaderDto } from './iccnsHeader-dto';
import { CreateOrEditICCNSDetailDto } from './iccnsDetails-dto';
import { IConsumptionDto } from '../interface/consumption-interface';

export class ConsumptionDto implements IConsumptionDto {
    iccnsHeader!: CreateOrEditICCNSHeaderDto | undefined;
    iccnsDetail!: CreateOrEditICCNSDetailDto[] | undefined;

    constructor(data?: IConsumptionDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.iccnsHeader = data["iccnsHeader"] ? CreateOrEditICCNSHeaderDto.fromJS(data["iccnsHeader"]) : <any>undefined;
            if (data["iccnsDetail"] && data["iccnsDetail"].constructor === Array) {
                this.iccnsDetail = [] as any;
                for (let item of data["iccnsDetail"])
                    this.iccnsDetail!.push(CreateOrEditICCNSDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ConsumptionDto {
        data = typeof data === 'object' ? data : {};
        let result = new ConsumptionDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["iccnsHeader"] = this.iccnsHeader ? this.iccnsHeader : <any>undefined;
        if (this.iccnsDetail && this.iccnsDetail.constructor === Array) {
            data["iccnsDetail"] = [];
            for (let item of this.iccnsDetail)
                data["iccnsDetail"].push(item);
        }
        return data; 
    }
}