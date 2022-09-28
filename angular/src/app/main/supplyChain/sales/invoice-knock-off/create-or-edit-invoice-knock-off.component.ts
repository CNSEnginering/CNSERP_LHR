import { Component, ViewChild,Injector,Output,EventEmitter,OnInit,ElementRef,AfterViewInit } from '@angular/core';
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import { saleQutationDetailDto } from "../shared/dtos/saleQutationDetail-dto";
import { Opt4LookupTableModalComponent } from '@app/main/supplyChain/inventory/FinderModals/opt4-lookup-table-modal.component';
import { invoiceKnockOffDto } from "../shared/dtos/invoiceKnockOff-dto";
import { invoiceKnockOffService } from '../shared/services/invoice-knock-off.service';
import { throwIfEmpty } from "rxjs/operators";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
import { Lightbox } from "ngx-lightbox";
import { ApprovalService } from '../../periodics/shared/services/approval-service.';
import { AppConsts } from "@shared/AppConsts";
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { TaxAuthoritiesComponent } from "@app/main/commonServices/taxAuthorities/taxAuthorities.component";
import { invoiceKnockOffDetailDto } from '../shared/dtos/invoiceKnockOffDetail-dto';
import { debug } from 'console';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';


@Component({
  selector: 'app-create-or-edit-invoice-knock-off',
  templateUrl: './create-or-edit-invoice-knock-off.component.html',
})
export class CreateOrEditInvoiceKnockOffComponent extends AppComponentBase
implements OnInit, AfterViewInit {

    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
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
    invoiceKnockOffH: invoiceKnockOffDto;
    invoiceKnockOffD: invoiceKnockOffDetailDto;
    invoiceKnockOffD2: invoiceKnockOffDetailDto;
    invoiceKnockOffDData: invoiceKnockOffDetailDto[] = new Array<invoiceKnockOffDetailDto>();
    agGridExtend: AgGridExtend = new AgGridExtend();
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


    columnDefs = [
      {
          headerName: this.l("Inv No"),
          editable: false,
          field: "invNo",
          sortable: true,
          width: 50,
          resizable: true,
      },
      {
        headerName: this.l("Inv Date"),
        editable: false,
        field: "invDate",
        sortable: true,
        width: 100,
        resizable: true,
      },
      {
        headerName: this.l("Amount"),
        editable: false,
        field: "amount",
        sortable: true,
        width: 100,
        resizable: true,
        valueFormatter: this.agGridExtend.formatNumber,
      },
      {
          headerName: this.l("Already Paid"),
          editable: false,
          field: "alreadyPaid",
          width: 100,
          resizable: true,
          valueFormatter: this.agGridExtend.formatNumber,
      },
      {
        headerName: this.l("Pending"),
        editable: false,
        field: "pending",
        sortable: true,
        width: 100,
        resizable: true,
        valueFormatter: this.agGridExtend.formatNumber,
      },
      {
        headerName: this.l("Adjust"),
        editable: true,
        field: "adjust",
        sortable: true,
        width: 100,
        resizable: true,
        valueFormatter: this.agGridExtend.formatNumber,
      },
      { 
        headerName: 'Check',
        field: 'isCheck',
        editable:false,width: 70,
        cellRenderer: params => {
          return `<input type='checkbox' ${params.value ? 'checked' : ''} />`;
        }
  }
  ];
  formValid: boolean = false;
    constructor(
        injector: Injector,
        private _invoiceKnockOffService: invoiceKnockOffService,
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
      if(this.invoiceKnockOffH.debtorCtrlAc=="" || this.invoiceKnockOffH.debtorCtrlAc==null){
        this.message.warn(this.l('Please select Debtor first'),'Account Required');
        return;
    }
        this.target = "CustomerByDebtor";
        this.type= "CustomerByDebtor";
        this.FinanceLookupTableModal.id = String(this.invoiceKnockOffH.custID);
        this.FinanceLookupTableModal.displayName = this.customerName;
        this.FinanceLookupTableModal.show(this.target,this.invoiceKnockOffH.debtorCtrlAc);
    }
    setCustomerIDNull() {
      this.invoiceKnockOffH.custID = null;
      this.customerName = "";
  }
  openSelectChartofACModal(ac) {
      this.target = "Debtors";
      this.type="Debtors";
      debugger
          this.FinanceLookupTableModal.id = this.invoiceKnockOffH.debtorCtrlAc;
          this.FinanceLookupTableModal.displayName = this.chAccountIDDesc;
          this.FinanceLookupTableModal.show(this.target);
     
      //this.target = ac;
  }
  getNewCommonServiceModal() {
    switch (this.target) {
        case "Bank":
        case "Cash":
            this.getNewBankId();
            break;
        case "Currency":
            this.getNewCurrencyRateId();
            break;
        default:
            break;
    }
}

  //=====================Bank Model==================
  openSelectBankIdModal() {
    debugger;
    this.target = this.invoiceKnockOffH.paymentOption;
    this.commonServiceLookupTableModal.id = this.invoiceKnockOffH.bankID;
    this.commonServiceLookupTableModal.accountId = this.invoiceKnockOffH.bAccountID;
    this.commonServiceLookupTableModal.show("Bank", this.invoiceKnockOffH.paymentOption == "Bank" ? "1" : "2");
}


setBankIdNull() {
    this.invoiceKnockOffH.bankID = '';
    this.invoiceKnockOffH.bAccountID = '';
}


getNewBankId() {
    debugger;
    this.invoiceKnockOffH.bankID = this.commonServiceLookupTableModal.id;
    this.invoiceKnockOffH.bAccountID = this.commonServiceLookupTableModal.accountId;
}

//=====================Currency Rate Model================
openSelectCurrencyRateModal() {
  debugger;
  this.target = "Currency";
  this.commonServiceLookupTableModal.id = this.invoiceKnockOffH.curID;
  this.commonServiceLookupTableModal.currRate = this.invoiceKnockOffH.curRate;
  this.commonServiceLookupTableModal.show(this.target);
}


setCurrencyRateIdNull() {
  this.invoiceKnockOffH.curID = '';
  this.invoiceKnockOffH.curRate = null;
}


getNewCurrencyRateId() {
  debugger;
  this.invoiceKnockOffH.curID = this.commonServiceLookupTableModal.id;
  this.invoiceKnockOffH.curRate = this.commonServiceLookupTableModal.currRate;
}
//================Currency Rate Model===============

openInstrumentNo() {
  if (this.invoiceKnockOffH.bAccountID != "" && this.invoiceKnockOffH.bAccountID != null) {
      this.target = "ChequeBookDetail";
      this.type = "ChequeBookDetail";
      this.FinanceLookupTableModal.id = "";
      this.FinanceLookupTableModal.displayName = "";
      this.FinanceLookupTableModal.show("ChequeBookDetail", this.invoiceKnockOffH.bAccountID, "", " Instrument No");
  }
  else {
      this.message.confirm("Please select account first");
  }
}
getChequeBookDetail() {
  debugger
  this.invoiceKnockOffH.insNo = this.FinanceLookupTableModal.id;
}

GetPostedInvoices() {
  if(this.invoiceKnockOffH.debtorCtrlAc=="" || this.invoiceKnockOffH.debtorCtrlAc==null || this.invoiceKnockOffH.custID==0 || this.invoiceKnockOffH.custID==undefined){
    this.message.warn(this.l('Please select Debtor and Customer First'),'Account Required');
    return;
  } 
  this._invoiceKnockOffService.GetPostedInvoices(this.invoiceKnockOffH.debtorCtrlAc,this.invoiceKnockOffH.custID).subscribe(res => {
    var result = res["result"];
    debugger
    if(result.length > 0)
    {
      this.addProcessRecordToGrid(res["result"]);
    }
    else{
      this.message.warn('No Record Found.');
    }
});
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
  debugger
    this.active = true;
    this.invoiceKnockOffH = new invoiceKnockOffDto();
    this.invoiceKnockOffDData = new Array<invoiceKnockOffDetailDto>();

    this.invoiceKnockOffD = new invoiceKnockOffDetailDto();
    this.invoiceKnockOffDData = new Array<invoiceKnockOffDetailDto>();
    this.editMode = false;
    this.totalQty = 0;
    this.tabMode = 0;
    this.formValid = false;
    this.invoiceKnockOffH.paymentOption = "Bank"
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
        this._invoiceKnockOffService.GetDocId().subscribe(res => {
            debugger
            this.invoiceKnockOffH.docNo = res["result"];
            this.invoiceKnockOffH.docDate = new Date();
            this.invoiceKnockOffH.postDate = new Date();
            this.isButtonVisible = false;
            //this.invoiceKnockOffH.mDocDate = new Date();
        });
    } else {
        this.editMode = true;
        this._invoiceKnockOffService.getDataForEdit(id).subscribe((data: any) => {
           debugger
            this.invoiceKnockOffH.id = data["result"]["id"];
           
            this.invoiceKnockOffH=data["result"];
            this.locDesc = data["result"]["locDesc"];
            this.chAccountIDDesc = data["result"]["debtorDesc"];
            this.customerName = data["result"]["customerDesc"];
            this.invoiceKnockOffH.docDate = new Date(
                data["result"]["docDate"]
            );
            this.invoiceKnockOffH.postDate = new Date(
                data["result"]["postDate"]
            );
            debugger
            this.addRecordToGrid(
                data["result"]["invoiceKnockOffDetailDto"]
            );
            this.isButtonVisible= this.invoiceKnockOffH.posted;
        });
    }
    this.modal.show();
}

