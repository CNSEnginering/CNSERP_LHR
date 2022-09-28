using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using ERP.AccountPayables.Setup.ApSetup.Dtos;
using ERP.Common.AlertLog.Dtos;
using ERP.Common.AuditPostingLogs;
using ERP.CommonServices;
using ERP.CommonServices.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.Reports;
using ERP.Reports.Dto;
using ERP.Reports.Finance;
using ERP.Reports.Finance.Dto;
using ERP.Reports.GeneralLedger;
using ERP.Reports.GeneralLedger.Dtos;
using ERP.Reports.PayRoll;
using ERP.Reports.PayRoll.Dtos;
using ERP.Reports.SupplyChain.Inventory;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.Reports.SupplyChain.Purchase;
using ERP.Reports.SupplyChain.Sales;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ERP.Reports.Finance.GeneralLedgerReportAppService;
using static ERP.Reports.Finance.SubledgerReportAppService;
using static ERP.Reports.PayRoll.TaxCertificateAppService;

namespace ERP.Web.DXServices.DataHandler
{
    /// <summary>
    /// Added By Waleed Khalid
    /// </summary>
    public class ReportDataHandlerBase
    {
        //for comman source(Company Information)
        private static ICompanyProfilesAppService _CompanyProfilesAppService;


        private static IRepository<AuditLog, long> _auditLogRepository;
        private static IRepository<ICSetup> _icSetupRepository;
        private static IRepository<GLOption> _glOptionRepository;
        private static ISubLedgerTrailAppService _subLedgerTrailAppService;
        private static IItemListingAppService _ItemListingAppService;
        private static IAssetRegListingAppService _assetRegListingAppService;
        private static IAssetRegistrationReportAppService _assetRegReportAppService;
        private static readonly IItemLedgerDetailAppService _itemLedgerDetailAppService;
        private static readonly AdjustmentReportAppService _adjustmentReportAppService;
        private static readonly ConsumptionReportAppService _consumptionReportAppService;
        private static readonly OpeningReportAppService _openingReportAppService;
        private static IItemsPriceListAppService _itemsPriceAppService;
        private static IDepartmentListingAppService _DepartmentListingAppService;
        private static IDesignationAppService _DesignationListingAppService;
        private static IEducationAppService _EducationListingAppService;
        private static ILocationAppService _LocationListingAppService;
        private static IReligionAppService _ReligionListingAppService;
        private static IEmployeeTypeAppService _EmployeeTypeListingAppService;
        private static IShiftListingAppService _ShiftListingAppService;
        private static ISectionListingAppService _SectionListingAppService;
        private static IGradeListingAppService _GradeListingAppService;
        private static IEmployeeArrearsTransactionAppService _EmployeeArrearsTransactionAppService;
        private static IEmployeeEarningTransactionAppService _EmployeeEarningTransactionAppService;
        private static IEmployeeListingAppService _EmployeeListingAppService;
        private static IEmployeeCardListingAppService _EmployeeCardListingAppService;
        private static IEmployeeDeductionTransactionAppService _EmployeeDeductionTransactionAppService;
        private static ISalarySheetListingAppService _SalarySheetListingAppService;
        private static ISalarySheetSummaryAppService _SalarySheetSummaryAppService;
        private static IAttendanceTransactionAppService _AttendanceTransactionAppService;
        private static IAttendanceSummaryAppService _AttendanceSummaryAppService;
        private static ISalarySlipAppService _SalarySlipAppService;
        private static IBankAdviceAppService _BankAdviceAppService;

        private static ILedgerTypeListingAppService _LedgerTypeListingAppService;
        private static IGLLocationListingAppService _GLLocationListingAppService;
        private static IGLCategoriesListingAppService _GLCategoriesListingAppService;
        private static IGLGroupListingAppService _GLGroupListingAppService;
        private static IGLLevel1ListingAppService _GLLevel1ListingAppService;
        private static IGLLevel2ListingAppService _GLLevel2ListingAppService;
        private static IGLLevel3ListingAppService _GLLevel3ListingAppService;
        private static IGLBooksListingAppService _GLBooksListingAppService;
        private static IGLConfigListingAppService _GLConfigListingAppService;
        private static ISalesTaxWithheldReportAppService _SalesTaxWithheldReportAppService;
        private static ITaxDueReportAppService _TaxDueReportAppService;
        private static IPostDatedChequeAppService _PostDatedChequeAppService;
        private static ICPRNumbersReportAppService _CPRNumbersAppService;
        private static SalesTaxDeductionAppService _SalesTaxDeductionAppService;
        private static IPartyTaxReportAppService _PartyTaxAppService;



        private static ICashReceiptReportAppService _CashReceiptReportAppService;
        private static ChartOfACListingReportAppService _chartOfACListingReportAppService;
        private static PDCSubReportAppService _pdcSubReportAppService;
        private static GeneralLedgerReportAppService _generalLedgerReportAppService;
        private static CustAnalysisReportAppService _custAnalysisReportAppService;
        private static SubledgerReportAppService _subledgerReportAppService;
        private static readonly IStockReportSegment2AppService _stockReportSegment2AppService;


        private static CashBookReportAppService _CashBookReportAppService;
        private static IItemQuantitativeStockAppService _itemQuantitativeStockAppService;
        private static ITrialBalanceAppService _trialBalanceAppService;
        private static IAPTransactionListingReportsAppService _apTransactionListingReportsAppService;
        private static IStockAssemblyAppService _stockAssemblyAppService;
        private static IStockTransferAppService _stockTransferAppService;
        private static IGatePassReportAppService _gatePassReportAppService;
        private static IRequisitionStatusReportAppService _requisitionStatusReportAppService;

