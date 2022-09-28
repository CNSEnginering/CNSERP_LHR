import * as moment from 'moment';
import { ICUOMDto } from '../dto/ic-uoms-dto';

export interface IPagedResultDtoOfICUOMDto {
    totalCount: number | undefined;
    items: ICUOMDto[] | undefined;
}

export interface IICUOMDto {
    unit: string | undefined;
    unitdesc: string | undefined;
    conver: number | undefined;
    active: boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}