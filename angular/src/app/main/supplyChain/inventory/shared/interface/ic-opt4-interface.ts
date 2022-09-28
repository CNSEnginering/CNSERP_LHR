import * as moment from 'moment';
import { ICOPT4Dto, GetICOPT4ForViewDto, CreateOrEditICOPT4Dto } from '../dto/ic-opt4-dto';
export interface IPagedResultDtoOfGetICOPT4ForViewDto {
    totalCount: number | undefined;
    items: GetICOPT4ForViewDto[] | undefined;
}

export interface IGetICOPT4ForViewDto {
    iCOPT4: ICOPT4Dto | undefined;
}

export interface IGetICOPT4ForEditOutput {
    iCOPT4: CreateOrEditICOPT4Dto | undefined;
}

export interface ICreateOrEditICOPT4Dto {
    flag: boolean |undefined;
    optID: number | undefined;
    descp: string | undefined;
    active: boolean;
    audtUser :string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IICOPT4Dto {
    optID: number | undefined;
    descp: string | undefined;
    active: boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}