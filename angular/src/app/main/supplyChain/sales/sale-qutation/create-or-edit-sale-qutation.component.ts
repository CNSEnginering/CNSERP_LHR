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
import { LogComponent } from "@app/finders/log/log.component";
@Component({
  selector: 'app-create-or-edit-sale-qutation',
  templateUrl: './create-or-edit-sale-qutation.component.html'
})
export class CreateOrEditSaleQutationComponent extends AppComponentBase
implements OnInit, AfterViewInit {
    @ViewChild('SalesLookupTableModal', { static: true }) SalesLookupTableModal: SalesLookupTableModalComponent;
  @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    
 @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
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
    SaleQutationH: saleQutationDto;
    SaleQutationD: saleQutationDetailDto;
    SaleQutationD2: saleQutationDetailDto;
    SaleQutationDData: saleQutationDetailDto[] = new Array<saleQutationDetailDto>();
    SaleQutationDData2: saleQutationDetailDto[] = new Array<saleQutationDetailDto>();
    tabMode: any;
    gridApi;
    private setParms;
    gridColumnApi;
    rowData;
    rowSelection;
    checkedval:boolean;
    paramsData;
    
   
    LocCheckVal:boolean;
    
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
        private _SaleQutationService: saleQutationService,
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
    approveDoc(id:any, mode:any, approve :any) {
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("Quotation", [id], mode, approve)
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
    OpenLog(){
        debugger
       this.LogTableModal.show(this.SaleQutationH.docNo,'Quotation');
    }
    show(id?: number): void {
        this.active = true;
        this.SaleQutationH = new saleQutationDto();
        this.SaleQutationDData = new Array<saleQutationDetailDto>();

        this.SaleQutationD = new saleQutationDetailDto();
        this.SaleQutationDData = new Array<saleQutationDetailDto>();
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
               
                this.SaleQutationH.id = data["result"]["oeqh"]["id"];
               
                this.SaleQutationH=data["result"]["oeqh"];
                this.SaleQutationH.docDate = new Date(
                    data["result"]["oeqh"]["docDate"]
                );
                this.SaleQutationH.mDocDate = new Date(
                    data["result"]["oeqh"]["mDocDate"]
                );
                debugger
                this.addRecordToGrid(
                    data["result"]["oeqh"]["qutationDetailDto"]
                );
                this.locDesc=data["result"]["oeqh"]["locDesc"];
                this.typeDesc=data["result"]["oeqh"]["saleTypeDesc"];
                this.customerName=data["result"]["oeqh"]["customerDesc"];
                this.chAccountIDDesc=data["result"]["oeqh"]["chartofAccountDesc"];
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
            this.SaleQutationD = new saleQutationDetailDto();
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
                this.SaleQutationH.id = result["result"]["id"];
                this.SaleQutationH=result["result"];
                this.SaleQutationH.typeID = result["result"]["typeID"];
                this.typeDesc = result["result"]["typeDesc"];
                this.SaleQutationH.custID = result["result"]["custID"];
                this.customerName = result["result"]["customerName"];
                this.SaleQutationH.narration = result["result"]["narration"];
                this.SaleQutationH.basicStyle = result["result"]["basicStyle"];
                this.SaleQutationH.license = result["result"]["license"];
                this.SaleQutationH.netAmount = result["result"]["totAmt"];
                this.SaleQutationH.saleDoc = result["result"]["docNo"];

                this.SaleQutationH.docDate = new Date();
                this.SaleQutationH.mDocDate = new Date();

                this._SaleQutationService.getSaleDetailRecord(this.SaleQutationH.id).subscribe((details: any)=>{
                    this.SaleQutationH.id = 0;
                    this.addRecordToGrid(
                        details["result"]["items"]
                    );

                    this.checkFormValid();
                    this.calculations();
                });


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
    this.setTaxClassIdGridNull1();
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


     //=====================Tax Authority 1 Grid Model================
     openSelectTaxAuthorityGridModal1() {
        debugger;
        this.target = "TaxAuthority";
        this.CommonServiceLookupTableModal.id = this.SaleQutationH.taxAuth1;
        this.CommonServiceLookupTableModal.show(this.target);
        this.target = "TaxAuthorityGrid1";
    }

    setTaxAuthorityIdGridNull1() {
        debugger;
        this.SaleQutationH.taxAuth1= '';
        this.setTaxClassIdGridNull1();
    }

    getNewTaxAuthorityGrid1() {
        this.SaleQutationH.taxAuth1 = this.CommonServiceLookupTableModal.id;
      
    }
    //=====================Tax Authority Grid Model================

    //=====================Tax Class 1 Grid================
    openSelectTaxClassGridModal1() {
        if (this.SaleQutationH.taxAuth1 == "" || this.SaleQutationH.taxAuth1 == null) {
            this.message.warn(this.l('Please select Tax authority'));
            return;
        }
        
        this.target = "TaxClass";
        this.CommonServiceLookupTableModal.id = String(this.SaleQutationH.taxClass1);
        this.CommonServiceLookupTableModal.displayName = this.SaleQutationH.taxClassDesc1;
        this.CommonServiceLookupTableModal.taxRate = this.SaleQutationH.taxRate1;
        this.CommonServiceLookupTableModal.show(this.target, this.SaleQutationH.taxAuth1);
        this.target = "TaxClassGrid1";
    }
    getNewTaxClassGrid1() {
        debugger;
        this.SaleQutationH.taxClass1 = Number(this.CommonServiceLookupTableModal.id);
        this.SaleQutationH.taxClassDesc1 = this.CommonServiceLookupTableModal.displayName;
        this.SaleQutationH.taxRate1 = this.CommonServiceLookupTableModal.taxRate;
        this.SaleQutationH.taxAmt1=Number( this.CalculateTax(this.SaleQutationH.taxRate1,this.SaleQutationH.netAmount));
    }
    setTaxClassIdGridNull1() {
        this.SaleQutationH.taxClass1 = 0;
        this.SaleQutationH.taxClassDesc1 = '';
        this.SaleQutationH.taxRate1= 0;
    }
    //
//=====================Tax Authority Grid Model 2================
openSelectTaxAuthorityGridModal2() {
    debugger;
    this.target = "TaxAuthority";
    this.CommonServiceLookupTableModal.id = this.SaleQutationH.taxAuth2;
    this.CommonServiceLookupTableModal.show(this.target);
    this.target = "TaxAuthorityGrid2";
}

setTaxAuthorityIdGridNull2() {
    debugger;
    this.SaleQutationH.taxAuth2= '';
    this.setTaxClassIdGridNull2();
}

getNewTaxAuthorityGrid2() {
    this.SaleQutationH.taxAuth2 = this.CommonServiceLookupTableModal.id;
  
}
//=====================Tax Authority Grid Model2================

//=====================Tax Class Grid2================
openSelectTaxClassGridModal2() {
    if (this.SaleQutationH.taxAuth2 == "" || this.SaleQutationH.taxAuth2 == null) {
        this.message.warn(this.l('Please select Tax authority'));
        return;
    }
    
    this.target = "TaxClass";
    this.CommonServiceLookupTableModal.id = String(this.SaleQutationH.taxClass2);
    this.CommonServiceLookupTableModal.displayName = this.SaleQutationH.taxClassDesc2;
    this.CommonServiceLookupTableModal.taxRate = this.SaleQutationH.taxRate2;
    this.CommonServiceLookupTableModal.show(this.target, this.SaleQutationH.taxAuth2);
    this.target = "TaxClassGrid2";
}
getNewTaxClassGrid2() {
    debugger;
    this.SaleQutationH.taxClass2 = Number(this.CommonServiceLookupTableModal.id);
    this.SaleQutationH.taxClassDesc2 = this.CommonServiceLookupTableModal.displayName;
    this.SaleQutationH.taxRate2 = this.CommonServiceLookupTableModal.taxRate;
    this.SaleQutationH.taxAmt2=Number( this.CalculateTax(this.SaleQutationH.taxRate2,this.SaleQutationH.netAmount));
}
setTaxClassIdGridNull2() {
    this.SaleQutationH.taxClass2 = 0;
    this.SaleQutationH.taxClassDesc2 = '';
    this.SaleQutationH.taxRate2= 0;
}
///

//=====================Tax Authority Grid Model 3================
openSelectTaxAuthorityGridModal3() {
    debugger;
    this.target = "TaxAuthority";
    this.CommonServiceLookupTableModal.id = this.SaleQutationH.taxAuth3;
    this.CommonServiceLookupTableModal.show(this.target);
    this.target = "TaxAuthorityGrid3";
}

setTaxAuthorityIdGridNull3() {
    debugger;
    this.SaleQutationH.taxAuth3= '';
    this.setTaxClassIdGridNull3();
}

getNewTaxAuthorityGrid3() {
    this.SaleQutationH.taxAuth3 = this.CommonServiceLookupTableModal.id;
  
}
//=====================Tax Authority Grid Model2================

//=====================Tax Class Grid3================
openSelectTaxClassGridModal3() {
    if (this.SaleQutationH.taxAuth3 == "" || this.SaleQutationH.taxAuth3 == null) {
        this.message.warn(this.l('Please select Tax authority'));
        return;
    }
    
    this.target = "TaxClass";
    this.CommonServiceLookupTableModal.id = String(this.SaleQutationH.taxClass3);
    this.CommonServiceLookupTableModal.displayName = this.SaleQutationH.taxClassDesc3;
    this.CommonServiceLookupTableModal.taxRate = this.SaleQutationH.taxRate3;
    this.CommonServiceLookupTableModal.show(this.target, this.SaleQutationH.taxAuth3);
    this.target = "TaxClassGrid3";
}
getNewTaxClassGrid3() {
    debugger;
    this.SaleQutationH.taxClass3 = Number(this.CommonServiceLookupTableModal.id);
    this.SaleQutationH.taxClassDesc3 = this.CommonServiceLookupTableModal.displayName;
    this.SaleQutationH.taxRate3 = this.CommonServiceLookupTableModal.taxRate;
    this.SaleQutationH.taxAmt3=Number( this.CalculateTax(this.SaleQutationH.taxRate3,this.SaleQutationH.netAmount));
}
setTaxClassIdGridNull3() {
    this.SaleQutationH.taxClass3 = 0;
    this.SaleQutationH.taxClassDesc3 = '';
    this.SaleQutationH.taxRate2= 0;
}
///


//=====================Tax Authority Grid Model 4================
openSelectTaxAuthorityGridModal4() {
    debugger;
    this.target = "TaxAuthority";
    this.CommonServiceLookupTableModal.id = this.SaleQutationH.taxAuth4;
    this.CommonServiceLookupTableModal.show(this.target);
    this.target = "TaxAuthorityGrid4";
}

setTaxAuthorityIdGridNull4() {
    debugger;
    this.SaleQutationH.taxAuth4= '';
    this.setTaxClassIdGridNull4();
}

getNewTaxAuthorityGrid4() {
    this.SaleQutationH.taxAuth4 = this.CommonServiceLookupTableModal.id;
  
}
//=====================Tax Authority Grid Model4================

//=====================Tax Class Grid4================
openSelectTaxClassGridModal4() {
    if (this.SaleQutationH.taxAuth4 == "" || this.SaleQutationH.taxAuth4 == null) {
        this.message.warn(this.l('Please select Tax authority'));
        return;
    }
    
    this.target = "TaxClass";
    this.CommonServiceLookupTableModal.id = String(this.SaleQutationH.taxClass4);
    this.CommonServiceLookupTableModal.displayName = this.SaleQutationH.taxClassDesc4;
    this.CommonServiceLookupTableModal.taxRate = this.SaleQutationH.taxRate4;
    this.CommonServiceLookupTableModal.show(this.target, this.SaleQutationH.taxAuth4);
    this.target = "TaxClassGrid4";
}
getNewTaxClassGrid4() {
    debugger;
    this.SaleQutationH.taxClass4 = Number(this.CommonServiceLookupTableModal.id);
    this.SaleQutationH.taxClassDesc4 = this.CommonServiceLookupTableModal.displayName;
    this.SaleQutationH.taxRate4 = this.CommonServiceLookupTableModal.taxRate;
    this.SaleQutationH.taxAmt4=Number( this.CalculateTax(this.SaleQutationH.taxRate4,this.SaleQutationH.netAmount));
}
setTaxClassIdGridNull4() {
    this.SaleQutationH.taxClass4 = 0;
    this.SaleQutationH.taxClassDesc4 = '';
    this.SaleQutationH.taxRate4= 0;
}
///


//=====================Tax Authority Grid Model 5================
openSelectTaxAuthorityGridModal5() {
    debugger;
    this.target = "TaxAuthority";
    this.CommonServiceLookupTableModal.id = this.SaleQutationH.taxAuth5;
    this.CommonServiceLookupTableModal.show(this.target);
    this.target = "TaxAuthorityGrid5";
}

setTaxAuthorityIdGridNull5() {
    debugger;
    this.SaleQutationH.taxAuth5= '';
    this.setTaxClassIdGridNull5();
}

getNewTaxAuthorityGrid5() {
    this.SaleQutationH.taxAuth5 = this.CommonServiceLookupTableModal.id;
  
}
//=====================Tax Authority Grid Model4================

//=====================Tax Class Grid4================
openSelectTaxClassGridModal5() {
    if (this.SaleQutationH.taxAuth5 == "" || this.SaleQutationH.taxAuth5 == null) {
        this.message.warn(this.l('Please select Tax authority'));
        return;
    }
    
    this.target = "TaxClass";
    this.CommonServiceLookupTableModal.id = String(this.SaleQutationH.taxClass5);
    this.CommonServiceLookupTableModal.displayName = this.SaleQutationH.taxClassDesc5;
    this.CommonServiceLookupTableModal.taxRate = this.SaleQutationH.taxRate5;
    this.CommonServiceLookupTableModal.show(this.target, this.SaleQutationH.taxAuth5);
    this.target = "TaxClassGrid5";
}
getNewTaxClassGrid5() {
    debugger;
    this.SaleQutationH.taxClass5 = Number(this.CommonServiceLookupTableModal.id);
    this.SaleQutationH.taxClassDesc5 = this.CommonServiceLookupTableModal.displayName;
    this.SaleQutationH.taxRate5 = this.CommonServiceLookupTableModal.taxRate;
    this.SaleQutationH.taxAmt5=Number( this.CalculateTax(this.SaleQutationH.taxRate5,this.SaleQutationH.netAmount));
}
setTaxClassIdGridNull5() {
    this.SaleQutationH.taxClass5 = 0;
    this.SaleQutationH.taxClassDesc5 = '';
    this.SaleQutationH.taxRate5= 0;
}
///



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
     
        this.SaleQutationH.netAmount = amount;
        this.UpdateTax();
     }
     UpdateTax(){
        this.SaleQutationH.taxAmt1=Number( this.CalculateTax(this.SaleQutationH.taxRate1,this.SaleQutationH.netAmount));
        this.SaleQutationH.taxAmt2=Number( this.CalculateTax(this.SaleQutationH.taxRate2,this.SaleQutationH.netAmount));
        this.SaleQutationH.taxAmt3=Number( this.CalculateTax(this.SaleQutationH.taxRate3,this.SaleQutationH.netAmount));
        this.SaleQutationH.taxAmt4=Number( this.CalculateTax(this.SaleQutationH.taxRate4,this.SaleQutationH.netAmount));
        this.SaleQutationH.taxAmt5=Number( this.CalculateTax(this.SaleQutationH.taxRate5,this.SaleQutationH.netAmount));
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
            case "TaxAuthorityGrid1":
                this.getNewTaxAuthorityGrid1();
                break;
            case "TaxAuthorityGrid2":
                    this.getNewTaxAuthorityGrid2();
                    break;
            case "TaxAuthorityGrid3":
                    this.getNewTaxAuthorityGrid3();
                       break;
            case "TaxAuthorityGrid4":
                     this.getNewTaxAuthorityGrid4();
                       break;
            case "TaxAuthorityGrid5":
                    this.getNewTaxAuthorityGrid5();
                        break;
            case "TaxClassGrid":
                    this.getNewTaxClassGrid();
                        break;
            case "TaxClassGrid":
                    this.getNewTaxClassGrid1();
                        break;
            case "TaxClassGrid2":
                    this.getNewTaxClassGrid2();
                    break;
            case "TaxClassGrid3":
                this.getNewTaxClassGrid3();
                       break;
            case "TaxClassGrid4":
                 this.getNewTaxClassGrid4();
                    break;
            case "TaxClassGrid5":
                this.getNewTaxClassGrid5();
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
                debugger
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
