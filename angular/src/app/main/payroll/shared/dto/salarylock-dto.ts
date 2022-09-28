export class salarylockDTo{
    id:number;
    tenantID?: number;
    salaryMonth?: number;
    salaryYear?: number;
    locked: boolean;
    jvLocked: boolean;
    lockDate?: string;
}




export class GetsalarylockForViewDto {
    salarylock: salarylockDTo
}

export class GetsalarylockEditOutput {
    salaryLock: salarylockDTo
}

export class PagedResultDtosalarylock {
    totalCount: number;
    items: GetsalarylockForViewDto[]
}
export class CreateOrEditsalarylockDto {
    
  id:number;
    tenantID?: number;
    salaryMonth?: number;
    salaryYear?: number;
    locked: boolean;
    lockDate?: Date;
    jvLocked: boolean;

}