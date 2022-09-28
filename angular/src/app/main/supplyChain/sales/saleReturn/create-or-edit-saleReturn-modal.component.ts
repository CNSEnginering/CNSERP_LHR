import { Component, ViewChild, Injector, Output, EventEmitter, ɵpublishDefaultGlobalUtils} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { CreateOrEditOERETHeaderDto } from '../shared/dtos/oeretHeader-dto';
import { CreateOrEditOERETDetailDto } from '../shared/dtos/oeretDetails-dto';
import { SaleReturnDto } from '../shared/dtos/saleReturn-dto';
import { OERETHeadersService } from '../shared/services/oeretHeader.service';
import { SaleReturnServiceProxy } from '../shared/services/saleReturn.service';
import { OERETDetailsService } from '../shared/services/oeretDetail.service';
import { OESALEDetailsService } from '../shared/services/oesaleDetail.service';
import { SalesLookupTableModalComponent } from '@app/finders/supplyChain/sales/sales-lookup-table-modal.component';
import { LogComponent } from '@app/finders/log/log.component';
import { ApprovalService } from '../../periodics/shared/services/approval-service.';

@Component({
    selector: 'createOrEditSaleReturnModal',
    templateUrl: './create-or-edit-saleReturn-modal.component.html'
})
export class CreateOrEditSaleReturnModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild('SalesLookupTableModal', { static: true }) SalesLookupTableModal: SalesLookupTableModalComponent;

    @ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>(); 

    active = false;
    saving = false;
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
    typeDesc:string;
    locDesc:string;
    whTermDesc:string;
    taxAuthorityDesc:string;
    taxClassDesc: string;

    oeretHeader: CreateOrEditOERETHeaderDto = new CreateOrEditOERETHeaderDto();
    oeretDetail: CreateOrEditOERETDetailDto = new CreateOrEditOERETDetailDto();
    saleReturn: SaleReturnDto=new SaleReturnDto();

            auditTime: Date;
            docDate:Date;
    processing=false;
    customerName: string;
    errorFlag: boolean;
    IsVenderID: boolean;
    priceListDesc: string;
    checkedval:boolean;
    LocCheckVal:boolean;

    constructor(
        injector: Injector,
        private _oeretHeadersServiceProxy: OERETHeadersService,
        private _oeretDetailServiceProxy: OERETDetailsService,
        private _oesaleDetailServiceProxy: OESALEDetailsService,
        private _saleReturnServiceProxy: SaleReturnServiceProxy,
        private _approvelService: ApprovalService,
    ) {
        super(injector);
    }
    SetDefaultRecord(result:any){
        console.log(result);
          this.oeretHeader.locID=result.currentLocID;
          this.locDesc=result.currentLocName;
          this.checkedval=result.cDateOnly;
          this.LocCheckVal=result.allowLocID;
      }
    show(oeretHeaderId?: number,maxId?: number): void {
    this.auditTime = null; 
    debugger;

        if (!oeretHeaderId) {
            this.oeretHeader = new CreateOrEditOERETHeaderDto();
            this.oeretHeader.id = oeretHeaderId;
            this.oeretHeader.docDate =moment().endOf('day');
            this.oeretHeader.paymentDate =moment().endOf('day');
            this.oeretHeader.docNo=maxId;
            this.oeretHeader.totalQty = 0;
            this.oeretHeader.totAmt=0;
            this.oeretHeader.amount=0;
            this.oeretHeader.tax=0;
            this.oeretHeader.addTax=0;
            this.oeretHeader.disc=0;
            this.oeretHeader.tradeDisc=0;
            this.oeretHeader.freight=0;
            this.oeretHeader.margin=0;
            this.totalItems=0;

            this.active = true;
            this.modal.show();
        } else {
            this._oeretHeadersServiceProxy.getOERETHeaderForEdit(oeretHeaderId).subscribe(result => {
                this.oeretHeader = result;
                debugger;
                if (this.oeretHeader.audtDate) {
					this.auditTime = this.oeretHeader.audtDate.toDate();
                }

                this.typeDesc=result.typeDesc;
                this.locDesc=result.locDesc;
                this.customerName=result.customerName;

                this.LockDocDate=this.oeretHeader.docDate.toDate();

                this._oeretDetailServiceProxy.getOERETDData(oeretHeaderId).subscribe(resultD => {
                    debugger;
                    var rData=[];
                    var qty=0;
                    var amount=0;
                    var items=0;
                    var taxAmt=0;
                    var disc=0;
                    resultD["items"].forEach(element => {
                        rData.push(element);
                        qty+=element.qty;
                        amount+=element.amount;
                        taxAmt+=element.taxAmt;
                        disc+=element.disc;
                        items+=items+1;
                    });

                    this.rowData=[];
                    this.rowData=rData;   

                    this.totalItems=items;
                    this.oeretHeader.totalQty=qty;
                    this.oeretHeader.amount=amount;
                    this.oeretHeader.tax=taxAmt;
                    this.oeretHeader.disc=disc;
                });

                this.active = true;
                this.modal.show();
            });
        }
    }
    OpenLog(){
        debugger
       this.LogTableModal.show(this.oeretHeader.docNo,'SaleReturn');
    }
    //==================================Grid=================================
    private gridApi;
    private gridColumnApi;
    private rowData;
    private rowSelection;
    columnDefs = [
        {headerName: this.l('SrNo'), field: 'srNo' ,sortable: true,width:50, valueGetter: 'node.rowIndex+1'},
        {headerName: this.l('ItemId'), field: 'itemID',sortable: true,filter: true,width:100,editable: false,resizable: true}, 
        {headerName: this.l('Description'), field: 'itemDesc',sortable: true,filter: true,width:200,resizable: true}, 
        {headerName: this.l('UOM'), field: 'unit',sortable: true,filter: true,width:80,editable: false,resizable: true},
        {headerName: this.l('Conversion'), field: 'conver',sortable: true,filter: true,width:100,resizable: true}, 
        {headerName: this.l('QtyInHand'), field: 'inHand',editable: false,sortable: true,filter: true,width:100,type: "numericColumn",resizable: true}, 
        {headerName: this.l('Qty'), field: 'qty',editable: true,sortable: true,filter: true,width:100,type: "numericColumn",resizable: true}, 
        {headerName: this.l('Rate'), field: 'rate',editable: false,sortable: true,filter: true,width:100,type: "numericColumn",resizable: true}, 
        {headerName: this.l('Amount'), field: 'amount',sortable: true,width:100,editable: false,type: "numericColumn",resizable: true},
        {headerName: this.l('Discount'), field: 'disc',sortable: true,width:100,editable: false,type: "numericColumn",resizable: true},
        {headerName: this.l('TAXAUTH'), field: 'taxAuth',sortable: true,width:100,editable: false,resizable: true},
        {headerName: this.l('ClassId'), field: 'taxClass',sortable: true,width:100,editable: false,resizable: true},
        {headerName: this.l('TaxClass'), field: 'taxClassDesc',sortable: true,width:100,editable: false,resizable: true},
        {headerName: this.l('TaxRate'), field: 'taxRate',sortable: true,width:100,editable: false,resizable: true},
        {headerName: this.l('TaxAmt'), field: 'taxAmt',sortable: true,width:100,editable: false,resizable: true},
        {headerName: this.l('NetAmt'), field: 'netAmount',sortable: true,width:100,editable: false,resizable: true},
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
        this.calculations();
        this.gridApi.refreshCells();
    }

    addIconCellRendererFunc(params) {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }


    onRemoveSelected() {
        debugger;
        var selectedData = this.gridApi.getSelectedRows();
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        this.calculations();
    }

    calculations(){
        debugger;
        var items=0;
        var qty=0;
        var amount=0;
        var taxAmt=0;
        var disc=0;
        this.gridApi.forEachNode(node=>{
            debugger;
            if((node.data.amount != "" || node.data.qty != "") && node.data.itemID!=""){
                qty+=parseFloat(node.data.qty);
                amount+=parseFloat(node.data.amount);
            }
            items=items+1;
            taxAmt+=parseFloat(node.data.taxAmt);
            disc+=parseFloat(node.data.disc);
        });
        this.totalItems=items;
        this.oeretHeader.totalQty=qty;
        this.oeretHeader.amount=amount;
        this.oeretHeader.disc=disc;
        this.oeretHeader.tax=taxAmt;
        this.oeretHeader.totAmt=this.oeretHeader.freight+this.oeretHeader.margin+this.oeretHeader.tax+this.oeretHeader.addTax+ this.oeretHeader.amount-this.oeretHeader.disc-this.oeretHeader.tradeDisc; 
    } 

    calculatetotal(){
        debugger;
        this.oeretHeader.totAmt=this.oeretHeader.freight+this.oeretHeader.margin+this.oeretHeader.tax+this.oeretHeader.addTax+ this.oeretHeader.amount-this.oeretHeader.disc-this.oeretHeader.tradeDisc; 
    }

    onCellValueChanged(params){
        debugger;
        if(params.data.qty!=null && params.data.rate!=null){
            params.data.amount=parseFloat(params.data.qty)*parseFloat(params.data.rate);
        }
        if(params.data.taxRate!=null && params.data.disc!=null && params.data.taxAmt!=null){
            params.data.taxAmt=Math.round((parseFloat(params.data.amount)*parseFloat(params.data.taxRate))/100);
            params.data.netAmount=parseFloat(params.data.amount)+parseFloat(params.data.taxAmt)-parseFloat(params.data.disc);
        }
        this.calculations();
        this.gridApi.refreshCells();
    }

    //==================================Grid=================================

    save(): void {
        debugger;
        this.message.confirm(
            'Save Sale Return',
            (isConfirmed) => {
                if (isConfirmed) {
                    
                    // if(moment(this.oeretHeader.docDate)>moment().endOf('day')){
                    //     this.message.warn("Document date greater than current date","Document Date Greater");
                    //     return;
                    // }

                    // if(moment(this.oeretHeader.paymentDate)>moment().endOf('day')){ 
                    //     this.message.warn("Payment date greater than current date","Payment Date Greater"); 
                    //     return;
                    // } 

                    if((moment(this.LockDocDate).month()+1)!=(moment(this.oeretHeader.docDate).month()+1) && this.oeretHeader.id!=null){
                        this.message.warn('Document month not changeable',"Document Month Error");
                        return;
                    }

                    if(this.oeretHeader.locID==null || this.oeretHeader.locID==0){
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
                        }else if(node.data.inHand!=0 && node.data.inHand<node.data.qty){
                            this.message.warn("Qty not greater than Sale Qty at row "+ Number(node.rowIndex+1),"Qty Greater");
                            this.errorFlag=true;
                            return;
                        }else{
                            this.errorFlag=false;
                        }
                    });

                    if(this.errorFlag){
                        return;
                    }

                    if(this.oeretHeader.totAmt<=0 || this.oeretHeader.totalQty<=0){
                        this.message.warn("Qty OR Amount not less than OR equal to zero","Qty OR Amount Zero");
                        return;
                    }
            
                    this.saving = true;
            
                    var rowData=[];
                    this.gridApi.forEachNode(node=>{
                        rowData.push(node.data);
                    });

                    if(moment(new Date()).format("A")==="AM" && !this.oeretHeader.id   && (moment(new Date()).month()+1)==(moment(this.oeretHeader.docDate).month()+1)){
                        this.oeretHeader.docDate =moment(this.oeretHeader.docDate);
                    }else{
                        this.oeretHeader.docDate =moment(this.oeretHeader.docDate).endOf('day');
                    }

                    this.oeretHeader.active=true;

                    this.saleReturn.oeretDetail=rowData;
                    this.saleReturn.oeretHeader=this.oeretHeader; 
        
                    this._saleReturnServiceProxy.createOrEditSaleReturn(this.saleReturn)
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
    approveDoc(id: any,mode:any, approve:any) {
        debugger;
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {

                    this._approvelService.ApprovalData("SaleReturn", [id], mode, approve)
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
    getSaleNoRecord():void{
        debugger;
        if(this.oeretHeader.locID==null || this.oeretHeader.locID==0){
            this.message.warn("Please select location","Location Required");
        }else if(this.oeretHeader.sDocNo==null){ 
            this.message.warn(this.l("Please Enter or Pick Sale No"),'Sale No Required'); 
        }else{
            this._saleReturnServiceProxy.getSaleNoHeaderRecord(this.oeretHeader.locID,this.oeretHeader.sDocNo).subscribe(result => {
                debugger;
                if(result.custID==undefined){
                    this.message.info(this.l("No record found for Sale No "+this.oeretHeader.sDocNo),'Empty Result');
                    this.rowData=[]; 
                    return;
                }

                this.oeretHeader = result;
                if (this.oeretHeader.audtDate) {
					this.auditTime = this.oeretHeader.audtDate.toDate();
                }
                
                this.oeretHeader.sDocNo=result.docNo;
                this.oeretHeader.posted=false;
                this.typeDesc=result.typeDesc;
                this.locDesc=result.locDesc;
                this.customerName=result.customerName;

                this.oeretHeader.docDate =moment().endOf('day');

                this._oesaleDetailServiceProxy.getOESaleDData(this.oeretHeader.docNo).subscribe(resultD => {
                    debugger;
                    if(resultD.totalCount==0){
                        this.message.info(this.l("No record found for Sale No "+this.oeretHeader.sDocNo),'Empty Result'); 
                        this.rowData=[];
                        return;
                    }
                    this.oeretHeader.id=null;
                    var rData=[];
                    var qty=0;
                    var amount=0;
                    var items=0;
                    var taxAmt=0;
                    var disc=0;
                    resultD["items"].forEach(element => {
                        rData.push(element);
                        qty+=element.qty;
                        amount+=element.amount;
                        items+=items+1;
                        taxAmt+=element.taxAmt;
                        disc+=element.disc;

                        //set qty zero and inhand=qty
                        element.inHand=element.qty;
                        element.qty=0;

                        element.id=null;
                    });

                    this.rowData=[];
                    this.rowData=rData;   

                    this.oeretDetail.id=null;
                    this.totalItems=items;
                    this.oeretHeader.totalQty=0;
                    this.oeretHeader.amount=0;
                    this.oeretHeader.disc=disc;
                    this.oeretHeader.tax=taxAmt;
                    this.oeretHeader.totAmt=this.oeretHeader.freight+this.oeretHeader.margin+this.oeretHeader.tax+this.oeretHeader.addTax+ this.oeretHeader.amount-this.oeretHeader.disc-this.oeretHeader.tradeDisc; 
                });
            });
        }
    }


    processSaleReturn():void{
        debugger;
        this.message.confirm(
            'Process Sale Return',
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing = true; 
                    debugger;

                    if(moment(new Date()).format("A")==="AM" && !this.oeretHeader.id   && (moment(new Date()).month()+1)==(moment(this.oeretHeader.docDate).month()+1)){
                        this.oeretHeader.docDate =moment(this.oeretHeader.docDate);
                    }else{
                        this.oeretHeader.docDate =moment(this.oeretHeader.docDate).endOf('day');
                    }

                this._saleReturnServiceProxy.processSaleReturn(this.oeretHeader).subscribe(result => {
                    debugger
                        if(result=="Save"){
                            this.saving = false;
                            this.notify.info(this.l('ProcessSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        }else if(result=="NoAccount"){
                            this.message.warn("Account not found","Account Required");
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


    getNewInventoryModal()
    {
        switch (this.target) {
            case "Location":
                this.getNewLocation();
                break;     
            default:
                break;
        }
    }

    getNewSalesModal()
    {
        debugger;
        this.getNewSaleNo();
    }


    //=====================Sale No Model================
    openSelectSaleNoModal() {
        debugger;
        if(this.oeretHeader.locID==null || this.oeretHeader.locID==0){
            this.message.warn("Please select location","Location Required");
            return;
        }
        this.target="SaleNo";
        this.SalesLookupTableModal.id =String(this.oeretHeader.sDocNo);
        this.SalesLookupTableModal.show(this.target,String(this.oeretHeader.locID));
    }

    setSaleNoNull() {
        this.oeretHeader.sDocNo = 0;
    } 

    getNewSaleNo() {
        debugger;
        this.oeretHeader.sDocNo =Number(this.SalesLookupTableModal.id);
    }
    //=====================Sale No Model================

    //=====================Location Model================
    openSelectLocationModal()
    {
        if(this.LocCheckVal==true){
            this.target="Location";
            this.InventoryLookupTableModal.id = String(this.oeretHeader.locID); 
            this.InventoryLookupTableModal.displayName = this.locDesc;
            this.InventoryLookupTableModal.show(this.target);
        }
      
    }
    getNewLocation() {
        debugger;
        this.oeretHeader.locID =Number(this.InventoryLookupTableModal.id);
        this.locDesc = this.InventoryLookupTableModal.displayName;    
    }
    setLocationIDNull()
    {
        this.oeretHeader.locID=null;
        this.locDesc="";

    }
    //=====================Location Model================

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
