import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild,
    OnInit,
    Output,
    EventEmitter
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import {
    VoucherPrintingReportsServiceProxy,
    VoucherEntryServiceProxy,
    GetBookViewModeldto,
    APTransactionListServiceProxy,
    MonthListDto,
    YearListDto
} from "@shared/service-proxies/service-proxies";
import { NotifyService } from "@abp/notify/notify.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { FileDownloadService } from "@shared/utils/file-download.service";
import * as _ from "lodash";
import * as moment from "moment";
///import { voucherprintDtoDto } from '../../dto/ledger-filters-dto';
import { ChartofcontrolLookupFinderComponent } from "../../chartofcontrol-lookup-finder/chartofcontrol-lookup-finder.component";
import { VoucherprintDto } from "../../dto/voucherprint-dto";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";
import { CommonServiceLookupTableModalComponent } from "@app/finders/commonService/commonService-lookup-table-modal.component";

@Component({
    selector: "app-voucherpring-report",
    templateUrl: "./voucherpring-Report.Component.html",
    // styleUrls: ['./ledger-rpt.component.css'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class VoucherpringReportComponent extends AppComponentBase
    implements OnInit {
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;
    @ViewChild("appchartofcontrollookupfinder", { static: true })
    appchartofcontrollookupfinder: ChartofcontrolLookupFinderComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild("CommonServiceLookupTableModal", { static: true })
    CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    bookId = "";
    year: number;
    month = "";
    fromConfig: number;
    toConfig: number;
    fromDoc: number;
    curid: any;
    currRate: any;
    toDoc: number;
    status: string;
    showReport: boolean = false;
    fromAccount = "";
    toAccount = "";
    private locationList;
    voucherprintDto: VoucherprintDto = new VoucherprintDto();
    chartOfAccountList: any;
    checkAccount: boolean;
    constructor(
        injector: Injector,
        private _voucherPrintingReportsServiceProxy: VoucherPrintingReportsServiceProxy,
        private _APTransactionListServiceProxy: APTransactionListServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private route: Router,
        private _route: ActivatedRoute
    ) {
        super(injector);
    }
    bookDto: GetBookViewModeldto[] = [];
    monthlistdto: MonthListDto[] = [];
    yearListdto: YearListDto[] = [];
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    nrSelect: string;
    ngOnInit() {
        //   this._voucherPrintingReportsServiceProxy.getAllChartofControlList().subscribe(result => {
        //     this.chartOfAccountList = result.items;

        // });
        this.voucherprintDto.bookId = "All";
        this.voucherprintDto.location = 0;
        this.getMonthListRecord(this.voucherprintDto.bookId);
        this.getYearListRecord(this.voucherprintDto.bookId);
        this.getBookListS();
        this.getLocationList();
        this.voucherprintDto.toConfig = 99999;
        this.voucherprintDto.fromConfig = 0;
        this.voucherprintDto.fromDoc = 0;
        this.voucherprintDto.toDoc = 99999;
        this._voucherEntryServiceProxy.getBaseCurrency().subscribe(result => {
            if (result) {
                this.voucherprintDto.curid = result.id;
                this.voucherprintDto.curRate = result.currRate;
            }
        });
    }

    ngAfterViewInit(): void {
        debugger
        this._route.queryParams.subscribe(params => {
        if(params['docNo'] != undefined)
        {
            var year = params['date'].toString().split('/')[2];
            var month = params['date'].toString().split('/')[1];
            this.voucherprintDto.fromDoc = params['docNo'];
            this.voucherprintDto.toDoc = params['docNo'];
           // this.voucherprintDto.year = year;
            this.voucherprintDto.month = month.toString();
            this.voucherprintDto.bookId = params['bookId'];
            this.voucherprintDto.fromConfig = 0;
            this.voucherprintDto.toConfig = 99999;
            this.voucherprintDto.location = 0;
            setTimeout(()=>{
              this.getReport()
            },5000);
        }
       });
     }


    getReport() {
        // this.parameters = {
        //   "bookId": this.voucherprintDto.bookId,
        //   "year": this.voucherprintDto.year,
        //   "month": this.voucherprintDto.month,
        //   "locId": this.voucherprintDto.location,
        //   "fromConfigId": this.voucherprintDto.fromConfig,
        //   "toConfigId": this.voucherprintDto.toConfig,
        //   "fromDoc": this.voucherprintDto.fromDoc,
        //   "toDoc": this.voucherprintDto.toDoc,
        //   "tenantId": this.appSession.tenantId
        // };

        let repParams = "";
        repParams += this.voucherprintDto.bookId + "$";
        if (this.voucherprintDto.year !== undefined) {
            repParams += this.voucherprintDto.year + "$";
        } else {
            repParams += null + "$";
        }

        if (this.voucherprintDto.month !== undefined) {
            repParams += this.voucherprintDto.month + "$";
        } else {
            repParams += null + "$";
        }

        repParams += this.voucherprintDto.location + "$";
        repParams += this.voucherprintDto.fromConfig + "$";
        repParams += this.voucherprintDto.toConfig + "$";
        repParams += this.voucherprintDto.fromDoc + "$";
        repParams += this.voucherprintDto.toDoc + "$";
        repParams += this.voucherprintDto.curid + "$";
        repParams += this.voucherprintDto.curRate + "$";
        repParams += this.status + "$";
        repParams = repParams.replace(/[?$]&/, "");
        this.reportView.show("CashReceipt", repParams);
    }

    openSelectCurrencyRateModal() {
        this.voucherprintDto.curid = "";
        this.voucherprintDto.curRate = 0;
        this.CommonServiceLookupTableModal.show("Currency");
    }

    setCurrencyRateIdNull() {
        this.voucherprintDto.curid = "";
        this.voucherprintDto.curRate = null;
    }

    getNewCommonServiceModal() {
        this.voucherprintDto.curid = this.CommonServiceLookupTableModal.id;
        this.voucherprintDto.curRate = this.CommonServiceLookupTableModal.currRate;
    }

    getLocationList(): void {
        this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
            this.locationList = resultL;
        });
    }

    getBookListS(): void {
        this._APTransactionListServiceProxy.getBookList().subscribe(result => {
            debugger;
            this.bookDto = result.items;
        });
    }

    getMonthListRecord(bookid: string): void {
        this._APTransactionListServiceProxy
            .getMonthList(bookid)
            .subscribe(result => {
                this.monthlistdto = result.items;
            });

        this.getYearListRecord(bookid);
    }
    getYearListRecord(bookid: string): void {
        this._APTransactionListServiceProxy
            .getYearList(bookid)
            .subscribe(result => {
                debugger;
                this.yearListdto = result.items;
            });
    }

    selectFromAccount() {
        // this.appchartofcontrollookupfinder.accid = this.voucherprintDto.fromAccount;
        // this.appchartofcontrollookupfinder.displayName = this.voucherprintDto.fromAccountName;
        // this.appchartofcontrollookupfinder.show();
    }

    setFromAccount() {
        // this.voucherprintDto.fromAccount = '';
        // this.voucherprintDto.fromAccountName = '';
    }

    getFromAndToAccount() {
        if (this.checkAccount) {
            // this.voucherprintDto.toAccount = this.appchartofcontrollookupfinder.accid;
            // this.voucherprintDto.toAccountName = this.appchartofcontrollookupfinder.displayName;
        } else {
            // this.voucherprintDto.fromAccount = this.appchartofcontrollookupfinder.accid;
            // this.voucherprintDto.fromAccountName = this.appchartofcontrollookupfinder.displayName;
            // this.voucherprintDto.toAccount = this.appchartofcontrollookupfinder.accid;
            // this.voucherprintDto.toAccountName = this.appchartofcontrollookupfinder.displayName;
        }
        this.checkAccount = false;
    }

    selectToAccount() {
        this.checkAccount = true;
        // this.appchartofcontrollookupfinder.accid = this.voucherprintDto.toAccount;
        // this.appchartofcontrollookupfinder.displayName = this.voucherprintDto.toAccountName;
        this.appchartofcontrollookupfinder.show();
    }

    setToAccount() {
        // this.voucherprintDto.toAccount = '';
        // this.voucherprintDto.toAccountName = '';
    }
}
