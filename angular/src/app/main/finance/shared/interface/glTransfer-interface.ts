import * as moment from 'moment';
import { GLTransferDto, GetGLTransferForViewDto, CreateOrEditGLTransferDto } from '../dto/glTransfer-dto';

export interface IPagedResultDtoOfGetGLTransferForViewDto {
    totalCount: number | undefined;
    items: GetGLTransferForViewDto[] | undefined;
}

export interface IGetGLTransferForViewDto {
    glTransfer: GLTransferDto | undefined;
}

export interface IGetGLTransferForEditOutput {
    glTransfer: CreateOrEditGLTransferDto | undefined;
}

export interface ICreateOrEditGLTransferDto {
    docid: number | undefined;
    docdate: moment.Moment | undefined;
    transferdate: moment.Moment | undefined;
    description: string | undefined;
    fromlocid: number | undefined;
    frombankid: string | undefined;
    fromconfigid: number | undefined;
    frombankaccid: string | undefined;
    fromaccid: string | undefined;
    tolocid: number | undefined;
    tobankid: string | undefined;
    toconfigid: number | undefined;
    tobankaccid: string | undefined;
    toaccid: string | undefined;
    status: boolean | undefined;
    transferamount: number | undefined;
    gllinkidfrom: number | undefined;
    gllinkidto: number | undefined;
    gldocidfrom: number | undefined;
    gldocidto: number | undefined;
    audtuser: string | undefined;
    audtdate: moment.Moment | undefined;
    createdBy: string | undefined;
    createdOn: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGLTransferDto {
    docid: number | undefined;
    docdate: moment.Moment | undefined;
    transferdate: moment.Moment | undefined;
    description: string | undefined;
    fromlocid: number | undefined;
    frombankid: string | undefined;
    fromconfigid: number | undefined;
    frombankaccid: string | undefined;
    fromaccid: string | undefined;
    tolocid: number | undefined;
    tobankid: string | undefined;
    toconfigid: number | undefined;
    tobankaccid: string | undefined;
    toaccid: string | undefined;
    status: boolean | undefined;
    transferamount: number | undefined;
    gllinkidfrom: number | undefined;
    gllinkidto: number | undefined;
    gldocidfrom: number | undefined;
    gldocidto: number | undefined;
    audtuser: string | undefined;
    audtdate: moment.Moment | undefined;
    createdBy: string | undefined;
    createdOn: moment.Moment | undefined;
    id: number | undefined;
}