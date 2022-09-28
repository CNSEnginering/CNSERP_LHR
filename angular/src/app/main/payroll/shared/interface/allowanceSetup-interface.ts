import * as moment from 'moment';
import { GetAllowanceSetupForViewDto, AllowanceSetupDto, CreateOrEditAllowanceSetupDto } from '../dto/allowanceSetup-dto';

export interface IPagedResultDtoOfGetAllowanceSetupForViewDto {
    totalCount: number | undefined;
    items: GetAllowanceSetupForViewDto[] | undefined;
}

export interface IGetAllowanceSetupForViewDto {
    allowanceSetup: AllowanceSetupDto | undefined;
}

export interface IAllowanceSetupDto {
    docID: number | undefined;
    fuelRate: number | undefined;
    milageRate: number | undefined;
    repairRate: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetAllowanceSetupForEditOutput {
    allowanceSetup: CreateOrEditAllowanceSetupDto | undefined;
}

export interface ICreateOrEditAllowanceSetupDto {
    docID: number | undefined;
    fuelRate: number | undefined;
    milageRate: number | undefined;
    repairRate: number | undefined;
    active: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    id: number | undefined;
}