import { CreateOrEditPORECHeaderDto } from "../dtos/porecHeader-dto";
import { CreateOrEditPORECDetailDto } from "../dtos/porecDetails-dto";
import { CreateOrEditICRECAExpDto } from "../dtos/icrecaExp-dto";


export interface IReceiptEntryDto {
    porecHeader: CreateOrEditPORECHeaderDto | undefined;
    porecDetail: CreateOrEditPORECDetailDto[] | undefined;
    icrecaExp: CreateOrEditICRECAExpDto[] | undefined;
}