import { PurchaseFindersDto } from "../dtos/purchaseFinders-dto";

export interface IPagedResultDtoOfPurchaseFindersDto {
    totalCount: number | undefined;
    items: PurchaseFindersDto[] | undefined;
}

export interface IPurchaseFindersDto {
    id: string | undefined;
    displayName: string | undefined;
    location: string | undefined;
}