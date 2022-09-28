import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TransferDto } from "../shared/dto/transfer-dto";
import { InventoryGlLinkLookupTableModalComponent } from "../FinderModals/InventoryGlLink-lookup-table-modal.component";
import { TransfersService } from "../shared/services/transfers.service";
import { TransferDetailDto } from "../shared/dto/transferDetail-dto";
import { CostCenterLookupTableModalComponent } from "../FinderModals/costCenter-lookup-table-modal.component";
import { ItemPricingLookupTableModalComponent } from "../FinderModals/itemPricing-lookup-table-modal.component";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { getUnpackedSettings } from "http2";
import { ApprovalService } from "../../periodics/shared/services/approval-service.";
import { Lightbox } from "ngx-lightbox";
import { AppConsts } from "@shared/AppConsts";
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
import { AgGridExtend } from "@app/shared/common/ag-grid-extend/ag-grid-extend";
import { finalize } from "rxjs/operators";
import * as moment from "moment";
import { LogComponent } from "@app/finders/log/log.component";
//import { LogComponent } from '@app/finders/log/log.component';
@Component({
    selector: "TransfersModal",
    templateUrl: "./create-or-edit-transfers-modal.component.html",
    styleUrls: ["./create-or-edit-transfers-modal.component.css"]
})
export class CreateOrEditTransfersModalComponent extends AppComponentBase
    implements OnInit {
    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    //@ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    // @ViewChild('ItemPricingLookupTableModal', { static: true }) ItemPricingLookupTableModal: ItemPricingLookupTableModalComponent;
    // @ViewChild('CostCenterLookupTableModal', { static: true }) CostCenterLookupTableModal: CostCenterLookupTableModalComponent;
    // @ViewChild('InventoryGlLinkLookupTableModal', { static: true }) InventoryGlLinkLookupTableModal: InventoryGlLinkLookupTableModalComponent
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    totalItems: number;
    editMode: boolean = false;
    totalQty: number;
    active = false;
    saving = false;
    processing = false;
    activeqty=false;
    priceListChk: boolean = false;
    transfer: TransferDto;
    transferDetail: TransferDetailDto;
    transfersService: TransferDto = new TransferDto();
    transferDetailData: TransferDetailDto[] = new Array<TransferDetailDto>();
    agGridExtend: AgGridExtend = new AgGridExtend();
    approving;
    gridApi;
    gridColumnApi;
    LocValCheck:boolean;
    rowData;
    rowSelection;
    CheckDate:boolean;
    type: string;
    editState: Boolean = false;
    errorFlag: boolean=false;
    paramsData;
    appId = 10;
    appName = "TransfersEntry";
    uploadedFiles = [];
    checkImage = true;
    image = [];
    url: string;
    uploadUrl: string;
    columnDefs = [
        {
            headerName: this.l("SrNo"),
            editable: false,
            field: "srNo",
            sortable: true,
            width: 100,
            valueGetter: "node.rowIndex+1"
        },
        {
            headerName: this.l("Item"),
            editable: false,
            field: "item",
            sortable: true,
            filter: true,
            width: 200,
            resizable: true
        },
        {
            headerName: this.l("Description"),
            editable: false,
            field: "description",
            sortable: true,
            filter: true,
            width: 220,
            resizable: true
        },
        {
            headerName: this.l("UOM"),
            editable: false,
            field: "unit",
            sortable: true,
            filter: true,
            width: 150,
            resizable: true
        },
        {
            headerName: this.l("Conver"),
            editable: false,
            field: "conver",
            sortable: true,
            filter: true,
            width: 200,
            resizable: true
        },
        {
            headerName: this.l("Qty In Hand"),
            editable: false,
            field: "maxQty",
            sortable: true,
            width: 200,
            resizable: true
        },
        {
            headerName: this.l("Qty"),
            editable: true,
            field: "qty",
            width: 100,
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber
        },
        // {
        //     headerName: this.l("LotNo"),
        //     editable: true,
        //     field: "lotNo",
        //     sortable: true,
        //     width: 150,
        //     resizable: true
        // },
        // {
        //     headerName: this.l("Bundle"),
        //     editable: true,
        //     field: "bundle",
        //     sortable: true,
        //     width: 150,
        //     resizable: true
        // },
        {
            headerName: this.l("Remarks"),
            editable: true,
            field: "remarks",
            sortable: true,
            width: 200,
            resizable: true
        }
        
    ];
    formValid: boolean = false;
    constructor(
        injector: Injector,
        private _transfersService: TransfersService,
        private _approvelService: ApprovalService,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
        private _lightbox: Lightbox
    ) {
        super(injector);
        // this.gatePassDetailData.length = 0;
        // this.gatePassDetailDataTemp.length = 0;
    }

    show(id?: number, type?: string): void {
        this.active = true;
        this.transfer = new TransferDto();
        this.transferDetailData = new Array<TransferDetailDto>();
        //this.gatePassDetail = new GatePassDetailDto();
        //this.gatePassDetailData = new Array<GatePassDetailDto>();

        this.editMode = false;
        this.totalQty = 0;
        this.totalItems = 0;

        this.url = null;
        this.image = [];
        this.uploadedFiles = [];
        this.uploadUrl = null;
        this.checkImage = true;

        this.formValid = false;
        if (!id) {
            this._transfersService.GetDocId().subscribe(res => {
                this.transfer.docNo = res["result"];
                this.transfer.docDate = new Date();
            });
        } else {
            this.editMode = true;
            this._transfersService
                .getDataForEdit(id, type)
                .subscribe((data: any) => {
                    this.transfer.id = data["result"]["transfer"]["id"];
                    this.transfer.ordNo = data["result"]["transfer"]["ordNo"];
                    this.transfer.docDate = new Date(
                        data["result"]["transfer"]["docDate"]
                    );
                    this.transfer.docNo = data["result"]["transfer"]["docNo"];
                    this.transfer.narration =
                        data["result"]["transfer"]["narration"];
                    this.transfer.fromLocId =
                        data["result"]["transfer"]["fromLocID"];
                    this.transfer.toLocId =
                        data["result"]["transfer"]["toLocID"];
                    this.transfer.fromLocDesc =
                        data["result"]["transfer"]["fromLocName"];
                    this.transfer.toLocDesc =
                        data["result"]["transfer"]["toLocName"];
                    this.transfer.narration =
                        data["result"]["transfer"]["narration"];
                    this.transfer.totalQty =
                        data["result"]["transfer"]["totalQty"];
                    this.transfer.totalAmt =
                        data["result"]["transfer"]["totalAmt"];
                    this.transfer.posted = data["result"]["transfer"]["posted"];
                    this.transfer.approved =
                        data["result"]["transfer"]["approved"];
                    this.transfer.postedBy =
                        data["result"]["transfer"][" postedBy"];
                    this.transfer.postedDate =
                        data["result"]["transfer"]["postedDate"];
                    this.transfer.linkDetID =
                        data["result"]["transfer"]["linkDetID"];
                    this.transfer.ordNo = data["result"]["transfer"]["ordNo"];
                    this.transfer.hold = data["result"]["transfer"]["hold"];
                    console.log( data["result"]["transfer"]["audtUser"]);
                    this.transfer.audtUser =
                        data["result"]["transfer"]["audtUser"];
                    this.transfer.audtDate =
                        data["result"]["transfer"]["audtDate"];
                    this.transfer.createdBy =
                        data["result"]["transfer"]["createdBy"];
                    this.transfer.createDate =
                        data["result"]["transfer"]["createDate"];
                    this.transfer.vehicleNo =
                        data["result"]["transfer"]["vehicleNo"];
                    this.transfer.referenceNo =
                        data["result"]["transfer"]["referenceNo"];
                    this.transfer.ccid = data["result"]["transfer"]["ccid"];
                    this.transfer.ccdesc = data["result"]["transfer"]["ccdesc"];
                    
                    this.addRecordToGrid(
                        data["result"]["transfer"]["transferDetailDto"]
                    );
                    this.checkFormValid();

                    this._gltrHeadersServiceProxy
                        .getImage(
                            this.appId,
                            data["result"]["transfer"]["docNo"]
                        )
                        .subscribe(fileResult => {
                            if (fileResult != null) {
                                this.url =
                                    "data:image/jpeg;base64," + fileResult;
                                const album = {
                                    src: this.url
                                };
                                this.image.push(album);
                                this.checkImage = false;
                            }
                        });
                });
        }
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this._lightbox.close();
    }

    addRecordToGrid(record: any) {
        debugger
        this.editState = true;
        if (record != undefined) {
            record.forEach((val, index) => {
                var str = val.itemID.split("*");
                var newData = {
                    srNo: index,
                    item: str[0],
                    description: str[1],
                    unit: str[2],
                    conver: str[3],
                    qty: val.qty==NaN?0:val.qty,
                    lotNo: val.lotNo,
                    bundle: val.bundle,
                    remarks: val.remarks,
                    maxQty: val.maxQty,                    
                };
                this.addOrUpdateRecordToDetailData(newData, "record");
                setTimeout(() => {
                    this.gridApi.updateRowData({ add: [newData] });
                }, 2000);
            });
            this.editState = false;
            this.checkFormValid();
        } else {
            let length = this.transferDetailData.length;
            var newData = {
                srNo: ++length,
                item: undefined,
                description: undefined,
                uni: undefined,
                conver: undefined,
                qty: 1,
                lotNo: undefined,
                bundle: undefined,
                remarks: undefined,
                maxQty: undefined
            };
            this.addOrUpdateRecordToDetailData(newData, "record");

            this.gridApi.updateRowData({ add: [newData] });
        }
        this.checkFormValid();
    }

    addOrUpdateRecordToDetailData(data: any, type: string) {
    debugger
    if (type == "record") {
            this.transferDetail = new TransferDetailDto();
            this.transferDetail.srNo = data.srNo;
            this.transferDetail.itemId = data.item;
            this.transferDetail.description = data.description;
            this.transferDetail.unit = data.unit;
            this.transferDetail.qty = data.qty;
            this.transferDetail.conver=data.conver;
            this.transferDetail.remarks = data.remarks;
            this.transferDetail.maxQty=data.maxQty;
            this.transferDetailData.push(this.transferDetail);
       } 
        else {
            var filteredData = this.transferDetailData.find(
                x => x.srNo == data.srNo
            );
            if (filteredData.srNo != undefined) {
                filteredData.itemId = data.item;
                filteredData.remarks = data.remarks;
                filteredData.lotNo = data.lotNo;
                filteredData.bundle = data.bundle;
                filteredData.conver = data.conver;
                filteredData.description = data.description;
                filteredData.maxQty=data.maxQty;
                if (Number(data.qty) == 0) {
                    this.notify.info("Qty Cannot Be Zero");
                } else if (Number(data.maxQty) < Number(data.qty)) {
                   
                        // this.notify.info(
                        //     "Qty Cannot Be Greater Than Remaining " +
                        //     data.maxQty +
                        //     " Qty"
                        // );
                } else {
                    filteredData.qty = data.qty;
                }
                filteredData.qty = data.qty;
                filteredData.unit = data.unit;
            }
        }

        this.totalItems = this.transferDetailData.length;
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

        debugger;
        if (params.colDef.headerName == "Item") {
            this.type = "item";
            // this.ItemPricingLookupTableModal.show("GatePassItem");
            this.inventoryLookupTableModal.id = "";
            this.inventoryLookupTableModal.displayName = "";
            this.inventoryLookupTableModal.show("Items");
            this.paramsData = params;
        }
    }
    SetDefaultRecord(result:any){
        console.log(result);
          this.CheckDate=result.cDateOnly;
          this.transfer.fromLocId=result.currentLocID;
          this.transfer.fromLocDesc=result.currentLocName;
        //  this.transfer.toLocId=result.currentLocID;
        //  this.transfer.toLocDesc=result.currentLocName;
          if(result.allowLocID==true){
            this.LocValCheck=true;
          }else{
            this.LocValCheck=false;
          }
          
      }
    getLookUpData() {
        debugger
        if (this.type == "fromLoc") {
            this.transfer.fromLocId =
                Number(this.inventoryLookupTableModal.id) == 0
                    ? undefined
                    : Number(this.inventoryLookupTableModal.id);
            this.transfer.fromLocDesc = this.inventoryLookupTableModal.displayName;
            this.checkLocValidTrasnfer();
        } else if (this.type == "toLoc") {
            this.transfer.toLocId =
                Number(this.inventoryLookupTableModal.id) == 0
                    ? undefined
                    : Number(this.inventoryLookupTableModal.id);
            this.transfer.toLocDesc = this.inventoryLookupTableModal.displayName;
            this.checkLocValidTrasnfer();
        } else if (this.type == "item") {
            var ConStatus=false;
            this.gridApi.forEachNode(node=>{
                debugger
             if(node.data.item!='' && node.data.item!=null){
                if(node.data.item==this.inventoryLookupTableModal.id){
                    this.message.warn("Item Has Already Exist At Row No "+ Number(node.rowIndex+1),"Item Duplicate!");
                    ConStatus=true;
                    return;
                }
             }
            });
    
            if(ConStatus==false){
    
            this.paramsData.data.item = this.inventoryLookupTableModal.id;
            this.paramsData.data.description = this.inventoryLookupTableModal.displayName;
            this.paramsData.data.unit = this.inventoryLookupTableModal.unit;
            this.paramsData.data.conver = this.inventoryLookupTableModal.conver;
            // this.paramsData.data.item = this.ItemPricingLookupTableModal.data.itemId;
            // this.paramsData.data.description = this.ItemPricingLookupTableModal.data.descp;
            // this.paramsData.data.unit = this.ItemPricingLookupTableModal.data.stockUnit;
            // this.paramsData.data.conver = this.ItemPricingLookupTableModal.data.conver;
            this._transfersService
                .GetQtyInHand( this.transfer.fromLocId,this.paramsData.data.item,moment(this.transfer.docDate).format("YYYY-MM-DD"))
                .subscribe(res => {
                    this.paramsData.data.maxQty = res["result"];
                    this.gridApi.refreshCells();
                    this.addOrUpdateRecordToDetailData(
                        this.paramsData.data,
                        ""
                    );
                    this.checkEditState();
                    this.checkFormValid();
                });
            }
            // this.checkFormValid();
        }
        else if (this.type == "CostCenter")
            this.getNewCostCenter();

        this.gridApi.refreshCells();
        if (this.paramsData != undefined) {
            this.addOrUpdateRecordToDetailData(this.paramsData.data, "");
            this.checkEditState();
            this.checkFormValid();
        }
    }
    checkEditState() {
        if (
            this.paramsData.data.item != "" &&
            this.paramsData.data.qty > 0 &&
            this.paramsData.data.qty <= this.paramsData.data.maxQty
        ) {
            this.editState = false;
        } else {
            this.editState = true;
        }
    }
   

    cellEditingStarted(params) {
        debugger
      //  this.formValid = false;
    }
    OpenLog(){
        debugger
       this.LogTableModal.show(this.transfer.docNo,'Tranfer');
    }
    approveDoc(id: number, mode, approve) {
        debugger;
        this.message.confirm(
            approve ? "Approve Transfer" : "Unapprove Transfer",
            isConfirmed => {
                if (isConfirmed) {
                    this.approving = true
                    this._approvelService
                        .ApprovalData("transfer", [id], mode, approve)
                        .pipe((finalize(() => this.approving = false)))
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
                    this._approvelService.ApprovalData("transfer", [id], mode, posting)
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

    processTransfer(): void {
        this.message.confirm("", isConfirmed => {
            if (isConfirmed) {
                this.processing = true;

                this._transfersService
                    .processTransfer(this.transfer)
                    .subscribe(result => {
                        if (result == "Save") {
                            this.saving = false;
                            this.notify.info(this.l("ProcessSuccessfully"));
                            this.close();
                            this.modalSave.emit(null);
                        } else {
                            this.saving = false;
                            this.notify.error(this.l("ProcessFailed"));
                        }
                    });
            }
        });
    }
    cellValueChanged(params) {

        if(params.data.qty>0 && params.data.qty !=undefined){
            if(params.data.qty>params.data.qtyInHand){
             //this.notify.error('Qty Is Greater Than Work Order Qty!');
             this.message.warn("Qty not greater than Qty In Hand ","Qty Greater");
             this.activeqty=true;
             return;
              }else{
                this.activeqty=true;
              }
            }
        this.addOrUpdateRecordToDetailData(params.data, "");
        this.calculateTotalQty();
        this.checkEditState();
        // if (params.data.qty <= 0) {
        //     this.notify.info("Qty Should Be Greater Than Zero");
        // }
        this.checkFormValid();
        this.CheckOnQtyInHand();
    }
    CheckOnQtyInHand() {

        this.gridApi.forEachNode(node => {
            debugger
            if (parseInt(node.data.qty) > parseInt(node.data.maxQty)) {
                this.errorFlag=true;
                this.message.error("Qty Not Greater than Qty In Hand at Row No."+ Number(node.rowIndex+1),"Qty Greater");
                throw new Error();
            } else {
                this.errorFlag = false;
            }
        });
    }

    save() {
        debugger
        if(this.transfer.createdBy==this.transfer.audtUser || this.transfer.audtUser=='admin'){

       
        if(!this.errorFlag){
        this.transfer.totalQty = this.totalQty;
        this.transfer.transferDetailDto.length = 0;
        this.transfer.transferDetailDto.push(
            ...this.transferDetailData.slice()
        );

        //    this.gridApi.forEachNode(node=>
        //     {
        //         debugger
        //         console.log(node.data)
        //         this.transfer.transferDetailDto(node.data);
        //     });

        if (this.checkQtyBeforeSave() == true) {
           // this.notify.info("No Qty Should Be Equal Or Less Than Zero");
           this.message.warn("No Qty Should Be Equal Or Less Than Zero");
          
            return;
        }
        this.gridApi.forEachNode(node=>{debugger
            //if(!this.errorFlag){
                if(node.data.item==""){
                    this.message.warn("Item not found at row "+ Number(node.rowIndex+1),"Item Required");
                   this.activeqty=true;
                    return;
                }else if(node.data.maxQty!=0 && node.data.maxQty<node.data.qty){
                    this.message.warn("Qty not greater than Qty In Hand at row "+ Number(node.rowIndex+1),"Qty Greater");
                    this.activeqty=true;
                    return;
                }else if (node.data.conver == 0 || node.data.conver == undefined || node.data.conver=='' ) {
                    this.message.warn("Conver Should Not Be Zero at row " + Number(node.rowIndex + 1), "Item Required");
                    this.activeqty=true;
                    return;
                }else {
                    this.activeqty=false;
                }
          //  }
        });
      
       // this.saving = true;
       if( this.activeqty==false )
       {
        this._transfersService.create(this.transfer).subscribe((mes) => {
            console.log(mes);
            debugger
            this.saving = false;
            this.notify.info(this.l("SavedSuccessfully"));
            this.close();
            this.modalSave.emit(null);
        });
      
       }else
       {
        this.saving = false;
       }
      
    }else{
        this.message.warn("Qty not greater than Qty in Hand ","Qty Greater");
     } 
    }else{
        this.message.info("You Can't Updated");
    }
    }
    
    calculateTotalQty() {
        let qty = 0;
        this.totalQty = 0;
        this.transferDetailData.forEach((val, index) => {
            qty = qty + Number(val.qty);
            if (!isNaN(qty)) this.totalQty = qty;
        });
        this.transferDetailData.length == 0 ? (this.totalQty = 0) : "";
        this.checkFormValid();
    }

    removeRecordFromGrid() {
        var selectedData = this.gridApi.getSelectedRows();
        var filteredDataIndex = this.transferDetailData.findIndex(
            x => x.srNo == selectedData[0].srNo
        );
        this.transferDetailData.splice(filteredDataIndex, 1);
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        this.totalItems = this.transferDetailData.length;
        this.calculateTotalQty();
        this.editState = false;
        this.checkFormValid();
    }

    dateChange(event: any) {
        this.transfer.docDate = event;
        var currDate = new Date();
        if (this.transfer.docDate > currDate) {
            this.notify.info("You cannot enter the date after today");
        }
        this.checkFormValid();
    }

    checkFormValid() {
        
        if (
            this.transfer.docDate == null ||
            this.transfer.docDate > new Date() ||
            this.transfer.fromLocId == undefined ||
            this.transfer.toLocId == undefined ||
            this.transfer.docNo == null ||
            this.transfer.docNo == undefined ||
            this.transferDetailData.length == 0 ||
            this.totalQty == 0 ||
            this.transfer.fromLocId === this.transfer.toLocId ||
            this.editState == true
        ) {
            this.formValid = false;
        } else {
            this.formValid = true;
        }
    }

    openlookUpModal(type: string) {
        debugger
        if(this.LocValCheck==true){
            if (type == "fromLoc") {
                this.inventoryLookupTableModal.id = "";
                this.inventoryLookupTableModal.displayName = "";
                this.inventoryLookupTableModal.show("Location");
                this.type = type;
                this.rowData = [];
                this.totalQty = 0;
                this.totalItems = 0;
                this.transferDetailData.length = 0;
            }
            else if (type == "toLoc"){
                this.inventoryLookupTableModal.id = "";
                this.inventoryLookupTableModal.displayName = "";
                this.inventoryLookupTableModal.show("ToLocation");
                this.type = type;
                this.rowData = [];
                this.totalQty = 0;
                this.totalItems = 0;
                this.transferDetailData.length = 0;
            }
        }
       
    }

    checkLocValidTrasnfer() {
        if (this.transfer.fromLocId == this.transfer.toLocId) {
            this.notify.info(
                this.l("From Location And To Location Cannot Be Equal")
            );
            this.transfer.fromLocId = null;
            this.transfer.toLocId = null;
        }
    }

    checkQtyBeforeSave() {
        var checkQtyZero: boolean = false;
        this.transfer.transferDetailDto.forEach((val, index) => {
            if (val.qty == 0) {
                checkQtyZero = true;
            }
        });
        return checkQtyZero;
    }

    enterDate() {
        this.formValid = false;
    }
    leaveDate() {
        this.checkFormValid();
    }
    //===========================File Attachment=============================
    onBeforeUpload(event): void {
        debugger;
        this.uploadUrl =
            AppConsts.remoteServiceBaseUrl + "/DemoUiComponents/UploadFiles?";
        if (this.appId !== undefined)
            this.uploadUrl +=
                "APPID=" + encodeURIComponent("" + this.appId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl +=
                "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.transfer.docNo !== undefined)
            this.uploadUrl +=
                "DocID=" + encodeURIComponent("" + this.transfer.docNo) + "&";
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

    getNewCostCenter() {
        debugger;
        this.transfer.ccid = this.inventoryLookupTableModal.id;
        this.transfer.ccdesc = this.inventoryLookupTableModal.displayName;
    }
    openSelectCostCenterModal() {
        debugger;
        this.type = "CostCenter"
        this.inventoryLookupTableModal.id = this.transfer.ccid;
        this.inventoryLookupTableModal.displayName = this.transfer.ccdesc;
        this.inventoryLookupTableModal.show(this.type);
    }

    setCostCenterNull() {
        this.transfer.ccid = null;
        this.transfer.ccdesc = null;
    }
}
