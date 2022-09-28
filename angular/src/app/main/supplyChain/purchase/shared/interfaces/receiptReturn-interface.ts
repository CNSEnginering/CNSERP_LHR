import { CreateOrEditPORETHeaderDto } from "../dtos/poretHeader-dto";
import { CreateOrEditPORETDetailDto } from "../dtos/poretDetails-dto";


export interface IReceiptReturnDto {
    poretHeader: CreateOrEditPORETHeaderDto | undefined;
    poretDetail: CreateOrEditPORETDetailDto[] | undefined;
}