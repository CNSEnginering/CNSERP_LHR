import { ICSetupDto } from "../dto/ic-setup-dto";
import * as moment from 'moment';

export interface IPagedResultDtoOfICSetupDto {
    totalCount: number | undefined;
    items: ICSetupDto[] | undefined;
}

export interface IICSetupDto {
    segment1: string | undefined;
    segment2: string | undefined;
    segment3: string | undefined;
    allowNegative: boolean;
    errSrNo: number | undefined;
    costingMethod: number | undefined;
    prBookID: string | undefined;
    rtBookID: string | undefined;
    cnsBookID: string | undefined;
    slBookID: string | undefined;
    srBookID: string | undefined;
    trBookID: string | undefined;
    prdBookID: string | undefined;
    pyRecBookID: string | undefined;
    adjBookID: string | undefined;
    asmBookID: string | undefined;
    wsBookID: string | undefined;
    dsBookID: string | undefined;
    salesReturnLinkOn: boolean;
    salesLinkOn: boolean; 
    accLinkOn: boolean;
    currentLocID:  number | undefined;
    damageLocID: number | undefined;
    
    allowLocID: boolean;
    cDateOnly: boolean;
    opt4: string | undefined;
    opt5: string | undefined;
    createdBy: string | undefined;
    createadOn: moment.Moment | undefined;
    id: number | undefined;
}