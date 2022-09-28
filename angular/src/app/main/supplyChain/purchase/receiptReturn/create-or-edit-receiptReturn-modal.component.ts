import { Component, ViewChild, Injector, Output, EventEmitter, ɵpublishDefaultGlobalUtils} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { GetDataService } from '../../inventory/shared/services/get-data.service';
import { CreateOrEditPORETHeaderDto } from '../shared/dtos/poretHeader-dto';
import { CreateOrEditPORETDetailDto } from '../shared/dtos/poretDetails-dto';
import { ReceiptReturnDto } from '../shared/dtos/receiptReturn-dto';
import { PORETHeadersService } from '../shared/services/poretHeader.service';
import { PORETDetailsService } from '../shared/services/poretDetail.service';
import { ReceiptReturnServiceProxy } from '../shared/services/receiptReturn.service';
import { PurchaseLookupTableModalComponent } from '@app/finders/supplyChain/purchase/purchase-lookup-table-modal.component';
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
import { LogComponent } from '@app/finders/log/log.component';

@Component({
    selector: 'createOrEditReceiptReturnModal',
    templateUrl: './create-or-edit-receiptReturn-modal.component.html'
})
export class CreateOrEditReceiptReturnModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('PurchaseLookupTableModal', { static: true }) PurchaseLookupTableModal: PurchaseLookupTableModalComponent;
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
    chartofAccDesc:string;
    subAccDesc:string;
    whTermDesc:string;
    taxAuthorityDesc:string;
    taxClassDesc: string;
    status="Incomplete";

    poretHeader: CreateOrEditPORETHeaderDto = new CreateOrEditPORETHeaderDto();
    poretDetail: CreateOrEditPORETDetailDto = new CreateOrEditPORETDetailDto();
    receiptReturn: ReceiptReturnDto=new ReceiptReturnDto();

            auditTime: Date;
            docDate:Date;
    processing=false;
    costCenterDesc: string;
    taxAmount: number;
    errorFlag: boolean;
    IsVenderID: boolean;
    poDocNo: number;
    LocCheckVal:boolean;
    checkedval:boolean;

    constructor(
        injector: Injector,
        private _poretHeadersServiceProxy: PORETHeadersService,
        private _poretDetailServiceProxy: PORETDetailsService,
        private _receiptReturnServiceProxy: ReceiptReturnServiceProxy,
        private _approvelService: ApprovalService,
        private _getDataService: GetDataService
    ) {
        super(injector);
    }

    show(poretHeaderId?: number,maxId?: number): void {
    this.auditTime = null; 
    debugger;

    this.getLocations("ICLocations");

        if (!poretHeaderId) {debugger;
            this.poretHeader = new CreateOrEditPORETHeaderDto();
            this.poretHeader.id = poretHeaderId;
            this.poretHeader.docDate =moment().endOf('day');
            this.poretHeader.billDate =moment().endOf('day');
            this.poretHeader.docNo=maxId;
            this.poretHeader.locID=0;
            this.poretHeader.totalQty = 0;
            this.poretHeader.billAmt=0;
            this.poretHeader.totalAmt=0;
            this.taxAmount=0;
            this.poretHeader.addExp=0;
            this.totalItems=0;

            this.active = true;
            this.modal.show();
        } else {
            this._poretHeadersServiceProxy.getPORETHeaderForEdit(poretHeaderId).subscribe(result => {
                this.poretHeader = result;
                debugger;
                if (this.poretHeader.audtDate) {
					this.auditTime = this.poretHeader.audtDate.toDate();
                }

                this.chartofAccDesc=result.accDesc;
                this.subAccDesc=result.subAccDesc;
                this.costCenterDesc=result.ccDesc;
                this.poDocNo=result.poDocNo;

                this.LockDocDate=this.poretHeader.docDate.toDate();

                this._poretDetailServiceProxy.getPORETDData(poretHeaderId).subscribe(resultD => {
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
                    this.poretHeader.totalQty=qty;
                    this.poretHeader.billAmt=amount;
                    this.taxAmount=taxAmt;
                });

                this.active = true;
                this.modal.show();
            });
        }
    }
    SetDefaultRecord(result:any){
        console.log(result);
          this.poretHeader.locID=result.currentLocID;
          //this.poretHeader.locDesc=result.currentLocName;
          this.checkedval=result.cDateOnly;
          if(result.allowLocID==false){
              this.LocCheckVal=true;
          }else{
            this.LocCheckVal=false;
          }
          //this.typeDesc=result.transTypeName;
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
        {headerName: this.l('Received'), field: 'pQty',editable: false,sortable: true,filter: true,width:100,type: "numericColumn",resizable: true}, 
        {headerName: this.l('Qty'), field: 'qty',editable: true,sortable: true,filter: true,width:100,type: "numericColumn",resizable: true}, 
        {headerName: this.l('Rate'), field: 'rate',editable: false,sortable: true,filter: true,width:100,type: "numericColumn",resizable: true}, 
        {headerName: this.l('Amount'), field: 'amount',sortable: true,width:100,editable: false,type: "numericColumn",resizable: true},
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

    onBtStartEditing(index,col) {
        debugger;
        this.gridApi.setFocusedCell(index, col);
        this.gridApi.startEditingCell({
          rowIndex: index,
          colKey: col
        });
      }

    calculations(){
        debugger;
        var items=0;
        var qty=0;
        var amount=0;
        var taxAmt=0;
        var addnlExp=0;
        this.gridApi.forEachNode(node=>{
            debugger;
            if((node.data.amount != "" || node.data.qty != "") && node.data.itemID!=""){
                qty+=parseFloat(node.data.qty);
                amount+=parseFloat(node.data.amount);
            }
            items=items+1;
            taxAmt+=parseFloat(node.data.taxAmt);
        });
        this.totalItems=items;
        this.poretHeader.addExp=addnlExp;
        this.poretHeader.totalQty=qty;
        this.poretHeader.billAmt=amount;
        this.taxAmount=taxAmt;
        this.poretHeader.totalAmt=this.poretHeader.billAmt+this.taxAmount+this.poretHeader.addExp; 
    } 

    onCellValueChanged(params){
        debugger;
        if(params.data.qty!=null && params.data.rate!=null){
            params.data.amount=parseFloat(params.data.qty)*parseFloat(params.data.rate);
        }
        if(params.data.taxRate!=null && params.data.taxAmt!=null){
            params.data.taxAmt=Math.round((parseFloat(params.data.amount)*parseFloat(params.data.taxRate))/100);
            params.data.netAmount=parseFloat(params.data.amount)+parseFloat(params.data.taxAmt);
        }
        this.calculations();
        this.gridApi.refreshCells();
    }

    //==================================Grid=================================


    getReceiptNoRecord():void{
        
        if(this.poretHeader.locID==null || this.poretHeader.locID==0){
            this.message.warn("Please select location","Location Required");
        }else if(this.poretHeader.recDocNo==null){ 
            this.message.warn(this.l("Please Enter or Pick Receipt No"),'Receipt No Required'); 
        }else{
            this._receiptReturnServiceProxy.getReceiptNoHeaderRecord(this.poretHeader.locID,this.poretHeader.recDocNo).subscribe(result => {
                 debugger
                if(result.accountID==undefined){
                    this.message.info(this.l("No record found for Receipt No "+this.poretHeader.recDocNo),'Empty Result');
                    this.rowData=[]; 
                    return;
                }
                result.recDocNo=this.poretHeader.recDocNo;
                this.poretHeader = result;
                this.poretHeader.posted = false;
                this.poretHeader.approved = false;
                if (this.poretHeader.audtDate) {
					this.auditTime = this.poretHeader.audtDate.toDate();
                }
                this.poretHeader.id=null;
                this.chartofAccDesc=result.accDesc;
                this.subAccDesc=result.subAccDesc;
                this.costCenterDesc=result.ccDesc;
                this.poDocNo=result.poDocNo;
                this.poretHeader.docDate =moment().endOf('day');
                
                this._receiptReturnServiceProxy.getReceiptNoRecords(this.poretHeader.locID,this.poretHeader.recDocNo).subscribe(resultD => {
                    debugger;
                    if(resultD.totalCount==0){
                        this.message.info(this.l("No record found for Receipt No "+this.poretHeader.recDocNo),'Empty Result'); 
                        this.rowData=[];
                        return;
                    }
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

                    this.rowData=[];
                    this.rowData=rData;   

                    this.poretDetail.id=null;
                    this.totalItems=items;
                    this.poretHeader.totalQty=qty;
                    this.poretHeader.billAmt=amount;
                    this.poretHeader.addExp=0;
                    this.taxAmount=0;
                    this.poretHeader.totalAmt=this.poretHeader.billAmt+this.taxAmount+this.poretHeader.addExp; 
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
                    debugger
                    // if(moment(this.poretHeader.docDate)>moment().endOf('day')){
                    //     this.message.warn("Document date greater than current date","Document Date Error");
                    //     return;
                    // }

                    // if(moment(this.poretHeader.billDate)>moment().endOf('day')){ 
                    //     this.message.warn("Invoice date greater than current date","Invoice Date Error"); 
                    //     return;
                    // } 

                    if((moment(this.LockDocDate).month()+1)!=(moment(this.poretHeader.docDate).month()+1) && this.poretHeader.id!=null ){
                        this.message.warn('Document month not changeable',"Document Month Error");
                        return;
                    }

                    if(this.poretHeader.locID==null || this.poretHeader.locID==0){
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
                        }else if(node.data.pQty!=0 && node.data.pQty<node.data.qty && this.poDocNo!=undefined){
                            this.message.warn("Qty not greater than POQty at row "+ Number(node.rowIndex+1),"Qty Greater");
                            this.errorFlag=true;
                            return;
                        }else if(node.data.qty!=0 && node.data.pQty>node.data.qty && this.poDocNo!=undefined){
                            this.errorFlag=false;
                            return;
                        }
                    });

                    if(this.errorFlag){
                        return;
                    }

                    if(this.poretHeader.totalAmt<=0 || this.poretHeader.totalQty<=0){
                        this.message.warn("Qty OR Amount not less than OR equal to zero","Qty OR Amount Zero");
                        return;
                    }
            
                    this.saving = true;
            
                    var rowData=[];
                    this.gridApi.forEachNode(node=>{
                        rowData.push(node.data);
                    });
            

                    if(moment(new Date()).format("A")==="AM" && !this.poretHeader.id   && (moment(new Date()).month()+1)==(moment(this.poretHeader.docDate).month()+1)){
                        this.poretHeader.docDate =moment(this.poretHeader.docDate);
                    }else{
                        this.poretHeader.docDate =moment(this.poretHeader.docDate).endOf('day');
                    }

                    //this.poretHeader.active=true;
                    
                    this.receiptReturn.poretDetail=rowData;
                    this.receiptReturn.poretHeader=this.poretHeader; 
        
                    this._receiptReturnServiceProxy.createOrEditReceiptReturn(this.receiptReturn)
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

    approveDoc(id: number,mode, approve) {
        debugger;
        this.message.confirm(
            approve?'Approve Receipt Return':'Unapprove Receipt Return',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._approvelService.ApprovalData("receiptReturn", [id], mode, approve)
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
        this.getNewReceiptNo();
    }

    OpenLog(){
        debugger
        this.LogTableModal.show(this.poretHeader.docNo,'ReceiptReturn');
    }
    //=====================Receipt No Model================
    openSelectReceiptNoModal() {
        debugger;
        if(this.poretHeader.locID==null || this.poretHeader.locID==0){
            this.message.warn("Please select location","Location Required");
            return;
        }
        this.target="ReceiptNo";
        this.PurchaseLookupTableModal.id = this.poretHeader.accountID;
        this.PurchaseLookupTableModal.show(this.target,String(this.poretHeader.locID));
    }

    setReceiptNoNull() {
        this.poretHeader.recDocNo = 0;
    } 

    getNewReceiptNo() {
        debugger;
        this.poretHeader.recDocNo =Number(this.PurchaseLookupTableModal.id);
    }
    //=====================Receipt No Model================

    close(): void {

        this.active = false;
        this.modal.hide();
    }


    processReceiptReturn():void{
        debugger;
        this.message.confirm(
            'Process Receipt Return',
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing = true;
                    debugger;

                    if(moment(new Date()).format("A")==="AM" && !this.poretHeader.id   && (moment(new Date()).month()+1)==(moment(this.poretHeader.docDate).month()+1)){
                        this.poretHeader.docDate =moment(this.poretHeader.docDate);
                    }else{
                        this.poretHeader.docDate =moment(this.poretHeader.docDate).endOf('day');
                    }

                this._receiptReturnServiceProxy.processReceiptReturn(this.poretHeader).subscribe(result => {
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
}
