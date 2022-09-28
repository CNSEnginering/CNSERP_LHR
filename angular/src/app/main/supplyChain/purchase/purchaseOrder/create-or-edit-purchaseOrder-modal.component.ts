import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    ɵpublishDefaultGlobalUtils,
} from "@angular/core";
import { ModalDirective, parseDate } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { CreateOrEditPOPOHeaderDto } from "../shared/dtos/popoHeader-dto";
import { CreateOrEditPOPODetailDto } from "../shared/dtos/popoDetails-dto";
import { PurchaseOrderDto } from "../shared/dtos/purchaseOrder-dto";
import { POPOHeadersService } from "../shared/services/popoHeader.service";
import { POPODetailsService } from "../shared/services/popoDetail.service";
import { PurchaseOrderServiceProxy } from "../shared/services/purchaseOrder.service";
import { GetDataService } from "../../inventory/shared/services/get-data.service";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { AppConsts } from "@shared/AppConsts";
import { Lightbox } from "ngx-lightbox";
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
// import { SaleEntryServiceProxy } from 'shared/services/sale';
import { SaleEntryServiceProxy } from '@app/main/supplyChain/sales/shared/services/saleEntry.service';
import { AgGridExtend } from "@app/shared/common/ag-grid-extend/ag-grid-extend";
import { ApprovalService } from "../../periodics/shared/services/approval-service.";
import { LogComponent } from "@app/finders/log/log.component";
import { param } from "jquery";
@Component({
    selector: "createOrEditPurchaseOrderModal",
    templateUrl: "./create-or-edit-purchaseOrder-modal.component.html",
})
export class CreateOrEditPurchaseOrderModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild("CommonServiceLookupTableModal", { static: true })
    CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    @ViewChild("InventoryLookupTableModal", { static: true })
    InventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild("LogTableModal", { static: true }) LogTableModal: LogComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    target: string;
    description = "";
    totalQty = 0;
    totalAmount = 0;
    netAmount = 0;
    totalItems = 0;
    venderTax = 0;
    private setParms;
    locations: any;
    LockDocDate: Date | String;
    isPoExist: boolean = false;
    chartofAccDesc: string;
    subAccDesc: string;
    whTermDesc: string;
    taxAuthorityDesc: string;
    taxClassDesc: string;
    status = "Incomplete";
    approved!: boolean | undefined;
    checkedval: boolean;
    LocCheckVal: boolean;
    popoHeader: CreateOrEditPOPOHeaderDto = new CreateOrEditPOPOHeaderDto();
    popoDetail: CreateOrEditPOPODetailDto = new CreateOrEditPOPODetailDto();
    purchaseOrder: PurchaseOrderDto = new PurchaseOrderDto();
    agGridExtend: AgGridExtend = new AgGridExtend();

    auditTime: Date;
    docDate: Date;
    processing = false;
    url: any;
    transType: number;
    transStatus: string;
    ItemPriceDesc: string;
    ItemPrice:string;

    constructor(
        injector: Injector,
        private _popoHeadersServiceProxy: POPOHeadersService,
        private _popoDetailServiceProxy: POPODetailsService,
        private _purchaseOrderServiceProxy: PurchaseOrderServiceProxy,
        private _getDataService: GetDataService,
        private _lightbox: Lightbox,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
        private _approvelService: ApprovalService,
        private _saleEntryServiceProxy: SaleEntryServiceProxy,
    ) {
        super(injector);
    }

    maxDocNo: number;

    approveDoc(id: number, mode, approve) {
        debugger;
        this.message.confirm("", (isConfirmed) => {
            if (isConfirmed) {
                this._approvelService
                    .ApprovalData("purchase", [id], mode, approve)
                    .subscribe(() => {
                        if (approve == true) {
                            this.notify.success(this.l("SuccessfullyApproved"));
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
        });
    }
    show(popoHeaderId?: number, maxId?: number): void {
        this.auditTime = null;
        this.url = null;
        this.uploadUrl = null;
        this.image = [];
        this.uploadedFiles = [];
        this.checkImage = true;

        debugger;

        this.getLocations("ICLocations");

        if (!popoHeaderId) {
            this.popoHeader = new CreateOrEditPOPOHeaderDto();
            this.popoHeader.id = popoHeaderId;
            this.popoHeader.docDate = new Date();
            this.popoHeader.arrivalDate = new Date();
            this.popoHeader.docNo = maxId;
            this.maxDocNo = maxId;
            this.popoHeader.locID = 0;
            this.popoHeader.totalQty = 0;
            this.popoHeader.taxAmount = 0;
            this.popoHeader.totalAmt = 0;
            this.popoHeader.taxRate = 0;
            this.popoHeader.taxAmount = 0;
            this.chartofAccDesc = "";
            this.subAccDesc = "";
            this.whTermDesc = "";
            this.totalItems = 0;

            this.active = true;
            this.modal.show();
        } else {
            this._popoHeadersServiceProxy
                .getICOPNHeaderForEdit(popoHeaderId)
                .subscribe((result) => {
                    this.popoHeader = result;
                    debugger;
                    this.popoHeader.docDate = new Date(result.docDate);
                    this.popoHeader.arrivalDate = new Date(result.docDate);
                    if (this.popoHeader.audtDate != undefined) {
                        this.auditTime = new Date(this.popoHeader.audtDate);
                    }
                    this._gltrHeadersServiceProxy
                        .getImage(this.requisitionAppId, result.docNo)
                        .subscribe((fileResult) => {
                            if (fileResult != null) {
                                this.url =
                                    "data:image/jpeg;base64," + fileResult;
                                const album = {
                                    src: this.url,
                                };
                                this.image.push(album);
                                this.checkImage = false;
                            }
                        });
                    this.chartofAccDesc = result.accDesc;
                    this.subAccDesc = result.subAccDesc;
                    this.whTermDesc = result.whTermDesc;
                    this.taxAuthorityDesc = result.taxAuthDesc;
                    this.taxClassDesc = result.taxClassDesc;
                    this.approved = result.approved;
                    if (result.completed == true) {
                        this.status = "Completed";
                    } else {
                        this.status = "InComplete";
                    }

                    this.LockDocDate = this.popoHeader.docDate;

                    this._popoDetailServiceProxy
                        .getPOPODData(popoHeaderId)
                        .subscribe((resultD) => {
                            debugger;
                            var rData = [];
                            var qty = 0;
                            var amount = 0;
                            var items = 0;
                            resultD["items"].forEach((element) => {
                                rData.push(element);
                                qty += element.qty;
                                amount += element.amount;
                                items += items + 1;
                                if (element.poDocNo == true) {
                                    this.isPoExist = element.poDocNo;
                                }
                            });
                            debugger;
                            this.rowData = [];
                            this.rowData = rData;

                            this.totalItems = items;
                            this.popoHeader.totalQty = qty;
                            this.popoHeader.totalAmt = amount;
                            this.venderTax =
                                (this.popoHeader.totalAmt *
                                    this.popoHeader.taxRate) /
                                100;
                            this.netAmount =
                                this.popoHeader.totalAmt +
                                this.popoHeader.taxAmount +
                                this.venderTax;
                        });

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    SetDefaultRecord(result: any) {
        console.log(result);
        this.popoHeader.locID = result.currentLocID;
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
    private rowSelection;
    private rowData;
    columnDefs = [
        {
            headerName: this.l("SrNo"),
            field: "srNo",
            sortable: true,
            width: 50,
            valueGetter: "node.rowIndex+1",
        },
        {
            headerName: this.l("ItemId"),
            field: "itemID",
            sortable: true,
            filter: true,
            width: 100,
            editable: (params) => (params.data.poDocNo == true ? false : true),
            resizable: true,
        },
        {
            headerName: this.l(""),
            field: "addItemId",
            width: 15,
            cellRenderer: this.addIconCellRendererFunc,
            resizable: false,
        },
        {
            headerName: this.l("Description"),
            field: "itemDesc",
            sortable: true,
            filter: true,
            width: 200,
            resizable: true,
        },
        {
            headerName: this.l("UOM"),
            field: "unit",
            sortable: true,
            filter: true,
            width: 80,
            editable: false,
            resizable: true,
        },
        // {headerName: this.l(''), field: 'addUOM',width:15,editable: false,cellRenderer: this.addIconCellRendererFunc,resizable: false},
        {
            headerName: this.l("Conversion"),
            field: "conver",
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
            editable: (params) => (params.data.poDocNo == true ? false : true),
        },
        {
            headerName: this.l("Qty"),
            field: "qty",
            editable: (params) => (params.data.poDocNo == true ? false : true),
            sortable: true,
            filter: true,
            width: 100,
            type: "numericColumn",
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
        {
            headerName: this.l("Rate"),
            field: "rate",
            editable: (params) => (params.data.poDocNo == true ? false : true),
            sortable: true,
            filter: true,
            width: 100,
            type: "numericColumn",
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
        {
            headerName: this.l("Amount"),
            field: "amount",
            sortable: true,
            editable: false,
            width: 100,
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
        {
            headerName: this.l("TaxAuth"),
            field: "taxAuth",
            sortable: true,
            width: 100,
            editable: false,
            resizable: true,
        },
        {
            headerName: this.l(""),
            field: "addTaxAuth",
            width: 15,
            cellRenderer: this.addIconCellRendererFunc,
            resizable: false,
        },
        {
            headerName: this.l("ClassId"),
            field: "taxClass",
            sortable: true,
            width: 100,
            editable: false,
            resizable: true,
        },
        {
            headerName: this.l(""),
            field: "addTaxClass",
            width: 15,
            cellRenderer: this.addIconCellRendererFunc,
            resizable: false,
        },
        {
            headerName: this.l("TaxClass"),
            field: "taxClassDesc",
            sortable: true,
            width: 100,
            editable: false,
            resizable: true,
        },
        {
            headerName: this.l("TaxRate"),
            field: "taxRate",
            sortable: true,
            width: 100,
            editable: (params) =>
                params.data.transType != 4 || params.data.poDocNo == true
                    ? false
                    : true,
            resizable: true,
        },
        {
            headerName: this.l("TaxAmt"),
            field: "taxAmt",
            sortable: true,
            width: 100,
            editable: (params) =>
                params.data.transType != 4 || params.data.poDocNo == true
                    ? false
                    : true,
            resizable: true,
        },
        {
            headerName: this.l("NetAmt"),
            field: "netAmount",
            sortable: true,
            width: 100,
            editable: false,
            resizable: true,
            valueFormatter: this.agGridExtend.formatNumber,
        },
        {
            headerName: this.l("Remarks"),
            field: "remarks",
            editable: (params) => (params.data.poDocNo == true ? false : true),
            resizable: true,
        },
        {
            headerName: this.l("ItemStatus"),
            field: "poDocNo",
            hide: true,
            editable: (params) => (params.data.poDocNo == true ? false : true),
            resizable: true,
        },
        {
            headerName: this.l("TransactionType"),
            field: "transType",
            hide: true,
            editable: false,
            resizable: true,
            maxLength: 8,
        },
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
        if (params.data.poDocNo == undefined) {
            params.data.poDocNo = false;
        }
        if (params.data.poDocNo == false) {
            switch (params.column["colId"]) {
                case "addItemId":
                    this.setParms = params;
                    this.openSelectItemModal();
                    break;
                case "addUOM":
                    this.setParms = params;
                    this.openSelectICUOMModal();
                    break;
                case "addTaxAuth":
                    this.setParms = params;
                    this.openSelectTaxAuthorityGridModal();
                    break;
                case "addTaxClass":
                    this.setParms = params;
                    this.openSelectTaxClassGridModal();
                    break;
                default:
                    break;
            }
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
            colKey: col,
        });
    }

    onRemoveSelected() {
        debugger;
        var selectedData = this.gridApi.getSelectedRows();
        if (selectedData[0].poDocNo == false) {
            this.gridApi.updateRowData({ remove: selectedData });
            this.gridApi.refreshCells();
            this.calculations();
        }
    }

    createNewRowData() {
        debugger;
        var newData = {
            itemID: "",
            itemDesc: "",
            unit: "",
            conver: "",

            qty: "0",
            rate: "0",
            amount: "0",

            taxAuth: "",
            taxClass: "",
            taxRate: "0",
            taxAmt: "0",
            netAmount: "0",
            remarks: "",
            transType: 0,
        };
        return newData;
    }

    calculations() {
        debugger;
        var items = 0;
        var qty = 0;
        var amount = 0;
        var taxAmt = 0;
        this.gridApi.forEachNode((node) => {
            debugger;
            if (
                (node.data.amount != "" || node.data.qty != "") &&
                node.data.itemID != ""
            ) {
                qty += parseFloat(node.data.qty);
                amount += parseFloat(node.data.amount);
            }
            items = items + 1;
            taxAmt += parseFloat(node.data.taxAmt);
        });
        this.totalItems = items;
        this.popoHeader.totalQty = qty;
        this.popoHeader.totalAmt = amount;
        this.popoHeader.taxAmount = taxAmt;
        this.venderTax = Math.round(
            (this.popoHeader.taxRate * this.popoHeader.totalAmt) / 100
        );
        this.netAmount =
            this.popoHeader.totalAmt +
            this.popoHeader.taxAmount +
            this.venderTax;
    }

    onCellValueChanged(params) {
        var colname = params.column["colId"];
        debugger;
        if (colname == "rate") {
            if (params.data.qty != null && params.data.rate != null) {
                params.data.amount =
                    parseFloat(params.data.qty) * parseFloat(params.data.rate);
                params.data.netAmount =
                    parseFloat(params.data.qty) * parseFloat(params.data.rate);
            }
        }

        // if (params.data.qty != null && params.data.rate != null) {
        //     params.data.amount =
        //         parseFloat(params.data.qty) * parseFloat(params.data.rate);
        //     params.data.netAmount =
        //         parseFloat(params.data.qty) * parseFloat(params.data.rate);
        // }

        if (colname == "taxRate") {
            params.data.taxAmt = Math.abs(
                (parseFloat(params.data.amount) *
                    parseFloat(params.data.taxRate)) /
                    100
            );
            params.data.netAmount =
                parseFloat(params.data.amount) + parseFloat(params.data.taxAmt); //- parseFloat(params.data.disc);
        }

        if (colname == "taxAmt") {
            params.data.taxRate = Math.abs(
                (parseFloat(params.data.taxAmt) * 100) /
                    parseFloat(params.data.amount)
            );
            params.data.netAmount =
                parseFloat(params.data.amount) + parseFloat(params.data.taxAmt); // - parseFloat(params.data.disc);
        }

        if (colname == "addTaxClass" && params.data.transType != 4) {
            params.data.taxAmt = Math.round(
                (parseFloat(params.data.amount) *
                    parseFloat(params.data.taxRate)) /
                    100
            );
            params.data.netAmount =
                parseFloat(params.data.amount) + parseFloat(params.data.taxAmt);
        }
        // if(params.data.taxRate!=null && params.data.taxAmt!=null){
        //     params.data.taxAmt=Math.round((parseFloat(params.data.amount)*parseFloat(params.data.taxRate))/100);
        //     params.data.netAmount=parseFloat(params.data.amount)+parseFloat(params.data.taxAmt);
        // }
        if (params.data.remarks.length > 150) {
            this.message.warn("Remarks Length", "Enter Only 150 Character!");

            var id = params.rowIndex;
            var rowNode = this.gridApi.getRowNode(id);
            debugger;
            var newData = {
                itemID: params.data.itemID,
                itemDesc: params.data.itemDesc,
                unit: params.data.unit,
                conver: params.data.conver,

                qty: params.data.qty,
                rate: params.data.rate,
                amount: params.data.amount,

                taxAuth: params.data.taxAuth,
                taxClass: params.data.taxClass,
                taxRate: params.data.taxRate,
                taxAmt: params.data.taxAmt,
                netAmount: params.data.netAmount,
                remarks: params.data.remarks.substring(0, 150),
                transType: params.data.transType,
            };
            rowNode.setData(newData);
            return;
        }
        this.calculations();
        this.gridApi.refreshCells();
    }

    //==================================Grid=================================

    getReqNoRecord(): void {
        debugger;
        if (this.popoHeader.reqNo == null) {
            this.message.warn(
                this.l("Please Enter Requistion No"),
                "Requistion No Required"
            );
        } else {
            this._purchaseOrderServiceProxy
                .pendingReqEntries(this.popoHeader.reqNo)
                .subscribe((result) => {
                    if (result.reqNo == undefined) {
                        this.message.info(
                            this.l(
                                "No record found for Requistion No " +
                                    this.popoHeader.reqNo
                            ),
                            "Empty Result"
                        );
                        return;
                    }
                    let temdocNo = this.popoHeader.docNo;
                    debugger;
                    this.popoHeader = result;

                    if (this.popoHeader.audtDate) {
                        this.auditTime = new Date(this.popoHeader.audtDate);
                    }

                    this.popoHeader.id = null;
                    this.popoHeader.docNo = temdocNo;
                    this.popoHeader.active = true;
                    this.chartofAccDesc = result.accDesc;
                    this.subAccDesc = result.subAccDesc;
                    this.whTermDesc = result.whTermDesc;
                    this.taxAuthorityDesc = result.taxAuthDesc;
                    this.taxClassDesc = result.taxClassDesc;
                    debugger;
                    this.popoHeader.docDate = new Date(result.docDate);
                    this.popoHeader.arrivalDate = new Date(result.docDate);
                    this.LockDocDate = this.popoHeader.docDate;

                    this._purchaseOrderServiceProxy
                        .pendingReqQty(this.popoHeader.reqNo)
                        .subscribe((resultD) => {
                            debugger;
                            var rData = [];
                            var qty = 0;
                            var amount = 0;
                            var items = 0;
                            resultD["items"].forEach((element) => {
                                rData.push(element);
                                qty += element.qty;
                                amount += element.amount;
                                items += items + 1;
                            });

                            this.rowData = [];
                            this.rowData = rData;

                            this.totalItems = items;
                            this.popoHeader.totalQty = qty;
                            this.popoHeader.totalAmt = amount;
                            this.venderTax =
                                (this.popoHeader.totalAmt *
                                    this.popoHeader.taxRate) /
                                100;
                            this.netAmount =
                                this.popoHeader.totalAmt +
                                this.popoHeader.taxAmount +
                                this.venderTax;
                        });

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        debugger;
        this.message.confirm("", (isConfirmed) => {
            if (isConfirmed) {
                // if(moment(this.popoHeader.docDate)>moment().endOf('day')){
                //     this.message.warn("Document date greater than current date","Document Date Error");
                //     return;
                // }

                // if(moment(this.popoHeader.arrivalDate)>moment().endOf('day')){
                //     this.message.warn("Arrival date greater than current date","Arrival Date Error");
                //     return;
                // }

                // if((moment(this.LockDocDate).month()+1)!=(moment(this.popoHeader.docDate).month()+1) && this.popoHeader.id!=null){
                //     this.message.warn('Document month not changeable',"Document Month Error");
                //     return;
                // }
                debugger;
                if (
                    this.popoHeader.locID == null ||
                    this.popoHeader.locID == 0
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

                if (
                    this.popoHeader.totalAmt <= 0 ||
                    this.popoHeader.totalQty <= 0
                ) {
                    this.message.warn(
                        "Qty OR Amount not less than OR equal to zero",
                        "Qty OR Amount Zero"
                    );
                    return;
                }

                this.saving = true;

                var rowData = [];
                this.gridApi.forEachNode((node) => {
                    rowData.push(node.data);
                });

                if (
                    moment(new Date()).format("A") === "AM" &&
                    !this.popoHeader.id &&
                    moment(new Date()).month() + 1 ==
                        moment(this.popoHeader.docDate).month() + 1
                ) {
                    this.popoHeader.docDate = this.popoHeader.docDate;
                } else {
                    this.popoHeader.docDate = this.popoHeader.docDate;
                }

                this.popoHeader.active = true;

                this.popoHeader.approved =
                    this.popoHeader.approved == null
                        ? false
                        : this.popoHeader.approved;

                this.purchaseOrder.popoDetail = rowData;
                this.purchaseOrder.popoHeader = this.popoHeader;

                this._purchaseOrderServiceProxy
                    .createOrEditPurchaseOrder(this.purchaseOrder)
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

    getLocations(target: string): void {
        debugger;
        this._getDataService.getList(target).subscribe((result) => {
            this.locations = result;
        });
    }

    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartOfAC();
                break;
            case "SubLedger":
                this.getNewSubAcc();
                break;
            case "WHTerm":
                this.getNewWHTerm();
                break;

            default:
                break;
        }
    }
    getNewCommonServiceModal() {
        switch (this.target) {
            case "TaxAuthority":
                this.getNewTaxAuthority();
                break;
            case "TaxAuthorityGrid":
                this.getNewTaxAuthorityGrid();
                break;
            case "TaxClass":
                this.getNewTaxClass();
                break;
            case "TaxClassGrid":
                this.getNewTaxClassGrid();
                break;
            case "RequsitionNo":
                this.getNewReq();
                break;
            default:
                break;
        }
    }

    getNewInventoryModal() {
        switch (this.target) {
            case "Items":
                this.getNewItemId();
                break;
            case "UOM":
                this.getNewICUOM();
                break;
                case "PriceList":
                    this.getNewPriceList();
                    break;
            default:
                break;
        }
    }

    OpenLog() {
        debugger;
        this.LogTableModal.show(this.popoHeader.docNo, "PurchaseOrder");
    }
    //=====================Chart of Ac Model================
    openSelectChartofACModal() {
        debugger;
        this.target = "ChartOfAccount";
        this.FinanceLookupTableModal.id = this.popoHeader.accountID;
        this.FinanceLookupTableModal.displayName = this.chartofAccDesc;
        this.FinanceLookupTableModal.show(this.target, "true");
    }
    openSelectReqNoModal() {
        debugger;
        this.target = "RequsitionNo";
        this.CommonServiceLookupTableModal.id = null;
        //this.FinanceLookupTableModal.displayName = this.popoHeader.reqNo.toString();
        this.CommonServiceLookupTableModal.show(this.target, "true");
    }
    setAccountIDNull() {
        this.popoHeader.accountID = "";
        this.chartofAccDesc = "";
        //this.setSubAccIDNull();
    }
    setReqnoNull() {
        this.popoHeader.reqNo = 0;
        //this.popoHeader.accountID = '';
        //this.chartofAccDesc = '';
        //this.setSubAccIDNull();
    }
    getNewChartOfAC() {
        debugger;
        if (this.FinanceLookupTableModal.id != this.popoHeader.accountID)
            this.setSubAccIDNull();
        this.popoHeader.accountID = this.FinanceLookupTableModal.id;
        this.chartofAccDesc = this.FinanceLookupTableModal.displayName;
    }
    //=====================Chart of Ac Model================

    //=====================Sub Account Model================
    openSelectSubAccModal() {
        if (
            this.popoHeader.accountID == "" ||
            this.popoHeader.accountID == null
        ) {
            this.message.warn(
                this.l("Please select account first"),
                "Account Required"
            );
            return;
        }
        this.target = "SubLedger";
        this.FinanceLookupTableModal.id = String(this.popoHeader.subAccID);
        this.FinanceLookupTableModal.displayName = this.subAccDesc;
        this.FinanceLookupTableModal.show(
            this.target,
            this.popoHeader.accountID
        );
    }

    setSubAccIDNull() {
        this.popoHeader.subAccID = null;
        this.subAccDesc = "";
    }

    getNewSubAcc() {
        debugger;
        this.popoHeader.subAccID =
            this.FinanceLookupTableModal.id == null ||
            this.FinanceLookupTableModal.id == "null"
                ? null
                : Number(this.FinanceLookupTableModal.id);
        this.subAccDesc = this.FinanceLookupTableModal.displayName;
    }
    //=====================Sub Account Model================

    //=====================Tax Authority Model================
    openSelectTaxAuthorityModal() {
        this.target = "TaxAuthority";
        this.CommonServiceLookupTableModal.id = this.popoHeader.taxAuth;
        this.CommonServiceLookupTableModal.displayName = this.taxAuthorityDesc;
        this.CommonServiceLookupTableModal.show(this.target);
    }

    setTaxAuthorityIdNull() {
        this.popoHeader.taxAuth = "";
        this.taxAuthorityDesc = "";
        this.setTaxClassIdNull();
    }

    getNewTaxAuthority() {
        if (this.CommonServiceLookupTableModal.id != this.popoHeader.taxAuth)
            this.setTaxClassIdNull();
        this.popoHeader.taxAuth = this.CommonServiceLookupTableModal.id;
        this.taxAuthorityDesc = this.CommonServiceLookupTableModal.displayName;
    }
    //=====================Tax Authority Model================

    //=====================Tax Class================
    openSelectTaxClassModal() {
        if (this.popoHeader.taxAuth == "" || this.popoHeader.taxAuth == null) {
            this.message.warn(
                this.l("Please select Tax authority first"),
                "Tax Authority Required"
            );
            return;
        }
        this.target = "TaxClass";
        this.CommonServiceLookupTableModal.id = String(
            this.popoHeader.taxClass
        );
        this.CommonServiceLookupTableModal.displayName = this.taxClassDesc;
        this.CommonServiceLookupTableModal.transType =
            this.setParms.data.transType;
        this.CommonServiceLookupTableModal.taxRate = this.popoHeader.taxRate;
        this.CommonServiceLookupTableModal.show(
            this.target,
            this.popoHeader.taxAuth
        );
    }
    getNewTaxClass() {
        debugger;
        this.target = "TaxClass";
        this.transType = this.CommonServiceLookupTableModal.transType;
        this.popoHeader.taxClass = Number(
            this.CommonServiceLookupTableModal.id
        );
        this.taxClassDesc = this.CommonServiceLookupTableModal.displayName;
        this.popoHeader.taxRate = this.CommonServiceLookupTableModal.taxRate;
        this.setParms.data.transType =
            this.CommonServiceLookupTableModal.transType;
        this.venderTax = Math.round(
            (this.popoHeader.taxRate * this.popoHeader.totalAmt) / 100
        );
        this.popoHeader.taxAmount = this.popoHeader.taxAmount + this.venderTax;
    }
    setTaxClassIdNull() {
        debugger;
        this.popoHeader.taxClass = null;
        this.taxClassDesc = "";
        this.popoHeader.taxRate = 0;
        this.setParms.data.transType = 0;
    }
    //=====================Tax Class================

    //=====================Tax Authority Grid Model================
    openSelectTaxAuthorityGridModal() {
        debugger;
        this.target = "TaxAuthority";
        this.CommonServiceLookupTableModal.id = this.setParms.data.taxAuth;
        this.CommonServiceLookupTableModal.show(this.target);
        this.target = "TaxAuthorityGrid";
    }

    setTaxAuthorityIdGridNull() {
        debugger;
        this.setParms.data.taxAuth = "";
        this.setTaxClassIdGridNull();
    }

    getNewTaxAuthorityGrid() {
        debugger;
        if (this.CommonServiceLookupTableModal.id != this.setParms.data.taxAuth)
            this.setTaxClassIdGridNull();
        this.setParms.data.taxAuth = this.CommonServiceLookupTableModal.id;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex, "addTaxClass");
    }
    //=====================Tax Authority Grid Model================

    //=====================Tax Class Grid================
    openSelectTaxClassGridModal() {
        if (
            this.setParms.data.taxAuth == "" ||
            this.setParms.data.taxAuth == null
        ) {
            this.message.warn(
                this.l(
                    "Please select Tax authority first at row " +
                        Number(this.setParms.rowIndex + 1)
                ),
                "Tax Authority Required"
            );
            return;
        }
        this.target = "TaxClass";
        this.CommonServiceLookupTableModal.id = String(
            this.setParms.data.taxClass
        );
        this.CommonServiceLookupTableModal.displayName =
            this.setParms.data.taxClassDesc;
        this.CommonServiceLookupTableModal.transType =
            this.setParms.data.transType;
        this.CommonServiceLookupTableModal.taxRate = this.setParms.data.taxRate;
        this.CommonServiceLookupTableModal.show(
            this.target,
            this.setParms.data.taxAuth
        );
        this.target = "TaxClassGrid";
    }
    getNewTaxClassGrid() {
        debugger;
        this.setParms.data.taxClass = Number(
            this.CommonServiceLookupTableModal.id
        );
        this.setParms.data.taxClassDesc =
            this.CommonServiceLookupTableModal.displayName;
        this.setParms.data.transType =
            this.CommonServiceLookupTableModal.transType;
        this.setParms.data.taxRate = this.CommonServiceLookupTableModal.taxRate;
        this.onBtStartEditing(this.setParms.rowIndex, "remarks");
        this.onCellValueChanged(this.setParms);
    }
    setTaxClassIdGridNull() {
        this.setParms.data.taxClass = "";
        this.setParms.data.taxClassDesc = "";
        this.setParms.data.taxRate = 0;
    }
    //=====================Tax Class Grid================

    //=====================Item Model================
    openSelectItemModal() {
        var CheckItemPrice = true;
        if($("#POEntry_ItemPrice").val()==""){
            this.message.warn(
                "Please Select Item Price " 

            );
            CheckItemPrice =false;
        }
        debugger;
         if (CheckItemPrice==true){
            this.target = "Items";
            this.InventoryLookupTableModal.id = this.setParms.data.itemID;
            this.InventoryLookupTableModal.displayName =this.setParms.data.itemDesc;
            this.InventoryLookupTableModal.unit = this.setParms.data.unit;
            this.InventoryLookupTableModal.conver = this.setParms.data.conver;
            this.InventoryLookupTableModal.show(this.target);
         }
    
    }

    setItemIdNull() {
        this.setParms.data.itemID = null;
        this.setParms.data.itemDesc = "";
        this.setParms.data.unit = "";
        this.setParms.data.conver = "";
    }

    getNewItemId() {
        debugger;

        var ConStatus = false;
        this.gridApi.forEachNode((node) => {
            debugger;
            if (node.data.itemID != "" && node.data.itemID != null) {
                if (node.data.itemID == this.InventoryLookupTableModal.id) {
                    this.message.warn(
                        "Item Has Already Exist At Row No " +
                            Number(node.rowIndex + 1),
                        "Item Duplicate!"
                    );
                    ConStatus = true;
                    return;
                }
            }
        });
        if (ConStatus == false) {
            debugger;
            this.setParms.data.itemID = this.InventoryLookupTableModal.id;
            this.setParms.data.itemDesc = this.InventoryLookupTableModal.displayName;
            this.setParms.data.unit = this.InventoryLookupTableModal.unit;
            this.setParms.data.conver = this.InventoryLookupTableModal.conver;
           
                this.getItemPriceRate(this.popoHeader.itemPrice, this.setParms.data.itemID);
            
          
            
            
            this.gridApi.refreshCells();
            this.onBtStartEditing(this.setParms.rowIndex, "qty");
        }
    }


    //================Item Model===============

    //=====================UOM Model================
    openSelectICUOMModal() {
        debugger;
        this.target = "UOM";
        this.InventoryLookupTableModal.unit = this.setParms.data.unit;
        this.InventoryLookupTableModal.conver = this.setParms.data.conver;
        this.InventoryLookupTableModal.show(this.target);
    }

    setICUOMNull() {
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
    //================UOM Model===============

    //=====================WH Term================
    openSelectWHTermModal() {
        this.target = "WHTerm";
        this.FinanceLookupTableModal.id = String(this.popoHeader.whTermID);
        this.FinanceLookupTableModal.displayName = this.whTermDesc;
        this.FinanceLookupTableModal.show(this.target);
    }
    getNewWHTerm() {
        debugger;
        this.popoHeader.whTermID = Number(this.FinanceLookupTableModal.id);
        this.whTermDesc = this.FinanceLookupTableModal.displayName;
    }
    getNewReq() {
        debugger;
        this.popoHeader.reqNo = Number(this.CommonServiceLookupTableModal.id);
        //this.whTermDesc = this.FinanceLookupTableModal.displayName;
    }

    setWHTermIDNull() {
        this.popoHeader.whTermID = null;
        this.whTermDesc = "";
    }
    //=====================WH Term================

    close(): void {
        this.active = false;
        this.modal.hide();
    }
    //===========================File Attachment=============================

    readonly requisitionAppId = 13;
    readonly appName = "PurchaseOrder";
    uploadUrl: string;
    checkImage: boolean = false;
    uploadedFiles: any[] = [];
    image: any[] = [];

    onBeforeUpload(event): void {
        debugger;
        this.uploadUrl =
            AppConsts.remoteServiceBaseUrl + "/DemoUiComponents/UploadFiles?";
        if (this.requisitionAppId !== undefined)
            this.uploadUrl +=
                "APPID=" + encodeURIComponent("" + this.requisitionAppId) + "&";
        if (this.appName !== undefined)
            this.uploadUrl +=
                "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.popoHeader.docNo !== undefined)
            this.uploadUrl +=
                "DocID=" + encodeURIComponent("" + this.popoHeader.docNo) + "&";
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


    getItemPriceRate(priceList, item) {
        debugger;
        this._saleEntryServiceProxy.getItemPriceRate(priceList, item).subscribe(result => {
       debugger;
        var test= result[0].purchasePrice;
            this.setParms.data.rate = test;
     
            // this.setParms.data.SalePrice=result;
            this.gridApi.refreshCells();
            
        });
       
}

 //=====================Price List Model================
 openSelectPriceListModal() {
     debugger;
    this.target = "PriceList";
    this.InventoryLookupTableModal.id = this.popoHeader.itemPrice;
    this.InventoryLookupTableModal.displayName = this.ItemPriceDesc;
    this.InventoryLookupTableModal.show(this.target);
}
getNewPriceList() {
    debugger;
    this.popoHeader.itemPrice = this.InventoryLookupTableModal.id;
    this.ItemPriceDesc = this.InventoryLookupTableModal.displayName;
}
setPriceListIDNull() {
    this.popoHeader.itemPrice = "";
    this.ItemPriceDesc = "";

}

//=====================Price List Model================
}