close(): void {
    this.active = false;
    this.modal.hide();
    this._lightbox.close();
}
getIndex(transType) {
    if (transType == 7) return ++this.fgIndex;
    else if (transType == 9) return ++this.rmIndex;
    else if (transType == 8) return ++this.bpIndex;
}

addProcessRecordToGrid(record: any) {
  //this.editState = true;
      if (record != undefined) {
        var rData = [];
          record.forEach((val, index) => {
            debugger
              var newData;
              newData = {
                  invNo: val.invNo,
                  invDate: val.date,
                  amount:val.amount,
                  alreadyPaid:val.alreadyPaid,
                  pending:val.pending,
                  adjust: 0,
                  isCheck:false
              };
              rData.push(newData)
              //this.gridApi.updateRowData({ add: [newData] });
          });
          this.rowData = [];
          this.rowData = rData;
          //this.editState = false;
      }
}
addRecordToGrid(record: any) {
    //this.editState = true;
        if(record != undefined && (this.invoiceKnockOffH.id != undefined || this.invoiceKnockOffH.id > 0))
        {
          var rData = [];
          var amount = 0;
          record.forEach((val, index) => {
            debugger
              if (val.adjust != "") {
                amount += parseFloat(val.adjust);
              }
              var newData;
              newData = {
                  invNo: val.invNo,
                  invDate: val.invDate,
                  amount:val.amount,
                  alreadyPaid:val.alreadyPaid,
                  pending:val.pending,
                  adjust: val.adjust,
                  isCheck:true
              };
              rData.push(newData)
              //this.gridApi.updateRowData({ add: [newData] });
          });
          this.invoiceKnockOffH.totalAdjust = amount;
          this.rowData = [];
          this.rowData = rData;
        }
}