        private static IRequisitionReportAppService _requisitionReportAppService;
        private static IBankReconcileReportAppService _bankReconcileReportAppService;
        private static IBankReconcilationReportAppService _bankReconcilationReportAppService;
        public static IPurchaseOrderReportAppService _purchaseOrderReportAppService;
        private static IPurchaseOrderStatusReportAppService _purchaseOrderStatusReportAppService;


        private static IMonthlyConsolidatedReportAppService _monthlyConsolidatedReportAppService;

        private static IReceiptReportAppService _receiptReportAppService;
        private static IReceiptReturnReportAppService _receiptReturnReportAppService;
        private static ITransferRegisterAppService _transferRegisterAppService;
        private static IStockAssemblyRegisterAppService _stockAssemblyRegisterAppService;

        private static IGatePassRegisterAppService _gatePassRegisterAppService;

        private static IInvoiceAppService _invoiceAppService;
        private static IStockReportAppService _StockReportAppService;
        private static IConsumptionReportDepartmentWiseAppService _consumptionReportDepartmentWiseAppService;

        private static IConsumptionSummaryDepartmentWiseAppService _consumptionSummaryDepartmentWiseAppService;
        private static IActualAndBudgetAppService _actualAndBudgetAppService;
        private static ILCExpensesReportAppService _LCExpensesReportAppService;
        private static ICustomerAgingReportAppService _cutomerAgingAppService;
        private static BillWiseAgingReportAppService _billWiseAgingReportAppService;

        private static IAlertLogReportAppService _AlertLogReportAppService;
        private static ProfitAndLossStatment _ProfitAndLossStatment;
        private static BalanceSheetAppService _balanceSheetAppService;
        private static PlStatementCategoryDetail _plStatementCategoryDetail;
        private static PlStatementVoucherDetail _plStatementVoucherDetail;
        private static EmployeeLeavesAppService _employeeLeavesAppService;
        private static AuditPostingLogsReportAppService _auditPostingLogsReportAppService;

