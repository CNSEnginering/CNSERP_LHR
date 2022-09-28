import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditAttendanceHeaderDto } from '../shared/dto/attendanceHeader-dto';
import { AttendanceHeaderService } from '../shared/services/attendanceHeader.service';
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';
import { CreateOrEditAttendanceDetailDto } from '../shared/dto/attendanceDetail-dto';
import { AttendanceDetailsServiceProxy } from '../shared/services/attendanceDetail.service';
import { TimeEditorComponent } from './timeComponents/time-editor.component';
import { TimeRendererComponent } from './timeComponents/time-renderer.component';
import { GridOptions } from 'ag-grid-community';
import * as XLSX from 'xlsx';
import { FileDownloadService } from '@shared/utils/file-download.service';


@Component({
    selector: 'createOrEditAttendanceModal',
    templateUrl: './create-or-edit-attendance-modal.component.html'
})
export class CreateOrEditAttendanceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    active = false;
    saving = false;
    processing = false;
    private setParms;
    absent: boolean;
    attendanceHeader: CreateOrEditAttendanceHeaderDto = new CreateOrEditAttendanceHeaderDto();
    attendanceDetail: CreateOrEditAttendanceDetailDto = new CreateOrEditAttendanceDetailDto();


    public gridOptions: GridOptions;

    target: string;
    docDate: Date;
    isUpdate: boolean;
    check: boolean;
    todayDate: Date = new Date();


    constructor(
        injector: Injector,
        private _attendanceHeaderService: AttendanceHeaderService,
        private _attendanceDetailService: AttendanceDetailsServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    exportToExcel() {
        debugger;
        this._attendanceDetailService.getAttendanceInExcelFile(this.attendanceHeader.id)
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });

    }

    importFromExcel(event) {
        debugger;
        let workBook = null;
        let jsonData = null;
        const reader = new FileReader();

        const file = event.target.files[0];
        reader.readAsBinaryString(file);
        reader.onload = (event) => {
            const data = reader.result;
            debugger;
            workBook = XLSX.read(data, { type: 'binary' });
            jsonData = workBook.SheetNames.reduce((initial, name) => {
                const sheet = workBook.Sheets[name];
                initial = XLSX.utils.sheet_to_json(sheet);
                return initial;
            }, {});

            this.rowData = [];
            this.rowData = jsonData;
            this.active = true;
            this.modal.show();
        }

    }

    show(flag: boolean | undefined, attendanceHeaderId?: number): void {
        debugger;

        if (!flag) {
            debugger;
            this.attendanceHeader = new CreateOrEditAttendanceHeaderDto();
            this.attendanceHeader.id = attendanceHeaderId;
            this.attendanceHeader.flag = flag;
            this.docDate = new Date();
            // this.docDate = moment().format("MM/DD/YYYY");
            this.rowData = [];
            this.active = true;
            this.modal.show();
        } else {
            this._attendanceHeaderService.getAttendanceHeaderForEdit(attendanceHeaderId).subscribe(result => {
                debugger;
                this.attendanceHeader = result.attendanceHeader;
                //console.log(result.attendanceHeader.attendanceDetail);
                this.rowData = result.attendanceHeader.attendanceDetail;
                this.attendanceHeader.flag = flag;
                this.isUpdate = flag;
                this.docDate = moment(result.attendanceHeader.docDate).toDate();
                // this.docDate = moment(result.attendanceHeader.docDate).format("MM/DD/YYYY");
                this.active = true;
                this.modal.show();
            });
        }
    }
    //==================================Grid=================================
    private gridApi;
    private gridColumnApi;
    private rowData;
    private rowSelection;
    columnDefs = [

        { headerName: this.l('SrNo'), field: 'srNo', sortable: true, width: 60, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('EmployeeID'), field: 'employeeID', sortable: true, width: 120, resizable: true },
        { headerName: this.l(''), field: 'addEmployeeId', width: 25, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        { headerName: this.l('EmployeeName'), field: 'employeeName', sortable: true, filter: true, width: 120, resizable: true },
        { headerName: this.l('ShiftID'), field: 'shiftID', sortable: true, filter: true, width: 100, editable: false, resizable: true },
        { headerName: this.l(''), field: 'addShiftID', width: 25, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        {
            headerName: this.l('TimeIn'), field: 'timeIn', editable: true, sortable: true, filter: true, width: 100, resizable: true,
            cellEditorFramework: TimeEditorComponent, cellRendererFramework: TimeRendererComponent
        },
        {
            headerName: this.l('TimeOut'), field: 'timeOut', editable: true, sortable: true, filter: true, width: 100, resizable: true,
            cellEditorFramework: TimeEditorComponent, cellRendererFramework: TimeRendererComponent
        },
        {
            headerName: this.l('BreakOut'), field: 'breakOut', sortable: true, width: 100, editable: true, resizable: true,
            cellEditorFramework: TimeEditorComponent, cellRendererFramework: TimeRendererComponent
        },
        {
            headerName: this.l('BreakIn'), field: 'breakIn', editable: true, sortable: true, filter: true, width: 100, resizable: true,
            cellEditorFramework: TimeEditorComponent, cellRendererFramework: TimeRendererComponent
        }

        // },
        // { headerName: this.l('TotalHrs'), field: 'totalHrs', sortable: true, filter: true, width: 100, editable: true, resizable: true , type: "numericColumn"}
    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];
        if (this.isUpdate) {
            this.rowData = this.attendanceHeader.attendanceDetail;
        }
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onAddRow(type: string): void {
        debugger;
        var index = this.gridApi.getDisplayedRowCount();
        if (type == "Add") {
            var newItem = this.createNewRowData();
            this.gridApi.updateRowData({ add: [newItem] });
        }
        else if (type == "Process") {

            this.processing = true;
            debugger;
            this._attendanceDetailService.getEmployeeDetails().subscribe(result => {
                debugger;
                this.rowData = [];
                this.rowData = result.items;
                if (this.absent) {
                    this.rowData.forEach(node => {
                        node.data.timeIn = null;
                        node.data.timeOut = null;
                    })
                };
                this.processing = false;
            });
        }
        this.gridApi.refreshCells();
        this.onBtStartEditing(index, "addEmployeeId");
    }

    onCellClicked(params) {
        debugger;
        if (params.column["colId"] == "addEmployeeId") {
            this.setParms = params;
            this.openEmployeeModal();
        }
        if (params.column["colId"] == "addShiftID") {
            this.setParms = params;
            this.openShiftModal();
        }
    }

    addIconCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }

    onBtStartEditing(index, col) {
        debugger;
        this.gridApi.setFocusedCell(index, col);
        this.gridApi.startEditingCell({
            rowIndex: index,
            colKey: col
        });
    }


    onRemoveSelected() {
        debugger;
        var selectedData = this.gridApi.getSelectedRows();
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
    }

    createNewRowData() {
        debugger;
        var newData = {
            employeeID: "",
            employeeName: "",
            shiftID: "",
            timeIn: "",
            timeOut: "",
            breakIn: "",
            breakOut: "",
            totalHrs: "",
        };
        return newData;
    }


    onCellValueChanged(params) {
        debugger;
        this.gridApi.refreshCells();
    }
    onCellEditingStarted(params) {
        debugger;

    }

    //==================================Grid=================================

    save(): void {
        debugger;

        this.message.confirm(
            'Save Attendance',
            (isConfirmed) => {
                if (isConfirmed) {
                    debugger;
                    var count = this.gridApi.getDisplayedRowCount();

                    if (count == 0) {
                        this.notify.error(this.l('Please Enter Grid Data'));
                        return;
                    }

                    this.gridApi.forEachNode(node => {
                        if (node.data.employeeID == "") {
                            this.notify.error(this.l('Please Enter Employee ID'));
                            this.check = true;
                        }
                        else {
                            this.check = false;
                        }
                    });
                    if (this.check) {
                        return;
                    }
                    if (this.absent) {
                        this.gridApi.forEachNode(node => {
                            node.data.timeIn = null;
                            node.data.timeOut = null;
                        });
                    }
                    else {
                        this.gridApi.forEachNode(node => {
                            if (moment(node.data.timeIn).format('A') == 'PM' && moment(node.data.timeOut).format('A') == 'AM') {
                                node.data.timeOut = moment(moment(node.data.timeOut).add(1, 'days').format('YYYY-MM-DDTHH:mm:ss.SSS'));
                            }
                        });

                        this.gridApi.forEachNode(node => {
                            if (moment.duration(moment(node.data.timeOut).diff(moment(node.data.timeIn))).asMinutes() < 1) {
                                this.notify.error(this.l('Time Out can not be less than Time In'));
                                this.check = true;
                            }
                        });

                        this.gridApi.forEachNode(node => {
                            if (moment.duration(moment(node.data.breakIn).diff(moment(node.data.breakOut))).asMinutes() < 1) {
                                this.notify.error(this.l('Break In can not be less than Break Out'));
                                this.check = true;
                            }
                        });
                    }

                    if (this.check) {
                        return;
                    }

                    this.saving = true;

                    var rowData = [];
                    this.gridApi.forEachNode(node => {
                        rowData.push(node.data);
                    });
                    this.attendanceHeader.docDate = moment(this.docDate).startOf('day');
                    // this.attendanceHeader.docDate = moment(this.docDate).endOf('day');


                    if (!this.attendanceHeader.id) {
                        this.attendanceHeader.createDate = moment();
                        this.attendanceHeader.createdBy = this.appSession.user.userName;
                    }
                    this.attendanceHeader.attendanceDetail = rowData;
                    this.attendanceHeader.attendanceDetail.forEach(element => {
                        element.attendanceDate = moment(this.docDate);
                        // element.attendanceDate = moment(this.docDate).endOf('day');

                    });
                    this._attendanceHeaderService.createOrEditAttendanceHeader(this.attendanceHeader)
                        .pipe(finalize(() => { this.saving = false; }))
                        .subscribe(() => {
                            this.notify.info(this.l('SavedSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        });
                }
            }
        );
    }
    getNewPayRollModal() {
        switch (this.target) {
            case "Employee":
                this.getNewEmployee();
                break;
            case "Shift":
                this.getNewShift();
                break;
            default:
                break;
        }
    }
    /////////////////////////////////////////////////////////Employee/////////////////////////////////////////////////////
    openEmployeeModal() {
        debugger;
        this.target = "Employee";
        this.PayRollLookupTableModal.id = this.setParms.data.employeeID;
        this.PayRollLookupTableModal.displayName = this.setParms.data.employeeName;
        this.PayRollLookupTableModal.show(this.target);
    }

    setEmployeeNull() {
        this.setParms.data.employeeID = null;
        this.setParms.data.employeeName = "";
    }


    getNewEmployee() {
        debugger;
        this.setParms.data.employeeID = this.PayRollLookupTableModal.id;
        this.setParms.data.employeeName = this.PayRollLookupTableModal.displayName;
        this.gridApi.refreshCells();
    }
    /////////////////////////////////////////////////////////Employee/////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////Shift/////////////////////////////////////////////////////
    openShiftModal() {
        debugger;
        this.target = "Shift";
        this.PayRollLookupTableModal.id = this.setParms.data.shiftID;
        this.PayRollLookupTableModal.show(this.target);
    }

    setShiftNull() {
        this.setParms.data.shiftID = null;
    }


    getNewShift() {
        this.setParms.data.shiftID = this.PayRollLookupTableModal.id;
        this.gridApi.refreshCells();
    }
    /////////////////////////////////////////////////////////Shift/////////////////////////////////////////////////////

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
