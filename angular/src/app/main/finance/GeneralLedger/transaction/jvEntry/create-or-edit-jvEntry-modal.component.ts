import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { GLTRHeadersServiceProxy, GLTRDetailsServiceProxy, AccountsPostingsServiceProxy, VoucherEntryServiceProxy, CreateOrEditGLTRHeaderDto, CreateOrEditGLTRDetailDto, VoucherEntryDto, FiscalCalendarsServiceProxy, FiscalCalendersServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { AgGridAngular } from 'ag-grid-angular';
import { AgGridExtend } from '@app/shared/common/ag-grid-extend/ag-grid-extend';
import { AppConsts } from '@shared/AppConsts';
import { Lightbox } from 'ngx-lightbox';
import { LogComponent } from '@app/finders/log/log.component';

@Component({
	selector: 'createOrEditJVEntryModal',
	templateUrl: './create-or-edit-jvEntry-modal.component.html'
})
export class CreateOrEditJVEntryModalComponent extends AppComponentBase {

	@ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
	@ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
	@ViewChild('CommonServiceLookupTableModal', { static: true }) CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
	@Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
	@ViewChild('LogTableModal', { static: true }) LogTableModal: LogComponent;

	active = false;
	saving = false;
	DocYear = 0;
	accountID = '';
	accountDesc = '';
	subAccID = '';
	subAccDesc = '';
	totalCredit = 0;
	totalDebit = 0;
	totalBalance = 0;
	BookNameShow = '';
	private setParms;
	NormalEntry = 0;
	private glLocationList;
	LockDocDate: Date;

	url: string;
	uploadUrl: string;
	uploadedFiles: any[] = [];
	checkImage: boolean = false;
	image: any[] = [];
	dateValid: boolean = false;
	readonly jvEntryAppId = 4;
	readonly appName = "JVEntry";

	gltrHeader: CreateOrEditGLTRHeaderDto = new CreateOrEditGLTRHeaderDto();
	gltrDetail: CreateOrEditGLTRDetailDto = new CreateOrEditGLTRDetailDto();
	voucherEntry: VoucherEntryDto = new VoucherEntryDto();
	agGridExtend: AgGridExtend = new AgGridExtend();

	auditTime: Date;
	accountIdC = '';
	target: any;
	errorFlag: boolean = false;
    editMode: boolean = false;

	constructor(
		injector: Injector,
		private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
		private _fiscalCalendarsServiceProxy: FiscalCalendersServiceProxy,
		private _gltrDetailsServiceProxy: GLTRDetailsServiceProxy,
		private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
		private _lightbox: Lightbox,
		private _accountsPostingsServiceProxy: AccountsPostingsServiceProxy
	) {
		super(injector);
	}
	OpenLog(){
        debugger
       this.LogTableModal.show(this.gltrHeader.fmtDocNo,this.gltrHeader.bookID);
    }
	show(gltrHeaderId?: number, maxId?: number, fmtDocNo?: number, cbookId?: string, bookName?: string): void {

		this.url = null;
		this.auditTime = null;
		this.image = [];
		this.uploadedFiles = [];
		this.uploadUrl = null;
		this.checkImage = true;
		debugger;

		this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
			debugger;
			this.glLocationList = resultL;
		});

		if (!gltrHeaderId) {
			this.gltrHeader = new CreateOrEditGLTRHeaderDto();
			this.gltrHeader.chType = 1;
			this.gltrHeader.id = gltrHeaderId;
			this.gltrHeader.docDate = moment();
			this.gltrHeader.docMonth = moment().month() + 1;
			this.DocYear = moment().year();
			this.gltrHeader.docNo = maxId;
			this.gltrHeader.fmtDocNo = fmtDocNo;
			this.gltrHeader.bookID = cbookId;
			this.gltrHeader.locId = 1;
			this.BookNameShow = bookName;
			this.accountIdC = '';
			this.totalCredit = 0;
			this.totalDebit = 0;
			this.totalBalance = 0;
			this.editMode = false;
			//this.shByNormalEntry('JV');
			this._voucherEntryServiceProxy.getBaseCurrency().subscribe(result => {
				debugger;
				if (result) {
					this.gltrHeader.curid = result.id;
					this.gltrHeader.currate = result.currRate;
				}
			});

			this.active = true;
			this.modal.show();
			this.getDateParams(null, true);
		} else {
			this._gltrHeadersServiceProxy.getGLTRHeaderForEdit(gltrHeaderId).subscribe(result => {
				debugger;
				this.editMode = true;
				this.gltrHeader = result.gltrHeader;
                
				this._gltrHeadersServiceProxy.getImage(this.jvEntryAppId, result.gltrHeader.docNo).subscribe(fileResult => {
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

				if (this.gltrHeader.auditTime) {
					this.auditTime = this.gltrHeader.auditTime.toDate();
				}

				this.LockDocDate = this.gltrHeader.docDate.toDate();

				this.accountIdC = result.glconfigConfigID;

				this.DocYear = moment(this.gltrHeader.docDate).year();

				// this.shByNormalEntry(result.gltrHeader.bookID);

				this._gltrDetailsServiceProxy.filterGLTRDData(gltrHeaderId).subscribe(resultD => {
					debugger;
					var rData = [];
					var totalSDebit = 0;
					var totalSCredit = 0;
					resultD["items"].forEach(element => {
						// if(element.gltrDetail.accountID){
						//     this._voucherEntryServiceProxy.getAccountDesc(element.gltrDetail.accountID).subscribe(resultAC => {
						//         debugger;
						//         if(resultAC){
						//             element.gltrDetail['accountDesc']=resultAC;
						//         }
						//     });
						// }
						// if(element.gltrDetail.subAccID){
						//     this._voucherEntryServiceProxy.getSubledgerDesc(element.gltrDetail.accountID,element.gltrDetail.subAccID).subscribe(resultSA => {
						//         debugger;
						//         if(resultSA){
						//             element.gltrDetail['subAccDesc']=resultSA;
						//         }
						//     });
						// }
						if (element.gltrDetail.isAuto == false) {
							debugger;
							rData.push(element.gltrDetail);
							// switch (this.NormalEntry) {
							//     case 1:
							//         element.gltrDetail['credit']=element.gltrDetail.amount;
							//         element.gltrDetail['debit']=0;
							//         break;
							//     case 2:
							//         element.gltrDetail['debit']=element.gltrDetail.amount;
							//         element.gltrDetail['credit']=0;
							//         break;
							//     default:
							//         break;
							// }

							if (element.gltrDetail.amount < 0) {
								element.gltrDetail['credit'] = -(Math.round(element.gltrDetail.amount / this.gltrHeader.currate));
								element.gltrDetail['debit'] = 0;
							} else {
								element.gltrDetail['debit'] = Math.round(element.gltrDetail.amount / this.gltrHeader.currate);
								element.gltrDetail['credit'] = 0;
							}

							totalSDebit += parseFloat(element.gltrDetail['debit']);
							totalSCredit += parseFloat(element.gltrDetail['credit']);
						}
						else {
							debugger;
							this.accountIdC = element.gltrDetail.accountID;
						}
					});

					this.rowData = [];
					this.rowData = rData;

					this.totalDebit = totalSDebit;
					this.totalCredit = totalSCredit;
					this.totalBalance = Math.abs(parseFloat((totalSDebit - totalSCredit).toFixed(2)));
				});



				this.active = true;
				this.modal.show();
				this.getDateParams(null, false);
			});
		}
	}

	JvForsalarySheetshow(Year?: number, month?: number, fmtDocNo?: number, cbookId?: string, bookName?: string): void {
		debugger;

		this.url = null;
		this.auditTime = null;
		this.image = [];
		this.uploadedFiles = [];
		this.uploadUrl = null;
		this.checkImage = true;
	
		this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
			
			this.glLocationList = resultL;
		});
           

		if(Year!=undefined && month!=undefined){
		this._gltrDetailsServiceProxy.filterGLTRDDataForSalarySheet(Year,month).subscribe(resultD => {
					debugger;
					if(resultD=="Exist"){
						this.notify.success(this.l('JV is Unlocked !!!'));
					}else if(resultD=="Done"){
						this.notify.success(this.l('JV Has Created Successfully'));
					}else if(resultD=="Locked"){
						this.notify.success(this.l('Salary is Unlocked!!'));
					}else if(resultD=="Gen"){
						this.notify.success(this.l('plz First Salary Generate!!'));
					}
				
				});	
			}		
	}

	postVoucher(vocherId: number, posted) {
		if (this.dateValid == true) {
			this.message.confirm(
				'',
				(isConfirmed) => {
					if (isConfirmed) {
						this._accountsPostingsServiceProxy.postingData([vocherId], 'AccountsPosting', posted)
							.subscribe(() => {
								this.notify.success(this.l('SuccessfullyPosted'));
								this.close();
								this.modalSave.emit(null);
							});
					}
				}
			);
		}
		else {
			this.notify.info("This Date Is Locked");
		}
	}

	approveVoucher(vocherId: number, approve) {
		debugger;
		if (this.dateValid == true) {
			this.message.confirm(
				'',
				(isConfirmed) => {
					if (isConfirmed) {
						this._accountsPostingsServiceProxy.postingData([vocherId], 'AccountsApproval', approve)
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
		else {
			this.notify.info("This Date Is Locked");
		}

	}

	shByNormalEntry(bookId: string) {
		this._voucherEntryServiceProxy.getBookNormalEntry(bookId).subscribe(result => {
			debugger;
			this.NormalEntry = result.normalEntry;
			if (result.normalEntry == 0) {
				this.message.warn(bookId + " book not found", "Book Required");
				return;
			}
			if (result.normalEntry == 1) {
				this.gridColumnApi.setColumnVisible("debit", false);
				this.gridApi.sizeColumnsToFit();
			}
			if (result.normalEntry == 2) {
				this.gridColumnApi.setColumnVisible("credit", false);
				this.gridApi.sizeColumnsToFit();
			}
		});
	}


	//==================================Grid=================================
	private gridApi;
	private gridColumnApi;
	private rowData;
	private rowSelection;
	columnDefs = [
		{ headerName: this.l('SrNo'), field: 'srNo', sortable: true, width: 50, valueGetter: 'node.rowIndex+1' },
		{ headerName: this.l('AccountID'), field: 'accountID', sortable: true, filter: true, width: 120, editable: false, resizable: true },
		{ headerName: this.l(''), field: 'addAccId', width: 15, editable: false, cellRenderer: this.accIdCellRendererFunc, resizable: false },
		{ headerName: this.l('AccountDesc'), field: 'accountDesc', sortable: true, filter: true, width: 200, resizable: true },
		{ headerName: this.l('Subledger'), field: 'subAccID', sortable: true, filter: true, width: 120, editable: false, resizable: true },
		{ headerName: this.l(''), field: 'addsubAccId', width: 15, editable: false, cellRenderer: this.subAccIDCellRendererFunc, resizable: false },
		{ headerName: this.l('IsSL'), field: 'isSL', width: 20, resizable: true, hide: true },
		{ headerName: this.l('SubledgerDesc'), field: 'subAccDesc', sortable: true, filter: true, width: 150, resizable: true },
		{ headerName: this.l('Narration'), field: 'narration', editable: true, resizable: true },
		// { headerName: this.l('ChequeNo'), field: 'chequeNo', sortable: true, width: 150, editable: true, resizable: true },
		{ headerName: this.l('Debit'), field: 'debit', sortable: true, width: 100, editable: true, type: "numericColumn", resizable: true, valueFormatter: this.agGridExtend.formatNumber },
		{ headerName: this.l('Credit'), field: 'credit', sortable: true, width: 100, editable: true, type: "numericColumn", resizable: true, valueFormatter: this.agGridExtend.formatNumber },
		// {headerName: this.l('Refrence'), field: 'refrence',width:150}
	];

	onGridReady(params) {
		debugger;
		this.rowData = [];
		this.gridApi = params.api;
		this.gridColumnApi = params.columnApi;
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

	subAccIDCellRendererFunc(params) {
		debugger;
		return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" (click)="openSelectGLSubledgerModal(params)"></i>';
	}

	accIdCellRendererFunc(params) {
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
			narration: this.gltrHeader.narration,
			chequeNo: this.gltrHeader.chNumber,
			debit: '0',
			credit: '0',
			isSL: false
			//refrence:""
		};
		return newData;
	}

	calculations() {
		debugger;
		var totalSDebit = 0;
		var totalSCredit = 0;
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

	cellClicked(params) {
		debugger;
		if (params.column["colId"] == "addAccId") {
			this.setParms = params;
			this.openSelectChartofControlModal();
		}
		if (params.column["colId"] == "addsubAccId") {
			this.setParms = params;
			this.openSelectGLSubledgerModal();
		}
	}

	//==================================Grid=================================


	save(): void {
		debugger;
		this.errorFlag = false;
		this.message.confirm(
			'Save JV',
			(isConfirmed) => {
				if (isConfirmed) {

					if (moment(this.gltrHeader.docDate) > moment().endOf('day')) {
						this.message.warn("Document date greater than current date", "Document Date Greater");
						return;
					}

					if ((moment(this.LockDocDate).month() + 1) != this.gltrHeader.docMonth && this.gltrHeader.id != null) {
						this.message.warn('Document month not changeable', "Document Month Error");
						return;
					}

					if ((moment(this.gltrHeader.docDate).month() + 1) != this.gltrHeader.docMonth) {
						this.message.warn(this.l('Document month not according to documrnt date'), 'Document Month Error');
						return;
					}

					if (this.gltrHeader.locId == null || this.gltrHeader.locId.toString() == "0") {
						this.message.warn(this.l('Please select location'), 'Location required');
						return;
					}

					if (this.gridApi.getDisplayedRowCount() <= 0) {
						this.message.warn("No details found", "Details Required");
						return;
					}

					this.gridApi.forEachNode(node => {
						debugger
						if (node.data.isSL && (node.data.subAccID == "" || node.data.subAccID == 0)) {
							this.message.warn("Subledger Account not found at row " + Number(node.rowIndex + 1))
							//, "Subledger Required");
							this.errorFlag = true;
							//return;
						}
						// else {
						//     this.errorFlag = false;
						// }
					});

					if (this.errorFlag) {
						debugger
						return;
					}

					// if (this.gltrHeader.bookID == "JV" && this.totalDebit == 0 && this.totalCredit == 0) {
					// 	this.message.warn(this.l('Debit or credit amount not equal to zero'), 'Debit/Credit Zero');
					// 	return;
					// }
					// if (this.gltrHeader.bookID == "JV" && this.totalBalance != 0) {
					// 	this.message.warn(this.l('OutOfBalanceAlert'), 'Out of Balance');
					// 	return;
					// }

					if (this.totalDebit == 0 && this.totalCredit == 0) {
						this.message.warn(this.l('Debit or credit amount not equal to zero'), 'Debit/Credit Zero');
						return;
					}


					
					if (this.totalBalance != 0) {
						this.message.warn(this.l('OutOfBalanceAlert'), 'Out of Balance');
						return;
					}



					this.saving = true;

					var rowData = [];
					this.gridApi.forEachNode(node => {
						if (node.data.credit == 0 && node.data.debit != 0) {
							rowData.push(node.data);
							if (this.gltrHeader.currate != null) {
								node.data['amount'] = (node.data.debit * this.gltrHeader.currate);
							}
						}
						if (node.data.debit == 0 && node.data.credit != 0) {
							rowData.push(node.data);
							if (this.gltrHeader.currate != null) {
								node.data['amount'] = -node.data.credit * this.gltrHeader.currate;
							}
						}
					});

					//check direct posted from GLSETUP
					// this._voucherEntryServiceProxy.getDirectPostedStatus(this.accountIdC).subscribe(result => {
					//     debugger;
					//     if(result==true){
					//         this.gltrHeader.posted=result;
					//     }
					// });

					this.voucherEntry.gltrDetail = rowData;
					this.voucherEntry.gltrHeader = this.gltrHeader;

					this.gltrHeader.auditTime = moment();
					this.gltrHeader.auditUser = this.appSession.user.userName;

					if (!this.gltrHeader.id) {
						this.gltrHeader.createdOn = moment();
						this.gltrHeader.createdBy = this.appSession.user.userName;
					}

					if (moment(new Date()).format("A") === "AM" && !this.gltrHeader.id && (moment(new Date()).month() + 1) == this.gltrHeader.docMonth) {
						this.gltrHeader.docDate = moment(this.gltrHeader.docDate);
					} else {
						this.gltrHeader.docDate = moment(this.gltrHeader.docDate).endOf('day');
					}

					this.gltrHeader.amount = this.totalDebit;

					debugger
					this._voucherEntryServiceProxy.createOrEditVoucherEntry(this.voucherEntry)
						.pipe(finalize(() => { this.saving = false; }))
						.subscribe(() => {
							//this.notify.info(this.l('SavedSuccessfully'));
							if(this.editMode == false)
							{
								this.message.confirm("Press 'Yes' for create new JV", this.l('SavedSuccessfully'), (isConfirmed) => {
									if (isConfirmed) {
										//this.gltrHeader.docNo = Number(this.gltrHeader.docNo) + 1;
										this._voucherEntryServiceProxy
											.getMaxDocId(
												this.gltrHeader.bookID,
												true,
												moment(this.gltrHeader.docDate).format("LLLL")
											)
											.subscribe(result => {
												this.gltrHeader.fmtDocNo = result;
	
											});
										this.rowData = [];
										this.modalSave.emit(null);
									} else {
										this.close();
										this.modalSave.emit(null);
									}
								});
							}
							else
							{
								this.close();
							    this.modalSave.emit(null);
							}
						});

				}
			}
		);

	}

	getNewFinanceModal() {
		debugger;
		switch (this.target) {
			case "ChartOfAccount":
				this.getNewChartofControlId();
				break;
			case "SubLedger":
				this.getNewGLSubledgerId();
				break;

			default:
				break;
		}
	}
	getNewCommonServiceModal() {
		debugger;
		switch (this.target) {
			case "Currency":
				this.getNewCurrencyRateId();
				break;


			default:
				break;
		}
	}

	//=====================Currency Rate Model================
	openSelectCurrencyRateModal() {
		debugger;
		this.target = "Currency";
		this.CommonServiceLookupTableModal.id = this.gltrHeader.curid;
		this.CommonServiceLookupTableModal.currRate = this.gltrHeader.currate;
		this.CommonServiceLookupTableModal.show(this.target);
	}


	setCurrencyRateIdNull() {
		this.gltrHeader.curid = '';
		this.gltrHeader.currate = null;
	}


	getNewCurrencyRateId() {
		debugger;
		this.gltrHeader.curid = this.CommonServiceLookupTableModal.id;
		this.gltrHeader.currate = this.CommonServiceLookupTableModal.currRate;
	}
	//=====================Currency Rate Model=================

	//=====================Account Code Model==============
	openSelectChartofControlModal() {
		debugger;
		this.target = "ChartOfAccount";
		this.FinanceLookupTableModal.id = this.accountID;
		this.FinanceLookupTableModal.displayName = this.accountDesc;
		this.FinanceLookupTableModal.show(this.target);
	}


	setChartofControlIdNull() {
		debugger;
		this.setParms.data.accountID = '';
		this.setParms.data.accountDesc = '';
	}

	getNewChartofControlId() {
		debugger;
		if (this.setParms.data.accountID != "" && this.FinanceLookupTableModal.id == "") {
			this.onBtStartEditing(this.setParms.rowIndex, "narration");
			return;
		}
		this.setParms.data.accountID = this.FinanceLookupTableModal.id;
		this.setParms.data.accountDesc = this.FinanceLookupTableModal.displayName;
		this.setGLSubledgerIdNull();
		this.gridApi.refreshCells();
		if (this.FinanceLookupTableModal.subledger == true) {
			this.setParms.data.isSL = true;
			this.onBtStartEditing(this.setParms.rowIndex, "addsubAccId");
		} else {
			this.setParms.data.isSL = false;
			this.onBtStartEditing(this.setParms.rowIndex, "narration");
		}
	}

	// getNewChartofControlId() {
	//     debugger;
	//     if(this.setParms.data.accountID!="" && this.FinanceLookupTableModal.id==""){
	//         this.onBtStartEditing(this.setParms.rowIndex,"narration");
	//         return;
	//     }
	//     this.setParms.data.accountID = this.FinanceLookupTableModal.id;
	//     this.setParms.data.accountDesc = this.FinanceLookupTableModal.displayName;
	//     this.setGLSubledgerIdNull();
	//     this.gridApi.refreshCells();
	//     if(this.FinanceLookupTableModal.subledger==true){
	//         this.onBtStartEditing(this.setParms.rowIndex,"addsubAccId");
	//     }else{
	//         this.onBtStartEditing(this.setParms.rowIndex,"narration");
	//     }
	// }

	//=====================Account Code Model==============

	//=====================GLSubledger Model==============
	openSelectGLSubledgerModal() {
		debugger;
		this.target = "SubLedger";
		this.FinanceLookupTableModal.id = this.subAccID;
		this.FinanceLookupTableModal.displayName = this.subAccDesc;
		this.FinanceLookupTableModal.show(this.target, this.setParms.data.accountID);
	}


	setGLSubledgerIdNull() {
		debugger;
		this.setParms.data.subAccID = 0;
		this.setParms.data.subAccDesc = '';
	}


	getNewGLSubledgerId() {
		debugger;
		this.setParms.data.subAccID = this.FinanceLookupTableModal.id;
		this.setParms.data.subAccDesc = this.FinanceLookupTableModal.displayName;
		this.gridApi.refreshCells();
		this.onBtStartEditing(this.setParms.rowIndex, "narration");
	}

	//=====================GLSubledger Model==============

	getDateParams(val?, checkForDocId?) {
		if(!this.gltrHeader.id){
		this._fiscalCalendarsServiceProxy.getFiscalYearStatus(this.gltrHeader.docDate, 'GL').subscribe(
			result => {
				debugger
				if (result == true) {
					if (checkForDocId == null || checkForDocId == true) {
						this.gltrHeader.docMonth = moment(this.gltrHeader.docDate).month() + 1;
						this.DocYear = moment(this.gltrHeader.docDate).year();
						this._voucherEntryServiceProxy
							.getMaxDocId("JV", true, moment(this.gltrHeader.docDate).format("LLLL"))
							.subscribe(result => {
								this.gltrHeader.fmtDocNo = undefined;
								this.gltrHeader.fmtDocNo = result;
							});
					}
					this.dateValid = true;
				}
				else {
					this.notify.info("This Date Is Locked");
					this.dateValid = false;
				}
			}
		)
		}else
		{
		this.dateValid = true;
	    }
	}
	//===========================File Attachment=============================
	onBeforeUpload(event): void {
		debugger;
		this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles?';
		if (this.jvEntryAppId !== undefined)
			this.uploadUrl += "APPID=" + encodeURIComponent("" + this.jvEntryAppId) + "&";
		if (this.appName !== undefined)
			this.uploadUrl += "AppName=" + encodeURIComponent("" + this.appName) + "&";
		if (this.gltrHeader.docNo !== undefined)
			this.uploadUrl += "DocID=" + encodeURIComponent("" + this.gltrHeader.docNo) + "&";
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

	close(): void {

		this.active = false;
		this.modal.hide();
		this._lightbox.close();
	}
}
