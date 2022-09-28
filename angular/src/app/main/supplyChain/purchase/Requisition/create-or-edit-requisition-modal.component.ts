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
import { RequisitionDto } from "../shared/dtos/requisition-dto";
import { RequisitionDetailDto } from "../shared/dtos/requisitionDetail-dto";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { RequisitionService } from "../shared/services/requisition.service";
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
import { Observable } from "rxjs/internal/Observable";
import { observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { id } from "@swimlane/ngx-charts/release/utils";
import { SlowBuffer } from "buffer";
import { AppConsts } from "@shared/AppConsts";
import { Lightbox } from 'ngx-lightbox';
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
import { AgGridExtend } from "@app/shared/common/ag-grid-extend/ag-grid-extend";
import { ApprovalService } from "../../periodics/shared/services/approval-service.";

@Component({
    selector: "RequisitionModal",
    templateUrl: "./create-or-edit-requisition-modal.component.html",
    styleUrls: ["./create-or-edit-requisition-modal.component.css"]
})
export class CreateOrEditRequisitionModalComponent extends AppComponentBase
    implements OnInit {
    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    agGridExtend: AgGridExtend = new AgGridExtend();
    totalItems: number;
    editMode: boolean = false;
    totalQty: number;
    active = false;
    saving = false;
    priceListChk: boolean = false;
    editState: boolean = false;
    errorFlag: boolean;
    gridApi;
    checkedval: boolean;
    gridColumnApi;
    rowData;
    LocCheckVal: boolean;
    rowSelection;
    paramsData;
    target: string;
    type: string;
    requisition: RequisitionDto;
    requisitionDetail: RequisitionDetailDto;
    requisitionDetailData: RequisitionDetailDto[] = new Array<
        RequisitionDetailDto
    >();
    columnDefs = [
        {
            headerName: this.l("SrNo"),
            editable: false,
            field: "srNo",
            sortable: true,
            width: 100,
            valueGetter: "node.rowIndex+1"
        },
        // {
        //     headerName: this.l("Trans Id"),
        //     editable: false,
        //     field: "transId",
        //     sortable: true,
        //     filter: true,
        //     width: 100,
        //     resizable: true,
        // },
        // { 
        //     headerName: this.l(''), 
        //     field: 'addTransId',
        //     filter: true, 
        //     width: 30, 
        //     editable: false, 
        //     resizable: false ,
        //    // cellRenderer: this.addIconCellRendererFunc
        // },
        // {
        //     headerName: this.l("Trans Type"),
        //     editable: false,
        //     field: "transName",
        //     sortable: true,
        //     filter: true,
        //     width: 250,
        //     resizable: true
        // },
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
            width: 200,
            resizable: true
        },
        {
            headerName: this.l("UOM"),
            editable: false,
            field: "unit",
            sortable: true,
            filter: true,
            width: 100,
            resizable: true
        },
        {
            headerName: this.l("Conver"),
            editable: false,
            field: "conver",
            sortable: true,
            filter: true,
            width: 150,
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
        {
            headerName: this.l("QtyInHand"),
            editable: false,
            field: "qtyInHand",
            width: 100,
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber
        },
        {
            headerName: this.l("Qty In PO"),
            editable: false,
            field: "qtyInPo",
            width: 150,
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber
        },
        {
            headerName: this.l("Comments"),
            editable: true,
            field: "comments",
            sortable: true,
            width: 150,
            resizable: true
        },
        {
            headerName: this.l("SubCostCenter"),
            editable: false,
            field: "subCostCenter",
            sortable: true,
            width: 180,
            resizable: true
        }
        //{ headerName: this.l('MaxQty'), editable: false, field: 'maxQty', sortable: true, width: 150, resizable: true }
    ];
    formValid: boolean = false;
    url: any;
    constructor(
        injector: Injector,
        private _service: RequisitionService,
        private _lightbox: Lightbox,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
        private _approvelService: ApprovalService,
    ) {
        super(injector);
    }
    addIconCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }
    show(id?: number, type?: string): void {
        this.active = true;
        this.editMode = false;
        this.totalQty = 0;
        this.totalItems = 0;
        this.requisition = new RequisitionDto();
        this.requisition.active = true;
        this.requisitionDetail = new RequisitionDetailDto();
        this.requisitionDetailData = new Array<RequisitionDetailDto>();
        this.formValid = false;

        this.url = null;
        this.uploadUrl = null;
        this.image = [];
        this.uploadedFiles = [];
        this.checkImage = true;
        this.getDocNo();

        var newdate = new Date();
        newdate.setDate(newdate.getDate() + 7);
        this.requisition.expArrivalDate = newdate;
        if (!id) {
            setTimeout(() => {
                this.requisition.docDate = new Date();
            }, 1000);
        } else {
            this.editMode = true;
            this._service.getDataForEdit(id, type).subscribe((data: any) => {
                this.requisition.id = data["result"]["requisition"]["id"];
                this.requisition.ordNo = data["result"]["requisition"]["ordNo"];
                this.requisition.approved = data["result"]["requisition"]["approved"];
                this.requisition.posted = data["result"]["requisition"]["posted"];
                this.requisition.hold = data["result"]["requisition"]["hold"];
                this.requisition.active = data["result"]["requisition"]["active"];
                this.requisition.docDate = new Date(
                    data["result"]["requisition"]["docDate"]
                );

                this._gltrHeadersServiceProxy.getImage(this.requisitionAppId, data["result"]["requisition"]["docNo"]).subscribe(fileResult => {
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

                this.requisition.docNo = data["result"]["requisition"]["docNo"];
                this.requisition.narration =
                    data["result"]["requisition"]["narration"];
                this.requisition.locID = data["result"]["requisition"]["locID"];
                this.requisition.ccid = data["result"]["requisition"]["ccid"];
                this.requisition.locName =
                    data["result"]["requisition"]["locName"];
                this.requisition.costCenterName =
                    data["result"]["requisition"]["ccName"];
                this.requisition.reqNo = data["result"]["requisition"]["reqNo"];
                this.requisition.expArrivalDate = new Date(
                    data["result"]["requisition"]["expArrivalDate"]
                );
                this.addRecordToGrid(
                    data["result"]["requisition"]["requisitionDetailDto"]
                );
                this.checkFormValid();
            });
        }
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
    SetDefaultRecord(result: any) {
        console.log(result);
        this.requisition.locID = result.currentLocID;
        this.requisition.locName = result.currentLocName;
        this.checkedval = result.cDateOnly;
        if (result.allowLocID == false) {
            this.LocCheckVal = false;
        } else {
            this.LocCheckVal = true;
        }
        //this.typeDesc=result.transTypeName;
    }

    addRecordToGrid(record: any) {
        debugger;
        this.editState = true;
        if (record != undefined) {
            record.forEach((val, index) => {
                console.log(record);
                var str = val.itemID.split("*");
                var newData;
                newData = {
                    srNo: index,
                    item: str[0],
                    description: str[1],
                    unit: val.unit,
                    conver: val.conver,
                    transId: val.transId,
                    transName: val.transName,
                    qty: val.qty,
                    comments: val.remarks,
                    subCostCenter: val.subccid,
                    qtyInHand: val.qih,
                    qtyInPo:val.qtyInPo
                    //maxQty: val.qty
                };

                this.addOrUpdateRecordToDetailData(newData, "record");
                setTimeout(() => {
                    this.gridApi.updateRowData({ add: [newData] });
                }, 2000);
                //this.gridApi.updateRowData({ add: [newData] });
            });
            this.editState = false;
            this.checkFormValid();
        } else {
            let length = this.requisitionDetailData.length;
            var newData = {
                srNo: ++length,
                item: undefined,
                description: undefined,
                unit: undefined,
                conver: undefined,
                qty: 0,
                comments: undefined,
                subCostCenter: 0,
                qtyInHand: 0,
                qtyInPo:0
                // maxQty: undefined
            };
            this.addOrUpdateRecordToDetailData(newData, "record");

            this.gridApi.updateRowData({ add: [newData] });
        }
        this.checkFormValid();
    }

    addOrUpdateRecordToDetailData(data: any, type: string) {
        if (type == "record") {
            this.requisitionDetail = new RequisitionDetailDto();
            this.requisitionDetail.srNo = data.srNo;
            this.requisitionDetail.itemId = data.item;
            this.requisitionDetail.description = data.description;
            this.requisitionDetail.qty = data.qty;
            this.requisitionDetail.transId = data.transId;
            this.requisitionDetail.conver = data.conver;
            this.requisitionDetail.transName = data.transName;
            this.requisitionDetail.unit = data.unit;
            this.requisitionDetail.remarks = data.comments;
            this.requisitionDetail.subCCId = data.subCostCenter;
            this.requisitionDetail.qih = data.qtyInHand;
            this.requisitionDetailData.push(this.requisitionDetail);
        } else {
            var filteredData = this.requisitionDetailData.find(
                x => x.srNo == data.srNo
            );
            if (filteredData.srNo != undefined) {
                filteredData.itemId = data.item;
                filteredData.remarks = data.comments;
                filteredData.transName = data.transName;
                filteredData.conver = data.conver;
                filteredData.unit = data.unit;
                filteredData.transId = data.transId;
                filteredData.description = data.description;
                filteredData.qty = data.qty;
                filteredData.subCCId = data.subCostCenter;
                filteredData.qih = data.qtyInHand;
            }
        }
        this.totalItems = this.requisitionDetailData.length;
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
        this.paramsData = params;
        this.inventoryLookupTableModal.id = "";
        this.inventoryLookupTableModal.displayName = "";
        if (params.colDef.field == "item") {
            this.type = "Item";
            this.inventoryLookupTableModal.show("Items", "", this.paramsData.data.transId);
        } else if (params.colDef.field == "subCostCenter") {
            this.type = "SubCostCenter";
            this.inventoryLookupTableModal.show(
                "SubCostCenter",
                this.requisition.ccid
            );
        } else if (params.colDef.field == "transId") {

            this.type = "TransType";
            this.inventoryLookupTableModal.show("TransType.");

        }
    }

    getDocNo() {
        this._service.GetDocId().subscribe((data: any) => {
            this.requisition.docNo = data["result"];
            this.checkFormValid();
        });
    }
    openModal(type: string) {

        this.type = type;
        this.inventoryLookupTableModal.id = undefined;
        this.inventoryLookupTableModal.displayName = "";
        if (type == "CostCenter") {
            this.inventoryLookupTableModal.show("CostCenter");
        }
        else if (type == "Location") {
            if (this.LocCheckVal == true) {
                this.inventoryLookupTableModal.show("Location");
            }
        }

    }

    cellValueChanged(params) {
        this.addOrUpdateRecordToDetailData(params.data, "");
        this.calculateTotalQty();
        this.checkEditState();
        if (params.data.qty <= 0) {
            this.message.info("Qty Should Be Greater Than Zero");
        }
        this.checkFormValid();
        //this.CheckOnQtyInHand();
    }

    cellEditingStarted(params) {
        this.formValid = false;
    }

    checkEditState() {
        if (
            this.paramsData.data.item != "" &&
            this.paramsData.data.qty > 0
            // && this.paramsData.data.subCostCenter > 0
        ) {
            this.editState = false;
        } else {
            this.editState = true;
        }
    }

    // CheckOnQtyInHand() {

    //     this.gridApi.forEachNode(node => {
    //         debugger

    //         if (parseInt(node.data.qty) > parseInt(node.data.maxQty)) {
    //             this.errorFlag=true;
    //             this.message.error("Qty Not Greater than Qty In Hand at Row No."+ Number(node.rowIndex+1),"Qty Greater");
    //             throw new Error();
    //         } else {
    //             this.errorFlag = false;

    //         }
    //     });
    // }
    approveDoc(id: number,mode, approve) {
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("requisitions", [id], mode, approve)
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
                    this._approvelService.ApprovalData("requisitions", [id], mode, posting)
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

    save() {
        debugger
        //if (!this.errorFlag) {
            this.requisition.totalQty = this.totalQty;
            this.requisition.RequisitionDetailDto.length = 0;
            this.requisition.RequisitionDetailDto.push(
                ...this.requisitionDetailData.slice()
            );
            this.saving = true;
            this._service.create(this.requisition).subscribe(() => {
                this.saving = false;
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
            this.close();
            // this.checkQty().subscribe(data => {
            //     if (data == false) {

            //     } else {
            //         this.notify.info("Qty Should Be Greater Than Zero");
            //     }
            // });
        // } else {
        //     this.message.warn("Qty not greater than Qty in Hand ", "Qty Greater");
        // }
    }
    dateExpChange(event: any) {
        this.requisition.expArrivalDate = event;
        var chk = this.requisition.docDate > event;
        if (chk)
            this.notify.info(
                "Expected Arrival Date Always Be Greater Than Current Date"
            );

        this.checkFormValid();
    }
    calculateTotalQty() {
        let qty = 0;
        this.requisitionDetailData.forEach((val, index) => {
            qty = qty + Number(val.qty);
            if (!isNaN(qty)) {
                this.totalQty = qty;
                this.requisition.totalQty = qty;
            }
        });
        this.requisitionDetailData.length == 0 ? (this.totalQty = 0) : "";
        this.checkFormValid();
    }

    removeRecordFromGrid() {
        var selectedData = this.gridApi.getSelectedRows();
        var filteredDataIndex = this.requisitionDetailData.findIndex(
            x => x.srNo == selectedData[0].srNo
        );
        this.requisitionDetailData.splice(filteredDataIndex, 1);
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        this.totalItems = this.requisitionDetailData.length;
        this.calculateTotalQty();
        //this.checkEditState();
        this.editState = false;
        this.checkFormValid();
    }

    checkFormValid() {
        if (
            this.requisition.docDate == null ||
            this.requisition.docNo == undefined ||
            this.requisitionDetailData.length == 0 ||
            this.totalQty == 0 ||
            this.editState == true ||
            this.requisition.ccid == "" ||
            this.requisition.locID == undefined ||
            this.requisition.locID == 0 ||
            this.requisition.docDate > this.requisition.expArrivalDate
        ) {
            this.formValid = false;
        } else {
            this.formValid = true;
        }
    }

    dateChange(event: any) {
        this.requisition.docDate = event;
        //this.dateExpChange();
        var chk = this.requisition.docDate > this.requisition.expArrivalDate;
        if (chk)
            this.notify.info(
                "Expected Arrival Date Always Be Greater Than Current Date"
            );

        this.checkFormValid();
    }

    getLookUpData() {
        if (this.type == "CostCenter") {
            this.requisition.ccid = this.inventoryLookupTableModal.id;
            this.requisition.costCenterName = this.inventoryLookupTableModal.displayName;
        } else if (this.type == "Item") {
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
            this._service
                .GetQtyInHand(
                    this.paramsData.data.item,
                    this.requisition.locID,
                    0
                )
                .subscribe(data => {
                    this.paramsData.data.qtyInHand = data["result"];
                    this.gridApi.refreshCells();
                    this.addOrUpdateRecordToDetailData(
                        this.paramsData.data,
                        ""
                    );
                    this.checkEditState();
                    this.checkFormValid();
                });
            }
        } else if (this.type == "SubCostCenter") {
            this.paramsData.data.subCostCenter = this.inventoryLookupTableModal.id;
        } else if (this.type == "Location") {
            if (!isNaN(+this.inventoryLookupTableModal.id))
                this.requisition.locID = +this.inventoryLookupTableModal.id;

            this.requisition.locName = this.inventoryLookupTableModal.displayName;
        } else if (this.type == "TransType") {
            this.paramsData.data.transId = this.inventoryLookupTableModal.id;
            this.paramsData.data.transName = this.inventoryLookupTableModal.displayName;
        }
        this.gridApi.refreshCells();
        this.addOrUpdateRecordToDetailData(this.paramsData.data, "");
        this.checkEditState();
        this.checkFormValid();
    }

    getData() {
        this.getLookUpData();
    }

    checkQty() {
        var checkQty = false;
        this.requisitionDetailData.forEach((val, index) => {
            if (val.qty <= 0) checkQty = true;
        });
        return of(checkQty);
    }

    //===========================File Attachment=============================

    readonly requisitionAppId = 12;
    readonly appName = "PurchaseRequisition";
    uploadUrl: string;
    checkImage: boolean = false;
    uploadedFiles: any[] = [];
    image: any[] = [];

    onBeforeUpload(event): void {
        debugger;
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles?';
        if (this.requisitionAppId !== undefined)
            this.uploadUrl += "APPID=" + encodeURIComponent("" + this.requisitionAppId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl += "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.requisition.docNo !== undefined)
            this.uploadUrl += "DocID=" + encodeURIComponent("" + this.requisition.docNo) + "&";
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
}
