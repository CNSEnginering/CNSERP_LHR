export class MFAREADto {
    areaid: string;
    areadesc: string;
    areaty: number;
    status: number;
    address: string;
    contname: string;
    contpos: string;
    contcell: string;
    contemail: string;
    locid: number;
    locDesc:string;
    audtUser: string;
    audtDate: Date;
    createdBy: string;
    createDate: Date;
    active:boolean;
}

export class GetMFAREAForViewDto {
    mfarea: MFAREADto;
}

export class PagedResultDtoMFAREA {
    totalCount: number;
    items: GetMFAREAForViewDto[];
}

export class CreateOrEditMFAREADto {
    areaid: string;
    areadesc: string;
    areaty: number;
    status: number;
    address: string;
    contname: string;
    contpos: string;
    contcell: string;
    contemail: string;
    locid: number;
    locDesc:string;
    audtUser: string;
    audtDate: Date;
    createdBy: string;
    createDate: Date;
    active:boolean;
}

export class GetMFAREAForEditOutput {
    mfarea: CreateOrEditMFAREADto;
}