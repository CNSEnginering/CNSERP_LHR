import { Component, ViewChild, Injector, Output, EventEmitter, ɵpublishDefaultGlobalUtils } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { AdjustmentServiceProxy } from '../shared/services/adjustment.service';
import { ICADJHeadersService } from '../shared/services/icadjHeader.service';
import { ICADJDetailsService } from '../shared/services/icadjDetail.service';
import { CreateOrEditICADJHeaderDto } from '../shared/dto/icadjHeader-dto';
import { CreateOrEditICADJDetailDto } from '../shared/dto/icadjDetails-dto';
import { AdjustmentDto } from '../shared/dto/adjustment-dto';
import { GetDataService } from '../shared/services/get-data.service';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
import { AppConsts } from '@shared/AppConsts';
import { Lightbox } from 'ngx-lightbox';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';
import { LogComponent } from '@app/finders/log/log.component';

@Component({
    selector: 'createOrEditAdjustmentModal',
    templateUrl: './create-or-edit-adjustment-modal.component.html'
})
export class CreateOrEditAdjustmentModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
	processing = false;

    agGridExtend: AgGridExtend = new AgGridExtend();
    
    description = '';
    private setParms;
    locations: any;
	LockDocDate: Date;
	conumptionQty:number;
	consumptionMode = false;

    icadjHeader: CreateOrEditICADJHeaderDto = new CreateOrEditICADJHeaderDto();
    icadjDetails: CreateOrEditICADJDetailDto = new CreateOrEditICADJDetailDto();
    adjustment: AdjustmentDto = new AdjustmentDto();

	auditTime: Date;
	editMode:Boolean = false;
    docDate: Date;
    target: any;
    errorFlag = false;
    checkedval:boolean;
    url: string;
    uploadUrl: string;
    uploadedFiles: any[] = [];
    checkImage: boolean = false;
    image: any[] = [];
    LocCheckVal:boolean;

    readonly adjustmentAppId = 9;
    readonly appName = "Adjustment";


    constructor(
        injector: Injector,
        private _icadjHeadersServiceProxy: ICADJHeadersService,
        private _icadjDetailsServiceProxy: ICADJDetailsService,
        private _adjustmentServiceProxy: AdjustmentServiceProxy,
        private _approvelService: ApprovalService,
        private _lightbox: Lightbox,
        private _getDataService: GetDataService
    ) {
        super(injector);
    }
    SetDefaultRecord(result:any){
        console.log(result);
          this.icadjHeader.locID=result.currentLocID;
          //this.locDesc=result.currentLocName;
          this.checkedval=result.cDateOnly;
          if(result.allowLocID==false){
              this.LocCheckVal=true;
          }else{
            this.LocCheckVal=false;
          }
          //this.typeDesc=result.transTypeName;
      }
    show(icadjHeaderId?: number, maxId?: number): void {

        this.auditTime = null;
        this.url = null;

        this.image = [];
        this.uploadedFiles = [];
        this.uploadUrl=null;
        this.checkImage = true;
        debugger;        
		

        this.getLocations("ICLocations");

        if (!icadjHeaderId) {
            this.icadjHeader = new CreateOrEditICADJHeaderDto();
            this.icadjHeader.id = icadjHeaderId;
            this.icadjHeader.docDate = moment().endOf('day');
            this.icadjHeader.docNo = maxId;
            this.icadjHeader.locID = 0;
            this.icadjHeader.totalQty = 0;
			this.icadjHeader.totalAmt = 0;
			this.icadjHeader.type = 1;
			this.consumptionMode = false; 
			this.active = true;
			this.editMode = false;
            this.modal.show();
        } else {
            debugger
            this._icadjHeadersServiceProxy.getICADJHeaderForEdit(icadjHeaderId).subscribe(result => {
                this.icadjHeader = result;
               this.editMode = true;
               
               this._icadjHeadersServiceProxy.getImage(this.adjustmentAppId, result.docNo).subscribe(fileResult => {
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

				debugger;
                if (this.icadjHeader.audtDate) {
                    this.auditTime = this.icadjHeader.audtDate.toDate();
				}
				
				if(this.icadjHeader.type == 2){
					this.consumptionMode = true; 
				}
				else
				{
					this.consumptionMode = false;
				}

                this.LockDocDate = this.icadjHeader.docDate.toDate();

                this._icadjDetailsServiceProxy.getICADJDData(icadjHeaderId).subscribe(resultD => {
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

                    this.icadjHeader.totalQty = qty;
                    this.icadjHeader.totalAmt = amount;
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
        {
            headerName: this.l('Type'), field: 'type', sortable: true, filter: false, width: 100, resizable: true,
            cellEditor: 'agSelectCellEditor', editable: true,
            cellEditorParams: {
                values: ['Qty', 'Cost', 'Both']
            }
        },
        { headerName: this.l('Qty'), field: 'qty',valueFormatter: this.agGridExtend.formatNumber, editable: (params) => { return (params.data.type === 'Qty' || params.data.type === 'Both') ? true : false }, sortable: true, filter: true, width: 100, type: "numericColumn", resizable: true },
        { headerName: this.l('Cost'), field: 'cost', editable: (params) => { return (params.data.type === 'Cost' || (params.data.type === 'Both' && this.consumptionMode == false )) ? true : false }, sortable: true, filter: true, width: 100, type: "numericColumn", resizable: true },
        // {headerName: this.l('Amount'), field: 'amount',sortable: true,width:100,editable: (params)=>{return (params.data.type==='Qty')?true:false},type: "numericColumn",resizable: true},
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

    addIconCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }

    cellClicked(params) {
        debugger;
        if (params.column["colId"] == "addItemId") {
			if(this.consumptionMode == false)
			{
				this.setParms = params;
				this.openSelectItemModal();
			}       
        }
        if (params.column["colId"] == "addUOM") {
			if(this.consumptionMode == false)
			{
               this.setParms = params;
			   this.openSelectICUOMModal();
			}
		}
		if (params.column["colId"] == "qty") {
		   this._adjustmentServiceProxy.GetConsumptionQty(this.icadjHeader.conDocNo,
			params.data.itemID).subscribe(
				data=>{
					this.conumptionQty = data["result"];
					this.gridApi.refreshCells();
				}
			)
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
            type: "",
            qty: '0',
            cost: '0',
            amount: '0',
            remarks: this.icadjHeader.narration
        };
        return newData;
    }

    calculations() {
        debugger;
        var qty = 0;
        var amount = 0;
        this.gridApi.forEachNode(node => {
            debugger;
            if ((node.data.amount != "" || node.data.qty != "") && node.data.itemID != "") {
                qty += parseFloat(node.data.qty);
                amount += parseFloat(node.data.amount);
            }
        });
        this.icadjHeader.totalQty = qty;
        this.icadjHeader.totalAmt = amount;
    }
    OpenLog(){
        debugger
       this.LogTableModal.show(this.icadjHeader.docNo,'Adjustment');
    }
    onCellValueChanged(params) {
        debugger;
        if (params.column["colId"] == "type") {
			if(this.icadjHeader.conDocNo > 0){
				params.data.type  = "Both";
				this.gridApi.refreshCells();
			}
			else
			{
				switch (params.data.type) {
					case 'Qty':
						params.data.cost = '0';
						this.onBtStartEditing(this.setParms.rowIndex, "qty");
						break;
					case 'Cost':
						params.data.qty = '0';
						this.onBtStartEditing(this.setParms.rowIndex, "cost");
						break;
					default:
						break;
				}
			}  
		}
		if (params.column["colId"] == "qty") {
              if(this.icadjHeader.conDocNo > 0){
				  if(params.data.qty > this.conumptionQty){
					  this.notify.info("This qty cannot be greater than "+this.conumptionQty.toString())
					  params.data.qty  = this.conumptionQty;
					  this.gridApi.refreshCells();
					}
			  }
		}

        this.calculations();
        this.gridApi.refreshCells();
    }

    //==================================Grid=================================


    save(): void {
        debugger;
        this.message.confirm(
            'Save Adjustment',
            (isConfirmed) => {
                if (isConfirmed) {

                    if (moment(this.icadjHeader.docDate) > moment().endOf('day')) {
                        this.message.warn("Document date greater than current date", "Document Date Error");
                        return;
                    }

                    if ((moment(this.LockDocDate).month() + 1) != (moment(this.icadjHeader.docDate).month() + 1) && this.icadjHeader.id != null) {
                        this.message.warn('Document month not changeable', "Document Month Error");
                        return;
                    }

                    if (this.icadjHeader.locID == null || this.icadjHeader.locID == 0) {
                        this.message.warn("Please select location", "Location Required");
                        return;
                    }

                    if (this.gridApi.getDisplayedRowCount() <= 0) {
                        this.message.warn("No items details found", "Items Details Required");
                        return;
                    }

                    this.gridApi.forEachNode(node => {
                        debugger
                        if(!this.errorFlag){
                            if (node.data.itemID == "") {
                                this.message.warn("Item not found at row " + Number(node.rowIndex + 1), "Item Required");
                                this.errorFlag = true;
                                return;
                            } else if (node.data.qty == 0 && node.data.type != "Cost") {
                                this.message.warn("Qty not greater than zero at row " + Number(node.rowIndex + 1), "Qty Zero");
                                this.errorFlag = true;
                                return;
                            } else if (node.data.type == "Cost" && node.data.cost == 0) {
                                this.message.warn("Cost not greater than zero at row " + Number(node.rowIndex + 1), "Cost Zero");
                                this.errorFlag = true;
                                return;
                            } else {
                                this.errorFlag = false;
                            }
                        }
                    });

                    if (this.errorFlag) {
                        this.message.warn("Qty or Cost not greater than zero", "Qty or Cost Zero")
                        return;
                    }

                    this.saving = true;

                    var rowData = [];
                    this.gridApi.forEachNode(node => {
                        if (node.data.qty <= 0) {

                        }
                        rowData.push(node.data);
                    });



                    if (moment(new Date()).format("A") === "AM" && !this.icadjHeader.id   && (moment(new Date()).month()+1)==(moment(this.icadjHeader.docDate).month()+1)) {
                        this.icadjHeader.docDate = moment(this.icadjHeader.docDate);
                    } else {
                        this.icadjHeader.docDate = moment(this.icadjHeader.docDate).endOf('day');
                    }

                    this.icadjHeader.active = true;

                    this.adjustment.icadjDetail = rowData;
                    this.adjustment.icadjHeader = this.icadjHeader;

                    this._adjustmentServiceProxy.createOrEditAdjustment(this.adjustment)
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

    approveDoc(id: number,mode, approve) {
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("adjustment", [id], mode, approve)
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
			case "Consumption":
				this.getNewConsumption();
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
        this.setParms.data.itemID = this.InventoryLookupTableModal.id;
        this.setParms.data.itemDesc = this.InventoryLookupTableModal.displayName;
        this.setParms.data.unit = this.InventoryLookupTableModal.unit;
        this.setParms.data.conver = this.InventoryLookupTableModal.conver;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex, "type");
    }
    //================Item Model===============

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
	
	openSelectConDocNoModal(){
		debugger;
        this.target = "Consumption";
        this.InventoryLookupTableModal.id = "";
        this.InventoryLookupTableModal.displayName = ""
        this.InventoryLookupTableModal.show(this.target);
	}

	getNewConsumption(){
		this.icadjHeader.conDocNo = Number(this.InventoryLookupTableModal.id);
		this.consumptionMode = true;
		this.getConsumptionData();
	}

	getConsumptionData()
	{
			this._icadjDetailsServiceProxy.getConsumptionData(this.icadjHeader.conDocNo).subscribe(resultD => {
				debugger;
				var rData = [];
				var qty = 0;
				var amount = 0;
				resultD["result"]["items"].forEach(element => {
					rData.push(element);
					qty += element.qty;
					amount += element.amount;
				});

				this.rowData = [];
				this.rowData = rData;

				this.icadjHeader.totalQty = qty;
				this.icadjHeader.totalAmt = amount;
				this.gridApi.refreshCells();
		});
	}

	typeChange()
	{
		debugger
		if(this.icadjHeader.type == 2)
		{
			this.consumptionMode = true;
		}
		else
		{
			this.consumptionMode = false;
			this.icadjHeader.conDocNo = 0;
		}
    }
    
    //===========================File Attachment=============================
    onBeforeUpload(event): void {
        debugger;
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles?';
        if (this.adjustmentAppId !== undefined)
            this.uploadUrl += "APPID=" + encodeURIComponent("" + this.adjustmentAppId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl += "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.icadjHeader.docNo !== undefined)
            this.uploadUrl += "DocID=" + encodeURIComponent("" + this.icadjHeader.docNo) + "&";
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
