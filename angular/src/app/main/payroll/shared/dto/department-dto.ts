import * as moment from 'moment';
import { IPagedResultDtoOfGetDepartmentForViewDto, IGetDepartmentForViewDto, IDepartmentDto, IGetDepartmentForEditOutput, ICreateOrEditDepartmentDto } from '../interface/department-interface';

export class PagedResultDtoOfGetDepartmentForViewDto implements IPagedResultDtoOfGetDepartmentForViewDto {
    totalCount!: number | undefined;
    items!: GetDepartmentForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetDepartmentForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(GetDepartmentForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetDepartmentForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetDepartmentForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }
}

export class GetDepartmentForViewDto implements IGetDepartmentForViewDto {
    department!: DepartmentDto | undefined;

    constructor(data?: IGetDepartmentForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.department = data["department"] ? DepartmentDto.fromJS(data["department"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetDepartmentForViewDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetDepartmentForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["department"] = this.department ? this.department.toJSON() : <any>undefined;
        return data;
    }
}

export class DepartmentDto implements IDepartmentDto {
    deptID!: number | undefined;
    deptName!: string | undefined;
    active!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;
    expenseAcc!: string | undefined;

    constructor(data?: IDepartmentDto) {
        debugger;
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.deptID = data["deptID"];
            this.deptName = data["deptName"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.expenseAcc = data["expenseAcc"];
        }
    }

    static fromJS(data: any): DepartmentDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new DepartmentDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["deptID"] = this.deptID;
        data["deptName"] = this.deptName;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["expenseAcc"]= this.expenseAcc ;
        return data;
    }
}

export class CreateOrEditDepartmentDto implements ICreateOrEditDepartmentDto {
    deptID!: number;
    deptName!: string | undefined;
    expenseAcc!: string | undefined;
    expenseAccName!: string | undefined;
    cader_Id!:number;
        caderName!:string | undefined;
    active!: number | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    id!: number | undefined;

    constructor(data?: ICreateOrEditDepartmentDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
             this.deptID = data["deptID"];
            this.deptName = data["deptName"];
            this.active = data["active"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.id = data["id"];
            this.expenseAcc = data["expenseAcc"];
            this.expenseAccName=data["expenseAccName"];
            this.cader_Id = data["cader_Id"];
            this.caderName = data["caderName"];
        }
    }

    static fromJS(data: any): CreateOrEditDepartmentDto {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditDepartmentDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["deptID"] = this.deptID;
        data["deptName"] = this.deptName;
        data["active"] = this.active;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["id"] = this.id;
        data["expenseAcc"]=this.expenseAcc;
        data["expenseAccName"]=this.expenseAccName;
        data["cader_Id"]=this.cader_Id;
        data["caderName"]=this.caderName;
        return data;
    }
}

export class GetDepartmentForEditOutput implements IGetDepartmentForEditOutput {
    department!: CreateOrEditDepartmentDto | undefined;

    constructor(data?: IGetDepartmentForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        debugger;
        if (data) {
            this.department = data["department"] ? CreateOrEditDepartmentDto.fromJS(data["department"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetDepartmentForEditOutput {
        debugger;
        data = typeof data === 'object' ? data : {};
        let result = new GetDepartmentForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["department"] = this.department ? this.department.toJSON() : <any>undefined;
        return data;
    }
}












