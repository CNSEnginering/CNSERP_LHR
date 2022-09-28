import * as moment from 'moment';
import { IDirectInvoiceDto } from '../interface/directinvoice-interface';
import { CreateOrEditGLINVHeaderDto } from './glinvHeader-dto';
import { CreateOrEditGLINVDetailDto } from './glinvDetails-dto';

export class DirectInvoiceDto implements IDirectInvoiceDto {
    glinvHeader!: CreateOrEditGLINVHeaderDto | undefined;
    glinvDetail!: CreateOrEditGLINVDetailDto[] | undefined;
    target!: string | undefined;

    constructor(data?: IDirectInvoiceDto) {
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
            this.glinvHeader = data["glinvHeader"] ? CreateOrEditGLINVHeaderDto.fromJS(data["glinvHeader"]) : <any>undefined;
            if (data["glinvDetail"] && data["glinvDetail"].constructor === Array) {
                this.glinvDetail = [] as any;
                for (let item of data["glinvDetail"])
                    this.glinvDetail!.push(CreateOrEditGLINVDetailDto.fromJS(item));
            }
            this.target=data["target"];
        }
    }

    static fromJS(data: any): DirectInvoiceDto {
        data = typeof data === 'object' ? data : {};
        let result = new DirectInvoiceDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["glinvHeader"] = this.glinvHeader ? this.glinvHeader : <any>undefined;
        if (this.glinvDetail && this.glinvDetail.constructor === Array) {
            data["glinvDetail"] = [];
            for (let item of this.glinvDetail)
                data["glinvDetail"].push(item);
        }
        data["target"]=this.target;
        return data; 
    }
}