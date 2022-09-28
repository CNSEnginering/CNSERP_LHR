import {
    AfterViewInit,
    Component,
    Injector,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from "@angular/core";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import {
    TenantDashboardServiceProxy,
    SalesSummaryDatePeriod,
    CashBookForViewDto,
    CashFlowSummaryDatePeriod,
    GetCashFlowSummaryInput,
    GetBankStatusSummaryInput,
    BankStatusSummaryDatePeriod,
} from "@shared/service-proxies/service-proxies";
import { curveBasis } from "d3-shape";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { VoucherEntryServiceProxy } from "@shared/service-proxies/service-proxies";
import * as _ from "lodash";
import * as moment from "moment";
import { runInThisContext } from "vm";
import { ActivatedRoute } from "@angular/router";
import { ReportviewrModalComponent } from "@app/shared/common/reportviewr-modal/reportviewr-modal.component";

abstract class DashboardChartBase {
    loading = true;

    showLoading() {
        setTimeout(() => {
            this.loading = true;
        });
    }

    hideLoading() {
        setTimeout(() => {
            this.loading = false;
        });
    }
}

class SalesSummaryChart extends DashboardChartBase {
    totalSales = 0;
    totalSalesCounter = 0;
    revenue = 0;
    revenuesCounter = 0;
    expenses = 0;
    expensesCounter = 0;
    growth = 0;
    growthCounter = 0;

    selectedDatePeriod: SalesSummaryDatePeriod = SalesSummaryDatePeriod.Daily;

    data = [];

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(salesSummaryData, totalSales, revenue, expenses, growth) {
        this.totalSales = totalSales;
        this.totalSalesCounter = totalSales;

        this.revenue = revenue;
        this.expenses = expenses;
        this.growth = growth;

        this.setChartData(salesSummaryData);

        this.hideLoading();
    }

    setChartData(items): void {
        let sales = [];
        let profit = [];

        _.forEach(items, (item) => {
            sales.push({
                name: item["period"],
                value: item["sales"],
            });

            profit.push({
                name: item["period"],
                value: item["profit"],
            });
        });

        this.data = [
            {
                name: "Sales",
                series: sales,
            },
            {
                name: "Profit",
                series: profit,
            },
        ];
    }

    reload(datePeriod) {
        this.selectedDatePeriod = datePeriod;

        this.showLoading();
        this._dashboardService
            .getSalesSummary(datePeriod)
            .subscribe((result) => {
                this.setChartData(result.salesSummary);
                this.hideLoading();
            });
    }
}

class RegionalStatsTable extends DashboardChartBase {
    stats: Array<any>;
    colors = ["#00c5dc", "#f4516c", "#34bfa3", "#ffb822"];
    customColors = [
        { name: "1", value: "#00c5dc" },
        { name: "2", value: "#f4516c" },
        { name: "3", value: "#34bfa3" },
        { name: "4", value: "#ffb822" },
        { name: "5", value: "#00c5dc" },
    ];

    curve: any = curveBasis;

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init() {
        this.reload();
    }

    formatData(): any {
        for (let j = 0; j < this.stats.length; j++) {
            let stat = this.stats[j];

            let series = [];
            for (let i = 0; i < stat.change.length; i++) {
                series.push({
                    name: i + 1,
                    value: stat.change[i],
                });
            }

            stat.changeData = [
                {
                    name: j + 1,
                    series: series,
                },
            ];
        }
    }

    reload() {
        this.showLoading();
        this._dashboardService.getRegionalStats().subscribe((result) => {
            this.stats = result.stats;
            this.formatData();
            this.hideLoading();
        });
    }
}

class GeneralStatsPieChart extends DashboardChartBase {
    public data = [];

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(transactionPercent, newVisitPercent, bouncePercent) {
        this.data = [
            {
                name: "Operations",
                value: transactionPercent,
            },
            {
                name: "New Visits",
                value: newVisitPercent,
            },
            {
                name: "Bounce",
                value: bouncePercent,
            },
        ];

        this.hideLoading();
    }

    reload() {
        this.showLoading();
        this._dashboardService.getGeneralStats().subscribe((result) => {
            this.init(
                result.transactionPercent,
                result.newVisitPercent,
                result.bouncePercent
            );
        });
    }
}

class DailySalesLineChart extends DashboardChartBase {
    chartData: any[];
    scheme: any = {
        name: "green",
        selectable: true,
        group: "Ordinal",
        domain: ["#34bfa3"],
    };

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(data) {
        this.chartData = [];
        for (let i = 0; i < data.length; i++) {
            this.chartData.push({
                name: i + 1,
                value: data[i],
            });
        }
    }

    reload() {
        this.showLoading();
        this._dashboardService
            .getSalesSummary(SalesSummaryDatePeriod.Monthly)
            .subscribe((result) => {
                this.init(result.salesSummary);
                this.hideLoading();
            });
    }
}

class ProfitSharePieChart extends DashboardChartBase {
    chartData: any[] = [];
    scheme: any = {
        name: "custom",
        selectable: true,
        group: "Ordinal",
        domain: ["#00c5dc", "#ffb822", "#716aca"],
    };

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(data: number[]) {
        let formattedData = [];
        for (let i = 0; i < data.length; i++) {
            formattedData.push({
                name: this.getChartItemName(i),
                value: data[i],
            });
        }

        this.chartData = formattedData;
    }

    getChartItemName(index: number) {
        if (index === 0) {
            return "Product Sales";
        }

        if (index === 1) {
            return "Online Courses";
        }

        if (index === 2) {
            return "Custom Development";
        }

        return "Other";
    }
}

class DashboardHeaderStats extends DashboardChartBase {
    totalProfit = 0;
    totalProfitCounter = 0;
    newFeedbacks = 0;
    newFeedbacksCounter = 0;
    newOrders = 0;
    newOrdersCounter = 0;
    newUsers = 0;
    newUsersCounter = 0;

    totalProfitChange = 76;
    totalProfitChangeCounter = 0;
    newFeedbacksChange = 85;
    newFeedbacksChangeCounter = 0;
    newOrdersChange = 45;
    newOrdersChangeCounter = 0;
    newUsersChange = 57;
    newUsersChangeCounter = 0;



    constructor(
        private _dashboardService: TenantDashboardServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy
    ) {
        super();
    }

    // init(totalProfit, newFeedbacks, newOrders, newUsers) {
    //     this.totalProfit = totalProfit;
    //     this.newFeedbacks = newFeedbacks;
    //     this.newOrders = newOrders;
    //     this.newUsers = newUsers;
    //     this.hideLoading();
    // }

    init() {
        this.hideLoading();
    }
}

class MemberActivityTable extends DashboardChartBase {
    memberActivities: Array<any>;

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init() {
        this.reload();
    }

    reload() {
        this.showLoading();
        this._dashboardService.getMemberActivity().subscribe((result) => {
            this.memberActivities = result.memberActivities;
            this.hideLoading();
        });
    }
}

class CashFlowSummary extends DashboardChartBase {
    selectedDatePeriod: CashFlowSummaryDatePeriod =
        CashFlowSummaryDatePeriod.Daily;

    GetCashFlowSummaryInput: GetCashFlowSummaryInput = new GetCashFlowSummaryInput();
    single: any[];
    multi: any[];

    obj = [];

    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = true;
    showXAxisLabel = true;
    xAxisLabel = "Date";
    showYAxisLabel = true;
    yAxisLabel = "Value";

    colorScheme = {
        domain: ["#5AA454", "#A10A28", "#C7B42C", "#AAAAAA"],
    };

    // pie
    showLabels = true;
    explodeSlices = false;
    doughnut = false;

    memberActivities: Array<any>;
    fromdate: moment.Moment;
    todate: moment.Moment;
    fromAccountID: string;
    toAccountID: string;
    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(param) {
        this.reload(param);
    }

    reload(datePeriod) {
        this.selectedDatePeriod = datePeriod;
        this.GetCashFlowSummaryInput.cashFlowSummaryDatePeriod = datePeriod;
        this.showLoading();
        this._dashboardService
            .cashFlowSummary(this.GetCashFlowSummaryInput)
            .subscribe((result) => {
                this.multi = this.chartData(result.items);
                this.hideLoading();
            });
    }

    chartData(data): any {
        var arr = [];

        var i = 0;
        data.forEach((e) => {
            arr.push({
                name: e.docDate,
                series: [
                    {
                        name: "Cash In",
                        value: e.debit,
                    },
                    {
                        name: "Cash Out",
                        value: Math.abs(e.credit),
                    },
                ],
            });

            // }
            i++;
        });

        return arr;
    }

    onSelect(event) {
        console.log(event);
    }
}

class BankStatusSummary extends DashboardChartBase {
    selectedDatePeriod: BankStatusSummaryDatePeriod =
        BankStatusSummaryDatePeriod.Daily;

    GetBankStatusSummaryInput: GetBankStatusSummaryInput = new GetBankStatusSummaryInput();
    single: any[];
    multi: any[];

    obj = [];

    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = true;
    showXAxisLabel = true;
    xAxisLabel = "Date";
    showYAxisLabel = true;
    yAxisLabel = "Value";

    colorScheme = {
        domain: ["#5AA454", "#A10A28", "#C7B42C", "#AAAAAA"],
    };

    // pie
    showLabels = true;
    explodeSlices = false;
    doughnut = false;

    memberActivities: Array<any>;
    fromdate: moment.Moment;
    todate: moment.Moment;
    fromAccountID: string;
    toAccountID: string;
    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(param) {
        this.reload(param);
    }

    reload(datePeriod) {
        this.selectedDatePeriod = datePeriod;
        this.GetBankStatusSummaryInput.bankStatusSummaryDatePeriod = datePeriod;
        this.showLoading();
        this._dashboardService
            .bankStatusSummary(this.GetBankStatusSummaryInput)
            .subscribe((result) => {
                this.multi = this.chartData(result.items);
                this.hideLoading();
            });
    }

    chartData(data): any {
        var arr = [];

        var i = 0;
        data.forEach((e) => {
            arr.push({
                name: e.docDate,
                series: [
                    {
                        name: "Cash In",
                        value: e.debit,
                    },
                    {
                        name: "Cash Out",
                        value: Math.abs(e.credit),
                    },
                ],
            });

            // }
            i++;
        });

        return arr;
    }

    onSelect(event) {
        console.log(event);
    }
}

interface IPostDatedCheque {
    docId: number;
    chequeDate: Date;
    partyID: number;
    subAccName: string;
    chequeAmt: number;
}

interface IBankOverDraft {
    bankID: string;
    bankname: string;
    idacctbank: string;
    odlimit: number;
    usedLimit: number;
    balanceLimit: number;
}

interface IPartyBalance {
    subAccID: string;
    subAccName: string;
    amount: number;
}

interface IStockBalance {
    accountid: string;
    accountName: string;
    balance: number;
}
interface ICashDetailBalance {
    accountid: string;
    accountName: string;
    balance: number;
}

// class PostDatedChequesRecieved {

//     constructor(private _dashboardService: TenantDashboardServiceProxy) {
//     }

//     init(data:any[]) {

//         this.postDatedChequeRcvd = data;
//         console.log(this.postDatedChequeRcvd);
//     }

// }

@Component({
    templateUrl: "./dashboard.component.html",
    styleUrls: ["./dashboard.component.less"],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class DashboardComponent
    extends AppComponentBase
    implements AfterViewInit {
    @ViewChild("reportviewrModalComponent", { static: false }) reportView: ReportviewrModalComponent;
    appSalesSummaryDateInterval = SalesSummaryDatePeriod;
    appCashFlowSummaryDateInterval = CashFlowSummaryDatePeriod;
    appBankStatusSummaryDateInterval = BankStatusSummaryDatePeriod;
    selectedSalesSummaryDatePeriod: any = SalesSummaryDatePeriod.Daily;
    selectedSCashFlowSummaryDatePeriod: any = CashFlowSummaryDatePeriod.Daily;
    selectedBankStatusSummaryDatePeriod: any =
        BankStatusSummaryDatePeriod.Daily;
    dashboardHeaderStats: DashboardHeaderStats;
    salesSummaryChart: SalesSummaryChart;
    regionalStatsTable: RegionalStatsTable;
    generalStatsPieChart: GeneralStatsPieChart;
    dailySalesLineChart: DailySalesLineChart;
    profitSharePieChart: ProfitSharePieChart;
    memberActivityTable: MemberActivityTable;
    cashFlowSummary: CashFlowSummary;
    bankStatusSummary: BankStatusSummary;

    //postDatedChequeRecieved: PostDatedChequesRecieved;

    cashBookForViewDto: CashBookForViewDto = new CashBookForViewDto();

    postDatedChequeRcvd: IPostDatedCheque[];
    postDatedChequeIssued: IPostDatedCheque[];
    bankOverDraft: IBankOverDraft[];
    partyRecieveable: IPartyBalance[];
    partyPayable: IPartyBalance[];
    stockBalance: IStockBalance[];
    CashDetail: ICashDetailBalance[];
    BankDetail: ICashDetailBalance[];
    currencySymbol: String;

    totalChequeRecieved: number;
    totalChequeIssued: number;
    totalPartyRecieveable: number;
    totalPartyPayable: number;
    totalBankOverDraft: number;
    totalStockBalance: number;
    totalCashDetail: number;
    totalBankDetail: number;
    cashBalance: number = 0;
    bankBalance: number = 0;
    totalCashPos: number = 0;
    view: any[] = [400, 300];
    CashDetailChartArr = [];
    PostDatedChequeRcvdChartArr = [];
    PostDatedChequeIssuedChartArr = [];
    BankOverDraftChartArr = [];
    PartyRecieveableChartArr = [];
    PartyPayableChartArr = [];
    StockBalanceChartArr = [];
    BankDetailChartArr = [];
    // options
    legendPosition: string = "below";
    showXAxis = true;
    showYAxis = true;
    gradient = true;
    showLegend = true;
    showXAxisLabel = true;
    xAxisLabel = "Account IDs";
    showYAxisLabel = true;
    yAxisLabel = "Balance";
    CashDetailChartYAxisMax;
    CashDetailChartYAxisMin;
    BankDetailChartYAxisMax;
    BankDetailChartYAxisMin;
    colorScheme = {
        domain: ["#5AA454", "#A10A28", "#C7B42C", "#AAAAAA"],
    };

    constructor(
        injector: Injector,
        private _dashboardService: TenantDashboardServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy
    ) {
        super(injector);
        this.dashboardHeaderStats = new DashboardHeaderStats(
            this._dashboardService,
            this._voucherEntryServiceProxy
        );
        this.salesSummaryChart = new SalesSummaryChart(this._dashboardService);
        this.regionalStatsTable = new RegionalStatsTable(
            this._dashboardService
        );
        this.generalStatsPieChart = new GeneralStatsPieChart(
            this._dashboardService
        );
        this.dailySalesLineChart = new DailySalesLineChart(
            this._dashboardService
        );
        this.profitSharePieChart = new ProfitSharePieChart(
            this._dashboardService
        );
        this.memberActivityTable = new MemberActivityTable(
            this._dashboardService
        );
        this.cashFlowSummary = new CashFlowSummary(this._dashboardService);
        this.bankStatusSummary = new BankStatusSummary(this._dashboardService);
    }

    numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    getDashboardStatisticsData(datePeriod): void {
        this.salesSummaryChart.showLoading();
        this.cashFlowSummary.showLoading();
        this.bankStatusSummary.showLoading();
        this.generalStatsPieChart.showLoading();
        this._dashboardService
            .getDashboardData(datePeriod)
            .subscribe((result) => {
                // this.dashboardHeaderStats.init(result.totalProfit, result.newFeedbacks, result.newOrders, result.newUsers);
                this.generalStatsPieChart.init(
                    result.transactionPercent,
                    result.newVisitPercent,
                    result.bouncePercent
                );
                this.dailySalesLineChart.init(result.dailySales);
                this.profitSharePieChart.init(result.profitShares);
                this.salesSummaryChart.init(
                    result.salesSummary,
                    result.totalSales,
                    result.revenue,
                    result.expenses,
                    result.growth
                );
            });
    }

    getCurrencySymbol() {
        this._voucherEntryServiceProxy.getBaseCurrency().subscribe((result) => {
            if (result) {
                this.currencySymbol = result.symbol;
            }
        });
    }

    getDashboardHeaderStats() {
        this._dashboardService.getheaderData().subscribe((res) => {
            console.log(res);
            
            this.cashBalance = res["result"]["cashBalance"];
            this.bankBalance = res["result"]["bankBalance"];
            this.totalCashPos = res["result"]["totalBalance"];
        });
    }

    getPostDatedChRcvd() {
        this._dashboardService
            .getPostDatedChequeRecievedData()
            .subscribe((res) => {
                //      console.log(res);
                this.postDatedChequeRcvd = res["result"];
                this.totalChequeRecieved = this.postDatedChequeRcvd.reduce(
                    (a, b) => a + b.chequeAmt,
                    0
                );
                // console.log(this.totalChequeRecieved);

                single = [];
                this.postDatedChequeRcvd.forEach((d) => {
                    single.push({
                        name: d.subAccName,
                        value: d.chequeAmt,
                    });
                });
                this.PostDatedChequeRcvdChartArr = single;
            });
    }

    getPostDatedChIssued() {
        this._dashboardService
            .getPostDatedChequeIssuedData()
            .subscribe((res) => {
                // console.log(res["result"]);
                this.postDatedChequeIssued = res["result"];
                this.totalChequeIssued = this.postDatedChequeIssued.reduce(
                    (a, b) => a + b.chequeAmt,
                    0
                );
                single = [];
                this.postDatedChequeIssued.forEach((d) => {
                    single.push({
                        name: d.subAccName,
                        value: d.chequeAmt,
                    });
                });
                this.PostDatedChequeIssuedChartArr = single;
            });
    }

    getBankOverDraft() {
        this._dashboardService.getBankOverDraftData().subscribe((res) => {
            //  console.log(res);
            this.bankOverDraft = res["result"];
            this.totalBankOverDraft = this.bankOverDraft.reduce(
                (a, b) => a + b.balanceLimit,
                0
            );

            single = [];
            this.bankOverDraft.forEach((d) => {
                single.push({
                    name: d.bankname,
                    value: d.balanceLimit,
                });
            });
            this.BankOverDraftChartArr = single;
        });
    }

    getPartyRecieveable() {
        this._dashboardService.getPartyRecieveableData().subscribe((res) => {
            //  console.log(res);
            this.partyRecieveable = res["result"];
            this.totalPartyRecieveable = this.partyRecieveable.reduce(
                (a, b) => a + b.amount,
                0
            );
            single = [];
            this.partyRecieveable.forEach((d) => {
                single.push({
                    name: d.subAccName,
                    value: d.amount,
                });
            });
            this.PartyRecieveableChartArr = single;
        });
    }

    getPartyPayable() {
        this._dashboardService.getPartyPayableData().subscribe((res) => {
            //  console.log(res);
            this.partyPayable = res["result"];
            this.totalPartyPayable = this.partyPayable.reduce(
                (a, b) => a + b.amount,
                0
            );
            single = [];
            this.partyPayable.forEach((d) => {
                single.push({
                    name: d.subAccName,
                    value: d.amount,
                });
            });
            this.PartyPayableChartArr = single;
        });
    }

    getStockBalances() {
        this._dashboardService.getStockBalancesData().subscribe((res) => {
            console.log(res);
            this.stockBalance = res["result"];
            this.totalStockBalance = this.stockBalance.reduce(
                (a, b) => a + b.balance,
                0
            );
            single = [];
            this.stockBalance.forEach((d) => {
                single.push({
                    name: d.accountName,
                    value: d.balance,
                });
            });
            this.StockBalanceChartArr = single;
        });
    }
    getCashDetailBalance() {
        
        this._dashboardService.getCashBalnceDetailData().subscribe((res) => {
            this.CashDetail = res["result"];
            this.totalCashDetail = this.CashDetail.reduce(
                (a, b) => a + b.balance,
                0
            );
            single = [];
            this.CashDetail.forEach((d) => {
                single.push({
                    name: d.accountName,
                    value: d.balance,
                });
            });
            this.CashDetailChartArr = single;
            this.CashDetailChartYAxisMax = Math.max.apply(
                Math,
                this.CashDetailChartArr.map(function (o) {
                    return o.value;
                })
            );
            this.CashDetailChartYAxisMax = this.CashDetailChartYAxisMax / 5;
            this.CashDetailChartYAxisMin = Math.min.apply(
                Math,
                this.CashDetailChartArr.map(function (o) {
                    return o.value;
                })
            );

            // console.log(this.CashDetailChartArr);
        });
    }
    getBankDetailBalance() {
        this._dashboardService.getBankBalnceDetailData().subscribe((res) => {
            //   console.log(res);
            this.BankDetail = res["result"];
            this.totalBankDetail = this.BankDetail.reduce(
                (a, b) => a + b.balance,
                0
            );
            single = [];
            this.BankDetail.forEach((d) => {
                single.push({
                    name: d.accountName,
                    value: d.balance,
                });
            });
            this.BankDetailChartArr = single;
            this.BankDetailChartYAxisMax = Math.max.apply(
                Math,
                this.BankDetailChartArr.map(function (o) {
                    return o.value;
                })
            );

            this.BankDetailChartYAxisMin = Math.min.apply(
                Math,
                this.BankDetailChartArr.map(function (o) {
                    return o.value;
                })
            );
        });
    }

    ngAfterViewInit(): void {
        this.reportView.show("", "$");
        this.getCurrencySymbol();
        this.getPostDatedChRcvd();
        this.getPostDatedChIssued();
        this.getBankOverDraft();
        this.getPartyPayable();
        this.getPartyRecieveable();
        this.getStockBalances();
        this.getCashDetailBalance();
        this.getBankDetailBalance();
        this.getDashboardHeaderStats();
        // this.getDashboardStatisticsData(SalesSummaryDatePeriod.Daily);
        // this.regionalStatsTable.init();
        // this.memberActivityTable.init();
        // this.cashFlowSummary.init(CashFlowSummaryDatePeriod.Daily);
        // this.bankStatusSummary.init(BankStatusSummaryDatePeriod.Daily);
    }
}

export var single = [];
