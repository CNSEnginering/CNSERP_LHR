import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GatePassHeaderDto } from '../shared/dto/gatePassHeader-dto';
import { GatePassDetailDto } from '../shared/dto/gatePassDetail-dto';
import { GatePassService } from '../shared/services/gatePass.service';
import { ItemPricingLookupTableModalComponent } from '../FinderModals/itemPricing-lookup-table-modal.component';
import { CostCenterLookupTableModalComponent } from '../FinderModals/costCenter-lookup-table-modal.component';
import { GatePassLookupTableModalComponent } from '../FinderModals/gatePass-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { Lightbox } from 'ngx-lightbox';
import { AppConsts } from '@shared/AppConsts';
import { GLTRHeadersServiceProxy } from '@shared/service-proxies/service-proxies';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';

@Component({
    selector: 'GatePassModal',
    templateUrl: './create-or-edit-gatePass-modal.component.html',
    styleUrls: ['./create-or-edit-gatePass-modal.component.css']
})
export class CreateOrEditGetPassModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('inventoryLookupTableModal', { static: true }) inventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('ItemPricingLookupTableModal', { static: true }) ItemPricingLookupTableModal: ItemPricingLookupTableModalComponent;
    @ViewChild('CostCenterLookupTableModal', { static: true }) CostCenterLookupTableModal: CostCenterLookupTableModalComponent;
    @ViewChild('GatePassLookupTableModal', { static: true }) GatePassLookupTableModal: GatePassLookupTableModalComponent
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    totalItems: number;
    editMode: boolean = false;
    totalQty: number;
    active = false;
    saving = false;
    mode: string;
    priceListChk: boolean = false;
    editState: boolean = false;
    gatePass: GatePassHeaderDto;
    gatePassDetail: GatePassDetailDto;
	gatePassDetailData: GatePassDetailDto[] = new Array<GatePassDetailDto>();
    
    agGridExtend: AgGridExtend = new AgGridExtend();
    
	image = [];
	appId = 8;
	appName = "GatePassEntry";
    uploadedFiles = [];
	checkImage = true;
	url: string;
    uploadUrl: string;
    // gatePassDetailDataTemp: GatePassDetailDto[] = new Array<GatePassDetailDto>();
    gridApi;
    gridColumnApi;
    rowData;
    rowSelection;
    CheckDocDate:boolean;
    OGPNo: number = 0;
    IGPNo: number = 0;
    errorFlag: boolean=false;
    paramsData;
    columnDefs = [
        { headerName: this.l('SrNo'), editable: false, field: 'srNo', sortable: true, width: 100, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('Item'), editable: false, field: 'item', sortable: true, filter: true, width: 200, resizable: true },
        { headerName: this.l('Description'), editable: false, field: 'description', sortable: true, filter: true, width: 200, resizable: true },
        { headerName: this.l('UOM'), editable: true, field: 'unit', sortable: true, filter: true, width: 150, resizable: true },
        { headerName: this.l('Conver'), editable: false, field: 'conver', sortable: true, filter: true, width: 150, resizable: true },
        { headerName: this.l('Qty'), editable: true, field: 'qty',valueFormatter: this.agGridExtend.formatNumber, width: 100, resizable: true },
        { headerName: this.l('Comments'), editable: true, field: 'comments', sortable: true, width: 150, resizable: true },
        { headerName: this.l('MaxQty'), editable: false, field: 'maxQty', sortable: true, width: 150, resizable: true }
    ];
    formValid: boolean = false;
    constructor(
        injector: Injector,
		private _gatePassService: GatePassService,
		private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
		private _lightbox: Lightbox
    ) {
        super(injector);
        this.gatePassDetailData.length = 0;
        // this.gatePassDetailDataTemp.length = 0;
    }
    SetDefaultRecord(result:any){
        //console.log(result);
          this.CheckDocDate=result.cDateOnly;
       
      }
    show(id?: number, type?: string): void {
        this.active = true;
        this.gatePass = new GatePassHeaderDto();
        this.gatePassDetail = new GatePassDetailDto();
        this.gatePassDetailData = new Array<GatePassDetailDto>();
        this.editMode = false;
        this.totalQty = 0;
        this.totalItems = 0;
		this.gatePass.docDate = new Date();
		this.url = null;
		this.image = [];
		this.uploadedFiles = [];
		this.uploadUrl=null;
		this.checkImage = true;
        if (type != "OGP")
            this.OGPNo = 0;

        if (type != "IGP")
            this.IGPNo = 0;

        if (type == "Inward") {
            this.gatePass.typeId = 1;
            this.getDocNo();
        }
        else if (type == "Outward") {
            this.gatePass.typeId = 2;
            this.getDocNo();
        }

        if (type != 'OGP' && type != 'IGP')
            this.mode = type;
        this.formValid = false;
        if (!id) {

        }
        else {
            if (type != "IGP" && type != "OGP")
                this.editMode = true;

            this._gatePassService.getDataForEdit(id, type).subscribe(
                (data: any) => {
                    this.gatePass.id = data["result"]["gatePass"]["id"];
                    this.gatePass.accountId = data["result"]["gatePass"]["accountID"];
                    this.gatePass.docDate = new Date(data["result"]["gatePass"]["docDate"]);
                    this.gatePass.driverName = data["result"]["gatePass"]["driverName"];
                    this.gatePass.accountName = data["result"]["gatePass"]["accountName"];
                    this.gatePass.partyName = data["result"]["gatePass"]["partyName"];
                    this.gatePass.orderNo = data["result"]["gatePass"]["orderNo"];
                    this.gatePass.dcNo = data["result"]["gatePass"]["dcNo"];
                    debugger
                    if (this.editMode == true)
                        this.gatePass.gpType = data["result"]["gatePass"]["gpType"];


                    this.gatePass.narration = data["result"]["gatePass"]["narration"];
                    this.gatePass.partyId = data["result"]["gatePass"]["partyID"];

                    if (this.editMode == true)
                        this.gatePass.typeId = data["result"]["gatePass"]["typeID"];

                    if (type == "OGP")
                        this.gatePass.typeId = 1;
                    else if (type == "IGP")
                        this.gatePass.typeId = 2;


                    if (this.gatePass.typeId == 1)
                        this.mode = "Inward"
                    else if (this.gatePass.typeId == 2)
                        this.mode = "Outward"



                    this.gatePass.docNo = data["result"]["gatePass"]["docNo"];


                    if (this.OGPNo > 0 || this.IGPNo > 0) {
                        this.getDocNo();
                        this.gatePass.gpType = "0";
                    }




                    this.gatePass.vehicleNo = data["result"]["gatePass"]["vehicleNo"];

                    if (this.editMode == true)
                        this.gatePass.gpDocNo = data["result"]["gatePass"]["gpDocNo"];


                    if (this.OGPNo > 0 || this.IGPNo > 0)
                        this.gatePass.gpDocNo = data["result"]["gatePass"]["docNo"];

                    this.addRecordToGrid(data["result"]["gatePass"]["gatePassDetailDto"]);

					this.checkFormValid();
					
								
			this._gltrHeadersServiceProxy.getImage(this.appId, data["result"]["gatePass"]["docNo"])
			.subscribe(fileResult => {
				if (fileResult != null) {
					this.url = 'data:image/jpeg;base64,' + fileResult;
					const album = {
						src: this.url
					};
					this.image.push(album);
					this.checkImage = false;
				}
			});
                }
			);
        }
        this.modal.show();
    }



    close(): void {
        this.active = false;
		this.modal.hide();
		this._lightbox.close();
	}
	


	 //===========================File Attachment=============================
	 onBeforeUpload(event): void {
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles?';
        if (this.appId !== undefined)
            this.uploadUrl += "APPID=" + encodeURIComponent("" + this.appId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl += "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.gatePass.docNo !== undefined)
            this.uploadUrl += "DocID=" + encodeURIComponent("" + this.gatePass.docNo) + "&";
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
        this._lightbox.open(this.image);
    }

    addRecordToGrid(record: any) {
        this.editState = true;
        if (record != undefined) {
            record.forEach((val, index) => {
                var str = val.itemID.split('*');
                var newData;
                if (this.gatePass.gpType != "1" && this.gatePass.gpType != null && this.gatePass.gpDocNo != null) {
                    this._gatePassService.GetQtyAgainstItem(str[0], this.gatePass.gpDocNo, this.gatePass.typeId, this.gatePass.id)
                        .subscribe(
                            data => {
                                if (this.OGPNo != 0 || this.IGPNo != 0) {
                                    newData = {
                                        srNo: index,
                                        item: str[0],
                                        description: str[1],
                                        unit: str[2],
                                        conver: str[3],
                                        qty: val.qty,
                                        comments: val.comments,
                                        maxQty: val.qty
                                        // maxQty: data["result"]
                                    };
                                }
                                else {
                                    newData = {
                                        srNo: index,
                                        item: str[0],
                                        description: str[1],
                                        unit: str[2],
                                        conver: str[3],
                                        qty: val.qty,
                                        comments: val.comments,
                                        maxQty: data["result"]
                                    };
                                }
                                this.addOrUpdateRecordToDetailData(newData, 'record');

                                setTimeout(() => {
                                    this.gridApi.updateRowData({ add: [newData] })  
                                },2000 );
                            }

                        )
                }
                else {
                    if (this.OGPNo != 0 || this.IGPNo != 0) {
                        newData = {
                            srNo: index,
                            item: str[0],
                            description: str[1],
                            unit: str[2],
                            conver: str[3],
                            qty: val.qty,
                            comments: val.comments,
                            maxQty: val.qty
                            // maxQty: data["result"]
                        };
                    }
                    else {
                        newData = {
                            srNo: index,
                            item: str[0],
                            description: str[1],
                            unit: str[2],
                            conver: str[3],
                            qty: val.qty,
                            comments: val.comments,
                            //maxQty: data["result"]
                        };
                    }
                    this.addOrUpdateRecordToDetailData(newData, 'record');

                    setTimeout(() => {
                        this.gridApi.updateRowData({ add: [newData] })  
                    },2000 );

                }
            });
            this.editState = false;
        }
        else {
            let length = this.gatePassDetailData.length;
            var newData = {
                srNo: ++length,
                item: undefined,
                description: undefined,
                unit: undefined,
                conver: undefined,
                qty: undefined,
                comments: undefined,
                maxQty: undefined
            };
            this.addOrUpdateRecordToDetailData(newData, 'record');

            this.gridApi.updateRowData({ add: [newData] });
        }
        this.checkFormValid();
    }

    addOrUpdateRecordToDetailData(data: any, type: string) {
        if (type == 'record') {
            this.gatePassDetail = new GatePassDetailDto();
            this.gatePassDetail.srNo = data.srNo;
            this.gatePassDetail.itemId = data.item;
            this.gatePassDetail.description = data.description;
            this.gatePassDetail.unit = data.unit;
            this.gatePassDetail.conver = data.conver;
            this.gatePassDetail.qty = data.qty;
            this.gatePassDetail.comments = data.comments;
            if (this.OGPNo > 0 || this.IGPNo > 0)
                this.gatePassDetail.maxQty = data.qty;

            this.gatePassDetailData.push(this.gatePassDetail);

            // this.gatePassDetailDataTemp = this.gatePassDetailData.slice();
        }
        else {
            var filteredData = this.gatePassDetailData.find(x => x.srNo == data.srNo);
            if (filteredData.srNo != undefined) {
                filteredData.itemId = data.item;
                filteredData.comments = data.comments;
                filteredData.conver = data.conver;
                filteredData.unit = data.unit;
                filteredData.description = data.description;

                if (this.OGPNo > 0 || this.IGPNo > 0 || (this.editMode == true && this.gatePass.gpType != "1")) {
                    if (Number(data.maxQty) < Number(data.qty)) {
                        this.notify.info("Qty Cannot Be Greater Than Remaining " + data.maxQty + " Qty");
                        data.qty = Number(data.maxQty);
                    }
                    else if (Number(data.qty) == 0) {
                        this.notify.info("Qty Cannot Be Zero");
                        data.qty = Number(data.maxQty);
                    }
                    else {
                        filteredData.qty = data.qty;
                    }
                }
                else {
                    filteredData.qty = data.qty;
                }

                // filteredData.unit = data.unit;
            }

        }

        this.totalItems = this.gatePassDetailData.length;
        this.calculateTotalQty();
    }

    ngOnInit(): void {
        this.rowData = [];
    }
    onGridReady(params) {
        this.rowData = [];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onRowDoubleClicked(params) {
        if (this.OGPNo == 0 && this.IGPNo == 0 && (this.gatePass.gpDocNo == 0 || this.gatePass.gpDocNo == null)) {
            this.inventoryLookupTableModal.show("Items");
            // this.ItemPricingLookupTableModal.show("GatePassItem");
            this.paramsData = params;
        }
    }

    getLookUpData() {
        this.paramsData.data.item = this.inventoryLookupTableModal.id;
        this.paramsData.data.description = this.inventoryLookupTableModal.displayName;
        this.paramsData.data.unit = this.inventoryLookupTableModal.unit;
        this.paramsData.data.conver = this.inventoryLookupTableModal.conver;
        // this.paramsData.data.item = this.ItemPricingLookupTableModal.data.itemId;
        // this.paramsData.data.description = this.ItemPricingLookupTableModal.data.descp;
        // this.paramsData.data.unit = this.ItemPricingLookupTableModal.data.stockUnit;
        // this.paramsData.data.conver = this.ItemPricingLookupTableModal.data.conver;
        this.gridApi.refreshCells();
        this.addOrUpdateRecordToDetailData(this.paramsData.data, '');
        this.checkFormValid();
    }

    getDocNo() {
        if (this.gatePass.typeId != 0) {
            this._gatePassService.GetDocIdAgainstGatePassType(this.gatePass.typeId).subscribe(
                (data: any) => {
                    this.gatePass.docNo = data["result"];
                    this.checkFormValid();
                }
            )
        }
        else {
            this.checkFormValid();
        }
    }
    openModal(type: string) {
        this.FinanceLookupTableModal.id = "";
        this.FinanceLookupTableModal.displayName = "";
        if (type == "ChartOfAccount")
            this.FinanceLookupTableModal.show("ChartOfAccount");
        else {
            this.FinanceLookupTableModal.show("SubLedger", this.gatePass.accountId);
        }

    }

    // openModal(type: string) {
    //     this.CostCenterLookupTableModal.accId = this.gatePass.accountId;
    //     this.CostCenterLookupTableModal.data = null;
    //     this.CostCenterLookupTableModal.show(type);
    // }

    getAccLookUpData() {
        if (this.FinanceLookupTableModal.target == "ChartOfAccount") {
            this.gatePass.accountId = this.FinanceLookupTableModal.id;
            this.gatePass.accountName = this.FinanceLookupTableModal.displayName;
        }
        else {
            this.gatePass.partyId = Number(this.FinanceLookupTableModal.id) == 0 ? undefined :
                Number(this.FinanceLookupTableModal.id);
            this.gatePass.partyName = this.FinanceLookupTableModal.displayName;
        }


        this.checkFormValid();
    }

    cellValueChanged(params) {
        this.addOrUpdateRecordToDetailData(params.data, '')
        this.calculateTotalQty();
        if (params.data.item != '' && params.data.qty != '' && params.data.qty != undefined
            && params.data.qty > 0
        ) {
            this.editState = false;
        }
        else {
            this.editState = true;
        }
        if (params.data.qty < 0) {
            this.notify.info("Qty Should Be Greater Than Zero");
        }
    }

    save() {
        this.message.confirm(
            'Save',
            (isConfirmed) => {
                if (isConfirmed) {

                    

                    this.saving = true;

                    this.gridApi.forEachNode(node=>{
                        debugger
                        if(node.data.item=="" || node.data.item== undefined){
                            this.message.warn("Item not found at row "+ Number(node.rowIndex+1),"Item Required");
                            this.errorFlag=true;
                            this.saving = false;
                            return;
                        }else if(node.data.qty <= 0 || isNaN(node.data.qty)){
                            this.message.warn("Qty should be greater than 0 at row "+ Number(node.rowIndex+1),"Qty Greater");
                            this.errorFlag=true;
                            this.saving = false;
                            return;
                        }else{
                            this.errorFlag=false;
                            this.saving = false;
                        }
                    });

                    if (this.OGPNo != 0 || this.IGPNo != 0)
                        this.gatePass.id = undefined;

                    this.gatePass.GatePassDetailDto.push(...this.gatePassDetailData.slice());
                    this._gatePassService.create(this.gatePass)
                        .subscribe(() => {
                            this.saving = false;
                            this.notify.info(this.l('SavedSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        });
                    //this.close();
                }
            });
    }

    calculateTotalQty() {
        let qty = 0;
        this.gatePassDetailData.forEach((val, index) => {
            qty = qty + Number(val.qty);
            if (!isNaN(qty))
                this.totalQty = qty;
        });
        this.gatePassDetailData.length == 0 ? this.totalQty = 0 : "";
        this.checkFormValid();
    }

    removeRecordFromGrid() {
        var selectedData = this.gridApi.getSelectedRows();
        var filteredDataIndex = this.gatePassDetailData.findIndex(x => x.srNo == selectedData[0].srNo);
        this.gatePassDetailData.splice(filteredDataIndex, 1);
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        this.totalItems = this.gatePassDetailData.length;
        this.calculateTotalQty();
        this.checkFormValid();
    }

    checkFormValid() {
        if (this.gatePass.typeId == 0 || this.gatePass.accountId == "" || this.gatePass.partyId == undefined
            || this.gatePass.docDate == null || this.gatePass.docNo == undefined
            || this.gatePassDetailData.length == 0 || this.totalQty == 0
            || this.editState == true ||
            this.gatePass.docDate > (new Date())
        ) {
            this.formValid = false;
        }
        else {
            this.formValid = true;
        }

    }

    openModalOGPNo() {
        this.GatePassLookupTableModal.show(this.mode);
    }

    getOGPLookUpData() {
        this.rowData = [];
        this.totalQty = 0;
        this.totalItems = 0;
        this.gatePassDetailData.length = 0;
        if (this.GatePassLookupTableModal.data != undefined) {
            if (this.mode == "Outward") {
                this.IGPNo = this.GatePassLookupTableModal.data;
                this.show(this.IGPNo, "IGP");
            }
            else if (this.mode == "Inward") {
                this.OGPNo = this.GatePassLookupTableModal.data;
                this.show(this.OGPNo, "OGP");
            }
        }
    }

    cellEditingStarted(params) {
        debugger
        this.formValid = false;
    }
    dateChange(event: any) {
        this.gatePass.docDate = event;
        var currDate = new Date();
        var selectedDate = this.gatePass.docDate;
        if (selectedDate > currDate) {
            this.notify.info("You cannot enter the date after today")
        }
        this.checkFormValid();
    }
}