        private static EmployeeLoanLedgerAppService _employeeLoanLedgerAppService;
        private static AssetListingAppService _AssetListingAppService;
        private static ITaxCertificateAppService _TaxCertificateAppService;
        static ReportDataHandlerBase()
        {
            _AssetListingAppService = IocManager.Instance.Resolve<AssetListingAppService>();
            _employeeLoanLedgerAppService = IocManager.Instance.Resolve<EmployeeLoanLedgerAppService>();
            _employeeLeavesAppService = IocManager.Instance.Resolve<EmployeeLeavesAppService>();
            _balanceSheetAppService = IocManager.Instance.Resolve<BalanceSheetAppService>();
            _auditPostingLogsReportAppService = IocManager.Instance.Resolve<AuditPostingLogsReportAppService>();
            _plStatementVoucherDetail = IocManager.Instance.Resolve<PlStatementVoucherDetail>();
            _plStatementCategoryDetail = IocManager.Instance.Resolve<PlStatementCategoryDetail>();
            _pdcSubReportAppService = IocManager.Instance.Resolve<PDCSubReportAppService>();
            _billWiseAgingReportAppService = IocManager.Instance.Resolve<BillWiseAgingReportAppService>();
            _CompanyProfilesAppService = IocManager.Instance.Resolve<ICompanyProfilesAppService>();
            _auditLogRepository = IocManager.Instance.Resolve<IRepository<AuditLog, long>>();
            _icSetupRepository = IocManager.Instance.Resolve<IRepository<ICSetup>>();
            _glOptionRepository = IocManager.Instance.Resolve<IRepository<GLOption>>();

            _subLedgerTrailAppService = IocManager.Instance.Resolve<ISubLedgerTrailAppService>();
            _ItemListingAppService = IocManager.Instance.Resolve<IItemListingAppService>();
            _assetRegListingAppService = IocManager.Instance.Resolve<IAssetRegListingAppService>();
            _assetRegReportAppService = IocManager.Instance.Resolve<IAssetRegistrationReportAppService>();
            _itemLedgerDetailAppService = IocManager.Instance.Resolve<IItemLedgerDetailAppService>();
            _adjustmentReportAppService = IocManager.Instance.Resolve<AdjustmentReportAppService>();
            _consumptionReportAppService = IocManager.Instance.Resolve<ConsumptionReportAppService>();
            _openingReportAppService = IocManager.Instance.Resolve<OpeningReportAppService>();
            _itemsPriceAppService = IocManager.Instance.Resolve<IItemsPriceListAppService>();
            _DepartmentListingAppService = IocManager.Instance.Resolve<IDepartmentListingAppService>();
            _DesignationListingAppService = IocManager.Instance.Resolve<IDesignationAppService>();
            _EducationListingAppService = IocManager.Instance.Resolve<IEducationAppService>();
            _LocationListingAppService = IocManager.Instance.Resolve<ILocationAppService>();
            _ReligionListingAppService = IocManager.Instance.Resolve<IReligionAppService>();
            _EmployeeTypeListingAppService = IocManager.Instance.Resolve<IEmployeeTypeAppService>();
            _ShiftListingAppService = IocManager.Instance.Resolve<IShiftListingAppService>();
            _SectionListingAppService = IocManager.Instance.Resolve<ISectionListingAppService>();
            _GradeListingAppService = IocManager.Instance.Resolve<IGradeListingAppService>();
            _chartOfACListingReportAppService = IocManager.Instance.Resolve<ChartOfACListingReportAppService>();
            _EmployeeArrearsTransactionAppService = IocManager.Instance.Resolve<IEmployeeArrearsTransactionAppService>();
            _EmployeeListingAppService = IocManager.Instance.Resolve<IEmployeeListingAppService>();
            _EmployeeCardListingAppService = IocManager.Instance.Resolve<IEmployeeCardListingAppService>();
            _AttendanceTransactionAppService = IocManager.Instance.Resolve<IAttendanceTransactionAppService>();
            _AttendanceSummaryAppService = IocManager.Instance.Resolve<IAttendanceSummaryAppService>();
            _SalarySlipAppService = IocManager.Instance.Resolve<ISalarySlipAppService>();
            _BankAdviceAppService = IocManager.Instance.Resolve<IBankAdviceAppService>();


            _LedgerTypeListingAppService = IocManager.Instance.Resolve<ILedgerTypeListingAppService>();
            _GLLocationListingAppService = IocManager.Instance.Resolve<IGLLocationListingAppService>();
            _GLCategoriesListingAppService = IocManager.Instance.Resolve<IGLCategoriesListingAppService>();
            _GLGroupListingAppService = IocManager.Instance.Resolve<IGLGroupListingAppService>();
            _GLLevel1ListingAppService = IocManager.Instance.Resolve<IGLLevel1ListingAppService>();
            _GLLevel2ListingAppService = IocManager.Instance.Resolve<IGLLevel2ListingAppService>();
            _GLLevel3ListingAppService = IocManager.Instance.Resolve<IGLLevel3ListingAppService>();
            _GLBooksListingAppService = IocManager.Instance.Resolve<IGLBooksListingAppService>();
            _GLConfigListingAppService = IocManager.Instance.Resolve<IGLConfigListingAppService>();
            _SalesTaxWithheldReportAppService = IocManager.Instance.Resolve<ISalesTaxWithheldReportAppService>();
            _TaxDueReportAppService = IocManager.Instance.Resolve<ITaxDueReportAppService>();
            _PostDatedChequeAppService = IocManager.Instance.Resolve<IPostDatedChequeAppService>();
            _CPRNumbersAppService = IocManager.Instance.Resolve<ICPRNumbersReportAppService>();
            _SalesTaxDeductionAppService = IocManager.Instance.Resolve<SalesTaxDeductionAppService>();
            _PartyTaxAppService = IocManager.Instance.Resolve<IPartyTaxReportAppService>();


            _stockReportSegment2AppService = IocManager.Instance.Resolve<IStockReportSegment2AppService>();
            _EmployeeEarningTransactionAppService = IocManager.Instance.Resolve<IEmployeeEarningTransactionAppService>();
            _EmployeeDeductionTransactionAppService = IocManager.Instance.Resolve<IEmployeeDeductionTransactionAppService>();
            _SalarySheetListingAppService = IocManager.Instance.Resolve<ISalarySheetListingAppService>();
            _SalarySheetSummaryAppService = IocManager.Instance.Resolve<ISalarySheetSummaryAppService>();



            _CashReceiptReportAppService = IocManager.Instance.Resolve<ICashReceiptReportAppService>();
            _generalLedgerReportAppService = IocManager.Instance.Resolve<GeneralLedgerReportAppService>();
            _custAnalysisReportAppService = IocManager.Instance.Resolve<CustAnalysisReportAppService>();
            _subledgerReportAppService = IocManager.Instance.Resolve<SubledgerReportAppService>();
            _CashBookReportAppService = IocManager.Instance.Resolve<CashBookReportAppService>();
            _itemQuantitativeStockAppService = IocManager.Instance.Resolve<IItemQuantitativeStockAppService>();
            _trialBalanceAppService = IocManager.Instance.Resolve<ITrialBalanceAppService>();
            _apTransactionListingReportsAppService = IocManager.Instance.Resolve<IAPTransactionListingReportsAppService>();
            _stockAssemblyAppService = IocManager.Instance.Resolve<IStockAssemblyAppService>();
            _stockTransferAppService = IocManager.Instance.Resolve<IStockTransferAppService>();
            _gatePassReportAppService = IocManager.Instance.Resolve<IGatePassReportAppService>();

            _requisitionReportAppService = IocManager.Instance.Resolve<IRequisitionReportAppService>();
            _requisitionStatusReportAppService = IocManager.Instance.Resolve<IRequisitionStatusReportAppService>();

            _bankReconcileReportAppService = IocManager.Instance.Resolve<IBankReconcileReportAppService>();
            _bankReconcilationReportAppService = IocManager.Instance.Resolve<IBankReconcilationReportAppService>();



            _purchaseOrderReportAppService = IocManager.Instance.Resolve<IPurchaseOrderReportAppService>();

            _purchaseOrderStatusReportAppService = IocManager.Instance.Resolve<IPurchaseOrderStatusReportAppService>();

            _monthlyConsolidatedReportAppService = IocManager.Instance.Resolve<IMonthlyConsolidatedReportAppService>();

            _receiptReportAppService = IocManager.Instance.Resolve<IReceiptReportAppService>();
            _receiptReturnReportAppService = IocManager.Instance.Resolve<IReceiptReturnReportAppService>();
            _transferRegisterAppService = IocManager.Instance.Resolve<ITransferRegisterAppService>();
            _stockAssemblyRegisterAppService = IocManager.Instance.Resolve<IStockAssemblyRegisterAppService>();

            _gatePassRegisterAppService = IocManager.Instance.Resolve<IGatePassRegisterAppService>();
            _invoiceAppService = IocManager.Instance.Resolve<IInvoiceAppService>();
            _StockReportAppService = IocManager.Instance.Resolve<StockReportAppService>();
            _consumptionReportDepartmentWiseAppService = IocManager.Instance.Resolve<IConsumptionReportDepartmentWiseAppService>();

            _consumptionSummaryDepartmentWiseAppService = IocManager.Instance.Resolve<IConsumptionSummaryDepartmentWiseAppService>();
            _actualAndBudgetAppService = IocManager.Instance.Resolve<IActualAndBudgetAppService>();
            _LCExpensesReportAppService = IocManager.Instance.Resolve<ILCExpensesReportAppService>();
            _cutomerAgingAppService = IocManager.Instance.Resolve<ICustomerAgingReportAppService>();
            _AlertLogReportAppService = IocManager.Instance.Resolve<IAlertLogReportAppService>();
            _ProfitAndLossStatment = IocManager.Instance.Resolve<ProfitAndLossStatment>();
            _TaxCertificateAppService = IocManager.Instance.Resolve<ITaxCertificateAppService>();
        }

