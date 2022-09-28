import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { GridOptions } from 'ag-grid-community';
import { CreateOrEditGLSecurityHeaderDto } from '@app/main/finance/shared/dto/glSecurityHeader-dto';
import { CreateOrEditGLSecurityDetailDto } from '@app/main/finance/shared/dto/glSecurityDetail-dto';
import { GLSecurityHeaderService } from '@app/main/finance/shared/services/glSecurityHeader.service';
import { GLSecurityDetailsServiceProxy } from '@app/main/finance/shared/services/glSecurityDetail.service';
import { CheckboxCellComponent } from '@app/shared/common/checkbox-cell/checkbox-cell.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { CheckBoxCellComponent } from './checkBoxCell/checkbox-cell.component';


@Component({
    selector: 'createOrEditGLSecurityModal',
    templateUrl: './create-or-edit-glSecurity-modal.component.html'
})
export class CreateOrEditGLSecurityModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    active = false;
    saving = false;
    processing = false;
    private setParms;

    glSecurityHeader: CreateOrEditGLSecurityHeaderDto = new CreateOrEditGLSecurityHeaderDto();
    glSecurityDetail: CreateOrEditGLSecurityDetailDto = new CreateOrEditGLSecurityDetailDto();


    public gridOptions: GridOptions;

    target: string;
    isUpdate: boolean;
    check: boolean;


    constructor(
        injector: Injector,
        private _gLSecurityHeaderService: GLSecurityHeaderService,
        private _gLSecurityDetailService: GLSecurityDetailsServiceProxy,
    ) {
        super(injector);
    }


    show(flag: boolean | undefined, gLSecurityHeaderId?: number): void {
        debugger;

        if (!flag) {
            debugger;
            this.glSecurityHeader = new CreateOrEditGLSecurityHeaderDto();
            this.glSecurityHeader.id = gLSecurityHeaderId;
            this.glSecurityHeader.flag = flag;
            this.rowData = [];
            this.active = true;
            this.modal.show();
        } else {
            this._gLSecurityHeaderService.getGLSecurityHeaderForEdit(gLSecurityHeaderId).subscribe(result => {
                debugger;
                this.glSecurityHeader = result.glSecurityHeader;
                //console.log(result.GLSecurityHeader.GLSecurityDetail);
                this.rowData = result.glSecurityHeader.glSecurityDetail;
                this.glSecurityHeader.flag = flag;
                this.isUpdate = flag;
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
        { headerName: this.l('Allow'), field: 'canSee', sortable: true, filter: true, width: 70, resizable: true ,
        cellRendererFramework: CheckBoxCellComponent},
        { headerName: this.l('Allow All'), field: 'allowAll', sortable: true, filter: true, width: 70, resizable: true ,
        cellRendererFramework: CheckBoxCellComponent},
        { headerName: this.l('From'), field: 'beginAcc', sortable: true, width: 120, editable: true, resizable: true },
        { headerName: this.l(''), field: 'addBeginAcc', width: 25, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        { headerName: this.l('To'), field: 'endAcc', sortable: true, filter: true, width: 100, editable: true, resizable: true },
        { headerName: this.l(''), field: 'addEndAcc', width: 25, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false }
    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];
        if (this.isUpdate) {
            this.rowData = this.glSecurityHeader.glSecurityDetail;
        }
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onAddRow(): void {
        debugger;
        var index = this.gridApi.getDisplayedRowCount();
        var newItem = this.createNewRowData();
        this.gridApi.updateRowData({ add: [newItem] });
        this.gridApi.refreshCells();
        //this.onBtStartEditing(index, "addEmployeeId");
    }

    onCellClicked(params) {
        debugger;
        if (params.column["colId"] == "addBeginAcc") {
            this.target="BeginAcc";
            this.setParms = params;
            this.openChartOfAccountModal();
        }
        if (params.column["colId"] == "addEndAcc") {
            this.target="EndAcc";
            this.setParms = params;
            this.openChartOfAccountModal();
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
            canSee: "",
            beginAcc: "",
            endAcc: "",
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

        this.message.confirm(
            'Save GL Security',
            (isConfirmed) => {
                if (isConfirmed) {
                    debugger;
                    var count = this.gridApi.getDisplayedRowCount();

                    if (count == 0) {
                        this.notify.error(this.l('Please Enter Grid Data'));
                        return;
                    }

                    this.gridApi.forEachNode(node => {
                        if (node.data.beginAcc == "" || node.data.endAcc == "") {
                            this.notify.error(this.l('Please Enter From and To Details'));
                            this.check = true;
                        }
                        else {
                            this.check = false;
                        }
                    });
                    if (this.check) {
                        return;
                    }

                    this.saving = true;

                    var rowData = [];
                    this.gridApi.forEachNode(node => {
                        if(node.data.canSee=="")
                        {
                            node.data.canSee=false;
                        }
                        rowData.push(node.data);
                    });

                    this.glSecurityHeader.audtDate = moment();
                    this.glSecurityHeader.audtUser = this.appSession.user.userName;
                    
                    if (!this.glSecurityHeader.id) {
                        this.glSecurityHeader.createdDate = moment();
                        this.glSecurityHeader.createdBy = this.appSession.user.userName;
                    }
                     this.glSecurityHeader.glSecurityDetail = rowData;
                     this.glSecurityHeader.glSecurityDetail.forEach(element => {
                         debugger;
                        element.userID = this.glSecurityHeader.userID;
                        element.audtDate=this.glSecurityHeader.audtDate;
                        element.audtUser=this.glSecurityHeader.audtUser;
                    });
                    this._gLSecurityHeaderService.createOrEditGLSecurityHeader(this.glSecurityHeader)
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

    getUserName()
    {
        debugger;
        this.glSecurityHeader.userID=this.glSecurityHeader.userID.trim();
        if(this.glSecurityHeader.userID!="" && this.glSecurityHeader.userID!=null)
        {
            this._gLSecurityHeaderService.getUserInfo(this.glSecurityHeader.userID).subscribe(result=>{
                    debugger;
                    this.glSecurityHeader.userName=result;
            });
        }
        
    }

    processGLSecurity():void{
        debugger;
        this.processing = true;

        this._gLSecurityDetailService.processGLSecurity(this.glSecurityHeader.userID, this.glSecurityHeader.id).subscribe(result => {
            debugger
            if (result == "done") {
                this.processing = false;
                this.notify.info(this.l('Processed Successfully'));
            } else {
                this.processing = false;
                this.notify.error(this.l('Process Failed'));
            }
        });
    }
    getNewFinanceModal() {
        switch (this.target) {
            case "BeginAcc":
                this.getNewChartOfAccount();
                break;
            case "EndAcc":
                this.getNewChartOfAccount();
                break;
            default:
                break;
        }
    }
    /////////////////////////////////////////////////////////ChartOfAccount/////////////////////////////////////////////////////
    openChartOfAccountModal() {
        debugger;
        if(this.target=="BeginAcc")
        {
            this.FinanceLookupTableModal.id = this.setParms.data.beginAcc;
            this.FinanceLookupTableModal.show("ChartOfAccount");
        }
        else if(this.target=="EndAcc")
        {
            this.FinanceLookupTableModal.id = this.setParms.data.endAcc;
            this.FinanceLookupTableModal.show("ChartOfAccount");
        }
    }

    setChartOfAccountNull() {
        if(this.target=="BeginAcc")
        {
            this.setParms.data.beginAcc = null;
        }
        else if(this.target=="EndAcc")
        {
            this.setParms.data.endAcc = null;
        }
    }


    getNewChartOfAccount() {
        debugger;
        if(this.target=="BeginAcc")
        {
            this.setParms.data.beginAcc = this.FinanceLookupTableModal.id;
            this.gridApi.refreshCells();
        }
        else if(this.target=="EndAcc")
        {
            this.setParms.data.endAcc = this.FinanceLookupTableModal.id;
            this.gridApi.refreshCells();
        }
    }
    /////////////////////////////////////////////////////////ChartOfAccount/////////////////////////////////////////////////////
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
