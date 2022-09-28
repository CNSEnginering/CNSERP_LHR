import * as moment from 'moment';
import { IReceiptEntryDto } from '../interfaces/receiptEntry-interface';
import { CreateOrEditPORECHeaderDto } from './porecHeader-dto';
import { CreateOrEditPORECDetailDto } from './porecDetails-dto';
import { CreateOrEditICRECAExpDto } from './icrecaExp-dto';

export class ReceiptEntryDto implements IReceiptEntryDto {
    porecHeader!: CreateOrEditPORECHeaderDto | undefined;
    porecDetail!: CreateOrEditPORECDetailDto[] | undefined;
    icrecaExp!: CreateOrEditICRECAExpDto[] | undefined;

    constructor(data?: IReceiptEntryDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.porecHeader = data["porecHeader"] ? CreateOrEditPORECHeaderDto.fromJS(data["porecHeader"]) : <any>undefined;
            if (data["porecDetail"] && data["porecDetail"].constructor === Array) {
                this.porecDetail = [] as any;
                for (let item of data["porecDetail"])
                    this.porecDetail!.push(CreateOrEditPORECDetailDto.fromJS(item));
            }
            if (data["icrecaExp"] && data["icrecaExp"].constructor === Array) {
                this.porecDetail = [] as any;
                for (let item of data["icrecaExp"])
                    this.icrecaExp!.push(CreateOrEditICRECAExpDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ReceiptEntryDto {
        data = typeof data === 'object' ? data : {};
        let result = new ReceiptEntryDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["porecHeader"] = this.porecHeader ? this.porecHeader : <any>undefined;
        if (this.porecDetail && this.porecDetail.constructor === Array) {
            data["porecDetail"] = [];
            for (let item of this.porecDetail)
                data["porecDetail"].push(item);
        }
        if (this.icrecaExp && this.icrecaExp.constructor === Array) {
            data["icrecaExp"] = [];
            for (let item of this.icrecaExp)
                data["icrecaExp"].push(item);
        }
        return data; 
    }
}