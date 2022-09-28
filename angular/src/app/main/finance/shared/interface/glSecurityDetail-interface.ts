import * as moment from 'moment';
import { GetGLSecurityDetailForViewDto, GLSecurityDetailDto, CreateOrEditGLSecurityDetailDto } from '../dto/glSecurityDetail-dto';


export interface IPagedResultDtoOfGetGLSecurityDetailForViewDto {
    totalCount: number | undefined;
    items: GetGLSecurityDetailForViewDto[] | undefined;
}
export interface IPagedResultDtoOfGLSecurityDetailsDto {
    totalCount: number | undefined;
    items: GLSecurityDetailDto[] | undefined;
}

export interface IGetGLSecurityDetailForViewDto {
    glSecurityDetail: GLSecurityDetailDto | undefined;
}

export interface IGLSecurityDetailDto {
    detID: number | undefined;
    userID: string | undefined;
    canSee: boolean | undefined;
    beginAcc: string | undefined;
    endAcc: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface IGetGLSecurityDetailForEditOutput {
    glSecurityDetail: CreateOrEditGLSecurityDetailDto | undefined;
}

export interface ICreateOrEditGLSecurityDetailDto {
    detID: number | undefined;
    userID: string | undefined;
    canSee: boolean | undefined;
    beginAcc: string | undefined;
    endAcc: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    id: number | undefined;
}