export class MFtooltySETDto {
    tenantId: number | null;
    tooltyid:string;
    id:number;
    tooltydesc: string;
    status: boolean;
    unit: string;
    unitcost: number;
    comments: string;
    audtUser: string;
    audtDate: Date | string | null;
    createdBy: string;
    createDate: Date | string | null;
}

export class GetMFtooltySETForViewDto {
    mfTooltyset: MFtooltySETDto
}

export class GetMFtooltySETDtoForEditOutput {
    mftoolty: MFtooltySETDto
}

export class PagedResultDtoMFtooltySET {
    totalCount: number;
    items: GetMFtooltySETForViewDto[]
}

export class CreateOrEditMFtooltySETDto {
    tenantId: number | null;
    tooltyid:string;
    tooltydesc: string;
    status: boolean;
    unit: string;
    unitcost: number;
    comments: string;
    audtUser: string;
    audtDate: Date | string | null;
    createDate: Date | string | null;
}

