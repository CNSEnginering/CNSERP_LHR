import { CreateOrEditGLReconHeaderDto } from '../dto/glReconHeader-dto';
import { CreateOrEditGLReconDetailsDto } from '../dto/glReconDetails-dto';

export interface IBankReconcileDto {
    glReconHeader: CreateOrEditGLReconHeaderDto | undefined;
    glReconDetail: CreateOrEditGLReconDetailsDto[] | undefined;
}