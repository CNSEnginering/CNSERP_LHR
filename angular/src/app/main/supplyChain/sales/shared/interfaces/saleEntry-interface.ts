import { CreateOrEditOESALEHeaderDto } from "../dtos/oesaleHeader-dto";
import { CreateOrEditOESALEDetailDto } from "../dtos/oesaleDetails-dto";

export interface ISaleEntryDto {
    oesaleHeader: CreateOrEditOESALEHeaderDto | undefined;
    oesaleDetail: CreateOrEditOESALEDetailDto[] | undefined;
}