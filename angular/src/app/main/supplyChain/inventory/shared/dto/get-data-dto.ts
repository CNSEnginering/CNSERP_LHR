import { IGetDataViewDto } from "../interface/get-data-interface";

export class GetDataViewDto implements IGetDataViewDto {
    displayName!: string | undefined;
    id!: number | undefined;
    descId!: string | undefined;

    constructor(data?: IGetDataViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.displayName = data["displayName"];
            this.descId = data["descId"];
            this.id = data["id"];
        }
    }

    static fromJS(data: any): GetDataViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetDataViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["displayName"]=this.displayName;
        data["descId"]=this.descId;
        data["id"]=this.id;
        return data; 
    }
}
