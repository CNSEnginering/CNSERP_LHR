import { CreateOrEditICOPNHeaderDto } from "../dto/icopnHeader-dto";
import { CreateOrEditICOPNDetailDto } from "../dto/icopnDetails-dto";

export interface IOpeningDto {
    icopnHeader: CreateOrEditICOPNHeaderDto | undefined;
    icopnDetail: CreateOrEditICOPNDetailDto[] | undefined;
}