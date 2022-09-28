import { InventoryFindersDto } from "../dtos/inventoryFinders-dto";


export interface IPagedResultDtoOfInventoryFindersDto {
    totalCount: number | undefined;
    items: InventoryFindersDto[] | undefined;
}

export interface IInventoryFindersDto {
    id: string | undefined;
    displayName: string | undefined;
    unit: string | undefined;
    conver: number | undefined;
    option5: string | undefined;

    
}