import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { CreateOrEditEmployeesDto } from "../shared/dto/employee-dto";
import { EmployeesServiceProxy } from "../shared/services/employee-service";
import { PayRollLookupTableModalComponent } from "@app/finders/payRoll/payRoll-lookup-table-modal.component";
import { EmployeeSalaryServiceProxy } from "../shared/services/employeeSalary.service";
import { AppConsts } from "@shared/AppConsts";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { Guid } from "guid-typescript";
import { Lightbox } from "ngx-lightbox";
import { EmployerBankDto } from "../shared/dto/employerBank-dto";
import { debug } from "console";

@Component({
    selector: "createOrEditEmployeesModal",
    templateUrl: "./create-or-edit-employee-modal.component.html",
    styleUrls: ["./create-or-edit-employee-modal.component.css"],
})
export class CreateOrEditEmployeesModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("PayRollLookupTableModal", { static: true })
    PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employees: CreateOrEditEmployeesDto = new CreateOrEditEmployeesDto();

    audtDate: Date;
    createDate: Date;
    target: any;
    educationName: string;
    locationName: string;
    religionName: string;
    deptName: string;
    designationName: string;
    subDesignationName: string;
    shiftName: string;
    empTypeName: string;
    secName: string;
    gender: string = "male";
    dateOfBirth: string;
    apointment_date: string;
    date_of_joining: string;
    date_of_leaving: string;
    EmployerBanks: EmployerBankDto[];

    reinstate_date: string;
    contractExpDate: string;
    first_rest_days = "1";
    second_rest_days = "1";
    first_rest_days_w2 = "1";
    second_rest_days_w2 = "1";

    type_of_notice_period = "Select Type of Notice Period";
    
    payment_mode = "Select Payment Mode";
    uploadUrl: string;
    profileUploadUrl: string = "";
    uploadedFiles: any[] = [];

    readonly employeeAppId = 1;
    readonly employeeProfileAppId = 2;
    readonly appName = "Employee";
    fileName: string;
    checkImage: boolean = false;
    fileId: string;
    contentType: string;
    url: string;
    alAmtChk:boolean;
    alQtyChk:boolean;
    image: any[] = [];
    branchCode:string;
    accountNo:string;

    constructor(
        injector: Injector,
        private _EmployeesServiceProxy: EmployeesServiceProxy,
        private _employeeSalaryServiceProxy: EmployeeSalaryServiceProxy,
        private _lightbox: Lightbox,
        private _fileDownloadServiceProxy: FileDownloadService
    ) {
        super(injector);
    }

    show(EmployeesId?: number): void {
        this.image = [];
        this.profileUploadUrl = null;
        this.url = null;
        this.audtDate = null;
        this.createDate = null;
        this.uploadedFiles = [];
        this.checkImage = true;
        this.getEmployerBank();

        if (!EmployeesId) {
            this.rowData = [];
            this.first_rest_days = "1";
            this.second_rest_days = "1";
            this.first_rest_days_w2 = "1";
            this.second_rest_days_w2 = "1";
            this.type_of_notice_period = "Select Type of Notice Period";
            this.payment_mode = "Select Payment Mode";
            this.employees = new CreateOrEditEmployeesDto();
            this.employees.id = EmployeesId;
            this.employees.duty_Hours = 8;
            this.dateOfBirth = "";
            this.apointment_date = "";
            this.date_of_joining = "";
            this.date_of_leaving = "";
            this.educationName = "";
            this.deptName = "";
            this.designationName = "";
            this.subDesignationName = "";
            this.secName = "";
            this.empTypeName = "";
            this.shiftName = "";
            this.religionName = "";
            this.gender = "male";
            this.reinstate_date = "";
            this.contractExpDate = "";
            this.employees.allowanceType = 0;
            this.employees.maritalStatus = "S";
            this.locationName = "";
            this.employees.active = true;
            this._EmployeesServiceProxy
                .getMaxEmployeeId()
                .subscribe((result) => {
                    this.employees.employeeID = result;
                });
            this.active = true;
            this.modal.show();
        } else {
            this.empTypeName = null;
            this.religionName = null;
            this.deptName = null;
            this.designationName = null;
            this.subDesignationName = null;
            this.shiftName = null;
            this.secName = null;
            this.educationName = null;
            this.locationName = null;

            this._EmployeesServiceProxy
                .getEmployeesForEdit(EmployeesId)
                .subscribe((result) => {
                    debugger;
                    this.employees = result.employees;
                    this.branchCode = this.employees.account_no.split("-")[0];
                    this.accountNo = this.employees.account_no.split("-")[1];
                    this._EmployeesServiceProxy
                        .getImage(
                            this.employeeProfileAppId,
                            result.employees.employeeID
                        )
                        .subscribe((imageResult) => {

                            if (imageResult != null) {
                                this.profileUploadUrl =
                                    "data:image/jpeg;base64," + imageResult;
                            }
                        });

                    this._EmployeesServiceProxy
                        .getFile(
                            this.employeeAppId,
                            result.employees.employeeID
                        )
                        .subscribe((fileResult) => {
                               console.log(fileResult);
                            if (fileResult != null) {
                                this.fileName =fileResult[0].toString();
                                this.fileId=fileResult[0].toString();
                                const album = {
                                    src: this.url,
                                };
                                debugger
                                this.image.push(album);
                                this.checkImage = false;
                            }
                        });

                    this._employeeSalaryServiceProxy
                        .getEmployeeSalaryHistory(result.employees.employeeID)
                        .subscribe((result) => {
                            this.rowData = [];
                            this.rowData = result.items;
                        });

                    if (this.employees.edID) {
                        this._EmployeesServiceProxy
                            .getPickerName(this.employees.edID, "Education")
                            .subscribe((result) => {
                                this.educationName = result;
                            });
                    }
                    if (this.employees.religionID) {
                        this._EmployeesServiceProxy
                            .getPickerName(
                                this.employees.religionID,
                                "Religion"
                            )
                            .subscribe((result) => {
                                this.religionName = result;
                            });
                    }
                    debugger
                    if (this.employees.locID) {
                        this._EmployeesServiceProxy
                            .getPickerName(
                                this.employees.locID,
                                "Location"
                            )
                            .subscribe((result) => {
                                this.locationName = result;
                            });
                    }
                    if (this.employees.deptID) {
                        this._EmployeesServiceProxy
                            .getPickerName(this.employees.deptID, "Department")
                            .subscribe((result) => {
                                this.deptName = result;
                            });
                    }
                    if (this.employees.designationID) {
                        this._EmployeesServiceProxy
                            .getPickerName(
                                this.employees.designationID,
                                "Designation"
                            )
                            .subscribe((result) => {
                                this.designationName = result;
                            });
                    }
                    if (this.employees.subDesignationID) {
                        this._EmployeesServiceProxy
                            .getPickerName(
                                this.employees.subDesignationID,
                                "SubDesignation"
                            )
                            .subscribe((result) => {
                                this.subDesignationName = result;
                            });
                    }
                    if (this.employees.shiftID) {
                        this._EmployeesServiceProxy
                            .getPickerName(this.employees.shiftID, "Shift")
                            .subscribe((result) => {
                                this.shiftName = result;
                            });
                    }
                    if (this.employees.typeID) {
                        this._EmployeesServiceProxy
                            .getPickerName(
                                this.employees.typeID,
                                "EmploymentType"
                            )
                            .subscribe((result) => {
                                this.empTypeName = result;
                            });
                    }
                    if (this.employees.secID) {
                        this._EmployeesServiceProxy
                            .getPickerName(this.employees.secID, "Section")
                            .subscribe((result) => {
                                this.secName = result;
                            });
                    }

                    this.first_rest_days =
                        this.employees.first_rest_days != null
                            ? String(this.employees.first_rest_days)
                            : "Select Rest Day";
                    this.second_rest_days =
                        this.employees.second_rest_days != null
                            ? String(this.employees.second_rest_days)
                            : "Select Rest Day";
                    this.first_rest_days_w2 =
                        this.employees.first_rest_days_w2 != null
                            ? String(this.employees.first_rest_days_w2)
                            : "Select Rest Day";
                    this.second_rest_days_w2 =
                        this.employees.second_rest_days_w2 != null
                            ? String(this.employees.second_rest_days_w2)
                            : "Select Rest Day";
                    this.payment_mode =
                        this.employees.payment_mode != null
                            ? String(this.employees.payment_mode)
                            : "Select Payment Mode";

                    this.type_of_notice_period =
                        this.employees.type_of_notice_period != null
                            ? String(this.employees.type_of_notice_period)
                            : "Select Type of Notice Period";

                    switch (this.employees.gender) {
                        case "M":
                            this.gender = "male";
                            break;
                        case "F":
                            this.gender = "female";
                            break;
                        default:
                            break;
                    }
                    this.employees.date_of_birth == null
                        ? (this.dateOfBirth = "")
                        : (this.dateOfBirth = moment(
                            this.employees.date_of_birth
                        ).format("DD/MM/YYYY"));
                    this.employees.apointment_date == null
                        ? (this.apointment_date = "")
                        : (this.apointment_date = moment(
                            this.employees.apointment_date
                        ).format("DD/MM/YYYY"));
                    this.employees.date_of_joining == null
                        ? (this.date_of_joining = "")
                        : (this.date_of_joining = moment(
                            this.employees.date_of_joining
                        ).format("DD/MM/YYYY"));
                    this.employees.date_of_leaving == null
                        ? (this.date_of_leaving = "")
                        : (this.date_of_leaving = moment(
                            this.employees.date_of_leaving
                        ).format("DD/MM/YYYY"));
                    this.employees.reinstateDate == null
                        ? (this.reinstate_date = "")
                        : (this.reinstate_date = moment(
                            this.employees.reinstateDate
                        ).format("DD/MM/YYYY"));
                    this.employees.contractExpDate == null
                        ? (this.contractExpDate = "")
                        : (this.contractExpDate = moment(
                            this.employees.contractExpDate
                        ).format("DD/MM/YYYY"));
                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        ;
        this.saving = true;
debugger
        this.employees.audtDate = moment();
        this.employees.audtUser = this.appSession.user.userName;

        this.employees.date_of_birth = moment(this.dateOfBirth).endOf("day");
        this.employees.apointment_date = moment(this.apointment_date).endOf(
            "day"
        );
        this.employees.date_of_joining = moment(this.date_of_joining).endOf(
            "day"
        );
        // this.employees.active
        //     ? (this.employees.date_of_leaving = null)
        //     : (this.employees.date_of_leaving = moment(
        //         this.date_of_leaving
        //     ).endOf("day"));
            this.employees.active
            ? (this.employees.date_of_leaving = null)
            : (this.employees.date_of_leaving = moment(
                this.date_of_leaving
            ).endOf("day"));
        this.employees.reinstateDate = moment(this.reinstate_date).endOf("day");
        this.employees.contractExpDate = moment(this.contractExpDate).endOf(
            "day"
        );

        this.employees.first_rest_days = Number(this.first_rest_days);
        this.employees.second_rest_days = Number(this.second_rest_days);

        this.employees.first_rest_days_w2 = Number(this.first_rest_days_w2);
        this.employees.second_rest_days_w2 = Number(this.second_rest_days_w2);

        this.employees.type_of_notice_period =
            this.type_of_notice_period == "Select Type of Notice Period"
                ? null
                : Number(this.type_of_notice_period);
                debugger
        // this.employees.payment_mode =
        //     this.payment_mode == "Select Payment Mode"
        //         ? null
        //         : String(this.payment_mode);

        if (!this.employees.id) {
            this.employees.createDate = moment();
            this.employees.createdBy = this.appSession.user.userName;
        }
        switch (this.gender) {
            case "male":
                this.employees.gender = "M";
                break;
            case "female":
                this.employees.gender = "F";
                break;
            default:
                break;
        }

       
        this._EmployeesServiceProxy
            .createOrEdit(this.employees)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
    }
    uncheck(){
        this.alQtyChk=false;
        this.alAmtChk=false;
        if(this.employees.allowanceType==0){
this.employees.allowanceAmt=undefined;
this.employees.allowanceQty=undefined;
        }
    }
    SetpaymentMode(){
        debugger;
    }
    Allowancereadonly(){
        debugger
       var allamt= this.employees.allowanceAmt;
       var allQty= this.employees.allowanceQty;
       if(allamt==undefined || allamt==null || allamt==0){
           this.alAmtChk=true;
           this.alQtyChk=false;
       }
       if(allQty==undefined || allQty==null || allQty==0){
        
        this.alAmtChk=false;
        this.alQtyChk=true;
       }
        if((allamt==undefined  || allamt==null || allamt==0) && (allQty==undefined || allQty==null || allQty==0)){
        this.alQtyChk=false;
        this.alAmtChk=false;
       }
    }
    downloadFile()
    {
        ;
        let temp=this.fileName.toLowerCase().split(".")[1];
        debugger
        switch (temp){
            case 'png':
                this.contentType="image/png";
                break;
            case 'jpeg':
                this.contentType="image/jpeg";
                break;
            case 'txt':
                this.contentType="text/plain";
                break;
            case 'xls':
                this.contentType="application/vnd.ms-excel";
                break;
            case 'xlsx':
                this.contentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case 'pdf':
                this.contentType="application/pdf";
                break;
            case 'csv':
                this.contentType="text/csv";
                break;
            // case 'zip':
            //    this.contentType="zip";
            //    break;
        }

        this._fileDownloadServiceProxy.downloadBinaryFile(Guid.parse(this.fileId),this.contentType,this.fileName);

    }
    removeFile() {
        this._fileDownloadServiceProxy
            .deleteFile(
                this.employeeAppId,
                this.employees.employeeID
            )
            .subscribe((fileResult) => {
                console.log(fileResult);
                debugger
                this.checkImage = true;
            });
    }
    getNewPayRollModal() {
        switch (this.target) {
            case "Education":
                this.getNewEducation();
                break;
            case "Religion":
                this.getNewReligion();
                break;
            case "Department":
                this.getNewDepartment();
                break;
            case "Designation":
                this.getNewDesignation();
                break;
            case "SubDesignation":
                this.getNewSubDesignation();
                break;
            case "Shift":
                this.getNewShift();
                break;
            case "EmploymentType":
                this.getNewEmpType();
                break;
            case "Section":
                this.getNewSection();
                break;
            case "Location":
                this.getNewLocation();
                break;
            default:
                break;
        }
    }
    //////////////////////////////////////////////////////EDUCATION//////////////////////////////////////////////////////////////////////////
    openEducationModal() {
        ;
        this.target = "Education";
        this.PayRollLookupTableModal.id = String(this.employees.edID);
        this.PayRollLookupTableModal.displayName = this.educationName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewEducation() {
        this.employees.edID = Number(this.PayRollLookupTableModal.id);
        this.educationName = this.PayRollLookupTableModal.displayName;
    }

    setEducationNull() {
        this.employees.edID = null;
        this.educationName = "";
    }
    //////////////////////////////////////////////////////EDUCATION//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////RELIGION//////////////////////////////////////////////////////////////////////////
    openReligionModal() {
        ;
        this.target = "Religion";
        this.PayRollLookupTableModal.id = String(this.employees.religionID);
        this.PayRollLookupTableModal.displayName = this.religionName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewReligion() {
        this.employees.religionID = Number(this.PayRollLookupTableModal.id);
        this.religionName = this.PayRollLookupTableModal.displayName;
    }

    setReligionNull() {
        this.employees.religionID = null;
        this.religionName = "";
    }
    //////////////////////////////////////////////////////RELIGION//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openDepartmentModal() {
        ;
        this.target = "Department";
        debugger
        this.PayRollLookupTableModal.id = String(this.employees.deptID);
        this.PayRollLookupTableModal.displayName = this.deptName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewDepartment() {
        this.employees.deptID = Number(this.PayRollLookupTableModal.id);
        this.deptName = this.PayRollLookupTableModal.displayName;
    }

    setDepartmentNull() {
        this.employees.deptID = null;
        this.deptName = "";
    }
    //////////////////////////////////////////////////////Department////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////Designation////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openDesignationModal() {
        ;
        this.target = "Designation";
        this.PayRollLookupTableModal.id = String(this.employees.designationID);
        this.PayRollLookupTableModal.displayName = this.designationName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewDesignation() {
        this.employees.designationID = Number(this.PayRollLookupTableModal.id);
        this.designationName = this.PayRollLookupTableModal.displayName;
    }

    setDesignationNull() {
        this.employees.designationID = null;
        this.designationName = "";
    }
    //////////////////////////////////////////////////////Designation//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////Sub Designation////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openSubDesignationModal() {
        ;
        this.target = "SubDesignation";
        this.PayRollLookupTableModal.id = String(
            this.employees.subDesignationID
        );
        this.PayRollLookupTableModal.displayName = this.subDesignationName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewSubDesignation() {
        this.employees.subDesignationID = Number(
            this.PayRollLookupTableModal.id
        );
        this.subDesignationName = this.PayRollLookupTableModal.displayName;
    }

    setSubDesignationNull() {
        this.employees.subDesignationID = null;
        this.subDesignationName = "";
    }
    //////////////////////////////////////////////////////Sub Designation//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////Shift////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openShiftModal() {
        ;
        this.target = "Shift";
        this.PayRollLookupTableModal.id = String(this.employees.shiftID);
        this.PayRollLookupTableModal.displayName = this.shiftName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewShift() {
        this.employees.shiftID = Number(this.PayRollLookupTableModal.id);
        this.shiftName = this.PayRollLookupTableModal.displayName;
        this.employees.duty_Hours = Number(
            this.PayRollLookupTableModal.dutyHours
        );
    }

    setShiftNull() {
        this.employees.shiftID = null;
        this.shiftName = "";
    }
    //////////////////////////////////////////////////////Shift//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////EmploymentType////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openEmpTypeModal() {
        ;
        this.target = "EmploymentType";
        this.PayRollLookupTableModal.id = String(this.employees.typeID);
        this.PayRollLookupTableModal.displayName = this.empTypeName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewEmpType() {
        this.employees.typeID = Number(this.PayRollLookupTableModal.id);
        this.empTypeName = this.PayRollLookupTableModal.displayName;
    }

    setEmpTypeNull() {
        this.employees.typeID = null;
        this.empTypeName = "";
    }
    //////////////////////////////////////////////////////EmploymentType//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////Section////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Department//////////////////////////////////////////////////////////////////////////
    openSectionModal() {
        ;
        this.target = "Section";
        this.PayRollLookupTableModal.id = String(this.employees.secID);
        this.PayRollLookupTableModal.displayName = this.secName;
        this.PayRollLookupTableModal.show(this.target);
    }

    getNewSection() {
        this.employees.secID = Number(this.PayRollLookupTableModal.id);
        this.secName = this.PayRollLookupTableModal.displayName;
    }

    setSectionNull() {
        this.employees.secID = null;
        this.secName = "";
    }
    //////////////////////////////////////////////////////Section//////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////Location//////////////////////////////////////////////////////////////////////////

    openLocationModal() {
        ;
        this.target = "Location";
        this.PayRollLookupTableModal.id = String(this.employees.locID);
        this.PayRollLookupTableModal.displayName = this.locationName;
        this.PayRollLookupTableModal.show(this.target);
    }


    getNewLocation() {
        this.employees.locID = Number(this.PayRollLookupTableModal.id);
        this.locationName = this.PayRollLookupTableModal.displayName;
    }

    setLocationNull() {
        this.employees.locID = null;
        this.locationName = "";
    }

    //////////////////////////////////////////////////////Location//////////////////////////////////////////////////////////////////////////
    close(): void {
        this.active = false;
        this.modal.hide();
        this._lightbox.close();
    }

    //==================================Grid=================================
    public rowData;

    columnDefs = [
        {
            headerName: "SrNo",
            field: "srNo",
            width: 55,
            sortable: true,
            valueGetter: "node.rowIndex+1",
        },
        {
            headerName: "Start Date",
            field: "startDate",
            resizable: true,
            sortable: true,
            filter: true,
            valueFormatter: this.dateFormatter,
        },
        {
            headerName: "Gross Salary",
            field: "gross_Salary",
            resizable: true,
            sortable: true,
            filter: true,
        },
        // {
        //     headerName: "Bank Amount",
        //     field: "bank_Amount",
        //     resizable: true,
        //     sortable: true,
        //     filter: true,
        // },
        {
            headerName: "Basic Salary",
            field: "basic_Salary",
            resizable: true,
            sortable: true,
            filter: true,
        },
        {
            headerName: "Tax",
            field: "tax",
            resizable: true,
            sortable: true,
            filter: true,
        },
        {
            headerName: "House Rent",
            field: "house_Rent",
            resizable: true,
            sortable: true,
            filter: true,
        },
        // {
        //     headerName: "Net Salary",
        //     field: "net_Salary",
        //     resizable: true,
        //     sortable: true,
        //     filter: true,
        // },
    ];

    dateFormatter(params) {
        return moment(params.value).format("DD/MM/YYYY");
    }

    //==================================Grid=================================
    //===========================File Attachment=============================
    onBeforeUpload(event): void {
        ;
        this.uploadUrl =
            AppConsts.remoteServiceBaseUrl + "/DemoUiComponents/UploadFiles?";
        if (this.employeeAppId !== undefined)
            this.uploadUrl +=
                "APPID=" + encodeURIComponent("" + this.employeeAppId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl +=
                "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.employees.employeeID !== undefined)
            this.uploadUrl +=
                "DocID=" +
                encodeURIComponent("" + this.employees.employeeID) +
                "&";
        this.uploadUrl = this.uploadUrl.replace(/[?&]$/, "");
    }

    onUpload(event): void {
        this.checkImage = true;
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }
    }
    //===========================File Attachment=============================

    //===========================Profile Picture=============================
    onBeforeProfileUpload(event): void {
        ;
        this.profileUploadUrl =
            AppConsts.remoteServiceBaseUrl + "/DemoUiComponents/UploadFiles?";
        if (this.employeeProfileAppId !== undefined)
            this.profileUploadUrl +=
                "APPID=" +
                encodeURIComponent("" + this.employeeProfileAppId) +
                "&";
        if (this.appName !== undefined)
            this.profileUploadUrl +=
                "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.employees.employeeID !== undefined)
            this.profileUploadUrl +=
                "DocID=" +
                encodeURIComponent("" + this.employees.employeeID) +
                "&";
        this.profileUploadUrl = this.profileUploadUrl.replace(/[?&]$/, "");
    }
    onProfileUpload(event): void {
        ;
        this._EmployeesServiceProxy
            .getImage(this.employeeProfileAppId, this.employees.employeeID)
            .subscribe((imageResult) => {
                ;
                if (imageResult != null) {
                   
                    this.profileUploadUrl =
                        "data:image/png;base64," + imageResult;
                }
            });
    }

    open(): void {
        ;
        this._lightbox.open(this.image);
    }

    updateFirstRestDayW2(val): void {
        // if (val == "2")
        //     this.second_rest_days = "0";
        this.first_rest_days_w2 = val;
    }

    updateSecondRestDayW2(val): void {
        this.second_rest_days_w2 = val;
    }

    isValidId(val) {
        ;
        this._EmployeesServiceProxy
            .checkValidEmpId(val.currentTarget.value)
            .subscribe((res) => {
                //  console.log(res["result"]);
                if (res["result"] == false) {
                    this.message.error("Employee ID already exists");
                    this.employees.employeeID = undefined;
                }
            });
    }
    getEmployerBank() {
        this._EmployeesServiceProxy.getEmployerBankList().subscribe((res) => {
            this.EmployerBanks = res["result"]
        })
    }

    BindAccNumber(val, type) {
        debugger;
        switch (type) {
            case "branch":
                this.employees.account_no = val + "-";
                break;
            case "account":
                this.employees.account_no += val;
                break;
        }
    }
}
