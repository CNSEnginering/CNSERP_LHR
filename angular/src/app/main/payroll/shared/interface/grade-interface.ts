import * as moment from 'moment';
import { GradeDto, GetGradeForViewDto, CreateOrEditGradeDto } from '../dto/grade-dto';
export interface IPagedResultDtoOfGetGradeForViewDto {
    totalCount: number | undefined;
    items: GetGradeForViewDto[] | undefined;
}

export interface IGetGradeForViewDto {
    grade: GradeDto | undefined;
}

export interface IGetGradeForEditOutput {
    grade: CreateOrEditGradeDto | undefined;
}

export interface ICreateOrEditGradeDto {
    gradeID: number | undefined;
    gradeName: string | undefined;
    type: number | undefined;
    active:  boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGradeDto {
    gradeID: number | undefined;
    gradeName: string | undefined;
    type: number | undefined;
    active:  boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}