        public static List<ReportDataForParams> GetCompanyInfo()
        {
            return _CompanyProfilesAppService.GetReportDataForParams();
        }

        public static List<AuditLog> getauditLogMaster()
        {
            var auditLog = _auditLogRepository.GetAll().ToList();
            return auditLog;
        }
        public static List<ProfitAndLossStatmentDto> GetCashFlowStatementReport(DateTime FromDate, DateTime ToDate)
        {
            return _ProfitAndLossStatment.GetCashFlowStatment(FromDate, ToDate, 0);
        }
        public static List<SubLedgerTrial> GetsubLedgerTrials(DateTime FromDate, DateTime ToDate, string FromAccountID, string ToAccountID, int? FromSubAccID, int? ToSubAccID, int? SLType, string Status, int? curRate)
        {
            return _subLedgerTrailAppService.GetAll(FromDate, ToDate, FromAccountID, ToAccountID, FromSubAccID, ToSubAccID, SLType, null, Status, curRate);
        }

        public static List<QuotationReport> GetQuotationReportData(int? tenantId, int fromDoc, int toDoc)
        {
            return _invoiceAppService.GetQutationData(tenantId, fromDoc, toDoc);
        }
        public static List<CostSheetReport> GetCostSheetReportData(int? tenantId, int fromDoc, int toDoc)
        {
            return _invoiceAppService.GetCostsheetData(tenantId, fromDoc, toDoc);
        }
        public static List<ItemListingDto> GetItemListings(string itemtype)
        {

            return _ItemListingAppService.GetData(itemtype);
        }

        public static List<DeleteLogDto> GetDeleteLogListing(int? TenantId, string FormName)
        {

            return _ItemListingAppService.GetDeleteLog(TenantId, FormName);
        }
        public static List<AssetRegListingDto> GetAssetRegListings(int? TenantID)
        {

            return _assetRegListingAppService.GetData(TenantID);
        }
        public static List<AssetRegistrationReportDto> GetAssetRegReport(int? TenantID)
        {

            return _assetRegReportAppService.GetData(TenantID);
        }

        public static List<ItemLedgerdetail> GetItemLedgerdetail(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            return _itemLedgerDetailAppService.GetData(tenantId, fromLocId, toLocId, fromItem, toItem, fromDate, toDate);
        }
        public static List<Assetdetail> GetAssetListingdetail(int tenantId, string fromLocId, string toLocId, string fromDate, string toDate)
        {
            
            return _AssetListingAppService.GetData(tenantId, fromLocId, toLocId,fromDate, toDate);
        }
        public static List<itemStatus> GetitemStatusListing(string fromDate, string toDate, string fromLocId, string toLocId,string fromitem,string toitem,string fromseg,string toseg )
        {

            return _AssetListingAppService.GetItemData(fromDate, toDate, fromLocId, toLocId, fromitem, toitem,fromseg,toseg);
        }
        public static List<ItemStockSegment2> GetItemStockSegment2Rpt(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            return _stockReportSegment2AppService.GetData(tenantId, fromLocId, toLocId, fromItem, toItem, fromDate, toDate);
        }
        public static List<ItemStockSegment2> GetItemStockSegment3Rpt(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            return _stockReportSegment2AppService.GetDataSeg3(tenantId, fromLocId, toLocId, fromItem, toItem, fromDate, toDate);
        }
        public static List<ItemStockSegment2> GetItemStockRpt(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            return _StockReportAppService.GetData(tenantId, fromLocId, toLocId, fromItem, toItem, fromDate, toDate);
        }

        public static List<AdjustmentReportDto> GetAdjustmentData(DateTime fromDate, DateTime toDate, int fromDoc, int toDoc, string fromLoc, string toLoc)
        {
            return _adjustmentReportAppService.GetAdjustmentData(fromDate, toDate, fromDoc, toDoc, fromLoc, toLoc);
        }

        public static List<ConsumptionReportDto> GetConsumptionData(DateTime fromDate, DateTime toDate, int fromDoc, string fromItem, string toItem, int toDoc, string fromLoc, string toLoc, string ccId)
        {
            return _consumptionReportAppService.GetConsumptionData(fromDate, toDate, fromDoc, toDoc, fromLoc, toLoc, ccId,fromItem,toItem);
        }


        public static List<OpeningReportDto> GetOpeningData(DateTime fromDate, DateTime toDate, int fromDoc, int toDoc, string fromLoc, string toLoc)
        {
            return _openingReportAppService.GetOpeningData(fromDate, toDate, fromDoc, toDoc, fromLoc, toLoc);
        }

        public static List<ChartOfAccountListDto> GetChartOfAccountsData(int slType, string fromAccount, string toAccount)
        {
            return _chartOfACListingReportAppService.GetChartOfAccountsData(slType, fromAccount, toAccount);
        }

        public static List<SubledgerListDto> GetSubLedgerData(int slType)
        {
            return _chartOfACListingReportAppService.GetSubLedgerData(slType);
        }
        public static List<AgeingInvoiceListingDto> GetAgeingInvoiceListing(int? TenantId, string fromDate, string fromAcc, int FromsubAcc, int TosubAcc)
        {
            return _ItemListingAppService.GetAgeingInvoice(TenantId, fromDate, fromAcc, FromsubAcc, TosubAcc);
        }

