import { ToolTypeFindersDto } from "../dtos/ToolTypefinders-dto";


export interface IPagedResultDtoOfToolTypeFindersDto {
    totalCount: number | undefined;
    items: IToolTypeFindersDto[] | undefined;
}

export interface IToolTypeFindersDto {
    id: string | undefined;
    displayName: string | undefined;
    typeId:string | undefined;

}