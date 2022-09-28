import * as moment from 'moment';
import { TransactionTypeDto } from '../dto/transaction-types-dto';

export interface IPagedResultDtoOfTransactionTypeDto {
    totalCount: number | undefined;
    items: TransactionTypeDto[] | undefined;
}

export interface ITransactionTypeDto {
    typeId: string | undefined;
    description: string | undefined;
    active: boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}