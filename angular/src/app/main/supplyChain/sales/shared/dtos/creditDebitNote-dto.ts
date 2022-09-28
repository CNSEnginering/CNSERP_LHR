import { CreditDebitNoteDetailDto } from "./creditDebitNoteDetail-dto";

export class CreditDebitNoteDto{
	id:number|undefined;	
    tenantId:number|undefined;	
	locID :number|undefined;
	locDesc:string|undefined;
	docNo :number|undefined;
    docDate:any|undefined;
	postingDate :any|undefined;
	dateTime :any;
	paymentDate :any|undefined;
	typeID :number|undefined;
	accountID :string|undefined;
	accountName :string|undefined;
	subAccID :number|undefined;
	subAccName :string|undefined;
	reason :string|undefined;
	narration :string|undefined;
	totalQty :number|undefined;
	totAmt :number|undefined;
	transType:string|undefined;
	trTypeID:string|undefined;
	trTypeDesc:string|undefined;
    creditDebitNoteDetailDto:CreditDebitNoteDetailDto[] = new Array<CreditDebitNoteDetailDto>()
}