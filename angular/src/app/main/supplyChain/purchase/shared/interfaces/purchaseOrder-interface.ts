import { CreateOrEditPOPOHeaderDto } from "../dtos/popoHeader-dto";
import { CreateOrEditPOPODetailDto } from "../dtos/popoDetails-dto";

export interface IPurchaseOrderDto {
    popoHeader: CreateOrEditPOPOHeaderDto | undefined;
    popoDetail: CreateOrEditPOPODetailDto[] | undefined;
}