// addOrUpdateRecordToDetailData(data: any, type: string) {
//   debugger
//   if (type == "record") {
//   } else {
//       var filteredData = this.SaleQutationDData.find(
//           x => x.srNo == data.srNo 
//       );
//       if (filteredData.srNo != undefined) {
          
//           filteredData.itemID = data.itemID;
//           filteredData.remarks = data.remarks;
//           filteredData.conver = data.conver;
//           filteredData.description = data.description;
//           filteredData.unit = data.unit;
//           filteredData.rate=data.rate;
//           filteredData.amount=data.amount;
//           filteredData.transType=data.transId;
//           filteredData.transName=data.transType;
//           filteredData.qty = data.qty;
//           filteredData.taxAuth=data.taxAuth;
//           filteredData.taxClass=data.taxClass,
//           filteredData.taxClassDesc=data.taxClassDesc;
//           filteredData.taxRate=data.taxRate,
//           filteredData.netAmount=data.netAmount,
//           filteredData.taxAmt=data.taxAmt

//       }
   
//   }
  
//   //this.totalItems = this.SaleQutationDData.length;

// }


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

//=====================Item Model================
openSelectItemModal(data:any) {
  debugger;
  this.target = "ItemsQ";
  this.inventoryLookupTableModal.id = this.setParms.data.itemID;
  this.inventoryLookupTableModal.displayName = this.setParms.data.description;
  this.inventoryLookupTableModal.unit = this.setParms.data.unit;
  this.inventoryLookupTableModal.conver = this.setParms.data.conver;
  this.inventoryLookupTableModal.show(this.target,data);
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
  //this.addOrUpdateRecordToDetailData(this.setParms.data, "");
  this.onBtStartEditing(this.setParms.rowIndex, "qty");
}
getOpt4() {
  debugger
  this.setParms.data.transId = this.inventoryLookupTableModal.id;
  this.setParms.data.transName = this.inventoryLookupTableModal.displayName;
  this.gridApi.refreshCells();
  //this.addOrUpdateRecordToDetailData(this.setParms.data, "");
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




  onCellValueChanged(params) {
      debugger;
      // if (params.data.qty != null && params.data.rate != null) {
      //     params.data.amount = parseFloat(params.data.qty) * parseFloat(params.data.rate);
      //     params.data.netAmount =    params.data.amount;
      // }
      // if (params.data.taxRate != null && params.data.description != null) {
      //     params.data.taxAmt = Math.round((parseFloat(params.data.amount) * parseFloat(params.data.taxRate)) / 100);
      //     params.data.netAmount = parseFloat(params.data.amount) + parseFloat(params.data.taxAmt) ;
      // }
      this.calculations();
      this.gridApi.refreshCells();
      //this.addOrUpdateRecordToDetailData(params.data, "");
  }
  calculations() {
    debugger
      var amount = 0;
      this.gridApi.forEachNode(node => {
        debugger
          if (node.data.adjust != "") {
              amount += parseFloat(node.data.adjust);
          }
      });
      this.invoiceKnockOffH.totalAdjust = amount;
   }
  getNewInventoryModal() {
      debugger
      switch (this.target) {
          case "ItemsQ":
              this.getNewItemId();
              break;
          case "loc":
              this.invoiceKnockOffH.gllocid =
              Number(this.inventoryLookupTableModal.id) == 0
                  ? undefined
                  : Number(this.inventoryLookupTableModal.id);
          this.locDesc = this.inventoryLookupTableModal.displayName;
              break;
          default:
              break;
      }
  }

  
   //---------------------Location-------------------------//
   openLocationModal() {
    this.target = "GLLocation";
    this.type = "GLLocation";
    this.FinanceLookupTableModal.id = String(this.invoiceKnockOffH.gllocid);
    this.FinanceLookupTableModal.displayName = this.locDesc;
    this.FinanceLookupTableModal.show(this.target);
}
setLocationNull() {
  this.invoiceKnockOffH.gllocid = null;
    this.locDesc = "";
}
getNewLocation() {
    debugger;
    this.invoiceKnockOffH.gllocid = Number(this.FinanceLookupTableModal.id);
    this.locDesc = this.FinanceLookupTableModal.displayName;
}
 

  getLookUpData() {
      debugger
      if (this.type == "GLLocation") {
        this.getNewLocation();
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
              //this.addOrUpdateRecordToDetailData(this.paramsData.data, "");
          
            

      }else if(this.type=="Customer"){
          this.getNewCustomer();
      }
      else if(this.type=="Debtors"){
          this.invoiceKnockOffH.debtorCtrlAc = this.FinanceLookupTableModal.id;
          this.chAccountIDDesc = this.FinanceLookupTableModal.displayName;
      }
      else if(this.type=="CustomerByDebtor"){
        this.invoiceKnockOffH.custID = parseInt(this.FinanceLookupTableModal.id);
        this.customerName = this.FinanceLookupTableModal.displayName;
    }
    else if(this.type=="Customer"){
      this.getNewCustomer();
    }
    else if(this.type=="Customer"){
      this.getNewCustomer();
    }
    else if(this.type=="ChequeBookDetail"){
      this.getChequeBookDetail();
    }
    
    
  }
  setCAIdNull(){
    this.invoiceKnockOffH.debtorCtrlAc =null;
      this.chAccountIDDesc = "";
  }
  getNewCustomer() {
      debugger;
      if (this.FinanceLookupTableModal.id != "null") {
          this.invoiceKnockOffH.custID = Number(this.FinanceLookupTableModal.id);
          this.customerName = this.FinanceLookupTableModal.displayName;
      }
  }
  
 
  changeOption(value) {
    this.invoiceKnockOffH.bankID = "";
    this.invoiceKnockOffH.bAccountID = "";
    this.invoiceKnockOffH.insNo = "";
    this.invoiceKnockOffH.insType = null;
  }
  processInv(target: string): void {
    debugger;
    this.message.confirm(
        'Process ' + target,
        (isConfirmed) => {
            if (isConfirmed) {
                this.saving = true;

                this._invoiceKnockOffService.processInvoice(this.invoiceKnockOffH).subscribe(result => {
                    debugger
                    if (result["result"] == "Save") {
                        this.saving = false;
                        this.notify.info(this.l('ProcessSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    } else {
                        this.saving = false;
                        this.notify.error(this.l('ProcessFailed'));
                    }
                });
            }
        }
    );
}
  save() {
      this.message.confirm("Save", isConfirmed => {
          if (isConfirmed) {
              this.saving = true;
              debugger
              // Validations
              if(this.invoiceKnockOffH.amount != this.invoiceKnockOffH.totalAdjust)
              {
                this.message.warn("Adjusted amount should be equal to Total Amount");
                  this.saving = false;
                  return;
              }
              var no = 0;
              this.gridApi.forEachNode(node => {
                if (node.data.isCheck == true && node.data.adjust != undefined) {
                  no++;
                }
              });
              if(no == 0)
              {
                this.message.warn("Please select row and Add Adjust Amount");
                this.saving = false;
                return;
              }
              
              var rowData = [];
              this.gridApi.forEachNode(node => {
                if (node.data.isCheck == true && node.data.adjust != undefined) {
                  rowData.push(node.data);
                }
              });
              this.invoiceKnockOffH.invoiceKnockOffDetailDto = rowData;
              debugger
              this._invoiceKnockOffService.create(this.invoiceKnockOffH).subscribe(() => {
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
    onCellClicked(params) {
      
      if(params.column["colId"] == "isCheck")
      {
          var isCheck=params.data.isCheck;
          if(isCheck==false){
            params.data.isCheck=true;
          }else if(isCheck==true){
            params.data.isCheck=false;
          }
      }
     //this.gridApi.refreshCells();
  }
  removeRecordFromGrid() {
      

      var selectedData = this.gridApi.getSelectedRows();
      //this.gridApi.updateRowData({ remove: selectedData });
     // this.gridApi.refreshCells();

     debugger
      // var filteredDataIndex = this.invoiceKnockOffDData.findIndex(
      //     x => x.srNo == selectedData[0].srNo
      // );
      // this.invoiceKnockOffDData.splice(filteredDataIndex, 1);
      // this.gridApi.updateRowData({ remove: selectedData });
      //     this.gridApi.refreshCells();

      // this.totalItems = this.invoiceKnockOffDData.length;
      // //this.calculateTotalQty();
      // this.editState = false;
  }

  // dateChange(event: any) {
  //     this.assembly.docDate = event;
  // }

  
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
