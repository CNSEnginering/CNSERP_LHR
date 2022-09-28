import {
	Component,
	Injector,
	ViewEncapsulation,
	ViewChild,
	OnInit
} from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import {
	GLTRHeadersServiceProxy,
	GLTRHeaderDto,
	GLBOOKSServiceProxy,
	VoucherEntryServiceProxy
} from "@shared/service-proxies/service-proxies";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { CreateOrEditVoucherEntryModalComponent } from "./create-or-edit-voucherEntry-modal.component";
import { ViewVoucherEntryModalComponent } from "./view-voucherEntry-modal.component";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { Table } from "primeng/components/table/table";
import { Paginator } from "primeng/components/paginator/paginator";
import { LazyLoadEvent } from "primeng/components/common/lazyloadevent";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { HttpClient } from "@angular/common/http";
import { AppConsts } from "@shared/AppConsts";
import { finalize } from "rxjs/operators";
import { FileUpload } from "primeng/primeng";

@Component({
	templateUrl: "./voucherEntry.component.html",
	encapsulation: ViewEncapsulation.None,
	styleUrls: ["./voucherEntry.component.less"],
	animations: [appModuleAnimation()]
})
export class VoucherEntryComponent extends AppComponentBase implements OnInit {
	@ViewChild("createOrEditVoucherEntryModal", { static: true })
	createOrEditVoucherEntryModal: CreateOrEditVoucherEntryModalComponent;

	@ViewChild("viewVoucherEntryModalComponent", { static: true })
	viewVoucherEntryModal: ViewVoucherEntryModalComponent;

	@ViewChild("reportviewrModalComponent", { static: false })
	reportviewrModalComponent: ReportviewrModalComponent;

	@ViewChild("dataTable", { static: true }) dataTable: Table;
	@ViewChild("paginator", { static: true }) paginator: Paginator;
	@ViewChild("ExcelFileUpload", { static: true }) excelFileUpload: FileUpload;

	advancedFiltersAreShown = false;
	filterText = "";
	bookIDFilter = "";
	maxConfigIDFilter: number;
	maxConfigIDFilterEmpty: number;
	minConfigIDFilter: number;
	minConfigIDFilterEmpty: number;
	maxDocNoFilter: number;
	maxDocNoFilterEmpty: number;
	minDocNoFilter: number;
	minDocNoFilterEmpty: number;
	maxDocMonthFilter = 0;
	maxDocMonthFilterEmpty: number;
	minDocMonthFilter = 0;
	minDocMonthFilterEmpty: number;
	maxDocDateFilter: moment.Moment;
	minDocDateFilter: moment.Moment;
	narrationFilter = "";
	accountIDFilter = "";
	accountDescFilter = "";
	postedFilter = -1;
	approvedFilter = -1;
	auditUserFilter = "";
	maxAuditTimeFilter: moment.Moment;
	minAuditTimeFilter: moment.Moment;
	oldCodeFilter = "";
	glconfigConfigIDFilter = "";
	locationFilter = 0;
	chNumberFilter=""
	maxID: any;
	btnBooksDiv = "";
	private glLocationList;
	viewOption: any;
	rptObj: string;
	fromDate: Date = moment()
		.startOf("day")
		.toDate();
	toDate: Date = moment()
		.endOf("day")
		.toDate();
	fromAccount: any;
	toAccount: any;
	reportUrl: string;
	rptName: string;

	uploadUrl: string;
	curid: string;
	curRate: number;
	allowedBooks: any[] = []
	data;
	constructor(
		injector: Injector,
		private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
		private _glbooksServiceProxy: GLBOOKSServiceProxy,
		private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
		private _notifyService: NotifyService,
		private _tokenAuth: TokenAuthServiceProxy,
		private _activatedRoute: ActivatedRoute,
		private _fileDownloadService: FileDownloadService,
		private _httpClient: HttpClient
	) {
		super(injector);
		this.uploadUrl =
			AppConsts.remoteServiceBaseUrl + "/GLTransaction/ImportFromExcel";

		this._voucherEntryServiceProxy.getBaseCurrency().subscribe(result => {
			if (result) {
				this.curid = result.id;
				this.curRate = result.currRate;
			}
		});
	}

	private btnBooks;

	transactionVoucher = false;

