import {
    Component,
    Injector,
    ViewEncapsulation,
    ViewChild
} from "@angular/core";
import { Router } from "@angular/router";
import {
    APTransactionListServiceProxy,
    GetBookViewModeldto,
    UserDto,
    VoucherEntryServiceProxy
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/common/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import * as _ from "lodash";
import * as moment from "moment";
import { FiscalDateService } from "@app/shared/services/fiscalDate.service";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";

@Component({
    templateUrl: "./aptransactionlist.component.html",
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class APTransactionListComponent extends AppComponentBase {
    @ViewChild("reportView", { static: true })
    reportView: ReportviewrModalComponent;

    fromDate;
    toDate;
    bookId = "";
    userId = "";
    directPost = false;
    status = "";
    showReport: boolean = false;
    reportServer: string;
    reportUrl: string;
    showParameters: string;
    parameters: any;
    language: string;
    width: number;
    height: number;
    toolbar: string;
    rptObj: any;
    private locationList;
    location: number;
    constructor(
        injector: Injector,
        private _apTransactionListServiceProxy: APTransactionListServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
        private route: Router,
        private _reportService: FiscalDateService
    ) {
        super(injector);
    }

    bookDto: GetBookViewModeldto[] = [];
    userDto: UserDto[] = [];

    getLocationList(): void {
        this._voucherEntryServiceProxy.getGLLocData().subscribe(resultL => {
            this.locationList = resultL;
        });
    }
    ngOnInit() {
        this.getLocationList();
        this.bookId = "All";
        this.userId = "All";
        // this.status = "0";
        this.getBookList();
        this.getUserList();
        this.toDate = new Date();
        this._reportService.getDate().subscribe(data => {
            this.fromDate = new Date(data["result"]);
        });
        // var toDate = moment().format("MM/DD/YYYY");
        // this.toDate = toDate;
        // var fromDate = moment("2019-07-01").format("MM/DD/YYYY");
        // this.fromDate = fromDate;
        // this._reportService.getDate().subscribe(
        //     data =>{
        //       var fromDate =  moment(data["result"]).format("MM/DD/YYYY");
        //          console.log(data["result"])
        //          console.log(fromDate)
        //          this.fromDate = fromDate;
        //     }
        //   )
        this.location = 0;
        this.status = "All";
        this.bookId = "CR";
    }

    getBookList(): void {
        this._apTransactionListServiceProxy.getBookList().subscribe(result => {
            this.bookDto = result.items;
        });
    }

    getUserList(): void {
        this._apTransactionListServiceProxy.getUserList().subscribe(result => {
            this.userDto = result.items;
        });
    }

    getReport() {
        var result = this.bookDto.find(x => x.bookId == this.bookId);
        let repParams = "";
        if (this.fromDate !== undefined)
            repParams += "" + moment(this.fromDate).format("YYYY/MM/DD") + "$";
        if (this.toDate !== undefined)
            repParams += "" + moment(this.toDate).format("YYYY/MM/DD") + "$";
        if (this.bookId !== undefined)
            repParams += encodeURIComponent("" + this.bookId) + "$";
        if (this.userId !== undefined)
            repParams += encodeURIComponent("" + this.userId) + "$";
        if (this.status !== undefined)
            repParams += encodeURIComponent("" + this.status) + "$";
        if (this.location !== undefined)
            repParams += encodeURIComponent("" + this.location) + "$";
        if (this.bookId !== undefined)
            repParams += encodeURIComponent("" + result.bookName) + "$";

        this.reportView.show("Finance_TransList", repParams);
    }
}
