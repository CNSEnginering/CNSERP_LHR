import { CreateOrEditMFWCRESDto } from './mfwcres.dto';
import { CreateOrEditMFWCTOLDto } from './mfwctol.dto'
export class CreateOrEditMFWCMDto {
    id:number;
    wCID: string;
    wCESC: string;
    tOTRSCCOST?: number;
    tOTTLCOST?: number;
    cOMMENTS: string;
    audtUser: string;
    audtDate?: string;
    createdBy: string;
    createDate?: string;
    resDetailDto: CreateOrEditMFWCRESDto[] = new Array<CreateOrEditMFWCRESDto>();
    toolDetailDto: CreateOrEditMFWCTOLDto[] = new Array<CreateOrEditMFWCTOLDto>();
}