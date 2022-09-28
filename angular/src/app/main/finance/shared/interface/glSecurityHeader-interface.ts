import * as moment from 'moment';
import { GLSecurityHeaderDto, CreateOrEditGLSecurityHeaderDto, GetGLSecurityHeaderForViewDto } from '../dto/glSecurityHeader-dto';
import { CreateOrEditGLSecurityDetailDto } from '../dto/glSecurityDetail-dto';

export interface IPagedResultDtoOfGLSecurityHeaderDto {
    totalCount: number | undefined;
    items: GLSecurityHeaderDto[] | undefined;
}

export interface IGLSecurityHeaderDto {
    userID: string | undefined;
    userName: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createdDate: moment.Moment | undefined;
    id: number | undefined;
}

export interface ICreateOrEditGLSecurityHeaderDto {
    flag: boolean |undefined
    glSecurityDetail: CreateOrEditGLSecurityDetailDto [] | undefined;
    userID: string | undefined;
    userName: string | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createdDate: moment.Moment | undefined;
    id: number | undefined;
}


export interface IGetGLSecurityHeaderForEditOutput {
    glSecurityHeader: CreateOrEditGLSecurityHeaderDto | undefined;
}


export interface IGetGLSecurityHeaderForViewDto {
    glSecurityHeader: GLSecurityHeaderDto | undefined;
}

export interface IPagedResultDtoOfGetGLSecurityHeaderForViewDto {
    totalCount: number | undefined;
    items: GetGLSecurityHeaderForViewDto[] | undefined;
}