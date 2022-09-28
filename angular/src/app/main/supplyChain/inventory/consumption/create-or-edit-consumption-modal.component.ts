import { Component, ViewChild, Injector, Output, EventEmitter, ɵpublishDefaultGlobalUtils } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { GetDataService } from '../shared/services/get-data.service';
import { CreateOrEditICCNSHeaderDto } from '../shared/dto/iccnsHeader-dto';
import { CreateOrEditICCNSDetailDto } from '../shared/dto/iccnsDetails-dto';
import { ConsumptionDto } from '../shared/dto/consumption-dto';
import { ICCNSHeadersService } from '../shared/services/iccnsHeader.service';
import { ICCNSDetailsService } from '../shared/services/iccnsDetail.service';
import { ConsumptionServiceProxy } from '../shared/services/consumption.service';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
import { AppConsts } from '@shared/AppConsts';
import { Lightbox } from 'ngx-lightbox';
//import { LogComponent } from '@app/finders/log/log.component';
import * as _ from 'lodash';
import { throwError } from 'rxjs';
import { LogComponent } from '@app/finders/log/log.component';
@Component({
    selector: 'createOrEditConsumptionModal',
    templateUrl: './create-or-edit-consumption-modal.component.html'
})
export class CreateOrEditConsumptionModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    active = false;
    saving = false;

    description = '';
    private setParms;
    locations: any;
    LockDocDate: Date;
    ccDesc: string;

    url: string;
    uploadUrl: string;
    uploadedFiles: any[] = [];
    checkImage: boolean = false;
    image: any[] = [];
    checkedval: boolean;
    readonly consumptionAppId = 6;
    readonly appName = "Consumption";

    iccnsHeader: CreateOrEditICCNSHeaderDto = new CreateOrEditICCNSHeaderDto();
    icadjDetails: CreateOrEditICCNSDetailDto = new CreateOrEditICCNSDetailDto();
    consumption: ConsumptionDto = new ConsumptionDto();

    auditTime: Date;
    docDate: Date;
    processing = false;
    target: any;
    locDesc: string;
    errorFlag: boolean = false;
    LocCheckVal: boolean;

    constructor(
        injector: Injector,
        private _iccnsHeadersServiceProxy: ICCNSHeadersService,
        private _icadjDetailsServiceProxy: ICCNSDetailsService,
        private _consumptionServiceProxy: ConsumptionServiceProxy,
        private _approvelService: ApprovalService,
        private _lightbox: Lightbox,
        private _getDataService: GetDataService
    ) {
        super(injector);
    }
    OpenLog() {
        debugger
        this.LogTableModal.show(this.iccnsHeader.docNo,'Consumption');
    }

    show(iccnsHeaderId?: number, maxId?: number): void {

        this.auditTime = null;
        this.url = null;
        this.image = [];
        this.uploadedFiles = [];
        this.uploadUrl = null;
        this.checkImage = true;
        debugger;

        this.getLocations("ICLocations");

        if (!iccnsHeaderId) {
            this.iccnsHeader = new CreateOrEditICCNSHeaderDto();
            this.iccnsHeader.id = iccnsHeaderId;
            this.iccnsHeader.docDate = moment().endOf('day');
            this.iccnsHeader.docNo = maxId;
            this.iccnsHeader.locID = 0;
            this.iccnsHeader.totalQty = 0;
            this.iccnsHeader.type = Number('1');

            this.active = true;
            this.modal.show();
        } else {
            this._iccnsHeadersServiceProxy.getICCNSHeaderForEdit(iccnsHeaderId).subscribe(result => {
                this.iccnsHeader = result;
                debugger;

                this._iccnsHeadersServiceProxy.getImage(this.consumptionAppId, result.docNo).subscribe(fileResult => {
                    debugger;
                    if (fileResult != null) {
                        this.url = 'data:image/jpeg;base64,' + fileResult;
                        const album = {
                            src: this.url
                        };
                        this.image.push(album);
                        this.checkImage = false;
                    }
                });

                if (this.iccnsHeader.audtDate) {
                    this.auditTime = this.iccnsHeader.audtDate.toDate();
                }

                this.ccDesc = result.ccDesc;
                this.LockDocDate = this.iccnsHeader.docDate.toDate();

                this._icadjDetailsServiceProxy.getICCNSDData(iccnsHeaderId, result.ccid).subscribe(resultD => {
                    debugger;
                    var rData = [];
                    var qty = 0;
                    var amount = 0;
                    resultD["items"].forEach(element => {
                        rData.push(element);
                        qty += element.qty;
                        amount += element.amount;
                    });

                    this.rowData = [];
                    this.rowData = rData;

                    this.iccnsHeader.totalQty = qty;
                    this.iccnsHeader.totalAmt = amount;
                });

                this.active = true;
                this.modal.show();
            });
        }
    }
    SetDefaultRecord(result: any) {
        console.log(result);
        this.iccnsHeader.locID = result.currentLocID;
        //this.locDesc=result.currentLocName;
        this.checkedval = result.cDateOnly;
        if (result.allowLocID == false) {
            this.LocCheckVal = true;
        } else {
            this.LocCheckVal = false;
        }
        //this.typeDesc=result.transTypeName;
    }

    //==================================Grid=================================
    private gridApi;
    private gridColumnApi;
    private rowData;
    private rowSelection;
    columnDefs = [
        { headerName: this.l('SrNo'), field: 'srNo', sortable: true, width: 50, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('ItemId'), field: 'itemID', sortable: true, filter: true, width: 100, editable: false, resizable: true },
        { headerName: this.l(''), field: 'addItemId', width: 15, editable: false, cellRenderer: this.addIconRendererFunc, resizable: false },
        { headerName: this.l('Description'), field: 'itemDesc', sortable: true, filter: true, width: 200, resizable: true },
        { headerName: this.l('UOM'), field: 'unit', sortable: true, filter: true, width: 80, editable: false, resizable: true },
        // { headerName: this.l(''), field: 'addUOM', width: 15, editable: false, cellRenderer: this.addIconRendererFunc, resizable: false },
        { headerName: this.l('Conversion'), field: 'conver', sortable: true, filter: true, width: 150, resizable: true },
        { headerName: this.l('QtyInHand'), field: 'qtyInHand', editable: false, sortable: true, filter: true, width: 100, type: "numericColumn", resizable: true },
        { headerName: this.l('Work Qty'), field: 'Wqty', editable: false, sortable: true, filter: true, width: 100, type: "numericColumn", resizable: true },
        { headerName: this.l('Qty'), field: 'qty', editable: true, sortable: true, filter: true, width: 100, type: "numericColumn", resizable: true },
        { headerName: this.l('SubCost'), field: 'subCCID', editable: false, width: 100, resizable: true },
        { headerName: this.l(''), field: 'addSubCost', width: 15, editable: false, cellRenderer: this.addIconRendererFunc, resizable: false },
        { headerName: this.l('SubCostName'), field: 'subCCName', editable: false, resizable: true },
        { headerName: this.l('Remarks'), field: 'remarks', editable: true, resizable: true },
        { headerName: this.l('Cost'), field: 'cost', editable: true, hide: true, resizable: true },
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
        // if(this.iccnsHeader.ccid==null || this.iccnsHeader.ccid==""){
        //     this.message.warn(this.l('CostCenterNotEmpty'),"Cost Center Required");
        //     return;
        // }
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
            case "addItemId":
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
            qty: '0',
            subCCID: '',
            sccDesc: '',
            remarks: this.iccnsHeader.narration
        };
        return newData;
    }

    calculations() {
        debugger;
        var qty = 0;
        this.gridApi.forEachNode(node => {
            debugger;
            if ((node.data.amount != "" || node.data.qty != "") && node.data.itemID != "") {
                qty += parseFloat(node.data.qty);
            }
        });
        this.iccnsHeader.totalQty = Number(parseFloat(qty.toString()).toFixed(2));
    }

    onCellValueChanged(params) {
        debugger;
        // if (params.data.qty > 0 && params.data.qty != undefined) {
        //     if (params.data.qty > params.data.qtyInHand) {
        //         //this.notify.error('Qty Is Greater Than Work Order Qty!');
        //         this.message.warn("Qty not greater than Qty In Hand ", "Qty Greater");
        //         this.errorFlag = true;
        //         return;
        //     } else {
        //         this.errorFlag = false;
        //     }
        // }
        this.calculations();
        this.gridApi.refreshCells();
        this.CheckOnQtyInHand();


    }
    CheckOnQtyInHand(){
        
        this.gridApi.forEachNode(node => {
            debugger
       
            if(parseInt(node.data.qty)>parseInt(node.data.qtyInHand))
            {
                this.errorFlag=true;
                this.message.error("Qty Not Greater than Qty In Hand at Row No"+ Number(node.rowIndex+1),"Qty Greater");
                throw new Error();
            }else{
                if(parseInt(this.iccnsHeader.ordNo)>0){
                  if(parseInt(node.data.qty)>parseInt(node.data.Wqty)){
                    this.errorFlag=true;
                    this.message.error("Qty Not Greater than Work Qty at Row No"+ Number(node.rowIndex+1),"Qty Greater");
                    throw new Error();
                  }
                }else{

                    this.errorFlag=false;
                }
              
            }
        });
    }

    //==================================Grid=================================


    save(): void {
        this.CheckOnQtyInHand();
        debugger
        if (!this.errorFlag) {
            this.message.confirm(
                'Save Consumption',
                (isConfirmed) => {
                    if (isConfirmed) {

                        if (moment(this.iccnsHeader.docDate) > moment().endOf('day')) {
                            this.message.warn("Document date greater than current date", "Document Date Error");
                            throw new Error();
                        }

                        if ((moment(this.LockDocDate).month() + 1) != (moment(this.iccnsHeader.docDate).month() + 1) && this.iccnsHeader.id != null) {
                            this.message.warn('Document month not changeable', "Document Month Error");
                            throw new Error();
                        }

                        if (this.iccnsHeader.locID == null || this.iccnsHeader.locID == 0) {
                            this.message.warn("Please select location", "Location Required");
                            throw new Error();
                        }
                        if (this.iccnsHeader.ccid == null || this.iccnsHeader.ccid == "" || this.iccnsHeader.ccid == undefined) {
                            this.message.warn("Please select cost center", "Cost Center Required");
                            throw new Error();
                        }

                        if (this.gridApi.getDisplayedRowCount() <= 0) {
                            this.message.warn("No items details found", "Items Details Required");
                            throw new Error();
                        }

                        this.gridApi.forEachNode(node => {
                            debugger
                            //if(!this.errorFlag){
                            if (node.data.itemID == "") {
                                this.message.warn("Item not found at row " + Number(node.rowIndex + 1), "Item Required");
                                this.errorFlag = true;
                                throw new Error();
                            }
                            //  if(parseInt(node.data.qtyInHand)<parseInt( node.data.qty)){
                            //     this.message.warn("Qty not greater than Qty In Hand at row "+ Number(node.rowIndex+1),"Qty Greater");
                            //     this.errorFlag=true;
                            //     return;
                            // }
                            if (node.data.conver == 0 || node.data.conver == undefined || node.data.conver == '') {
                                this.message.warn("Conver Should Not Be Zero at row " + Number(node.rowIndex + 1), "Item Required");
                                this.errorFlag = true;
                                throw new Error();
                            }
                            if (node.data.Wqty < node.data.qty) {
                                this.message.warn("Qty not greater than Work Qty at row " + Number(node.rowIndex + 1), "Qty Greater");
                                this.errorFlag = true;
                                throw new Error();
                            }
                            if (node.data.subCCID=="") {
                                this.message.warn("SubCost Center Is Required at row " + Number(node.rowIndex + 1), "Required SubCost Center");
                                this.errorFlag = true;
                                throw new Error();
                            }  else {
                                this.errorFlag = false;
                            }
                            //  }
                        });

                        // if (this.errorFlag) {
                        //     this.message.warn("Qty not greater than Qty InHand", "Qty Greater");
                        //     return;
                        // }
                        debugger;
                        if (this.iccnsHeader.totalQty == 0) {
                            this.message.warn("Qty not greater than zero", "Qty Zero");
                            this.errorFlag = true;
                            throw new Error();
                        }
                        
                        this.saving = true;

                        var rowData = [];
                        this.gridApi.forEachNode(node => {
                            rowData.push(node.data);
                        });



                        if (moment(new Date()).format("A") === "AM" && !this.iccnsHeader.id && (moment(new Date()).month() + 1) == (moment(this.iccnsHeader.docDate).month() + 1)) {
                            this.iccnsHeader.docDate = moment(this.iccnsHeader.docDate);
                        } else {
                            this.iccnsHeader.docDate = moment(this.iccnsHeader.docDate).endOf('day');
                        }

                        this.iccnsHeader.active = true;

                        this.consumption.iccnsDetail = rowData;
                        this.consumption.iccnsHeader = this.iccnsHeader;
                         if(this.errorFlag==false){

                            this._consumptionServiceProxy.createOrEditConsumption(this.consumption)
                            // .pipe(finalize(() => { this.saving = false;}))
                            .subscribe(data => {
                                debugger
                                console.log(data);
                                this.notify.info(this.l('SavedSuccessfully'));
                                this.close();
                                this.modalSave.emit(null);
                            });
                         }else{
                            this.saving = false;
                            this.errorFlag = false;
                         }
                      

                    }
                }
            );
        } else {
            this.message.warn("Qty not greater than Qty in Hand ", "Qty Greater");
        }
    }


    approveDoc(id: number, mode, approve) {
        debugger;
        this.message.confirm(
            approve ? 'Approve Consumption' : 'Unapprove Consumption',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("consumption", [id], mode, approve)
                        .subscribe(() => {
                            if (approve == true) {
                                this.notify.success(this.l('SuccessfullyApproved'));
                                this.close();
                                this.modalSave.emit(null);
                            } else {
                                this.notify.success(this.l('SuccessfullyUnApproved'));
                                this.close();
                                this.modalSave.emit(null);
                            }
                        });
                }
            }
        );
    }

    getWorkOrderData(): void {
        debugger;
        if (this.iccnsHeader.ordNo == null || this.iccnsHeader.ordNo == "") {
            this.message.warn("Please enter Order No", "Order No Required");
            return;
        }
        if (this.iccnsHeader.locID == null || this.iccnsHeader.locID == 0) {
            this.message.warn("Please enter Select Location", "Location Required");
            return;
        }

        this._icadjDetailsServiceProxy.getICWODData(Number(this.iccnsHeader.ordNo), this.iccnsHeader.locID).subscribe(resultD => {
            debugger;
            var rData = [];
            var Wqty = 0;
            resultD["items"].forEach(element => {
                debugger
                if (element.Wqty > 0) {
                    element.qty = 0;
                    rData.push(element);
                    Wqty += element.Wqty;
                }

            });

            this.rowData = [];
            this.rowData = rData;

            // this.iccnsHeader.totalQty=qty;
        });
    }

    processConsumption(): void {
        debugger;
        this.message.confirm(
            'Process Consumption',
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing = true;
                    debugger;

                    if (moment(new Date()).format("A") === "AM" && !this.iccnsHeader.id && (moment(new Date()).month() + 1) == (moment(this.iccnsHeader.docDate).month() + 1)) {
                        this.iccnsHeader.docDate = moment(this.iccnsHeader.docDate);
                    } else {
                        this.iccnsHeader.docDate = moment(this.iccnsHeader.docDate).endOf('day');
                    }

                    this._consumptionServiceProxy.processConsumption(this.iccnsHeader).subscribe(result => {
                        debugger
                        if (result == "Save") {
                            this.saving = false;
                            this.notify.info(this.l('ProcessSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        } else if (result == "NoAccount") {
                            this.message.warn("Please check Cost Center or Inventory Account configuration", "Account Required");
                            this.processing = false;
                        } else {
                            this.message.error("Input not valid", "Verify Input");
                            this.processing = false;
                        }
                    });
                }
            }
        );
    }

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
            case "workOrder":
                this.getNewworkOrder();
                break;
            default:
                break;
        }
    }

    gridempty(){
        this.gridApi.setRowData([]);
    }
    //=====================workOrder Model================
    openSelectworkOrderModal() {
        debugger;
        this.target = "workOrder";
        this.InventoryLookupTableModal.id = this.iccnsHeader.ordNo;
        //this.InventoryLookupTableModal.displayName=this.ccDesc;
        this.InventoryLookupTableModal.show(this.target);
    }


    setworkOrderNull() {
        this.iccnsHeader.ordNo = "";
        this.iccnsHeader.ccid = "";
        this.iccnsHeader.locID = 0;
        this.gridempty();
    }


    getNewworkOrder() {
        debugger;
        this.iccnsHeader.ordNo = this.InventoryLookupTableModal.id;
        this.iccnsHeader.ccid = this.InventoryLookupTableModal.unit;
        this.ccDesc = this.InventoryLookupTableModal.manfDate.toString();
        this.iccnsHeader.locID = parseInt(this.InventoryLookupTableModal.option5);
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
        this.setParms.data.itemDesc = '';
        this.setParms.data.unit = '';
        this.setParms.data.conver = '';
        this.setParms.data.cost = '';
    }


    getNewItemId() {
        debugger;
        var ConStatus = false;
        this.gridApi.forEachNode(node => {
            debugger
            if (node.data.itemID != '' && node.data.itemID != null) {
                if (node.data.itemID == this.InventoryLookupTableModal.id) {
                    this.message.warn("Item Has Already Exist At Row No " + Number(node.rowIndex + 1), "Item Duplicate!");
                    ConStatus = true;
                    return;
                }
            }
        });

        if (ConStatus == false) {
            this.setParms.data.itemID = this.InventoryLookupTableModal.id;
            this.setParms.data.itemDesc = this.InventoryLookupTableModal.displayName;
            this.setParms.data.unit = this.InventoryLookupTableModal.unit;
            this.setParms.data.conver = this.InventoryLookupTableModal.conver;
            this.setParms.data.cost = this.InventoryLookupTableModal.rate;
            //this.setParms.data.qtyInHand=this.InventoryLookupTableModal.qty;
            this._consumptionServiceProxy.GetQtyInHand(this.iccnsHeader.locID, this.setParms.data.itemID, this.iccnsHeader.docDate.format("YYYY-MM-DD")).subscribe(
                res => {
                    debugger
                    this.setParms.data.qtyInHand = res["result"];
                    this.gridApi.refreshCells();
                    this.onBtStartEditing(this.setParms.rowIndex, "qty");
                }
            )
        }

    }
    //================Item Model===============

    //=====================Location Model================
    openSelectLocationModal() {
        this.target = "Location";
        this.InventoryLookupTableModal.id = String(this.iccnsHeader.locID);
        this.InventoryLookupTableModal.displayName = this.locDesc;
        this.InventoryLookupTableModal.show(this.target);
    }
    getNewLocation() {
        debugger;
        this.iccnsHeader.locID = Number(this.InventoryLookupTableModal.id);
        this.locDesc = this.InventoryLookupTableModal.displayName;
    }
    setLocationIDNull() {
        this.iccnsHeader.locID = null;
        this.locDesc = "";

    }
    //=====================Location Model================

    //=====================ICUOM Model================
    openSelectICUOMModal() {
        debugger;
        this.target = "UOM";
        this.InventoryLookupTableModal.id = this.setParms.data.unit;
        this.InventoryLookupTableModal.conver = this.setParms.data.conver
        this.InventoryLookupTableModal.show(this.target);
    }


    setICUOMIdNull() {
        this.setParms.data.unit = '';
        this.setParms.data.conver = '';
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
        this.InventoryLookupTableModal.id = this.iccnsHeader.ccid;
        this.InventoryLookupTableModal.displayName = this.ccDesc;
        this.InventoryLookupTableModal.show(this.target);
    }


    setCostCenterNull() {
        this.iccnsHeader.ccid = "";
        this.ccDesc = '';
    }


    getNewCostCenter() {
        debugger;
        this.iccnsHeader.ccid = this.InventoryLookupTableModal.id;
        this.ccDesc = this.InventoryLookupTableModal.displayName;
    }
    //================CostCenter Model===============

    //=====================SubCostCenter Model================
    openSelectSubCostModal() {
        debugger;
        this.target = "SubCostCenter";
        this.InventoryLookupTableModal.id = this.setParms.data.subCCID;
        this.InventoryLookupTableModal.displayName = this.setParms.data.subCCName;
        this.InventoryLookupTableModal.show(this.target,'', this.iccnsHeader.ccid.trim());
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

    //===========================File Attachment=============================
    onBeforeUpload(event): void {
        debugger;
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles?';
        if (this.consumptionAppId !== undefined)
            this.uploadUrl += "APPID=" + encodeURIComponent("" + this.consumptionAppId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl += "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.iccnsHeader.docNo !== undefined)
            this.uploadUrl += "DocID=" + encodeURIComponent("" + this.iccnsHeader.docNo) + "&";
        this.uploadUrl = this.uploadUrl.replace(/[?&]$/, "");
    }

    onUpload(event): void {
        this.checkImage = true;
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }
    }
    //===========================File Attachment=============================
    open(): void {
        debugger;
        this._lightbox.open(this.image);
    }

    close(): void {

        this.active = false;
        this.modal.hide();
        this._lightbox.close();
    }
}
