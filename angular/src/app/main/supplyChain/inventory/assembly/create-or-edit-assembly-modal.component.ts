import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit,
    ElementRef,
    AfterViewInit
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import { InventoryGlLinkLookupTableModalComponent } from "../FinderModals/InventoryGlLink-lookup-table-modal.component";
import { CostCenterLookupTableModalComponent } from "../FinderModals/costCenter-lookup-table-modal.component";
import { ItemPricingLookupTableModalComponent } from "../FinderModals/itemPricing-lookup-table-modal.component";
import { AssemblyDetailDto } from "../shared/dto/assemblyDetail-dto";
import { AssemblyDto } from "../shared/dto/assembly-dto";
import { AssemblyService } from "../shared/services/assembly.service";
import { throwIfEmpty } from "rxjs/operators";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
import { Lightbox } from "ngx-lightbox";
import { AppConsts } from "@shared/AppConsts";

@Component({
    selector: "AssemblyModal",
    templateUrl: "./create-or-edit-assembly-modal.component.html",
    styleUrls: ["./create-or-edit-assembly-modal.component.css"]
})
export class CreateOrEditAssemblyModalComponent extends AppComponentBase
    implements OnInit, AfterViewInit {
    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    // @ViewChild('ItemPricingLookupTableModal', { static: true }) ItemPricingLookupTableModal: ItemPricingLookupTableModalComponent;
    // @ViewChild('CostCenterLookupTableModal', { static: true }) CostCenterLookupTableModal: CostCenterLookupTableModalComponent;
    // @ViewChild('InventoryGlLinkLookupTableModal', { static: true }) InventoryGlLinkLookupTableModal: InventoryGlLinkLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    totalItems: number;
    editMode: boolean = false;
    totalQty: number;
    active = false;
    saving = false;
    priceListChk: boolean = false;
    assembly: AssemblyDto;
    assemblyDetail: AssemblyDetailDto;
    assemblyDetailData: AssemblyDetailDto[] = new Array<AssemblyDetailDto>();
    tabMode: any;
    gridApi;
    gridColumnApi;
    rowData;
    rowSelection;
    checkedval:boolean;
    paramsData;
    gridApi1;
    gridColumnApi1;
    gridApi2;
    gridColumnApi2;
    rowData1;
    rowSelection1;
    paramsData1;
    LocCheckVal:boolean;
    paramsData2;
    fgIndex: number = 0;
    rmIndex: number = 0;
    bpIndex: number = 0;
    type: string;
    editState: Boolean = false;
    editState1: Boolean = false;
    editState2: Boolean = false;
    appId = 11;
    appName = "AssemblyEntry";
    uploadedFiles = [];
    checkImage = true;
    image = [];
    url: string;
    uploadUrl: string;
    //private _lightbox: Lightbox;
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
            headerName: this.l("Qty"),
            editable: true,
            field: "qty",
            width: 100,
            resizable: true
        },
        // { headerName: this.l('LotNo'), editable: true, field: 'lotNo', sortable: true, width: 150, resizable: true },
        // { headerName: this.l('Bundle'), editable: true, field: 'bundle', sortable: true, width: 150, resizable: true },
        {
            headerName: this.l("Remarks"),
            editable: true,
            field: "remarks",
            sortable: true,
            width: 200,
            resizable: true
        }
        //  { headerName: this.l('Qty In Hand'), editable: false, field: 'maxQty', sortable: true, width: 200, resizable: true }
    ];
    formValid: boolean = false;
    constructor(
        injector: Injector,
        private _assemblyService: AssemblyService,
        private _lightbox: Lightbox,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy
    ) {
        super(injector);
        // this.gatePassDetailData.length = 0;
        // this.gatePassDetailDataTemp.length = 0;
    }
    ngOnInit(): void {
        this.rowData = [];
        this.rowData1 = [];
        this.tabMode = 7;
    }
    ngAfterViewInit() { }

    show(id?: number): void {
        this.active = true;
        this.assembly = new AssemblyDto();
        this.assemblyDetailData = new Array<AssemblyDetailDto>();

        //this.gatePassDetail = new GatePassDetailDto();
        //this.gatePassDetailData = new Array<GatePassDetailDto>();
        this.editMode = false;
        this.totalQty = 0;
        this.totalItems = 0;
        this.assembly.overHead = 0;
        this.tabMode = 7;
        this.formValid = false;

        this.url = null;
        this.image = [];
        this.uploadedFiles = [];
        this.uploadUrl = null;
        this.checkImage = true;

        if (!id) {
            this._assemblyService.GetDocId().subscribe(res => {
                this.assembly.docNo = res["result"];
                this.assembly.docDate = new Date();
            });
        } else {
            this.editMode = true;
            this._assemblyService.getDataForEdit(id).subscribe((data: any) => {
                this.assembly.id = data["result"]["assembly"]["id"];
                this.assembly.ordNo = data["result"]["assembly"]["ordNo"];
                this.assembly.docDate = new Date(
                    data["result"]["assembly"]["docDate"]
                );
                this.assembly.docNo = data["result"]["assembly"]["docNo"];
                this.assembly.narration =
                    data["result"]["assembly"]["narration"];
                this.assembly.locId = data["result"]["assembly"]["locID"];
                this.assembly.locDesc = data["result"]["assembly"]["locDesc"];
                this.assembly.overHead = data["result"]["assembly"]["overHead"];
                this.addRecordToGrid(
                    data["result"]["assembly"]["assemblyDetailDto"]
                );
                this.checkFormValid();
                //this.tabMode = 7;
                debugger;
                this._gltrHeadersServiceProxy
                    .getImage(this.appId, this.assembly.docNo)
                    .subscribe(fileResult => {
                        if (fileResult != null) {
                            this.url = "data:image/jpeg;base64," + fileResult;
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
    SetDefaultRecord(result:any){
        console.log(result);
          this.assembly.locId=result.currentLocID;
          this.assembly.locDesc=result.currentLocName;
          this.checkedval=result.cDateOnly;
          if(result.allowLocID==false){
              this.LocCheckVal=false;
          }else{
            this.LocCheckVal=true;
          }
          //this.typeDesc=result.transTypeName;
      }
    getIndex(transType) {
        if (transType == 7) return ++this.fgIndex;
        else if (transType == 9) return ++this.rmIndex;
        else if (transType == 8) return ++this.bpIndex;
    }

    addRecordToGrid(record: any) {
        if (this.checkItemInFinishGoods() != true) {
            if (this.tabMode == 7) this.editState = true;
            else if (this.tabMode == 9) this.editState1 = true;
            else if (this.tabMode == 8) this.editState2 = true;

            if (record != undefined) {
                record.forEach((val, index) => {
                    if (this.editMode == false) {
                        var str = val.itemId.split("*");
                        var newData = {
                            srNo: index,
                            item: str[0],
                            description: str[1],
                            unit: str[2],
                            conver: str[3],
                            qty: val.qty,
                            //lotNo: val.lotNo,
                            // bundle: val.bundle,
                            remarks: val.remarks
                            //  maxQty: val.qty
                        };
                        this.addOrUpdateRecordToDetailData(newData, "record");
                        if (this.tabMode == 7)
                            this.gridApi.updateRowData({ add: [newData] });
                        else if (this.tabMode == 9) this.gridApi1.updateRowData({ add: [newData] });
                        else if (this.tabMode == 8) this.gridApi2.updateRowData({ add: [newData] });
                    } else {
                        index = this.getIndex(val.transType);
                        var str = val.itemId.split("*");
                        var newData = {
                            srNo: index,
                            item: str[0],
                            description: str[1],
                            unit: str[2],
                            conver: str[3],
                            qty: val.qty,
                            //lotNo: val.lotNo,
                            // bundle: val.bundle,
                            remarks: val.remarks
                            //  maxQty: val.qty
                        };
                        if (val.transType == 7) {
                            this.tabMode = 7;
                            setTimeout(() => {
                                this.gridApi.updateRowData({ add: [newData] });
                            }, 2000);
                            //this.gridApi.updateRowData({ add: [newData] });
                        } else if (val.transType == 9) {
                            this.tabMode = 9;
                            setTimeout(() => {
                                this.gridApi1.updateRowData({ add: [newData] });
                            }, 2000);
                            // this.gridApi1.updateRowData({ add: [newData] });
                        }
                        else if (val.transType == 8) {
                            this.tabMode = 8;
                            setTimeout(() => {
                                this.gridApi2.updateRowData({ add: [newData] });
                            }, 2000);
                            // this.gridApi1.updateRowData({ add: [newData] });
                        }
                        this.addOrUpdateRecordToDetailData(newData, "record");
                    }
                });
                if (this.tabMode == 7) this.editState = false;
                else if (this.tabMode == 9) this.editState1 = false;
                else if (this.tabMode == 8) this.editState2 = false;

            } else {
                let length = this.assemblyDetailData.length;
                var newData = {
                    srNo: ++length,
                    item: undefined,
                    description: undefined,
                    unit: undefined,
                    conver: undefined,
                    qty: undefined,
                    //  lotNo: undefined,
                    //   bundle: undefined,
                    remarks: undefined
                    //  maxQty: undefined
                };
                this.addOrUpdateRecordToDetailData(newData, "record");
                if (this.tabMode == 7)
                    this.gridApi.updateRowData({ add: [newData] });
                else if (this.tabMode == 9) this.gridApi1.updateRowData({ add: [newData] });
                else if (this.tabMode == 8) this.gridApi2.updateRowData({ add: [newData] });
            }
        } else {
            this.notify.info("One Item Is Already Added To The Finsihed Goods");
        }
        this.checkFormValid();
    }

    checkItemInFinishGoods() {
        var checkItem = false;
        if (this.tabMode == 7) {
            this.assemblyDetailData.forEach((val, index) => {
                if (val.transType == 7) {
                    checkItem = true;
                }
            });
        }
        return checkItem;
    }

    enterDate() {
        this.formValid = false;
    }
    leaveDate() {
        this.checkFormValid();
    }
    addOrUpdateRecordToDetailData(data: any, type: string) {
        if (type == "record") {
            this.assemblyDetail = new AssemblyDetailDto();
            this.assemblyDetail.srNo = data.srNo;
            this.assemblyDetail.itemId = data.item;
            this.assemblyDetail.description = data.description;
            this.assemblyDetail.unit = data.unit;
            this.assemblyDetail.qty = data.qty;
            this.assemblyDetail.conver = data.conver;
            this.assemblyDetail.remarks = data.remarks;
            this.assemblyDetail.transType = this.tabMode;
            this.assemblyDetailData.push(this.assemblyDetail);
        } else {
            var filteredData = this.assemblyDetailData.find(
                x => x.srNo == data.srNo && x.transType == this.tabMode
            );
            if (filteredData.srNo != undefined) {
                filteredData.itemId = data.item;
                filteredData.remarks = data.remarks;
                filteredData.lotNo = data.lotNo;
                filteredData.bundle = data.bundle;
                filteredData.conver = data.conver;
                filteredData.description = data.description;
                filteredData.unit = data.unit;
                // if (Number(data.maxQty) < Number(data.qty)) {
                //     this.notify.info("Qty Cannot Be Greater Than Remaining " + data.maxQty + " Qty");
                //     data.qty = Number(data.maxQty);
                // }
                // else if (Number(data.qty) == 0) {
                //     this.notify.info("Qty Cannot Be Zero");
                //     data.qty = Number(data.maxQty);
                // }
                // else {
                //     filteredData.qty = data.qty;
                // }
                filteredData.qty = data.qty;
            }
        }

        this.totalItems = this.assemblyDetailData.length;
    }

    checkEditState() {
        if (this.tabMode == 7) {
            if (
                this.paramsData.data.item != "" &&
                this.paramsData.data.qty > 0
            ) {
                this.editState = false;
            } else {
                this.editState = true;
            }
        } else if (this.tabMode == 9) {
            if (
                this.paramsData1.data.item != "" &&
                this.paramsData1.data.qty > 0
            ) {
                this.editState1 = false;
            } else {
                this.editState1 = true;
            }
        }
        else if (this.tabMode == 8) {
            if (
                this.paramsData2.data.item != "" &&
                this.paramsData2.data.qty > 0
            ) {
                this.editState2 = false;
            } else {
                this.editState2 = true;
            }
        }
    }

    onGridReady(params) {
        this.rowData = [];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onGridReady1(params) {
        this.rowData1 = [];
        this.gridApi1 = params.api;
        this.gridColumnApi1 = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection1 = "multiple";
    }

    onGridReady2(params) {
        this.gridApi2 = params.api;
        this.gridColumnApi2 = params.columnApi;
        params.api.sizeColumnsToFit();
    }


    onRowDoubleClicked(params) {
        this.type = "item";
        //this.ItemPricingLookupTableModal.show("GatePassItem");
        this.inventoryLookupTableModal.show("Items");
        this.paramsData = params;
    }

    onRowDoubleClicked1(params) {
        this.type = "item";
        // this.ItemPricingLookupTableModal.show("GatePassItem");
        this.inventoryLookupTableModal.show("Items");
        this.paramsData1 = params;
    }

    onRowDoubleClicked2(params) {
        this.type = "item";
        // this.ItemPricingLookupTableModal.show("GatePassItem");
        this.inventoryLookupTableModal.show("Items");
        this.paramsData2 = params;
    }

    setTabMode(event) {
        if (event.srcElement.innerText == "Finished Goods") {
            this.tabMode = 7;
        } else if (event.srcElement.innerText == "Raw Materials") {
            this.tabMode = 9;
        } else if (event.srcElement.innerText == "Buy Products") {
            this.tabMode = 8;
        }
    }

    getLookUpData() {
        if (this.type == "loc") {
            // this.assembly.locId = this.InventoryGlLinkLookupTableModal.data.locID;
            // this.assembly.locDesc = this.InventoryGlLinkLookupTableModal.data.locName;
            this.assembly.locId =
                Number(this.inventoryLookupTableModal.id) == 0
                    ? undefined
                    : Number(this.inventoryLookupTableModal.id);
            this.assembly.locDesc = this.inventoryLookupTableModal.displayName;
        } else if (this.type == "item") {
            if (this.tabMode == 7) {
                this.paramsData.data.item = this.inventoryLookupTableModal.id;
                this.paramsData.data.description = this.inventoryLookupTableModal.displayName;
                this.paramsData.data.unit = this.inventoryLookupTableModal.unit;
                this.paramsData.data.conver = this.inventoryLookupTableModal.conver;
                // this.paramsData.data.item = this.ItemPricingLookupTableModal.data.itemId;
                // this.paramsData.data.description = this.ItemPricingLookupTableModal.data.descp;
                // this.paramsData.data.unit = this.ItemPricingLookupTableModal.data.stockUnit;
                // this.paramsData.data.conver = this.ItemPricingLookupTableModal.data.conver;
                this.gridApi.refreshCells();
                this.addOrUpdateRecordToDetailData(this.paramsData.data, "");
            } else if (this.tabMode == 9) {
                if (
                    this.checkItemForFinishGoods(
                        this.inventoryLookupTableModal.id
                    ) == false
                ) {
                    this.paramsData1.data.item = this.inventoryLookupTableModal.id;
                    this.paramsData1.data.description = this.inventoryLookupTableModal.displayName;
                    this.paramsData1.data.unit = this.inventoryLookupTableModal.unit;
                    this.paramsData1.data.conver = this.inventoryLookupTableModal.conver;
                    this.gridApi1.refreshCells();
                    this.addOrUpdateRecordToDetailData(
                        this.paramsData1.data,
                        ""
                    );
                }
            }
            else if (this.tabMode == 8) {
                if (
                    this.checkItemForFinishGoods(
                        this.inventoryLookupTableModal.id
                    ) == false
                ) {
                    this.paramsData2.data.item = this.inventoryLookupTableModal.id;
                    this.paramsData2.data.description = this.inventoryLookupTableModal.displayName;
                    this.paramsData2.data.unit = this.inventoryLookupTableModal.unit;
                    this.paramsData2.data.conver = this.inventoryLookupTableModal.conver;
                    this.gridApi2.refreshCells();
                    this.addOrUpdateRecordToDetailData(
                        this.paramsData2.data,
                        ""
                    );
                }
            }

            this.checkFormValid();
        }
        this.checkEditState();
        this.checkFormValid();
    }

    checkItemForFinishGoods(item) {
        var flag = false;
        this.assemblyDetailData.forEach((val, index) => {
            if (val.itemId == item && val.transType == 7) {
                this.notify.info(
                    this.l("This Item Already Exists In Finish Goods Grid")
                );
                flag = true;
            }
        });
        return flag;
    }

    cellValueChanged(params) {
        this.addOrUpdateRecordToDetailData(params.data, "");
        this.calculateTotalQty();
        // this.checkEditState();
        if (params.data.item != "" && params.data.qty > 0) {
            this.editState = false;
        } else {
            this.editState = true;
        }
        if (params.data.qty <= 0) {
            this.notify.info("Qty Should Be Greater Than Zero");
        }
        this.checkFormValid();
    }
    cellValueChanged1(params) {
        this.addOrUpdateRecordToDetailData(params.data, "");
        this.calculateTotalQty();
        // this.checkEditState();
        if (params.data.item != "" && params.data.qty > 0) {
            this.editState1 = false;
        } else {
            this.editState1 = true;
        }
        if (params.data.qty <= 0) {
            this.notify.info("Qty Should Be Greater Than Zero");
        }
        this.checkFormValid();
    }

    cellValueChanged2(params) {
        this.addOrUpdateRecordToDetailData(params.data, "");
        this.calculateTotalQty();
        // this.checkEditState();
        if (params.data.item != "" && params.data.qty > 0) {
            this.editState1 = false;
        } else {
            this.editState1 = true;
        }
        if (params.data.qty <= 0) {
            this.notify.info("Qty Should Be Greater Than Zero");
        }
        this.checkFormValid();
    }
    
    save() {
        this.message.confirm("Save", isConfirmed => {
            if (isConfirmed) {
                this.saving = true;
                this.assembly.assemblyDetailDto.push(
                    ...this.assemblyDetailData.slice()
                );
                this._assemblyService.create(this.assembly).subscribe(() => {
                    this.saving = false;
                    this.notify.info(this.l("SavedSuccessfully"));
                    this.close();
                    this.modalSave.emit(null);
                });
                this.close();
                this.tabMode=7;
            }
        });
    }

    calculateTotalQty() {
        let qty = 0;
        this.totalQty = 0;
        this.assemblyDetailData.forEach((val, index) => {
            qty = qty + Number(val.qty);
            if (!isNaN(qty)) this.totalQty = qty;
        });
        this.assemblyDetailData.length == 0 ? (this.totalQty = 0) : "";
        this.checkFormValid();
    }

    removeRecordFromGrid() {
        var selectedData;
        if (this.tabMode == 7) selectedData = this.gridApi.getSelectedRows();
        else if (this.tabMode == 9) selectedData = this.gridApi1.getSelectedRows();
        else if (this.tabMode == 8) selectedData = this.gridApi2.getSelectedRows();

        var filteredDataIndex = this.assemblyDetailData.findIndex(
            x => x.srNo == selectedData[0].srNo && x.transType == this.tabMode
        );
        this.assemblyDetailData.splice(filteredDataIndex, 1);
        if (this.tabMode == 7) {
            this.gridApi.updateRowData({ remove: selectedData });
            this.gridApi.refreshCells();
        } else if (this.tabMode == 9) {
            this.gridApi1.updateRowData({ remove: selectedData });
            this.gridApi1.refreshCells();
        } else if (this.tabMode == 8) {
            this.gridApi2.updateRowData({ remove: selectedData });
            this.gridApi2.refreshCells();
        }

        this.totalItems = this.assemblyDetailData.length;
        this.calculateTotalQty();
        this.editState = false;
        this.checkFormValid();
    }

    // dateChange(event: any) {
    //     this.assembly.docDate = event;
    //     this.checkFormValid();
    // }

    dateChange(event: any) {
        this.assembly.docDate = event;
        var currDate = new Date();
        if (this.assembly.docDate > currDate) {
            this.notify.info("You cannot enter the date after today");
        }
        this.checkFormValid();
    }

    checkFormValid() {
        if (
            this.assembly.docDate == null ||
            this.assembly.docDate > new Date() ||
            this.assembly.locId == undefined ||
            this.assembly.docNo == null ||
            this.assembly.docNo == undefined ||
            this.assemblyDetailData.length == 0 ||
            this.assemblyDetailData.length == 1 ||
            this.editState == true ||
            this.editState1 == true
        ) {
            this.formValid = false;
        } else {
            this.formValid = true;
        }
    }

    openlookUpModal() {
        if(this.LocCheckVal==true){
            this.type = "loc";
            this.inventoryLookupTableModal.show("Location");
        }
        
    }

    cellEditingStarted(params) {
        this.formValid = false;
    }

    cellEditingStarted1(params) {
        this.formValid = false;
    }
    cellEditingStarted2(params) {
        this.formValid = false;
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
        if (this.assembly.docNo !== undefined)
            this.uploadUrl +=
                "DocID=" + encodeURIComponent("" + this.assembly.docNo) + "&";
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
}
