import { Component, ViewChild, Injector, Output, EventEmitter, ɵpublishDefaultGlobalUtils} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { GetDataService } from '../../inventory/shared/services/get-data.service';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { CreateOrEditPORECHeaderDto } from '../shared/dtos/porecHeader-dto';
import { CreateOrEditPORECDetailDto } from '../shared/dtos/porecDetails-dto';
import { ReceiptEntryDto } from '../shared/dtos/receiptEntry-dto';
import { PORECHeadersService } from '../shared/services/porecHeader.service';
import { PORECDetailsService } from '../shared/services/porecDetail.service';
import { ReceiptEntryServiceProxy } from '../shared/services/receiptEntry.service';
import { CreateOrEditICRECAExpDto } from '../shared/dtos/icrecaExp-dto';
import { ICRECAExpsService } from '../shared/services/icrecaExp.service';
import { PurchaseLookupTableModalComponent } from '@app/finders/supplyChain/purchase/purchase-lookup-table-modal.component';
import { VoucherPostingServiceProxy } from '../../periodics/shared/services/voucher-posting-service';
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
import { AppConsts } from '@shared/AppConsts';
import { Lightbox } from 'ngx-lightbox';
import { GLTRHeadersServiceProxy } from '@shared/service-proxies/service-proxies';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';
import { FileDownloadService } from "@shared/utils/file-download.service";
import { Guid } from "guid-typescript";
import { LogComponent } from '@app/finders/log/log.component';
// import { LogComponent } from '@app/finders/log/log.component';
@Component({
    selector: 'createOrEditReceiptEntryModal',
    templateUrl: './create-or-edit-receiptEntry-modal.component.html'
})
export class CreateOrEditReceiptEntryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('CommonServiceLookupTableModal', { static: true }) CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild('PurchaseLookupTableModal', { static: true }) PurchaseLookupTableModal: PurchaseLookupTableModalComponent;
    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>(); 

    active = false;
    saving = false;
    showAmounts = false;
    target:string;
    description='';
    totalQty=0;
    totalAmount=0;
    netAmount=0;
    totalItems=0;
    venderTax=0;
    private setParms;
    locations:any;
    LockDocDate:Date;
    chartofAccDesc:string;
    subAccDesc:string;
    whTermDesc:string;
    taxAuthorityDesc:string;
    taxClassDesc: string;
    status="Incomplete";
    fileName: string;
    contentType: string;
    fileId: string;

    porecHeader: CreateOrEditPORECHeaderDto = new CreateOrEditPORECHeaderDto();
    porecDetail: CreateOrEditPORECDetailDto = new CreateOrEditPORECDetailDto();
    icrecaExp: CreateOrEditICRECAExpDto = new CreateOrEditICRECAExpDto();
    receiptEntry: ReceiptEntryDto=new ReceiptEntryDto();
    agGridExtend: AgGridExtend=new AgGridExtend();

            auditTime: Date;
            docDate:Date;
    processing=false;
    costCenterDesc: string;
    taxAmount: number;
    glNo:number;
    errorFlag: boolean;
    IsVenderID: boolean;
    url: any;
    checkedval:boolean;
    LocCheckVal:boolean;


    constructor(
        injector: Injector,
        private _porecHeadersServiceProxy: PORECHeadersService,
        private _porecDetailServiceProxy: PORECDetailsService,
        private _receiptEntryServiceProxy: ReceiptEntryServiceProxy,
        private _icrecaExpServiceProxy: ICRECAExpsService,
        private _approvelService: ApprovalService,
        private _getDataService: GetDataService,
        private _lightbox: Lightbox,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
        private _fileDownloadServiceProxy: FileDownloadService
    ) {
        super(injector);
    }

    show(porecHeaderId?: number,maxId?: number): void {
    this.auditTime = null; 
    this.url = null;
    this.uploadUrl=null;
    this.image = [];
    this.uploadedFiles = [];
    this.checkImage = true;
    debugger;

    this.getLocations("ICLocations");

        if (!porecHeaderId) {
            this.porecHeader = new CreateOrEditPORECHeaderDto();
            this.porecHeader.id = porecHeaderId;
            this.porecHeader.docDate =moment().endOf('day');
            this.porecHeader.billDate =moment().endOf('day');
            this.porecHeader.docNo=maxId;
            this.porecHeader.locID=0;
            this.porecHeader.totalQty = 0;
            this.porecHeader.billAmt=0;
            this.porecHeader.totalAmt=0;
            this.porecHeader.active=true;
            this.taxAmount=0;
            this.porecHeader.addExp=0;
            this.totalItems=0;

            this.active = true;
            this.modal.show();
        } else {
            this._porecHeadersServiceProxy.getPORECHeaderForEdit(porecHeaderId).subscribe(result => {
                this.porecHeader = result;
                debugger;
                if (this.porecHeader.audtDate) {
					this.auditTime = this.porecHeader.audtDate.toDate();
                }

                this._gltrHeadersServiceProxy.getImage(this.requisitionAppId, result.docNo).subscribe(fileResult => {
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
                this._gltrHeadersServiceProxy
                .getFile(
                    this.requisitionAppId,
                    result.docNo
                )
                .subscribe((fileResult) => {
                    console.log(fileResult);
                    debugger
                    if (fileResult != null) {
                        this.fileName = fileResult[0].toString();
                        this.fileId = fileResult[1].toString();
                    }
                });
                this.porecHeader.postedDate =moment().endOf('day'); 

                this.chartofAccDesc=result.accDesc;
                this.subAccDesc=result.subAccDesc;
                this.costCenterDesc=result.ccDesc;

                this.LockDocDate=this.porecHeader.docDate.toDate();

                this._porecDetailServiceProxy.getPORECDData(porecHeaderId).subscribe(resultD => {
                    debugger;
                    var rData=[];
                    var qty=0;
                    var amount=0;
                    var items=0;
                    var taxAmt=0;
                    resultD["items"].forEach(element => {
                        rData.push(element);
                        qty+=element.qty;
                        amount+=element.amount;
                        taxAmt+=element.taxAmt;
                        items+=items+1;
                    });

                    this.rowData=[];
                    this.rowData=rData;   

                    this.totalItems=items;
                    this.porecHeader.totalQty=qty;
                    this.porecHeader.billAmt=amount;
                    this.taxAmount=taxAmt;

                    this._icrecaExpServiceProxy.getICRECAExpData(porecHeaderId).subscribe(resultA => {
                        debugger;
                        var rDataA=[];
                        var addExp=0;
                        resultA["items"].forEach(element => {
                            debugger
                            if(element.expType=="Discount"){
                                addExp=element.amount-addExp;
                            }else{
                                addExp+=element.amount;
                            }
                            rDataA.push(element);
                            
                        });
    
                        this.rowDataA=[];
                        this.rowDataA=rDataA;
    
                        this.porecHeader.addExp=this.porecHeader.addExp;
                        this.porecHeader.totalAmt=this.porecHeader.billAmt+this.taxAmount+(this.porecHeader.addLeak+this.porecHeader.addFreight-this.porecHeader.addDisc); 
                    });
                });

                this.active = true;
                this.modal.show();
            });
        }
    }

    OpenLog(){
        debugger
      this.LogTableModal.show(this.porecHeader.docNo,'ReceiptEntry');
    }
   SetDefaultRecord(result:any){
    console.log(result);
      this.porecHeader.locID=result.currentLocID;
      //this.locDesc=result.currentLocName;
      this.checkedval=result.cDateOnly;
      if(result.allowLocID==false){
          this.LocCheckVal=true;
      }else{
        this.LocCheckVal=false;
      }
      //this.typeDesc=result.transTypeName;
  }
    //==================================Grid Additional Exp=================================
    private gridApiA;
    private gridColumnApiA;
    private rowDataA;
    private rowSelectionA;
    columnDefsA = [
        {headerName: this.l('SrNo'), field: 'srNo' ,sortable: true,width:50, valueGetter: 'node.rowIndex+1'},
        {headerName: this.l('Type'), field: 'expType',sortable: true,filter: false,width:200,resizable: true,
        cellEditor: 'agSelectCellEditor',editable: true,
        cellEditorParams: {
            values: ['Discount', 'Leakage', 'Freight']
        }}, 
        {headerName: this.l('AccountID'), field: 'accountID',sortable: true,filter: true,width:200,editable: false,resizable: true}, 
        {headerName: this.l(''), field: 'addAcc',width:15,editable: false,cellRenderer: this.addIconCellRendererFunc,resizable: false},
        {headerName: this.l('AccountName'), field: 'accountName',sortable: true,filter: true,width:300,resizable: true}, 
        {headerName: this.l('Amount'), field: 'amount',sortable: true,width:200,editable: true,type: "numericColumn",resizable: true},
    ];

    onGridReadyA(params) {
        debugger;
        this.rowDataA=[];
        this.gridApiA = params.api;
        this.gridColumnApiA = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelectionA = "multiple";
      }

    onAddRowA():void{
        debugger;
        var index=this.gridApiA.getDisplayedRowCount();
        var newItem = this.createNewRowDataA();
        this.gridApiA.updateRowData({ add: [newItem] });
        this.calculations();
        this.gridApiA.refreshCells();
        this.onBtStartEditingA(index,"expType");
    }

    cellClickedA(params){
        debugger;
        if(params.column["colId"]=="addAcc"){
            this.setParms=params;
            this.openSelectChartofACGridModal();
        }
    }

    onBtStartEditingA(index,col) {
        debugger;
        this.gridApiA.setFocusedCell(index, col);
        this.gridApiA.startEditingCell({
          rowIndex: index,
          colKey: col
        });
      }

    onRemoveSelectedA() {
        debugger;
        var selectedData = this.gridApiA.getSelectedRows();
        this.gridApiA.updateRowData({ remove: selectedData });
        this.gridApiA.refreshCells();
        this.calculations();
    }

    createNewRowDataA(){
        debugger;
        var newData = {
            expType: "",
            accountID: "",
            accountName: "",
            amount:'0',
        };
        return newData;
    }

    onCellValueChangedA(params){
        debugger;
        this.calculations();
        this.gridApiA.refreshCells();
    }
    //==================================Grid Additional Exp=================================

    //==================================Grid=================================
    private gridApi;
    private gridColumnApi;
    private rowData;
    private rowSelection;
    columnDefs = [
        {headerName: this.l('SrNo'), field: 'srNo' ,sortable: true,width:50, valueGetter: 'node.rowIndex+1'},
        {headerName: this.l('ItemId'), field: 'itemID',sortable: true,filter: true,width:100,editable: false,resizable: true}, 
        {headerName: this.l(''), field: 'addItemId',width:15,editable: false,cellRenderer: this.addIconCellRendererFunc,resizable: false},
        {headerName: this.l('Description'), field: 'itemDesc',sortable: true,filter: true,width:200,resizable: true}, 
        {headerName: this.l('UOM'), field: 'unit',sortable: true,filter: true,width:80,editable: false,resizable: true},
        // {headerName: this.l(''), field: 'addUOM',width:15,editable: false,cellRenderer: this.addIconCellRendererFunc,resizable: false},
        {headerName: this.l('Conversion'), field: 'conver',sortable: true,filter: true,width:100,resizable: true}, 
        {headerName: this.l('PQty'), field: 'poQty',editable: false,sortable: true,filter: true,width:100,type: "numericColumn",resizable: true}, 
        {headerName: this.l('Qty'), field: 'qty',editable: true,sortable: true,filter: true,width:100,
        type: "numericColumn",resizable: true, valueFormatter: this.agGridExtend.formatNumber}, 
        {headerName: this.l('Rate'), field: 'rate',editable: false,sortable: true,filter: true,width:100,
        type: "numericColumn",resizable: true, valueFormatter: this.agGridExtend.formatNumber,hide:true}, 
        {headerName: this.l('Amount'), field: 'amount',sortable: true,width:100,editable: true,
        type: "numericColumn",resizable: true, valueFormatter: this.agGridExtend.formatNumber,hide:true},
        {headerName: this.l('TAXAUTH'), field: 'taxAuth',sortable: true,width:100,editable: false,resizable: true,hide:true},
        {headerName: this.l(''), field: 'addTaxAuth',width:15,editable: false,cellRenderer: this.addIconCellRendererFunc,resizable: false,hide:true},
        {headerName: this.l('ClassId'), field: 'taxClass',sortable: true,width:100,editable: false,resizable: true,hide:true},
        {headerName: this.l(''), field: 'addTaxClass',width:15,editable: false,cellRenderer: this.addIconCellRendererFunc,resizable: false,hide:true},
        {headerName: this.l('TaxClass'), field: 'taxClassDesc',sortable: true,width:100,editable: false,resizable: true,hide:true},
        {headerName: this.l('TaxRate'), field: 'taxRate',sortable: true,width:100,editable: false,resizable: true,hide:true},
        {headerName: this.l('TaxAmt'), field: 'taxAmt',sortable: true,width:100,editable: false,resizable: true,hide:true},
        {headerName: this.l('NetAmt'), field: 'netAmount',sortable: true,width:100,editable: false,resizable: true, 
        valueFormatter: this.agGridExtend.formatNumber,hide:true},
        {headerName: this.l('Remarks'), field: 'remarks',editable: true,resizable: true}
    ];

    onGridReady(params) {
        debugger;
        this.rowData=[];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
      }

    onAddRow():void{
        debugger;
        var index=this.gridApi.getDisplayedRowCount();
        var newItem = this.createNewRowData();
        this.gridApi.updateRowData({ add: [newItem] });
        this.calculations();
        this.gridApi.refreshCells();
        this.onBtStartEditing(index,"addItemId");
    }

    cellClicked(params){
        debugger;
        switch (params.column["colId"]) {
            case "addItemId":
                this.setParms=params;
                this.openSelectItemModal();
                break;
            case "addUOM":
                this.setParms=params;
                this.openSelectICUOMModal();
                break;
            case "addTaxAuth":
                this.setParms=params;
                this.openSelectTaxAuthorityGridModal();
                break;
            case "addTaxClass":
                this.setParms=params;
                this.openSelectTaxClassGridModal();
                break;
            default:
                break;
        }
    }

    addIconCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }

    onBtStartEditing(index,col) {
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

    createNewRowData(){
        debugger;
        var newData = {
            itemID: "",
            itemDesc: "",
            unit: "",
            conver: "",
            poQty:'0',
            qty: '0',
            rate: '0',
            amount:'0',
            taxAuth:"",
            taxClass:"",
            taxRate:'0',
            taxAmt:'0',
            netAmount:'0',
            remarks: this.porecHeader.narration
        };
        return newData;
    }

    calculations(){
        debugger;
        var items=0;
        var qty=0;
        var amount=0;
        var taxAmt=0;
        var addFreight=0;
        var addLeakage=0;
        var addDisc=0;
        var addnlExp=0;
        this.gridApi.forEachNode(node=>{
            debugger;
            if(node.data.qty != "" && node.data.poQty != "")
            {
                if(parseFloat(node.data.qty) > parseFloat(node.data.poQty) && this.porecHeader.poDocNo > 0)
                {
                    this.message.warn("Quantity is not Greater than PO Quantity!","Quantity Greater"); 
                    node.data.qty = 0;
                    return;
                }
            }
            if((node.data.amount != "" || node.data.qty != "") && node.data.itemID!=""){
                qty+=parseFloat(node.data.qty);
                amount+=parseFloat(node.data.amount);
            }
            items=items+1;
            taxAmt+=parseFloat(node.data.taxAmt);
        });
        this.gridApiA.forEachNode(node=>{
            debugger;
            if(node.data.amount != ""){
                if(node.data.expType=="Discount"){
                    addDisc=parseFloat(node.data.amount)+addDisc;
                } else if(node.data.expType=="Leakage"){
                    addLeakage=parseFloat(node.data.amount)+addLeakage;
                } else if(node.data.expType=="Freight"){
                    addFreight=parseFloat(node.data.amount)+addFreight;
                }
               
            }
        });
        this.totalItems=items;
        this.porecHeader.addExp=addFreight+addLeakage-addDisc;
        this.porecHeader.addDisc=addDisc;
        this.porecHeader.addLeak=addLeakage;
        this.porecHeader.addFreight=addFreight;
        this.porecHeader.totalQty=qty;
        this.porecHeader.billAmt=amount;
        this.taxAmount=taxAmt;
        
        this.porecHeader.totalAmt=this.porecHeader.billAmt+this.taxAmount+this.porecHeader.addExp; 
    } 

    onCellValueChanged(params){
        debugger;
        if (params.data.qty != null && params.data.amount != null) {
            params.data.rate =(parseFloat(params.data.amount) / parseFloat(params.data.qty)).toFixed(2);
       }
        if(params.data.taxRate!=null && params.data.taxAmt!=null){
            params.data.taxAmt=Math.round((parseFloat(params.data.amount)*parseFloat(params.data.taxRate))/100);
            params.data.netAmount=parseFloat(params.data.amount)+parseFloat(params.data.taxAmt);
        }
        this.calculations();
        this.gridApi.refreshCells();
    }

    fnShowAmounts(val){
        debugger
        const rate = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'rate');
        this.gridColumnApi.setColumnVisible(rate, val);
        const amount = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'amount');
        this.gridColumnApi.setColumnVisible(amount, val);
        const netAmount = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'netAmount');
        this.gridColumnApi.setColumnVisible(netAmount, val);
        const taxAuth = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'taxAuth');
        this.gridColumnApi.setColumnVisible(taxAuth, val);
        const addTaxAuth = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'addTaxAuth');
        this.gridColumnApi.setColumnVisible(addTaxAuth, val);
        const taxClass = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'taxClass');
        this.gridColumnApi.setColumnVisible(taxClass, val);
        const addTaxClass = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'addTaxClass');
        this.gridColumnApi.setColumnVisible(addTaxClass, val);
        const taxClassDesc = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'taxClassDesc');
        this.gridColumnApi.setColumnVisible(taxClassDesc, val);
        const taxRate = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'taxRate');
        this.gridColumnApi.setColumnVisible(taxRate, val);
        const taxAmt = this.gridColumnApi.getAllColumns().find(x => x.colDef.field == 'taxAmt');
        this.gridColumnApi.setColumnVisible(taxAmt, val);
        this.showAmounts == val;
     }
    //==================================Grid=================================


    getPONoRecord():void{
        debugger;
        if(this.porecHeader.locID==null || this.porecHeader.locID==0){
            this.message.warn("Please select location","Location Required");
        }else if(this.porecHeader.poDocNo==null){ 
            this.message.warn(this.l("Please Enter or Pick PO No"),'PO No Required'); 
        }else{
            this._receiptEntryServiceProxy.getPONoHeaderRecord(this.porecHeader.locID,this.porecHeader.poDocNo).subscribe(result => {
                debugger;
                if(result.accountID==undefined){
                    this.message.info(this.l("No record found for PO No "+this.porecHeader.poDocNo),'Empty Result'); 
                    return;
                }
                let tempdocno = this.porecHeader.docNo;
                result.poDocNo=this.porecHeader.poDocNo;
                this.porecHeader = result;
                if (this.porecHeader.audtDate) {
					this.auditTime = this.porecHeader.audtDate.toDate();
                }
                
                this.porecHeader.docNo = tempdocno;
                this.porecHeader.id=null;
                this.chartofAccDesc=result.accDesc;
                this.subAccDesc=result.subAccDesc;
                this.costCenterDesc=result.ccDesc;
                this.porecHeader.poDocNo=result.poDocNo;
                this.porecHeader.approved=false;
                this.porecHeader.posted=false;
                this.chartofAccDesc=result.accDesc;
                this.subAccDesc=result.subAccDesc;
                this.costCenterDesc=result.ccDesc;
                this.porecHeader.docDate =moment().endOf('day');
                this.porecHeader.billDate =moment().endOf('day');

                this._receiptEntryServiceProxy.getPONoRecords(this.porecHeader.locID,this.porecHeader.poDocNo).subscribe(resultD => {
                    debugger;
                    if(resultD.totalCount==0){
                        this.message.info(this.l("No record found for PO No "+this.porecHeader.poDocNo),'Empty Result'); 
                        this.rowData=[];
                        return;
                    }

                    // this.porecHeader.locID=resultD.items[0].locID;
                    // this.porecHeader.docNo=resultD.items[0].docNo;

                    var rData=[];
                    var qty=0;
                    var amount=0;
                    var items=0;
                    resultD["items"].forEach(element => {
                        rData.push(element);
                        qty+=element.qty;
                        amount+=element.amount;
                        items+=items+1;
                    });
                    debugger
                    this.rowData=[];
                    this.rowData=rData;   

                    this.porecDetail.id=null;
                    this.totalItems=items;
                    this.porecHeader.totalQty=qty;
                    this.porecHeader.billAmt=amount;
                    this.porecHeader.addExp=0;
                    this.taxAmount=0;
                    this.porecHeader.totalAmt=this.porecHeader.billAmt+this.taxAmount+this.porecHeader.addExp; 
                });
            });
        }
    }

    save(): void {
        debugger;
        this.message.confirm(
            'Save Receipt Entry',
            (isConfirmed) => {
                if (isConfirmed) {
                    
                    // if(moment(this.porecHeader.docDate)>moment().endOf('day')){
                    //     this.message.warn("Document date greater than current date","Document Date Error");
                    //     return;
                    // }

                    // if(moment(this.porecHeader.billDate)>moment().endOf('day')){ 
                    //     this.message.warn("Invoice date greater than current date","Invoice Date Error"); 
                    //     return;
                    // } 

                    if((moment(this.LockDocDate).month()+1)!=(moment(this.porecHeader.docDate).month()+1) && this.porecHeader.id!=null){
                        this.message.warn('Document month not changeable',"Document Month Error");
                        return;
                    }

                    if(this.porecHeader.locID==null || this.porecHeader.locID==0){
                        this.message.warn("Please select location","Location Required");
                        return;
                    }

                    if(this.gridApi.getDisplayedRowCount()<=0){
                        this.message.warn("No items details found","Items Details Required");
                        return;
                    }

                    this.gridApi.forEachNode(node=>{debugger
                        if(node.data.itemID==""){
                            this.message.warn("Item not found at row "+ Number(node.rowIndex+1),"Item Required");
                            this.errorFlag=true;
                            return;
                        // }else if(node.data.qty<=0){
                        //     this.message.warn("Qty not greater than zero at row "+ Number(node.rowIndex+1),"Qty Zero");
                        //     this.errorFlag=true;
                        //     return;
                        }else if(node.data.poQty!=0 && node.data.poQty<node.data.qty){
                            this.message.warn("Qty not greater than POQty at row "+ Number(node.rowIndex+1),"Qty Greater");
                            this.errorFlag=true;
                            return;
                        }else{
                            this.errorFlag=false;
                        }
                    });

                    if(this.errorFlag){
                        return;
                    }

                    if(this.porecHeader.totalAmt<=0 || this.porecHeader.totalQty<=0){
                        this.message.warn("Qty OR Amount not less than OR equal to zero","Qty OR Amount Error");
                        return;
                    }
            
                    this.saving = true;
            
                    var rowData=[];
                    this.gridApi.forEachNode(node=>{
                        rowData.push(node.data);
                    });
                    
                    var rowDataA=[];
                    this.gridApiA.forEachNode(node=>{
                        rowDataA.push(node.data);
                    });
            

                    if(moment(new Date()).format("A")==="AM" && !this.porecHeader.id   && (moment(new Date()).month()+1)==(moment(this.porecHeader.docDate).month()+1)){
                        this.porecHeader.docDate =moment(this.porecHeader.docDate);
                    }else{
                        this.porecHeader.docDate =moment(this.porecHeader.docDate).endOf('day');
                    }

                    //this.porecHeader.active=true;
                    
                    this.receiptEntry.porecDetail=rowData;
                    this.receiptEntry.icrecaExp=rowDataA;
                    this.receiptEntry.porecHeader=this.porecHeader; 
        
                    this._receiptEntryServiceProxy.createOrEditReceiptEntry(this.receiptEntry)
                        .pipe(finalize(() => { this.saving = false;}))
                        .subscribe(() => {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);  
                        });

                }
            }
        );
        
    }

    processReceiptEntry():void{
        debugger;
        if(moment(this.porecHeader.docDate).endOf('day')>moment(this.porecHeader.postedDate).endOf('day')){ 
            this.message.warn("Posted date less than document date","Posted Date Less"); 
            return;
        } 
        this.message.confirm(
            "Process Receipt Entry with Posted Date: "+this.porecHeader.postedDate.toDate().toLocaleDateString(),
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing = true; 
                    debugger;

                    if(moment(new Date()).format("A")==="AM" && !this.porecHeader.id   && (moment(new Date()).month()+1)==(moment(this.porecHeader.docDate).month()+1)){
                        this.porecHeader.docDate =moment(this.porecHeader.docDate);
                    }else{
                        this.porecHeader.docDate =moment(this.porecHeader.docDate).endOf('day');
                    }

                this._receiptEntryServiceProxy.processReceiptEntry(this.porecHeader).subscribe(result => {
                    debugger
                        if(result=="Save"){
                            this.saving = false;
                            this.notify.info(this.l('ProcessSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        }else if(result=="NoAccount"){
                            this.message.warn("Account Receivable not found","Account Required");
                            this.processing = false;
                        }else if(result=="CreditDebitMismatch"){
                            this.message.warn("Credit and Debit amounts are mismatched","Cannot Process");
                            this.processing = false;
                        }else{
                            this.message.error("Input not valid","Verify Input");
                            this.processing = false;
                        } 
                    });
                }
            }
        );
    }

    approveDoc(id: number,mode, approve) {
        debugger;
        this.message.confirm(
            approve?'Approve Receipt':'Unapprove Receipt',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("receipt", [id], mode, approve)
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

    getLocations(target:string):void{
        debugger;
        this._getDataService.getList(target).subscribe(result => {
            this.locations = result;
        }); 
    }

    getNewPurchaseModal()
    {
        debugger;
        switch (this.target) {
            case "PONo":
                this.getNewPONo();
                break;
            default:
                break;
        }
    }
    getNewFinanceModal()
    {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartOfAC();
                break;
            case "ChartOfAccountGrid":
                this.getNewChartOfACGrid();
                break;
            case "SubLedger":
                this.getNewSubAcc(); 
                break;
            default:
                break;
        }
    }
    getNewCommonServiceModal()
    {
        switch (this.target) {
            case "TaxAuthorityGrid":
                this.getNewTaxAuthorityGrid();
                break;
            case "TaxClassGrid":
                this.getNewTaxClassGrid();
                break;
            default:
                break;
        }
    }

    getNewInventoryModal()
    {
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
            default:
                break;
        }
    }

    //=====================PO No Model================
    openSelectPONoModal() {
        debugger;
        if(this.porecHeader.locID==null || this.porecHeader.locID==0){
            this.message.warn("Please select location","Location Required");
            return;
        }
        this.target="PONo";
        this.PurchaseLookupTableModal.id = String(this.porecHeader.poDocNo);
        this.PurchaseLookupTableModal.show(this.target,String(this.porecHeader.locID));
    }

    setPONoNull() {
        this.porecHeader.poDocNo = 0;
    } 

    getNewPONo() {
        debugger;
        this.porecHeader.poDocNo = Number(this.PurchaseLookupTableModal.id);
    }
    //=====================PO No Model================

    //=====================Chart of Ac Grid Model================
    openSelectChartofACGridModal() {
        debugger;
        this.target="ChartOfAccount";
        this.FinanceLookupTableModal.id = this.setParms.data.accountID;
        this.FinanceLookupTableModal.displayName = this.setParms.data.accountName;
        this.FinanceLookupTableModal.show(this.target);
        this.target="ChartOfAccountGrid";
    }

    setAccountIDGridNull() {
        this.setParms.data.accountID = '';
        this.setParms.data.accountName = '';
    } 

    getNewChartOfACGrid() {
        debugger;
        this.setParms.data.accountID = this.FinanceLookupTableModal.id;
        this.setParms.data.accountName = this.FinanceLookupTableModal.displayName; 
        this.gridApiA.refreshCells();
        this.onBtStartEditingA(this.setParms.rowIndex,"amount");  
    }
    
    //=====================Chart of Ac Grid Model================

    //=====================Chart of Ac Model================
    openSelectChartofACModal() {
        debugger;
        this.target="ChartOfAccount";
        this.FinanceLookupTableModal.id = this.porecHeader.accountID;
        this.FinanceLookupTableModal.displayName = this.chartofAccDesc;
        this.FinanceLookupTableModal.show(this.target,"true");
    }

    setAccountIDNull() {
        this.porecHeader.accountID = '';
        this.chartofAccDesc = '';
        this.setSubAccIDNull();
    } 

    getNewChartOfAC() {
        debugger;
        if(this.FinanceLookupTableModal.id!=this.porecHeader.accountID)
            this.setSubAccIDNull();
        this.porecHeader.accountID = this.FinanceLookupTableModal.id;
        this.chartofAccDesc = this.FinanceLookupTableModal.displayName;  
        if(this.FinanceLookupTableModal.subledger){
            this.IsVenderID=true;
        }else{
            this.IsVenderID=false;
        }
    }
    //=====================Chart of Ac Model================

    //=====================Sub Account Model================
    openSelectSubAccModal() {
        debugger;
        if(this.porecHeader.accountID=="" || this.porecHeader.accountID==null){
            this.message.warn(this.l('Please select account first'),'Account Required');
            return;
        }
        this.target="SubLedger";
        this.FinanceLookupTableModal.id = String(this.porecHeader.subAccID); 
        this.FinanceLookupTableModal.displayName = this.subAccDesc;
        this.FinanceLookupTableModal.show(this.target,this.porecHeader.accountID);
    }

    setSubAccIDNull() {
        this.porecHeader.subAccID = null;
        this.subAccDesc = '';
    }

    getNewSubAcc() {
        this.porecHeader.subAccID = Number(this.FinanceLookupTableModal.id);
        this.subAccDesc = this.FinanceLookupTableModal.displayName;    
    }
    //=====================Sub Account Model================

    //=====================Tax Authority Grid Model================
    openSelectTaxAuthorityGridModal() {
        debugger;
        this.target="TaxAuthority";
        this.CommonServiceLookupTableModal.id = this.setParms.data.taxAuth;
        this.CommonServiceLookupTableModal.show(this.target);
        this.target="TaxAuthorityGrid";
    }

    setTaxAuthorityIdGridNull() {
        debugger;
        this.setParms.data.taxAuth = '';
        this.setTaxClassIdGridNull();
    }

    getNewTaxAuthorityGrid() {
        debugger;
        if(this.CommonServiceLookupTableModal.id!=this.setParms.data.taxAuth)
            this.setTaxClassIdGridNull();
        this.setParms.data.taxAuth = this.CommonServiceLookupTableModal.id;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex,"addTaxClass");
    }
    //=====================Tax Authority Grid Model================

    //=====================Tax Class Grid================
    openSelectTaxClassGridModal()
    {
        if(this.setParms.data.taxAuth=="" || this.setParms.data.taxAuth==null){
            this.message.warn(this.l('Please select Tax authority first at row '+ Number(this.setParms.rowIndex+1)),'Tax Authority Required');
            return;
        }
        this.target="TaxClass";
        this.CommonServiceLookupTableModal.id = String(this.setParms.data.taxClass);
        this.CommonServiceLookupTableModal.displayName = this.setParms.data.taxClassDesc;
        this.CommonServiceLookupTableModal.taxRate = this.setParms.data.taxRate;
        this.CommonServiceLookupTableModal.show(this.target,this.setParms.data.taxAuth);
        this.target="TaxClassGrid";
    }
    getNewTaxClassGrid() {
        debugger;
        this.setParms.data.taxClass = Number(this.CommonServiceLookupTableModal.id);
        this.setParms.data.taxClassDesc = this.CommonServiceLookupTableModal.displayName;  
        this.setParms.data.taxRate = this.CommonServiceLookupTableModal.taxRate;  
        this.onBtStartEditing(this.setParms.rowIndex,"remarks"); 
        this.onCellValueChanged(this.setParms);
    }
    setTaxClassIdGridNull() {
        this.setParms.data.taxClass = '';
        this.setParms.data.taxClassDesc = '';
        this.setParms.data.taxRate = 0;
    }
    //=====================Tax Class Grid================

    //=====================Item Model================
    openSelectItemModal() {
        debugger;
        this.target="Items";
        this.InventoryLookupTableModal.id = this.setParms.data.itemID;
        this.InventoryLookupTableModal.displayName = this.setParms.data.itemDesc;
        this.InventoryLookupTableModal.unit = this.setParms.data.unit;
        this.InventoryLookupTableModal.conver = this.setParms.data.conver;
        this.InventoryLookupTableModal.show(this.target);
    }


    setItemIdNull() {
        this.setParms.data.itemID = null;
        this.setParms.data.itemDesc='';
        this.setParms.data.unit = '';
        this.setParms.data.conver='';
    }


    getNewItemId() {
        debugger;
        var ConStatus=false;
        this.gridApi.forEachNode(node=>{
            debugger
         if(node.data.itemID!='' && node.data.itemID!=null){
            if(node.data.itemID==this.InventoryLookupTableModal.id){
                this.message.warn("Item Has Already Exist At Row No "+ Number(node.rowIndex+1),"Item Duplicate!");
                ConStatus=true;
                return;
            }
         }
        });

        if(ConStatus==false){

        this.setParms.data.itemID = this.InventoryLookupTableModal.id;
        this.setParms.data.itemDesc=this.InventoryLookupTableModal.displayName;
        this.setParms.data.unit=this.InventoryLookupTableModal.unit;
        this.setParms.data.conver=this.InventoryLookupTableModal.conver;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex,"qty");
    }

    }
    //================Item Model===============

    //=====================UOM Model================
    openSelectICUOMModal() {
        debugger;
        this.target="UOM";
        this.InventoryLookupTableModal.unit = this.setParms.data.unit;
        this.InventoryLookupTableModal.conver = this.setParms.data.conver;
        this.InventoryLookupTableModal.show(this.target);
    }


    setICUOMNull() {
        this.setParms.data.unit = '';
        this.setParms.data.conver='';
    }


    getNewICUOM() {
        debugger;
        this.setParms.data.unit = this.InventoryLookupTableModal.unit;
        this.setParms.data.conver=this.InventoryLookupTableModal.conver;
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex,"qty");
    }
    //================UOM Model===============

    //=====================Cost Center================
    openSelectCostCenterModal()
    {
        this.target="CostCenter";
        this.InventoryLookupTableModal.id = String(this.porecHeader.ccid); 
        this.InventoryLookupTableModal.displayName = this.costCenterDesc;
        this.InventoryLookupTableModal.show(this.target);
    }
    getNewCostCenter() {
        debugger;
        this.porecHeader.ccid = this.InventoryLookupTableModal.id;
        this.costCenterDesc = this.InventoryLookupTableModal.displayName;    
    }
    setCostCenterIDNull()
    {
        this.porecHeader.ccid=null;
        this.costCenterDesc="";

    }
    //=====================Cost Center================

    close(): void {

        this.active = false;
        this.modal.hide();
    }

     //===========================File Attachment=============================

     readonly requisitionAppId = 14;
     readonly appName = "purchaseReceiptEntry";
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
         if (this.porecHeader.docNo !== undefined)
             this.uploadUrl += "DocID=" + encodeURIComponent("" + this.porecHeader.docNo) + "&";
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
     downloadFile() {
        debugger
        let temp = this.fileName.toLowerCase().split(".")[1];
        debugger
        switch (temp) {
            case 'png':
                this.contentType = "image/png";
                break;
            case 'jpeg':
                this.contentType = "image/jpeg";
            case 'jpg':
                this.contentType = "image/jpg";
                break;
            case 'txt':
                this.contentType = "text/plain";
                break;
            case 'xls':
                this.contentType = "application/vnd.ms-excel";
                break;
            case 'xlsx':
                this.contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case 'pdf':
                this.contentType = "application/pdf";
                break;
            case 'csv':
                this.contentType = "text/csv";
                break;
            case 'zip':
                this.contentType = "application/zip";
                break;
            case 'rar':
                this.contentType = "application/x-rar-compressed";
                break;
        }
        this._receiptEntryServiceProxy.downloadFile(Guid.parse(this.fileId), this.contentType, this.fileName).subscribe(res=>{

        });
    }
    removeFile() {
        this._receiptEntryServiceProxy
            .deleteFile(
                this.requisitionAppId, this.porecHeader.docNo
            )
            .subscribe((fileResult) => {
                console.log(fileResult);
                debugger
                this.checkImage = true;
            });
    }
}
