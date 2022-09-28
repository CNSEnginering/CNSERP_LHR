import { CreateOrEditOERETHeaderDto } from "../dtos/oeretHeader-dto";
import { CreateOrEditOERETDetailDto } from "../dtos/oeretDetails-dto";

export interface ISaleReturnDto {
    oeretHeader: CreateOrEditOERETHeaderDto | undefined;
    oeretDetail: CreateOrEditOERETDetailDto[] | undefined;
}