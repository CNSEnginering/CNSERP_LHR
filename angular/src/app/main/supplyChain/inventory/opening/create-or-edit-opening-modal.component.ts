import { Component, ViewChild, Injector, Output, EventEmitter, ɵpublishDefaultGlobalUtils } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditICOPNHeaderDto } from '../shared/dto/icopnHeader-dto';
import { CreateOrEditICOPNDetailDto } from '../shared/dto/icopnDetails-dto';
import { OpeningDto } from '../shared/dto/opening-dto';
import { ICOPNHeadersService } from '../shared/services/icopnHeader.service';
import { ICOPNDetailsService } from '../shared/services/icopnDetail.service';
import { OpeningServiceProxy } from '../shared/services/opening.service';
import { GetDataService } from '../shared/services/get-data.service';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
import { AppConsts } from '@shared/AppConsts';
import { Lightbox } from 'ngx-lightbox';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';
import { LogComponent } from '@app/finders/log/log.component';

@Component({
    selector: 'createOrEditOpeningModal',
    templateUrl: './create-or-edit-opening-modal.component.html'
})
export class CreateOrEditOpeningModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    description = '';
    private setParms;
    locations: any;
    LockDocDate: Date;

    icopnHeader: CreateOrEditICOPNHeaderDto = new CreateOrEditICOPNHeaderDto();
    icopnDetail: CreateOrEditICOPNDetailDto = new CreateOrEditICOPNDetailDto();
    opening: OpeningDto = new OpeningDto();

    agGridExtend: AgGridExtend = new AgGridExtend();

    auditTime: Date;
    docDate: Date;
    processing = false;
    target: any;
    checkedval:boolean;
    url: string;
    uploadUrl: string;
    uploadedFiles: any[] = [];
    checkImage: boolean = false;
    image: any[] = [];
    allowLocIDbit:boolean;
    readonly openingAppId = 7;
    readonly appName = "Opening";


    constructor(
        injector: Injector,
        private _icopnHeadersServiceProxy: ICOPNHeadersService,
        private _icopnDetailServiceProxy: ICOPNDetailsService,
        private _openingServiceProxy: OpeningServiceProxy,
        private _approvelService: ApprovalService,
        private _getDataService: GetDataService,
        private _lightbox: Lightbox,
    ) {
        super(injector);
    }
    SetDefaultRecord(result:any){
        console.log(result);
        debugger
          this.icopnHeader.locID=result.currentLocID;
          this.checkedval=result.cDateOnly;
          if(result.allowLocID==true){
            this.allowLocIDbit=false;
          }else{
            this.allowLocIDbit=true;
          }
        
         // this.locDesc=result.currentLocName;
        //  this.oesaleHeader.typeID=result.transType;
         // this.typeDesc=result.transTypeName;
      }
    show(icopnHeaderId?: number, maxId?: number): void {

        this.auditTime = null;
        this.image = [];
        this.uploadedFiles = [];
        this.uploadUrl = null;
        this.checkImage = true;
        this.url = null;
        debugger;

        this.getLocations("ICLocations");

        if (!icopnHeaderId) {
            this.icopnHeader = new CreateOrEditICOPNHeaderDto();
            this.icopnHeader.id = icopnHeaderId;
            this.icopnHeader.docDate = moment().endOf('day');
            this.icopnHeader.docNo = maxId;
            this.icopnHeader.locID = 0;
            this.icopnHeader.totalItems = 0;
            this.icopnHeader.totalQty = 0;
            this.icopnHeader.totalAmt = 0;

            this.active = true;
            this.modal.show();
        } else {
            this._icopnHeadersServiceProxy.getICOPNHeaderForEdit(icopnHeaderId).subscribe(result => {
                this.icopnHeader = result;
                debugger;

                this._icopnHeadersServiceProxy.getImage(this.openingAppId, result.docNo).subscribe(fileResult => {
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


                if (this.icopnHeader.audtDate) {
                    this.auditTime = this.icopnHeader.audtDate.toDate();
                }

                this.LockDocDate = this.icopnHeader.docDate.toDate();

                this._icopnDetailServiceProxy.getICOPNDData(icopnHeaderId).subscribe(resultD => {
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

                    this.icopnHeader.totalQty = qty;
                    this.icopnHeader.totalAmt = amount;
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
        { headerName: this.l('SrNo'), field: 'srNo', sortable: true, width: 50, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('ItemId'), field: 'itemID', sortable: true, filter: true, width: 100, editable: false, resizable: true },
        { headerName: this.l(''), field: 'addItemId', width: 15, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        { headerName: this.l('Description'), field: 'itemDesc', sortable: true, filter: true, width: 200, resizable: true },
        { headerName: this.l('UOM'), field: 'unit', sortable: true, filter: true, width: 80, editable: false, resizable: true },
        // { headerName: this.l(''), field: 'addUOM', width: 15, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        { headerName: this.l('Conversion'), field: 'conver', sortable: true, filter: true, width: 150, resizable: true },
        { headerName: this.l('Qty'), field: 'qty', editable: true, sortable: true, filter: true, width: 100, type: "numericColumn", valueFormatter: this.agGridExtend.formatNumber, resizable: true },
        { headerName: this.l('Rate'), field: 'rate', editable: true, sortable: true, filter: true, width: 100, type: "numericColumn", valueFormatter: this.agGridExtend.formatNumber, resizable: true },
        { headerName: this.l('Amount'), field: 'amount', sortable: true, width: 100, editable: false, type: "numericColumn", valueFormatter: this.agGridExtend.formatNumber, resizable: true },
        { headerName: this.l('Remarks'), field: 'remarks', editable: true, resizable: true }
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
        var index = this.gridApi.getDisplayedRowCount();
        var newItem = this.createNewRowData();
        this.gridApi.updateRowData({ add: [newItem] });
        this.calculations();
        this.gridApi.refreshCells();
        this.onBtStartEditing(index, "addItemId");
    }

    cellClicked(params) {
        debugger;
        if (params.column["colId"] == "addItemId") {
            this.setParms = params;
            this.openSelectItemModal();
        }
        if (params.column["colId"] == "addUOM") {
            this.setParms = params;
            this.openSelectICUOMModal();
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
            rate: '0',
            amount: '0',
            remarks: this.icopnHeader.narration
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
            if ((node.data.amount != "" || node.data.qty != "") && node.data.itemID != "") {
                qty += parseFloat(node.data.qty);
                amount += parseFloat(node.data.amount);
            }
            items = items + 1;
        });
        this.icopnHeader.totalItems = items;
        this.icopnHeader.totalQty = qty;
        this.icopnHeader.totalAmt = amount;
    }

    onCellValueChanged(params) {
        debugger;
        if (params.data.qty != null && params.data.rate != null) {
            params.data.amount = parseFloat(params.data.qty) * parseFloat(params.data.rate);
        }
        this.calculations();
        this.gridApi.refreshCells();
    }

    //==================================Grid=================================

    save(): void {
        debugger;
        this.message.confirm(
            'Save Opening',
            (isConfirmed) => {
                if (isConfirmed) {

                    if (moment(this.icopnHeader.docDate) > moment().endOf('day')) {
                        this.message.warn("Document date greater than current date", "Document Date Greater");
                        return;
                    }

                    if ((moment(this.LockDocDate).month() + 1) != (moment(this.icopnHeader.docDate).month() + 1) && this.icopnHeader.id != null) {
                        this.message.warn('Document month not changeable', "Document Month Error");
                        return;
                    }

                    if (this.icopnHeader.locID == null || this.icopnHeader.locID == 0) {
                        this.message.warn("Please select location", "Location Required");
                        return;
                    }

                    if (this.gridApi.getDisplayedRowCount() <= 0) {
                        this.message.warn("No item detail found", "Items Details Required");
                        return;
                    }

                    if (this.icopnHeader.totalQty == 0) {
                        this.message.warn("Qty not greater than zero", "Qty Zero");
                        return;
                    }
                    var checkrate="";
                    this.gridApi.forEachNode(node => {
                        if(node.data.rate=="0"){
                        this.message.warn("Rate Should Not Be Zero.", "Qty Zero");
                        checkrate="1";
                         return;
                        }
                    });

                    this.saving = true;

                    var rowData = [];
                    this.gridApi.forEachNode(node => {
                        rowData.push(node.data);
                    });



                    if (moment(new Date()).format("A") === "AM" && !this.icopnHeader.id && (moment(new Date()).month() + 1) == (moment(this.icopnHeader.docDate).month() + 1)) {
                        this.icopnHeader.docDate = moment(this.icopnHeader.docDate);
                    } else {
                        this.icopnHeader.docDate = moment(this.icopnHeader.docDate).endOf('day');
                    }

                    this.icopnHeader.active = true;

                    // this.icopnHeader.audtDate = moment();
                    // this.icopnHeader.audtUser = this.appSession.user.userName;

                    // if (!this.icopnHeader.id) {
                    //     this.icopnHeader.createDate = moment();
                    //     this.icopnHeader.createdBy = this.appSession.user.userName;
                    // }

                    this.opening.icopnDetail = rowData;
                    this.opening.icopnHeader = this.icopnHeader;
                     if( checkrate==""){
                        this._openingServiceProxy.createOrEditOpening(this.opening)
                        .pipe(finalize(() => { this.saving = false; }))
                        .subscribe(() => {
                            this.notify.info(this.l('SavedSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        });
                     }else{
                        this.saving = false;
                     }
                    

                }
            }
        );

    }
    
    OpenLog(){
        debugger
       this.LogTableModal.show(this.icopnHeader.docNo,'Opening');
    } 

    approveDoc(id: number, mode, approve) {
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("opening", [id], mode, approve)
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
    postDoc(id: number, mode, posting){
        debugger
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    debugger
                    this._approvelService.ApprovalData("opening", [id], mode, posting)
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
        this.InventoryLookupTableModal.show(this.target);
    }


    setItemIdNull() {
        this.setParms.data.itemID = null;
        this.setParms.data.itemDesc = '';
        this.setParms.data.unit = '';
        this.setParms.data.conver = '';
    }


    getNewItemId() {
        debugger;
        var ConStatus = false;
        this.gridApi.forEachNode(node => {
            debugger
            if (node.data.itemID != '' && node.data.itemID != null) {
                if (node.data.itemID == this.InventoryLookupTableModal.id) {
                    this.message.warn("Item Has Already Exist At Row No. " + Number(node.rowIndex + 1), "Item Duplicate!");
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
            this.gridApi.refreshCells();
            this.onBtStartEditing(this.setParms.rowIndex, "type");
        }
    }
    // getNewItemId() {
    //     debugger;
    //     this.setParms.data.itemID = this.InventoryLookupTableModal.id;
    //     this.setParms.data.itemDesc = this.InventoryLookupTableModal.displayName;
    //     this.setParms.data.unit = this.InventoryLookupTableModal.unit;
    //     this.setParms.data.conver = this.InventoryLookupTableModal.conver;
    //     this.gridApi.refreshCells();
    //     this.onBtStartEditing(this.setParms.rowIndex, "qty");
    // }
    //================Item Model===============

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

    //===========================File Attachment=============================
    onBeforeUpload(event): void {
        debugger;
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles?';
        if (this.openingAppId !== undefined)
            this.uploadUrl += "APPID=" + encodeURIComponent("" + this.openingAppId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl += "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.icopnHeader.docNo !== undefined)
            this.uploadUrl += "DocID=" + encodeURIComponent("" + this.icopnHeader.docNo) + "&";
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
