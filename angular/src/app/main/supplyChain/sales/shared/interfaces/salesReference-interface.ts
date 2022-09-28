import * as moment from 'moment';
import { GetSalesReferenceForViewDto, SalesReferenceDto, CreateOrEditSalesReferenceDto } from '../dtos/salesReference-dto';

export interface IPagedResultDtoOfGetSalesReferenceForViewDto {
    totalCount: number | undefined;
    items: GetSalesReferenceForViewDto[] | undefined;
}

export interface IGetSalesReferenceForViewDto {
    salesReference: SalesReferenceDto | undefined;
}

export interface ISalesReferenceDto {
    refID: number | undefined;
    refName: string | undefined;
    active: boolean | undefined;
    audtdate: moment.Moment | undefined;
    audtuser: string | undefined;
    createdDATE: moment.Moment | undefined;
    createdUSER: string | undefined;
    id: number | undefined;
    refType:string | undefined;
}

export interface IGetSalesReferenceForEditOutput {
    salesReference: CreateOrEditSalesReferenceDto | undefined;
}

export interface ICreateOrEditSalesReferenceDto {
    refID: number;
    refName: string | undefined;
    active: boolean | undefined;
    audtdate: moment.Moment | undefined;
    audtuser: string | undefined;
    createdDATE: moment.Moment | undefined;
    createdUSER: string | undefined;
    id: number | undefined;
    refType:string | undefined;
}