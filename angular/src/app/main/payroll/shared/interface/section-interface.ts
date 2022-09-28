import { GetSectionForViewDto, SectionDto, CreateOrEditSectionDto } from "../dto/section-dto";
import * as moment from 'moment';


export interface IPagedResultDtoOfGetSectionForViewDto {
    totalCount: number | undefined;
    items: GetSectionForViewDto[] | undefined;
}



export interface IGetSectionForViewDto {
    section: SectionDto | undefined;
}



export interface ISectionDto {
    secID: number | undefined;
    secName: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}



export interface IGetSectionForEditOutput {
    section: CreateOrEditSectionDto | undefined;
}



export interface ICreateOrEditSectionDto {
    secID: number;
    secName: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
    deptID:number | undefined;
    deptName: string | undefined;
}