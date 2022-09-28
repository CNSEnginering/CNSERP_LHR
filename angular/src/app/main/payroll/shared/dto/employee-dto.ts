import * as moment from 'moment';
import { IPagedResultDtoOfGetEmployeesForViewDto, IGetEmployeesForViewDto, IEmployeesDto, IGetEmployeesForEditOutput, ICreateOrEditEmployeesDto } from '../interface/employee-interface';


export class PagedResultDtoOfGetEmployeesForViewDto implements IPagedResultDtoOfGetEmployeesForViewDto {
    totalCount!: number | undefined;
    items!: GetEmployeesForViewDto[] | undefined;

    constructor(data?: IPagedResultDtoOfGetEmployeesForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [] as any;
                for (let item of data["items"])
                    this.items!.push(GetEmployeesForViewDto.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGetEmployeesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new PagedResultDtoOfGetEmployeesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
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

export class GetEmployeesForViewDto implements IGetEmployeesForViewDto {
    employees!: EmployeesDto | undefined;

    constructor(data?: IGetEmployeesForViewDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employees = data["employees"] ? EmployeesDto.fromJS(data["employees"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeesForViewDto {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeesForViewDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employees"] = this.employees ? this.employees.toJSON() : <any>undefined;
        return data;
    }
}

export class EmployeesDto implements IEmployeesDto {
    employeeID!: number | undefined;
    employeeName!: string | undefined;
    fatherName!: string | undefined;
    date_of_birth!: moment.Moment | undefined;
    home_address!: string | undefined;
    phoneNo!: string | undefined;
    ntn!: string | undefined;
    apointment_date!: moment.Moment | undefined;
    date_of_joining!: moment.Moment | undefined;
    date_of_leaving!: moment.Moment | undefined;
    city!: string | undefined;
    cnic!: string | undefined;
    edID!: number | undefined;
    deptID!: number | undefined;
    designationID!: number | undefined;
    subDesignationID!: number | undefined;
    gender!: string | undefined;
    department!: string | undefined;
    designation!: string | undefined;
    status!: boolean | undefined;
    shiftID!: number | undefined;
    typeID!: number | undefined;
    secID!: number | undefined;
    religionID!: number | undefined;
    social_security!: boolean | undefined;
    eobi!: boolean | undefined;
    wppf!: boolean | undefined;
    payment_mode!: string | undefined;
    bank_name!: string | undefined;
    account_no!: string | undefined;
    academic_qualification!: string | undefined;
    professional_qualification!: string | undefined;
    first_rest_days!: number | undefined;
    second_rest_days!: number | undefined;
    first_rest_days_w2!: number | undefined;
    second_rest_days_w2!: number | undefined;
    bloodGroup!: string | undefined;
    reference!: string | undefined;
    visa_Details!: string | undefined;
    driving_Licence!: string | undefined;
    duty_Hours!: number | undefined;
    active!: boolean | undefined;
    confirmed!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    waveOff!: boolean | undefined;
    type_of_notice_period!: number | undefined;
    reinstate!: boolean | undefined;
    reinstateDate!: moment.Moment | undefined;
    reinstateReason!: string | undefined;
    allowanceType!: number;
    allowanceAmt!: number;
    allowanceQty!: number;
    id!: number | undefined;
    contractExpDate!: moment.Moment | undefined;
    employeeOldId!: number | undefined;
    maritalStatus!: string | undefined;
    eobiNo!: string | undefined;
    sscNo!: string | undefined;
    eobiAmt!: number | undefined;
    sscAmt!: number | undefined;
    eBankID:number|undefined;
    lunchStatus:boolean|undefined;
    constructor(data?: IEmployeesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            debugger;
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.fatherName = data["fatherName"];
            this.date_of_birth = data["date_of_birth"];
            this.home_address = data["home_address"];
            this.phoneNo = data["phoneNo"];
            this.ntn = data["ntn"];
            this.apointment_date = data["apointment_date"];
            this.date_of_joining = data["date_of_joining"];
            this.date_of_leaving = data["date_of_leaving"];
            this.city = data["city"];
            this.cnic = data["cnic"];
            this.edID = data["edID"];
            this.designation = data["designation"];
            this.department = data["department"];
            this.deptID = data["deptID"];
            this.designationID = data["designationID"];
            this.subDesignationID = data["subDesignationID"];
            this.gender = data["gender"];
            this.status = data["status"];
            this.shiftID = data["shiftID"];
            this.typeID = data["typeID"];
            this.secID = data["secID"];
            this.religionID = data["religionID"];
            this.social_security = data["social_security"];
            this.eobi = data["eobi"];
            this.wppf = data["wppf"];
            this.payment_mode = data["payment_mode"];
            this.bank_name = data["bank_name"];
            this.account_no = data["account_no"];
            this.academic_qualification = data["academic_qualification"];
            this.professional_qualification = data["professional_qualification"];
            this.first_rest_days = data["first_rest_days"];
            this.second_rest_days = data["second_rest_days"];
            this.first_rest_days_w2 = data["first_rest_days_w2"];
            this.second_rest_days_w2 = data["second_rest_days_w2"];
            this.bloodGroup = data["bloodGroup"];
            this.reference = data["reference"];
            this.visa_Details = data["visa_Details"];
            this.driving_Licence = data["driving_Licence"];
            this.duty_Hours = data["duty_Hours"];
            this.active = data["active"];
            this.confirmed = data["confirmed"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"]
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.waveOff = data["waveOff"];
            this.type_of_notice_period = data["type_of_notice_period"];
            this.reinstate = data["reinstate"];
            this.reinstateDate = data["reinstateDate"];
            this.reinstateReason = data["reinstateReason"];
            this.allowanceType = data["allowanceType"];
            this.allowanceAmt = data["allowanceAmt"];
            this.allowanceQty = data["allowanceQty"];
            this.id = data["id"];
            this.contractExpDate = data["contractExpDate"];

            this.employeeOldId = data["oldEmployeeID"];
            this.maritalStatus = data["mStatus"];
            this.eobiNo = data["eoBiNo"];
            this.sscNo = data["sscNo"];
            this.eobiAmt = data["eobiAmt"];
            this.sscAmt = data["sScAmt"];
            this.eBankID=data["eBankID"];
            this.lunchStatus=data["lunchStatus"];
        }
    }

    static fromJS(data: any): EmployeesDto {
        data = typeof data === 'object' ? data : {};
        let result = new EmployeesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["fatherName"] = this.fatherName;
        data["date_of_birth"] = this.date_of_birth;
        data["home_address"] = this.home_address;
        data["phoneNo"] = this.phoneNo;
        data["ntn"] = this.ntn;
        data["apointment_date"] = this.apointment_date;
        data["date_of_joining"] = this.date_of_joining;
        data["date_of_leaving"] = this.date_of_leaving;
        data["city"] = this.city;
        data["cnic"] = this.cnic;
        data["edID"] = this.edID;
        data["deptID"] = this.deptID;
        data["designationID"] = this.designationID;
        data["subDesignationID"] = this.subDesignationID;
        data["gender"] = this.gender;
        data["status"] = this.status;
        data["shiftID"] = this.shiftID;
        data["typeID"] = this.typeID;
        data["secID"] = this.secID;
        data["designation"]=this.designation;
        data["department"]=this.department ;
        data["religionID"] = this.religionID;
        data["social_security"] = this.social_security;
        data["eobi"] = this.eobi;
        data["wppf"] = this.wppf;
        data["payment_mode"] = this.payment_mode;
        data["bank_name"] = this.bank_name;
        data["account_no"] = this.account_no;
        data["academic_qualification"] = this.academic_qualification;
        data["professional_qualification"] = this.professional_qualification;
        data["first_rest_days"] = this.first_rest_days;
        data["second_rest_days"] = this.second_rest_days;
        data["first_rest_days_w2"] = this.first_rest_days_w2;
        data["second_rest_days_w2"] = this.second_rest_days_w2;
        data["bloodGroup"] = this.bloodGroup;
        data["reference"] = this.reference;
        data["visa_Details"] = this.visa_Details;
        data["driving_Licence"] = this.driving_Licence;
        data["duty_Hours"] = this.duty_Hours;
        data["active"] = this.active;
        data["confirmed"] = this.confirmed;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["waveOff"] = this.waveOff;
        data["type_of_notice_period"] = this.type_of_notice_period;
        data["reinstate"] = this.reinstate;
        data["reinstateDate"] = this.reinstateDate;
        data["reinstateReason"] = this.reinstateReason;
        data["allowanceType"] = this.allowanceType;
        data["allowanceAmt"] = this.allowanceAmt;
        data["allowanceQty"] = this.allowanceQty;
        data["id"] = this.id;
        data["contractExpDate"] = this.contractExpDate;
        data["oldEmployeeID"] = this.employeeOldId
        data["mStatus"] = this.maritalStatus;
        data["eoBiNo"] = this.eobiNo;
        data["sscNo"] = this.sscNo;
        data["eobiAmt"] = this.eobiAmt;
        data["sScAmt"] = this.sscAmt;
        data["eBankID"]=this.eBankID;
         data["lunchStatus"]=this.lunchStatus;
        return data;
    }
}

export class GetEmployeesForEditOutput implements IGetEmployeesForEditOutput {
    employees!: CreateOrEditEmployeesDto | undefined;

    constructor(data?: IGetEmployeesForEditOutput) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.employees = data["employees"] ? CreateOrEditEmployeesDto.fromJS(data["employees"]) : <any>undefined;
        }
    }

    static fromJS(data: any): GetEmployeesForEditOutput {
        data = typeof data === 'object' ? data : {};
        let result = new GetEmployeesForEditOutput();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["employees"] = this.employees ? this.employees.toJSON() : <any>undefined;
        return data;
    }
}

export class CreateOrEditEmployeesDto implements ICreateOrEditEmployeesDto {
    employeeID!: number;
    employeeName!: string;
    fatherName!: string | undefined;
    date_of_birth!: moment.Moment | undefined;
    home_address!: string | undefined;
    phoneNo!: string | undefined;
    ntn!: string | undefined;
    apointment_date!: moment.Moment | undefined;
    date_of_joining!: moment.Moment | undefined;
    date_of_leaving!: moment.Moment | undefined;
    city!: string | undefined;
    cnic!: string | undefined;
    edID!: number | undefined;
    locID!: number | undefined;
    department!: string | undefined;
    designation!: string | undefined;
    deptID!: number | undefined;
    designationID!: number | undefined;
    subDesignationID!: number | undefined;
    gender!: string | undefined;
    status!: boolean | undefined;
    shiftID!: number | undefined;
    typeID!: number | undefined;
    secID!: number | undefined;
    religionID!: number | undefined;
    social_security!: boolean | undefined;
    eobi!: boolean | undefined;
    eobiAmt!: number | undefined;
    sscAmt!: number | undefined;
    wppf!: boolean | undefined;
    payment_mode!: string | undefined;
    bank_name!: string | undefined;
    account_no!: string | undefined;
    academic_qualification!: string | undefined;
    professional_qualification!: string | undefined;
    first_rest_days!: number | undefined;
    second_rest_days!: number | undefined;
    first_rest_days_w2!: number | undefined;
    second_rest_days_w2!: number | undefined;
    bloodGroup!: string | undefined;
    reference!: string | undefined;
    visa_Details!: string | undefined;
    driving_Licence!: string | undefined;
    duty_Hours!: number | undefined;
    active!: boolean | undefined;
    confirmed!: boolean | undefined;
    audtUser!: string | undefined;
    audtDate!: moment.Moment | undefined;
    createdBy!: string | undefined;
    createDate!: moment.Moment | undefined;
    waveOff!: boolean | undefined;
    type_of_notice_period!: number | undefined;
    reinstate!: boolean | undefined;
    reinstateDate!: moment.Moment | undefined;
    reinstateReason!: string | undefined;
    allowanceType!: number;
    allowanceAmt!: number;
    allowanceQty!: number;
    id!: number | undefined;
    contractExpDate!: moment.Moment | undefined;
    employeeOldId!: number | undefined;
    maritalStatus!: string | undefined;
    eobiNo!: string | undefined;
    sscNo!: string | undefined;
    joningReport: boolean | undefined;
    signEmplForm: boolean | undefined;
    academicRec: boolean | undefined;
    pasPhoto: boolean | undefined;
    harrasmentComp: boolean | undefined;
    validCnic: boolean | undefined;
    cVResume: boolean | undefined;
    salarySlip: boolean | undefined;
    serviceCertificate: boolean | undefined;
    refCninc: boolean | undefined;
    resPrevEmp: boolean | undefined;
    disclosureForm: boolean | undefined;
    eBankID:number|undefined;
    lunchStatus:boolean|undefined;
    constructor(data?: ICreateOrEditEmployeesDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            debugger;
            this.employeeID = data["employeeID"];
            this.employeeName = data["employeeName"];
            this.fatherName = data["fatherName"];
            this.date_of_birth = data["date_of_birth"];
            this.home_address = data["home_address"];
            this.phoneNo = data["phoneNo"];
            this.ntn = data["ntn"];
            this.designation = data["designation"];
            this.department = data["department"];
            this.apointment_date = data["apointment_date"];
            this.date_of_joining = data["date_of_joining"];
            this.date_of_leaving = data["date_of_leaving"];
            this.city = data["city"];
            this.cnic = data["cnic"];
            this.edID = data["edID"];
            this.locID = data["locID"];
            this.deptID = data["deptID"];
            this.designationID = data["designationID"];
            this.subDesignationID = data["subDesignationID"];
            this.gender = data["gender"];
            this.status = data["status"];
            this.shiftID = data["shiftID"];
            this.typeID = data["typeID"];
            this.secID = data["secID"];
            this.religionID = data["religionID"];
            this.social_security = data["social_security"];
            this.eobi = data["eobi"];
            this.wppf = data["wppf"];
            this.sscAmt = data["sScAmt"];
            this.eobiAmt = data["eobiAmt"];
            this.payment_mode = data["payment_mode"];
            this.bank_name = data["bank_name"];
            this.account_no = data["account_no"];
            this.academic_qualification = data["academic_qualification"];
            this.professional_qualification = data["professional_qualification"];
            this.first_rest_days = data["first_rest_days"];
            this.second_rest_days = data["second_rest_days"];
            this.first_rest_days_w2 = data["first_rest_days_w2"];
            this.second_rest_days_w2 = data["second_rest_days_w2"];
            this.bloodGroup = data["bloodGroup"];
            this.reference = data["reference"];
            this.visa_Details = data["visa_Details"];
            this.driving_Licence = data["driving_Licence"];
            this.duty_Hours = data["duty_Hours"];
            this.active = data["active"];
            this.confirmed = data["confirmed"];
            this.audtUser = data["audtUser"];
            this.audtDate = data["audtDate"];
            this.createdBy = data["createdBy"];
            this.createDate = data["createDate"];
            this.waveOff = data["waveOff"];
            this.type_of_notice_period = data["type_of_notice_period"];
            this.reinstate = data["reinstate"];
            this.reinstateDate = data["reinstateDate"];
            this.reinstateReason = data["reinstateReason"];
            this.allowanceType = data["allowanceType"];
            this.allowanceAmt = data["allowanceAmt"];
            this.allowanceQty = data["allowanceQty"];
            this.id = data["id"];
            this.contractExpDate = data["contractExpDate"];
            this.employeeOldId = data["oldEmployeeID"];
            this.maritalStatus = data["mStatus"];
            this.eobiNo = data["eoBiNo"];
            this.sscNo = data["sscNo"];
            this.joningReport = data["joningReport"];
            this.signEmplForm=data["signEmplForm"];
            this.pasPhoto=data["pasPhoto"];
            this.refCninc=data["refCninc"];
            this.resPrevEmp=data["resPrevEmp"];
            this.salarySlip=data["salarySlip"];
            this.serviceCertificate=data["serviceCertificate"];
            this.validCnic=data["validCnic"];
            this.academicRec=data["academicRec"];
            this.cVResume=data["cVResume"];
            this.disclosureForm=data["disclosureForm"];
           this.eBankID=data["eBankID"];
           this.lunchStatus=data["lunchStatus"];
        }
    }

    static fromJS(data: any): CreateOrEditEmployeesDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateOrEditEmployeesDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        debugger;
        data = typeof data === 'object' ? data : {};
        data["employeeID"] = this.employeeID;
        data["employeeName"] = this.employeeName;
        data["fatherName"] = this.fatherName;
        data["date_of_birth"] = this.date_of_birth;
        data["home_address"] = this.home_address;
        data["phoneNo"] = this.phoneNo;
        data["ntn"] = this.ntn;
        data["apointment_date"] = this.apointment_date;
        data["date_of_joining"] = this.date_of_joining;
        data["date_of_leaving"] = this.date_of_leaving;
        data["designation"]=this.designation;
        data["department"]=this.department ;
        data["city"] = this.city;
        data["cnic"] = this.cnic;
        data["edID"] = this.edID;
        data["locID"] = this.locID;
        data["deptID"] = this.deptID;
        data["designationID"] = this.designationID;
        data["subDesignationID"] = this.subDesignationID;
        data["gender"] = this.gender;
        data["status"] = this.status;
        data["shiftID"] = this.shiftID;
        data["typeID"] = this.typeID;
        data["secID"] = this.secID;
        data["religionID"] = this.religionID;
        data["social_security"] = this.social_security;
        data["eobi"] = this.eobi;
        data["wppf"] = this.wppf;
        data["payment_mode"] = this.payment_mode;
        data["bank_name"] = this.bank_name;
        data["account_no"] = this.account_no;
        data["academic_qualification"] = this.academic_qualification;
        data["professional_qualification"] = this.professional_qualification;
        data["first_rest_days"] = this.first_rest_days;
        data["second_rest_days"] = this.second_rest_days;
        data["first_rest_days_w2"] = this.first_rest_days_w2;
        data["second_rest_days_w2"] = this.second_rest_days_w2;
        data["bloodGroup"] = this.bloodGroup;
        data["reference"] = this.reference;
        data["visa_Details"] = this.visa_Details;
        data["driving_Licence"] = this.driving_Licence;
        data["duty_Hours"] = this.duty_Hours;
        data["active"] = this.active;
        data["confirmed"] = this.confirmed;
        data["audtUser"] = this.audtUser;
        data["audtDate"] = this.audtDate;
        data["createdBy"] = this.createdBy;
        data["createDate"] = this.createDate;
        data["waveOff"] = this.waveOff;
        data["type_of_notice_period"] = this.type_of_notice_period;
        data["reinstate"] = this.reinstate;
        data["reinstateDate"] = this.reinstateDate;
        data["reinstateReason"] = this.reinstateReason;
        data["allowanceType"] = this.allowanceType;
        data["allowanceAmt"] = this.allowanceAmt;
        data["allowanceQty"] = this.allowanceQty;
        data["id"] = this.id;
        data["contractExpDate"] = this.contractExpDate;
        data["oldEmployeeID"] = this.employeeOldId
        data["mStatus"] = this.maritalStatus;
        data["eoBiNo"] = this.eobiNo;
        data["sscNo"] = this.sscNo;
        data["eobiAmt"] = this.eobiAmt;
        data["sScAmt"] = this.sscAmt;
        data["joningReport"] = this.joningReport
        data["joningReport"]=this.joningReport;
        data["signEmplForm"]=this.signEmplForm;
        data["pasPhoto"]=this.pasPhoto;
        data["refCninc"]=this.refCninc;
        data["resPrevEmp"]=this.resPrevEmp;
        data["salarySlip"]=this.salarySlip;
        data["serviceCertificate"]=this.serviceCertificate;
        data["validCnic"]=this.validCnic;
        data["academicRec"]=this.academicRec;
        data["cVResume"]=this.cVResume;
        data["disclosureForm"]=this.disclosureForm;
        data["eBankID"]=this.eBankID;
        data["lunchStatus"]= this.lunchStatus;
        return data;
    }
}