export class ARINVDDto {
    id: number;
    detID: number;
    accountID: string;
    subAccID: number | null;
    docNo: number | null;
    invNumber: string;
    invAmount: number | null;
    taxAmount: string;
    recpAmount: number | null;
    chequeNo: string;
    adjust: boolean;
    narration: string;
}