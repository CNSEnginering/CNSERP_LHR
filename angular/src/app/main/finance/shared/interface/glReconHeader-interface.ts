import * as moment from 'moment';
import { GLReconHeaderDto, CreateOrEditGLReconHeaderDto } from '../dto/glReconHeader-dto';
import { CreateOrEditGLReconDetailsDto } from '../dto/glReconDetails-dto';

export interface IPagedResultDtoOfGLReconHeaderDto {
    totalCount: number | undefined;
    items: GLReconHeaderDto[] | undefined;
}

export interface IGLReconHeaderDto {
    docID: string | undefined;
    docDate: moment.Moment | undefined;
    bankID:string | undefined;
    bankName:string | undefined;
    beginBalance: number | undefined;
    endBalance: number | undefined;
    reconcileAmt: number | undefined;
    diffAmount: number | undefined;
    statementAmt: number | undefined;
    clDepAmt: number | undefined;
    clPayAmt: number | undefined;
    unClDepAmt: number | undefined;
    unClPayAmt: number | undefined;
    clItems: number | undefined;
    unClItems: number | undefined;
    narration: string | undefined;
    completed: boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createdDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditGLReconHeaderDto {
    flag: boolean |undefined
    bankReconcileDetail: CreateOrEditGLReconDetailsDto [] |undefined;
    docID: string | undefined;
    docDate: moment.Moment | undefined;
    bankID:string | undefined;
    bankName:string | undefined;
    beginBalance: number | undefined;
    endBalance: number | undefined;
    reconcileAmt: number | undefined;
    diffAmount: number | undefined;
    statementAmt: number | undefined;
    clDepAmt: number | undefined;
    clPayAmt: number | undefined;
    unClDepAmt: number | undefined;
    unClPayAmt: number | undefined;
    clItems: number | undefined;
    unClItems: number | undefined;
    narration: string | undefined;
    completed: boolean;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createdDate: moment.Moment | undefined;
    id: number | undefined;
}


export interface IGetBankReconcileForEditOutput {
    gLReconHeader: CreateOrEditGLReconHeaderDto | undefined;
}