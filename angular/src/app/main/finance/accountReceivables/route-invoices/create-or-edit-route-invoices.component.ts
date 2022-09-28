import { ARINVDDto } from './../../shared/dto/arinvd-dto';
import { Lightbox } from 'ngx-lightbox';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ARINVHDto } from './../../shared/dto/arinvh-dto';
import { Component, OnInit, Injector, ViewChild, EventEmitter, Output } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';
import { RouteInvoiceService } from '../../shared/services/route-invoice.service';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { SalesLookupTableModalComponent } from '@app/finders/supplyChain/sales/sales-lookup-table-modal.component';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';

@Component({
  selector: 'app-create-or-edit-route-invoices',
  templateUrl: './create-or-edit-route-invoices.component.html',
  styles: []
})
export class CreateOrEditRouteInvoicesComponent extends AppComponentBase implements OnInit {

  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild("FinanceLookupTableModal", { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
  @ViewChild("InventoryLookupTableModal", { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;
  @ViewChild('SalesLookupTableModal', { static: true }) SalesLookupTableModal: SalesLookupTableModalComponent;
  @ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active = false;
  saving = false;
  arinvoiceH: ARINVHDto;
  arinvoiceD: ARINVDDto;
  arinvoiceD2: ARINVDDto;
  arinvoiceData: ARINVDDto[] = new Array<ARINVDDto>();
  //Picker Descriptions Start
  locDesc: string = "";
  routDesc: string = "";
  refName: string = "";
  saleTypeDesc: string = "";
  //Picker Descriptions End
  isButtonVisible = false;
  checkedval: boolean;
  editMode: boolean = false;
  formValid: boolean = false;
  target: string;
  type: string;

  ///GRID START///
  agGridExtend: AgGridExtend = new AgGridExtend();
  tabMode: any;
  gridApi;
  private setParms;
  gridColumnApi;
  rowData;
  rowSelection;
  paramsData;
  ///GRID END///
  constructor(injector: Injector,
    private _lightbox: Lightbox,
    private _routeInvoiceService: RouteInvoiceService) {
    super(injector);
  }

  ngOnInit() {
  }
  show(id?: number): void {
    debugger
    this.active = true;
    this.arinvoiceH = new ARINVHDto();
    this.arinvoiceData = new Array<ARINVDDto>();
    this.arinvoiceD = new ARINVDDto();
    this.arinvoiceData = new Array<ARINVDDto>();
    this.editMode = false;
    this.formValid = false;
    this.arinvoiceH.paymentOption = "Bank"
    this.locDesc = undefined;
    this.routDesc = undefined;
    this.saleTypeDesc = undefined;
    this.refName = undefined;
    if (!id) {
      this._routeInvoiceService.GetDocId().subscribe(res => {
        debugger
        this.arinvoiceH.docNo = res["result"];
        this.arinvoiceH.docDate = new Date();
        this.arinvoiceH.postedDate = new Date();
        this.isButtonVisible = false;
        //this.invoiceKnockOffH.mDocDate = new Date();
      });
    } else {
      this.editMode = true;
      this._routeInvoiceService.getDataForEdit(id).subscribe((data: any) => {
        debugger
        this.arinvoiceH.id = data["result"]["id"];

        this.arinvoiceH = data["result"];
        this.locDesc = data["result"]["locDesc"];
        this.routDesc = data["result"]["routDesc"];
        this.saleTypeDesc = data["result"]["saleTypeDesc"];
        this.refName = data["result"]["refDesc"];
        this.arinvoiceH.docDate = new Date(data["result"]["docDate"]);
        this.arinvoiceH.invDate = new Date(data["result"]["invDate"]);
        this.arinvoiceH.postedDate = new Date(data["result"]["postDate"]);
        debugger
        this.fnShowChequeNo(this.arinvoiceH.paymentOption);
        this.addRecordToGrid(data["result"]["arinvDetailDto"]);
        this.isButtonVisible = this.arinvoiceH.posted;
      });
    }
    this.modal.show();
  }
  save() {
    this.message.confirm("Save", isConfirmed => {
      if (isConfirmed) {
        this.saving = true;
        debugger
        var rowData = [];
        this.gridApi.forEachNode(node => {
          rowData.push(node.data);
        });
        this.arinvoiceH.arinvDetailDto = rowData;
        debugger
        this.arinvoiceH.docDate = new Date(this.arinvoiceH.docDate.toLocaleString());
        this.arinvoiceH.invDate = new Date(this.arinvoiceH.invDate.toLocaleString());
        this._routeInvoiceService.create(this.arinvoiceH).subscribe(() => {
          this.saving = false;
          this.notify.info(this.l("SavedSuccessfully"));
          this.close();
          this.modalSave.emit(null);
        });
        this.close();
        this.tabMode = 7;
      }
    });
  }

  close(): void {
    this.active = false;
    this.modal.hide();
    this._lightbox.close();
  }

  GetPostedInvoices() {
    debugger
    if (
      //this.arinvoiceH.saleTypeID == "" || this.arinvoiceH.saleTypeID == null || 
      this.arinvoiceH.routID == 0 || this.arinvoiceH.routID == undefined
      || this.arinvoiceH.invDate == null || this.arinvoiceH.invDate == undefined) {
      this.message.warn(this.l('Please select Route ID & Invoice Date First'), 'Route & Invoice Date Required');
      return;
    }
    this._routeInvoiceService.GetPostedInvoices(this.arinvoiceH.routID, this.arinvoiceH.invDate).subscribe(res => {
      var result = res["result"];
      debugger
      if (result.length > 0) {
        this.addProcessRecordToGrid(res["result"]);
      }
      else {
        this.message.warn('No Record Found.');
      }
    });
  }

  changeOption(value) {
    this.arinvoiceH.bankID = "";
    this.arinvoiceH.accountID = "";
    this.arinvoiceH.chequeNo = "";
  }

  fnShowChequeNo(val) {
    debugger
    if(this.arinvoiceH.paymentOption == "Cash")
    {
      const chequeNo = this.gridColumnApi.getAllColumns().find((x)=>x.colDef.field == "chequeNo");
      this.gridColumnApi.setColumnVisible(chequeNo,false); 
    }
    if(this.arinvoiceH.paymentOption == "Bank")
    {
      const chequeNo = this.gridColumnApi.getAllColumns().find((x)=>x.colDef.field == "chequeNo");
      this.gridColumnApi.setColumnVisible(chequeNo,true); 
    }
  }

  //----------------------FinanceLookupTableModal---------------------------//
  getLookUpData() {
    debugger
    if (this.type == "GLLocation") {
      this.getNewLocation();
    }
    else if (this.type == "Routes") {
      this.getNewRoutes();
    }
    else if (this.type == "ChequeBookDetail") {
      this.getChequeBookDetail();
    }
  }

  //---------------------Location-------------------------//
  openLocationModal() {
    this.target = "GLLocation";
    this.type = "GLLocation";
    this.FinanceLookupTableModal.id = String(this.arinvoiceH.locID);
    this.FinanceLookupTableModal.displayName = this.locDesc;
    this.FinanceLookupTableModal.show(this.target);
  }
  setLocationNull() {
    this.arinvoiceH.locID = null;
    this.locDesc = "";
  }
  getNewLocation() {
    debugger;
    this.arinvoiceH.locID = Number(this.FinanceLookupTableModal.id);
    this.locDesc = this.FinanceLookupTableModal.displayName;
  }

  //=====================Route Model================
  openSelectRouteModal() {
    debugger;
    this.target = "Routes";
    this.type = "Routes";
    this.FinanceLookupTableModal.id = String(this.arinvoiceH.routID);
    this.FinanceLookupTableModal.displayName = this.routDesc;
    this.FinanceLookupTableModal.show(this.target, "");
  }
  getNewRoutes() {
    debugger;
    this.arinvoiceH.routID = Number(this.FinanceLookupTableModal.id);
    this.routDesc = this.FinanceLookupTableModal.displayName;
  }
  setRouteIDNull() {
    debugger;
    this.arinvoiceH.routID = null;
    this.routDesc = "";

  }
  //====================cheque no ===================///
  openInstrumentNo() {
    if (this.arinvoiceH.accountID != "" && this.arinvoiceH.accountID != null) {
      this.target = "ChequeBookDetail";
      this.type = "ChequeBookDetail";
      this.FinanceLookupTableModal.id = "";
      this.FinanceLookupTableModal.displayName = "";
      this.FinanceLookupTableModal.show("ChequeBookDetail", this.arinvoiceH.accountID);
    }
    else {
      this.message.warn("Please select account first");
    }
  }
  getChequeBookDetail() {
    debugger
    this.arinvoiceH.chequeNo = this.FinanceLookupTableModal.id;
  }


  //----------------------SalesLookupTableModal---------------------------//
  getNewSalesModal() {
    debugger;
    this.getNewReference();
  }
  //=====================Sale Refrence Model================
  openSelectReferenceModal() {
    this.target = "Reference";
    this.SalesLookupTableModal.id = String(this.arinvoiceH.refNo);
    this.SalesLookupTableModal.displayName = this.refName;
    this.SalesLookupTableModal.show(this.target, "OE");
  }
  getNewReference() {
    debugger;
    this.arinvoiceH.refNo = Number(this.SalesLookupTableModal.id);
    this.refName = this.SalesLookupTableModal.displayName;
  }
  setReferenceIDNull() {
    this.arinvoiceH.refNo = null;
    this.refName = "";
  }




  //----------------------InventoryLookupTableModal---------------------------//

  getNewInventoryModal() {
    switch (this.target) {
      case "TransactionType":
        this.getNewTransaction();
        break;
      default:
        break;
    }
  }
  //=====================Transaction Type Model================
  openSelectTransactionModal() {
    this.target = "TransactionType";
    this.InventoryLookupTableModal.id = this.arinvoiceH.saleTypeID;
    this.InventoryLookupTableModal.displayName = this.saleTypeDesc;
    this.InventoryLookupTableModal.show(this.target);
  }
  getNewTransaction() {
    debugger;
    this.arinvoiceH.saleTypeID = this.InventoryLookupTableModal.id;
    this.saleTypeDesc = this.InventoryLookupTableModal.displayName;

  }
  setTransactionIDNull() {
    this.arinvoiceH.saleTypeID = "";
    this.saleTypeDesc = "";

  }


  ////===================bank model================
  getNewCommonServiceModal() {
    switch (this.target) {
      case "Bank":
      case "Cash":
        this.getNewBankId();
        break;
      default:
        break;
    }
  }
  //=====================Bank Model==================
  openSelectBankIdModal() {
    debugger;
    this.target = this.arinvoiceH.paymentOption;
    this.commonServiceLookupTableModal.id = this.arinvoiceH.bankID;
    this.commonServiceLookupTableModal.accountId = this.arinvoiceH.accountID;
    this.commonServiceLookupTableModal.show("Bank", this.arinvoiceH.paymentOption == "Bank" ? "1" : "2");
  }

  getNewBankId() {
    debugger;
    this.arinvoiceH.bankID = this.commonServiceLookupTableModal.id;
    this.arinvoiceH.accountID = this.commonServiceLookupTableModal.accountId;
  }

  setBankIdNull() {
    this.arinvoiceH.bankID = '';
    this.arinvoiceH.accountID = '';
  }

  //////////////////////GRID/////////////////////////
  addIconCellRendererFunc(params) {
    debugger;
    return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
  }
  columnDefs = [
    {
      headerName: this.l("Account ID"),
      editable: false,
      field: "accountID",
      sortable: true,
      // width: 100,
      resizable: true,
    },
    {
      headerName: this.l("Account Name"),
      editable: false,
      field: "accountName",
      sortable: true,
      // width: 100,
      resizable: true,
    },
    {
      headerName: this.l("Sub Acc ID"),
      editable: false,
      field: "subAccID",
      sortable: true,
      // width: 100,
      resizable: true,
    },
    {
      headerName: this.l("Sub Acc Name"),
      editable: false,
      field: "subAccName",
      sortable: true,
      // width: 100,
      resizable: true,
    },
    {
      headerName: this.l("Doc No"),
      editable: false,
      field: "docNo",
      sortable: true,
      // width: 100,
      resizable: true,
      valueFormatter: this.agGridExtend.formatNumber,
    },
    {
      headerName: this.l("Invoice Number"),
      editable: false,
      field: "invNumber",
      // width: 100,
      resizable: true,
    },
    {
      headerName: this.l("Invoice Amount"),
      editable: false,
      field: "invAmount",
      sortable: true,
      // width: 100,
      resizable: true,
      valueFormatter: this.agGridExtend.formatNumber,
    },
    {
      headerName: this.l('TaxAmount'),
      field: 'taxAmount',
      sortable: true,
      // width: 100,
      editable: true,
      resizable: true,
      valueFormatter: this.agGridExtend.formatNumber
    },
    {
      headerName: this.l("Receipt Amount"),
      editable: true,
      field: "recpAmount",
      sortable: true,
      // width: 100,
      resizable: true,
      valueFormatter: this.agGridExtend.formatNumber,
    },
    {
      headerName: this.l("Cheque No"),
      editable: (params) => { return (this.arinvoiceH.paymentOption == "Bank") ? true : false },
      field: "chequeNo",
      sortable: true,
      // width: 100,
      resizable: true
      //hide: (params) => { return (this.arinvoiceH.paymentOption != "Bank") ? true : false }
    },
    {
      headerName: this.l('Narration'),
      field: 'narration',
      editable: true,
      resizable: true
    },
    {
      headerName: 'Check',
      field: 'adjust',
      editable: false,
      cellRenderer: params => {
        return `<input type='checkbox' ${params.value ? 'checked' : ''} />`;
      }
    }
  ];

  addProcessRecordToGrid(record: any) {
    //this.editState = true;
    if (record != undefined) {
      var rData = [];
      record.forEach((val, index) => {
        debugger
        var newData;
        newData = {
          accountID: val.accountID,
          accountName: val.accountName,
          subAccID: val.subAccID,
          subAccName: val.subAccName,
          docNo: val.docNo,
          invNumber: val.invNumber,
          invAmount: val.invAmount,
          taxAmount: val.taxAmount,
          recpAmount: val.recpAmount,
          chequeNo: val.chequeNo,
          //adjust: false
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
    if (record != undefined && (this.arinvoiceH.id != undefined || this.arinvoiceH.id > 0)) {
      var rData = [];
      var amount = 0;
      record.forEach((val, index) => {
        debugger
        var newData;
        newData = {
          accountID: val.accountID,
          accountName: val.accountName,
          subAccID: val.subAccID,
          subAccName: val.subAccName,
          docNo: val.docNo,
          invNumber: val.invNumber,
          invAmount: val.invAmount,
          taxAmount: val.taxAmount,
          recpAmount: val.recpAmount,
          chequeNo: val.chequeNo,
          narration: val.narration,
          adjust: val.adjust
        };
        rData.push(newData)
        //this.gridApi.updateRowData({ add: [newData] });
      });
      //this.invoiceKnockOffH.totalAdjust = amount;
      this.rowData = [];
      this.rowData = rData;
    }
  }

  removeRecordFromGrid() {
    var selectedData = this.gridApi.getSelectedRows();
  }

  onGridReady(params) {
    this.rowData = [];
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
    params.api.sizeColumnsToFit();
    this.rowSelection = "multiple";
  }

  onCellClicked(params) {

    if (params.column["colId"] == "adjust") {
      var adjust = params.data.adjust;
      if (adjust == false) {
        params.data.adjust = true;
      } else if (adjust == true) {
        params.data.adjust = false;
      }
    }
    this.gridApi.refreshCells();
  }
  onCellValueChanged(params) {
    debugger;
    this.calculations();
    this.gridApi.refreshCells();
  }

  cellEditingStarted(params) {

  }

  calculations() {
    debugger
    var amount = 0;
    this.gridApi.forEachNode(node => {
      debugger
    });
  }


}
