import * as moment from 'moment';
import { GetDepartmentForViewDto, DepartmentDto, CreateOrEditDepartmentDto } from '../dto/department-dto';

export interface IDepartmentDto {
    deptID: number | undefined;
    deptName: string | undefined;
    active: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditDepartmentDto {
    deptID: number;
    deptName: string | undefined;
    active: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetDepartmentForViewDto {
    department: DepartmentDto | undefined;
}

export interface IPagedResultDtoOfGetDepartmentForViewDto {
    totalCount: number | undefined;
    items: GetDepartmentForViewDto[] | undefined;
}

export interface IGetDepartmentForEditOutput {
    department: CreateOrEditDepartmentDto | undefined;
}

