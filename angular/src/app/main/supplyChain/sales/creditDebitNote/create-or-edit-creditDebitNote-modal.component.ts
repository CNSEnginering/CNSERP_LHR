import { Component, ViewChild, Injector, Output, EventEmitter, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap';
import { CreditDebitNoteService } from '../shared/services/creditDebitNote.service';
import {  CreditDebitNoteDto } from '../shared/dtos/creditDebitNote-dto';
import {  CreditDebitNoteDetailDto } from '../shared/dtos/creditDebitNoteDetail-dto';
import { VoucherPostingDto } from '../../periodics/shared/dto/voucher-posting-dto';
import { VoucherPostingServiceProxy } from '../../periodics/shared/services/voucher-posting-service';
import { timingSafeEqual } from 'crypto';


@Component({
    selector: 'CreditDebitNoteModal',
    templateUrl: './create-or-edit-creditDebitNote-modal.component.html'
})
export class CreateOrEditCreditDebitNoteModalComponent extends AppComponentBase {
    editState: boolean = false;
    saveBtnShow:boolean = true;
    processing=false;
    noteType:string;
    gridApi;
    gridColumnApi;
    rowData;
    rowSelection;
    paramsData;
    target: string;
    type: string;
    totalQty:number;
    totalItems:number;
    totalAmount:Number;
    creditDebitNote: CreditDebitNoteDto;
    creditDebitNoteDetail: CreditDebitNoteDetailDto;
    creditDebitNoteDetailData: CreditDebitNoteDetailDto[] = new Array<CreditDebitNoteDetailDto>();
    columnDefs = [
        { headerName: this.l('SrNo'), editable: false, field: 'srNo', sortable: true, width: 100, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('Item'), editable: false, field: 'item', sortable: true, filter: true, width: 200, resizable: true },
        { headerName: this.l('Description'), editable: false, field: 'description', sortable: true, filter: true, width: 200, resizable: true },
        { headerName: this.l('UOM'), editable: false, field: 'unit', sortable: true, filter: true, width: 150, resizable: true },
        { headerName: this.l('Conver'), editable: false, field: 'conver', sortable: true, filter: true, width: 150, resizable: true },
        { headerName: this.l('Qty'), editable: true, field: 'qty', width: 100, resizable: true },
        { headerName: this.l('Rate'), editable: true, field: 'rate', width: 100, resizable: true },
        { headerName: this.l('Amount'), editable: false, field: 'amount', sortable: true, width: 150, resizable: true },
        { headerName: this.l('Remarks'), editable: true, field: 'remarks', sortable: true, width: 150, resizable: true },
    ];
    formValid: boolean = false;
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    //@ViewChild('InventoryGlLinkLookupTableModal', { static: true }) InventoryGlLinkLookupTableModal: InventoryGlLinkLookupTableModalComponent;
    @ViewChild('financeLookupTableModal', { static: true }) financeLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('inventoryLookupTableModal', { static: true }) inventoryLookupTableModal: InventoryLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    
    voucherPosting: VoucherPostingDto = new VoucherPostingDto();
    constructor(
        injector: Injector,
        private _creditDebitNoteService: CreditDebitNoteService,
        private _voucherPostingServiceProxy:VoucherPostingServiceProxy
    ) {
        super(injector);
    }

    getDocNo() {
        this._creditDebitNoteService.GetDocId().subscribe(
            (data: any) => {
                this.creditDebitNote.docNo = data["result"];
                this.checkFormValid();
            }
        )
    }

    openTRTypeID(){
        this.target = "TransactionType";
        this.inventoryLookupTableModal.show("TransactionType");
    }

    show(id?: number, type?:string): void {

        this.active = true;
        this.creditDebitNote = new CreditDebitNoteDto();
        this.creditDebitNoteDetail = new CreditDebitNoteDetailDto();
        this.creditDebitNoteDetailData = new Array<CreditDebitNoteDetailDto>();
        this.totalQty = 0;
        this.totalItems = 0;
        this.totalAmount = 0;
        if(type == "CreditNote")
        {
            this.creditDebitNote.transType = "2";
            this.creditDebitNote.typeID = 1;
            this.noteType = "CreditNote";
        }
        else  if(type == "DebitNote")
        {
            this.creditDebitNote.transType = "1";
            this.creditDebitNote.typeID = 2;
            this.noteType = "DebitNote";
        }

        if (!id) {     
            this.creditDebitNote.docDate = new Date();
            this.creditDebitNote.postingDate = new Date();
            this.creditDebitNote.paymentDate = new Date();
            //this.creditDebitNote.typeID = 1;
            this.getDocNo();
        }
        else {
            this.formValid = true;
            this.editState = false;
             this.primengTableHelper.showLoadingIndicator();
             this._creditDebitNoteService.getDataForEdit(id).subscribe(data => {

                this.creditDebitNote.id = data["result"]["creditDebitNote"]["id"];
                this.creditDebitNote.locID = data["result"]["creditDebitNote"]["locID"];
                this.creditDebitNote.locDesc = data["result"]["creditDebitNote"]["locDesc"];
                this.creditDebitNote.accountName = data["result"]["creditDebitNote"]["accountDesc"];
                this.creditDebitNote.subAccName = data["result"]["creditDebitNote"]["subAccDesc"];
                this.creditDebitNote.typeID = data["result"]["creditDebitNote"]["typeID"];
                this.creditDebitNote.docDate  =  new Date(data["result"]["creditDebitNote"]["docDate"]);
                this.creditDebitNote.docNo  = data["result"]["creditDebitNote"]["docNo"];
                this.creditDebitNote.narration  = data["result"]["creditDebitNote"]["narration"];
                this.creditDebitNote.postingDate  =  new Date(data["result"]["creditDebitNote"]["postingDate"]);
                this.creditDebitNote.paymentDate  = new Date( data["result"]["creditDebitNote"]["paymentDate"]);
                this.creditDebitNote.subAccID  = data["result"]["creditDebitNote"]["subAccID"];
                this.creditDebitNote.accountID  = data["result"]["creditDebitNote"]["accountID"];
                this.creditDebitNote.transType  = data["result"]["creditDebitNote"]["transType"];
                this.creditDebitNote.trTypeID  = data["result"]["creditDebitNote"]["trTypeID"];
                this.creditDebitNote.trTypeDesc  = data["result"]["creditDebitNote"]["trTypeDesc"];

                data["result"]["creditDebitNote"]["posted"] == "1" ? (this.saveBtnShow = false) : this.saveBtnShow = true;
                this.addRecordToGrid(data["result"]["creditDebitNote"]["creditDebitNoteDetailDto"]);
                this.checkFormValid();
            });
        }
        this.modal.show();
    }

    save(): void {
       this.saving = true;
       this.creditDebitNote.totalQty = this.totalQty;
       this.creditDebitNote.creditDebitNoteDetailDto.length = 0;
       this.creditDebitNote.creditDebitNoteDetailDto.push(...this.creditDebitNoteDetailData.slice());
       this._creditDebitNoteService.create(this.creditDebitNote)
      //this._creditDebitNoteService.create(this.creditDebitNoteDetailData.slice())
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
        this.close();
    }

    getNewFinanceModal() {
        if(this.target == "ChartOfAccount")
          this.getNewChartOfAC();
          else if (this.target == "SubLedger")
          this.getNewSubAC();

          this.checkFormValid();
    }

    getNewSubAC()
    {
        this.creditDebitNote.subAccID = +this.financeLookupTableModal.id == 0
        ? undefined : +this.financeLookupTableModal.id;
        this.creditDebitNote.subAccName = this.financeLookupTableModal.displayName;
        this.checkFormValid();
    }

    getNewInventoryModal() {
        if (this.target == "Location")
            this.getNewLocation();
         else if (this.target == "Items")
         {
                this.paramsData.data.item = this.inventoryLookupTableModal.id;
                this.paramsData.data.description = this.inventoryLookupTableModal.displayName;
                this.paramsData.data.unit = this.inventoryLookupTableModal.unit;
                this.paramsData.data.conver = this.inventoryLookupTableModal.conver;
                this.paramsData.data.amount = 0;
                this.gridApi.refreshCells();
                this.addOrUpdateRecordToDetailData(this.paramsData.data, '');
                this.checkEditState();
                this.checkFormValid();
         }
         else if( this.target == "TransactionType")
         {
             this.creditDebitNote.trTypeID = this.inventoryLookupTableModal.id;
             this.creditDebitNote.trTypeDesc = this.inventoryLookupTableModal.displayName;
             this.GetAccIdAgainstTransTypeAndLoc();
         }
         this.checkFormValid();
    }

    //////////////////////////////////////Chart of Account///////////////////////////////////////////////
    openSelectChartofACModal() {
        this.target = "ChartOfAccount";
        this.financeLookupTableModal.id = this.creditDebitNote.accountID;
        this.financeLookupTableModal.displayName = this.creditDebitNote.accountName;
        this.financeLookupTableModal.show(this.target,this.creditDebitNote.transType);
    }

    openSubACModal() {
        if(this.creditDebitNote.accountID !=  "" &&
        this.creditDebitNote.accountID != undefined
        )
        {
            this.target = "SubLedger";
            this.financeLookupTableModal.id = "";
            this.financeLookupTableModal.displayName = "";
            this.financeLookupTableModal.show(this.target,this.creditDebitNote.accountID);
        }
        else
        {
            this.notify.info("Please Select The Account First");
        }

    }

    getNewChartOfAC() {
      this.creditDebitNote.accountID = this.financeLookupTableModal.id;
      this.creditDebitNote.accountName = this.financeLookupTableModal.displayName;
    }

    openLocationModal() {
        this.target = "Location";
        this.inventoryLookupTableModal.id = "";
        this.inventoryLookupTableModal.displayName = "";
        this.inventoryLookupTableModal.show(this.target);
        
    }
    getNewLocation() {
        this.creditDebitNote.locID = Number(this.inventoryLookupTableModal.id) == 0 ?
        undefined : Number(this.inventoryLookupTableModal.id) ;
        this.creditDebitNote.locDesc = this.inventoryLookupTableModal.displayName;
        this.GetAccIdAgainstTransTypeAndLoc();
    }


    close(): void {
        this.active = false;
        this.modal.hide();
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
            this.target = "Items";
            this.inventoryLookupTableModal.show("Items");
        }
    }
    removeRecordFromGrid() {
        var selectedData = this.gridApi.getSelectedRows();
        var filteredDataIndex = this.creditDebitNoteDetailData.findIndex(x => x.srNo == selectedData[0].srNo);
        this.creditDebitNoteDetailData.splice(filteredDataIndex, 1);
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        this.totalItems = this.creditDebitNoteDetailData.length;
        this.calculateTotalQty();
        this.calculateTotalAmt();
        //this.checkEditState();
        this.editState = false;
        this.checkFormValid();
    }
    addRecordToGrid(record: any) {
        this.editState = true;
        if (record != undefined) {
            record.forEach((val, index) => {
                console.log(record);
                var str = val.itemID.split('*');
                var newData;
                newData = {
                    srNo: index,
                    item: str[0],
                    description: str[1],
                    unit: str[2],
                    conver: str[3],
                    qty: val.qty,
                    rate: val.rate,
                    amount: val.amount,
                    remarks: val.remarks,
                    //maxQty: val.qty
                }

                this.addOrUpdateRecordToDetailData(newData, 'record');

                this.gridApi.updateRowData({ add: [newData] });
            });
            this.editState = false;
            this.checkFormValid();
        }
        else {
            let length = this.creditDebitNoteDetailData.length;
            var newData = {
                srNo: ++length,
                item: undefined,
                description: undefined,
                unit: undefined,
                conver: undefined,
                qty: undefined,
                remarks: undefined,
                rate: undefined,
                amount: undefined,
                // maxQty: undefined
            };
            this.addOrUpdateRecordToDetailData(newData, 'record');

            this.gridApi.updateRowData({ add: [newData] });
        }
        this.checkFormValid();
    }


    checkFormValid() {
        if (this.creditDebitNote.docDate == null || this.creditDebitNote.docNo == undefined
            || this.creditDebitNoteDetailData.length == 0 || this.totalQty == 0
            || this.editState == true || this.creditDebitNote.locID == undefined
            || this.creditDebitNote.locID == 0 ||
            this.creditDebitNote.subAccID == undefined
            || this.creditDebitNote.subAccID == 0 ||
            this.creditDebitNote.accountID == undefined
            || this.creditDebitNote.accountID == ""
            || this.creditDebitNote.docDate > new Date()
        ) {
            this.formValid = false;
        }
        else {
            this.formValid = true;
        }
    }

    addOrUpdateRecordToDetailData(data: any, type: string) {
        debugger
        if (type == 'record') {
           // this.creditDebitNote = new CreditDebitNoteDto();
            this.creditDebitNoteDetail = new CreditDebitNoteDetailDto();
            this.creditDebitNoteDetail.srNo = data.srNo;
            this.creditDebitNoteDetail.itemId = data.item;
            this.creditDebitNoteDetail.description = data.description;
            this.creditDebitNoteDetail.qty = data.qty;
            this.creditDebitNoteDetail.rate = data.rate;
            this.creditDebitNoteDetail.unit = data.unit;
            this.creditDebitNoteDetail.conver = data.conver;
            this.creditDebitNoteDetail.amount = data.amount;
            this.creditDebitNoteDetail.remarks = data.remarks;
            this.creditDebitNoteDetailData.push(this.creditDebitNoteDetail);
        }
        else {
            var filteredData = this.creditDebitNoteDetailData.find(x => x.srNo == data.srNo);
            if (filteredData.srNo != undefined) {
                filteredData.itemId = data.item;
                filteredData.conver = data.conver;
                filteredData.unit = data.unit;
                filteredData.description = data.description;
                filteredData.qty = data.qty;
                filteredData.rate = data.rate;
                filteredData.amount = data.amount;
                filteredData.remarks = data.remarks;
                if(filteredData.qty != undefined && filteredData.rate != undefined)
                {
                    filteredData.amount = Number(filteredData.qty) * Number(filteredData.rate);                          
                    data.amount =   Number(filteredData.qty) * Number(filteredData.rate);
                }           
                this.gridApi.refreshCells();
            }
        }
        this.totalItems = this.creditDebitNoteDetailData.length;
        this.calculateTotalQty();
        this.calculateTotalAmt();
    }

    calculateTotalQty() {
        let qty = 0;
        this.creditDebitNoteDetailData.forEach((val, index) => {
            qty = qty + Number(val.qty);
            if (!isNaN(qty)) {
                this.totalQty = qty;
                this.creditDebitNote.totalQty = qty;
            }
        });
        this.creditDebitNoteDetailData.length == 0 ? this.totalQty = 0 : "";
        this.checkFormValid();
    }


    calculateTotalAmt() {
        let amt = 0;
        this.creditDebitNoteDetailData.forEach((val, index) => {
            amt = amt + Number(val.amount);
            if (!isNaN(amt)) {
                this.totalAmount = amt;
                this.creditDebitNote.totAmt = amt;
            }
        });
        this.creditDebitNoteDetailData.length == 0 ? this.totalAmount = 0 : "";
        this.checkFormValid();
    }
    cellValueChanged(params) {
        this.addOrUpdateRecordToDetailData(params.data, '')
       // this.calculateTotalQty();
        this.checkEditState();
        if (params.data.qty <= 0) {
            this.notify.info("Qty Should Be Greater Than Zero");
        }
       // this.checkFormValid();
    }

    cellEditingStarted(params) {
        this.formValid = false;
    }

    checkEditState() {
        if (this.paramsData.data.item != '' && this.paramsData.data.qty > 0
        && this.paramsData.data.rate > 0  && this.paramsData.data.amount > 0
        ) {
            this.editState = false;
        }
        else {
            this.editState = true;
        }
    }
    dateChange(event:any){
        this.creditDebitNote.docDate = event;
        this.checkFormValid();
    }
    // typeChange(event:any){
    //   console.log(this.creditDebitNote.typeID);
    // }

    GetAccIdAgainstTransTypeAndLoc()
    {
        if(this.creditDebitNote.locID > 0 && this.creditDebitNote.locID != undefined
            && this.creditDebitNote.trTypeID != undefined &&
            this.creditDebitNote.trTypeDesc != ""
            )
            {
                this._creditDebitNoteService.GetAccIdAgainstTransTypeAndLoc(this.creditDebitNote.locID,
                    this.creditDebitNote.trTypeID).subscribe(
                    data => {
                        var str = data["result"].split('*');
                        this.creditDebitNote.accountID = str[0];
                        this.creditDebitNote.accountName = str[1];    
                        this.checkFormValid();               
                    }
                )          
            }
            this.checkFormValid();
    }

    process()
    {
        debugger
        this.message.confirm(
            'Process Voucher Posting',
            (isConfirmed) => {
              if (isConfirmed) {
                this.saving = true;
                this.processing = true;
                this.noteType == "CreditNote" ?  this.voucherPosting.creditNote = true : this.voucherPosting.debitNote = true;
                this.voucherPosting.fromDoc = this.creditDebitNote.docNo;
                this.voucherPosting.toDoc = this.creditDebitNote.docNo;
                this._voucherPostingServiceProxy.processVoucherPosting(this.voucherPosting).subscribe(result => {
                  if (result == "Save") {
                    this.saving = false;
                    this.processing = false;
                    this.notify.info(this.l('ProcessSuccessfully'));
                    this.modalSave.emit(null);
                    this.formValid = false;
                  } else {
                    this.saving = false;
                    this.processing = false;
                    this.notify.error(this.l('ProcessFailed'));
                  }
                });
              }
            }
          );
    }
}
