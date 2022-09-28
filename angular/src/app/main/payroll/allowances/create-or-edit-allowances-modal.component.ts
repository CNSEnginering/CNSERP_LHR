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
import { CreateOrEditAllowancesDto } from "../shared/dto/allowances-dto";
import { AllowancesServiceProxy } from "../shared/services/allowances.service";
import { PayRollLookupTableModalComponent } from "@app/finders/payRoll/payRoll-lookup-table-modal.component";
import { AllowanceSetupServiceProxy } from "../shared/services/allowanceSetup.service";
import { AllowanceSetupDto } from "../shared/dto/allowanceSetup-dto";

@Component({
    selector: "createOrEditAllowancesModal",
    templateUrl: "./create-or-edit-allowances-modal.component.html",
})
export class CreateOrEditAllowancesModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("PayRollLookupTableModal", { static: true })
    PayRollLookupTableModal: PayRollLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    validchek:boolean=false;
    active = false;
    saving = false;
    processing = false;

    allowances: CreateOrEditAllowancesDto = new CreateOrEditAllowancesDto();
    monthlyFuelAllowanceRate: AllowanceSetupDto = new AllowanceSetupDto();

    docYear: string;
    docMonth: string;
    docDate: Date;

    target: string;

    constructor(
        injector: Injector,
        private _allowancesServiceProxy: AllowancesServiceProxy,
        private _allowanceSetupServiceProxy: AllowanceSetupServiceProxy
    ) {
        super(injector);
    }
    monthbtn:boolean=false;
    show(flag: boolean | undefined, allowancesId?: number): void {
        this._allowanceSetupServiceProxy
            .getLatestAllowanceData()
            .subscribe((result) => {
                this.monthlyFuelAllowanceRate = result;
            });
            debugger
        if (!flag) {
            this.allowances = new CreateOrEditAllowancesDto();
            this.allowances.id = allowancesId;
            this.allowances.flag = flag;

            this.docDate = new Date();
            this.docYear = String(this.docDate.getFullYear());
            this.docMonth = ("0" + String(this.docDate.getMonth() + 1)).slice(
                -2
            );

            this._allowancesServiceProxy.getMaxtID().subscribe((result) => {
                this.allowances.docID = result;
            });
           
            this.allowances.docMonth=parseInt(this.docMonth);
            this.allowances.docYear=parseInt(this.docYear);
            this.DuplicateMonth();
            this.active = true;
            this.modal.show();
        } else {
            this._allowancesServiceProxy
                .getAllowancesForEdit(allowancesId)
                .subscribe((result) => {
                    debugger;
                    this.allowances = result.allowances;

                    this.rowData = [];
                    this.rowData = result.allowances.allowancesDetail;
                    this.allowances.flag = true;

                    this.docDate = moment(this.allowances.docdate).toDate();
                    this.docYear = String(this.docDate.getFullYear());
                    this.docMonth = (
                        "0" + String(this.docDate.getMonth() + 1)
                    ).slice(-2);

                    this.active = true;
                    this.modal.show();
                });
        }
    }
   DuplicateMonth(){
    this._allowancesServiceProxy.getDataForMonth(this.docMonth,this.docYear).subscribe((result) => {
       debugger;
       var res= result;
       if(result["result"]==true){
        this.monthbtn=true;
       }else{
        this.monthbtn=false;
       }
        
        
        });
   }
    MilageCheck(){
       
        this.gridApi.forEachNode((node) => {
           
                if(node.data.allowanceType ==2)
                {
                 
                    if(node.data.milage==undefined || node.data.milage==null ){
                        this.notify.error("Plz Enter Milage"); 
                        this.validchek=true;
                     
                        return;

                    }else{
                        this.validchek=false;
                    }
                }

                
        });
    }

    save(): void {
        this.MilageCheck();
        debugger
        //this.saving = true;
    if(this.validchek==false){

debugger
    var count = this.gridApi.getDisplayedRowCount();
    if (count == 0) {
        this.notify.error(this.l("Please Process to Enter Grid Data"));
        return;
    }

    this.saving = true;
    var amtstatus=false;
    var rowData = [];
    this.gridApi.forEachNode((node) => {
        debugger
        
            if (node.data.allowanceType == "Car") {
                node.data.allowanceType = 1;
            }
            else if(node.data.allowanceType =="Motorcycle")
            {
                node.data.allowanceType = 2;
               
            }
            if(node.data.amount == 0)
            {
            debugger
            this.notify.error(this.l("Amount should be greater than 0!"));
            amtstatus=true;
            this.saving = false;
            return; 
            }
            rowData.push(node.data);
    });

    this.allowances.docdate = moment(this.docDate);
    this.allowances.docMonth = Number(this.docMonth);
    this.allowances.docYear = Number(this.docYear);

    this.allowances.audtDate = moment();
    this.allowances.audtUser = this.appSession.user.userName;

    if (!this.allowances.id) {
        this.allowances.createDate = moment();
        this.allowances.createdBy = this.appSession.user.userName;
    }

    this.allowances.allowancesDetail = rowData;

  //  this.allowances.allowancesDetail = rowData;
    this.allowances.allowancesDetail.forEach((element) => {
        element.audtDate = moment();
        element.audtUser = this.appSession.user.userName;
    });
    if(amtstatus==false){
        this._allowancesServiceProxy
        .createOrEdit(this.allowances)
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
    
 
      }
       
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    onChange(newvalue) {
        this.docYear = String(newvalue.getFullYear());
        this.docMonth = ("0" + String(newvalue.getMonth() + 1)).slice(-2);
        this.DuplicateMonth();
    }

    //==================================Grid=================================
    public gridApi;
    public gridColumnApi;
    public rowData;
    public rowSelection;
    private setParms;

    columnDefs = [
        {
            headerName: this.l("SrNo"),
            field: "srNo",
            sortable: true,
            width: 60,
            valueGetter: "node.rowIndex+1",
        },
        {
            headerName: this.l("EmployeeID"),
            field: "employeeID",
            editable: false,
            sortable: true,
            width: 120,
            resizable: true,
        },
        //{ headerName: this.l(''), field: 'addEmployeeId', width: 25, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        {
            headerName: this.l("EmployeeName"),
            field: "employeeName",
            sortable: true,
            filter: true,
            width: 120,
            editable: false,
            resizable: true,
        },{
            headerName: this.l("AllowanceType"),
            field: "allowanceType",
            sortable: true,
            filter: true,
            width: 100,
            editable: false,
            resizable: true,
            hide: true,
        },
        {
            headerName: this.l("AllowanceType Name"),
            field: "allowanceTypeName",
            sortable: true,
            filter: true,
            width: 100,
            editable: false,
            resizable: true,
        },
        {
            headerName: this.l("Per Milage Rate"),
            field: "perlitrMilg",
            editable: false,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        
        {
            headerName: this.l("Repair Rate"),
            field: "repairRate",
            editable: false,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        {
            headerName: this.l("Milage"),
            field: "milage",
            editable: true,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        {
            headerName: this.l("Parking Fees"),
            field: "parkingFees",
            editable: true,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        {
            headerName: this.l("WorkedDays"),
            field: "workedDays",
            editable: false,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        {
            headerName: this.l("AllowanceAmt"),
            field: "allowanceAmt",
            editable: false,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        {
            headerName: this.l("AllowanceQty"),
            field: "allowanceQty",
            editable: false,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        {
            headerName: this.l("Amount"),
            field: "amount",
            editable: false,
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];

        if (this.allowances.flag) {
            this.rowData = this.allowances.allowancesDetail;
        }
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;

        this.gridApi.forEachNode((node) => {
            debugger;
            if (node.data.allowanceType == 1) {
                node.data.allowanceType = "Car";
            }
            else if(node.data.allowanceType == 2)
            {
                node.data.allowanceType = "Motorcycle";
            }
        });

        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onAddRow(type: string): void {
        debugger;
        var index = this.gridApi.getDisplayedRowCount();
        if (type == "Add") {
            var newItem = this.createNewRowData();
            this.gridApi.updateRowData({ add: [newItem] });
        } else 
        if (type == "Process") {
            this.processing = true;
            debugger;
            this._allowancesServiceProxy
                .getCarAllowanceData(this.docMonth,this.docYear)
                .subscribe((result) => {
                    debugger;
                    this.rowData = [];
                    this.rowData = result.items;

                    // this.gridApi.forEachNode(node => {
                    //     debugger;
                    //     if (node.data.allowanceType == 1) {
                    //         node.data.allowanceType = "Car"
                    //     }
                    // });

                    this.processing = false;
                });
        }
        this.gridApi.refreshCells();
        //this.onBtStartEditing(index, "addEmployeeId");
    }

    onCellClicked(params) {
        debugger;
        // if (params.column["colId"] == "addEmployeeId") {
        //     this.setParms = params;
        //     this.openEmployeeModal();
        // }
        if (
            params.column["colId"] == "milage" &&
            params.data.allowanceType == 1
        ) {
            this.gridApi.stopEditing();
        }
    }

    addIconCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }

    onBtStartEditing(index, col) {
        debugger;
        this.gridApi.setFocusedCell(index, col);
        this.gridApi.isNodeSelected(true);
        //node.setSelected(true);
        this.gridApi.startEditingCell({
            rowIndex: index,
            colKey: col,
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
            allowanceType: "",
            workedDays: "",
            allowanceAmt: "",
            allowanceQty: "",
            amount: "",
        };
        return newData;
    }

    onCellValueChanged(params) {
        debugger;
        if (
            params.column["colId"] == "milage" &&
            params.data.allowanceType != 1
        ) {
            this.calculations(params);
        }
        // else if (params.data.allowanceType == 1) {
        //     this.onBtStartEditing(params.rowIndex + 1, "milage");
        // }

        this.gridApi.refreshCells();
    }
    calculations(params) {
        debugger;
        var kilometres =params.data.milage;
        params.data.amount = Math.round(
            kilometres * params.data.perlitrMilg +
                kilometres * params.data.repairRate
        );
        // this.gridApi.forEachNode(node => {
        //     debugger;
        //     node.data.amount = node.data.milage;
        // });

        //this.onBtStartEditing(params.rowIndex + 1, "milage");
    }

    onCellEditingStarted(params) {
        debugger;
        if (
            params.column["colId"] == "milage" &&
            params.data.allowanceType == 1
        ) {
            this.gridApi.stopEditing();
        }
    }

    //==================================Grid=================================

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

    getNewPayRollModal() {
        switch (this.target) {
            case "Employee":
                this.getNewEmployee();
                break;
            default:
                break;
        }
    }
    /////////////////////////////////////////////////////////Employee/////////////////////////////////////////////////////
}
