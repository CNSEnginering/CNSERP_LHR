export class ICLOTDto  {
    id:number|undefined;
    tenantID?: number|undefined;
    lotNo: string;
    manfDate?: string;
    expiryDate?: string;
    active: boolean;
  
}
export class GeticLotForViewDto {
    icLotDto: ICLOTDto
}
export class GeticLotEditOutput {
    iclot: ICLOTDto
}

export class PagedResultDtoiclot {
    totalCount: number;
    items: GeticLotForViewDto[]
}
export class CreateOrEditLotNoDto {
    
    id:number|undefined;
    tenantID?: number|undefined;
    lotNo: string;
    manfDate?: string|Date|null;
    expiryDate?: string|Date|null;
    active: boolean;
    

}