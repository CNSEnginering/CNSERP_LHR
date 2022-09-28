import * as moment from 'moment';
import { ReorderLevelDto } from '../dto/reorder-levels-dto';

export interface IPagedResultDtoOfReorderLevelDto {
    totalCount: number | undefined;
    items: ReorderLevelDto[] | undefined;
}

export interface IReorderLevelDto {
    locId: number | undefined;
    itemId: string | undefined;
    minLevel: number | undefined;
    maxLevel: number | undefined;
    ordLevel: number | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}