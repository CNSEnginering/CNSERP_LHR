export class MFtoolSETDto {
    tenantId: number | null;
    toolid:string;
    tooltyid:string;
    tooltypedesc:string;
    id:number;
    tooldesc: string;
    status: boolean;
    audtUser: string;
    audtDate: Date | string | null;
    createdBy: string;
    createDate: Date | string | null;
}

export class GetMFtoolSETForViewDto {
    mfToolset: MFtoolSETDto
}

export class GetMFtoolSETDtoForEditOutput {
    mftool: MFtoolSETDto
}

export class PagedResultDtoMFtoolSET {
    totalCount: number;
    items: GetMFtoolSETForViewDto[]
}

export class CreateOrEditMFtoolSETDto {
    tenantId: number | null;
    toolid:string;
    id:number;
    tooldesc: string;
    status: boolean;
    audtUser: string;
    audtDate: Date | string | null;
    createDate: Date | string | null;
}

