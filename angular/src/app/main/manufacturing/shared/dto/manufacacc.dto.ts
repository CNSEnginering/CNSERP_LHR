export class MFACSETDto {
    tenantID: number | null;
    acctset: string;
    desc: string;
    wipacct: string;
    wipaccDesc: string;
    setlabacct: string;
    labaccDesc: string;
    runlabacct: string;
    runLabAccDesc: string;
    ovhacct: string;
    ovhacctDesc: string;
    audtUser: string;
    audtDate: Date | string | null;
    createdBy: string;
    createDate: Date | string | null;
}

export class GetMFACSETForViewDto {
    mfacset: MFACSETDto
}

export class GetMFACSETForEditOutput {
    mfacset: MFACSETDto
}

export class PagedResultDtoMFACSET {
    totalCount: number;
    items: GetMFACSETForViewDto[]
}

export class CreateOrEditMFACSETDto {
    tenantId: number | null;
    acctset: string;
    desc: string;
    wipacct: string;
    setlabacct: string;
    runlabacct: string;
    ovhacct: string;
    audtUser: string;
    audtDate: Date | string | null;
    createDate: Date | string | null;
}