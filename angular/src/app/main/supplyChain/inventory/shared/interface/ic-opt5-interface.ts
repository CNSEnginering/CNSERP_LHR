import * as moment from 'moment';
import { GetICOPT5ForViewDto, ICOPT5Dto, CreateOrEditICOPT5Dto } from '../dto/ic-opt5-dto';
import 'rxjs/operators/map';


export interface IPagedResultDtoOfGetICOPT5ForViewDto {
    totalCount: number | undefined;
    items: GetICOPT5ForViewDto[] | undefined;
}

export interface IGetICOPT5ForViewDto {
    iCOPT5: ICOPT5Dto | undefined;
}

export interface IICOPT5Dto {
    optID: number | undefined;
    descp: string | undefined;
    id: number | undefined;
    active: boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
}

export interface IGetICOPT5ForEditOutput {
    ICOPT5: CreateOrEditICOPT5Dto | undefined;
}

export interface ICreateOrEditICOPT5Dto {
    flag: boolean | undefined;
    optID: number | undefined;
    descp: string | undefined;
    id: number | undefined;
    active: boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
}

export interface IFileDto {
    fileName: string;
    fileType: string | undefined;
    fileToken: string;
}