import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    ɵpublishDefaultGlobalUtils
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { GetDataService } from "../shared/services/get-data.service";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { ApprovalService } from "../../periodics/shared/services/approval-service.";
import { CreateOrEditICWOHeaderDto } from "../shared/dto/icwoHeader-dto";
import { CreateOrEditICWODetailDto } from "../shared/dto/icwoDetails-dto";
import { WorkOrderDto } from "../shared/dto/workOrder-dto";
import { ICWOHeadersService } from "../shared/services/icwoHeader.service";
import { ICWODetailsService } from "../shared/services/icwoDetail.service";
import { WorkOrderServiceProxy } from "../shared/services/workOrder.service";
import { LogComponent } from "@app/finders/log/log.component";

@Component({
    selector: "createOrEditWorkOrderModal",
    templateUrl: "./create-or-edit-workOrder-modal.component.html"
})
export class CreateOrEditWorkOrderModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("InventoryLookupTableModal", { static: true })
    InventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    approving;

    errorFlag: boolean;

    description = "";
    private setParms;
    locations: any;
    LockDocDate: Date;
    ccDesc: string;

    icwoHeader: CreateOrEditICWOHeaderDto = new CreateOrEditICWOHeaderDto();
    icwoDetails: CreateOrEditICWODetailDto = new CreateOrEditICWODetailDto();
    workOrder: WorkOrderDto = new WorkOrderDto();

    auditTime: Date;
    docDate: Date;
    processing = false;
    target: any;
    locDesc: string;
    totalItems: number;
    CheckDatVal:boolean;
    LocCheckVal:boolean;
    constructor(
        injector: Injector,
        private _icwoHeadersServiceProxy: ICWOHeadersService,
        private _icwoDetailsServiceProxy: ICWODetailsService,
        private _workOrderServiceProxy: WorkOrderServiceProxy,
        private _approvelService: ApprovalService,
        private _getDataService: GetDataService
    ) {
        super(injector);
    }

    SetDefaultRecord(result:any){
        console.log(result);
          this.icwoHeader.locID=result.currentLocID;
          //this.locDesc=result.currentLocName;
          this.CheckDatVal=result.cDateOnly;
          if(result.allowLocID==false){
              this.LocCheckVal=true;
          }else{
            this.LocCheckVal=false;
          }
          //this.typeDesc=result.transTypeName;
      }

      OpenLog(){
        debugger
       this.LogTableModal.show(this.icwoHeader.docNo,'WorkOrder');
    }
    show(icwoHeaderId?: number, maxId?: number): void {
        this.auditTime = null;
        debugger;

        this.getLocations("ICLocations");

        if (!icwoHeaderId) {
            this.icwoHeader = new CreateOrEditICWOHeaderDto();
            this.icwoHeader.id = icwoHeaderId;
            this.icwoHeader.docDate = moment().endOf("day");
            this.icwoHeader.docNo = maxId;
            this.icwoHeader.locID = 0;
            this.icwoHeader.totalQty = 0;
            this.icwoHeader.totalAmt = 0;

            this.active = true;
            this.modal.show();
        } else {
            this._icwoHeadersServiceProxy
                .getICWOHeaderForEdit(icwoHeaderId)
                .subscribe(result => {
                    debugger
                    this.icwoHeader = result;
                    debugger;
                    if (this.icwoHeader.audtDate) {
                        this.auditTime = this.icwoHeader.audtDate.toDate();
                    }

                    this.ccDesc = result.ccDesc;
                    this.LockDocDate = this.icwoHeader.docDate.toDate();
                    
                    this._icwoDetailsServiceProxy
                        .getICWODData(icwoHeaderId,this.icwoHeader.ccid)
                        .subscribe(resultD => {
                            debugger;
                            var rData = [];
                            var qty = 0;
                            var amount = 0;
                            var items = 0;
                            resultD["items"].forEach(element => {
                                rData.push(element);
                                qty += element.qty;
                                amount += element.amount;
                                items++;
                            });

                            this.rowData = [];
                            this.rowData = rData;

                            this.icwoHeader.totalQty = qty;
                            this.icwoHeader.totalAmt = amount;
                            this.totalItems = items;
                        });

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
        {
            headerName: this.l("SrNo"),
            field: "srNo",
            sortable: true,
            width: 50,
            valueGetter: "node.rowIndex+1"
        },
        {
            headerName: this.l("ItemId"),
            field: "itemID",
            sortable: true,
            filter: true,
            width: 100,
            editable: false,
            resizable: true
        },
        // {
        //     headerName: this.l(""),
        //     field: "addItemId",
        //     width: 15,
        //     editable: false,
        //     cellRenderer: this.addIconRendererFunc,
        //     resizable: false
        // },
        {
            headerName: this.l("Description"),
            field: "itemDesc",
            sortable: true,
            filter: true,
            width: 200,
            resizable: true
        },
        {
            headerName: this.l("UOM"),
            field: "unit",
            sortable: true,
            filter: true,
            width: 80,
            editable: false,
            resizable: true
        },
        {
            headerName: this.l(""),
            field: "addUOM",
            width: 15,
            editable: false,
            cellRenderer: this.addIconRendererFunc,
            resizable: false
        },
        {
            headerName: this.l("Conversion"),
            field: "conver",
            sortable: true,
            filter: true,
            width: 150,
            resizable: true
        },
        {
            headerName: this.l("Qty"),
            field: "qty",
            editable: true,
            sortable: true,
            filter: true,
            width: 100,
            type: "numericColumn",
            resizable: true
        },
        {
            headerName: this.l("Cost"),
            field: "cost",
            editable: false,
            sortable: true,
            filter: true,
            width: 100,
            type: "numericColumn",
            resizable: true
        },
        {
            headerName: this.l("Amount"),
            field: "amount",
            sortable: true,
            width: 100,
            editable: false,
            type: "numericColumn",
            resizable: true
        },
        { headerName: this.l('SubCost'), field: 'subCCID', editable: false, width: 100, resizable: true },
        { headerName: this.l(''), field: 'addSubCost', width: 15, editable: false, cellRenderer: this.addIconRendererFunc, resizable: false },
        { headerName: this.l('SubCostName'), field: 'subCCName', editable: false, resizable: true },
        {
            headerName: this.l("Remarks"),
            field: "remarks",
            editable: true,
            resizable: true
        }
    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onAddRow(): void {
        debugger;
        if (this.icwoHeader.ccid == null || this.icwoHeader.ccid == "") {
            this.notify.error(this.l("CostCenterNotEmpty"));
            return;
        }
        var index = this.gridApi.getDisplayedRowCount();
        var newItem = this.createNewRowData();
        this.gridApi.updateRowData({ add: [newItem] });
        this.calculations();
        this.gridApi.refreshCells();
        this.onBtStartEditing(index, "addItemId");
    }

    addIconRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }

    cellClicked(params) {
        debugger;
        switch (params.column["colId"]) {
            case "itemID":
                this.setParms = params;
                this.openSelectItemModal();
                break;
            case "addUOM":
                this.setParms = params;
                this.openSelectICUOMModal();
                break;
            case "addSubCost":
                this.setParms = params;
                this.openSelectSubCostModal();
                break;
            default:
                break;
        }
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
        this.calculations();
    }

    createNewRowData() {
        debugger;
        var newData = {
            itemID: "",
            itemDesc: "",
            unit: "",
            conver: "",
            qty: "0",
            cost: "0",
            amount: "0",
            remarks: this.icwoHeader.narration
        };
        return newData;
    }

    calculations() {
        debugger;
        var items = 0;
        var qty = 0;
        var amount = 0;
        this.gridApi.forEachNode(node => {
            debugger;
            if (
                (node.data.amount != "" || node.data.qty != "") &&
                node.data.itemID != ""
            ) {
                qty += parseFloat(node.data.qty);
                amount += parseFloat(node.data.amount);
            }
            items = items + 1;
        });
        this.totalItems = items;
        this.icwoHeader.totalQty = qty;
        this.icwoHeader.totalAmt = amount;
    }

    onCellValueChanged(params) {
        debugger;
        if (params.data.qty != null && params.data.cost != null) {
            params.data.amount =
                parseFloat(params.data.qty) * parseFloat(params.data.cost);
        }
        this.calculations();
        this.gridApi.refreshCells();
    }
    //==================================Grid=================================

    save(): void {
        debugger;
        this.message.confirm("Save Work Order", isConfirmed => {
            if (isConfirmed) {
                if (moment(this.icwoHeader.docDate) > moment().endOf("day")) {
                    this.message.warn(
                        "Document date greater than current date",
                        "Document Date Error"
                    );
                    return;
                }

                if (
                    moment(this.LockDocDate).month() + 1 !=
                        moment(this.icwoHeader.docDate).month() + 1 &&
                    this.icwoHeader.id != null
                ) {
                    this.message.warn(
                        "Document month not changeable",
                        "Document Month Error"
                    );
                    return;
                }

                if (
                    this.icwoHeader.locID == null ||
                    this.icwoHeader.locID == 0
                ) {
                    this.message.warn(
                        "Please select location",
                        "Location Required"
                    );
                    return;
                }

                if (this.gridApi.getDisplayedRowCount() <= 0) {
                    this.message.warn(
                        "No items details found",
                        "Items Details Required"
                    );
                    return;
                }
                this.gridApi.forEachNode(node=>{
                    debugger
                    if(node.data.subCCID==undefined || node.data.subCCID==null){
                        this.message.warn("Sub Cost not found at row "+ Number(node.rowIndex+1)," Required");
                        this.errorFlag=true;
                        return;
                    }
                    else{
                        this.errorFlag=false;
                    }
                });
                if(this.errorFlag){
                    return;
                }

                if (this.icwoHeader.totalQty == 0) {
                    this.message.warn("Qty not greater than zero", "Qty Zero");
                    return;
                }

                this.saving = true;

                var rowData = [];
                this.gridApi.forEachNode(node => {
                    rowData.push(node.data);
                });

                if (
                    moment(new Date()).format("A") === "AM" &&
                    !this.icwoHeader.id &&
                    moment(new Date()).month() + 1 ==
                        moment(this.icwoHeader.docDate).month() + 1
                ) {
                    this.icwoHeader.docDate = moment(this.icwoHeader.docDate);
                } else {
                    this.icwoHeader.docDate = moment(
                        this.icwoHeader.docDate
                    ).endOf("day");
                }

                this.icwoHeader.active = true;

                this.workOrder.icwoDetail = rowData;
                this.workOrder.icwoHeader = this.icwoHeader;

                this._workOrderServiceProxy
                    .createOrEditWorkOrder(this.workOrder)
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
        });
    }
//=====================Qutation Model================
openSelectQutationModal() {
    debugger;
    this.target = "Qutation";
    this.InventoryLookupTableModal.id = this.icwoHeader.qutationDoc;
    this.InventoryLookupTableModal.show(this.target);
}

setQutationNull() {
    this.icwoHeader.qutationDoc = "";
   
}

getNewQutation() {

    this.icwoHeader.qutationDoc = this.InventoryLookupTableModal.id;

    this._workOrderServiceProxy
    .getDataForWorkorder(this.icwoHeader.qutationDoc).subscribe(result => {
        var rData = [];
        var qty = 0;
        var amount = 0;
        var items = 0;
        debugger
        result["result"]["items"].forEach(element => {
            this._workOrderServiceProxy
            .getQtyForItem(element.itemID,this.icwoHeader.qutationDoc).subscribe(result => {
               element.qty =result["result"];
              debugger
               element.amount=parseFloat(result["result"])*parseFloat(element.cost);
               this.gridApi.refreshCells();
            })
            
            rData.push(element);
            //qty += element.qty;
           
            amount += element.amount;
            items++;
        });

        this.rowData = [];
        this.rowData = rData;

        this.icwoHeader.totalQty = qty;
        this.icwoHeader.totalAmt = amount;
        this.totalItems = items;
    });
 
}
    approveDoc(id: number, mode, approve) {
        debugger;
        this.message.confirm(
            approve ? "Approve Work Order" : "Unapprove Work Order",
            isConfirmed => {
                if (isConfirmed) {
                    this._approvelService
                        .ApprovalData("workOrder", [id], mode, approve)
                        .subscribe(() => {
                            if (approve == true) {
                                this.notify.success(
                                    this.l("SuccessfullyApproved")
                                );
                                this.close();
                                this.modalSave.emit(null);
                            } else {
                                this.notify.success(
                                    this.l("SuccessfullyUnApproved")
                                );
                                this.close();
                                this.modalSave.emit(null);
                            }
                        });
                }
            }
        );
    }
    postDoc(id: number, mode, posting){
        debugger
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    debugger
                    this._approvelService.ApprovalData("workOrder", [id], mode, posting)
                        .subscribe(() => {
                            if (posting == true) {
                                this.notify.success(this.l('SuccessfullyPosted'));
                                this.close();
                                this.modalSave.emit(null);
                            } 
                        });
                }
            }
        );
          
    }

    // processWorkOrder():void{
    //     debugger;
    //     this.message.confirm(
    //         'Process Work Order',
    //         (isConfirmed) => {
    //             if (isConfirmed) {
    //                 this.processing = true;
    //                 debugger;

    //                 if(moment(new Date()).format("A")==="AM" && !this.icwoHeader.id   && (moment(new Date()).month()+1)==(moment(this.icwoHeader.docDate).month()+1)){
    //                     this.icwoHeader.docDate =moment(this.icwoHeader.docDate);
    //                 }else{
    //                     this.icwoHeader.docDate =moment(this.icwoHeader.docDate).endOf('day');
    //                 }

    //             this._workOrderServiceProxy.processWorkOrder(this.icwoHeader).subscribe(result => {
    //                 debugger
    //                     if(result=="Save"){
    //                         this.saving = false;
    //                         this.notify.info(this.l('ProcessSuccessfully'));
    //                         this.close();
    //                         this.modalSave.emit(null);
    //                     }else if(result=="NoAccount"){
    //                         this.message.warn("Please check Cost Center or Inventory Account configuration","Account Required");
    //                         this.processing = false;
    //                     }else{
    //                         this.message.error("Input not valid","Verify Input");
    //                         this.processing = false;
    //                     }
    //                 });
    //             }
    //         }
    //     );
    // }

    getLocations(target: string): void {
        debugger;
        this._getDataService.getList(target).subscribe(result => {
            this.locations = result;
        });
    }
    getNewInventoryModal() {
        switch (this.target) {
            case "Items":
                this.getNewItemId();
                break;
            case "UOM":
                this.getNewICUOM();
                break;
            case "CostCenter":
                this.getNewCostCenter();
                break;
            case "SubCostCenter":
                this.getNewSubCost();
                break;
            case "Location":
                this.getNewLocation();
                break;
            case "Qutation":
                    this.getNewQutation();
                    break;
            default:
                break;
        }
    }

    //=====================Item Model================
    openSelectItemModal() {
        debugger;
        this.target = "Items";
        this.InventoryLookupTableModal.id = this.setParms.data.itemID;
        this.InventoryLookupTableModal.displayName = this.setParms.data.itemDesc;
        this.InventoryLookupTableModal.unit = this.setParms.data.unit;
        this.InventoryLookupTableModal.conver = this.setParms.data.conver;
        this.InventoryLookupTableModal.rate = this.setParms.data.cost;
        this.InventoryLookupTableModal.show(this.target);
    }

    setItemIdNull() {
        this.setParms.data.itemID = null;
        this.setParms.data.itemDesc = "";
        this.setParms.data.unit = "";
        this.setParms.data.conver = "";
        this.setParms.data.cost = 0;
    }

    getNewItemId() {
        debugger;
        this.setParms.data.itemID = this.InventoryLookupTableModal.id;
        this.setParms.data.itemDesc = this.InventoryLookupTableModal.displayName;
        this.setParms.data.unit = this.InventoryLookupTableModal.unit;
        this.setParms.data.conver = this.InventoryLookupTableModal.conver;

        this._workOrderServiceProxy
            .getItemCosting(
                moment(this.icwoHeader.docDate).format("MM/DD/YYYY"),
                this.setParms.data.itemID,
                this.icwoHeader.locID,
                this.icwoHeader.docNo
            )
            .subscribe(data => {
                this.setParms.data.cost = data["result"];
                this.gridApi.refreshCells();
                this.onBtStartEditing(this.setParms.rowIndex, "qty");
                this.gridApi.refreshCells();
            });
        this.gridApi.refreshCells();
    }
    //================Item Model===============

    //=====================Location Model================
    openSelectLocationModal() {
        this.target = "Location";
        this.InventoryLookupTableModal.id = String(this.icwoHeader.locID);
        this.InventoryLookupTableModal.displayName = this.locDesc;
        this.InventoryLookupTableModal.show(this.target);
    }
    getNewLocation() {
        debugger;
        this.icwoHeader.locID = Number(this.InventoryLookupTableModal.id);
        this.locDesc = this.InventoryLookupTableModal.displayName;
    }
    setLocationIDNull() {
        this.icwoHeader.locID = null;
        this.locDesc = "";
    }
    //=====================Location Model================

    //=====================ICUOM Model================
    openSelectICUOMModal() {
        debugger;
        this.target = "UOM";
        this.InventoryLookupTableModal.id = this.setParms.data.unit;
        this.InventoryLookupTableModal.conver = this.setParms.data.conver;
        this.InventoryLookupTableModal.show(this.target);
    }

    setICUOMIdNull() {
        this.setParms.data.unit = "";
        this.setParms.data.conver = "";
    }

    getNewICUOM() {
        debugger;
        this.setParms.data.unit = this.InventoryLookupTableModal.unit;
        this.setParms.data.conver = this.InventoryLookupTableModal.conver;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex, "qty");
    }
    //================ICUOM Model===============

    //=====================CostCenter Model================
    openSelectCostCenterModal() {
        debugger;
        this.target = "CostCenter";
        this.InventoryLookupTableModal.id = this.icwoHeader.ccid;
        this.InventoryLookupTableModal.displayName = this.ccDesc;
        this.InventoryLookupTableModal.show(this.target);
    }

    setCostCenterNull() {
        this.icwoHeader.ccid = "";
        this.ccDesc = "";
    }

    getNewCostCenter() {
        debugger;
        this.icwoHeader.ccid = this.InventoryLookupTableModal.id;
        this.ccDesc = this.InventoryLookupTableModal.displayName;
    }
    //================CostCenter Model===============

    //=====================SubCostCenter Model================
    openSelectSubCostModal() {
        debugger;
        this.target = "SubCostCenter";
        this.InventoryLookupTableModal.id = this.setParms.data.subCCID;
        this.InventoryLookupTableModal.displayName = this.setParms.data.subCCName;
        this.InventoryLookupTableModal.show(this.target, '',this.icwoHeader.ccid.trim());
    }


    setSubCostNull() {
        this.setParms.data.subCCID = null;
        this.setParms.data.subCCName = '';
    }


    getNewSubCost() {
        debugger;
        if (this.setParms.data.subCCID != "" && this.InventoryLookupTableModal.id == null) {
            this.onBtStartEditing(this.setParms.rowIndex, "remarks");
            return;
        }
        this.setParms.data.subCCID = this.InventoryLookupTableModal.id;
        this.setParms.data.subCCName = this.InventoryLookupTableModal.displayName;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex, "remarks");
    }
    //================SubCostCenter Model===============

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
