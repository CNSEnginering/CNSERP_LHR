import * as moment from 'moment';
import { IWorkOrderDto } from '../interface/workOrder-interface';
import { CreateOrEditICWOHeaderDto } from './icwoHeader-dto';
import { CreateOrEditICWODetailDto } from './icwoDetails-dto';

export class WorkOrderDto implements IWorkOrderDto {
    icwoHeader!: CreateOrEditICWOHeaderDto | undefined;
    icwoDetail!: CreateOrEditICWODetailDto[] | undefined;

    constructor(data?: IWorkOrderDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.icwoHeader = data["icwoHeader"] ? CreateOrEditICWOHeaderDto.fromJS(data["icwoHeader"]) : <any>undefined;
            if (data["icwoDetail"] && data["icwoDetail"].constructor === Array) {
                this.icwoDetail = [] as any;
                for (let item of data["icwoDetail"])
                    this.icwoDetail!.push(CreateOrEditICWODetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): WorkOrderDto {
        data = typeof data === 'object' ? data : {};
        let result = new WorkOrderDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icwoHeader"] = this.icwoHeader ? this.icwoHeader : <any>undefined;
        if (this.icwoDetail && this.icwoDetail.constructor === Array) {
            data["icwoDetail"] = [];
            for (let item of this.icwoDetail)
                data["icwoDetail"].push(item);
        }
        return data; 
    }
}