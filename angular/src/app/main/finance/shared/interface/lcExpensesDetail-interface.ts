import { GetLCExpensesDetailForViewDto, LCExpensesDetailDto, CreateOrEditLCExpensesDetailDto } from '../dto/lcExpensesDetail-dto';


export interface IPagedResultDtoOfGetLCExpensesDetailForViewDto {
    totalCount: number | undefined;
    items: GetLCExpensesDetailForViewDto[] | undefined;
}
export interface IPagedResultDtoOfLCExpensesDetailsDto {
    totalCount: number | undefined;
    items: LCExpensesDetailDto[] | undefined;
}

export interface IGetLCExpensesDetailForViewDto {
    lcExpensesDetail: LCExpensesDetailDto | undefined;
}
export interface ILCExpensesDetailDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    expDesc: string | undefined;
    amount: number | undefined;
    id: number | undefined;
}

export interface IGetLCExpensesDetailForEditOutput {
    lcExpensesDetail: CreateOrEditLCExpensesDetailDto | undefined;
}

export interface ICreateOrEditLCExpensesDetailDto {
    detID: number | undefined;
    locID: number | undefined;
    docNo: number | undefined;
    expDesc: string | undefined;
    amount: number | undefined;
    id: number | undefined;
}

export interface IListResultDtoOfLCExpenses {
    items: CreateOrEditLCExpensesDetailDto[] | undefined;
}