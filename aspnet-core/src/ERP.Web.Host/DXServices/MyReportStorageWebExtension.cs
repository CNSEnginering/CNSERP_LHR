
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.DataHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace ERP.Web.DXServices
{
    /// <summary>
    /// Added By Waleed Khalid
    /// </summary>
    public class MyReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {

        List<string> paramHolder = new List<string>();
        string CompanyName;
        string Address;
        string Phone;
        string salesTaxReg;
        string tenantId;
        string Address2;
        string UserName;
        string Inventorypoint;
        string Financepoint;

        XtraReport xtraReport = new XtraReport();

        public override bool IsValidUrl(string url)
        {
            // Determines whether or not the URL passed to the current Report Storage is valid. 
            // For instance, implement your own logic to prohibit URLs that contain white spaces or some other special characters. 
            // This method is called before the CanSetData and GetData methods.

            //All URLs will be valid as not the same name as index.
            return true;
        }

        public override byte[] GetData(string url)
        {

            //get report name from url;

            string repoertName = url.Split('?')[0];
            // initialize parameter for every request
            initParameters(url);
            initCompanyInfo();


            using (MemoryStream ms = new MemoryStream())
            {

                //int idx = url.IndexOf('?');
                //string query = idx >= 0 ? url.Substring(idx) : "";
                //string[] myurl = url.Split('?');
                //url = myurl[0];


                string[] myurl = url.Split('?');
                url = myurl[0];

                


                switch (repoertName)
                {
                    case "AUDITLOG":
                        XtraReport report = new Reports.AuditLogRpt();

                        string repJsonP = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.getauditLogMaster());

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)report.DataSource).JsonSource).Json = repJsonP;
                        report.DataMember = null;

                        report.SaveLayoutToXml(ms);
                        break;
                    case "SUBLEDGER":
                        initPointInfo();
                        DateTime FromDate = DateTime.Parse(paramHolder[0]);
                        DateTime ToDate = DateTime.Parse(paramHolder[1]);
                        string FromAccount = paramHolder[2];
                        string ToAccount = paramHolder[3];
                        int FromsubAcc = Convert.ToInt32(paramHolder[4]);
                        int TosubAcc = Convert.ToInt32(paramHolder[5]);
                        // string status = paramHolder[6];
                        //  int curRate = Convert.ToInt32(paramHolder[9]);
                        int? sltype;

                        if (paramHolder[6] == "0")
                            sltype = null;
                        else
                            sltype = Convert.ToInt32(paramHolder[6]);

                        XtraReport SUBLEDGER = new Reports.rptSubLedgerTrail();
                        SUBLEDGER.Parameters["CompanyName"].Value = CompanyName;
                        SUBLEDGER.Parameters["Address"].Value = Address;
                        SUBLEDGER.Parameters["Phone"].Value = Phone;
                        SUBLEDGER.Parameters["TenantId"].Value = tenantId;
                        SUBLEDGER.Parameters["FinancePoint"].Value = Financepoint;
                        string subJsonP = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetsubLedgerTrials(FromDate, ToDate, FromAccount, ToAccount, FromsubAcc, TosubAcc, sltype, "Approved", null));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)SUBLEDGER.DataSource).JsonSource).Json = subJsonP;
                        SUBLEDGER.DataMember = null;

                        SUBLEDGER.SaveLayoutToXml(ms);
                        break;
                 case "cashflowstatement":
                        XtraReport rptCFStatment = new Reports.Finance.CashFlowStatement();

                        rptCFStatment.Parameters["CompanyName"].Value = CompanyName;
                        rptCFStatment.Parameters["TenantId"].Value = tenantId;
                        rptCFStatment.Parameters["FromDate"].Value = paramHolder[0];
                        rptCFStatment.Parameters["ToDate"].Value = paramHolder[1];


                        string CFStatmentJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetCashFlowStatementReport(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1])));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptCFStatment.DataSource).JsonSource).Json = CFStatmentJson;
                        rptCFStatment.SaveLayoutToXml(ms);

                        break;
                    case "ItemListing":
                        initPointInfo();
                        XtraReport itemlisting = new Reports.Inventory.ItemListing();
                        itemlisting.Parameters["CompanyName"].Value = CompanyName;
                        itemlisting.Parameters["Address"].Value = Address;
                        itemlisting.Parameters["Phone"].Value = Phone;
                        itemlisting.Parameters["TenantId"].Value = tenantId;
                        itemlisting.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string itemlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemListings(paramHolder[0]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemlisting.DataSource).JsonSource).Json = itemlistjson;
                        itemlisting.SaveLayoutToXml(ms);
                        break;
                    case "SaleQuotation":
                        XtraReport QuotationRegister = new Reports.Sales.SaleQuotation();
                        string salesQuotationJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetQuotationReportData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)QuotationRegister.DataSource).JsonSource).Json = salesQuotationJson;
                        QuotationRegister.Parameters["CompanyName"].Value = CompanyName;
                        QuotationRegister.Parameters["Address"].Value = Address;
                        QuotationRegister.Parameters["Address2"].Value = Address2;
                        QuotationRegister.Parameters["TenantId"].Value = tenantId;
                        QuotationRegister.Parameters["Phone"].Value = Phone;

                        //salesRegister.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        //salesRegister.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        //salesRegister.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                        //salesRegister.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                        QuotationRegister.SaveLayoutToXml(ms);
                        break;
                    case "costsheet":
                        XtraReport CostSheet = new Reports.Sales.CostSheet();
                        string CostSheetJson2 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetCostSheetReportData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)CostSheet.DataSource).JsonSource).Json = CostSheetJson2;
                        CostSheet.Parameters["CompanyName"].Value = CompanyName;
                        CostSheet.Parameters["Address"].Value = Address;
                        CostSheet.Parameters["Address2"].Value = Address2;
                        CostSheet.Parameters["TenantId"].Value = tenantId;
                        CostSheet.Parameters["Phone"].Value = Phone;

                        //salesRegister.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        //salesRegister.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        //salesRegister.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                        //salesRegister.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                        CostSheet.SaveLayoutToXml(ms);
                        break;
                    case "ItemLedgerDetail":
                        ///int tenantId = Convert.ToInt32(paramHolder[0]);
                       initPointInfo();
                        string fromDate = paramHolder[0];
                        string toDate = paramHolder[1];
                        string fromLocId = paramHolder[2];
                        string toLocId = "";
                        string fromItem = paramHolder[3];
                        string toItem = paramHolder[4];
                        string orderBy = paramHolder[5];
                        XtraReport itemledgerDetailRpt = new Reports.Inventory.ItemLedgerDetail();


                        itemledgerDetailRpt.Parameters["CompanyName"].Value = CompanyName;
                        itemledgerDetailRpt.Parameters["FromDate"].Value = fromDate;
                        itemledgerDetailRpt.Parameters["ToDate"].Value = toDate;
                        itemledgerDetailRpt.Parameters["FromItem"].Value = fromItem;
                        itemledgerDetailRpt.Parameters["ToItem"].Value = toItem;
                        itemledgerDetailRpt.Parameters["TenantId"].Value = tenantId;
                        itemledgerDetailRpt.Parameters["Address"].Value = Address;
                        itemledgerDetailRpt.Parameters["Address2"].Value = Address2;
                        itemledgerDetailRpt.Parameters["Phone"].Value = Phone;
                        itemledgerDetailRpt.Parameters["InventoryPoint"].Value = Inventorypoint;

                        string itemledgerDetailJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemLedgerdetail(0, fromLocId, toLocId, fromItem, toItem, fromDate, toDate));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemledgerDetailRpt.DataSource).JsonSource).Json = itemledgerDetailJson;
                        itemledgerDetailRpt.SaveLayoutToXml(ms);
                        break;
                    case "AssetResourceList":
                        ///int tenantId = Convert.ToInt32(paramHolder[0]);
                        fromDate = paramHolder[0];
                         toDate = paramHolder[1];
                         fromLocId = paramHolder[2];
                         toLocId = paramHolder[3];
                        
                        XtraReport AssetListingDetailRpt = new Reports.Inventory.AssetResourceListing();


                        AssetListingDetailRpt.Parameters["CompanyName"].Value = CompanyName;
                        //AssetListingDetailRpt.Parameters["FromDate"].Value = fromDate;
                        //AssetListingDetailRpt.Parameters["ToDate"].Value = toDate;
                        AssetListingDetailRpt.Parameters["TenantId"].Value = tenantId;
                        AssetListingDetailRpt.Parameters["Address"].Value = Address;
                        AssetListingDetailRpt.Parameters["Address1"].Value = Address2;
                        AssetListingDetailRpt.Parameters["Phone"].Value = Phone;

                        string AssetListingDetailJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAssetListingdetail(0, fromLocId, toLocId, fromDate, toDate));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)AssetListingDetailRpt.DataSource).JsonSource).Json = AssetListingDetailJson;
                        AssetListingDetailRpt.SaveLayoutToXml(ms);
                        break;
                    case "itemStatus":
                        ///int tenantId = Convert.ToInt32(paramHolder[0]);
                        fromDate = paramHolder[0];
                        toDate = paramHolder[1];
                        fromLocId = paramHolder[2];
                        toLocId = "";// paramHolder[3];
                        fromItem = paramHolder[3];
                        toItem = paramHolder[4];
                        string fromseg = paramHolder[5];
                        string toseg = paramHolder[6];
                        XtraReport ItemListing = new Reports.Inventory.ItemStatusListing();


                        ItemListing.Parameters["CompanyName"].Value = CompanyName;
                        //AssetListingDetailRpt.Parameters["FromDate"].Value = fromDate;
                        //AssetListingDetailRpt.Parameters["ToDate"].Value = toDate;
                        ItemListing.Parameters["TenantId"].Value = tenantId;
                        ItemListing.Parameters["Address"].Value = Address;
                        ItemListing.Parameters["Address1"].Value = Address2;
                        ItemListing.Parameters["Phone"].Value = Phone;

                        string ItemStatusJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetitemStatusListing(fromDate, toDate,fromLocId, toLocId,fromItem,toItem,fromseg,toseg));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)ItemListing.DataSource).JsonSource).Json = ItemStatusJson;
                        ItemListing.SaveLayoutToXml(ms);
                        break;
                    case "ItemLedgerQuantitative":
                        initPointInfo();
                        XtraReport itemledgerRpt = new Reports.Inventory.ItemLedgerQuantitative();
                        itemledgerRpt.Parameters["CompanyName"].Value = CompanyName;
                        itemledgerRpt.Parameters["FromDate"].Value = paramHolder[0];
                        itemledgerRpt.Parameters["ToDate"].Value = paramHolder[1];
                        itemledgerRpt.Parameters["FromItem"].Value = paramHolder[4];
                        itemledgerRpt.Parameters["ToItem"].Value = paramHolder[5];
                        itemledgerRpt.Parameters["TenantId"].Value = tenantId;
                        itemledgerRpt.Parameters["Address"].Value = Address;
                        itemledgerRpt.Parameters["Address2"].Value = Address2;
                        itemledgerRpt.Parameters["Phone"].Value = Phone;
                        itemledgerRpt.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string itemledgerJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemLedgerdetail(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemledgerRpt.DataSource).JsonSource).Json = itemledgerJson;
                        itemledgerRpt.SaveLayoutToXml(ms);
                        break;
                    case "ItemsPriceList":
                        initPointInfo();
                        string priceList = paramHolder[0];
                        fromItem = paramHolder[1];
                        toItem = paramHolder[2];
                        XtraReport itemsPricelisting = new Reports.Inventory.ItemsPriceList();
                        itemsPricelisting.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string itemsPricelistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemsPriceListing(null, priceList, fromItem, toItem));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemsPricelisting.DataSource).JsonSource).Json = itemsPricelistjson;
                        itemsPricelisting.SaveLayoutToXml(ms);
                        break;

                    case "AssetRegListing":
                        XtraReport assetReglisting = new Reports.Inventory.AssetRegListing();

                        assetReglisting.Parameters["CompanyName"].Value = CompanyName;
                        assetReglisting.Parameters["Address2"].Value = Address2;
                        assetReglisting.Parameters["Address"].Value = Address;
                        assetReglisting.Parameters["Phone"].Value = Phone;
                        assetReglisting.Parameters["TenantId"].Value = tenantId;
                        string assetReglistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAssetRegListings(null));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)assetReglisting.DataSource).JsonSource).Json = assetReglistjson;
                        assetReglisting.SaveLayoutToXml(ms);
                        break;

                    case "AssetRegistrationReport":
                        XtraReport assetRegreport = new Reports.Inventory.AssetRegistraionReport();

                        assetRegreport.Parameters["CompanyName"].Value = CompanyName;
                        assetRegreport.Parameters["Address2"].Value = Address2;
                        assetRegreport.Parameters["Address"].Value = Address;
                        assetRegreport.Parameters["Phone"].Value = Phone;
                        assetRegreport.Parameters["TenantId"].Value = tenantId;

                        string assetRegreportjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAssetRegReport(null));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)assetRegreport.DataSource).JsonSource).Json = assetRegreportjson;
                        assetRegreport.SaveLayoutToXml(ms);
                        break;

                    case "LoanLedger":
                        XtraReport loanLedger = new Reports.PayRoll.LoanLedger();

                        loanLedger.Parameters["CompanyName"].Value = CompanyName;
                        loanLedger.Parameters["Address"].Value = Address;
                        loanLedger.Parameters["Phone"].Value = Phone;
                        loanLedger.Parameters["TenantId"].Value = tenantId;
                        loanLedger.Parameters["Address2"].Value = Address2;


                        string loanLedgerJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeLoanLedgerReport(Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), Convert.ToInt32(paramHolder[2])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)loanLedger.DataSource).JsonSource).Json = loanLedgerJson;
                        loanLedger.SaveLayoutToXml(ms);
                        break;


                    case "Department":
                        string fromCode = paramHolder[0];
                        string toCode = paramHolder[1];
                        string description = paramHolder[2];
                        XtraReport departmentlisting = new Reports.PayRoll.Departments();

                        departmentlisting.Parameters["CompanyName"].Value = CompanyName;
                        departmentlisting.Parameters["Address"].Value = Address;
                        departmentlisting.Parameters["Phone"].Value = Phone;
                        departmentlisting.Parameters["TenantId"].Value = tenantId;
                        departmentlisting.Parameters["Address2"].Value = Address2;


                        string departmentlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetDepartmentListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)departmentlisting.DataSource).JsonSource).Json = departmentlistjson;
                        departmentlisting.SaveLayoutToXml(ms);
                        break;
                    case "Designation":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport designationListing = new Reports.PayRoll.Designation();

                        designationListing.Parameters["CompanyName"].Value = CompanyName;
                        designationListing.Parameters["Address"].Value = Address;
                        designationListing.Parameters["Phone"].Value = Phone;
                        designationListing.Parameters["TenantId"].Value = tenantId;
                        designationListing.Parameters["Address2"].Value = Address2;

                        string designationlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetDesignationListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)designationListing.DataSource).JsonSource).Json = designationlistjson;
                        designationListing.SaveLayoutToXml(ms);
                        break;
                    case "EmployeeLeavesLedger":
                        int salYear = Convert.ToInt32(paramHolder[0]);
                        int frmEmpId = Convert.ToInt32(paramHolder[1]);
                        int toEmpId = Convert.ToInt32(paramHolder[2]);
                        XtraReport employeeLeaves = new Reports.PayRoll.EmployeeLeaves();

                        employeeLeaves.Parameters["CompanyName"].Value = CompanyName;
                        employeeLeaves.Parameters["Address"].Value = Address;
                        employeeLeaves.Parameters["Phone"].Value = Phone;
                        employeeLeaves.Parameters["TenantId"].Value = tenantId;
                        employeeLeaves.Parameters["Address2"].Value = Address2;
                        employeeLeaves.Parameters["FromEmpID"].Value = frmEmpId;
                        employeeLeaves.Parameters["ToEmpID"].Value = toEmpId;
                        employeeLeaves.Parameters["SalaryYear"].Value = salYear;

                        string employeeLeavesjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeLeavesReport(salYear, frmEmpId, toEmpId));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)employeeLeaves.DataSource).JsonSource).Json = employeeLeavesjson;
                        employeeLeaves.SaveLayoutToXml(ms);
                        break;
                    case "Education":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport educationListing = new Reports.PayRoll.Education();

                        educationListing.Parameters["CompanyName"].Value = CompanyName;
                        educationListing.Parameters["Address"].Value = Address;
                        educationListing.Parameters["Phone"].Value = Phone;
                        educationListing.Parameters["TenantId"].Value = tenantId;
                        educationListing.Parameters["Address2"].Value = Address2;

                        string educationlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEducationListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)educationListing.DataSource).JsonSource).Json = educationlistjson;
                        educationListing.SaveLayoutToXml(ms);
                        break;
                    case "Location":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport locationListing = new Reports.PayRoll.Location();

                        locationListing.Parameters["CompanyName"].Value = CompanyName;
                        locationListing.Parameters["Address"].Value = Address;
                        locationListing.Parameters["Phone"].Value = Phone;
                        locationListing.Parameters["TenantId"].Value = tenantId;
                        locationListing.Parameters["Address2"].Value = Address2;

                        string locationlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetLocationListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)locationListing.DataSource).JsonSource).Json = locationlistjson;
                        locationListing.SaveLayoutToXml(ms);
                        break;
                    case "Religion":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport religionListing = new Reports.PayRoll.Religion();

                        religionListing.Parameters["CompanyName"].Value = CompanyName;
                        religionListing.Parameters["Address"].Value = Address;
                        religionListing.Parameters["Phone"].Value = Phone;
                        religionListing.Parameters["TenantId"].Value = tenantId;
                        religionListing.Parameters["Address2"].Value = Address2;

                        string religionlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetReligionListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)religionListing.DataSource).JsonSource).Json = religionlistjson;
                        religionListing.SaveLayoutToXml(ms);
                        break;
                    case "EmployeeType":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport employeeTypeListing = new Reports.PayRoll.EmployeeType();

                        employeeTypeListing.Parameters["CompanyName"].Value = CompanyName;
                        employeeTypeListing.Parameters["Address"].Value = Address;
                        employeeTypeListing.Parameters["Phone"].Value = Phone;
                        employeeTypeListing.Parameters["TenantId"].Value = tenantId;
                        employeeTypeListing.Parameters["Address2"].Value = Address2;

                        string employeeTypelistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeTypeListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)employeeTypeListing.DataSource).JsonSource).Json = employeeTypelistjson;
                        employeeTypeListing.SaveLayoutToXml(ms);
                        break;

                    case "Shift":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport shiftListing = new Reports.PayRoll.Shift();

                        shiftListing.Parameters["CompanyName"].Value = CompanyName;
                        shiftListing.Parameters["Address"].Value = Address;
                        shiftListing.Parameters["Phone"].Value = Phone;
                        shiftListing.Parameters["TenantId"].Value = tenantId;
                        shiftListing.Parameters["Address2"].Value = Address2;

                        string shiftlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetShiftListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)shiftListing.DataSource).JsonSource).Json = shiftlistjson;
                        shiftListing.SaveLayoutToXml(ms);
                        break;

                    case "Section":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport sectionListing = new Reports.PayRoll.Sections();

                        sectionListing.Parameters["CompanyName"].Value = CompanyName;
                        sectionListing.Parameters["Address"].Value = Address;
                        sectionListing.Parameters["Phone"].Value = Phone;
                        sectionListing.Parameters["TenantId"].Value = tenantId;
                        sectionListing.Parameters["Address2"].Value = Address2;

                        string sectionlistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSectionListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)sectionListing.DataSource).JsonSource).Json = sectionlistjson;
                        sectionListing.SaveLayoutToXml(ms);
                        break;

                    case "Grade":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        description = paramHolder[2];
                        XtraReport gradeListing = new Reports.PayRoll.Grade();

                        gradeListing.Parameters["CompanyName"].Value = CompanyName;
                        gradeListing.Parameters["Address"].Value = Address;
                        gradeListing.Parameters["Phone"].Value = Phone;
                        gradeListing.Parameters["TenantId"].Value = tenantId;
                        gradeListing.Parameters["Address2"].Value = Address2;

                        string gradelistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGradeListings(null, fromCode, toCode, description));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)gradeListing.DataSource).JsonSource).Json = gradelistjson;
                        gradeListing.SaveLayoutToXml(ms);
                        break;
                    case "EmployeeArrears":
                        string fromEmpID = paramHolder[0];
                        string toEmpID = paramHolder[1];
                        string fromDeptID = paramHolder[2];
                        string toDeptID = paramHolder[3];
                        string fromSecID = paramHolder[4];
                        string toSecID = paramHolder[5];
                        string fromSalary = paramHolder[6];
                        string toSalary = paramHolder[7];
                        short salaryYear = Convert.ToInt16(paramHolder[8]);
                        short salaryMonth = Convert.ToInt16(paramHolder[9]);
                        XtraReport employeeArrears = new Reports.PayRoll.EmployeeArrears();

                        employeeArrears.Parameters["CompanyName"].Value = CompanyName;
                        employeeArrears.Parameters["Address"].Value = Address;
                        employeeArrears.Parameters["Phone"].Value = Phone;
                        employeeArrears.Parameters["TenantId"].Value = tenantId;
                        employeeArrears.Parameters["Address2"].Value = Address2;

                        string employeearrearsjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeArrearsTransaction(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryYear, salaryMonth));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)employeeArrears.DataSource).JsonSource).Json = employeearrearsjson;
                        employeeArrears.SaveLayoutToXml(ms);
                        break;

                    case "Employees":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        var fromloc = paramHolder[8];
                        var Toloc = paramHolder[9];
                        var EmpType = paramHolder[10];
                        bool isActive = Convert.ToBoolean(paramHolder[11]);
                        XtraReport employeesListing = new Reports.PayRoll.Employee();

                        employeesListing.Parameters["CompanyName"].Value = CompanyName;
                        employeesListing.Parameters["TenantId"].Value = tenantId;
                        employeesListing.Parameters["Address"].Value = Address;
                        employeesListing.Parameters["Address2"].Value = Address2;
                        employeesListing.Parameters["Phone"].Value = Phone;
                        employeesListing.Parameters["EmpRptTitle"].Value = "List of Employees";

                        string employeeslistingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, isActive, fromloc, Toloc, EmpType));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)employeesListing.DataSource).JsonSource).Json = employeeslistingjson;
                        employeesListing.SaveLayoutToXml(ms);
                        break;
                    case "AllowanceType":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        var  AllowanceYear = paramHolder[6];
                        var allowanceMonth = paramHolder[7];
                        fromloc = paramHolder[8];
                        Toloc = paramHolder[9];
                        var AllowType = paramHolder[10];
                        EmpType= paramHolder[11];
                        var allowanceBtype = paramHolder[12];
                        isActive = Convert.ToBoolean(paramHolder[13]);

                        XtraReport AllowanceListing = new XtraReport();
                        string AllowanceListingjson = "";
                        if (allowanceBtype== "AllowanceBank")
                        {
                            AllowanceListing = new Reports.PayRoll.AllowanceBank();
                             AllowanceListingjson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetEmployeeAllowanceListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromloc, Toloc, AllowType, EmpType, allowanceBtype));
                        }
                        else if(allowanceBtype == "Allowance")
                        {
                            if (AllowType == "1")
                            {
                                AllowanceListing = new Reports.PayRoll.Allowance();
                            }
                            else if(AllowType == "2")
                            {
                                AllowanceListing = new Reports.PayRoll.AllowanceBike();
                            }
                             AllowanceListingjson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetEmployeeAllowanceListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromloc, Toloc, AllowType, EmpType, allowanceBtype));
                        } else if(allowanceBtype == "FixedAllowance")
                        {
                            if (AllowType == "1")
                            {
                                AllowanceListing = new Reports.PayRoll.FixedAllowance();
                            }
                            else
                            {
                                AllowanceListing = new Reports.PayRoll.AllowanceBike();
                            }
                             AllowanceListingjson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetEmployeeAllowanceListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromloc, Toloc, AllowType, EmpType, allowanceBtype));
                        }
                        else if(allowanceBtype== "AllowanceDisbursement")
                        {
                            AllowanceListing = new Reports.PayRoll.AllowanceDisbrsment();
                             AllowanceListingjson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetEmployeeAllowanceDisturb(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromloc, Toloc, AllowType, EmpType, allowanceBtype));
                           
                        }
                        else if (allowanceBtype == "AllowanceSummary")
                        {
                            AllowanceListing = new Reports.PayRoll.AllowanceSummary();
                            AllowanceListingjson = JsonConvert.SerializeObject(
                                                  ReportDataHandlerBase.GetEmployeeAllowanceSummary(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, AllowanceYear, allowanceMonth, isActive, fromloc, Toloc, AllowType, EmpType, allowanceBtype));
                        }

                        AllowanceListing.Parameters["TenantId"].Value = tenantId;
                        AllowanceListing.Parameters["CompanyName"].Value = CompanyName;
                        AllowanceListing.Parameters["MonthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(allowanceMonth)) + " " + AllowanceYear; 
                        AllowanceListing.Parameters["Address"].Value = Address;
                        AllowanceListing.Parameters["Address2"].Value = Address2;
                        AllowanceListing.Parameters["Phone"].Value = Phone;
                     


                       
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)AllowanceListing.DataSource).JsonSource).Json = AllowanceListingjson;
                       
                        AllowanceListing.SaveLayoutToXml(ms);
                        break;

                    case "ResignedEmployees":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                         fromloc = paramHolder[8];
                         Toloc = paramHolder[9];
                        EmpType = paramHolder[10];
                        isActive = false;// Convert.ToBoolean(paramHolder[11]);
                        XtraReport resignedEmployeesListing = new Reports.PayRoll.Employee();

                        resignedEmployeesListing.Parameters["CompanyName"].Value = CompanyName;
                        resignedEmployeesListing.Parameters["TenantId"].Value = tenantId;
                        resignedEmployeesListing.Parameters["Address"].Value = Address;
                        resignedEmployeesListing.Parameters["Address2"].Value = Address2;
                        resignedEmployeesListing.Parameters["Phone"].Value = Phone;
                        resignedEmployeesListing.Parameters["EmpRptTitle"].Value = "List of Resigned Employees";

                        string resignedEmployeeslistingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, isActive,fromloc,Toloc,EmpType));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)resignedEmployeesListing.DataSource).JsonSource).Json = resignedEmployeeslistingjson;
                        resignedEmployeesListing.SaveLayoutToXml(ms);
                        break;

                    case "EmployeeEarnings":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        salaryYear = Convert.ToInt16(paramHolder[8]);
                        salaryMonth = Convert.ToInt16(paramHolder[9]);
                        XtraReport employeeEarnings = new Reports.PayRoll.EmployeeEarnings();

                        employeeEarnings.Parameters["CompanyName"].Value = CompanyName;
                        employeeEarnings.Parameters["Address"].Value = Address;
                        employeeEarnings.Parameters["Phone"].Value = Phone;
                        employeeEarnings.Parameters["TenantId"].Value = tenantId;
                        employeeEarnings.Parameters["Address2"].Value = Address2;

                        string employeeearningsjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeEarningTransaction(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryMonth, salaryYear));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)employeeEarnings.DataSource).JsonSource).Json = employeeearningsjson;
                        employeeEarnings.SaveLayoutToXml(ms);
                        break;
                    case "EmployeeCard":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        XtraReport employeeCard = new Reports.PayRoll.EmployeeCard();
                        string employeecardjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeCardListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)employeeCard.DataSource).JsonSource).Json = employeecardjson;
                        employeeCard.SaveLayoutToXml(ms);
                        break;

                    case "EmployeeDeductions":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        salaryYear = Convert.ToInt16(paramHolder[8]);
                        salaryMonth = Convert.ToInt16(paramHolder[9]);
                        XtraReport employeeDeductions = new Reports.PayRoll.EmployeeDeductions();

                        employeeDeductions.Parameters["CompanyName"].Value = CompanyName;
                        employeeDeductions.Parameters["Address"].Value = Address;
                        employeeDeductions.Parameters["Phone"].Value = Phone;
                        employeeDeductions.Parameters["TenantId"].Value = tenantId;
                        employeeDeductions.Parameters["Address2"].Value = Address2;

                        string employeedeductionsjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetEmployeeDeductionTransaction(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryMonth, salaryYear));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)employeeDeductions.DataSource).JsonSource).Json = employeedeductionsjson;
                        employeeDeductions.SaveLayoutToXml(ms);
                        break;

                    case "SalarySheet":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        salaryYear = Convert.ToInt16(paramHolder[8]);
                        salaryMonth = Convert.ToInt16(paramHolder[9]);
                        fromLocId = paramHolder[10];
                        toLocId = paramHolder[11];
                        var Emptype = paramHolder[12];

                        XtraReport salarySheetListing = new Reports.PayRoll.SalarySheet();

                        salarySheetListing.Parameters["CompanyName"].Value = CompanyName;
                        salarySheetListing.Parameters["Address"].Value = Address;
                        salarySheetListing.Parameters["Phone"].Value = Phone;
                        salarySheetListing.Parameters["TenantId"].Value = tenantId;
                        salarySheetListing.Parameters["Address2"].Value = Address2;
                        
                        salarySheetListing.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        string salarySheetListingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalarySheetListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryMonth, salaryYear, fromLocId, toLocId, Emptype));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)salarySheetListing.DataSource).JsonSource).Json = salarySheetListingjson;
                        salarySheetListing.SaveLayoutToXml(ms);
                        break;


                    case "DeductionRegister":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        salaryYear = Convert.ToInt16(paramHolder[8]);
                        salaryMonth = Convert.ToInt16(paramHolder[9]);
                        fromLocId = paramHolder[10];
                        toLocId = paramHolder[11];
                        Emptype = paramHolder[12];

                        XtraReport deductionregisterListing = new Reports.PayRoll.DeductionRegister();

                        deductionregisterListing.Parameters["CompanyName"].Value = CompanyName;
                        deductionregisterListing.Parameters["Address"].Value = Address;
                        deductionregisterListing.Parameters["Phone"].Value = Phone;
                        deductionregisterListing.Parameters["TenantId"].Value = tenantId;
                        deductionregisterListing.Parameters["Address2"].Value = Address2;

                        deductionregisterListing.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        string deductionregisterListingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalarySheetListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryMonth, salaryYear, fromLocId, toLocId, Emptype));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)deductionregisterListing.DataSource).JsonSource).Json = deductionregisterListingjson;
                        deductionregisterListing.SaveLayoutToXml(ms);
                        break;

                    case "AllowanceRegister":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        salaryYear = Convert.ToInt16(paramHolder[8]);
                        salaryMonth = Convert.ToInt16(paramHolder[9]);
                        fromLocId = paramHolder[10];
                        toLocId = paramHolder[11];
                        Emptype = paramHolder[12];

                        XtraReport alloawanceregisterListing = new Reports.PayRoll.AllowanceRegister();

                        alloawanceregisterListing.Parameters["CompanyName"].Value = CompanyName;
                        alloawanceregisterListing.Parameters["Address"].Value = Address;
                        alloawanceregisterListing.Parameters["Phone"].Value = Phone;
                        alloawanceregisterListing.Parameters["TenantId"].Value = tenantId;
                        alloawanceregisterListing.Parameters["Address2"].Value = Address2;

                        alloawanceregisterListing.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        string alloawanceregisterListingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalarySheetListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryMonth, salaryYear, fromLocId, toLocId, Emptype));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)alloawanceregisterListing.DataSource).JsonSource).Json = alloawanceregisterListingjson;
                        alloawanceregisterListing.SaveLayoutToXml(ms);
                        break;




                    case "SalarySheetSummary":
                        //salaryYear = Convert.ToInt16(paramHolder[0]);
                        //salaryMonth = Convert.ToInt16(paramHolder[1]);

                        //XtraReport salarySheetSummaryListing = new Reports.PayRoll.SalarySheetSummary();

                        ////salarySheetSummaryListing.Parameters["CompanyName"].Value = CompanyName;
                        ////salarySheetSummaryListing.Parameters["Address"].Value = Address;
                        ////salarySheetSummaryListing.Parameters["Phone"].Value = Phone;
                        ////salarySheetSummaryListing.Parameters["TenantId"].Value = tenantId;
                        ////salarySheetSummaryListing.Parameters["Address2"].Value = Address2;
                        ////salarySheetSummaryListing.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        //string salarySheetSummaryListingjson = JsonConvert.SerializeObject(
                        //                            ReportDataHandlerBase.GetSalarySheetSummaryListing(null, salaryMonth, salaryYear));
                        //((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)salarySheetSummaryListing.DataSource).JsonSource).Json = salarySheetSummaryListingjson;
                        //salarySheetSummaryListing.SaveLayoutToXml(ms);

                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        salaryYear = Convert.ToInt16(paramHolder[8]);
                        salaryMonth = Convert.ToInt16(paramHolder[9]);
                        fromLocId = "0";
                        toLocId = "9999";
                        Emptype = paramHolder[10];
                        XtraReport salarySheetSummaryListing = new Reports.PayRoll.SalarySheetSummaryNew();

                        salarySheetSummaryListing.Parameters["CompanyName"].Value = CompanyName;
                        salarySheetSummaryListing.Parameters["Address"].Value = Address;
                        salarySheetSummaryListing.Parameters["Phone"].Value = Phone;
                        salarySheetSummaryListing.Parameters["TenantId"].Value = tenantId;
                        salarySheetSummaryListing.Parameters["Address2"].Value = Address2;
                        salarySheetSummaryListing.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        string salarySheetSummaryListingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalarySheetListing(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryMonth, salaryYear, fromLocId, toLocId, Emptype));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)salarySheetSummaryListing.DataSource).JsonSource).Json = salarySheetSummaryListingjson;
                        salarySheetSummaryListing.SaveLayoutToXml(ms);

                        break;
                    case "disbursmentSummary":

                        salaryYear = Convert.ToInt16(paramHolder[0]);
                        salaryMonth = Convert.ToInt16(paramHolder[1]);

                        XtraReport disbursmentSummaryListing = new Reports.PayRoll.SalarySheetSummary();
                        disbursmentSummaryListing.Parameters["TenantId"].Value = tenantId;
                        //salarySheetSummaryListing.Parameters["CompanyName"].Value = CompanyName;
                        //salarySheetSummaryListing.Parameters["Address"].Value = Address;
                        //salarySheetSummaryListing.Parameters["Phone"].Value = Phone;
                        //salarySheetSummaryListing.Parameters["TenantId"].Value = tenantId;
                        //salarySheetSummaryListing.Parameters["Address2"].Value = Address2;
                        disbursmentSummaryListing.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        string disbursmentSummaryListingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalarySheetSummaryListing(null, salaryMonth, salaryYear));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)disbursmentSummaryListing.DataSource).JsonSource).Json = disbursmentSummaryListingjson;
                        disbursmentSummaryListing.SaveLayoutToXml(ms);


                        break;
                    case "SalarySlips":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        salaryYear = Convert.ToInt16(paramHolder[8]);
                        salaryMonth = Convert.ToInt16(paramHolder[9]);
                        XtraReport salarySlips = new Reports.PayRoll.SalarySlipsNew();

                        salarySlips.Parameters["CompanyName"].Value = CompanyName;
                        salarySlips.Parameters["Dates"].Value = salaryYear;// salaryMonth + "/"+salaryYear;
                        salarySlips.Parameters["Address"].Value = Address;
                        salarySlips.Parameters["Phone"].Value = Phone;
                        salarySlips.Parameters["TenantId"].Value = tenantId;
                        salarySlips.Parameters["Address2"].Value = Address2;

                        string salarySlipsjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalarySlips(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, salaryMonth, salaryYear));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)salarySlips.DataSource).JsonSource).Json = salarySlipsjson;
                        salarySlips.SaveLayoutToXml(ms);
                        break;

                    case "Attendance":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        short AttendanceYear = Convert.ToInt16(paramHolder[8]);
                        short AttendanceMonth = Convert.ToInt16(paramHolder[9]);
                        XtraReport attendanceTransaction = new Reports.PayRoll.Attendance();

                        attendanceTransaction.Parameters["CompanyName"].Value = CompanyName;
                        attendanceTransaction.Parameters["Address"].Value = Address;
                        attendanceTransaction.Parameters["Phone"].Value = Phone;
                        attendanceTransaction.Parameters["TenantId"].Value = tenantId;
                        attendanceTransaction.Parameters["Address2"].Value = Address2;

                        string attendanceTransactionjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAttendanceTransaction(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, AttendanceYear, AttendanceMonth));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)attendanceTransaction.DataSource).JsonSource).Json = attendanceTransactionjson;
                        attendanceTransaction.SaveLayoutToXml(ms);
                        break;

                    case "AttendanceSummary":
                        fromEmpID = paramHolder[0];
                        toEmpID = paramHolder[1];
                        fromDeptID = paramHolder[2];
                        toDeptID = paramHolder[3];
                        fromSecID = paramHolder[4];
                        toSecID = paramHolder[5];
                        fromSalary = paramHolder[6];
                        toSalary = paramHolder[7];
                        short attendanceSummaryYear = Convert.ToInt16(paramHolder[8]);
                        short attendanceSummaryMonth = Convert.ToInt16(paramHolder[9]);
                        XtraReport attendanceSummary = new Reports.PayRoll.AttendanceSummary();

                        attendanceSummary.Parameters["CompanyName"].Value = CompanyName;
                        attendanceSummary.Parameters["Address"].Value = Address;
                        attendanceSummary.Parameters["Phone"].Value = Phone;
                        attendanceSummary.Parameters["TenantId"].Value = tenantId;
                        attendanceSummary.Parameters["Address2"].Value = Address2;
                        string attendanceSummaryjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAttendanceSummary(null, fromEmpID, toEmpID, fromDeptID, toDeptID, fromSecID, toSecID, fromSalary, toSalary, attendanceSummaryYear, attendanceSummaryMonth));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)attendanceSummary.DataSource).JsonSource).Json = attendanceSummaryjson;
                        attendanceSummary.SaveLayoutToXml(ms);
                        break;

                    case "BankAdvicePermanent":
                        salaryMonth = Convert.ToInt16(paramHolder[0]);
                        salaryYear = Convert.ToInt16(paramHolder[1]);
                        XtraReport bankAdvice = new Reports.PayRoll.BankAdvice();

                        bankAdvice.Parameters["CompanyName"].Value = CompanyName;
                        bankAdvice.Parameters["Address"].Value = Address;
                        bankAdvice.Parameters["Phone"].Value = Phone;
                        bankAdvice.Parameters["TenantId"].Value = tenantId;
                        bankAdvice.Parameters["Address2"].Value = Address2;
                        bankAdvice.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        string bankAdvicejson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetBankAdvice(null, salaryMonth, salaryYear, 1));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)bankAdvice.DataSource).JsonSource).Json = bankAdvicejson;
                        bankAdvice.SaveLayoutToXml(ms);
                        break;

                    case "BankAdviceContract":
                        salaryMonth = Convert.ToInt16(paramHolder[0]);
                        salaryYear = Convert.ToInt16(paramHolder[1]);
                        XtraReport bankAdviceContract = new Reports.PayRoll.BankAdvice();

                        bankAdviceContract.Parameters["CompanyName"].Value = CompanyName;
                        bankAdviceContract.Parameters["Address"].Value = Address;
                        bankAdviceContract.Parameters["Phone"].Value = Phone;
                        bankAdviceContract.Parameters["TenantId"].Value = tenantId;
                        bankAdviceContract.Parameters["Address2"].Value = Address2;
                        bankAdviceContract.Parameters["monthYear"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(salaryMonth) + " " + salaryYear;

                        string bankAdviceContractjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetBankAdvice(null, salaryMonth, salaryYear, 2));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)bankAdviceContract.DataSource).JsonSource).Json = bankAdviceContractjson;
                        bankAdviceContract.SaveLayoutToXml(ms);
                        break;

                    case "GLLedgerTypes":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport LedgerTypeListing = new Reports.GeneralLedger.LedgerTypes();

                        LedgerTypeListing.Parameters["CompanyName"].Value = CompanyName;
                        LedgerTypeListing.Parameters["Address"].Value = Address;
                        LedgerTypeListing.Parameters["Address2"].Value = Address2;
                        LedgerTypeListing.Parameters["Phone"].Value = Phone;
                        LedgerTypeListing.Parameters["TenantId"].Value = tenantId;
                        LedgerTypeListing.Parameters["fromCode"].Value = fromCode;
                        LedgerTypeListing.Parameters["toCode"].Value = toCode;
                        string ledgerTypelistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetLedgerTypeListings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)LedgerTypeListing.DataSource).JsonSource).Json = ledgerTypelistjson;
                        LedgerTypeListing.SaveLayoutToXml(ms);
                        break;

                    case "GLlocations":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLLocationsListing = new Reports.GeneralLedger.GLLocations();

                        GLLocationsListing.Parameters["CompanyName"].Value = CompanyName;
                        GLLocationsListing.Parameters["Address"].Value = Address;
                        GLLocationsListing.Parameters["Address2"].Value = Address2;
                        GLLocationsListing.Parameters["Phone"].Value = Phone;
                        GLLocationsListing.Parameters["TenantId"].Value = tenantId;
                        GLLocationsListing.Parameters["fromCode"].Value = fromCode;
                        GLLocationsListing.Parameters["toCode"].Value = toCode;

                        string gLLocationslistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLLocationListings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLLocationsListing.DataSource).JsonSource).Json = gLLocationslistjson;
                        GLLocationsListing.SaveLayoutToXml(ms);
                        break;

                    case "GLCategories":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLCategoriesListing = new Reports.GeneralLedger.GLCategories();

                        GLCategoriesListing.Parameters["CompanyName"].Value = CompanyName;
                        GLCategoriesListing.Parameters["Address"].Value = Address;
                        GLCategoriesListing.Parameters["Address2"].Value = Address2;
                        GLCategoriesListing.Parameters["Phone"].Value = Phone;
                        GLCategoriesListing.Parameters["TenantId"].Value = tenantId;
                        GLCategoriesListing.Parameters["fromCode"].Value = fromCode;
                        GLCategoriesListing.Parameters["toCode"].Value = toCode;

                        string gLCategorieslistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLCategoriesListings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLCategoriesListing.DataSource).JsonSource).Json = gLCategorieslistjson;
                        GLCategoriesListing.SaveLayoutToXml(ms);
                        break;

                    case "GLGroup":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLGroupListing = new Reports.GeneralLedger.GLGroup();

                        GLGroupListing.Parameters["CompanyName"].Value = CompanyName;
                        GLGroupListing.Parameters["Address"].Value = Address;
                        GLGroupListing.Parameters["Address2"].Value = Address2;
                        GLGroupListing.Parameters["Phone"].Value = Phone;
                        GLGroupListing.Parameters["TenantId"].Value = tenantId;
                        GLGroupListing.Parameters["fromCode"].Value = fromCode;
                        GLGroupListing.Parameters["toCode"].Value = toCode;

                        string gLGrouplistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLGroupListings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLGroupListing.DataSource).JsonSource).Json = gLGrouplistjson;
                        GLGroupListing.SaveLayoutToXml(ms);
                        break;

                    case "GLLevel1":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLLevel1Listing = new Reports.GeneralLedger.GLLevel1();

                        GLLevel1Listing.Parameters["CompanyName"].Value = CompanyName;
                        GLLevel1Listing.Parameters["Address"].Value = Address;
                        GLLevel1Listing.Parameters["Address2"].Value = Address2;
                        GLLevel1Listing.Parameters["Phone"].Value = Phone;
                        GLLevel1Listing.Parameters["TenantId"].Value = tenantId;
                        GLLevel1Listing.Parameters["fromCode"].Value = fromCode;
                        GLLevel1Listing.Parameters["toCode"].Value = toCode;

                        string gLLevel1listjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLLevel1Listings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLLevel1Listing.DataSource).JsonSource).Json = gLLevel1listjson;
                        GLLevel1Listing.SaveLayoutToXml(ms);
                        break;

                    case "GLLevel2":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLLevel2Listing = new Reports.GeneralLedger.GLLevel2();

                        GLLevel2Listing.Parameters["CompanyName"].Value = CompanyName;
                        GLLevel2Listing.Parameters["Address"].Value = Address;
                        GLLevel2Listing.Parameters["Address2"].Value = Address2;
                        GLLevel2Listing.Parameters["Phone"].Value = Phone;
                        GLLevel2Listing.Parameters["TenantId"].Value = tenantId;
                        GLLevel2Listing.Parameters["fromCode"].Value = fromCode;
                        GLLevel2Listing.Parameters["toCode"].Value = toCode;

                        string gLLevel2listjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLLevel2Listings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLLevel2Listing.DataSource).JsonSource).Json = gLLevel2listjson;
                        GLLevel2Listing.SaveLayoutToXml(ms);
                        break;

                    case "GLLevel3":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLLevel3Listing = new Reports.GeneralLedger.GLLevel3();

                        GLLevel3Listing.Parameters["CompanyName"].Value = CompanyName;
                        GLLevel3Listing.Parameters["Address"].Value = Address;
                        GLLevel3Listing.Parameters["Address2"].Value = Address2;
                        GLLevel3Listing.Parameters["Phone"].Value = Phone;
                        GLLevel3Listing.Parameters["TenantId"].Value = tenantId;
                        GLLevel3Listing.Parameters["fromCode"].Value = fromCode;
                        GLLevel3Listing.Parameters["toCode"].Value = toCode;

                        string gLLevel3listjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLLevel3Listings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLLevel3Listing.DataSource).JsonSource).Json = gLLevel3listjson;
                        GLLevel3Listing.SaveLayoutToXml(ms);
                        break;

                    case "GLbooks":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLbooksListing = new Reports.GeneralLedger.GLBooks();

                        GLbooksListing.Parameters["CompanyName"].Value = CompanyName;
                        GLbooksListing.Parameters["Address"].Value = Address;
                        GLbooksListing.Parameters["Address2"].Value = Address2;
                        GLbooksListing.Parameters["Phone"].Value = Phone;
                        GLbooksListing.Parameters["TenantId"].Value = tenantId;
                        GLbooksListing.Parameters["fromCode"].Value = fromCode;
                        GLbooksListing.Parameters["toCode"].Value = toCode;

                        string gLbookslistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLBooksListings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLbooksListing.DataSource).JsonSource).Json = gLbookslistjson;
                        GLbooksListing.SaveLayoutToXml(ms);
                        break;

                    case "GLConfiguration":
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        XtraReport GLConfigListing = new Reports.GeneralLedger.GLConfig();

                        GLConfigListing.Parameters["CompanyName"].Value = CompanyName;
                        GLConfigListing.Parameters["Address"].Value = Address;
                        GLConfigListing.Parameters["Address2"].Value = Address2;
                        GLConfigListing.Parameters["Phone"].Value = Phone;
                        GLConfigListing.Parameters["TenantId"].Value = tenantId;
                        GLConfigListing.Parameters["fromCode"].Value = fromCode;
                        GLConfigListing.Parameters["toCode"].Value = toCode;

                        string gLConfiglistjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGLConfigListings(null, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)GLConfigListing.DataSource).JsonSource).Json = gLConfiglistjson;
                        GLConfigListing.SaveLayoutToXml(ms);
                        break;

                    case "PostDatedCheque":
                        initPointInfo();
                        fromCode = paramHolder[0];
                        toCode = paramHolder[1];
                        fromDate = paramHolder[2];
                        toDate = paramHolder[3];
                        int typeID = int.Parse(paramHolder[4]);
                        XtraReport PostDatedCheque = new Reports.GeneralLedger.PostDatedCheque();

                        PostDatedCheque.Parameters["CompanyName"].Value = CompanyName;
                        PostDatedCheque.Parameters["fromCode"].Value = fromCode;
                        PostDatedCheque.Parameters["toCode"].Value = toCode;
                        PostDatedCheque.Parameters["TenantId"].Value = tenantId;
                        PostDatedCheque.Parameters["type"].Value = typeID == 0 ? "Post Dated Cheques Status Report-Vendor (Issued)" : "Post Dated Cheques Status Report-Customer (Received)";
                        PostDatedCheque.Parameters["fromDate"].Value = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                        PostDatedCheque.Parameters["toDate"].Value = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                        PostDatedCheque.Parameters["currentDateTime"].Value = DateTime.Now.ToString("ddd, dd MMMM yyyy hh:mm tt");
                        PostDatedCheque.Parameters["FinancePoint"].Value = Financepoint;

                        string PostDatedChequejson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetPostDatedCheques(null, fromCode, toCode, fromDate, toDate, typeID));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)PostDatedCheque.DataSource).JsonSource).Json = PostDatedChequejson;
                        PostDatedCheque.SaveLayoutToXml(ms);
                        break;
                    case "CPRNumbers":
                        initPointInfo();
                        fromDate = paramHolder[0];
                        toDate = paramHolder[1];
                        string fromAcc = paramHolder[2];
                        string toAcc = paramHolder[3];
                        string fromSubAcc = paramHolder[4];
                        string toSubAcc = paramHolder[5];
                        string taxAuth = paramHolder[6];
                        string taxClass = paramHolder[7];
                        string taxAuthDesc = paramHolder[8];
                        string taxClassDesc = paramHolder[9];
                        taxClassDesc = taxClassDesc.Replace("25", " ");
                        XtraReport CPRNumbers = new Reports.Finance.CPRNumbersReport();

                        CPRNumbers.Parameters["CompanyName"].Value = CompanyName;
                        CPRNumbers.Parameters["TenantId"].Value = tenantId;
                        CPRNumbers.Parameters["fromDate"].Value = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                        CPRNumbers.Parameters["toDate"].Value = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                        CPRNumbers.Parameters["fromAcc"].Value = fromAcc;
                        CPRNumbers.Parameters["toAcc"].Value = toAcc;
                        CPRNumbers.Parameters["fromSubAcc"].Value = fromSubAcc;
                        CPRNumbers.Parameters["toSubAcc"].Value = toSubAcc;
                        CPRNumbers.Parameters["taxAuthDesc"].Value = taxAuthDesc.Replace("%20", " ");
                        CPRNumbers.Parameters["taxClassDesc"].Value = taxClassDesc.Replace("%20", " ");
                        CPRNumbers.Parameters["currentDateTime"].Value = DateTime.Now.ToString("ddd, dd MMMM yyyy hh:mm tt");
                        CPRNumbers.Parameters["FinancePoint"].Value = Financepoint;

                        string CPRNumbersjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetCPRNumbers(null, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)CPRNumbers.DataSource).JsonSource).Json = CPRNumbersjson;
                        CPRNumbers.SaveLayoutToXml(ms);
                        break;

                    case "SalesTaxDeduction":
                        initPointInfo();
                        fromDate = paramHolder[0];
                        toDate = paramHolder[1];
                        fromAcc = paramHolder[2];
                        toAcc = paramHolder[3];
                        fromSubAcc = paramHolder[4];
                        toSubAcc = paramHolder[5];
                        taxAuth = paramHolder[6];
                        taxClass = paramHolder[7];
                        taxAuthDesc = paramHolder[8];
                        taxClassDesc = paramHolder[9];
                        taxClassDesc = taxClassDesc.Replace("25", " ");
                        XtraReport SalexTaxDeduction = new Reports.Finance.SalesTaxDeduction();

                        SalexTaxDeduction.Parameters["CompanyName"].Value = CompanyName;
                        SalexTaxDeduction.Parameters["TenantId"].Value = tenantId;
                        SalexTaxDeduction.Parameters["fromDate"].Value = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                        SalexTaxDeduction.Parameters["toDate"].Value = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                        SalexTaxDeduction.Parameters["fromAcc"].Value = fromAcc;
                        SalexTaxDeduction.Parameters["toAcc"].Value = toAcc;
                        SalexTaxDeduction.Parameters["fromSubAcc"].Value = fromSubAcc;
                        SalexTaxDeduction.Parameters["toSubAcc"].Value = toSubAcc;
                        SalexTaxDeduction.Parameters["taxAuthDesc"].Value = taxAuthDesc.Replace("%20", " ");
                        SalexTaxDeduction.Parameters["taxClassDesc"].Value = taxClassDesc.Replace("%20", " ");
                        SalexTaxDeduction.Parameters["FinancePoint"].Value = Financepoint;

                        string SalexTaxDeductionjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalesTaxDeduction(null, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)SalexTaxDeduction.DataSource).JsonSource).Json = SalexTaxDeductionjson;
                        SalexTaxDeduction.SaveLayoutToXml(ms);
                        break;

                    case "PartyTax":
                        initPointInfo();
                        fromDate = paramHolder[0];
                        toDate = paramHolder[1];
                        fromAcc = paramHolder[2];
                        toAcc = paramHolder[3];
                        fromSubAcc = paramHolder[4];
                        toSubAcc = paramHolder[5];
                        taxAuth = paramHolder[6];
                        taxAuthDesc = paramHolder[7];
                        string fromTaxClass = paramHolder[8];
                        string toTaxClass = paramHolder[9];
                        string fromTaxClassDesc = paramHolder[10];
                        string toTaxClassDesc = paramHolder[11];
                        XtraReport PartyTax = new Reports.Finance.PartyTax();

                        PartyTax.Parameters["CompanyName"].Value = CompanyName;
                        PartyTax.Parameters["TenantId"].Value = tenantId;
                        PartyTax.Parameters["fromDate"].Value = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                        PartyTax.Parameters["toDate"].Value = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                        PartyTax.Parameters["fromAcc"].Value = fromAcc;
                        PartyTax.Parameters["toAcc"].Value = toAcc;
                        PartyTax.Parameters["fromSubAcc"].Value = fromSubAcc;
                        PartyTax.Parameters["toSubAcc"].Value = toSubAcc;
                        PartyTax.Parameters["taxAuthDesc"].Value = taxAuthDesc.Replace("%20", " ");
                        PartyTax.Parameters["fromTaxClass"].Value = fromTaxClass;
                        PartyTax.Parameters["toTaxClass"].Value = toTaxClass;
                        PartyTax.Parameters["FinancePoint"].Value = Financepoint;

                        string PartyTaxjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetPartyTax(null, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, fromTaxClass, toTaxClass));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)PartyTax.DataSource).JsonSource).Json = PartyTaxjson;
                        PartyTax.SaveLayoutToXml(ms);
                        break;

                    case "salesTaxWithheld":
                        initPointInfo();
                        fromDate = paramHolder[0];
                        toDate = paramHolder[1];
                        fromAcc = paramHolder[2];
                        toAcc = paramHolder[3];
                        fromSubAcc = paramHolder[4];
                        toSubAcc = paramHolder[5];
                        taxAuth = paramHolder[6];
                        taxClass = paramHolder[7];
                        taxAuthDesc = paramHolder[8];
                        taxClassDesc = paramHolder[9];
                        taxClassDesc = taxClassDesc.Replace("25", " ");
                        string type = paramHolder[10];
                        string reportType = paramHolder[11];
                        XtraReport SalesTaxWithheld = new Reports.Finance.SalesTaxWithheld();

                        SalesTaxWithheld.Parameters["CompanyName"].Value = CompanyName;
                        SalesTaxWithheld.Parameters["TenantId"].Value = tenantId;
                        SalesTaxWithheld.Parameters["fromDate"].Value = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                        SalesTaxWithheld.Parameters["toDate"].Value = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                        SalesTaxWithheld.Parameters["fromAcc"].Value = fromAcc;
                        SalesTaxWithheld.Parameters["toAcc"].Value = toAcc;
                        SalesTaxWithheld.Parameters["fromSubAcc"].Value = fromSubAcc;
                        SalesTaxWithheld.Parameters["toSubAcc"].Value = toSubAcc;
                        SalesTaxWithheld.Parameters["taxAuthDesc"].Value = taxAuthDesc.Replace("%20", " ");
                        SalesTaxWithheld.Parameters["taxClassDesc"].Value = taxClassDesc.Replace("%20", " ");
                        SalesTaxWithheld.Parameters["type"].Value = type;
                        SalesTaxWithheld.Parameters["reportType"].Value = reportType;
                        SalesTaxWithheld.Parameters["FinancePoint"].Value = Financepoint;
                        string salesTaxWithheldjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSalesTaxWithheld(null, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass, type));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)SalesTaxWithheld.DataSource).JsonSource).Json = salesTaxWithheldjson;
                        SalesTaxWithheld.SaveLayoutToXml(ms);
                        break;

                    case "taxDueReport":
                        initPointInfo();
                        fromDate = paramHolder[0];
                        toDate = paramHolder[1];
                        fromAcc = paramHolder[2];
                        toAcc = paramHolder[3];
                        fromSubAcc = paramHolder[4];
                        toSubAcc = paramHolder[5];
                        taxAuth = paramHolder[6];
                        taxClass = paramHolder[7];
                        taxAuthDesc = paramHolder[8];
                        taxClassDesc = paramHolder[9];
                        taxClassDesc = taxClassDesc.Replace("25", " ");
                        type = paramHolder[10];
                        reportType = paramHolder[11];
                        XtraReport taxDueReport = new Reports.Finance.TaxDueReport();

                        taxDueReport.Parameters["CompanyName"].Value = CompanyName;
                        taxDueReport.Parameters["TenantId"].Value = tenantId;
                        taxDueReport.Parameters["fromDate"].Value = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy");
                        taxDueReport.Parameters["toDate"].Value = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy");
                        taxDueReport.Parameters["fromAcc"].Value = fromAcc;
                        taxDueReport.Parameters["toAcc"].Value = toAcc;
                        taxDueReport.Parameters["fromSubAcc"].Value = fromSubAcc;
                        taxDueReport.Parameters["toSubAcc"].Value = toSubAcc;
                        taxDueReport.Parameters["taxAuthDesc"].Value = taxAuthDesc.Replace("%20", " ");
                        taxDueReport.Parameters["taxClassDesc"].Value = taxClassDesc.Replace("%20", " ");
                        taxDueReport.Parameters["type"].Value = type;
                        taxDueReport.Parameters["reportType"].Value = reportType;
                        taxDueReport.Parameters["FinancePoint"].Value = Financepoint;
                        string taxDueReportjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTaxDue(null, fromDate, toDate, fromAcc, toAcc, fromSubAcc, toSubAcc, taxAuth, taxClass, type));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)taxDueReport.DataSource).JsonSource).Json = taxDueReportjson;
                        taxDueReport.SaveLayoutToXml(ms);
                        break;



                    case "CashReceipt":
                        initPointInfo();
                        string bookId = paramHolder[0];

                        int? year = null;
                        if (paramHolder[1] != "null")
                        {
                            year = Convert.ToInt32(paramHolder[1]);
                        }

                        int? month = null;
                        if (paramHolder[2] != "null")
                        {
                            month = Convert.ToInt32(paramHolder[2]);
                        }

                        int locId = Convert.ToInt32(paramHolder[3]);
                        int fromConfigId = Convert.ToInt32(paramHolder[4]);
                        int toConfigId = Convert.ToInt32(paramHolder[5]);
                        string status = paramHolder[10].ToString();
                        int fromDoc = 0;
                        //if (!string.IsNullOrWhiteSpace(paramHolder[6]))
                        //{
                        //    fromDoc = Convert.ToInt32(paramHolder[6]);
                        //}

                        int.TryParse(paramHolder[6], out fromDoc);

                        int toDoc = 0;
                        int.TryParse(paramHolder[7], out toDoc);
                        string curId = paramHolder[8].ToString();
                        double currate = Convert.ToDouble(paramHolder[9]);
                        var signaturesData = ReportDataHandlerBase.GetSingnatures(Convert.ToInt32(tenantId));

                        XtraReport CashRec = new Reports.Finance.CashReceiptNew();
                        CashRec.Parameters["CompanyName"].Value = CompanyName;
                        CashRec.Parameters["TenantId"].Value = tenantId;
                        CashRec.Parameters["Address"].Value = Address;
                        CashRec.Parameters["Address2"].Value = Address2;
                        CashRec.Parameters["Phone"].Value = Phone;
                        CashRec.Parameters["FirstSignature"].Value = signaturesData.FirstSignature;
                        CashRec.Parameters["SecondSignature"].Value = signaturesData.SecondSignature;
                        CashRec.Parameters["ThirdSignature"].Value = signaturesData.ThirdSignature;
                        CashRec.Parameters["FourthSignature"].Value = signaturesData.FourthSignature;
                        CashRec.Parameters["FifthSignature"].Value = signaturesData.FifthSignature;
                        CashRec.Parameters["SixthSignature"].Value = signaturesData.SixthSignature;
                        CashRec.Parameters["FinancePoint"].Value = Financepoint;
                        string CashRecJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.CashReceipt(bookId, year, month, fromConfigId, toConfigId, fromDoc, toDoc, locId, curId, currate, status));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)CashRec.DataSource).JsonSource).Json = CashRecJson;
                        CashRec.SaveLayoutToXml(ms);
                        break;

                    case "OpeningReport":
                    case "OpeningActivityReport":
                        initPointInfo();
                        if (repoertName == "OpeningReport")
                        {
                            if (paramHolder[6] == "true")
                                xtraReport = new Reports.Inventory.OpeningReport();
                            else
                                xtraReport = new Reports.Inventory.OpeningReportWithoutRates();
                            xtraReport.Parameters["TenantId"].Value = tenantId;
                            xtraReport.Parameters["CompanyName"].Value = CompanyName;
                            xtraReport.Parameters["Address"].Value = Address;
                            xtraReport.Parameters["Address2"].Value = Address2;
                            xtraReport.Parameters["InventoryPoint"].Value = Inventorypoint;
                            xtraReport.Parameters["Phone"].Value = Phone;
                        }
                        else if (repoertName == "OpeningActivityReport")
                        {
                            xtraReport = new Reports.Inventory.OpeningActivityReport();
                            xtraReport.Parameters["CompanyName"].Value = CompanyName;
                            xtraReport.Parameters["TenantId"].Value = tenantId;
                            xtraReport.Parameters["Address"].Value = Address;
                            xtraReport.Parameters["Address2"].Value = Address2;
                            xtraReport.Parameters["Phone"].Value = Phone;
                            xtraReport.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                            xtraReport.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                            xtraReport.Parameters["FromDate"].Value = DateTime.Parse(paramHolder[0]);
                            xtraReport.Parameters["ToDate"].Value = DateTime.Parse(paramHolder[1]);
                            xtraReport.Parameters["InventoryPoint"].Value = Inventorypoint;
                            //xtraReport.Parameters["FromLoc"].Value = DateTime.Parse(paramHolder[4]);
                            //xtraReport.Parameters["ToLoc"].Value = DateTime.Parse(paramHolder[5]);
                        }

                        string rptOpnJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetOpeningData(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3]), paramHolder[4], paramHolder[5]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = rptOpnJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;
                    case "ConsumptionReport":
                    case "ConsumptionActivityReport":
                    case "ConsumptionActivityReportCostWise":
                        initPointInfo();
                        if (repoertName == "ConsumptionReport")
                        {
                            xtraReport = new Reports.Inventory.ConsumptionReport();
                        }
                        else if (repoertName == "ConsumptionActivityReport")
                        {
                            xtraReport = new Reports.Inventory.ConsumptionActivityReport();
                        }
                        else if (repoertName == "ConsumptionActivityReportCostWise")
                        {
                            xtraReport = new Reports.Inventory.ConsumptionActivityReportCostWise();

                        }
                        xtraReport.Parameters["CompanyName"].Value = CompanyName;
                        xtraReport.Parameters["TenantId"].Value = tenantId;
                        xtraReport.Parameters["Address"].Value = Address;
                        xtraReport.Parameters["Address2"].Value = Address2;
                        xtraReport.Parameters["Phone"].Value = Phone;
                        //xtraReport.Parameters["InventoryPoint"].Value = Inventorypoint;
                        //fromItem = paramHolder[4];
                        //toItem  = paramHolder[5];
                        if (paramHolder.Count > 7)
                        {
                            fromItem = paramHolder[6];
                            toItem = paramHolder[7];
                        }
                        else
                        {
                            fromItem = "0";
                            toItem = "99-999-99-9999";
                        }


                        string rptConJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetConsumptionData(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), Convert.ToInt32(paramHolder[2]),fromItem,toItem, Convert.ToInt32(paramHolder[3]), paramHolder[4], paramHolder[5], paramHolder[6]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = rptConJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;
                    case "ConsumptionDepartmentWise":
                        initPointInfo();
                        XtraReport consumptionReportDepartmentWise = new Reports.Inventory.ConsumptionReportDepartmentWise();
                        consumptionReportDepartmentWise.Parameters["CompanyName"].Value = CompanyName;
                        consumptionReportDepartmentWise.Parameters["TenantId"].Value = tenantId;
                        consumptionReportDepartmentWise.Parameters["Address"].Value = Address;
                        consumptionReportDepartmentWise.Parameters["Address2"].Value = Address2;
                        consumptionReportDepartmentWise.Parameters["Phone"].Value = Phone;
                        consumptionReportDepartmentWise.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string consumptionReportDepartmentWiseJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetConsumptionDepartmentWiseData(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], "ConsumptionDepartmentWise"));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)consumptionReportDepartmentWise.DataSource).JsonSource).Json = consumptionReportDepartmentWiseJson;
                        consumptionReportDepartmentWise.SaveLayoutToXml(ms);
                        break;
                    case "ConsumptionReportAccWise":
                        initPointInfo();
                        XtraReport consumptionReportAccWise = new Reports.Inventory.ConsumptionReportAccWise();
                        consumptionReportAccWise.Parameters["CompanyName"].Value = CompanyName;
                        consumptionReportAccWise.Parameters["TenantId"].Value = tenantId;
                        consumptionReportAccWise.Parameters["Address"].Value = Address;
                        consumptionReportAccWise.Parameters["Address2"].Value = Address2;
                        consumptionReportAccWise.Parameters["Phone"].Value = Phone;
                        consumptionReportAccWise.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string consumptionReportAccWiseJson = JsonConvert.SerializeObject(
                                                     ReportDataHandlerBase.GetConsumptionDepartmentWiseData(0, "0", "0", paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], "ConsumptionReportAccWise"));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)consumptionReportAccWise.DataSource).JsonSource).Json = consumptionReportAccWiseJson;
                        consumptionReportAccWise.SaveLayoutToXml(ms);
                        break;
                    case "ConsumptionReportOrderWise":
                        initPointInfo();
                        XtraReport consumptionReportOrderWise = new Reports.Inventory.ConsumptionReportOrderWise();
                        consumptionReportOrderWise.Parameters["CompanyName"].Value = CompanyName;
                        consumptionReportOrderWise.Parameters["TenantId"].Value = tenantId;
                        consumptionReportOrderWise.Parameters["Address"].Value = Address;
                        consumptionReportOrderWise.Parameters["Address2"].Value = Address2;
                        consumptionReportOrderWise.Parameters["Phone"].Value = Phone;
                        consumptionReportOrderWise.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string consumptionReportOrderWiseJson = JsonConvert.SerializeObject(
                                                     ReportDataHandlerBase.GetConsumptionDepartmentWiseData(0, "0", "0", paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], "ConsumptionReportOrderWise"));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)consumptionReportOrderWise.DataSource).JsonSource).Json = consumptionReportOrderWiseJson;
                        consumptionReportOrderWise.SaveLayoutToXml(ms);
                        break;
                    case "ConsumptionReportItemWise":
                        initPointInfo();
                        XtraReport consumptionReportItemWise = new Reports.Inventory.ConsumptionReportItemWise();
                        consumptionReportItemWise.Parameters["CompanyName"].Value = CompanyName;
                        consumptionReportItemWise.Parameters["TenantId"].Value = tenantId;
                        consumptionReportItemWise.Parameters["Address"].Value = Address;
                        consumptionReportItemWise.Parameters["Address2"].Value = Address2;
                        consumptionReportItemWise.Parameters["Phone"].Value = Phone;
                        consumptionReportItemWise.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string consumptionReportItemWiseJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetConsumptionDepartmentWiseData(0, "0", "0", paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], "ConsumptionReportItemWise"));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)consumptionReportItemWise.DataSource).JsonSource).Json = consumptionReportItemWiseJson;
                        consumptionReportItemWise.SaveLayoutToXml(ms);
                        break;
                    case "AdjustmentReport":
                    case "AdjustmentActivityReport":
                        initPointInfo();
                        if (repoertName == "AdjustmentReport")
                        {
                            if (paramHolder[6] == "true")
                                xtraReport = new Reports.Inventory.AdjustmentReport();
                            else
                                xtraReport = new Reports.Inventory.AdjustmentReportWithoutRates();
                        }
                        else if (repoertName == "AdjustmentActivityReport")
                        {
                            xtraReport = new Reports.Inventory.AdjustmentActivityReport();
                        }
                        xtraReport.Parameters["CompanyName"].Value = CompanyName;
                        xtraReport.Parameters["TenantId"].Value = tenantId;
                        xtraReport.Parameters["Address"].Value = Address;
                        xtraReport.Parameters["Address2"].Value = Address2;
                        xtraReport.Parameters["Phone"].Value = Phone;
                        xtraReport.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string rptAdjJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAdjustmentData(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3]), paramHolder[4], paramHolder[5]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = rptAdjJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;

                    case "ChartOfACListing":
                        xtraReport = new Reports.Finance.ChartOfACListing();

                        xtraReport.Parameters["CompanyName"].Value = CompanyName;
                        xtraReport.Parameters["TenantId"].Value = tenantId;
                        xtraReport.Parameters["Address"].Value = Address;
                        xtraReport.Parameters["Phone"].Value = Phone;


                        string rptCHLJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetChartOfAccountsData(Convert.ToInt32(paramHolder[0]), paramHolder[1], paramHolder[2]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = rptCHLJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;

                    case "SubledgerListing":
                        xtraReport = new Reports.Finance.SubledgerListing();

                        xtraReport.Parameters["CompanyName"].Value = CompanyName;
                        xtraReport.Parameters["TenantId"].Value = tenantId;
                        xtraReport.Parameters["Address"].Value = Address;
                        xtraReport.Parameters["Phone"].Value = Phone;

                        string rptSLLJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSubLedgerData(Convert.ToInt32(paramHolder[0])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = rptSLLJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;

                    case "AgeingInvoiceReport":

                        fromDate = paramHolder[0];
                        fromAcc = paramHolder[2];
                        FromsubAcc = Convert.ToInt32(paramHolder[4]);
                        TosubAcc = Convert.ToInt32(paramHolder[5]);

                        XtraReport ageingInvoice = new Reports.Finance.AgeingInvoice();
                        ageingInvoice.Parameters["CompanyName"].Value = CompanyName;
                        ageingInvoice.Parameters["fromDate"].Value = fromDate;
                        ageingInvoice.Parameters["FromAcc"].Value = fromAcc;

                        //ageingInvoice.Parameters["Address"].Value = Address;
                        //ageingInvoice.Parameters["Phone"].Value = Phone;
                        ageingInvoice.Parameters["TenantId"].Value = tenantId;



                        string ageingjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAgeingInvoiceListing(0, fromDate, fromAcc, FromsubAcc, TosubAcc));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)ageingInvoice.DataSource).JsonSource).Json = ageingjson;
                        ageingInvoice.SaveLayoutToXml(ms);
                        break;

                    case "Subledger":
                        initPointInfo();
                        var text = "";
                        if (paramHolder[11] == "true")
                        {
                            xtraReport = new Reports.Finance.SubledgerSummary();
                            text = "Summary";
                        }
                        else
                        {

                            xtraReport = new Reports.Finance.Subledger();
                        }

                        xtraReport.Parameters["fromDate"].Value = DateTime.Parse(paramHolder[0]);
                        xtraReport.Parameters["toDate"].Value = DateTime.Parse(paramHolder[1]);
                        xtraReport.Parameters["CompanyName"].Value = CompanyName;
                        xtraReport.Parameters["CompanyAddress"].Value = Address;
                        xtraReport.Parameters["CompanyPhone"].Value = Phone;
                        xtraReport.Parameters["TenantId"].Value = tenantId;
                        xtraReport.Parameters["FromAcc"].Value = paramHolder[2];
                        xtraReport.Parameters["ToAcc"].Value = paramHolder[3];
                        xtraReport.Parameters["cur"].Value = paramHolder[10];
                        xtraReport.Parameters["FinancePoint"].Value = Financepoint;
                        //xtraReport.Parameters["summary"].Value = paramHolder[11];

                        string rptSLJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetSubledger(text,DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[4]), Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[6]), Convert.ToInt32(paramHolder[7]), paramHolder[8], Convert.ToInt32(paramHolder[9])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = rptSLJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;

                    case "GeneralLedger":
                    case "GeneralLedgerBookWise":
                        initPointInfo();
                        if (repoertName == "GeneralLedger")
                        {
                            xtraReport = new Reports.Finance.GeneralLedger();

                            xtraReport.Parameters["FromAcc"].Value = paramHolder[2];
                            xtraReport.Parameters["ToAcc"].Value = paramHolder[3];
                            xtraReport.Parameters["FromLoc"].Value = paramHolder[9] == "undefined" ? "" : paramHolder[9].Replace("%20", " ");
                            xtraReport.Parameters["ToLoc"].Value = paramHolder[10] == "undefined" ? "" : paramHolder[10].Replace("%20"," ");
                            //xtraReport.Parameters["TenantId"].Value = tenantId;
                            xtraReport.Parameters["Status"].Value = paramHolder[4];
                            xtraReport.Parameters["cur"].Value = paramHolder[7];

                        }
                        else if (repoertName == "GeneralLedgerBookWise")
                        {
                            xtraReport = new Reports.Finance.GeneralLedgerBookWise();
                        }

                        xtraReport.Parameters["fromDate"].Value = paramHolder[0];
                       
                        xtraReport.Parameters["toDate"].Value = paramHolder[1];
                        xtraReport.Parameters["CompanyName"].Value = CompanyName;
                        xtraReport.Parameters["CompanyAddress"].Value = Address;
                        xtraReport.Parameters["CompanyPhone"].Value = Phone;
                        xtraReport.Parameters["TenantId"].Value = tenantId;
                     
                        xtraReport.Parameters["FinancePoint"].Value = Financepoint;
                        //xtraReport.Parameters["fromLoc"].Value = paramHolder[5];
                        //xtraReport.Parameters["toLoc"].Value = paramHolder[7];

                        string rptGLLJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGeneralLedger(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), paramHolder[2], paramHolder[3], paramHolder[4], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[8]), Convert.ToInt32(paramHolder[6])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = rptGLLJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;
                    case "CustAnalysisReport":
                        initPointInfo();
                        if (paramHolder[11] == "true")
                            xtraReport = new Reports.Finance.CustAnalysisReportSummary();
                        else
                            xtraReport = new Reports.Finance.CustAnalysisReport();

                        xtraReport.Parameters["fromDate"].Value = paramHolder[0];
                        xtraReport.Parameters["toDate"].Value = paramHolder[1];
                        xtraReport.Parameters["FromAcc"].Value = paramHolder[4];
                        xtraReport.Parameters["ToAcc"].Value = paramHolder[5];
                        xtraReport.Parameters["CompanyName"].Value = CompanyName;
                        xtraReport.Parameters["Address"].Value = Address;
                        xtraReport.Parameters["Address2"].Value = Address2;
                        xtraReport.Parameters["Phone"].Value = Phone;
                        xtraReport.Parameters["TenantId"].Value = tenantId;
                        xtraReport.Parameters["FromSubAcc"].Value = paramHolder[6];
                        xtraReport.Parameters["ToSubAcc"].Value = paramHolder[7];
                        xtraReport.Parameters["cur"].Value = paramHolder[8];
                        xtraReport.Parameters["LocId"].Value = paramHolder[9];

                        xtraReport.Parameters["FinancePoint"].Value = Financepoint;
                        // xtraReport.Parameters["summary"].Value = paramHolder[9];

                        string custAnalysisJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetCustAnalysis(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), paramHolder[2], Convert.ToInt32(paramHolder[3]), paramHolder[4], paramHolder[5], paramHolder[6], paramHolder[7], Convert.ToInt32(paramHolder[8]), Convert.ToInt32(paramHolder[9])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)xtraReport.DataSource).JsonSource).Json = custAnalysisJson;
                        xtraReport.SaveLayoutToXml(ms);
                        break;

                    case "CASHBOOK":
                        initPointInfo();
                        XtraReport rptcashBook = new Reports.Finance.rptCashBookNew();


                        bool IsCashBook = Convert.ToBoolean(paramHolder[7]);

                        rptcashBook.Parameters["fromDate"].Value = paramHolder[0];
                        rptcashBook.Parameters["FromAcc"].Value = paramHolder[2];
                        rptcashBook.Parameters["ToAcc"].Value = paramHolder[3];
                        rptcashBook.Parameters["toDate"].Value = paramHolder[1];
                        rptcashBook.Parameters["CompanyName"].Value = CompanyName;
                        rptcashBook.Parameters["CompanyAddress"].Value = Address;
                        rptcashBook.Parameters["CompanyPhone"].Value = Phone;
                        rptcashBook.Parameters["TenantId"].Value = tenantId;
                        rptcashBook.Parameters["Status"].Value = paramHolder[4];
                        rptcashBook.Parameters["CashBook"].Value = IsCashBook;
                        rptcashBook.Parameters["FinancePoint"].Value = Financepoint;


                        string cashBookjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.getCashBook(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), paramHolder[2], paramHolder[3], paramHolder[4], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[6]), IsCashBook));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptcashBook.DataSource).JsonSource).Json = cashBookjson;
                        rptcashBook.SaveLayoutToXml(ms);
                        break;


                    case "ItemStockSegment2":
                        initPointInfo();
                        XtraReport itemStockSegment2Rpt = new Reports.Inventory.ItemStockSegment2();
                        itemStockSegment2Rpt.Parameters["CompanyName"].Value = CompanyName;
                        itemStockSegment2Rpt.Parameters["FromDate"].Value = paramHolder[0];
                        itemStockSegment2Rpt.Parameters["ToDate"].Value = paramHolder[1];
                        itemStockSegment2Rpt.Parameters["FromItem"].Value = paramHolder[4];
                        itemStockSegment2Rpt.Parameters["ToItem"].Value = paramHolder[5];
                        itemStockSegment2Rpt.Parameters["TenantId"].Value = tenantId;
                        itemStockSegment2Rpt.Parameters["Address"].Value = Address;
                        itemStockSegment2Rpt.Parameters["Address2"].Value = Address2;
                        itemStockSegment2Rpt.Parameters["Phone"].Value = Phone;
                        itemStockSegment2Rpt.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string itemStockSegment2RptJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemStockSegment2Rpt(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemStockSegment2Rpt.DataSource).JsonSource).Json = itemStockSegment2RptJson;
                        itemStockSegment2Rpt.SaveLayoutToXml(ms);
                        break;
                    case "ItemStockSegment3":
                        initPointInfo();
                        XtraReport itemStockSegment3Rpt = new Reports.Inventory.ItemStockSegment3();
                        itemStockSegment3Rpt.Parameters["CompanyName"].Value = CompanyName;
                        itemStockSegment3Rpt.Parameters["FromDate"].Value = paramHolder[0];
                        itemStockSegment3Rpt.Parameters["ToDate"].Value = paramHolder[1];
                        itemStockSegment3Rpt.Parameters["FromItem"].Value = paramHolder[4];
                        itemStockSegment3Rpt.Parameters["ToItem"].Value = paramHolder[5];
                        itemStockSegment3Rpt.Parameters["TenantId"].Value = tenantId;
                        itemStockSegment3Rpt.Parameters["Address"].Value = Address;
                        itemStockSegment3Rpt.Parameters["Address2"].Value = Address2;
                        itemStockSegment3Rpt.Parameters["Phone"].Value = Phone;
                        itemStockSegment3Rpt.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string itemStockSegment3RptJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemStockSegment3Rpt(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemStockSegment3Rpt.DataSource).JsonSource).Json = itemStockSegment3RptJson;
                        itemStockSegment3Rpt.SaveLayoutToXml(ms);
                        break;

                    case "StockReportDetail":
                        initPointInfo();
                        XtraReport stockReportDetailRpt = new Reports.Inventory.StockReportDetail();
                        stockReportDetailRpt.Parameters["CompanyName"].Value = CompanyName;
                        stockReportDetailRpt.Parameters["FromDate"].Value = paramHolder[0];
                        stockReportDetailRpt.Parameters["ToDate"].Value = paramHolder[1];
                        stockReportDetailRpt.Parameters["FromItem"].Value = paramHolder[4];
                        stockReportDetailRpt.Parameters["ToItem"].Value = paramHolder[5];
                        stockReportDetailRpt.Parameters["TenantId"].Value = tenantId;
                        stockReportDetailRpt.Parameters["Address"].Value = Address;
                        stockReportDetailRpt.Parameters["Address2"].Value = Address2;
                        stockReportDetailRpt.Parameters["Phone"].Value = Phone;
                        stockReportDetailRpt.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string stockReportDetailRptJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemStockRpt(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)stockReportDetailRpt.DataSource).JsonSource).Json = stockReportDetailRptJson;
                        stockReportDetailRpt.SaveLayoutToXml(ms);
                        break;
                    case "StockReportQuantitative":
                        initPointInfo();
                        XtraReport stockReportQuantitativeRpt = new Reports.Inventory.StockReportQuantitative();
                        stockReportQuantitativeRpt.Parameters["CompanyName"].Value = CompanyName;
                        stockReportQuantitativeRpt.Parameters["FromDate"].Value = paramHolder[0];
                        stockReportQuantitativeRpt.Parameters["ToDate"].Value = paramHolder[1];
                        stockReportQuantitativeRpt.Parameters["FromItem"].Value = paramHolder[4];
                        stockReportQuantitativeRpt.Parameters["ToItem"].Value = paramHolder[5];
                        stockReportQuantitativeRpt.Parameters["TenantId"].Value = tenantId;
                        stockReportQuantitativeRpt.Parameters["Address"].Value = Address;
                        stockReportQuantitativeRpt.Parameters["Address2"].Value = Address2;
                        stockReportQuantitativeRpt.Parameters["Phone"].Value = Phone;
                        stockReportQuantitativeRpt.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string stockReportQuantitativeRptJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemStockRpt(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)stockReportQuantitativeRpt.DataSource).JsonSource).Json = stockReportQuantitativeRptJson;
                        stockReportQuantitativeRpt.SaveLayoutToXml(ms);
                        break;
                    case "StocklevelWise":
                        initPointInfo();
                        XtraReport stockReportLevelWise = new Reports.Inventory.StockReportLevelWise();
                        stockReportLevelWise.Parameters["CompanyName"].Value = CompanyName;
                        stockReportLevelWise.Parameters["FromDate"].Value = paramHolder[0];
                        stockReportLevelWise.Parameters["ToDate"].Value = paramHolder[1];
                        stockReportLevelWise.Parameters["FromItem"].Value = paramHolder[4];
                        stockReportLevelWise.Parameters["ToItem"].Value = paramHolder[5];
                        stockReportLevelWise.Parameters["TenantId"].Value = tenantId;
                        stockReportLevelWise.Parameters["Address"].Value = Address;
                        stockReportLevelWise.Parameters["Address2"].Value = Address2;
                        stockReportLevelWise.Parameters["Phone"].Value = Phone;
                        stockReportLevelWise.Parameters["InventoryPoint"].Value = Inventorypoint;
                        stockReportLevelWise.Parameters["FinancePoint"].Value = Financepoint;
                        string stockReportLevelWiseRptJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemStockRpt(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)stockReportLevelWise.DataSource).JsonSource).Json = stockReportLevelWiseRptJson;
                        stockReportLevelWise.SaveLayoutToXml(ms);
                        break;
                    case "ItemStockSegmentSummary":
                        initPointInfo();
                        XtraReport itemStockSegmentSummary = new Reports.Inventory.ItemStockSegmentSummary();
                        itemStockSegmentSummary.Parameters["CompanyName"].Value = CompanyName;
                        itemStockSegmentSummary.Parameters["FromDate"].Value = paramHolder[0];
                        itemStockSegmentSummary.Parameters["ToDate"].Value = paramHolder[1];
                        itemStockSegmentSummary.Parameters["FromItem"].Value = paramHolder[4];
                        itemStockSegmentSummary.Parameters["ToItem"].Value = paramHolder[5];
                        itemStockSegmentSummary.Parameters["TenantId"].Value = tenantId;
                        itemStockSegmentSummary.Parameters["Address"].Value = Address;
                        itemStockSegmentSummary.Parameters["Address2"].Value = Address2;
                        itemStockSegmentSummary.Parameters["Phone"].Value = Phone;
                        itemStockSegmentSummary.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string itemStockSegmentSummaryJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemStockSegment2Rpt(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemStockSegmentSummary.DataSource).JsonSource).Json = itemStockSegmentSummaryJson;
                        itemStockSegmentSummary.SaveLayoutToXml(ms);
                        break;
                    case "QuantitativeStock":
                        initPointInfo();
                        XtraReport quantitativeStock = new Reports.Inventory.QuantitativeStock();
                        quantitativeStock.Parameters["CompanyName"].Value = CompanyName;
                        quantitativeStock.Parameters["FromItem"].Value = paramHolder[2];
                        quantitativeStock.Parameters["ToItem"].Value = paramHolder[3];
                        quantitativeStock.Parameters["TenantId"].Value = tenantId;
                        quantitativeStock.Parameters["Address"].Value = Address;
                        quantitativeStock.Parameters["Address2"].Value = Address2;
                        quantitativeStock.Parameters["Phone"].Value = Phone;
                        quantitativeStock.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string quantitativeStockJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemQuantitativeStock(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)quantitativeStock.DataSource).JsonSource).Json = quantitativeStockJson;
                        quantitativeStock.SaveLayoutToXml(ms);
                        break;
                    case "QuantitativeStock3":
                        initPointInfo();
                        XtraReport quantitativeStock3 = new Reports.Inventory.QuantitativeStock3();
                        quantitativeStock3.Parameters["CompanyName"].Value = CompanyName;
                        quantitativeStock3.Parameters["FromItem"].Value = paramHolder[2];
                        quantitativeStock3.Parameters["ToItem"].Value = paramHolder[3];
                        quantitativeStock3.Parameters["TenantId"].Value = tenantId;
                        quantitativeStock3.Parameters["Address"].Value = Address;
                        quantitativeStock3.Parameters["Address2"].Value = Address2;
                        quantitativeStock3.Parameters["Phone"].Value = Phone;
                        quantitativeStock3.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string quantitativeStock3Json = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemQuantitativeStock3(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)quantitativeStock3.DataSource).JsonSource).Json = quantitativeStock3Json;
                        quantitativeStock3.SaveLayoutToXml(ms);
                        break;
                    case "ConsolidatstockReport":
                        initPointInfo();
                        XtraReport ConsolidatRpt = new Reports.Inventory.ConsolidateStockReport();
                        ConsolidatRpt.Parameters["CompanyName"].Value = CompanyName;
                        ConsolidatRpt.Parameters["FromDate"].Value = paramHolder[0];
                        ConsolidatRpt.Parameters["ToDate"].Value = paramHolder[1];
                        ConsolidatRpt.Parameters["FromItem"].Value = paramHolder[4];
                        ConsolidatRpt.Parameters["ToItem"].Value = paramHolder[5];
                        ConsolidatRpt.Parameters["TenantId"].Value = tenantId;
                        ConsolidatRpt.Parameters["Address"].Value = Address;
                        ConsolidatRpt.Parameters["Address2"].Value = Address2;
                        ConsolidatRpt.Parameters["Phone"].Value = Phone;
                        ConsolidatRpt.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string stockReportConsolidatJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetItemStockRpt(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)ConsolidatRpt.DataSource).JsonSource).Json = stockReportConsolidatJson;
                        ConsolidatRpt.SaveLayoutToXml(ms);
                        break;
                    case "MonthlyConsolidatedLedger":
                        initPointInfo();
                        XtraReport monthlyConsolidatedReport = new Reports.Finance.RptMonthlyConsolidated();

                        monthlyConsolidatedReport.Parameters["CompanyName"].Value = CompanyName;
                        monthlyConsolidatedReport.Parameters["Address"].Value = Address;
                        monthlyConsolidatedReport.Parameters["Phone"].Value = Phone;
                        monthlyConsolidatedReport.Parameters["TenantId"].Value = tenantId;
                        monthlyConsolidatedReport.Parameters["FromLoc"].Value = paramHolder[8] == "undefined" ? "" : paramHolder[8].Replace("%20", " ");
                        monthlyConsolidatedReport.Parameters["ToLoc"].Value = paramHolder[9] == "undefined" ? "" : paramHolder[9].Replace("%20", " ");
                        monthlyConsolidatedReport.Parameters["FromDate"].Value = paramHolder[0];
                        monthlyConsolidatedReport.Parameters["ToDate"].Value = paramHolder[1];
                        monthlyConsolidatedReport.Parameters["FromAccount"].Value = paramHolder[2];
                        monthlyConsolidatedReport.Parameters["ToAccount"].Value = paramHolder[3];
                        monthlyConsolidatedReport.Parameters["FinancePoint"].Value = Financepoint;
                        string monthlyConsolidatedReportJson = JsonConvert.SerializeObject(ReportDataHandlerBase.GetMonthlyConsolidatedReportData(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1]), paramHolder[2], paramHolder[3], paramHolder[4], Convert.ToInt32(paramHolder[6]), Convert.ToInt32(paramHolder[7]), Convert.ToInt32(paramHolder[5])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)monthlyConsolidatedReport.DataSource).JsonSource).Json = monthlyConsolidatedReportJson;
                        monthlyConsolidatedReport.SaveLayoutToXml(ms);
                        break;
                    case "AssemblyStock":
                        initPointInfo();
                        XtraReport assemblyStock = new Reports.Inventory.AssemblyStock();
                        string assemblyStockJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetStockAssembly(0, paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)assemblyStock.DataSource).JsonSource).Json = assemblyStockJson;
                        assemblyStock.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        assemblyStock.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        assemblyStock.Parameters["CompanyName"].Value = CompanyName;
                        assemblyStock.Parameters["TenantId"].Value = tenantId;
                        assemblyStock.Parameters["Address"].Value = Address;
                        assemblyStock.Parameters["Address2"].Value = Address2;
                        assemblyStock.Parameters["Phone"].Value = Phone;
                        assemblyStock.Parameters["InventoryPoint"].Value = Inventorypoint;
                        assemblyStock.SaveLayoutToXml(ms);
                        break;
                    case "assemblyCost":
                        initPointInfo();
                        XtraReport assemblyCost = new Reports.Inventory.AssemblyCost();
                        paramHolder[0] = paramHolder[0];
                        string assemblyCostJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetStockAssemblyForCost(0, paramHolder[0]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)assemblyCost.DataSource).JsonSource).Json = assemblyCostJson;
                        //assemblyCost.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        //assemblyCost.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        assemblyCost.Parameters["CompanyName"].Value = CompanyName;
                        assemblyCost.Parameters["TenantId"].Value = tenantId;
                        assemblyCost.Parameters["Address"].Value = Address;
                        assemblyCost.Parameters["Address2"].Value = Address2;
                        assemblyCost.Parameters["Phone"].Value = Phone;
                        assemblyCost.Parameters["InventoryPoint"].Value = Inventorypoint;
                        assemblyCost.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalance":
                        XtraReport trialBalance = new Reports.Finance.TrialBalance();
                        string trialBalanceJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)trialBalance.DataSource).JsonSource).Json = trialBalanceJson;
                        trialBalance.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        trialBalance.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        //trialBalance.Parameters["FromDate"].Value = DateTime.ParseExact(paramHolder[0], "dd/mm/yyyy", CultureInfo.InvariantCulture);
                        //trialBalance.Parameters["ToDate"].Value = DateTime.ParseExact(paramHolder[1], "dd/mm/yyyy", CultureInfo.InvariantCulture);
                        trialBalance.Parameters["Status"].Value = paramHolder[5];
                        trialBalance.Parameters["FromAcc"].Value = paramHolder[4];
                        trialBalance.Parameters["ToAcc"].Value = paramHolder[3];
                        trialBalance.Parameters["TenantId"].Value = tenantId;
                        trialBalance.Parameters["Address"].Value = Address;
                        trialBalance.Parameters["Address2"].Value = Address2;
                        trialBalance.Parameters["Phone"].Value = Phone;
                        trialBalance.Parameters["CompanyName"].Value = CompanyName;
                        trialBalance.Parameters["includeLevel3"].Value = paramHolder[6];
                        trialBalance.Parameters["cur"].Value = paramHolder[9];
                        trialBalance.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalance1":
                        initPointInfo();
                        XtraReport trialBalance1 = new Reports.Finance.TrialBalance1();
                        string trialBalanceJson1 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)trialBalance1.DataSource).JsonSource).Json = trialBalanceJson1;
                        trialBalance1.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        trialBalance1.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        trialBalance1.Parameters["Status"].Value = paramHolder[5];
                        trialBalance1.Parameters["FromAcc"].Value = paramHolder[4];
                        trialBalance1.Parameters["ToAcc"].Value = paramHolder[3];
                        trialBalance1.Parameters["TenantId"].Value = tenantId;
                        trialBalance1.Parameters["Address"].Value = Address;
                        trialBalance1.Parameters["Address2"].Value = Address2;
                        trialBalance1.Parameters["Phone"].Value = Phone;
                        trialBalance1.Parameters["CompanyName"].Value = CompanyName;
                        trialBalance1.Parameters["cur"].Value = paramHolder[9];
                        trialBalance1.Parameters["FinancePoint"].Value = Financepoint;
                        trialBalance1.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalance2":
                        XtraReport trialBalance2 = new Reports.Finance.TrialBalance2();
                        string trialBalanceJson2 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)trialBalance2.DataSource).JsonSource).Json = trialBalanceJson2;
                        trialBalance2.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        trialBalance2.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        trialBalance2.Parameters["Status"].Value = paramHolder[5];
                        trialBalance2.Parameters["FromAcc"].Value = paramHolder[4];
                        trialBalance2.Parameters["ToAcc"].Value = paramHolder[3];
                        trialBalance2.Parameters["TenantId"].Value = tenantId;
                        trialBalance2.Parameters["Address"].Value = Address;
                        trialBalance2.Parameters["Address2"].Value = Address2;
                        trialBalance2.Parameters["CompanyName"].Value = CompanyName;
                        trialBalance2.Parameters["Phone"].Value = Phone;
                        trialBalance2.Parameters["cur"].Value = paramHolder[9];
                        trialBalance2.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalance3":
                        XtraReport trialBalance3 = new Reports.Finance.TrialBalance3();
                        string trialBalanceJson3 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)trialBalance3.DataSource).JsonSource).Json = trialBalanceJson3;
                        trialBalance3.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        trialBalance3.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        trialBalance3.Parameters["Status"].Value = paramHolder[5];
                        trialBalance3.Parameters["FromAcc"].Value = paramHolder[4];
                        trialBalance3.Parameters["ToAcc"].Value = paramHolder[3];
                        trialBalance3.Parameters["TenantId"].Value = tenantId;
                        trialBalance3.Parameters["Address"].Value = Address;
                        trialBalance3.Parameters["Address2"].Value = Address2;
                        trialBalance3.Parameters["CompanyName"].Value = CompanyName;
                        trialBalance3.Parameters["Phone"].Value = Phone;
                        trialBalance3.Parameters["cur"].Value = paramHolder[9];
                        trialBalance3.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalanceOpening":
                        XtraReport trialBalanceOpening = new Reports.Finance.TrialBalanceOpening();
                        string trialBalanceOpeningJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)trialBalanceOpening.DataSource).JsonSource).Json = trialBalanceOpeningJson;
                        trialBalanceOpening.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        trialBalanceOpening.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        trialBalanceOpening.Parameters["Status"].Value = paramHolder[5];
                        trialBalanceOpening.Parameters["FromAcc"].Value = paramHolder[4];
                        trialBalanceOpening.Parameters["ToAcc"].Value = paramHolder[3];
                        trialBalanceOpening.Parameters["TenantId"].Value = tenantId;
                        trialBalanceOpening.Parameters["Address"].Value = Address;
                        trialBalanceOpening.Parameters["Address2"].Value = Address2;
                        trialBalanceOpening.Parameters["Phone"].Value = Phone;
                        trialBalanceOpening.Parameters["CompanyName"].Value = CompanyName;
                        trialBalanceOpening.Parameters["includeLevel3"].Value = paramHolder[6];
                        trialBalanceOpening.Parameters["cur"].Value = paramHolder[9];
                        trialBalanceOpening.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalanceOpening1":
                        XtraReport TrialBalanceOpening1 = new Reports.Finance.TrialBalanceOpening1();
                        string TrialBalanceOpening1Json = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)TrialBalanceOpening1.DataSource).JsonSource).Json = TrialBalanceOpening1Json;
                        TrialBalanceOpening1.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        TrialBalanceOpening1.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        TrialBalanceOpening1.Parameters["Status"].Value = paramHolder[5];
                        TrialBalanceOpening1.Parameters["FromAcc"].Value = paramHolder[4];
                        TrialBalanceOpening1.Parameters["ToAcc"].Value = paramHolder[3];
                        TrialBalanceOpening1.Parameters["TenantId"].Value = tenantId;
                        TrialBalanceOpening1.Parameters["Address"].Value = Address;
                        TrialBalanceOpening1.Parameters["Address2"].Value = Address2;
                        TrialBalanceOpening1.Parameters["Phone"].Value = Phone;
                        TrialBalanceOpening1.Parameters["CompanyName"].Value = CompanyName;
                        TrialBalanceOpening1.Parameters["cur"].Value = paramHolder[9];
                        TrialBalanceOpening1.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalanceOpening2":
                        XtraReport TrialBalanceOpening2 = new Reports.Finance.TrialBalanceOpening2();
                        string TrialBalanceOpening2Json = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)TrialBalanceOpening2.DataSource).JsonSource).Json = TrialBalanceOpening2Json;
                        TrialBalanceOpening2.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        TrialBalanceOpening2.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        TrialBalanceOpening2.Parameters["Status"].Value = paramHolder[5];
                        TrialBalanceOpening2.Parameters["FromAcc"].Value = paramHolder[4];
                        TrialBalanceOpening2.Parameters["ToAcc"].Value = paramHolder[3];
                        TrialBalanceOpening2.Parameters["TenantId"].Value = tenantId;
                        TrialBalanceOpening2.Parameters["Address"].Value = Address;
                        TrialBalanceOpening2.Parameters["Address2"].Value = Address2;
                        TrialBalanceOpening2.Parameters["Phone"].Value = Phone;
                        TrialBalanceOpening2.Parameters["CompanyName"].Value = CompanyName;
                        TrialBalanceOpening2.Parameters["cur"].Value = paramHolder[9];
                        TrialBalanceOpening2.SaveLayoutToXml(ms);
                        break;
                    case "TrialBalanceOpening3":
                        XtraReport TrialBalanceOpening3 = new Reports.Finance.TrialBalanceOpening3();
                        string TrialBalanceOpening3Json = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)TrialBalanceOpening3.DataSource).JsonSource).Json = TrialBalanceOpening3Json;
                        TrialBalanceOpening3.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        TrialBalanceOpening3.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        TrialBalanceOpening3.Parameters["Status"].Value = paramHolder[5];
                        TrialBalanceOpening3.Parameters["FromAcc"].Value = paramHolder[4];
                        TrialBalanceOpening3.Parameters["ToAcc"].Value = paramHolder[3];
                        TrialBalanceOpening3.Parameters["TenantId"].Value = tenantId;
                        TrialBalanceOpening3.Parameters["Address"].Value = Address;
                        TrialBalanceOpening3.Parameters["Address2"].Value = Address2;
                        TrialBalanceOpening3.Parameters["CompanyName"].Value = CompanyName;
                        TrialBalanceOpening3.Parameters["Phone"].Value = Phone;
                        TrialBalanceOpening3.Parameters["cur"].Value = paramHolder[9];
                        TrialBalanceOpening3.SaveLayoutToXml(ms);
                        break;
                    case "PartyBalances":
                        initPointInfo();
                        XtraReport PartyBalances = new Reports.Finance.PartyBalances();
                        string PartyBalancesJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTrialBalance(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], Convert.ToInt32(paramHolder[5]), Convert.ToInt32(paramHolder[4]), Convert.ToBoolean(paramHolder[7]), (paramHolder[8])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)PartyBalances.DataSource).JsonSource).Json = PartyBalancesJson;
                        PartyBalances.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        PartyBalances.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        PartyBalances.Parameters["Status"].Value = paramHolder[5];
                        PartyBalances.Parameters["FromAcc"].Value = paramHolder[4];
                        PartyBalances.Parameters["ToAcc"].Value = paramHolder[3];
                        PartyBalances.Parameters["TenantId"].Value = tenantId;
                        PartyBalances.Parameters["Address"].Value = Address;
                        PartyBalances.Parameters["Address2"].Value = Address2;
                        PartyBalances.Parameters["CompanyName"].Value = CompanyName;
                        PartyBalances.Parameters["Phone"].Value = Phone;
                        PartyBalances.Parameters["cur"].Value = paramHolder[9];
                        PartyBalances.Parameters["FinancePoint"].Value = Financepoint;
                        PartyBalances.SaveLayoutToXml(ms);
                        break;

                    case "Finance_TransList":
                        initPointInfo();
                        XtraReport financeTransList = new Reports.Finance.FinanceTransList();
                        string financeTransListJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTransListing(Convert.ToDateTime(paramHolder[0]), Convert.ToDateTime(paramHolder[1]), paramHolder[2], paramHolder[3], 0, paramHolder[4], Convert.ToInt32(paramHolder[5])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)financeTransList.DataSource).JsonSource).Json = financeTransListJson;
                        financeTransList.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        financeTransList.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        financeTransList.Parameters["Status"].Value = paramHolder[4].ToString().Replace("%20", " ");
                        financeTransList.Parameters["TenantId"].Value = tenantId;
                        financeTransList.Parameters["Address"].Value = Address;
                        financeTransList.Parameters["Address2"].Value = Address2;
                        financeTransList.Parameters["CompanyName"].Value = CompanyName;
                        financeTransList.Parameters["Phone"].Value = Phone;
                        financeTransList.Parameters["VoucherType"].Value = paramHolder[6].ToString().Replace("%20", " ");
                        financeTransList.Parameters["User"].Value = UserName;
                        financeTransList.Parameters["Loc"].Value = Convert.ToInt32(paramHolder[5]);
                        financeTransList.Parameters["FinancePoint"].Value = Financepoint;
                        financeTransList.SaveLayoutToXml(ms);
                        break;
                    case "StockTransfer":
                        initPointInfo();
                        XtraReport stockTransferList = new Reports.Inventory.StockTransfer();
                        string stockTransferListson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetStockTransfer(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)stockTransferList.DataSource).JsonSource).Json = stockTransferListson;
                        stockTransferList.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        stockTransferList.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        stockTransferList.Parameters["TenantId"].Value = tenantId;
                        stockTransferList.Parameters["Address"].Value = Address;
                        stockTransferList.Parameters["Address2"].Value = Address2;
                        stockTransferList.Parameters["CompanyName"].Value = CompanyName;
                        stockTransferList.Parameters["Phone"].Value = Phone;
                        stockTransferList.Parameters["InventoryPoint"].Value = Inventorypoint;
                        stockTransferList.SaveLayoutToXml(ms);
                        break;
                    case "InwardGatePass":
                        initPointInfo();
                        XtraReport inwardGatePassList = new Reports.Inventory.InwardGatePass();
                        string inwardGatePassListJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGatePass(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 1));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)inwardGatePassList.DataSource).JsonSource).Json = inwardGatePassListJson;
                        inwardGatePassList.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        inwardGatePassList.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        inwardGatePassList.Parameters["CompanyName"].Value = CompanyName;
                        inwardGatePassList.Parameters["TenantId"].Value = tenantId;
                        inwardGatePassList.Parameters["Address"].Value = Address + "-" + Address2;
                        inwardGatePassList.Parameters["CompanyName"].Value = CompanyName;
                        inwardGatePassList.Parameters["Phone"].Value = Phone;
                        inwardGatePassList.Parameters["InventoryPoint"].Value = Inventorypoint;
                        inwardGatePassList.SaveLayoutToXml(ms);
                        break;
                    case "OutwardGatePass":
                        initPointInfo();
                        XtraReport outwardGatePassList = new Reports.Inventory.OutwardGatePass();
                        string outwardGatePassListJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetGatePass(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 2));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)outwardGatePassList.DataSource).JsonSource).Json = outwardGatePassListJson;
                        outwardGatePassList.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        outwardGatePassList.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        outwardGatePassList.Parameters["TenantId"].Value = tenantId;
                        outwardGatePassList.Parameters["CompanyName"].Value = CompanyName;
                        outwardGatePassList.Parameters["Phone"].Value = Phone;
                        outwardGatePassList.Parameters["Address"].Value = Address + "-" + Address2;
                        outwardGatePassList.Parameters["InventoryPoint"].Value = Inventorypoint;
                        outwardGatePassList.SaveLayoutToXml(ms);
                        break;

                    case "BankReconcileReport":
                        initPointInfo();
                        string bankID = paramHolder[0];
                        string fromDocID = paramHolder[1];
                        string toDocID = paramHolder[2];
                        XtraReport bankReconcileReport = new Reports.Finance.BankReconcileReport();
                        string bankReconcileReportjson = JsonConvert.SerializeObject(
                        ReportDataHandlerBase.GetBankReconcileReport(null, bankID, fromDocID, toDocID));

                        bankReconcileReport.Parameters["CompanyName"].Value = CompanyName;
                        bankReconcileReport.Parameters["Address"].Value = Address;
                        bankReconcileReport.Parameters["Address2"].Value = Address2;
                        bankReconcileReport.Parameters["TenantId"].Value = tenantId;
                        bankReconcileReport.Parameters["FinancePoint"].Value = Financepoint;
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)bankReconcileReport.DataSource).JsonSource).Json = bankReconcileReportjson;
                        bankReconcileReport.SaveLayoutToXml(ms);
                        break;
                    case "BankReconcilationReport":
                        initPointInfo();
                        bankID = paramHolder[0];
                        fromDate = paramHolder[1];
                        toDate = paramHolder[2];
                        XtraReport reoncilationReport = new Reports.Finance.BankReconcilationReport();
                        string reoncilationReportjson = JsonConvert.SerializeObject(
                        ReportDataHandlerBase.GetBankReconcilationReport(null, bankID, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate)));


                        reoncilationReport.Parameters["BankId"].Value = bankID;
                        reoncilationReport.Parameters["BankName"].Value = paramHolder[3].Replace("%20", " ");
                        reoncilationReport.Parameters["CompanyName"].Value = CompanyName;
                        reoncilationReport.Parameters["Address"].Value = Address;
                        reoncilationReport.Parameters["Address2"].Value = Address2;
                        reoncilationReport.Parameters["Phone"].Value = Phone;
                        reoncilationReport.Parameters["TenantId"].Value = tenantId;
                        reoncilationReport.Parameters["FromDateStr"].Value = fromDate;
                        reoncilationReport.Parameters["ToDateStr"].Value = toDate;
                        reoncilationReport.Parameters["FromDate"].Value = fromDate;
                        reoncilationReport.Parameters["ToDate"].Value = toDate;
                        reoncilationReport.Parameters["FinancePoint"].Value = Financepoint;
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)reoncilationReport.DataSource).JsonSource).Json = reoncilationReportjson;
                        reoncilationReport.SaveLayoutToXml(ms);
                        break;
                    case "Requisition":
                        initPointInfo();
                        XtraReport requisitionList = new Reports.Purchase.Requisition();
                        string requisitionListJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetRequisitionReport(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 0));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)requisitionList.DataSource).JsonSource).Json = requisitionListJson;
                        requisitionList.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        requisitionList.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        requisitionList.Parameters["CompanyName"].Value = CompanyName;
                        requisitionList.Parameters["Phone"].Value = Phone;
                        requisitionList.Parameters["Address"].Value = Address + "-" + Address2;
                        requisitionList.Parameters["TenantId"].Value = tenantId;
                        requisitionList.Parameters["InventoryPoint"].Value = Inventorypoint;
                        requisitionList.SaveLayoutToXml(ms);
                        break;
                    case "Grouprequisition":
                        initPointInfo();
                        XtraReport requisitionList1 = new Reports.Purchase.GroupRequisition();
                        string requisitionListJson1 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetRequisitionReport(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 0));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)requisitionList1.DataSource).JsonSource).Json = requisitionListJson1;
                        requisitionList1.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        requisitionList1.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        requisitionList1.Parameters["CompanyName"].Value = CompanyName;
                        requisitionList1.Parameters["Phone"].Value = Phone;
                        requisitionList1.Parameters["Address"].Value = Address + "-" + Address2;
                        requisitionList1.Parameters["TenantId"].Value = tenantId;
                        requisitionList1.Parameters["InventoryPoint"].Value = Inventorypoint;
                        requisitionList1.SaveLayoutToXml(ms);
                        break;

                    case "ReqOrderStatus":
                        initPointInfo();
                        XtraReport reqOrderStatusList = new Reports.Purchase.ReqOrderStatus();
                        string reqOrderStatusJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetRequisitionStatusReport(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 0));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)reqOrderStatusList.DataSource).JsonSource).Json = reqOrderStatusJson;
                        reqOrderStatusList.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        reqOrderStatusList.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        reqOrderStatusList.Parameters["CompanyName"].Value = CompanyName;
                        reqOrderStatusList.Parameters["Address"].Value = Address;
                        reqOrderStatusList.Parameters["Address2"].Value = Address2;
                        reqOrderStatusList.Parameters["Phone"].Value = Phone;
                        reqOrderStatusList.Parameters["TenantId"].Value = tenantId;
                        reqOrderStatusList.Parameters["InventoryPoint"].Value = Inventorypoint;
                        reqOrderStatusList.SaveLayoutToXml(ms);
                        break;
                    case "PurchaseOrder":
                        initPointInfo();
                        XtraReport purchaseOrder = new Reports.Purchase.PurchaseOrder();
                        string purchaseOrderJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetPurchaseOrderReport(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 0, paramHolder[2], paramHolder[3]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)purchaseOrder.DataSource).JsonSource).Json = purchaseOrderJson;
                        purchaseOrder.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        purchaseOrder.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        purchaseOrder.Parameters["CompanyName"].Value = CompanyName;
                        purchaseOrder.Parameters["TenantId"].Value = tenantId;
                        purchaseOrder.Parameters["Address"].Value = Address + "-" + Address2;
                        purchaseOrder.Parameters["Phone"].Value = Phone;
                        purchaseOrder.Parameters["InventoryPoint"].Value = Inventorypoint;
                        purchaseOrder.SaveLayoutToXml(ms);
                        break;
                    case "PurchaseOrderStatus":
                        initPointInfo();
                        XtraReport purchaseOrderStatus = new Reports.Purchase.PurchaseOrderStatus();
                        string purchaseOrderStatusJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetPurchaseOrderStatusReport(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)purchaseOrderStatus.DataSource).JsonSource).Json = purchaseOrderStatusJson;
                        purchaseOrderStatus.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        purchaseOrderStatus.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        purchaseOrderStatus.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[2]);
                        purchaseOrderStatus.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[3]);
                        purchaseOrderStatus.Parameters["FromArrival"].Value = Convert.ToDateTime(paramHolder[4]);
                        purchaseOrderStatus.Parameters["ToArrival"].Value = Convert.ToDateTime(paramHolder[5]);
                        purchaseOrderStatus.Parameters["CompanyName"].Value = CompanyName;
                        purchaseOrderStatus.Parameters["TenantId"].Value = tenantId;
                        purchaseOrderStatus.Parameters["Address"].Value = Address;
                        purchaseOrderStatus.Parameters["Address2"].Value = Address2;
                        purchaseOrderStatus.Parameters["Phone"].Value = Phone;
                        purchaseOrderStatus.Parameters["InventoryPoint"].Value = Inventorypoint;
                        purchaseOrderStatus.SaveLayoutToXml(ms);
                        break;
                    case "Receipt":
                        initPointInfo();
                        XtraReport receipt = null;
                        if(paramHolder[2] == "true")
                            receipt = new Reports.Purchase.Receipt();
                        else
                            receipt = new Reports.Purchase.ReceiptWithoutRates();
                        string receiptJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetReceiptReportData(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 0));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)receipt.DataSource).JsonSource).Json = receiptJson;
                        receipt.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        receipt.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        receipt.Parameters["CompanyName"].Value = CompanyName;
                        receipt.Parameters["Address"].Value = Address + "-" + Address2;
                        receipt.Parameters["TenantId"].Value = tenantId;
                        receipt.Parameters["Phone"].Value = Phone;
                        receipt.Parameters["InventoryPoint"].Value = Inventorypoint;
                        receipt.SaveLayoutToXml(ms);
                        break;
                    case "ReceiptReturn":
                        initPointInfo();
                        XtraReport receiptReturn = null;
                        if (paramHolder[2] == "true")
                            receiptReturn = new Reports.Purchase.ReceiptReturn();
                        else
                            receiptReturn = new Reports.Purchase.ReceiptReturnWithoutRates();
                        string receiptReturnJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetReceiptReturnReportData(0, Convert.ToInt32(paramHolder[0]), Convert.ToInt32(paramHolder[1]), 0));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)receiptReturn.DataSource).JsonSource).Json = receiptReturnJson;
                        receiptReturn.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[0]);
                        receiptReturn.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[1]);
                        receiptReturn.Parameters["CompanyName"].Value = CompanyName;
                        receiptReturn.Parameters["TenantId"].Value = tenantId;
                        receiptReturn.Parameters["Address"].Value = Address + "-" + Address2;
                        receiptReturn.Parameters["Phone"].Value = Phone;
                        receiptReturn.Parameters["InventoryPoint"].Value = Inventorypoint;
                        receiptReturn.SaveLayoutToXml(ms);
                        break;
                    case "TransferRegister":
                        initPointInfo();
                        XtraReport transferRegister = new Reports.Inventory.TransferRegister();
                        string transferRegisterJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTransferRegisterData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3]), Convert.ToDateTime(paramHolder[0]), Convert.ToDateTime(paramHolder[1])
                                                    , paramHolder[4], paramHolder[5]
                                                    ));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)transferRegister.DataSource).JsonSource).Json = transferRegisterJson;
                        transferRegister.Parameters["CompanyName"].Value = CompanyName;
                        transferRegister.Parameters["TenantId"].Value = tenantId;
                        transferRegister.Parameters["Address"].Value = Address;
                        transferRegister.Parameters["Address2"].Value = Address2;
                        transferRegister.Parameters["Phone"].Value = Phone;
                        transferRegister.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        transferRegister.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        transferRegister.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                        transferRegister.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                        transferRegister.Parameters["InventoryPoint"].Value = Inventorypoint;
                        transferRegister.Parameters["Phone"].Value = Phone;
                        transferRegister.SaveLayoutToXml(ms);
                        break;
                    case "AssemblyRegister":
                        initPointInfo();
                        XtraReport assemblyRegister = new Reports.Inventory.AssemblyRegister();
                        assemblyRegister.Parameters["CompanyName"].Value = CompanyName;
                        assemblyRegister.Parameters["TenantId"].Value = tenantId;
                        assemblyRegister.Parameters["Address"].Value = Address;
                        assemblyRegister.Parameters["Address2"].Value = Address2;
                        assemblyRegister.Parameters["Phone"].Value = Phone;
                        assemblyRegister.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        assemblyRegister.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        assemblyRegister.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                        assemblyRegister.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                        assemblyRegister.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string assemblyRegisterJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAssemblyRegisterData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3]), (paramHolder[0]), (paramHolder[1])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)assemblyRegister.DataSource).JsonSource).Json = assemblyRegisterJson;

                        assemblyRegister.SaveLayoutToXml(ms);
                        break;
                    case "InwardGatePassRegister":
                        initPointInfo();
                        XtraReport inwardGatePassRegister = new Reports.Inventory.InwardGatePassRegister();
                        inwardGatePassRegister.Parameters["CompanyName"].Value = CompanyName;
                        inwardGatePassRegister.Parameters["TenantId"].Value = tenantId;
                        inwardGatePassRegister.Parameters["Address"].Value = Address;
                        inwardGatePassRegister.Parameters["Address2"].Value = Address2;
                        inwardGatePassRegister.Parameters["Phone"].Value = Phone;
                        //inwardGatePassRegister.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        //inwardGatePassRegister.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        inwardGatePassRegister.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                        inwardGatePassRegister.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                        inwardGatePassRegister.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string inwardGatePassRegisterJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetInwardGatePassRegisterData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)inwardGatePassRegister.DataSource).JsonSource).Json = inwardGatePassRegisterJson;
                        inwardGatePassRegister.SaveLayoutToXml(ms);
                        break;
                    case "OutwardGatePassRegister":
                        initPointInfo();
                        XtraReport outwardGatePassRegister = new Reports.Inventory.OutwardGatePassRegister();
                        outwardGatePassRegister.Parameters["CompanyName"].Value = CompanyName;
                        outwardGatePassRegister.Parameters["TenantId"].Value = tenantId;
                        outwardGatePassRegister.Parameters["Address"].Value = Address;
                        outwardGatePassRegister.Parameters["Address2"].Value = Address2;
                        outwardGatePassRegister.Parameters["Phone"].Value = Phone;
                        //outwardGatePassRegister.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        //outwardGatePassRegister.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        outwardGatePassRegister.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                        outwardGatePassRegister.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                        outwardGatePassRegister.Parameters["InventoryPoint"].Value = Inventorypoint;
                        string outwardGatePassRegisterJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetOutwardGatePassRegisterData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)outwardGatePassRegister.DataSource).JsonSource).Json = outwardGatePassRegisterJson;
                        outwardGatePassRegister.SaveLayoutToXml(ms);
                        break;
                    case "Invoice":
                        XtraReport invoice = new Reports.Sales.Invoice();
                        string invoiceJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetInvoiceReportData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3]),
                                                    Convert.ToDateTime(paramHolder[0]), Convert.ToDateTime(paramHolder[1])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)invoice.DataSource).JsonSource).Json = invoiceJson;
                        invoice.Parameters["CompanyName"].Value = CompanyName;
                        invoice.Parameters["Address"].Value = Address + "-" + Address2;
                        invoice.Parameters["Phone"].Value = Phone;
                        invoice.Parameters["TenantId"].Value = tenantId;
                        invoice.SaveLayoutToXml(ms);
                        break;
                    case "InvoiceTax":
                        XtraReport invoiceTax = new Reports.Sales.InvoiceTax();
                        string invoiceTaxJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetInvoiceReportData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3]),
                                                    Convert.ToDateTime(paramHolder[0]), Convert.ToDateTime(paramHolder[1])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)invoiceTax.DataSource).JsonSource).Json = invoiceTaxJson;
                        invoiceTax.Parameters["CompanyName"].Value = CompanyName;
                        invoiceTax.Parameters["Address"].Value = Address + "-" + Address2;
                        invoiceTax.Parameters["PhoneNo"].Value = Phone;
                        invoiceTax.Parameters["SalesTaxReg"].Value = salesTaxReg;
                        invoiceTax.Parameters["Phone"].Value = Phone;
                        invoiceTax.Parameters["TenantId"].Value = tenantId;
                        invoiceTax.SaveLayoutToXml(ms);
                        break;
                    case "SalesRegister":
                        XtraReport salesRegister = new Reports.Sales.SalesRegister();
                        string salesRegisterJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetInvoiceReportData(0, Convert.ToInt32(paramHolder[2]), Convert.ToInt32(paramHolder[3]),
                                                    Convert.ToDateTime(paramHolder[0]), Convert.ToDateTime(paramHolder[1])));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)salesRegister.DataSource).JsonSource).Json = salesRegisterJson;
                        salesRegister.Parameters["CompanyName"].Value = CompanyName;
                        salesRegister.Parameters["Address"].Value = Address;
                        salesRegister.Parameters["Address2"].Value = Address2;
                        salesRegister.Parameters["TenantId"].Value = tenantId;
                        salesRegister.Parameters["Phone"].Value = Phone;

                        //salesRegister.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[0]);
                        //salesRegister.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        //salesRegister.Parameters["FromDoc"].Value = Convert.ToInt32(paramHolder[2]);
                        //salesRegister.Parameters["ToDoc"].Value = Convert.ToInt32(paramHolder[3]);
                        salesRegister.SaveLayoutToXml(ms);
                        break;
                    case "ConsumptionSummaryDepartmentWise":
                        initPointInfo();
                        XtraReport consumptionSummaryDepartmentWiseAppService = new Reports.Inventory.ConsumptionSummaryDepartmentWise();
                        string consumptionSummaryDepartmentWiseAppServiceJson = JsonConvert.SerializeObject(
                        // ReportDataHandlerBase.GetConsumptionDepartmentWiseData(0, paramHolder[0], paramHolder[1]));
                        ReportDataHandlerBase.GetConsumptionDepartmentWiseData(0, paramHolder[0], paramHolder[1], paramHolder[2], paramHolder[3], paramHolder[4], paramHolder[5], "ConsumptionDepartmentWise"));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)consumptionSummaryDepartmentWiseAppService.DataSource).JsonSource).Json = consumptionSummaryDepartmentWiseAppServiceJson;
                        consumptionSummaryDepartmentWiseAppService.Parameters["CompanyName"].Value = CompanyName;
                        consumptionSummaryDepartmentWiseAppService.Parameters["TenantId"].Value = tenantId;
                        consumptionSummaryDepartmentWiseAppService.Parameters["Address"].Value = Address;
                        consumptionSummaryDepartmentWiseAppService.Parameters["Address2"].Value = Address2;
                        consumptionSummaryDepartmentWiseAppService.Parameters["Phone"].Value = Phone;
                        consumptionSummaryDepartmentWiseAppService.Parameters["InventoryPoint"].Value = Inventorypoint;
                        consumptionSummaryDepartmentWiseAppService.SaveLayoutToXml(ms);
                        break;
                    case "ActualAndBudget":
                        initPointInfo();
                        XtraReport actualAndBudget = new Reports.Inventory.ActualAndBudget();
                        string acactualAndBudgetJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetActualAndBudgetData(0, paramHolder[0], paramHolder[1]));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)actualAndBudget.DataSource).JsonSource).Json = acactualAndBudgetJson;
                        actualAndBudget.Parameters["CompanyName"].Value = CompanyName;
                        actualAndBudget.Parameters["TenantId"].Value = tenantId;
                        actualAndBudget.Parameters["FromDate"].Value = paramHolder[0];
                        actualAndBudget.Parameters["ToDate"].Value = paramHolder[1];
                        actualAndBudget.Parameters["Address"].Value = Address;
                        actualAndBudget.Parameters["Address2"].Value = Address2;
                        actualAndBudget.Parameters["Phone"].Value = Phone;
                        actualAndBudget.Parameters["InventoryPoint"].Value = Inventorypoint;
                        actualAndBudget.SaveLayoutToXml(ms);
                        break;

                    case "LCExpenses":
                        initPointInfo();
                        fromDate = paramHolder[0];
                        toDate = paramHolder[1];
                        fromCode = paramHolder[2];
                        toCode = paramHolder[3];
                        XtraReport lcExpenses = new Reports.Finance.LCExpenses();
                        string lcExpensesJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetLCExpensesData(null, fromDate, toDate, fromCode, toCode));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)lcExpenses.DataSource).JsonSource).Json = lcExpensesJson;
                        lcExpenses.Parameters["CompanyName"].Value = CompanyName;
                        lcExpenses.Parameters["TenantId"].Value = tenantId;
                        lcExpenses.Parameters["fromDate"].Value = Convert.ToDateTime(fromDate).ToString("dd/MM/yyyy"); ;
                        lcExpenses.Parameters["toDate"].Value = Convert.ToDateTime(toDate).ToString("dd/MM/yyyy"); ;
                        lcExpenses.Parameters["fromCode"].Value = fromCode;
                        lcExpenses.Parameters["toCode"].Value = toCode;
                        lcExpenses.Parameters["FinancePoint"].Value = Financepoint;
                        lcExpenses.SaveLayoutToXml(ms);
                        break;
                    case "BillWiseAgingReport":
                        initPointInfo();
                        XtraReport rptBillAging = new Reports.Finance.BillWiseAgingReport();
                        DateTime asonDate = DateTime.Parse(paramHolder[0]);

                        string fromAccount = paramHolder[1];
                        string ToAccountaging = paramHolder[2];
                        int fromsub = int.Parse(paramHolder[3]);
                        int toSub = int.Parse(paramHolder[4]);
                        string ag1 = paramHolder[5];
                        string ag2 = paramHolder[6];
                        string ag3 = paramHolder[7];
                        string ag4 = paramHolder[8];
                        string ag5 = paramHolder[9];
                        var agingPeriod1 = "0-" + ag1;
                        var agingPeriod2 = (Convert.ToInt32(ag1) + 1).ToString() + "-" + ag2;
                        var agingPeriod3 = (Convert.ToInt32(ag2) + 1).ToString() + "-" + ag3;
                        var agingPeriod4 = (Convert.ToInt32(ag3) + 1).ToString() + "-" + ag4;

                        rptBillAging.Parameters["CompanyName"].Value = CompanyName;
                        rptBillAging.Parameters["Address"].Value = Address;
                        rptBillAging.Parameters["UserName"].Value = UserName;
                        rptBillAging.Parameters["Phone"].Value = Phone;
                        rptBillAging.Parameters["AsOnDate"].Value = (Convert.ToDateTime(paramHolder[0]).Day).ToString() + "/" +
                            (Convert.ToDateTime(paramHolder[0]).Month).ToString() + "/" +
                            (Convert.ToDateTime(paramHolder[0]).Year).ToString();
                        rptBillAging.Parameters["FromAcc"].Value = fromAccount;
                        rptBillAging.Parameters["ToAcc"].Value = ToAccountaging;
                        rptBillAging.Parameters["AP1"].Value = agingPeriod1;
                        rptBillAging.Parameters["AP2"].Value = agingPeriod2;
                        rptBillAging.Parameters["AP3"].Value = agingPeriod3;
                        rptBillAging.Parameters["AP4"].Value = agingPeriod4;
                        rptBillAging.Parameters["TenantId"].Value = tenantId;
                        rptBillAging.Parameters["FinancePoint"].Value = Financepoint;

                        string rptBillAgingJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetBillWiseAgingReport(asonDate, fromAccount, ToAccountaging, fromsub, toSub, ag1, ag2, ag3, ag4, ag5));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptBillAging.DataSource).JsonSource).Json = rptBillAgingJson;
                        rptBillAging.SaveLayoutToXml(ms);
                        break;
                    //XtraReport rptbillAging = new Reports.Finance.BillWiseAgingReport();
                    //DateTime billAsonDate = DateTime.Parse(paramHolder[0]);

                    //string billFromAccount = paramHolder[1];
                    //string billToAccount = paramHolder[2];
                    //int billFromsub = int.Parse(paramHolder[3]);
                    //int billToSub = int.Parse(paramHolder[4]);
                    //string billTypeId = paramHolder[5];

                    //rptbillAging.Parameters["CompanyName"].Value = CompanyName;
                    //rptbillAging.Parameters["AsOnDate"].Value = Convert.ToDateTime(paramHolder[0]);
                    //rptbillAging.Parameters["FromAcc"].Value = billFromAccount;
                    //rptbillAging.Parameters["ToAcc"].Value = billToAccount;
                    //rptbillAging.Parameters["FromSubAcc"].Value = billFromsub;
                    //rptbillAging.Parameters["ToSubAcc"].Value = billToSub;
                    //rptbillAging.Parameters["TenantId"].Value = tenantId;

                    //string billRptAgingJson = JsonConvert.SerializeObject(
                    //                            ReportDataHandlerBase.GetBillWiseAgingReport(billAsonDate, billFromAccount, billToAccount, billFromsub, billToSub, billTypeId));
                    //((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptbillAging.DataSource).JsonSource).Json = billRptAgingJson;
                    //rptbillAging.SaveLayoutToXml(ms);
                    //break;
                    case "CUSTOMERAGING":
                        initPointInfo();
                        XtraReport rptcusAging = new Reports.Finance.CustomerAging();
                        asonDate = DateTime.Parse(paramHolder[0]);

                        fromAccount = paramHolder[1];
                        ToAccountaging = paramHolder[2];
                        fromsub = int.Parse(paramHolder[3]);
                        toSub = int.Parse(paramHolder[4]);
                        ag1 = paramHolder[5];
                        ag2 = paramHolder[6];
                        ag3 = paramHolder[7];
                        ag4 = paramHolder[8];
                        ag5 = paramHolder[9];
                        agingPeriod1 = "0-" + ag1;
                        agingPeriod2 = (Convert.ToInt32(ag1) + 1).ToString() + "-" + ag2;
                        agingPeriod3 = (Convert.ToInt32(ag2) + 1).ToString() + "-" + ag3;
                        agingPeriod4 = (Convert.ToInt32(ag3) + 1).ToString() + "-" + ag4;

                        rptcusAging.Parameters["CompanyName"].Value = CompanyName;
                        rptcusAging.Parameters["Address"].Value = Address;
                        rptcusAging.Parameters["UserName"].Value = UserName;
                        rptcusAging.Parameters["Phone"].Value = Phone;
                        rptcusAging.Parameters["AsOnDate"].Value = (Convert.ToDateTime(paramHolder[0]).Day).ToString() + "/" +
                            (Convert.ToDateTime(paramHolder[0]).Month).ToString() + "/" +
                            (Convert.ToDateTime(paramHolder[0]).Year).ToString();
                        rptcusAging.Parameters["FromAcc"].Value = fromAccount;
                        rptcusAging.Parameters["ToAcc"].Value = ToAccountaging;
                        rptcusAging.Parameters["AP1"].Value = agingPeriod1;
                        rptcusAging.Parameters["AP2"].Value = agingPeriod2;
                        rptcusAging.Parameters["AP3"].Value = agingPeriod3;
                        rptcusAging.Parameters["AP4"].Value = agingPeriod4;
                        rptcusAging.Parameters["TenantId"].Value = tenantId;
                        rptcusAging.Parameters["FinancePoint"].Value = Financepoint; 

                        string rptcusAgingJson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetCustomerAgingReport(asonDate, fromAccount, ToAccountaging, fromsub, toSub, ag1, ag2, ag3, ag4, ag5));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptcusAging.DataSource).JsonSource).Json = rptcusAgingJson;
                        rptcusAging.SaveLayoutToXml(ms);
                        break;
                    case "ALERTLOG":
                        XtraReport rptAlertLog = new Reports.AlertLog();

                        rptAlertLog.Parameters["CompanyInfo"].Value = CompanyName;
                        rptAlertLog.Parameters["Address"].Value = Address;
                        rptAlertLog.Parameters["Phone"].Value = Phone;
                        rptAlertLog.Parameters["TenantId"].Value = tenantId;


                        string AlertLogJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetAlerLogData());

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptAlertLog.DataSource).JsonSource).Json = AlertLogJson;
                        rptAlertLog.SaveLayoutToXml(ms);

                        break;
                    case "PLSTATMENT":
                        initPointInfo();
                        XtraReport rptPLStatment = new Reports.Finance.PLStatementNew();

                        rptPLStatment.Parameters["CompanyName"].Value = CompanyName;
                        rptPLStatment.Parameters["Tenant"].Value = tenantId;
                        rptPLStatment.Parameters["FromDate"].Value = paramHolder[0];
                        rptPLStatment.Parameters["ToDate"].Value = paramHolder[1];
                        rptPLStatment.Parameters["FinancePoint"].Value = Financepoint;

                        string PLStatmentJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetProfitAndLossReport(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1])));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptPLStatment.DataSource).JsonSource).Json = PLStatmentJson;
                        rptPLStatment.SaveLayoutToXml(ms);

                        break;
                    case "BalanceSheet":
                        initPointInfo();
                        XtraReport rptBalanceSheet = new Reports.Finance.BalanceSheet();

                        rptBalanceSheet.Parameters["CompanyName"].Value = CompanyName;
                        rptBalanceSheet.Parameters["Tenant"].Value = tenantId;
                        rptBalanceSheet.Parameters["FromDate"].Value = paramHolder[0];
                        rptBalanceSheet.Parameters["ToDate"].Value = paramHolder[1];
                        rptBalanceSheet.Parameters["FinancePoint"].Value = Financepoint;

                        string BalanceSheetJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetBalanceSheet(DateTime.Parse(paramHolder[0]), DateTime.Parse(paramHolder[1])));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptBalanceSheet.DataSource).JsonSource).Json = BalanceSheetJson;
                        rptBalanceSheet.SaveLayoutToXml(ms);

                        break;
                    case "PlStatementVoucherDetail":
                        XtraReport rptPlStatementVoucherDetail = new Reports.Finance.PlStatementVoucherDetail();

                        //rptPlStatementVoucherDetail.Parameters["CompanyName"].Value = CompanyName;
                        rptPlStatementVoucherDetail.Parameters["TenantId"].Value = tenantId;
                        //rptPlStatementVoucherDetail.Parameters["FromDate"].Value = paramHolder[0];
                        rptPlStatementVoucherDetail.Parameters["Todate"].Value = paramHolder[1];


                        string PlStatementVoucherDetailJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetPlStatementVoucherDetailReport(paramHolder[2], paramHolder[0], paramHolder[1]));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptPlStatementVoucherDetail.DataSource).JsonSource).Json = PlStatementVoucherDetailJson;
                        rptPlStatementVoucherDetail.SaveLayoutToXml(ms);

                        break;
                    case "PlStatementCategoryDetail":
                        XtraReport rptPlStatementCategoryDetail = new Reports.Finance.PlStatementCategoryDetail();

                        rptPlStatementCategoryDetail.Parameters["TenantId"].Value = tenantId;
                        rptPlStatementCategoryDetail.Parameters["Year"].Value = Convert.ToDateTime(paramHolder[1]).Year;
                        rptPlStatementCategoryDetail.Parameters["PrevYear"].Value = Convert.ToDateTime(paramHolder[1]).Year - 1;

                        string PlStatementCategoryDetailJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetPlStatementCategoryDetail(paramHolder[0], paramHolder[1], paramHolder[2]));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)rptPlStatementCategoryDetail.DataSource).JsonSource).Json = PlStatementCategoryDetailJson;
                        rptPlStatementCategoryDetail.SaveLayoutToXml(ms);

                        break;

                    case "AuditPostingLogs":
                        XtraReport auditPostingLogs = new Reports.AuditPostingLogs();

                        auditPostingLogs.Parameters["TenantId"].Value = tenantId;
                        auditPostingLogs.Parameters["FromDate"].Value = Convert.ToDateTime(paramHolder[1]);
                        auditPostingLogs.Parameters["ToDate"].Value = Convert.ToDateTime(paramHolder[2]);

                        string auditPostingLogsJson = JsonConvert.SerializeObject(
                                                   ReportDataHandlerBase.GetAuditPostingLogsReport(paramHolder[0], Convert.ToDateTime(paramHolder[1]), Convert.ToDateTime(paramHolder[2]), paramHolder[3]));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)auditPostingLogs.DataSource).JsonSource).Json = auditPostingLogsJson;
                        auditPostingLogs.SaveLayoutToXml(ms);

                        break;
                    case "TaxCertificate":
                        fromEmpID = paramHolder[2];
                        salaryYear = Convert.ToInt16(paramHolder[0]);
                        salaryMonth = Convert.ToInt16(paramHolder[1]);
                        var toSalaryMonth = Convert.ToInt16(paramHolder[4]);
                        var toSalaryYear = Convert.ToInt16(paramHolder[3]);
                        XtraReport taxCertificate = new Reports.PayRoll.TaxCertificate();

                        taxCertificate.Parameters["CompanyName"].Value = CompanyName;
                        taxCertificate.Parameters["Address"].Value = Address;
                        taxCertificate.Parameters["Phone"].Value = Phone;
                        taxCertificate.Parameters["TenantId"].Value = tenantId;
                        taxCertificate.Parameters["Address2"].Value = Address2;

                        string taxCertificatejson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetTaxCertificateReport(null, fromEmpID, salaryYear, salaryMonth, toSalaryYear, toSalaryMonth));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)taxCertificate.DataSource).JsonSource).Json = taxCertificatejson;
                        taxCertificate.SaveLayoutToXml(ms);
                        break;
                    case "":
                        XtraReport itemlisting1 = new Reports.Inventory.ItemListing();
                        itemlisting1.Parameters["CompanyName"].Value = CompanyName;
                        itemlisting1.Parameters["Address"].Value = Address;
                        itemlisting1.Parameters["Phone"].Value = Phone;
                        itemlisting1.Parameters["TenantId"].Value = tenantId;

                        //string itemlistjson1 = JsonConvert.SerializeObject(
                        //                            ReportDataHandlerBase.GetItemListings(null));
                        //((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)itemlisting1.DataSource).JsonSource).Json = itemlistjson1;
                        itemlisting1.SaveLayoutToXml(ms);
                        break;


                    case "DeleteLog":

                        string FormName = paramHolder[0];
                        //int  DocNo = Convert.ToInt32(paramHolder[1]);

                        XtraReport DeleteLog = new Reports.DeleteLog();
                        DeleteLog.Parameters["CompanyName"].Value = CompanyName;
                        DeleteLog.Parameters["Address"].Value = Address;
                        DeleteLog.Parameters["Phone"].Value = Phone;
                        DeleteLog.Parameters["TenantId"].Value = tenantId;
                        string Logjson = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetDeleteLogListing(0, FormName));
                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)DeleteLog.DataSource).JsonSource).Json = Logjson;
                        DeleteLog.SaveLayoutToXml(ms);
                        break;

                    default:
                        break;
                }

                return ms.ToArray();
            }
        }

        private void initParameters(string urlBase)
        {
            try
            {
                paramHolder = new List<string>();
                if (urlBase.IndexOf("?") != -1)
                {
                    string[] splPar = urlBase.Split("?");

                    foreach (var item in splPar[1].Split("$"))
                    {
                        paramHolder.Add(item);
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception("Invalid Parameters");
            }
        }

        private void initCompanyInfo()
        {
            try
            {
                var companyInfo = ReportDataHandlerBase.GetCompanyInfo();
                CompanyName = companyInfo[0].CompanyName;
                Address = companyInfo[0].Address;
                Address2 = companyInfo[0].Address2;
                Phone = companyInfo[0].Phone;
                salesTaxReg = companyInfo[0].SalesTaxRegNo;
                tenantId = companyInfo[0].TenantId;
                UserName = companyInfo[0].UserName;
            }
            catch (Exception ex)
            {

                throw new Exception("Invalid Parameters");
            }
        }
        private void initPointInfo()
        {
            try
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"];
                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetDecimalPlaces", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenantID", tenantId);
                        cn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Inventorypoint = rdr["InventoryPoint"].ToString();
                                Financepoint = rdr["FinancePoint"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Parameters");
            }
        }
        public override Dictionary<string, string> GetUrls()
        {
            return new Dictionary<string, string>() {
                {"AUDITLOG", "AUDITLOG" }
            };
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            base.SetData(report, url);
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            return base.SetNewData(report, defaultUrl);
        }

    }
}
