import { CommonServiceFindersDto } from "../dtos/commonServicesFinders-dto";


export interface IPagedResultDtoOfCommonServiceFindersDto {
    totalCount: number | undefined;
    items: CommonServiceFindersDto[] | undefined;
}

export interface ICommonServiceFindersDto {
    id: string | undefined;
    displayName: string | undefined;
    accountID: string | undefined;
    docType: number | undefined;
    availableLimit: number | undefined;
    currRate: number | undefined;
    taxRate: number | undefined;
    detId: number | undefined;
    bKAccountID :string |undefined;
}