        public static List<GeneralLedgerDto> GetGeneralLedger(DateTime fromDate, DateTime toDate, string fromAC, string toAC, string status, int fromLocId, int toLocId, int? curRate)
        {
            return _generalLedgerReportAppService.GetGeneralLedger(fromDate, toDate, fromAC, toAC, status, fromLocId, toLocId, curRate);
        }

        public static List<GeneralLedgerDto> GetCustAnalysis(DateTime fromDate, DateTime toDate, string status, int slType, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, int? curRate, int loc)
        {
            return _custAnalysisReportAppService.GetCustAnalysisReport(fromDate, toDate, status, slType, fromAcc, toAcc, fromSubAcc, toSubAcc ,curRate, loc);
        }


        public static List<SubedgerDto> GetSubledger(string text,DateTime fromDate, DateTime toDate, string fromAC, string toAC, int fromSubledgerId, int toSubledgerId, int locId, int slType, string status, int? curRate)
        {
            return _subledgerReportAppService.GetSubledger(text,fromDate, toDate, fromAC, toAC, fromSubledgerId, toSubledgerId, locId, slType, status, curRate);
        }

        public static List<ItemsPriceListDto> GetItemsPriceListing(int? TenantID, string priceList, string fromItem, string toItem)
        {

            return _itemsPriceAppService.GetData(TenantID, priceList, fromItem, toItem);
        }

