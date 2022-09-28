import * as moment from 'moment';
import { GLReconDetailsDto, CreateOrEditGLReconDetailsDto } from '../dto/glReconDetails-dto';

export interface IPagedResultDtoOfGLReconDetailsDto {
    totalCount: number | undefined;
    items: GLReconDetailsDto[] | undefined;
}

export interface IGLReconDetailsDto {
    detID: number | undefined;
    bookID: string | undefined;
    configID: number | undefined;
    voucherID: number | undefined;
    voucherDate: moment.Moment | undefined;
    clearingDate: moment.Moment | undefined;
    amount: number | undefined;
    include: boolean |undefined;
    id: number | undefined;
    glDetID: number | undefined;
}



export interface ICreateOrEditGLReconDetailsDto {
    detID: number | undefined;
    bookID: string | undefined;
    configID: string | undefined;
    voucherID: number | undefined;
    voucherDate: moment.Moment | undefined;
    clearingDate: moment.Moment | undefined;
    Dr: number | undefined;
    Cr: number | undefined;
    include: boolean |undefined;
    id: number | undefined;
    glDetID: number | undefined;
}

export interface IlistResultDtoOfBankReconcileDetail {
    items: CreateOrEditGLReconDetailsDto[] | undefined;
}