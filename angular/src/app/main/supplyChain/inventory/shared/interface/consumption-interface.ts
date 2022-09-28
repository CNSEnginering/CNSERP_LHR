import * as moment from 'moment';
import { CreateOrEditICCNSHeaderDto } from '../dto/iccnsHeader-dto';
import { CreateOrEditICCNSDetailDto } from '../dto/iccnsDetails-dto';

export interface IConsumptionDto {
    iccnsHeader: CreateOrEditICCNSHeaderDto | undefined;
    iccnsDetail: CreateOrEditICCNSDetailDto[] | undefined;
}