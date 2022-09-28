import * as moment from 'moment';
import { CreateOrEditICOPNHeaderDto } from './icopnHeader-dto';
import { CreateOrEditICOPNDetailDto } from './icopnDetails-dto';
import { IOpeningDto } from '../interface/opening-interface';

export class OpeningDto implements IOpeningDto {
    icopnHeader!: CreateOrEditICOPNHeaderDto | undefined;
    icopnDetail!: CreateOrEditICOPNDetailDto[] | undefined;

    constructor(data?: IOpeningDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.icopnHeader = data["icopnHeader"] ? CreateOrEditICOPNHeaderDto.fromJS(data["icopnHeader"]) : <any>undefined;
            if (data["icopnDetail"] && data["icopnDetail"].constructor === Array) {
                this.icopnDetail = [] as any;
                for (let item of data["icopnDetail"])
                    this.icopnDetail!.push(CreateOrEditICOPNDetailDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): OpeningDto {
        data = typeof data === 'object' ? data : {};
        let result = new OpeningDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["icopnHeader"] = this.icopnHeader ? this.icopnHeader : <any>undefined;
        if (this.icopnDetail && this.icopnDetail.constructor === Array) {
            data["icopnDetail"] = [];
            for (let item of this.icopnDetail)
                data["icopnDetail"].push(item);
        }
        return data; 
    }
}