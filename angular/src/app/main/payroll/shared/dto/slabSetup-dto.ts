import { ICreateOrEditSlabSetupDto, IGetSlabSetupForEditOutput, IGetSlabSetupForViewDto, IPagedResultDtoOfGetSlabSetupForViewDto, ISlabSetupDto } from "../interface/taxSlabs-interface";

export class SlabSetupDto implements ISlabSetupDto {

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

export class GetSlabSetupForViewDto implements IGetSlabSetupForViewDto {
    slabSetup: SlabSetupDto;

}

export class CreateOrEditSlabSetupDto implements ICreateOrEditSlabSetupDto {
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

export class PagedResultDtoOfGetSlabSetupForViewDto implements IPagedResultDtoOfGetSlabSetupForViewDto {
    totalCount: number;
    items: GetSlabSetupForViewDto[];

}

export class GetSlabSetupForEditOutput implements IGetSlabSetupForEditOutput {
    slabSetup: CreateOrEditSlabSetupDto;

}