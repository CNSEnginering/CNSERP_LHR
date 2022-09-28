import * as moment from 'moment';
import { CreateOrEditICADJHeaderDto } from '../dto/icadjHeader-dto';
import { CreateOrEditICADJDetailDto } from '../dto/icadjDetails-dto';

export interface IAdjustmentDto {
    icadjHeader: CreateOrEditICADJHeaderDto | undefined;
    icadjDetail: CreateOrEditICADJDetailDto[] | undefined;
}