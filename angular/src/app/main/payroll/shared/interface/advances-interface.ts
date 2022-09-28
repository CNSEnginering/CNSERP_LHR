import * as moment from "moment";
import {
    EmployeeAdvancesDto,
    CreateOrEditEmployeeAdvancesDto,
    GetEmployeeAdvancesForViewDto,
} from "../dto/advances-dto";

export interface IEmployeeAdvancesDto {
    advanceID: number | undefined;
    employeeID: number | undefined;
    employeeName: string | undefined;
    remarks: string | undefined;
    salaryYear: number | undefined;
    salaryMonth: number | undefined;
    advanceDate: moment.Moment | undefined;
    amount: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditEmployeeAdvancesDto {
    advanceID: number | undefined;
    employeeID: number | undefined;
    employeeName: string | undefined;
    remarks: string | undefined;
    salaryYear: number | undefined;
    salaryMonth: number | undefined;
    advanceDate: moment.Moment | undefined;
    amount: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetEmployeeAdvancesForViewDto {
    employeeAdvances: EmployeeAdvancesDto | undefined;
}

export interface IPagedResultDtoOfGetEmployeeAdvancesForViewDto {
    totalCount: number | undefined;
    items: GetEmployeeAdvancesForViewDto[] | undefined;
}

export interface IGetEmployeeAdvancesForEditOutput {
    employeeAdvances: CreateOrEditEmployeeAdvancesDto | undefined;
}
