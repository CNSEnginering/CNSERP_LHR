import * as moment from 'moment';
import { CreateOrEditGLINVHeaderDto } from '../dto/glinvHeader-dto';
import { CreateOrEditGLINVDetailDto } from '../dto/glinvDetails-dto';

export interface IDirectInvoiceDto {
    glinvHeader: CreateOrEditGLINVHeaderDto | undefined;
    glinvDetail: CreateOrEditGLINVDetailDto[] | undefined;
    target: string | undefined;
}