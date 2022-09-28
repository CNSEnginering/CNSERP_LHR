
export class MonthlyCPRDto {
    id:number|undefined;
    salaryYear: number;
    salaryMonth: number;
    cprNumber: string;
    cprDate: Date | string | null;
    amount: number | null;
    remarks: string;
    active: boolean;
    audtUser: string;
    audtDate: Date | string | null;
    createdBy: string;
    createDate: Date | string | null;
    }

export class GetMonthlyCprForViewDto {
    monthlyCprDto: MonthlyCPRDto
}
export class GetMonthlyCprEditOutput {
    monthlyCPR: MonthlyCPRDto
}

export class PagedResultDtoMonthlyCpr {
    totalCount: number;
    items: GetMonthlyCprForViewDto[]
}
export class CreateOrEditMonthlyCprDto {
    id:number|undefined;
    salaryYear: number;
    salaryMonth: number;
    cprNumber: string;
    cprDate: Date | string | null;
    amount: number | null;
    remarks: string;
    active: boolean;
}