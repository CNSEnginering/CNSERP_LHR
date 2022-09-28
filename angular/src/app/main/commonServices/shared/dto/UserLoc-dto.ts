export class CreateOrEditCSUserLocHDto {
    id:number;
    typeID?: number;
    createdBy: string;
    createDate?: string;
    userId: string;
    userName: string;
    audtUser: string;
    audtDate?: string;
    userLocD: CreateOrEditCSUserLocDDto[];
}



export class CreateOrEditCSUserLocDDto {
    typeID?: number;
    userID: string;
    detId?: number;
    locId?: number;
    status: boolean;
}