
export class caderDTo{
    cadeR_NAME :string;
    id:number;
}

export class GetcaderDToOutput {
    caderDTo: caderDTo
}

export class PagedResultDtocader {
    totalCount: number;
    items: GetcaderDToOutput[]
}