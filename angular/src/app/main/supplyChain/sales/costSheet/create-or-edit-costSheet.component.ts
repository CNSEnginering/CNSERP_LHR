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
// import { InventoryGlLinkLookupTableModalComponent } from "../FinderModals/InventoryGlLink-lookup-table-modal.component";
// import { CostCenterLookupTableModalComponent } from "../FinderModals/costCenter-lookup-table-modal.component";
// import { ItemPricingLookupTableModalComponent } from "../FinderModals/itemPricing-lookup-table-modal.component";
import { saleQutationDetailDto } from "../shared/dtos/saleQutationDetail-dto";
import { Opt4LookupTableModalComponent } from '@app/main/supplyChain/inventory/FinderModals/opt4-lookup-table-modal.component';
import { saleQutationDto } from "../shared/dtos/saleQutation-dto";
import { saleQutationService } from "../shared/services/saleQutation.service";
import { throwIfEmpty } from "rxjs/operators";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
import { Lightbox } from "ngx-lightbox";
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
import { AppConsts } from "@shared/AppConsts";
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { TaxAuthoritiesComponent } from "@app/main/commonServices/taxAuthorities/taxAuthorities.component";
import { SalesLookupTableModalComponent } from "@app/finders/supplyChain/sales/sales-lookup-table-modal.component";
import * as moment from "moment";
import { costSheetDto } from "../shared/dtos/costSheet-dto";
import { costSheetDetailDto } from "../shared/dtos/costSheetDetail-dto";
import { costSheetService } from "../shared/services/costSheet.service";
import { convertAbpLocaleToAngularLocale } from "root.module";
@Component({
  selector: 'app-create-or-edit-costSheet',
  templateUrl: './create-or-edit-costSheet.component.html'
})
export class CreateOrEditCostSheetComponent extends AppComponentBase
implements OnInit, AfterViewInit {
    @ViewChild('SalesLookupTableModal', { static: true }) SalesLookupTableModal: SalesLookupTableModalComponent;
  @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild('CommonServiceLookupTableModal', { static: true }) CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    @ViewChild('opt4LookupTableModal', { static: true }) opt4LookupTableModal: Opt4LookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    totalItems: number;
    editMode: boolean = false;
    totalQty: number;
    active = false;
    saving = false;
    customerName:string;
    isButtonVisible = false;
    typeDesc:string;
    chAccountIDDesc:string;
    priceListChk: boolean = false;
    SaleQutationH: costSheetDto;
    SaleQutationD: costSheetDetailDto;
    SaleQutationD2: costSheetDetailDto;
    SaleQutationDData: costSheetDetailDto[] = new Array<costSheetDetailDto>();
    SaleQutationDData2: costSheetDetailDto[] = new Array<costSheetDetailDto>();
    tabMode: any;
    gridApi;
    private setParms;
    gridColumnApi;
    rowData;
    rowSelection;
    checkedval:boolean;
    paramsData;
    
   
    LocCheckVal:boolean;
    curID:string="";
    fgIndex: number = 0;
    rmIndex: number = 0;
    bpIndex: number = 0;
    type: string;
    editState: Boolean = false;
    
    appId = 11;
    appName = "OEQEntry";
    uploadedFiles = [];
    checkImage = true;
    image = [];
    target:string;
    locDesc:string;
    url: string;
    description:string;
    uploadUrl: string;
    taxAuthorityDesc: string;
    taxClassDesc: string;
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
            headerName: this.l("Trans Id"),
            editable: false,
            field: "transId",
            sortable: true,
            filter: true,
            width: 200,
            resizable: true,
        },
        { 
            headerName: this.l(''), 
            field: 'addTransId',
            filter: true, 
            width: 30, 
            editable: false, 
            resizable: false ,
            cellRenderer: this.addIconCellRendererFunc
        },
        {
            headerName: this.l("Trans Type"),
            editable: false,
            field: "transName",
            sortable: true,
            filter: true,
            width: 300,
            resizable: true
        },
        {
            headerName: this.l("Item"),
            editable: false,
            field: "itemID",
            sortable: true,
            filter: true,
            width: 300,
            resizable: true,
            
            
        },
        { 
            headerName: this.l(''), 
            field: 'addItemId',
            filter: true, 
            width: 30, 
            editable: false, 
            resizable: false ,
            cellRenderer: this.addIconCellRendererFunc
        },
        {
            headerName: this.l("Description"),
            editable: false,
            field: "description",
            sortable: true,
            filter: true,
            width: 300,
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
            width: 200,
            resizable: true
        },
        {
            headerName: this.l("Rate"),
            editable: true,
            field: "rate",
            width: 200,
            resizable: true
        },
        {
            headerName: this.l("Amount"),
            editable: false,
            field: "amount",
            width: 200,
            resizable: true
        },
        // { headerName: this.l('LotNo'), editable: true, field: 'lotNo', sortable: true, width: 150, resizable: true },
        // { headerName: this.l('Bundle'), editable: true, field: 'bundle', sortable: true, width: 150, resizable: true },
        { headerName: this.l('TAXAUTH'), field: 'taxAuth',sortable: true, width: 200, editable: false, resizable: true, visible:false },
        { headerName: this.l(''), field: 'addTaxAuth', width: 15,  visible:false, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        { headerName: this.l('ClassId'), field: 'taxClass', sortable: true, visible:false, width: 200, editable: false, resizable: true },
        { headerName: this.l(''), field: 'addTaxClass', width: 15, editable: false, cellRenderer: this.addIconCellRendererFunc, resizable: false },
        { headerName: this.l('TaxClass'), field: 'taxClassDesc', sortable: true, width: 200, editable: false, resizable: true },
        { headerName: this.l('TaxRate'), field: 'taxRate', sortable: true, width: 200, editable: false, resizable: true },
        { headerName: this.l('TaxAmt'), field: 'taxAmt', sortable: true, width: 200, editable: false, resizable: true },
        { headerName: this.l('NetAmt'), field: 'netAmount', sortable: true, width: 200, editable: false, resizable: true },
       
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
        private _SaleQutationService: costSheetService,
        private _lightbox: Lightbox,
        private _approvelService: ApprovalService,
      //  private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy
    ) {
        super(injector);
        // this.gatePassDetailData.length = 0;
        // this.gatePassDetailDataTemp.length = 0;
    }
    ngOnInit(): void {
        this.rowData = [];
    }
    addIconCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }
    ngAfterViewInit() { }
    openSelectCustomerModal() {
        if (this.SaleQutationH.typeID == null || this.SaleQutationH.typeID == "") {
            this.message.warn("Please select Sale Type", "Sale Type Required");
            return;
        }
        this.target = "Customer";
        this.type= "Customer";
        this.FinanceLookupTableModal.id = String(this.SaleQutationH.custID);
        this.FinanceLookupTableModal.displayName = this.customerName;
        this.FinanceLookupTableModal.show(this.target, this.SaleQutationH.typeID);
    }
   

    setCustomerIDNull() {
        this.SaleQutationH.custID = null;
        this.customerName = "";

    }
    openSelectChartofACModal(ac) {
        this.target = "ChartOfAccount";
        this.type="ChartOfAccount";
            this.FinanceLookupTableModal.id = this.SaleQutationH.salesCtrlAcc;
            this.FinanceLookupTableModal.displayName = this.chAccountIDDesc;
            this.FinanceLookupTableModal.show(this.target,"true");
       
        //this.target = ac;
    }
      //=====================Transaction Type Model================
      openSelectTransactionModal() {
        this.target = "TransactionType";
        this.type="TransactionType";
        this.inventoryLookupTableModal.id = this.SaleQutationH.typeID;
        this.inventoryLookupTableModal.displayName = this.typeDesc;
        this.inventoryLookupTableModal.show(this.target);
    }
    getNewTransaction() {
        debugger;
        this.SaleQutationH.typeID = this.inventoryLookupTableModal.id;
        this.typeDesc = this.inventoryLookupTableModal.displayName;
        this.setCustomerIDNull();
    }
    setTransactionIDNull() {
        this.SaleQutationH.typeID = "";
        this.typeDesc = "";
        this.setCustomerIDNull();
    }
    ///Approve And Unapprove
    approveDoc(id: number,mode, approve) {
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("SaleQuotation", [id], mode, approve)
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
    ///
    //=====================Transaction Type Model================
    show(id?: number): void {
        this.active = true;
        this.SaleQutationH = new costSheetDto();
        this.SaleQutationDData = new Array<costSheetDetailDto>();

        this.SaleQutationD = new costSheetDetailDto();
        this.SaleQutationDData = new Array<costSheetDetailDto>();
        this.editMode = false;
        this.totalQty = 0;
        this.totalItems = 0;
        //this.assembly.overHead = 0;
        this.tabMode = 0;
        this.formValid = false;

        this.url = null;
        this.image = [];
        this.uploadedFiles = [];
        this.uploadUrl = null;
        this.checkImage = true;
        this.locDesc=undefined;
        this.chAccountIDDesc=undefined;
        this.customerName=undefined;
        this.typeDesc=undefined;
        if (!id) {
            this._SaleQutationService.GetDocId().subscribe(res => {
                debugger
                this.SaleQutationH.docNo = res["result"];
                this.SaleQutationH.docDate = new Date();
                this.SaleQutationH.mDocDate = new Date();
            });
        } else {
            this.editMode = true;
            this._SaleQutationService.getDataForEdit(id).subscribe((data: any) => {
                debugger
                this.SaleQutationH.id = data["result"]["oecsh"]["id"];
               
                this.SaleQutationH=data["result"]["oecsh"];
                this.SaleQutationH.docDate = new Date(
                    data["result"]["oecsh"]["docDate"]
                );
                this.SaleQutationH.mDocDate = new Date(
                    data["result"]["oecsh"]["mDocDate"]
                );
                debugger
                this.addRecordToGrid(
                    data["result"]["oecsh"]["qutationDetailDto"]
                );
                this.locDesc=data["result"]["oecsh"]["locDesc"];
                this.typeDesc=data["result"]["oecsh"]["saleTypeDesc"];
                this.customerName=data["result"]["oecsh"]["customerDesc"];
                this.chAccountIDDesc=data["result"]["oecsh"]["chartofAccountDesc"];
                this.isButtonVisible= this.SaleQutationH.approved;
                this.checkFormValid();
               this.calculations();
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
          this.SaleQutationH.locID=result.currentLocID;
          this.locDesc=result.currentLocName;
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
        debugger
        this.editState = true;
            if (record != undefined) {
                record.forEach((val, index) => {
                    console.log(record);
                    var str = val.itemID.split("*");
                    var newData;
                    newData = {
                        srNo: index,
                        itemID: str[0],
                        description: str[1],
                        unit: val.unit,
                        rate:val.rate,
                        transId:val.transType,
                        transName:val.transName,
                        conver: val.conver,
                        qty: val.qty,
                        amount:val.rate*val.qty,
                        taxAuth:val.taxAuth,
                        taxClass:val.taxClass,
                        taxClassDesc:val.taxClassDesc,
                        taxRate:val.taxRate,
                        taxAmt:val.taxAmt,
                        netAmount:val.netAmount,
                        remarks:val.remarks
                    };
    
                    this.addOrUpdateRecordToDetailData(newData, "record");
                    this.gridApi.updateRowData({ add: [newData] });
                   
                });
                
                this.editState = false;
                this.checkFormValid();
            } else {
                let length = this.SaleQutationDData.length;
                var newData = {
                    srNo: ++length,
                    itemID: "",
                    description: "",
                    unit: "",
                    conver: "",
                    transId:"",
                    transName:"",
                    qty: '0',
                    rate: '0',
                    remarks: '',
                    amount:'0',
                    taxClassDesc:"",
                    taxAuth:"",
                    taxClass:"",
                    taxRate:'0',
                    taxAmt:'0',
                    netAmount:'0',
                    
                };
                this.addOrUpdateRecordToDetailData(newData, "record");
                this.gridApi.updateRowData({ add: [newData] });
               
            }
       
        this.checkFormValid();
    }

    checkItemInFinishGoods() {
        debugger
        var checkItem = false;
        if (this.tabMode == 7) {
            this.SaleQutationDData.forEach((val, index) => {
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
        debugger
        if (type == "record") {
            this.SaleQutationD = new costSheetDetailDto();
            this.SaleQutationD.srNo = data.srNo;
            this.SaleQutationD.itemID = data.itemID;
            this.SaleQutationD.description = data.description;
            this.SaleQutationD.unit = data.unit;
            this.SaleQutationD.qty = data.qty;
            this.SaleQutationD.conver = data.conver;
            this.SaleQutationD.remarks = data.remarks;
            this.SaleQutationD.amount=data.amount;
            this.SaleQutationD.rate=data.rate;
            this.SaleQutationD.taxAuth=data.taxAuth;
            this.SaleQutationD.taxClass=data.taxClass,
            this.SaleQutationD.taxClassDesc=data.taxClassDesc;
            this.SaleQutationD.taxRate=data.taxRate,
            this.SaleQutationD.taxAmt=data.taxAmt,
            this.SaleQutationD.netAmount=data.netAmount,
            this.SaleQutationD.transType=data.transId,
            this.SaleQutationD.transName=data.transName;
            this.SaleQutationDData.push(this.SaleQutationD);
        } else {
            var filteredData = this.SaleQutationDData.find(
                x => x.srNo == data.srNo 
            );
            if (filteredData.srNo != undefined) {
                
                filteredData.itemID = data.itemID;
                filteredData.remarks = data.remarks;
                filteredData.conver = data.conver;
                filteredData.description = data.description;
                filteredData.unit = data.unit;
                filteredData.rate=data.rate;
                filteredData.amount=data.amount;
                filteredData.transType=data.transId;
                filteredData.transName=data.transType;
                filteredData.qty = data.qty;
                filteredData.taxAuth=data.taxAuth;
                filteredData.taxClass=data.taxClass,
                filteredData.taxClassDesc=data.taxClassDesc;
                filteredData.taxRate=data.taxRate,
                filteredData.netAmount=data.netAmount,
                filteredData.taxAmt=data.taxAmt

            }
         
        }
        
        //this.totalItems = this.SaleQutationDData.length;
      
    }

   
    onGridReady(params) {
        this.rowData = [];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

  

 

    onRowDoubleClicked(params) {
        this.type = "item";
        //this.ItemPricingLookupTableModal.show("GatePassItem");
        this.inventoryLookupTableModal.show("Items");
        this.paramsData = params;
    }
    cellClicked(params) {
        debugger;
        switch (params.column["colId"]) {
            case "addItemId":
               debugger

                this.setParms = params;
                this.openSelectItemModal(params.data.transId);
                break;
            case "addTransId":
                this.setParms = params;
                this.openTransTypeModal();
                break;
            // case "addUOM":
            //     this.setParms = params;
            //     this.openSelectICUOMModal();
            //     break;
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
        this.checkFormValid();
    }

//=====================Sale No Model================

openSelectSaleNoModal() {
    if(this.SaleQutationH.locID==null || this.SaleQutationH.locID==0){
        this.message.warn("Please select location","Location Required");
        return;
    }
    this.target="SaleNo";
    this.SalesLookupTableModal.id =String(this.SaleQutationH.saleDoc);
    this.SalesLookupTableModal.show(this.target,String(this.SaleQutationH.locID));
}

setSaleNoNull() {
    this.SaleQutationH.saleDoc = "";
} 

getNewSaleNo() {
    debugger
    this.SaleQutationH.saleDoc =this.SalesLookupTableModal.id;
}
getSaleNoRecord():void{
    debugger;
    if(this.SaleQutationH.locID==null || this.SaleQutationH.locID==0){
        this.message.warn("Please select location","Location Required");
    }else if(this.SaleQutationH.saleDoc==null){ 
        this.message.warn(this.l("Please Enter or Pick Sale No"),'Sale No Required'); 
    }else{
        this.rowData = [];
        this._SaleQutationService.getSaleHeaderRecord(this.SaleQutationH.locID,Number(this.SaleQutationH.saleDoc)).subscribe((result: any) => {
            if(result["result"] != null)
            {
                //this.SaleQutationH.id = result["result"]["id"];
                this.SaleQutationH.typeID = result["result"]["typeID"];
                this.typeDesc = result["result"]["typeDesc"];
                this.SaleQutationH.custID = result["result"]["custID"];
                this.customerName = result["result"]["customerName"];
                this.SaleQutationH.narration = result["result"]["narration"];
                this.SaleQutationH.basicStyle = result["result"]["basicStyle"];
                this.SaleQutationH.license = result["result"]["license"];
                this.SaleQutationH.directCost = result["result"]["directCost"];
                this.SaleQutationH.saleDoc = result["result"]["docNo"];
                this.SaleQutationH.itemName = result["result"]["itemName"];
                this.SaleQutationH.orderQty = result["result"]["totalQty"];

                this.SaleQutationH.docDate = new Date();
                this.SaleQutationH.mDocDate = new Date();

                // this._SaleQutationService.getSaleDetailRecord(this.SaleQutationH.id).subscribe((details: any)=>{
                //     this.SaleQutationH.id = 0;
                //     this.addRecordToGrid(
                //         details["result"]["items"]
                //     );

                //     this.checkFormValid();
                //     this.calculations();
                // });


            }
            else{
                this.rowData=[];
                return;
            }
        });
    }
}
//=====================Sale No Model================


     //=====================Item Model================
     openSelectItemModal(data:any) {
        debugger;
        this.target = "ItemsQ";
        this.inventoryLookupTableModal.id = this.setParms.data.itemID;
        this.inventoryLookupTableModal.displayName = this.setParms.data.description;
        this.inventoryLookupTableModal.unit = this.setParms.data.unit;
        this.inventoryLookupTableModal.conver = this.setParms.data.conver;
        this.inventoryLookupTableModal.show(this.target,undefined,data);
    }
    openTransTypeModal() {
        debugger;
        this.target = "TransType";
         this.inventoryLookupTableModal.id = this.setParms.data.transId;
         this.inventoryLookupTableModal.displayName = this.setParms.data.transType;
        
        this.inventoryLookupTableModal.show(this.target);
    }



    setItemIdNull() {
        this.setParms.data.itemID = null;
        this.setParms.data.itemDesc = '';
        this.setParms.data.unit = '';
        this.setParms.data.conver = '';
    }


    getNewItemId() {
        debugger;
        this.setParms.data.itemID = this.inventoryLookupTableModal.id;
        this.setParms.data.description = this.inventoryLookupTableModal.displayName;
        this.setParms.data.unit = this.inventoryLookupTableModal.unit;
        this.setParms.data.conver = this.inventoryLookupTableModal.conver;
        // if (this.oesaleHeader.priceList != null && this.oesaleHeader.priceList != "") {
        //     this.getItemPriceRate(this.oesaleHeader.priceList, this.setParms.data.itemID);
        // }
        this.gridApi.refreshCells();
        this.addOrUpdateRecordToDetailData(this.setParms.data, "");
        this.onBtStartEditing(this.setParms.rowIndex, "qty");
    }
    getOpt4() {
        debugger
        this.setParms.data.transId = this.inventoryLookupTableModal.id;
        this.setParms.data.transName = this.inventoryLookupTableModal.displayName;
        this.gridApi.refreshCells();
        this.addOrUpdateRecordToDetailData(this.setParms.data, "");
    }
    onBtStartEditing(index, col) {
        debugger;
        this.gridApi.setFocusedCell(index, col);
        this.gridApi.startEditingCell({
            rowIndex: index,
            colKey: col
        });
    }
    //================Item Model===============


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
    this.setParms.data.taxAuth= '';
    this.setTaxClassIdGridNull();
}

getNewTaxAuthorityGrid() {
    if (this.CommonServiceLookupTableModal.id != this.setParms.data.taxAuth)
            this.setTaxClassIdGridNull();
        this.setParms.data.taxAuth = this.CommonServiceLookupTableModal.id;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex, "addTaxClass");
}
//=====================Tax Authority Grid Model================

//=====================Tax Class Grid================
openSelectTaxClassGridModal() {
    if (this.setParms.data.taxAuth == "" || this.setParms.data.taxAuth == null) {
        this.message.warn(this.l('Please select Tax authority first at row ' + Number(this.setParms.rowIndex + 1)), 'Tax Authority Required');
            return;
    }
    
    this.target = "TaxClass";
    this.CommonServiceLookupTableModal.id = String(this.setParms.data.taxClass);
    this.CommonServiceLookupTableModal.displayName = this.setParms.data.taxClassDesc;
    this.CommonServiceLookupTableModal.taxRate = this.setParms.data.taxRate;
    this.CommonServiceLookupTableModal.show(this.target, this.setParms.data.taxAuth);
    this.target = "TaxClassGrid";
}
getNewTaxClassGrid() {
    debugger;
    this.setParms.data.taxClass = Number(this.CommonServiceLookupTableModal.id);
    this.setParms.data.taxClassDesc = this.CommonServiceLookupTableModal.displayName;
    this.setParms.data.taxRate = this.CommonServiceLookupTableModal.taxRate;
    this.onBtStartEditing(this.setParms.rowIndex, "remarks");
    this.onCellValueChanged(this.setParms);
}
setTaxClassIdGridNull() {
    this.setParms.data.taxClass = '';
    this.setParms.data.taxClassDesc = '';
    this.setParms.data.taxRate = 0;
}
//


     



    onCellValueChanged(params) {
        debugger;
        if (params.data.qty != null && params.data.rate != null) {
            params.data.amount = parseFloat(params.data.qty) * parseFloat(params.data.rate);
        }
        if (params.data.taxRate != null && params.data.description != null) {
            params.data.taxAmt = Math.round((parseFloat(params.data.amount) * parseFloat(params.data.taxRate)) / 100);
            params.data.netAmount = parseFloat(params.data.amount) + parseFloat(params.data.taxAmt) ;
        }
        this.calculations();
        this.gridApi.refreshCells();
        this.addOrUpdateRecordToDetailData(params.data, "");
        this.checkFormValid();
    }
    calculations() {
        debugger;
        
        var amount = 0;
     
        this.gridApi.forEachNode(node => {
            debugger;
            if ((node.data.amount != "" || node.data.qty != "") && node.data.itemID != "" && node.data.netAmount != "") {
                
                amount += parseFloat(node.data.netAmount);
            }
           
        });
     
        this.SaleQutationH.directCost = amount;
        this.taxCalculation();
     }
     taxCalculation(){
        debugger
        var netAmount = this.SaleQutationH.directCost == null ? 0 : this.SaleQutationH.directCost;
        this.SaleQutationH.commRate = this.SaleQutationH.commRate == null ? 0 : this.SaleQutationH.commRate;
        this.SaleQutationH.ohRate = this.SaleQutationH.ohRate == null ? 0 : this.SaleQutationH.ohRate;
        this.SaleQutationH.taxRate = this.SaleQutationH.taxRate == null ? 0 : this.SaleQutationH.taxRate;
        this.SaleQutationH.profRate = this.SaleQutationH.profRate == null ? 0 : this.SaleQutationH.profRate;
        
        this.SaleQutationH.commAmt = parseFloat(((netAmount * this.SaleQutationH.commRate) / 100).toFixed(2));
        this.SaleQutationH.ohAmt = parseFloat(((netAmount * this.SaleQutationH.ohRate) / 100).toFixed(2));
        this.SaleQutationH.taxAmt = parseFloat(((netAmount * this.SaleQutationH.taxRate) / 100).toFixed(2));
        
        // Total Cost
        this.SaleQutationH.totalCost = parseFloat((netAmount + this.SaleQutationH.commAmt + this.SaleQutationH.ohAmt + this.SaleQutationH.taxAmt).toFixed(2));
        this.SaleQutationH.totalCost = this.SaleQutationH.totalCost == null ? 0 : this.SaleQutationH.totalCost;

        this.SaleQutationH.profAmt = parseFloat(((this.SaleQutationH.totalCost * this.SaleQutationH.profRate) / 100).toFixed(2));

        // Sale Price
        this.SaleQutationH.salePrice = parseFloat((this.SaleQutationH.totalCost + this.SaleQutationH.profAmt).toFixed(2));

        // Cost PerPiece
        this.SaleQutationH.costPP = parseFloat((this.SaleQutationH.totalCost / 12).toFixed(2));

        // Sale PerPiece
        this.SaleQutationH.salePP = parseFloat((this.SaleQutationH.salePrice / 12).toFixed(2));
        
        // Sale US
        if(this.SaleQutationH.usRate>0){
            this.SaleQutationH.saleUS = parseFloat((this.SaleQutationH.salePP / this.SaleQutationH.usRate).toFixed(2));
        }

     }
         CalculateTax(tax,amt){
           var amount=0;
           if(tax>0 && amt>0){
             amount=tax*amt/100;
           }
        return amount;
         }
    getNewInventoryModal() {
        debugger
        switch (this.target) {
            case "ItemsQ":
                this.getNewItemId();
                break;
           
            case "loc":
                this.SaleQutationH.locID =
                Number(this.inventoryLookupTableModal.id) == 0
                    ? undefined
                    : Number(this.inventoryLookupTableModal.id);
            this.locDesc = this.inventoryLookupTableModal.displayName;
                break;
            case "TransactionType":
                this.getNewTransaction();
                break;
            case "TransType":
                 this.getOpt4();
                 break;
            default:
                break;
        }
    }
    //=====================Tax Class Grid================
   
//=====================Currency Rate Model================
openSelectCurrencyRateModal() {
    debugger;
    this.target = "Currency";
    this.CommonServiceLookupTableModal.id = this.curID;
    this.CommonServiceLookupTableModal.currRate = this.SaleQutationH.usRate;
    this.CommonServiceLookupTableModal.show(this.target);
}


setCurrencyRateIdNull() {
    this.curID = '';
    this.SaleQutationH.usRate = 0;
}


getNewCurrencyRateId() {
    debugger;
    this.curID = this.CommonServiceLookupTableModal.id;
    this.SaleQutationH.usRate = this.CommonServiceLookupTableModal.currRate;
}
//================Currency Rate Model===============


    getLookUpData() {
        debugger
        if (this.type == "loc") {
            // this.assembly.locId = this.InventoryGlLinkLookupTableModal.data.locID;
            // this.assembly.locDesc = this.InventoryGlLinkLookupTableModal.data.locName;
            this.SaleQutationH.locID =
                Number(this.inventoryLookupTableModal.id) == 0
                    ? undefined
                    : Number(this.inventoryLookupTableModal.id);
            this.locDesc = this.inventoryLookupTableModal.displayName;
        } else if (this.type == "item") {
          debugger
                this.paramsData.data.itemID = this.inventoryLookupTableModal.id;
                this.paramsData.data.description = this.inventoryLookupTableModal.displayName;
                this.paramsData.data.unit = this.inventoryLookupTableModal.unit;
                this.paramsData.data.conver = this.inventoryLookupTableModal.conver;
                // this.paramsData.data.item = this.ItemPricingLookupTableModal.data.itemId;
                // this.paramsData.data.description = this.ItemPricingLookupTableModal.data.descp;
                // this.paramsData.data.unit = this.ItemPricingLookupTableModal.data.stockUnit;
                // this.paramsData.data.conver = this.ItemPricingLookupTableModal.data.conver;
                this.gridApi.refreshCells();
                this.addOrUpdateRecordToDetailData(this.paramsData.data, "");
            
              

            this.checkFormValid();
        }else if(this.type=="Customer"){
            this.getNewCustomer();
        }
        else if(this.type=="TransactionType"){
            this.getNewTransaction();
        }
        else if(this.type=="ChartOfAccount"){
            this.SaleQutationH.salesCtrlAcc = this.FinanceLookupTableModal.id;
            this.chAccountIDDesc = this.FinanceLookupTableModal.displayName;
        }
        
      
        this.checkFormValid();
    }
    getNewCommonServiceModal() {
        debugger
        switch (this.target) {
            case "TaxAuthorityGrid":
                this.getNewTaxAuthorityGrid();
                break;
            case "Currency":
                this.getNewCurrencyRateId();
                this.taxCalculation();
                break;
            case "TaxClassGrid":
                this.getNewTaxClassGrid();
                break;
            default:
                break;
        }
    }
    setCAIdNull(){
        this.SaleQutationH.salesCtrlAcc =null;
        this.chAccountIDDesc = "";
    }
    getNewCustomer() {
        debugger;
        if (this.FinanceLookupTableModal.id != "null") {
            this.SaleQutationH.custID = Number(this.FinanceLookupTableModal.id);
            this.customerName = this.FinanceLookupTableModal.displayName;
        }
    }
    checkItemForFinishGoods(item) {
        var flag = false;
        this.SaleQutationDData.forEach((val, index) => {
            if (val.itemID == item && val.transType == 7) {
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
       // this.calculateTotalQty();
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
    
    
    save() {
        this.message.confirm("Save", isConfirmed => {
            if (isConfirmed) {
                this.saving = true;
                this.SaleQutationH.qutationDetailDto=[];
                this.SaleQutationH.qutationDetailDto.push(
                    ...this.SaleQutationDData.slice()
                );
                this.SaleQutationH.saleDoc = this.SaleQutationH.saleDoc == null ? null : this.SaleQutationH.saleDoc.toString();
                this._SaleQutationService.create(this.SaleQutationH).subscribe(() => {
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
        this.SaleQutationDData.forEach((val, index) => {
            qty = qty + Number(val.qty);
            if (!isNaN(qty)) this.totalQty = qty;
        });
        this.SaleQutationDData.length == 0 ? (this.totalQty = 0) : "";
        this.checkFormValid();
    }

    removeRecordFromGrid() {
        

        var selectedData = this.gridApi.getSelectedRows();
        //this.gridApi.updateRowData({ remove: selectedData });
       // this.gridApi.refreshCells();

       debugger
        var filteredDataIndex = this.SaleQutationDData.findIndex(
            x => x.srNo == selectedData[0].srNo
        );
        this.SaleQutationDData.splice(filteredDataIndex, 1);
        this.gridApi.updateRowData({ remove: selectedData });
            this.gridApi.refreshCells();

        this.totalItems = this.SaleQutationDData.length;
        //this.calculateTotalQty();
        this.editState = false;
        this.checkFormValid();
    }

    // dateChange(event: any) {
    //     this.assembly.docDate = event;
    //     this.checkFormValid();
    // }

    dateChange(event: any) {
        this.SaleQutationH.docDate = event;
        var currDate = new Date();
        if (this.SaleQutationH.docDate > currDate) {
            this.notify.info("You cannot enter the date after today");
        }
        this.checkFormValid();
    }

    checkFormValid() {
        if (
            this.SaleQutationH.docDate == null ||
            this.SaleQutationH.docDate > new Date() ||
            this.SaleQutationH.locID == undefined ||
            this.SaleQutationH.docNo == null ||
            this.SaleQutationH.docNo == undefined ||
            this.SaleQutationDData.length == 0 
           
           // this.editState == true ||
          //  this.editState1 == true
        ) {
            this.formValid = false;
        } else {
            this.SaleQutationDData.forEach((val, index) => {
                debugger
                if(val.itemID==undefined || val.qty==undefined || val.transType==undefined){
                    this.formValid = false;
                }else{
                    this.formValid = true;
                   
                }
             })
          
        }
    }

   
    openlookUpModal() {
       // if(this.LocCheckVal==true){
        this.target="loc";
            this.type = "loc";
            this.inventoryLookupTableModal.show("Location");
       // }
        
    }

    cellEditingStarted(params) {
      //  this.formValid = false;
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
        if (this.SaleQutationH.docNo !== undefined)
            this.uploadUrl +=
                "DocID=" + encodeURIComponent("" + this.SaleQutationH.docNo) + "&";
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
