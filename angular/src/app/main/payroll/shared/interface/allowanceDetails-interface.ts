import * as moment from 'moment';
import { GetAllowancesDetailForViewDto, AllowancesDetailDto, CreateOrEditAllowancesDetailDto } from '../dto/allowanceDetails-dto';

export interface IPagedResultDtoOfGetAllowancesDetailForViewDto {
    totalCount: number | undefined;
    items: GetAllowancesDetailForViewDto[] | undefined;
}

export interface IPagedResultDtoOfGetAllowancesDetail {
    totalCount: number | undefined;
    items: AllowancesDetailDto[] | undefined;
}

export interface IGetAllowancesDetailForViewDto {
    allowancesDetail: AllowancesDetailDto | undefined;
}

export interface IAllowancesDetailDto {
    detID: number | undefined;
    tenantID: number | undefined;
    employeeID: number | undefined;
    allowanceType: number | undefined;
    allowanceTypeName: string | undefined;
    allowanceAmt: number | undefined;
    allowanceQty: number | undefined;
    milage: number | undefined;
    amount: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetAllowancesDetailForEditOutput {
    allowancesDetail: CreateOrEditAllowancesDetailDto | undefined;
}

export interface ICreateOrEditAllowancesDetailDto {
    detID: number;
    employeeID: number | undefined;
    allowanceType: number | undefined;
    allowanceAmt: number | undefined;
    allowanceQty: number | undefined;
    milage: number | undefined;
    amount: number | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}