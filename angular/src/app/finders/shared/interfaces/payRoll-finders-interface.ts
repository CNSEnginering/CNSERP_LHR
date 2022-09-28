import { PayRollFindersDto } from "../dtos/payRollFinders-dto";


export interface IPagedResultDtoOfPayRollFindersDto {
    totalCount: number | undefined;
    items: PayRollFindersDto[] | undefined;
}

export interface IPayRollFindersDto {
    id: string | undefined;
    displayName: string | undefined;
    dutyHours: string | undefined;
    joiningDate: string | undefined;
}