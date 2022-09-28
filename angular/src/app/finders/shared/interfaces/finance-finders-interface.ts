import { FinanceFindersDto } from "../dtos/financeFinders-dto";


export interface IPagedResultDtoOfFinanceFindersDto {
    totalCount: number | undefined;
    items: FinanceFindersDto[] | undefined;
}

export interface IFinanceFindersDto {
    id: string | undefined;
    displayName: string | undefined;
    accountID: string | undefined;
    subledger: boolean | undefined;
    termRate: number | undefined;
    slType:number | undefined

}