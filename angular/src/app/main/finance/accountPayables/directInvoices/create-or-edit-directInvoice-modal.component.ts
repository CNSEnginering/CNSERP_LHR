import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditGLINVHeaderDto } from '../../shared/dto/glinvHeader-dto';
import { CreateOrEditGLINVDetailDto } from '../../shared/dto/glinvDetails-dto';
import { DirectInvoiceDto } from '../../shared/dto/directinvoice-dto';
import { DirectInvoiceServiceProxy } from '../../shared/services/directinvoice.service';
import { GLINVHeadersService } from '../../shared/services/glinvHeader.service';
import { GLINVDetailsService } from '../../shared/services/glinvDetail.service';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';
import { GLTRHeadersServiceProxy, FiscalCalendarsServiceProxy, FiscalCalendersServiceProxy } from '@shared/service-proxies/service-proxies';
import { Observable, of } from 'rxjs';

@Component({
    selector: 'createOrEditDirectInvoiceModal',
    templateUrl: './create-or-edit-directInvoice-modal.component.html'
})
export class CreateOrEditDirectInvoiceModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    subLedgerAccList = [];
    accountID = '';
    accountDesc = '';
    subAccID = '';
    subAccDesc = '';
    totalAmount = 0;
    InvAmount = 0;
    private setParms;
    TypeIdShow = '';
    LockDocDate: Date;
    depositDate: Date;
    creditLimit = true;
    tab: string;
    glinvHeader: CreateOrEditGLINVHeaderDto = new CreateOrEditGLINVHeaderDto();
    glinvDetails: CreateOrEditGLINVDetailDto = new CreateOrEditGLINVDetailDto();
    directInvoice: DirectInvoiceDto = new DirectInvoiceDto();
    agGridExtend: AgGridExtend = new AgGridExtend();

    auditTime: Date;
    docDate: Date;
    accountIdC = '';
    processing = false;
    processing1 = false;
    target: string;
    totalDebit: number;
    totalCredit: number;
    totalBalance: number;
    taxAccDesc: string;
    taxClassDesc: string;
    arTaxClassDesc: string;
    ictaxClassDesc: string;
    taxAuthDesc: string;
    ictaxAuthDesc: string;
    locDesc: string;
    instrumentNoChk: boolean = false;
    typeTaxClass: string;
    validDate: boolean = false;
    constructor(
        injector: Injector,
        private _glinvHeadersServiceProxy: GLINVHeadersService,
        private _glinvDetailsServiceProxy: GLINVDetailsService,
        private _directInvoiceServiceProxy: DirectInvoiceServiceProxy,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
        private _fiscalCalendarsServiceProxy: FiscalCalendersServiceProxy,
    ) {
        super(injector);
    }

    show(glinvHeaderId?: number, maxId?: number, typeID?: string): void {
        this.auditTime = null;
        debugger;
        this._gltrHeadersServiceProxy.getInstrumentNoChk().subscribe(resultL => {
            this.instrumentNoChk = resultL["result"];
        });
        if (!glinvHeaderId) {
            this.glinvHeader = new CreateOrEditGLINVHeaderDto();
            this.glinvHeader.id = glinvHeaderId;
            this.glinvHeader.docDate = moment().endOf('day');
            this.glinvHeader.postDate = moment().endOf('day');
            this.glinvHeader.partyInvDate = moment().endOf('day');
            this.glinvHeader.docNo = maxId;
            this.glinvHeader.partyInvNo = "Invoice-" + maxId;
            this.glinvHeader.payReason = "";
            this.glinvHeader.docStatus = "Open";
            this.accountIdC = "";
            this.taxAuthDesc = "";
            this.ictaxClassDesc = "";
            this.ictaxAuthDesc = "";
            this.totalAmount = 0;
            this.totalCredit = 0;
            this.totalDebit = 0;
            this.totalBalance = 0;
            this.glinvHeader.typeID = typeID;
            this.TypeIdShow = this.l('CreateNewDirectInvoice') + " (" + typeID + ")";

            this.active = true;
            this.getDateParams();
            this.modal.show();
        } else {
            this._glinvHeadersServiceProxy.getGLINVHeaderForEdit(glinvHeaderId).subscribe(result => {
                this.glinvHeader = result;
                debugger;
                if (this.glinvHeader.audtDate) {
                    this.auditTime = this.glinvHeader.audtDate.toDate();
                }

                this.locDesc = result.locDesc;
                this.taxClassDesc = result.taxClassDesc;
                this.taxAuthDesc = result.taxAuthDesc;

                if (this.glinvHeader.postedStock) {
                    this.glinvHeader.paymentOption = "Bank";
                }

                this.depositDate = moment(this.glinvHeader.cprDate).toDate();

                this.LockDocDate = this.glinvHeader.docDate.toDate();

                this.TypeIdShow = "";
                this.TypeIdShow = this.l('EditDirectInvoice') + " (" + this.glinvHeader.typeID + ")";

                this._glinvDetailsServiceProxy.getGLINVDData(glinvHeaderId).subscribe(resultD => {
                    debugger;
                    var rData = [];
                    var totalSDebit = 0;
                    var totalSCredit = 0;
                    resultD["items"].forEach(element => {
                        rData.push(element);

                        if (element.amount < 0) {
                            element.credit = -(Math.round(element.amount));
                            element.debit = 0;
                        } else {
                            element.debit = Math.round(element.amount);
                            element.credit = 0;
                        }

                        totalSDebit += element.debit;
                        totalSCredit += element.credit;
                    });

                    this.rowData = [];
                    this.rowData = rData;

                    this.totalDebit = totalSDebit;
                    this.totalCredit = totalSCredit;
                    this.totalBalance = Math.abs(parseFloat((totalSDebit - totalSCredit).toFixed(2)));
                    this.totalAmount = totalSDebit;
                });



                this.active = true;
                this.getDateParams();
                this.modal.show();
            });
        }
    }

    getDateParams(val?) {
        this._fiscalCalendarsServiceProxy.getFiscalYearStatus(moment(this.glinvHeader.docDate), this.glinvHeader.typeID == "AR" ? 'AR' : 'AP').subscribe(
            result => {
                debugger
                if (result == true) {
                    this.validDate = true;
                }
                else {
                    this.notify.info("This Date Is Locked");
                    this.validDate = false;
                }
            }
        )
    }

    processDirectInv(target: string): void {
        debugger;
        if(target=='Payment'){
            if(this.glinvHeader.accountID==null){
                this.message.info("Plz Enter Bank Or Cash Account From Tax Tab","Account ID");
                return;
              }
        }
      

        this.message.confirm(
            'Process ' + target,
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing = true;
                    this.directInvoice.glinvHeader = this.glinvHeader;
                    this.directInvoice.target = target;

                    if (moment(new Date()).format("A") === "AM" && !this.glinvHeader.id && (moment(new Date()).month() + 1) == (moment(this.glinvHeader.docDate).month() + 1)) {
                        this.glinvHeader.docDate = moment(this.glinvHeader.docDate);
                        this.glinvHeader.postDate = moment(this.glinvHeader.postDate);
                        this.glinvHeader.partyInvDate = moment(this.glinvHeader.partyInvDate);
                    } else {
                        this.glinvHeader.docDate = moment(this.glinvHeader.docDate).endOf('day');
                        this.glinvHeader.postDate = moment(this.glinvHeader.postDate).endOf('day');
                        this.glinvHeader.partyInvDate = moment(this.glinvHeader.partyInvDate).endOf('day');
                    }

                    this._directInvoiceServiceProxy.processDirectInvoice(this.directInvoice).subscribe(result => {
                        if (result == "Save") {
                            this.processing = false;
                            this.notify.info(this.l('ProcessSuccessfully'));
                            this.close();
                            this.modalSave.emit(null);
                        } else {
                            this.processing = false;
                            this.notify.error(this.l('ProcessFailed'));
                        }
                    });
                }
            }
        );
    }

    updateCPR() {
        debugger;
        this.message.confirm(
            'Update CPR',
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing1 = true;
                    this.glinvHeader.cprDate = moment(this.depositDate);
                    this.directInvoice.glinvHeader = this.glinvHeader;
                    debugger;
                    this._directInvoiceServiceProxy.updateCPR(this.directInvoice).subscribe(() => {
                        debugger
                        this.processing1 = false;
                        this.notify.info(this.l('UpdatedSuccessfully'));
                    });
                }
            }
        );
    }
    //In order to retrieve default tax class and authority based on Account and Sub Account Id//
    getUpdate(tab: string) {
        let accountId = ""
        let subAccId = ""

        debugger;
        if (this.glinvHeader.typeID == 'ST') {
            this.gridApi.forEachNode(node => {
                debugger;
                if (node.data.subAccDesc != "") {
                    accountId = node.data.accountID
                    subAccId = node.data.subAccID
                }
            })


            // this.gridApi.forEach(element => {
            //     if (element.subAccDesc.length != 0) {
            //         accountId = element.accountID
            //         subAccId = element.subAccID
            //     }
            // });
        }
        else {
            this.rowData.forEach(element => {
                if (element.subAccDesc.length != 0) {
                    accountId = element.accountID
                    subAccId = element.subAccID
                }
            });
        }
        if (tab == "Tab2" && this.glinvHeader.typeID == 'ST') {
            this._directInvoiceServiceProxy.getUpdate(accountId, Number(subAccId)).subscribe(result => {
                debugger;

                this.glinvHeader.icTaxAuth = result[0];
                this.ictaxAuthDesc = result[1];
                this.glinvHeader.icTaxClass = Number(result[2]);
                this.ictaxClassDesc = result[3];
                this.glinvHeader.icTaxRate = Number(result[4]);
                this.glinvHeader.icTaxAccID = result[5];
                this.creditLimit = result[6] == "T" ? true : false;
                this.glinvHeader.icTaxAmount = Math.round((this.glinvHeader.taxRate * this.totalAmount) / 100);


            });
        }
        else if (this.glinvHeader.typeID == 'ST') {
            this._directInvoiceServiceProxy.getSalesUpdate(accountId, Number(subAccId)).subscribe(result => {
                debugger;

                this.glinvHeader.taxAuth = result[0];
                this.taxAuthDesc = result[1];
                this.glinvHeader.taxClass = Number(result[2]);
                this.taxClassDesc = result[3];
                this.glinvHeader.taxRate = Number(result[4]);
                this.glinvHeader.taxAccID = result[5];
                this.creditLimit = result[6] == "T" ? true : false;
                this.glinvHeader.taxAmount = Math.round((this.glinvHeader.taxRate * this.totalAmount) / 100);


            });
        }
        else {

            this._directInvoiceServiceProxy.getUpdate(accountId, Number(subAccId)).subscribe(result => {
                debugger;

                this.glinvHeader.taxAuth = result[0];
                this.taxAuthDesc = result[1];
                this.glinvHeader.taxClass = Number(result[2]);
                this.taxClassDesc = result[3];
                this.glinvHeader.taxRate = Number(result[4]);
                this.glinvHeader.taxAccID = result[5];
                this.creditLimit = result[6] == "T" ? true : false;
                this.glinvHeader.taxAmount = Math.round((this.glinvHeader.taxRate * this.totalAmount) / 100);


            });

        }

    }



    //==================================Grid=================================
    private gridApi;
    private gridColumnApi;
    private rowData;
    private rowSelection;
    columnDefs = [
        { headerName: this.l('SrNo'), field: 'srNo', sortable: true, width: 50, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('AccountID'), field: 'accountID', sortable: true, filter: true, width: 100, editable: false, resizable: true },
        { headerName: this.l(''), field: 'addAccId', width: 15, editable: false, cellRenderer: this.accIdCellRendererFunc, resizable: false },
        { headerName: this.l('AccountDesc'), field: 'accountDesc', sortable: true, filter: true, width: 200, resizable: true },
        { headerName: this.l('Subledger'), field: 'subAccID', sortable: true, filter: true, width: 80, editable: false, resizable: true },
        { headerName: this.l(''), field: 'addsubAccId', width: 15, editable: false, cellRenderer: this.subAccIDCellRendererFunc, resizable: false },
        { headerName: this.l('SubledgerDesc'), field: 'subAccDesc', sortable: true, filter: true, width: 150, resizable: true },
        //{headerName: this.l('Amount'), field: 'amount',sortable: true,width:100,editable: true,type: "numericColumn",resizable: true},
        { headerName: this.l('Debit'), field: 'debit', sortable: true, width: 100, editable: true, type: "numericColumn", resizable: true, valueFormatter: this.agGridExtend.formatNumber },
        { headerName: this.l('Credit'), field: 'credit', sortable: true, width: 100, editable: true, type: "numericColumn", resizable: true, valueFormatter: this.agGridExtend.formatNumber },
        { headerName: this.l('Comments'), field: 'narration', editable: true, resizable: true }
    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];
        this.gridApi = params.api;
        this.gridApi = params.api;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
    }

    onAddRow(): void {
        debugger;
        var index = this.gridApi.getDisplayedRowCount();
        var newItem = this.createNewRowData();
        this.gridApi.updateRowData({ add: [newItem] });
        this.gridApi.refreshCells();
        this.onBtStartEditing(index, "addAccId");
    }

    subAccIDCellRendererFunc() {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" (click)="openSelectGLSubledgerModal(params)"></i>';
    }

    accIdCellRendererFunc() {
        debugger;
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" (click)="openSelectChartofControlModal(params)"></i>';
    }

    onBtStartEditing(index, col) {
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

    createNewRowData() {
        debugger;
        var newData = {
            accountID: "",
            subAccID: '0',
            narration: this.glinvHeader.narration,
            chequeNo: "",
            //amount:'0',
            debit: '0',
            credit: '0'
        };
        return newData;
    }

    cellClicked(params) {
        debugger;
        if (params.column["colId"] == "addAccId") {
            this.setParms = params;
            this.openSelectChartofACModal();
        }
        if (params.column["colId"] == "addsubAccId") {
            this.setParms = params;
            this.openSelectSubAccModal();
        }
    }

    calculations() {
        debugger;
        var totalSDebit = 0;
        var totalSCredit = 0;
        let accountId = ""
        let subAccId = ""
        this.gridApi.forEachNode(node => {
            debugger;
            if (node.data.debit != "" || node.data.credit != "") {
                totalSDebit += parseFloat(node.data.debit);
                totalSCredit += parseFloat(node.data.credit);
            }
        })
        this.totalDebit = totalSDebit;
        this.totalCredit = totalSCredit;
        this.totalBalance = Math.abs(parseFloat((totalSDebit - totalSCredit).toFixed(2)));
        this.gridApi.forEachNode(node => {
            if (node.data.subAccDesc.length != 0) {
                accountId = node.data.accountID
                subAccId = node.data.subAccID
            }
        });
        this._directInvoiceServiceProxy.getCreditLimitCheck(accountId, Number(subAccId)).subscribe(result => {
            debugger;
            this.creditLimit = result;
            if (this.creditLimit == true) {
                if (this.totalAmount <= this.glinvHeader.creditLimit - this.glinvHeader.closingBalance) {
                    // debugger;
                    // this.totalAmount = Math.abs(parseFloat((totalSDebit).toFixed(2)));
                }
                // else if(this.glinvHeader.closingBalance!=null && this.creditLimit==true){
                //     //this.totalAmount=null
                //     this.notify.error(this.l('Limit Not Available'));
                // }
                else {
                    this.notify.error(this.l('Limit Not Available'));
                    // this.totalAmount = Math.abs(parseFloat((totalSDebit).toFixed(2)));
                }
            }
        });

        this.glinvHeader.icTaxAmount = Math.round((this.glinvHeader.icTaxRate * this.totalAmount) / 100);
        this.glinvHeader.taxAmount = Math.round((this.glinvHeader.taxRate * this.totalAmount) / 100);
        if (this.glinvHeader.typeID == 'AP') {
            this.totalAmount = this.totalCredit;
        }
        else if (this.glinvHeader.typeID == 'AR' || this.glinvHeader.typeID == 'ST') {
            this.totalAmount = this.totalDebit;
        }
    }

    onCellValueChanged(params) {
        debugger;
        if (params.column["colId"] === "debit") {
            if ((parseFloat(params.data.credit) > 0 || parseFloat(params.data.credit) !== NaN) && parseFloat(params.data.debit) > 0) {
                params.data.credit = 0;
            }
        } else if (params.column["colId"] === "credit") {
            if ((parseFloat(params.data.debit) > 0 || parseFloat(params.data.debit) !== NaN) && parseFloat(params.data.credit) > 0) {
                params.data.debit = 0;
            }
        }
        this.calculations();
        this.gridApi.refreshCells();
    }

    //==================================Grid=================================


    save(): void {
        // this.checkSubLedgerAcc().subscribe(
        //     () => {
        //         if (this.subLedgerAccList[0] == false
        //             && this.subLedgerAccList[1] == false) {
        //             this.message.warn("There should be one Subledger Account");
        //         }
        //         else if (this.subLedgerAccList[0] == true
        //             && this.subLedgerAccList[1] == true
        //         ) {
        //             this.message.warn("There should be one Subledger Account");
        //         }
        //         else if (
        //             (this.subLedgerAccList[0] == false
        //             && this.subLedgerAccList[1] == true) || 
        //             (this.subLedgerAccList[0] == true
        //                 && this.subLedgerAccList[1] == false)
        //         ) {
        this.saveAfter();
        // }
        // }
        //);
    }


    saveAfter() {
        this.message.confirm(
            'Save Direct Invoice',
            (isConfirmed) => {
                if (isConfirmed) {

                    if (moment(this.glinvHeader.docDate) > moment().endOf('day')) {
                        this.message.warn("Document date greater than current date", "Document Date Greater");
                        return;
                    }

                    if ((moment(this.LockDocDate).month() + 1) != (moment(this.glinvHeader.docDate).month() + 1) && this.glinvHeader.id != null) {
                        this.message.warn('Document month not changeable', "Document Month Error");
                        return;
                    }

                    if (this.gridApi.getDisplayedRowCount() <= 0) {
                        this.message.warn("No details found", "Details Required");
                        return;
                    }

                    if (this.totalDebit == 0 && this.totalCredit == 0) {
                        this.message.warn(this.l('Debit or credit amount not equal to zero'), 'Debit/Credit Zero');
                        return;
                    }
                    if (this.totalBalance != 0) {
                        this.message.warn(this.l('OutOfBalanceAlert'), 'Out of Balance');
                        return;
                    }

                    if (this.totalAmount == 0) {
                        this.message.warn(this.l('Amount not greater than zero'), 'Amount Zero');
                        return;
                    }

                    this.saving = true;

                    var rowData = [];
                    this.gridApi.forEachNode(node => {
                        rowData.push(node.data);
                    });





                    this.gridApi.forEachNode(node => {
                        if (node.data.credit == 0 && node.data.debit != 0) {
                            //rowData.push(node.data);
                            node.data['amount'] = node.data.debit;
                        }
                        if (node.data.debit == 0 && node.data.credit != 0) {
                            // rowData.push(node.data);
                            node.data['amount'] = -node.data.credit;
                        }
                    });


                    this.glinvHeader.cprDate = moment(this.depositDate);


                    if (moment(new Date()).format("A") === "AM" && !this.glinvHeader.id && (moment(new Date()).month() + 1) == (moment(this.glinvHeader.docDate).month() + 1)) {
                        this.glinvHeader.docDate = moment(this.glinvHeader.docDate);
                        this.glinvHeader.postDate = moment(this.glinvHeader.postDate);
                        this.glinvHeader.partyInvDate = moment(this.glinvHeader.partyInvDate);
                    } else {
                        this.glinvHeader.docDate = moment(this.glinvHeader.docDate).endOf('day');
                        this.glinvHeader.postDate = moment(this.glinvHeader.postDate).endOf('day');
                        this.glinvHeader.partyInvDate = moment(this.glinvHeader.partyInvDate).endOf('day');
                    }

                    this.glinvHeader.audtDate = moment();
                    this.glinvHeader.audtUser = this.appSession.user.userName;

                    if (!this.glinvHeader.id) {
                        this.glinvHeader.createDate = moment();
                        this.glinvHeader.createdBy = this.appSession.user.userName;
                    }

                    this.directInvoice.glinvDetail = rowData;
                    this.directInvoice.glinvHeader = this.glinvHeader;

                    this._directInvoiceServiceProxy.createOrEditDirectInvoice(this.directInvoice)
                        .pipe(finalize(() => { this.saving = false; }))
                        .subscribe(() => {
                            // this.notify.info(this.l('SavedSuccessfully'));
                            this.message.confirm("Press 'Yes' for create new Invoice", this.l('SavedSuccessfully'), (isConfirmed) => {
                                if (isConfirmed) {
                                    this.glinvHeader.docNo = this.glinvHeader.docNo + 1;
                                    this.rowData = [];
                                    this.modalSave.emit(null);
                                } else {
                                    this.close();
                                    this.modalSave.emit(null);
                                }
                            });
                        });

                }
            }
        );
    }

    // checkSubLedgerAcc() {
    //     this.subLedgerAccList.length = 0;
    //     return of(this.gridApi.forEachNode(node => {
    //         if (node.data.subAccID == 0) {
    //             this.subLedgerAccList.push(false);

    //         }
    //         else {
    //             this.subLedgerAccList.push(true);
    //         }
    //     }));
    // }


    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "ChartOfAccount":
                this.getNewChartOfAC();
                break;
            case "SubLedger":
                this.getNewSubAcc();
                break;
            case "GLLocation":
                this.getNewLocation();
                break;
            case "ChequeBookDetail":
                this.getChequeBookDetail();
                break;
            case "ArTerm":
                this.getArTerm();
                break;
            case "IncomeTax":
                this.getNewTaxClass();
                break;
            default:
                break;
        }
    }
    getArTerm() {
        debugger
        this.glinvHeader.arClass = Number(this.FinanceLookupTableModal.id);
        this.arTaxClassDesc = this.FinanceLookupTableModal.displayName;
        this.glinvHeader.arRate = this.FinanceLookupTableModal.taxRate;
        this.glinvHeader.arAccID = this.FinanceLookupTableModal.accountID;
        this.glinvHeader.arAmount = Math.round((this.glinvHeader.arRate * this.glinvHeader.taxAmount) / 100);
    }
    getChequeBookDetail() {
        debugger
        this.glinvHeader.chNumber = this.FinanceLookupTableModal.id;
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
            case "TaxAuthority":
                this.getNewTaxAuthority();
                break;
            case "TaxClass":
                this.getNewTaxClass();
                break;
            case "IncomeTax":
                this.getNewTaxClass();
                break;
            case "CPR":
                this.getNewCPR()
            default:
                break;
        }
    }
    //=====================Currency Rate Model================
    openSelectCurrencyRateModal() {
        debugger;
        this.target = "Currency";
        this.commonServiceLookupTableModal.id = this.glinvHeader.curID;
        this.commonServiceLookupTableModal.currRate = this.glinvHeader.curRate;
        this.commonServiceLookupTableModal.show(this.target);
    }


    setCurrencyRateIdNull() {
        this.glinvHeader.curID = '';
        this.glinvHeader.curRate = null;
    }


    getNewCurrencyRateId() {
        debugger;
        this.glinvHeader.curID = this.commonServiceLookupTableModal.id;
        this.glinvHeader.curRate = this.commonServiceLookupTableModal.currRate;
    }
    //================Currency Rate Model===============

    //================CPR Modal=========================
    openSelectCPRModal() {
        this.target = "CPR";
        this.commonServiceLookupTableModal.id = "";
        this.commonServiceLookupTableModal.displayName = "";
        this.commonServiceLookupTableModal.show(this.target);

    }
    getNewCPR() {

        this.glinvHeader.cprID = Number(this.commonServiceLookupTableModal.id);
        this.glinvHeader.cprNo = this.commonServiceLookupTableModal.displayName;
    }

    //=====================Bank Model==================
    openSelectBankIdModal() {
        debugger;
        this.target = this.glinvHeader.paymentOption;
        this.commonServiceLookupTableModal.id = this.glinvHeader.bankID;
        this.commonServiceLookupTableModal.accountId = this.glinvHeader.accountID;
        this.commonServiceLookupTableModal.show("Bank", this.glinvHeader.paymentOption == "Bank" ? "1" : "2");
    }


    setBankIdNull() {
        this.glinvHeader.bankID = '';
        this.glinvHeader.accountID = '';
    }


    getNewBankId() {
        debugger;
        this.glinvHeader.bankID = this.commonServiceLookupTableModal.id;
        this.glinvHeader.accountID = this.commonServiceLookupTableModal.accountId;
    }
    //=====================Bank Model======================


    //=====================Chart of Ac Model================
    openSelectChartofACModal() {
        debugger;
        this.target = "ChartOfAccount";
        this.FinanceLookupTableModal.id = this.setParms.data.accountID;
        this.FinanceLookupTableModal.displayName = this.setParms.data.accountDesc;
        this.FinanceLookupTableModal.show(this.target);
    }

    setAccountIDNull() {
        this.setParms.data.accountID = '';
        this.setParms.data.accountDesc = '';
        this.setSubAccIDNull();
    }

    getNewChartOfAC() {
        debugger;
        if (this.FinanceLookupTableModal.id != this.setParms.data.accountID)
            this.setSubAccIDNull();
        this.setParms.data.accountID = this.FinanceLookupTableModal.id;
        this.setParms.data.accountDesc = this.FinanceLookupTableModal.displayName;
        this.setSubAccIDNull();
        this.gridApi.refreshCells();
        if (this.FinanceLookupTableModal.subledger == true) {
            this.onBtStartEditing(this.setParms.rowIndex, "addsubAccId");
        } else {
            this.onBtStartEditing(this.setParms.rowIndex, "debit");
        }
    }
    //=====================Chart of Ac Model================

    //=====================Sub Account Model================
    openSelectSubAccModal() {
        debugger;
        if (this.setParms.data.accountID == "" || this.setParms.data.accountID == null) {
            this.message.warn(this.l('Please select account first'), 'Account Required');
            return;
        }
        this.target = "SubLedger";
        this.FinanceLookupTableModal.id = this.setParms.data.subAccID;
        this.FinanceLookupTableModal.displayName = this.setParms.data.subAccDesc;
        this.FinanceLookupTableModal.show(this.target, this.setParms.data.accountID);
    }

    setSubAccIDNull() {
        this.setParms.data.subAccID = 0;
        this.setParms.data.subAccDesc = '';
    }

    getNewSubAcc() {
        this.setParms.data.subAccID = Number(this.FinanceLookupTableModal.id);
        this.setParms.data.subAccDesc = this.FinanceLookupTableModal.displayName;
        if (this.setParms.data.subAccDesc.length != 0) {
            this._directInvoiceServiceProxy.getCreditLimit(this.setParms.data.accountID, this.setParms.data.subAccID).subscribe(result => {
                debugger;

                this.glinvHeader.creditLimit = result;
            });
            this._directInvoiceServiceProxy.getClosingBalance(this.setParms.data.accountID, this.setParms.data.subAccID, moment(new Date())).subscribe(result => {
                debugger;

                this.glinvHeader.closingBalance = result;
            });

        }
        this.gridApi.refreshCells();
        this.onBtStartEditing(this.setParms.rowIndex, "debit");
    }
    //=====================Sub Account Model================

    //=====================Tax Authority Model================
    openSelectTaxAuthorityModal(tab: string) {
        this.target = "TaxAuthority";
        this.tab = tab;
        this.commonServiceLookupTableModal.id = this.glinvHeader.taxAuth;
        this.commonServiceLookupTableModal.displayName = this.taxAuthDesc;
        this.commonServiceLookupTableModal.show(this.target);
    }

    setTaxAuthorityIdNull() {
        this.glinvHeader.icTaxAuth = '';
        this.ictaxAuthDesc = '';
        this.glinvHeader.icTaxAccID = '';
        this.glinvHeader.icTaxRate = 0;
        this.glinvHeader.icTaxClass =null;
    }

    getNewTaxAuthority() {
        debugger
        if (this.tab == "Tab2") {
            if (this.commonServiceLookupTableModal.id != this.glinvHeader.taxAuth)
                this.setTaxAuthorityIdNull();
            this.glinvHeader.icTaxAuth = this.commonServiceLookupTableModal.id;
            this.ictaxAuthDesc = this.commonServiceLookupTableModal.displayName;
        }
        else {
            if (this.commonServiceLookupTableModal.id != this.glinvHeader.taxAuth)
                this.setTaxClassIdNull();
            this.glinvHeader.taxAuth = this.commonServiceLookupTableModal.id;
            this.taxAuthDesc = this.commonServiceLookupTableModal.displayName;
        }
    }
    //=====================Tax Authority Model================

    //=====================Tax Class================
    openSelectTaxClassModal(tab: string, typeTaxClass: string) {
        debugger
        this.tab = tab;
        if (this.glinvHeader.typeID == 'ST' && tab == 'Tab2' && typeTaxClass == "IncomeTax") {
            if (this.glinvHeader.icTaxAuth == "" || this.glinvHeader.icTaxAuth == null) {
                this.message.warn(this.l('Please select Tax authority'), 'Tax Authority Required');
                return;
            }
            this.target = "IncomeTax";
            this.commonServiceLookupTableModal.id = String(this.glinvHeader.taxClass);
            this.commonServiceLookupTableModal.displayName = this.taxClassDesc;
            this.commonServiceLookupTableModal.accountId = this.glinvHeader.taxAccID;
            this.commonServiceLookupTableModal.taxRate = this.glinvHeader.taxRate;
            this.commonServiceLookupTableModal.show("TaxClass", this.glinvHeader.icTaxAuth);
        }
        else if (this.glinvHeader.typeID == 'ST' && tab == 'Tab2' && typeTaxClass != "TaxClass") {
            // if (this.glinvHeader.taxAuth == "" || this.glinvHeader.taxAuth == null) {
            //     this.message.warn(this.l('Please select Tax authority'), 'Tax Authority Required');
            //     return;
            // }
            this.target = "ArTerm";
            this.FinanceLookupTableModal.id = String(this.glinvHeader.taxClass);
            this.FinanceLookupTableModal.displayName = this.taxClassDesc;
            this.FinanceLookupTableModal.accountID = this.glinvHeader.taxAccID;
            this.FinanceLookupTableModal.taxRate = this.glinvHeader.taxRate;
            this.FinanceLookupTableModal.show(this.target, "", "", " Ar Term");
        }
        else {
            if (this.glinvHeader.taxAuth == "" || this.glinvHeader.taxAuth == null) {
                this.message.warn(this.l('Please select Tax authority'), 'Tax Authority Required');
                return;
            }
            this.target = "TaxClass";
            this.commonServiceLookupTableModal.id = String(this.glinvHeader.taxClass);
            this.commonServiceLookupTableModal.displayName = this.taxClassDesc;
            this.commonServiceLookupTableModal.accountId = this.glinvHeader.taxAccID;
            this.commonServiceLookupTableModal.taxRate = this.glinvHeader.taxRate;
            this.commonServiceLookupTableModal.show(this.target, this.glinvHeader.taxAuth);
        }
    }
    getNewTaxClass() {
        debugger
        if (this.target == 'IncomeTax') {
            this.glinvHeader.icTaxClass = Number(this.commonServiceLookupTableModal.id);
            this.ictaxClassDesc = this.commonServiceLookupTableModal.displayName;
            this.glinvHeader.icTaxRate = this.commonServiceLookupTableModal.taxRate;
            this.glinvHeader.icTaxAccID = this.commonServiceLookupTableModal.accountId;
            this.glinvHeader.icTaxAmount = Math.round((this.glinvHeader.icTaxRate * this.glinvHeader.invAmount) / 100);
        }
        else {
            this.glinvHeader.taxClass = Number(this.commonServiceLookupTableModal.id);
            this.taxClassDesc = this.commonServiceLookupTableModal.displayName;
            this.glinvHeader.taxRate = this.commonServiceLookupTableModal.taxRate;
            this.glinvHeader.taxAccID = this.commonServiceLookupTableModal.accountId;
            this.glinvHeader.taxAmount = Math.round((this.glinvHeader.taxRate * this.totalAmount) / 100);
        }
    }
    setTaxClassIdNull() {
        debugger
        this.glinvHeader.taxClass = null;
        this.taxClassDesc = '';
        this.glinvHeader.taxAccID = '';
        this.glinvHeader.taxRate = 0;
    }

    openInstrumentNo() {
        if (this.glinvHeader.accountID != "" && this.glinvHeader.accountID != null) {
            this.target = "ChequeBookDetail";
            this.FinanceLookupTableModal.id = "";
            this.FinanceLookupTableModal.displayName = "";
            this.FinanceLookupTableModal.show("ChequeBookDetail", this.glinvHeader.accountID, "", " Instrument No");
        }
        else {
            this.message.confirm("Please select account first");
        }
    }
    //=====================Tax Class================

    //=====================Location Model================
    openSelectLocationModal() {
        this.target = "GLLocation";
        this.FinanceLookupTableModal.id = String(this.glinvHeader.locID);
        this.FinanceLookupTableModal.displayName = this.locDesc;
        this.FinanceLookupTableModal.show(this.target);
    }
    getNewLocation() {
        debugger;
        this.glinvHeader.locID = Number(this.FinanceLookupTableModal.id);
        this.locDesc = this.FinanceLookupTableModal.displayName;
    }
    setLocationIDNull() {
        this.glinvHeader.locID = null;
        this.locDesc = "";

    }
    //=====================Location Model================

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
