export class oeroutesdto {
    id:number;
    routID:number;
    routDesc:string;
    active: boolean;
    createdBy: string;
    createDate: string | Date;
    audtUser: string;
    audtDate: string | Date;
}
export class GetoeroutesDtoOutput {
    oeRoutesDto: oeroutesdto
}

export class PagedResultDtocader {
    totalCount: number;
    items: GetoeroutesDtoOutput[]
}