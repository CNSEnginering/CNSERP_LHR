import * as moment from 'moment';
import { GetEmployeesForViewDto, EmployeesDto, CreateOrEditEmployeesDto } from '../dto/employee-dto';


export interface IPagedResultDtoOfGetEmployeesForViewDto {
    totalCount: number | undefined;
    items: GetEmployeesForViewDto[] | undefined;
}

export interface IGetEmployeesForViewDto {
    employees: EmployeesDto | undefined;
}

export interface IEmployeesDto {
    employeeID: number | undefined;
    employeeName: string | undefined;
    fatherName: string | undefined;
    date_of_birth: moment.Moment | undefined;
    home_address: string | undefined;
    phoneNo: string | undefined;
    ntn: string | undefined;
    apointment_date: moment.Moment | undefined;
    date_of_joining: moment.Moment | undefined;
    date_of_leaving: moment.Moment | undefined;
    city: string | undefined;
    cnic: string | undefined;
    edID: number | undefined;
    deptID: number | undefined;
    designationID: number | undefined;
    subDesignationID: number | undefined;
    gender: string | undefined;
    status: boolean | undefined;
    shiftID: number | undefined;
    typeID: number | undefined;
    secID: number | undefined;
    religionID: number | undefined;
    social_security: boolean | undefined;
    eobi: boolean | undefined;
    wppf: boolean | undefined;
    payment_mode: string | undefined;
    bank_name: string | undefined;
    account_no: string | undefined;
    academic_qualification: string | undefined;
    professional_qualification: string | undefined;
    first_rest_days: number | undefined;
    second_rest_days: number | undefined;
    first_rest_days_w2: number | undefined;
    second_rest_days_w2: number | undefined;
    bloodGroup: string | undefined;
    reference: string | undefined;
    visa_Details: string | undefined;
    driving_Licence: string | undefined;
    duty_Hours: number | undefined;
    active: boolean | undefined;
    confirmed: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    waveOff: boolean | undefined;
    type_of_notice_period: number | undefined;
    reinstate: boolean | undefined;
    reinstateDate: moment.Moment | undefined;
    reinstateReason: string | undefined;
    allowanceType: number;
    allowanceAmt: number;
    allowanceQty: number;
    id: number | undefined;
    contractExpDate: moment.Moment | undefined;
}

export interface IGetEmployeesForEditOutput {
    employees: CreateOrEditEmployeesDto | undefined;
}

export interface ICreateOrEditEmployeesDto {
    employeeID: number;
    employeeName: string;
    fatherName: string | undefined;
    date_of_birth: moment.Moment | undefined;
    home_address: string | undefined;
    phoneNo: string | undefined;
    ntn: string | undefined;
    apointment_date: moment.Moment | undefined;
    date_of_joining: moment.Moment | undefined;
    date_of_leaving: moment.Moment | undefined;
    city: string | undefined;
    cnic: string | undefined;
    edID: number | undefined;
    locID: number | undefined;
    deptID: number | undefined;
    designationID: number | undefined;
    subDesignationID: number | undefined;
    gender: string | undefined;
    status: boolean | undefined;
    shiftID: number | undefined;
    typeID: number | undefined;
    secID: number | undefined;
    religionID: number | undefined;
    social_security: boolean | undefined;
    eobi: boolean | undefined;
    wppf: boolean | undefined;
    payment_mode: string | undefined;
    bank_name: string | undefined;
    account_no: string | undefined;
    academic_qualification: string | undefined;
    professional_qualification: string | undefined;
    first_rest_days: number | undefined;
    second_rest_days: number | undefined;
    first_rest_days_w2: number | undefined;
    second_rest_days_w2: number | undefined;
    bloodGroup: string | undefined;
    reference: string | undefined;
    visa_Details: string | undefined;
    driving_Licence: string | undefined;
    duty_Hours: number | undefined;
    active: boolean | undefined;
    confirmed: boolean | undefined;
    audtUser: string | undefined;
    audtDate: moment.Moment | undefined;
    createdBy: string | undefined;
    createDate: moment.Moment | undefined;
    waveOff: boolean | undefined;
    type_of_notice_period: number | undefined;
    reinstate: boolean | undefined;
    reinstateDate: moment.Moment | undefined;
    reinstateReason: string | undefined;
    allowanceType: number;
    allowanceAmt: number;
    allowanceQty: number;
    id: number | undefined;
    contractExpDate: moment.Moment | undefined;
    employeeOldId:number|undefined;
    maritalStatus:string|undefined;
    eobiNo:string|undefined;
    sscNo:string|undefined;
    eobiAmt: number | undefined;
    sscAmt: number | undefined;
    joningReport:boolean| undefined;
    signEmplForm:boolean| undefined;
    academicRec:boolean| undefined;
    pasPhoto:boolean| undefined;
    harrasmentComp:boolean| undefined;
    validCnic:boolean| undefined;
    cVResume:boolean| undefined;
    salarySlip:boolean| undefined;
    serviceCertificate:boolean|undefined;
    refCninc:boolean|undefined;
    resPrevEmp:boolean|undefined;
    disclosureForm:boolean|undefined;
}