        public static List<DepartmentListingDto> GetDepartmentListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _DepartmentListingAppService.GetData(TenantID, fromCode, toCode, description);
        }
        public static List<DesignationListingDto> GetDesignationListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _DesignationListingAppService.GetData(TenantID, fromCode, toCode, description);
        }
        public static List<EducationListingDto> GetEducationListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _EducationListingAppService.GetData(TenantID, fromCode, toCode, description);
        }
        public static List<LocationListingDto> GetLocationListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _LocationListingAppService.GetData(TenantID, fromCode, toCode, description);
        }
        public static List<ReligionListingDto> GetReligionListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _ReligionListingAppService.GetData(TenantID, fromCode, toCode, description);
        }
        public static List<EmployeeTypeListingDto> GetEmployeeTypeListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _EmployeeTypeListingAppService.GetData(TenantID, fromCode, toCode, description);
        }

        public static List<ShiftListingDto> GetShiftListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _ShiftListingAppService.GetData(TenantID, fromCode, toCode, description);
        }

        public static List<SectionListingDto> GetSectionListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _SectionListingAppService.GetData(TenantID, fromCode, toCode, description);
        }

        public static List<GradeListingDto> GetGradeListings(int? TenantID, string fromCode, string toCode, string description)
        {

            return _GradeListingAppService.GetData(TenantID, fromCode, toCode, description);
        }

        public static List<EmployeeArrearsTransactionDto> GetEmployeeArrearsTransaction(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear)
        {
            return _EmployeeArrearsTransactionAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, SalaryMonth, SalaryYear);
        }
        public static List<EmployeeEarningTransactionDto> GetEmployeeEarningTransaction(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear)
        {
            return _EmployeeEarningTransactionAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, SalaryMonth, SalaryYear);
        }

        public static List<EmployeeDeductionTransactionDto> GetEmployeeDeductionTransaction(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear)
        {
            return _EmployeeDeductionTransactionAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, SalaryMonth, SalaryYear);
        }
        public static List<EmployeeListingDto> GetEmployeeListing(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string fromSalary, string toSalary, bool isActive,string fromlocid,string tolocid,string emptype)
        {

            return _EmployeeListingAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, isActive, fromlocid, tolocid, emptype);
        }
        public static List<AllowanceEmployeeListingDto> GetEmployeeAllowanceListing(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string AllowanceYear, string allowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype, string EmpType,string allowanceBtype)
        {

            return _EmployeeListingAppService.GetDataForAllowance(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromlocid, tolocid, Allowtype, EmpType, allowanceBtype);
        }
        public static List<SalarySheetSummaryDto> GetEmployeeAllowanceDisturb(int? TenantID, string fromEmpID, string toEmpID,
    string fromDeptID, string toDeptID, string fromSecID, string toSecID, string AllowanceYear, string allowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype, string EmpType, string allowanceBtype)
        {

            return _EmployeeListingAppService.GetDataForAllowanceDisburseMent(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromlocid, tolocid, Allowtype, EmpType, allowanceBtype);
        }
        public static List<AllowanceSummaryDto> GetEmployeeAllowanceSummary(int? TenantID, string fromEmpID, string toEmpID,
    string fromDeptID, string toDeptID, string fromSecID, string toSecID, string AllowanceYear, string allowanceMonth, bool isActive, string fromlocid, string tolocid, string Allowtype, string EmpType, string allowanceBtype)
        {

            return _EmployeeListingAppService.GetDataForAllowanceSummary(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromlocid, tolocid, Allowtype, EmpType, allowanceBtype);
        }
        public static List<EmployeeCardListingDto> GetEmployeeCardListing(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string fromSalary, string toSalary)
        {

            return _EmployeeCardListingAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary);
        }
        public static List<SalarySheetListingDto> GetSalarySheetListing(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear,string fromLocId,string ToLocId,string Emptype)
        {
            return _SalarySheetListingAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, SalaryMonth, SalaryYear, fromLocId, ToLocId, Emptype);
        }
        public static List<SalarySheetSummaryDto> GetSalarySheetSummaryListing(int? TenantID, short SalaryMonth, short SalaryYear)
        {
            return _SalarySheetSummaryAppService.GetData(TenantID, SalaryMonth, SalaryYear);
        }

        public static List<SalarySlipDto> GetSalarySlips(int? TenantID, string fromEmpID, string toEmpID,
            string fromDeptID, string toDeptID, string fromSecID, string toSecID, string fromSalary, string toSalary, short SalaryMonth, short SalaryYear)
        {
            return _SalarySlipAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, SalaryMonth, SalaryYear);
        }

        public static List<AttendanceTransactionDto> GetAttendanceTransaction(int? TenantID, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short AttendanceYear, short AttendanceMonth)
        {
            return _AttendanceTransactionAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, AttendanceYear, AttendanceMonth);
        }
        public static List<AttendanceSummaryDto> GetAttendanceSummary(int? TenantID, string fromEmpID, string toEmpID, string fromDeptID, string toDeptID,
            string fromSecID, string toSecID, string fromSalary, string toSalary, short AttendanceYear, short AttendanceMonth)
        {
            return _AttendanceSummaryAppService.GetData(TenantID, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, AttendanceYear, AttendanceMonth);
        }
        public static List<BankAdviceDto> GetBankAdvice(int? TenantID, short SalaryMonth, short SalaryYear, int typeID)
        {
            return _BankAdviceAppService.GetData(TenantID, SalaryMonth, SalaryYear, typeID);
        }

        public static List<LedgerTypeListingDto> GetLedgerTypeListings(int? TenantID, string fromCode, string toCode)
        {
            return _LedgerTypeListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLLocationListingDto> GetGLLocationListings(int? TenantID, string fromCode, string toCode)
        {
            return _GLLocationListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLCategoriesListingDto> GetGLCategoriesListings(int? TenantID, string fromCode, string toCode)
        {
            return _GLCategoriesListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLGroupListingDto> GetGLGroupListings(int? TenantID, string fromCode, string toCode)
        {
            return _GLGroupListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLLevel1ListingDto> GetGLLevel1Listings(int? TenantID, string fromCode, string toCode)
        {
            return _GLLevel1ListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLLevel2ListingDto> GetGLLevel2Listings(int? TenantID, string fromCode, string toCode)
        {
            return _GLLevel2ListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLLevel3ListingDto> GetGLLevel3Listings(int? TenantID, string fromCode, string toCode)
        {
            return _GLLevel3ListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLBooksListingDto> GetGLBooksListings(int? TenantID, string fromCode, string toCode)
        {
            return _GLBooksListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<GLConfigListingDto> GetGLConfigListings(int? TenantID, string fromCode, string toCode)
        {
            return _GLConfigListingAppService.GetData(TenantID, fromCode, toCode);
        }
        public static List<PostDatedChequeDto> GetPostDatedCheques(int? TenantID, string fromCode, string toCode, string fromDate, string toDate, int typeID)
        {
            return _PostDatedChequeAppService.GetData(TenantID, fromCode, toCode, fromDate, toDate, typeID);
        }
        public static List<CPRNumbersReportDto> GetCPRNumbers(int? TenantID, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string taxClass)
        {
            return _CPRNumbersAppService.GetData(TenantID, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass);
        }
        public static List<SalesTaxDeductionReportDto> GetSalesTaxDeduction(int? TenantID, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string taxClass)
        {
            return _SalesTaxDeductionAppService.GetData(TenantID, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass);
        }
        public static List<PartyTaxReportDto> GetPartyTax(int? TenantID, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string fromTaxClass, string toTaxClass)
        {
            return _PartyTaxAppService.GetData(TenantID, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, fromTaxClass, toTaxClass);
        }
        public static List<SalesTaxWithheldReportDto> GetSalesTaxWithheld(int? TenantId, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string taxClass, string type)
        {
            return _SalesTaxWithheldReportAppService.GetData(TenantId, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass, type);
        }
        public static List<TaxDueReportDto> GetTaxDue(int? TenantId, string fromDate, string toDate, string fromAcc, string toAcc, string fromSubAcc, string toSubAcc, string taxAuth, string taxClass, string type)
        {
            return _TaxDueReportAppService.GetData(TenantId, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass, type);
        }
        public static List<CashReceiptModel> CashReceipt(string bookId, int? year, int? month, int fromConfigId, int toConfigId, int fromDoc, int toDoc, int locId, string curId, double curRate, string status)
        {
            return _CashReceiptReportAppService.GetCashReceipt(null, bookId, year, month, fromConfigId, toConfigId, fromDoc, toDoc, locId, curId, curRate, status);
        }
        public static double? GetDebitSum(int tenantId, int docNo, string bookId, int docMonth)
        {
            return _CashReceiptReportAppService.GetDebitSum(tenantId, docNo, bookId, docMonth);
        }
        public static GLOption GetSingnatures(int tenantId)
        {
            return _CashReceiptReportAppService.GetSignatures(tenantId);
        }

        public static List<GeneralLedgerDto> getCashBook(DateTime fromDate, DateTime toDate, string fromAC, string toAC, string status, int locId, int? curRate, bool CashBook)
        {

            return _CashBookReportAppService.GetCashBook(fromDate, toDate, fromAC, toAC, status, locId, curRate, CashBook);
        }

        public static List<ItemQuantitative> GetItemQuantitativeStock(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem)
        {

            return _itemQuantitativeStockAppService.GetData(0, fromLocId, toLocId, fromItem, toItem);
        }
        public static List<ItemQuantitative> GetItemQuantitativeStock3(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem)
        {

            return _itemQuantitativeStockAppService.GetData3(0, fromLocId, toLocId, fromItem, toItem);
        }

        public static List<TrialBalanceDto> GetTrialBalance(int TenantId, string FromDate, string ToDate, string FromAcc, string ToAcc, int status, int locId, bool includeZeroBalance, string curRate)
        {
            return _trialBalanceAppService.GetData(TenantId, FromDate, ToDate, FromAcc, ToAcc, status, locId, includeZeroBalance, curRate);
        }

        public static List<TransactionListDto> GetTransListing(DateTime FromDate, DateTime ToDate, string Book, string User, int tenantId, string status, int locId)
        {
            return _apTransactionListingReportsAppService.GetData(FromDate, ToDate, Book, User, 0, status, locId);
        }
        public static List<AssemblyStock> GetStockAssembly(int tenantId, string fromLocId, string toLocId,
            string fromItem, string toItem, string fromDate, string toDate)
        {
            return _stockAssemblyAppService.GetData(0, fromLocId, toLocId, fromItem, toItem, fromDate, toDate);
        }
        public static List<AssemblyStockCost> GetStockAssemblyForCost(int tenantId, string orderNo)
        {
            return _stockAssemblyAppService.GetDataForCost(0, orderNo);
        }

        public static List<StockTransfer> GetStockTransfer(int tenantId, int fromDoc, int toDoc)
        {
            return _stockTransferAppService.GetData(0, fromDoc, toDoc);
        }

        public static List<GatePassReport> GetGatePass(int tenantId, int fromDoc, int toDoc, int typeId)
        {
            return _gatePassReportAppService.GetData(0, fromDoc, toDoc, typeId);
        }

        public static List<RequisitionReport> GetRequisitionReport(int tenantId, int fromDoc, int toDoc, int typeId)
        {
            return _requisitionReportAppService.GetData(0, fromDoc, toDoc, 0);
        }
        public static List<RequisitionStatusReport> GetRequisitionStatusReport(int tenantId, int fromDoc, int toDoc, int typeId)
        {
            return _requisitionStatusReportAppService.GetData(0, fromDoc, toDoc, 0);
        }

        public static List<BankReconcileReportDto> GetBankReconcileReport(int? TenantId, string bankID, string fromDocID, string toDocID)
        {
            return _bankReconcileReportAppService.GetData(TenantId, bankID, fromDocID, toDocID);
        }

        public static List<BankReconcilationReportDetailDto> GetBankReconcilationDetailReport(int? TenantId, string bankID, DateTime fromDate, DateTime toDate)
        {
            return _bankReconcilationReportAppService.GetDetailData(TenantId, bankID, fromDate, toDate);
        }

        public static List<BankReconcilationReportDto> GetBankReconcilationReport(int? TenantId, string bankID, DateTime fromDate, DateTime toDate)
        {
            return _bankReconcilationReportAppService.GetData(TenantId, bankID, fromDate, toDate);
        }

        public static List<PurchaseOrderReport> GetPurchaseOrderReport(int tenantId, int fromDoc, int toDoc, int typeId,string exfromdate,string extodate)
        {
            return _purchaseOrderReportAppService.GetData(0, fromDoc, toDoc, 0, exfromdate, extodate);
        }

        public static List<PurchaseOrderStatus> GetPurchaseOrderStatusReport(int tenantId, int fromDoc, int toDoc, string fromDate, string toDate, string fromArDate, string toArDate)
        {
            return _purchaseOrderStatusReportAppService.GetData(0, fromDoc, toDoc, fromDate, toDate, fromArDate, toArDate);
        }


        public static List<MonthlyConsolidatedRpt> GetMonthlyConsolidatedReportData(DateTime fromDate, DateTime toDate, string fromAccount, string toAccount, string status, int fromLocId, int toLocId, int curRate)
        {
            return _monthlyConsolidatedReportAppService.GetConsolidatedLedgers(fromDate, toDate, fromAccount, toAccount, status,fromLocId,toLocId, curRate);
        }
        public static List<ReceiptReport> GetReceiptReportData(int tenantId, int fromDoc, int toDoc, int typeId)
        {
            return _receiptReportAppService.GetData(0, fromDoc, toDoc, 0);
        }
        public static List<ReceiptReturnReport> GetReceiptReturnReportData(int tenantId, int fromDoc, int toDoc, int typeId)
        {
            return _receiptReturnReportAppService.GetData(0, fromDoc, toDoc, 0);
        }

        public static List<TransferRegister> GetTransferRegisterData(int tenantId, int fromDoc, int toDoc, DateTime fromdate, DateTime toDate, string fromLoc, string toLoc)
        {
            return _transferRegisterAppService.GetData(0, fromDoc, toDoc, fromdate, toDate, fromLoc, toLoc);
        }
        public static List<AssemblyStockRegister> GetAssemblyRegisterData(int tenantId, int fromDoc, int toDoc, string fromdate, string toDate)
        {
            return _stockAssemblyRegisterAppService.GetData(0, fromdate, toDate, fromDoc, toDoc);
        }
        public static List<GatePassRegister> GetInwardGatePassRegisterData(int tenantId, int fromDoc, int toDoc)
        {
            return _gatePassRegisterAppService.GetData(0, fromDoc, toDoc, 1);
        }
        public static List<GatePassRegister> GetOutwardGatePassRegisterData(int tenantId, int fromDoc, int toDoc)
        {
            return _gatePassRegisterAppService.GetData(0, fromDoc, toDoc, 2);
        }

        public static List<InvoiceReport> GetInvoiceReportData(int? tenantId, int fromDoc, int toDoc, DateTime fromDate, DateTime toDate)
        {
            return _invoiceAppService.GetData(tenantId, fromDoc, toDoc, fromDate, toDate);
        }

        public static List<ConsumptionSummaryDepartmentWise> GetConsumptionSummaryDepartmentWiseReportData(int? tenantId, string fromLocId, string toLocId)
        {
            return _consumptionSummaryDepartmentWiseAppService.GetData(tenantId, Convert.ToInt32(fromLocId), Convert.ToInt32(toLocId));
        }

        public static List<ConsumptionDepartmentWise> GetConsumptionDepartmentWiseData(int? tenantId, string fromLocId, string toLocId, string fromDate
            , string toDate, string fromItem, string toItem, string reportName)
        {
            return _consumptionReportDepartmentWiseAppService.GetData(tenantId,fromLocId, toLocId, fromDate, toDate, fromItem, toItem, reportName);
        }
        public static List<ActualAndBudget> GetActualAndBudgetData(int? tenantId, string fromDoc, string toDoc)
        {
            return _actualAndBudgetAppService.GetData(tenantId, fromDoc, toDoc);
        }


        public static List<LCExpensesReportDto> GetLCExpensesData(int? tenantId, string fromDate, string toDate, string fromCode, string toCode)
        {
            return _LCExpensesReportAppService.GetData(tenantId, fromDate, toDate, fromCode, toCode);
        }

        public static List<CSAlertLogDto> GetAlerLogData()
        {
            return _AlertLogReportAppService.GetAll();
        }
        public static List<PDCSubReport> GetPDCSubReport(int subLedgerCode, int tenantId, string accId)
        {
            return _pdcSubReportAppService.GetPDCSubReport(subLedgerCode, tenantId, accId);
        }

        public static List<CustomerAgingDto> GetCustomerAgingReport(DateTime asonDate, string fromAccount, string ToAccount, int fromSub, int toSub, string agingPer1, string agingPer2, string agingPer3, string agingPer4, string agingPer5)
        {
            CustomerAgingInputs customerAgingInputs = new CustomerAgingInputs();

            customerAgingInputs.asOnDate = asonDate;
            customerAgingInputs.fromAccount = fromAccount;
            customerAgingInputs.toAccount = ToAccount;
            customerAgingInputs.frmSubAcc = fromSub;
            customerAgingInputs.toSubAcc = toSub;
            customerAgingInputs.agingPeriod1 = agingPer1;
            customerAgingInputs.agingPeriod2 = agingPer2;
            customerAgingInputs.agingPeriod3 = agingPer3;
            customerAgingInputs.agingPeriod4 = agingPer4;
            customerAgingInputs.agingPeriod5 = agingPer5;

            return _cutomerAgingAppService.GetAll(customerAgingInputs);
        }
        public static List<CustomerAgingDto> GetBillWiseAgingReport(DateTime asonDate, string fromAccount, string ToAccount, int fromSub, int toSub, string agingPer1, string agingPer2, string agingPer3, string agingPer4, string agingPer5)
        {
            CustomerAgingInputs customerAgingInputs = new CustomerAgingInputs();

            customerAgingInputs.asOnDate = asonDate;
            customerAgingInputs.fromAccount = fromAccount;
            customerAgingInputs.toAccount = ToAccount;
            customerAgingInputs.frmSubAcc = fromSub;
            customerAgingInputs.toSubAcc = toSub;
            customerAgingInputs.agingPeriod1 = agingPer1;
            customerAgingInputs.agingPeriod2 = agingPer2;
            customerAgingInputs.agingPeriod3 = agingPer3;
            customerAgingInputs.agingPeriod4 = agingPer4;
            customerAgingInputs.agingPeriod5 = agingPer5;

            // return _cutomerAgingAppService.GetAll(customerAgingInputs);
            return _billWiseAgingReportAppService.GetAll(customerAgingInputs);
        }
        public static List<GetReport> GetPlStatementCategoryDetail(string fromDate, string toDate, string headingText)
        {
            return _plStatementCategoryDetail.GetPlStatementCategoryDetail(fromDate, toDate, headingText);
        }

        public static List<ProfitAndLossStatmentDto> GetProfitAndLossReport(DateTime FromDate, DateTime ToDate)
        {
            return _ProfitAndLossStatment.GetProfitAndLossStatment(FromDate, ToDate, 0);
        }

        public static List<ProfitAndLossStatmentDto> GetBalanceSheet(DateTime FromDate, DateTime ToDate)
        {
            return _balanceSheetAppService.GetBalanceSheet(FromDate, ToDate, 0);
        }

        public static List<PlStatementVoucherDetailReport> GetPlStatementVoucherDetailReport(string accId, string fromDate, string toDate)
        {
            return _plStatementVoucherDetail.GetPlStatementVoucherDetailReport(accId, fromDate, toDate);
        }

        public static List<AuditPostingReport> GetAuditPostingLogsReport(string user, DateTime fromDate, DateTime toDate, string voucher)
        {
            return _auditPostingLogsReportAppService.Getreport(user, fromDate, toDate, voucher);
        }

        public static List<EmployeeLeavesLedger> GetEmployeeLeavesReport(int salaryYear, int frmEmpId, int toEmpId)
        {
            return _employeeLeavesAppService.GetEmployeeLeaves(salaryYear, frmEmpId, toEmpId);
        }

        public static List<EmployeeLoanLedger> GetEmployeeLoanLedgerReport(int? fromEmpID, int? toEmpId, int? loanTypeId)
        {
            return _employeeLoanLedgerAppService.GeEmployeeLoanLedger(fromEmpID, toEmpId, loanTypeId);
        }

        public static List<TaxCertificate> GetTaxCertificateReport(int? TenantID, string fromEmpID,short SalaryYear, short SalaryMonth,short toSalaryYear, short toSalaryMonth)
        {
            return _TaxCertificateAppService.GetData(TenantID, fromEmpID, SalaryYear, SalaryMonth, toSalaryYear, toSalaryMonth);
        }
    }
}
