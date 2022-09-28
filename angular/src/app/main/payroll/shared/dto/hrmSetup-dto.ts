export class hrmSetupDto{
    Id : number;
    AdvToStSal : string;
    AdvToPayable : string;
    LoanToStSal : string;
    LoanToPayable : string   ; 
}


export class gethrmSetupDtoView
{
    hrmSetupDto : hrmSetupDto
}

export class GethrmSetupDToOutput 
{
    hrmSetupDto: hrmSetupDto
}

export class PagedResultDtocader 
{
    totalCount: number;
    items: gethrmSetupDtoView[]
}
export class CreateOrEdithrmSetupDto 
{
    
  id:number;
  AdvToStSal : string;
  AdvToPayable : string;
  LoanToStSal : string;
  LoanToPayable : string ;
  
}