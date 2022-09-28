export class MFRESMASDto {
    tenantId: number | null;
    resid: string;
    resdesc: string;
    active: boolean;
    costtype: number;
    unitcost: number;
    uomtype: number;
    unit: string;
    costbasis:number;
    audtUser: string;
    audtDate: Date ;
    createdBy: string;
    createDate: Date ;
}

export class GetMFRESMASForViewDto {
    mfresmas: MFRESMASDto
}

export class GetMFRESMASForEditOutput {
    mfresmas: MFRESMASDto
}

export class PagedResultDtoMFRESMAS {
    totalCount: number;
    items: GetMFRESMASForViewDto[]
}

export class CreateOrEditMFRESMASDto {
    tenantId: number | null;
    resid: string;
    resdesc: string;
    active: boolean;
    costtype: number;
    unitcost: number;
    uomtype: number;
    unit: string;
    costbasis:number;
    audtUser: string;
    audtDate: Date ;
    createdBy: string;
    createDate: Date ;
}