import { SalesFindersDto } from "../dtos/salesFinders-dto";


export interface IPagedResultDtoOfSalesFindersDto {
    totalCount: number | undefined;
    items: SalesFindersDto[] | undefined;
}

export interface ISalesFindersDto {
    id: string | undefined;
    displayName: string | undefined;
    location: string | undefined;
}