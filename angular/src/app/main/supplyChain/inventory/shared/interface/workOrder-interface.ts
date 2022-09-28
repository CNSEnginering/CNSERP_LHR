import { CreateOrEditICWOHeaderDto } from "../dto/icwoHeader-dto";
import { CreateOrEditICWODetailDto } from "../dto/icwoDetails-dto";

export interface IWorkOrderDto {
    icwoHeader: CreateOrEditICWOHeaderDto | undefined;
    icwoDetail: CreateOrEditICWODetailDto[] | undefined;
}