	ngOnInit() {
		debugger;
		this.viewOption = this._activatedRoute.snapshot.data.viewOption;
		if (this.viewOption === "integratedVoucher") {
			this.transactionVoucher = true;
		}

		this._voucherEntryServiceProxy
			.getBooksDetails(this.transactionVoucher, "")
			.subscribe(resultB => {
				debugger;
				this.btnBooks = resultB;

			});

		this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
			debugger;
			this.glLocationList = resultL;
		});
	}

	getGLTRHeaders(event?: LazyLoadEvent) {
		if (this.primengTableHelper.shouldResetPaging(event)) {
			this.paginator.changePage(0);
			return;
		}

		this.primengTableHelper.showLoadingIndicator();

		this._gltrHeadersServiceProxy
			.getAll(
				this.filterText,
				this.bookIDFilter,
				this.locationFilter,
				this.chNumberFilter,
				this.maxConfigIDFilter == null
					? this.maxConfigIDFilterEmpty
					: this.maxConfigIDFilter,
				this.minConfigIDFilter == null
					? this.minConfigIDFilterEmpty
					: this.minConfigIDFilter,
				this.maxDocNoFilter == null
					? this.maxDocNoFilterEmpty
					: this.maxDocNoFilter,
				this.minDocNoFilter == null
					? this.minDocNoFilterEmpty
					: this.minDocNoFilter,
				this.maxDocMonthFilter == 0
					? this.maxDocMonthFilterEmpty
					: this.maxDocMonthFilter,
				this.minDocMonthFilter == 0
					? this.minDocMonthFilterEmpty
					: this.minDocMonthFilter,
				this.maxDocDateFilter,
				this.minDocDateFilter,
				this.narrationFilter,
				this.accountIDFilter,
				this.accountDescFilter,
				this.postedFilter,
				this.approvedFilter,
				this.auditUserFilter,
				this.maxAuditTimeFilter,
				this.minAuditTimeFilter,
				this.oldCodeFilter,
				this.glconfigConfigIDFilter,
				this.transactionVoucher,
				this.primengTableHelper.getSorting(this.dataTable),
				this.primengTableHelper.getSkipCount(this.paginator, event),
				this.primengTableHelper.getMaxResultCount(this.paginator, event)
			)
			.subscribe(result => {
				debugger;
				this.data = result;
				this.primengTableHelper.totalRecordsCount =  result.totalCount;
				this.primengTableHelper.records = result.items;
				this.primengTableHelper.hideLoadingIndicator();
			});
	}

	reloadPage(): void {
		this.paginator.changePage(this.paginator.getPage());
	}

	createGLTRHeader(BookID:string,BookName:string) {
		debugger;
		var bookId =BookID;// val["toElement"]["value"];
		var bookName =BookName;// val["toElement"]["innerText"];

		//get maxid of document on base of book
		// this._voucherEntryServiceProxy
		// 	.getMaxDocId(bookId, false, undefined)
		// 	.subscribe(result => {
		// 		//debugger;
		// 		if (result != undefined) {
		// 			this.maxID = result;
		// 		}
		this._voucherEntryServiceProxy
			.GetMaxSrNo()
			.subscribe(result => {
				debugger
				this.maxID = result["result"];;
			});
		this._voucherEntryServiceProxy
			.getMaxDocId(
				bookId,
				true,
				moment(new Date()).format("LLLL")
			)
			.subscribe(result => {
				//debugger;
				// if (result != 0) {
				//     this.maxID = result;
				// }
				this.createOrEditVoucherEntryModal.show(
					null,
					this.maxID,
					result,
					bookId,
					bookName
				);
			});
		// this.createOrEditJVEntryModal.show(
		//     null,
		//     this.maxID,
		//     bookId,
		//     bookName
		// );
		//});
	}

	deleteGLTRHeader(gltrHeader: GLTRHeaderDto): void {
		this.message.confirm("", isConfirmed => {
			if (isConfirmed) {
				this._voucherEntryServiceProxy
					.delete(gltrHeader.id)
					.subscribe(() => {
						this.reloadPage();
						this.notify.success(this.l("SuccessfullyDeleted"));
					});
			}
		});
	}

	exportToExcel(): void {
		this._gltrHeadersServiceProxy
			.getGLTRHeadersToExcel(
				this.filterText,
				this.bookIDFilter,
				this.maxConfigIDFilter == null
					? this.maxConfigIDFilterEmpty
					: this.maxConfigIDFilter,
				this.minConfigIDFilter == null
					? this.minConfigIDFilterEmpty
					: this.minConfigIDFilter,
				this.maxDocNoFilter == null
					? this.maxDocNoFilterEmpty
					: this.maxDocNoFilter,
				this.minDocNoFilter == null
					? this.minDocNoFilterEmpty
					: this.minDocNoFilter,
				this.maxDocMonthFilter == null
					? this.maxDocMonthFilterEmpty
					: this.maxDocMonthFilter,
				this.minDocMonthFilter == null
					? this.minDocMonthFilterEmpty
					: this.minDocMonthFilter,
				this.maxDocDateFilter,
				this.minDocDateFilter,
				this.narrationFilter,
				this.postedFilter,
				this.auditUserFilter,
				this.maxAuditTimeFilter,
				this.minAuditTimeFilter,
				this.oldCodeFilter,
				this.glconfigConfigIDFilter
			)
			.subscribe(result => {
				this._fileDownloadService.downloadTempFile(result);
			});
	}

	getReport(gltrHeader: GLTRHeaderDto) {
		//this.visible = true;
		this.rptObj = JSON.stringify({
			bookId: gltrHeader.bookID,
			year: gltrHeader.docDate.toDate().getFullYear(),
			month: gltrHeader.docMonth,
			locId: 0,
			fromConfigId: gltrHeader.configID,
			toConfigId: gltrHeader.configID,
			fromDoc: gltrHeader.fmtDocNo,
			toDoc: gltrHeader.fmtDocNo,
			tenantId: this.appSession.tenantId
		});
		this.reportUrl = "CashReceipt";

		let repParams = "";
		if (gltrHeader.bookID !== undefined)
			repParams += encodeURIComponent("" + gltrHeader.bookID) + "$";
		if (gltrHeader.docDate !== undefined)
			repParams +=
				encodeURIComponent(
					"" + gltrHeader.docDate.toDate().getFullYear()
				) + "$";
		if (gltrHeader.docMonth !== undefined)
			repParams += encodeURIComponent("" + gltrHeader.docMonth) + "$";
		if (gltrHeader.locId !== undefined)
			repParams += encodeURIComponent("" + gltrHeader.locId) + "$";
		if (gltrHeader.configID !== undefined)
			repParams += encodeURIComponent("" + gltrHeader.configID) + "$";
		if (gltrHeader.configID !== undefined)
			repParams += encodeURIComponent("" + gltrHeader.configID) + "$";
		if (gltrHeader.docNo !== undefined)
			repParams += encodeURIComponent("" + gltrHeader.fmtDocNo) + "$";
		if (gltrHeader.docNo !== undefined)
			repParams += encodeURIComponent("" + gltrHeader.fmtDocNo) + "$";

		repParams += encodeURIComponent("" + this.curid) + "$";
		repParams += encodeURIComponent("" + this.curRate) + "$";
		repParams = repParams.replace(/[?$]&/, "");
		// this.reportUrl = "CashReceipt";
		this.reportviewrModalComponent.show("CashReceipt", repParams);

		//this.reportviewrModalComponent.show();
	}

	uploadedFiles: any[] = [];

	onUpload(event): void {
		for (const file of event.files) {
			this.uploadedFiles.push(file);
		}
	}

	onBeforeSend(event): void {
		event.xhr.setRequestHeader(
			"Authorization",
			"Bearer " + abp.auth.getToken()
		);
	}

	uploadExcel(data: { files: File }): void {
		const formData: FormData = new FormData();
		const file = data.files[0];
		formData.append("file", file, file.name);

		this._httpClient
			.post<any>(this.uploadUrl, formData)
			.pipe(finalize(() => this.excelFileUpload.clear()))
			.subscribe(response => {
				if (response.success) {
					this.notify.success(
						this.l("ImportTransactionProcessStart")
					);
				} else if (response.error != null) {
					this.notify.error(this.l("ImportTransactionUploadFailed"));
				}
			});
	}

	onUploadExcelError(): void {
		this.notify.error(this.l("ImportTransationUploadFailed"));
	}

	filterBooks(result) {
		return result.items.filter(function (x) {
			return this.allowedBooks.indexOf(x.gltrHeader.bookID) >= 0
		});
	}
}
