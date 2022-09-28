import * as moment from 'moment';
import { GetEmployeeTypeForViewDto, EmployeeTypeDto, CreateOrEditEmployeeTypeDto } from '../dto/employeeType-dto';


export interface IPagedResultDtoOfGetEmployeeTypeForViewDto {
    totalCount: number | undefined;
    items: GetEmployeeTypeForViewDto[] | undefined;
}

export interface IGetEmployeeTypeForViewDto {
    employeeType: EmployeeTypeDto | undefined;
}

export interface IEmployeeTypeDto {
    typeID: number | undefined;
    empType: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEmployeeTypeForEditOutput {
    employeeType: CreateOrEditEmployeeTypeDto | undefined;
}

export interface ICreateOrEditEmployeeTypeDto {
    typeID: number;
    empType: string | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}