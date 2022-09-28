import * as moment from "moment";
import { ICLocationDto } from "../dto/ic-locations-dto";

export interface IPagedResultDtoOfICLocationDto {
    totalCount: number | undefined;
    items: ICLocationDto[] | undefined;
}

export interface IICLocationDto {
    locID: number | undefined;
    locName: string | undefined;
    locShort: string | undefined;
    address: string | undefined;
    city: string | undefined;
    allowRec: boolean;
    allowNeg: boolean;
    active: boolean;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    eLoc1: number | undefined;
    eLoc2: number | undefined;
    eLoc3: number | undefined;
    eLoc4: number | undefined;
    eLoc5: number | undefined;
    id: number | undefined;
}
