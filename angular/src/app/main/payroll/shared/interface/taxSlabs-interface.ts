import * as moment from 'moment';
import { CreateOrEditSlabSetupDto, GetSlabSetupForViewDto, SlabSetupDto } from '../dto/slabSetup-dto';


export interface ISlabSetupDto {
    id: number;
    typeID: number;
    slabFrom: number;
    slabTo: number;
    rate: number;
    amount: number;
    active: boolean;
    audtUser: string;
    audtDate: Date;
    createdBy: String;
    createDate: Date;
}

export interface ICreateOrEditSlabSetupDto {
    id: number;
    typeID: number;
    slabFrom: number;
    slabTo: number;
    rate: number;
    amount: number;
    active: boolean;
    audtUser: string;
    audtDate: Date;
    createdBy: String;
    createDate: Date;
}

export interface IGetSlabSetupForViewDto {
    slabSetup: SlabSetupDto | undefined;
}

export interface IPagedResultDtoOfGetSlabSetupForViewDto {
    totalCount: number | undefined;
    items: GetSlabSetupForViewDto[] | undefined;
}

export interface IGetSlabSetupForEditOutput {
    slabSetup: CreateOrEditSlabSetupDto | undefined;
}

