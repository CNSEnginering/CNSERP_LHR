export class invoiceKnockOffDetailDto {
    invNo: number;
    invDate: string | null;
    amount: number | null;
    alreadyPaid: number | null;
    pending: number | null;
    adjust: number | null;
    isCheck: boolean;
}