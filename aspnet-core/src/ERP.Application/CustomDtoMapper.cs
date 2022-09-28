using ERP.SupplyChain.Sales.OEDrivers.Dtos;
using ERP.SupplyChain.Sales.OEDrivers;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.AccountReceivables.RouteInvoices;
using ERP.SupplyChain.Sales.OERoutes.Dtos;
using ERP.SupplyChain.Sales.OERoutes;
using ERP.GeneralLedger.SetupForms.GLDocRev.Dtos;
using ERP.GeneralLedger.SetupForms.GLDocRev;
using ERP.SupplyChain.Sales.Invoices.Dtos;
using ERP.SupplyChain.Sales.Invoices;
using ERP.PayRoll.hrmSetup.Dtos;
using ERP.PayRoll.hrmSetup;
using ERP.CommonServices.UserLoc.CSUserLocD.Dtos;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.CommonServices.UserLoc.CSUserLocH.Dtos;
using ERP.CommonServices.UserLoc.CSUserLocH;
using ERP.GeneralLedger.SetupForms.GLSLGroups.Dtos;
using ERP.GeneralLedger.SetupForms.GLSLGroups;
using ERP.PayRoll.CaderMaster.cader_link_D.Dtos;
using ERP.PayRoll.CaderMaster.cader_link_D;
using ERP.PayRoll.CaderMaster.cader_link_H.Dtos;
using ERP.PayRoll.CaderMaster.cader_link_H;
using ERP.PayRoll.Cader.Dtos;
using ERP.PayRoll.Cader;
using ERP.SupplyChain.Sales.OECSD.Dtos;
using ERP.SupplyChain.Sales.OECSD;
using ERP.SupplyChain.Sales.OECSH.Dtos;
using ERP.SupplyChain.Sales.OECSH;
using ERP.PayRoll.MonthlyCPR.Dtos;
using ERP.PayRoll.MonthlyCPR;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.SupplyChain.Sales.SaleQutation;
using ERP.SupplyChain.Inventory.ICLOT.Dtos;
using ERP.SupplyChain.Inventory.ICLOT;
using ERP.PayRoll.SalaryLock.Dtos;
using ERP.PayRoll.SalaryLock;
//using ERP.SupplyChain.Purchase.PorcDetailForEdit.Dtos;
//using ERP.SupplyChain.Purchase.PorcDetailForEdit;
using ERP.SupplyChain.Purchase.APINVH.Dtos;
using ERP.SupplyChain.Purchase.APINVH;
using ERP.Payroll.SlabSetup;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Manufacturing.SetupForms;
using ERP.Payroll.SlabSetup.Dtos;
using ERP.PayRoll.Dtos;
using ERP.PayRoll;
using ERP.Payroll.EmployeeLeaveBalance.Dtos;
using ERP.Payroll.EmployeeLeaveBalance;
using ERP.PayRoll.Adjustments.Dtos;
using ERP.PayRoll.Adjustments;
using ERP.PayRoll.StopSalary.Dtos;
using ERP.PayRoll.EmployeeLoansType.Dtos;
using ERP.PayRoll.EmployeeLoans.Dtos;
using ERP.Common.AuditPostingLogs.Dtos;
using ERP.PayRoll.SubDesignations.Dtos;
using ERP.PayRoll.SubDesignations;

using ERP.PayRoll.Allowances.Dtos;
using ERP.PayRoll.Allowances;
using ERP.PayRoll.AllowanceSetup.Dtos;
using ERP.PayRoll.AllowanceSetup;
using ERP.PayRoll.Holidays.Dtos;
using ERP.PayRoll.Holidays;
using ERP.PayRoll.EarningTypes.Dtos;
using ERP.PayRoll.EarningTypes;
using ERP.PayRoll.DeductionTypes.Dtos;
using ERP.PayRoll.DeductionTypes;
using ERP.AccountReceivables.Dtos;
using ERP.AccountReceivables;
using ERP.CommonServices.RecurringVoucher.Dtos;
using ERP.CommonServices.ChequeBooks.Dtos;
using ERP.CommonServices.ChequeBooks;
using ERP.Common.AlertLog.Dtos;
using ERP.Common.AlertLog;
using ERP.SupplyChain.Inventory.WorkOrder.Dtos;
using ERP.SupplyChain.Inventory.WorkOrder;
using ERP.SupplyChain.Sales.SalesReference.Dtos;
using ERP.SupplyChain.Sales.SalesReference;
using ERP.PayRoll.SalarySheet.Dtos;
using ERP.PayRoll.SalarySheet;
using ERP.SupplyChain.Sales.SaleReturn.Dtos;
using ERP.SupplyChain.Sales.SaleReturn;
using ERP.GeneralLedger.SetupForms.LedgerType.Dtos;
using ERP.SupplyChain.Sales.Dtos;
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using ERP.SupplyChain.Sales.SaleEntry;
using ERP.SupplyChain.Inventory.AssetRegistration.Dtos;
using ERP.SupplyChain.Inventory.AssetRegistration;
using ERP.PayRoll.Attendance.Dtos;
using ERP.PayRoll.Attendance;
using ERP.PayRoll.EmployeeSalary.Dtos;
using ERP.PayRoll.EmployeeSalary;
using ERP.PayRoll.Education.Dtos;
using ERP.PayRoll.Education;
using ERP.PayRoll.EmployeeDeductions.Dtos;
using ERP.PayRoll.EmployeeDeductions;
using ERP.PayRoll.Employees.Dtos;
using ERP.PayRoll.Employees;
using ERP.PayRoll.EmployeeEarnings.Dtos;
using ERP.PayRoll.EmployeeEarnings;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using ERP.SupplyChain.Purchase.ReceiptReturn.Dtos;
using ERP.SupplyChain.Purchase.Dtos;
using ERP.PayRoll.Section.Dtos;
using ERP.PayRoll.Section;
using ERP.PayRoll.Department.Dtos;
using ERP.PayRoll.Department;
using ERP.SupplyChain.Purchase.PurchaseOrder.Dtos;
using ERP.SupplyChain.Purchase.PurchaseOrder;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using ERP.SupplyChain.Purchase.Requisition;
using ERP.SupplyChain.Inventory.Opening.Dtos;
using ERP.SupplyChain.Inventory.Opening;
using ERP.SupplyChain.Inventory.ICOPT5.Dtos;
using ERP.SupplyChain.Inventory.ICOPT5;
using ERP.SupplyChain.Inventory.ICOPT4.Dtos;
using ERP.SupplyChain.Inventory.ICOPT4;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.SupplyChain.Inventory.Adjustment;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.GeneralLedger.Transaction.Dtos;
using ERP.GeneralLedger.Transaction;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.AccountPayables.Dtos;
using ERP.AccountPayables;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.CommonServices.Dtos;
using ERP.CommonServices;

using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using ERP.Auditing.Dto;
using ERP.Authorization.Accounts.Dto;
using ERP.Authorization.Permissions.Dto;
using ERP.Authorization.Roles;
using ERP.Authorization.Roles.Dto;
using ERP.Authorization.Users;
using ERP.Authorization.Users.Dto;
using ERP.Authorization.Users.Importing.Dto;
using ERP.Authorization.Users.Profile.Dto;
using ERP.Chat;
using ERP.Chat.Dto;
using ERP.Editions;
using ERP.Editions.Dto;
using ERP.Friendships;
using ERP.Friendships.Cache;
using ERP.Friendships.Dto;
using ERP.Localization.Dto;
using ERP.MultiTenancy;
using ERP.MultiTenancy.Dto;
using ERP.MultiTenancy.HostDashboard.Dto;
using ERP.MultiTenancy.Payments;
using ERP.MultiTenancy.Payments.Dto;
using ERP.Notifications.Dto;
using ERP.Organizations.Dto;
using ERP.Sessions.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.GeneralLedger.DirectInvoice.Dtos;
using ERP.GeneralLedger.DirectInvoice;
using ERP.SupplyChain.Inventory.IC_Item.Dto;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.Consumption.Dtos;
using ERP.SupplyChain.Inventory.Consumption;
using ERP.SupplyChain.Inventory.IC_UNIT.Dto;
using ERP.SupplyChain.Inventory.IC_UNIT;
using ERP.GeneralLedger.Transaction.BankReconcile;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.SupplyChain.Purchase;
using ERP.PayRoll.Designation.Dtos;
using ERP.PayRoll.Designation;
using ERP.SupplyChain.Purchase.ReceiptReturn;
using ERP.PayRoll.Shifts.Dtos;
using ERP.PayRoll.Shifts;
using ERP.PayRoll.Grades.Dtos;
using ERP.PayRoll.Grades;
using ERP.PayRoll.Location.Dtos;
using ERP.PayRoll.Location;
using ERP.PayRoll.EmployeeType.Dtos;
using ERP.PayRoll.EmployeeType;
using ERP.PayRoll.EmployeeArrears.Dtos;
using ERP.PayRoll.EmployeeArrears;
using ERP.PayRoll.Religion.Dtos;
using ERP.PayRoll.Religion;
using ERP.PayRoll.EmployeeLeaves.Dtos;
using ERP.PayRoll.EmployeeLeaves;
using ERP.SupplyChain.Sales.SaleAccounts.Dtos;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.SupplyChain.Sales.CreditDebitNote;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;
using ERP.Common.CSAlertInfo.Dtos;
using ERP.Common.CSAlertInfo;
using ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos;
using ERP.GeneralLedger.SetupForms.GLPLCategory;
using ERP.GeneralLedger.SetupForms.LCExpenses.Dtos;
using ERP.GeneralLedger.SetupForms.LCExpenses;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;
using ERP.GeneralLedger.SetupForms.LCExpensesHD;
using ERP.AccountPayables.CRDRNote.Dtos;
using ERP.AccountPayables.CRDRNote;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto;
using ERP.GeneralLedger.Transaction.GLTransfer.Dtos;
using ERP.GeneralLedger.Transaction.GLTransfer;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing;

namespace ERP
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditOEDriversDto, SupplyChain.Sales.OEDrivers.OEDrivers>().ReverseMap();
            configuration.CreateMap<OEDriversDto, SupplyChain.Sales.OEDrivers.OEDrivers>().ReverseMap();
            configuration.CreateMap<CreateOrEditARINVDDto, ARINVD>().ReverseMap();
            configuration.CreateMap<ARINVDDto, ARINVD>().ReverseMap();
            configuration.CreateMap<CreateOrEditARINVHDto, ARINVH>().ReverseMap();
            configuration.CreateMap<ARINVHDto, ARINVH>().ReverseMap();
            configuration.CreateMap<CreateOrEditOERoutesDto, SupplyChain.Sales.OERoutes.OERoutes>().ReverseMap();
            configuration.CreateMap<OERoutesDto, SupplyChain.Sales.OERoutes.OERoutes>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLDocRevDto, GeneralLedger.SetupForms.GLDocRev.GLDocRev>().ReverseMap();
            configuration.CreateMap<GLDocRevDto, GeneralLedger.SetupForms.GLDocRev.GLDocRev>().ReverseMap();
            configuration.CreateMap<CreateOrEditOEINVKNOCKDDto, OEINVKNOCKD>().ReverseMap();
            configuration.CreateMap<OEINVKNOCKDDto, OEINVKNOCKD>().ReverseMap();
            configuration.CreateMap<CreateOrEditOEINVKNOCKHDto, OEINVKNOCKH>().ReverseMap();
            configuration.CreateMap<OEINVKNOCKHDto, OEINVKNOCKH>().ReverseMap();
            configuration.CreateMap<CreateOrEditHrmSetupDto, HrmSetup>().ReverseMap();
            configuration.CreateMap<HrmSetupDto, HrmSetup>().ReverseMap();
            configuration.CreateMap<CreateOrEditCSUserLocDDto, CommonServices.UserLoc.CSUserLocD.CSUserLocD>().ReverseMap();
            configuration.CreateMap<CSUserLocDDto, CommonServices.UserLoc.CSUserLocD.CSUserLocD>().ReverseMap();
            configuration.CreateMap<CreateOrEditCSUserLocHDto, CommonServices.UserLoc.CSUserLocH.CSUserLocH>().ReverseMap();
            configuration.CreateMap<CSUserLocHDto, CommonServices.UserLoc.CSUserLocH.CSUserLocH>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLSLGroupsDto, GeneralLedger.SetupForms.GLSLGroups.GLSLGroups>().ReverseMap();
            configuration.CreateMap<GLSLGroupsDto, GeneralLedger.SetupForms.GLSLGroups.GLSLGroups>().ReverseMap();
            configuration.CreateMap<CreateOrEditCader_link_DDto, Cader_link_D>().ReverseMap();
            configuration.CreateMap<Cader_link_DDto, Cader_link_D>().ReverseMap();
            configuration.CreateMap<CreateOrEditCader_link_HDto, Cader_link_H>().ReverseMap();
            configuration.CreateMap<Cader_link_HDto, Cader_link_H>().ReverseMap();
            configuration.CreateMap<CreateOrEditCaderDto, PayRoll.Cader.Cader>().ReverseMap();
            configuration.CreateMap<CaderDto, PayRoll.Cader.Cader>().ReverseMap();
            configuration.CreateMap<CreateOrEditOECSDDto, SupplyChain.Sales.OECSD.OECSD>().ReverseMap();
            configuration.CreateMap<OECSDDto, SupplyChain.Sales.OECSD.OECSD>().ReverseMap();
            configuration.CreateMap<CreateOrEditOECSHDto, SupplyChain.Sales.OECSH.OECSH>().ReverseMap();
            configuration.CreateMap<OECSHDto, SupplyChain.Sales.OECSH.OECSH>().ReverseMap();
            configuration.CreateMap<CreateOrEditMonthlyCPRDto, PayRoll.MonthlyCPR.MonthlyCPR>().ReverseMap();
            configuration.CreateMap<MonthlyCPRDto, PayRoll.MonthlyCPR.MonthlyCPR>().ReverseMap();
            configuration.CreateMap<CreateOrEditOEQDDto, OEQD>().ReverseMap();
            configuration.CreateMap<OEQDDto, OEQD>().ReverseMap();
            configuration.CreateMap<CreateOrEditOEQHDto, OEQH>().ReverseMap();
            configuration.CreateMap<OEQHDto, OEQH>().ReverseMap();
            configuration.CreateMap<CreateOrEditICLOTDto, SupplyChain.Inventory.ICLOT.ICLOT>().ReverseMap();
            configuration.CreateMap<ICLOTDto, SupplyChain.Inventory.ICLOT.ICLOT>().ReverseMap();
            configuration.CreateMap<CreateOrEditSalaryLockDto, PayRoll.SalaryLock.SalaryLock>().ReverseMap();
            configuration.CreateMap<SalaryLockDto, PayRoll.SalaryLock.SalaryLock>().ReverseMap();
            //configuration.CreateMap<CreateOrEditPorcDetailForEditDto, SupplyChain.Purchase.PorcDetailForEdit.PorcDetailForEdit>().ReverseMap();
            //configuration.CreateMap<PorcDetailForEditDto, SupplyChain.Purchase.PorcDetailForEdit.PorcDetailForEdit>().ReverseMap();
            configuration.CreateMap<CreateOrEditAPINVHDto, SupplyChain.Purchase.APINVH.APINVH>().ReverseMap();
            configuration.CreateMap<APINVHDto, SupplyChain.Purchase.APINVH.APINVH>().ReverseMap();
            configuration.CreateMap<CreateOrEditMFWCTOLDto, MFWCTOL>().ReverseMap();
            configuration.CreateMap<MFWCTOLDto, MFWCTOL>().ReverseMap();
            configuration.CreateMap<CreateOrEditMFWCRESDto, MFWCRES>().ReverseMap();
            configuration.CreateMap<MFWCRESDto, MFWCRES>().ReverseMap();
            configuration.CreateMap<CreateOrEditMFWCMDto, MFWCM>().ReverseMap();
            configuration.CreateMap<MFWCMDto, MFWCM>().ReverseMap();
            configuration.CreateMap<CreateOrEditMFRESMASDto, MFRESMAS>().ReverseMap();
            configuration.CreateMap<MFRESMASDto, MFRESMAS>().ReverseMap();
            configuration.CreateMap<CreateOrEditMFAREADto, MFAREA>().ReverseMap();
            configuration.CreateMap<MFAREADto, MFAREA>().ReverseMap();
            configuration.CreateMap<CreateOrEditMFACSETDto, MFACSET>().ReverseMap();
            configuration.CreateMap<MFACSETDto, MFACSET>().ReverseMap();
            configuration.CreateMap<CreateOrEditSlabSetupDto, Payroll.SlabSetup.SlabSetup>().ReverseMap();
            configuration.CreateMap<SlabSetupDto, Payroll.SlabSetup.SlabSetup>().ReverseMap();
            configuration.CreateMap<CurrencyRateHistoryDto, CurrencyRate>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployerBankDto, EmployerBank>().ReverseMap();
            configuration.CreateMap<EmployerBankDto, EmployerBank>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLBSCtgDto, GLBSCtg>().ReverseMap();
            configuration.CreateMap<GLBSCtgDto, GLBSCtg>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeLeavesTotalDto, EmployeeLeavesTotal>().ReverseMap();
            configuration.CreateMap<EmployeeLeavesTotalDto, EmployeeLeavesTotal>().ReverseMap();
            configuration.CreateMap<CreateOrEditAdjHDto, AdjH>().ReverseMap();
            configuration.CreateMap<AdjHDto, AdjH>().ReverseMap();
            configuration.CreateMap<CreateOrEditStopSalaryDto, PayRoll.StopSalary.StopSalary>().ReverseMap();
            configuration.CreateMap<StopSalaryDto, PayRoll.StopSalary.StopSalary>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeLoansTypeDto, PayRoll.EmployeeLoansType.EmployeeLoansTypes>().ReverseMap();
            configuration.CreateMap<EmployeeLoansTypeDto, PayRoll.EmployeeLoansType.EmployeeLoansTypes>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeLoansDto, PayRoll.EmployeeLoans.EmployeeLoans>().ReverseMap();
            configuration.CreateMap<EmployeeLoansDto, PayRoll.EmployeeLoans.EmployeeLoans>().ReverseMap();
            configuration.CreateMap<CreateOrEditAuditPostingLogsDto, Common.AuditPostingLogs.AuditPostingLogs>().ReverseMap();
            configuration.CreateMap<AuditPostingLogsDto, Common.AuditPostingLogs.AuditPostingLogs>().ReverseMap();
            configuration.CreateMap<CreateOrEditSubDesignationsDto, SubDesignations>().ReverseMap();
            configuration.CreateMap<SubDesignationsDto, SubDesignations>().ReverseMap();
            configuration.CreateMap<CreateOrEditAllowancesDetailDto, AllowancesDetail>().ReverseMap();
            configuration.CreateMap<AllowancesDetailDto, AllowancesDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditAllowancesDto, Allowances>().ReverseMap();
            configuration.CreateMap<AllowancesDto, Allowances>().ReverseMap();
            configuration.CreateMap<CreateOrEditAllowanceSetupDto, AllowanceSetup>().ReverseMap();
            configuration.CreateMap<AllowanceSetupDto, AllowanceSetup>().ReverseMap();
            configuration.CreateMap<ImportChartofAccountDto, ChartofControl>().ReverseMap();
            configuration.CreateMap<CreateOrEditHolidaysDto, Holidays>().ReverseMap();
            configuration.CreateMap<HolidaysDto, Holidays>().ReverseMap();
            configuration.CreateMap<CreateOrEditEarningTypesDto, EarningTypes>().ReverseMap();
            configuration.CreateMap<EarningTypesDto, EarningTypes>().ReverseMap();
            configuration.CreateMap<CreateOrEditDeductionTypesDto, DeductionTypes>().ReverseMap();
            configuration.CreateMap<DeductionTypesDto, DeductionTypes>().ReverseMap();
            configuration.CreateMap<CityDto, City>().ReverseMap();

            configuration.CreateMap<CreateOrEditARTermDto, ARTerm>().ReverseMap();
            configuration.CreateMap<ARTermDto, ARTerm>().ReverseMap();
            configuration.CreateMap<CreateOrEditRecurringVoucherDto, CommonServices.RecurringVoucher.RecurringVoucher>().ReverseMap();
            configuration.CreateMap<RecurringVoucherDto, CommonServices.RecurringVoucher.RecurringVoucher>().ReverseMap();
            configuration.CreateMap<CreateOrEditPLCategoryDto, PLCategory>().ReverseMap();
            configuration.CreateMap<PLCategoryDto, PLCategory>().ReverseMap();

            configuration.CreateMap<CreateOrEditChequeBookDetailDto, ChequeBookDetail>().ReverseMap();
            configuration.CreateMap<ChequeBookDetailDto, ChequeBookDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditChequeBookDto, ChequeBook>().ReverseMap();
            configuration.CreateMap<ChequeBookDto, ChequeBook>().ReverseMap();
            configuration.CreateMap<CreateOrEditCSAlertDto, CSAlert>().ReverseMap();
            configuration.CreateMap<CSAlertDto, CSAlert>().ReverseMap();
            configuration.CreateMap<CreateOrEditCSAlertLogDto, CSAlertLog>().ReverseMap();
            configuration.CreateMap<CSAlertLogDto, CSAlertLog>().ReverseMap();
            configuration.CreateMap<CreateOrEditICELocationDto, ICELocation>().ReverseMap();
            configuration.CreateMap<ICELocationDto, ICELocation>().ReverseMap();

            configuration.CreateMap<AttachmentDto, Attachment.Attachment>().ReverseMap();
            configuration.CreateMap<CreateOrEditICWODetailDto, ICWODetail>().ReverseMap();
            configuration.CreateMap<ICWODetailDto, ICWODetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditICWOHeaderDto, ICWOHeader>().ReverseMap();
            configuration.CreateMap<ICWOHeaderDto, ICWOHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditGlChequeDto, GlCheque>().ReverseMap();
            configuration.CreateMap<GlChequeDto, GlCheque>().ReverseMap();

            configuration.CreateMap<CreateOrEditCRDRNoteDto, CRDRNote>().ReverseMap();
            configuration.CreateMap<CRDRNoteDto, CRDRNote>().ReverseMap();

            configuration.CreateMap<CreateOrEditGLTransferDto, GLTransfer>().ReverseMap();
            configuration.CreateMap<GLTransferDto, GLTransfer>().ReverseMap();

            configuration.CreateMap<CreateOrEditSalesReferenceDto, SalesReference>().ReverseMap();
            configuration.CreateMap<SalesReferenceDto, SalesReference>().ReverseMap();
            configuration.CreateMap<CreateOrEditAssetRegistrationDetailDto, AssetRegistrationDetail>().ReverseMap();
            configuration.CreateMap<AssetRegistrationDetailDto, AssetRegistrationDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditOERETDetailDto, OERETDetail>().ReverseMap();
            configuration.CreateMap<OERETDetailDto, OERETDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditOERETHeaderDto, OERETHeader>().ReverseMap();
            configuration.CreateMap<OERETHeaderDto, OERETHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditLedgerTypeDto, GeneralLedger.SetupForms.LedgerType.LedgerType>().ReverseMap();
            configuration.CreateMap<LedgerTypeDto, GeneralLedger.SetupForms.LedgerType.LedgerType>().ReverseMap();
            configuration.CreateMap<CreateOrEditCreditDebitNoteDto, CreditDebitNoteHeader>().ReverseMap();
            configuration.CreateMap<CreditDebitNoteDto, CreditDebitNoteHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditOESALEDetailDto, OESALEDetail>().ReverseMap();
            configuration.CreateMap<OESALEDetailDto, OESALEDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditOESALEHeaderDto, OESALEHeader>().ReverseMap();
            configuration.CreateMap<OESALEHeaderDto, OESALEHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditOECOLLDto, OECOLL>().ReverseMap();
            configuration.CreateMap<OECOLLDto, OECOLL>().ReverseMap();
            configuration.CreateMap<CreateOrEditAssetRegistrationDto, AssetRegistration>().ReverseMap();
            configuration.CreateMap<AssetRegistrationDto, AssetRegistration>().ReverseMap();

            configuration.CreateMap<CreateOrEditVwReqStatus2Dto, VwReqStatus2>().ReverseMap();
            configuration.CreateMap<VwReqStatus2Dto, VwReqStatus2>().ReverseMap();
            configuration.CreateMap<CreateOrEditVwRetQtyDto, VwRetQty>().ReverseMap();
            configuration.CreateMap<VwRetQtyDto, VwRetQty>().ReverseMap();
            configuration.CreateMap<CreateOrEditVwReqStatusDto, VwReqStatus>().ReverseMap();
            configuration.CreateMap<VwReqStatusDto, VwReqStatus>().ReverseMap();

            configuration.CreateMap<CreateOrEditPOSTATDto, POSTAT>().ReverseMap();
            configuration.CreateMap<POSTATDto, POSTAT>().ReverseMap();

            configuration.CreateMap<CreateOrEditICRECAExpDto, ICRECAExp>().ReverseMap();
            configuration.CreateMap<ICRECAExpDto, ICRECAExp>().ReverseMap();
            configuration.CreateMap<CreateOrEditPORECDetailDto, PORECDetail>().ReverseMap();
            configuration.CreateMap<PORECDetailDto, PORECDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditPORECHeaderDto, PORECHeader>().ReverseMap();
            configuration.CreateMap<PORECHeaderDto, PORECHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditPORETDetailDto, PORETDetail>().ReverseMap();
            configuration.CreateMap<PORETDetailDto, PORETDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditPORETHeaderDto, PORETHeader>().ReverseMap();
            configuration.CreateMap<PORETHeaderDto, PORETHeader>().ReverseMap();

            configuration.CreateMap<CreateOrEditGLSecurityDetailDto, GLSecurityDetail>().ReverseMap();
            configuration.CreateMap<GLSecurityDetailDto, GLSecurityDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLSecurityHeaderDto, GLSecurityHeader>().ReverseMap();
            configuration.CreateMap<GLSecurityHeaderDto, GLSecurityHeader>().ReverseMap();

            configuration.CreateMap<CreateOrEditLCExpensesDetailDto, LCExpensesDetail>().ReverseMap();
            configuration.CreateMap<LCExpensesDetailDto, LCExpensesDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditLCExpensesHeaderDto, LCExpensesHeader>().ReverseMap();
            configuration.CreateMap<LCExpensesHeaderDto, LCExpensesHeader>().ReverseMap();

            configuration.CreateMap<CreateOrEditAttendanceDto, Attendance>().ReverseMap();

            configuration.CreateMap<CreateOrEditSalarySheetDto, SalarySheet>().ReverseMap();
            configuration.CreateMap<SalarySheetDto, SalarySheet>().ReverseMap();

            configuration.CreateMap<CreateOrEditAttendanceDetailDto, AttendanceDetail>().ReverseMap();
            configuration.CreateMap<AttendanceDetailDto, AttendanceDetail>().ReverseMap();

            configuration.CreateMap<CreateOrEditEmployeeSalaryDto, EmployeeSalary>().ReverseMap();
            configuration.CreateMap<EmployeeSalaryDto, EmployeeSalary>().ReverseMap();

            configuration.CreateMap<CreateOrEditAttendanceHeaderDto, AttendanceHeader>().ReverseMap();
            configuration.CreateMap<AttendanceHeaderDto, AttendanceHeader>().ReverseMap();

            configuration.CreateMap<CreateOrEditEducationDto, Education>().ReverseMap();
            configuration.CreateMap<EducationDto, Education>().ReverseMap();

            configuration.CreateMap<CreateOrEditEmployeeDeductionsDto, EmployeeDeductions>().ReverseMap();
            configuration.CreateMap<EmployeeDeductionsDto, EmployeeDeductions>().ReverseMap();

            configuration.CreateMap<CreateOrEditEmployeesDto, Employees>().ReverseMap();
            configuration.CreateMap<EmployeesDto, Employees>().ReverseMap();

            configuration.CreateMap<CreateOrEditEmployeeEarningsDto, EmployeeEarnings>().ReverseMap();
            configuration.CreateMap<EmployeeEarningsDto, EmployeeEarnings>().ReverseMap();

            configuration.CreateMap<CreateOrEditEmployeeArrearsDto, EmployeeArrears>().ReverseMap();
            configuration.CreateMap<EmployeeArrearsDto, EmployeeArrears>().ReverseMap();

            configuration.CreateMap<CreateOrEditEmployeeLeavesDto, EmployeeLeaves>().ReverseMap();
            configuration.CreateMap<EmployeeLeavesDto, EmployeeLeaves>().ReverseMap();

            configuration.CreateMap<CreateOrEditEmployeeTypeDto, EmployeeType>().ReverseMap();
            configuration.CreateMap<EmployeeTypeDto, EmployeeType>().ReverseMap();

            configuration.CreateMap<CreateOrEditSectionDto, Section>().ReverseMap();
            configuration.CreateMap<SectionDto, Section>().ReverseMap();

            configuration.CreateMap<CreateOrEditDepartmentDto, Department>().ReverseMap();
            configuration.CreateMap<DepartmentDto, Department>().ReverseMap();

            configuration.CreateMap<CreateOrEditShiftDto, Shift>().ReverseMap();
            configuration.CreateMap<ShiftDto, Shift>().ReverseMap();

            configuration.CreateMap<CreateOrEditBankReconcileDetailDto, BankReconcileDetail>().ReverseMap();
            configuration.CreateMap<BankReconcileDetailDto, BankReconcileDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditBankReconcileDto, BankReconcile>().ReverseMap();
            configuration.CreateMap<BankReconcileDto, BankReconcile>().ReverseMap();
            configuration.CreateMap<CreateOrEditReqStatDto, ReqStat>().ReverseMap();
            configuration.CreateMap<ReqStatDto, ReqStat>().ReverseMap();
            configuration.CreateMap<CreateOrEditPOPODetailDto, POPODetail>().ReverseMap();
            configuration.CreateMap<POPODetailDto, POPODetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditPOPOHeaderDto, POPOHeader>().ReverseMap();
            configuration.CreateMap<POPOHeaderDto, POPOHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditRequisitionDto, Requisitions>().ReverseMap();
            configuration.CreateMap<RequisitionDto, Requisitions>().ReverseMap();
            configuration.CreateMap<CreateOrEditIC_UNITDto, IC_UNIT>().ReverseMap();
            configuration.CreateMap<IC_UNITDto, IC_UNIT>().ReverseMap();
            configuration.CreateMap<CreateOrEditICOPNDetailDto, ICOPNDetail>().ReverseMap();
            configuration.CreateMap<ICOPNDetailDto, ICOPNDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditICOPNHeaderDto, ICOPNHeader>().ReverseMap();
            configuration.CreateMap<ICOPNHeaderDto, ICOPNHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditAssemblyDto, Assembly>().ReverseMap();
            configuration.CreateMap<AssemblyDto, Assembly>().ReverseMap();
            configuration.CreateMap<CreateOrEditICOPT5Dto, ICOPT5>().ReverseMap();
            configuration.CreateMap<ICOPT5Dto, ICOPT5>().ReverseMap();
            configuration.CreateMap<CreateOrEditICOPT4Dto, ICOPT4>().ReverseMap();
            configuration.CreateMap<ICOPT4Dto, ICOPT4>().ReverseMap();
            configuration.CreateMap<CreateOrEditDesignationDto, Designations>().ReverseMap();
            configuration.CreateMap<DesignationDto, Designations>().ReverseMap();
            configuration.CreateMap<CreateOrEditReligionDto, Religions>().ReverseMap();
            configuration.CreateMap<ReligionDto, Religions>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeTypeDto, EmployeeType>().ReverseMap();
            configuration.CreateMap<EmployeeTypeDto, EmployeeType>().ReverseMap();
            configuration.CreateMap<CreateOrEditGradeDto, Grade>().ReverseMap();
            configuration.CreateMap<GradeDto, Grade>().ReverseMap();
            configuration.CreateMap<CreateOrEditLocationDto, Locations>().ReverseMap();
            configuration.CreateMap<LocationDto, Locations>().ReverseMap();
            configuration.CreateMap<CreateOrEditICLEDGDto, ICLEDG>().ReverseMap();
            configuration.CreateMap<ICLEDGDto, ICLEDG>().ReverseMap();
            configuration.CreateMap<CreateOrEditTransferDto, Transfer>().ReverseMap()
                .ForMember(o => o.FromLocName, p => p.Ignore())
                .ForMember(o => o.ToLocName, p => p.Ignore());
            configuration.CreateMap<TransferDetailDto, TransferDetail>().ReverseMap().ForMember(o => o.MaxQty, p => p.Ignore());
            configuration.CreateMap<TransferDto, Transfer>().ReverseMap();

            configuration.CreateMap<ICCNSDetailDto, ICCNSDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditICCNSDetailDto, ICCNSDetail>().ReverseMap();

            configuration.CreateMap<ICCNSHeaderDto, ICCNSHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditICCNSHeaderDto, ICCNSHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditICADJDetailDto, ICADJDetail>().ReverseMap();
            configuration.CreateMap<ICADJDetailDto, ICADJDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditICADJHeaderDto, ICADJHeader>().ReverseMap();
            configuration.CreateMap<ICADJHeaderDto, ICADJHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLINVDetailDto, GLINVDetail>().ReverseMap();
            configuration.CreateMap<GLINVDetailDto, GLINVDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLINVHeaderDto, GLINVHeader>().ReverseMap();
            configuration.CreateMap<GLINVHeaderDto, GLINVHeader>().ReverseMap();
            configuration.CreateMap<CreateOrEditGatePassDto, GatePass>().ReverseMap();
            configuration.CreateMap<GatePassDto, GatePass>().ReverseMap();
            configuration.CreateMap<CreateOrEditSubCostCenterDto, SubCostCenter>().ReverseMap();
            configuration.CreateMap<SubCostCenterDto, SubCostCenter>().ReverseMap();
            configuration.CreateMap<CreateOrEditICSegment1Dto, ICSegment1>().ReverseMap();
            configuration.CreateMap<ICSegment1Dto, ICSegment1>().ReverseMap();
            configuration.CreateMap<CreateOrEditICSegment2Dto, ICSegment2>().ReverseMap();
            configuration.CreateMap<ICSegment2Dto, ICSegment2>().ReverseMap();
            configuration.CreateMap<CreateOrEditICSegment3Dto, ICSegment3>().ReverseMap();
            configuration.CreateMap<ICSegment3Dto, ICSegment3>().ReverseMap();
            configuration.CreateMap<CreateOrEditIcItemDto, ICItem>().ReverseMap();
            configuration.CreateMap<IcItemDto, ICItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditCostCenterDto, CostCenter>().ReverseMap();
            configuration.CreateMap<CostCenterDto, CostCenter>().ReverseMap();
            configuration.CreateMap<CreateOrEditItemPricingDto, ItemPricing>().ReverseMap();
            configuration.CreateMap<ItemPricingDto, ItemPricing>().ReverseMap();
            configuration.CreateMap<CreateOrEditReorderLevelDto, ReorderLevel>().ReverseMap();
            configuration.CreateMap<ReorderLevelDto, ReorderLevel>().ReverseMap();
            configuration.CreateMap<CreateOrEditTransactionTypeDto, TransactionType>().ReverseMap();
            configuration.CreateMap<TransactionTypeDto, TransactionType>().ReverseMap();
            configuration.CreateMap<CreateOrEditPriceListDto, PriceLists>().ReverseMap();
            configuration.CreateMap<CreateOrEditICUOMDto, ICUOM>().ReverseMap();
            configuration.CreateMap<PriceListDto, PriceLists>().ReverseMap();
            configuration.CreateMap<ICUOMDto, ICUOM>().ReverseMap();
            configuration.CreateMap<CreateOrEditICLocationDto, ICLocation>().ReverseMap();
            configuration.CreateMap<ICLocationDto, ICLocation>().ReverseMap();
            configuration.CreateMap<CreateOrEditInventoryGlLinkDto, InventoryGlLink>().ReverseMap();
            configuration.CreateMap<InventoryGlLinkDto, InventoryGlLink>().ReverseMap();
            configuration.CreateMap<CreateOrEditICSetupDto, ICSetup>().ReverseMap();
            configuration.CreateMap<ICSetupDto, ICSetup>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLLocationDto, GLLocation>().ReverseMap();
            configuration.CreateMap<GLLocationDto, GLLocation>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLOptionDto, GLOption>().ReverseMap();
            configuration.CreateMap<GLOptionDto, GLOption>().ReverseMap();
            configuration.CreateMap<CreateOrEditAROptionDto, AROption>().ReverseMap();
            configuration.CreateMap<AROptionDto, AROption>().ReverseMap();
            configuration.CreateMap<CreateOrEditAPOptionDto, APOption>().ReverseMap();
            configuration.CreateMap<APOptionDto, APOption>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaxClassDto, TaxClass>().ReverseMap();
            configuration.CreateMap<TaxClassDto, TaxClass>().ReverseMap();
            configuration.CreateMap<CreateOrEditFiscalCalenderDto, FiscalCalender>().ReverseMap();
            configuration.CreateMap<FiscalCalenderDto, FiscalCalender>().ReverseMap();
            configuration.CreateMap<CreateOrEditAccountsPostingDto, AccountsPosting>().ReverseMap();
            configuration.CreateMap<AccountsPostingDto, AccountsPosting>().ReverseMap();
            configuration.CreateMap<CreateOrEditLCExpensesDto, LCExpenses>().ReverseMap();
            configuration.CreateMap<LCExpensesDto, LCExpenses>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLTRDetailDto, GLTRDetail>().ReverseMap();
            configuration.CreateMap<GLTRDetailDto, GLTRDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLTRHeaderDto, GLTRHeader>().ReverseMap();
            configuration.CreateMap<GLTRHeaderDto, GLTRHeader>().ReverseMap();
            configuration.CreateMap<ImportTransactionDto, GLTRHeader>().ReverseMap();

            configuration.CreateMap<CreateOrEditBatchListPreviewDto, BatchListPreview>().ReverseMap();
            configuration.CreateMap<BatchListPreviewDto, BatchListPreview>().ReverseMap();

            configuration.CreateMap<CreateOrEditBkTransferDto, BkTransfer>().ReverseMap();
            configuration.CreateMap<CreateOrEditBkTransferDto, BkTransfer>().ReverseMap();

            configuration.CreateMap<CreateOrEditBankDto, Bank>().ReverseMap();
            configuration.CreateMap<BankDto, Bank>().ReverseMap();
            configuration.CreateMap<CreateOrEditCPRDto, CPR>().ReverseMap();
            configuration.CreateMap<CPRDto, CPR>().ReverseMap();
            configuration.CreateMap<CreateOrEditAPTermDto, APTerm>().ReverseMap();
            configuration.CreateMap<APTermDto, APTerm>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLCONFIGDto, GLCONFIG>().ReverseMap();
            configuration.CreateMap<GLCONFIGDto, GLCONFIG>().ReverseMap();
            configuration.CreateMap<CreateOrEditGLBOOKSDto, GLBOOKS>().ReverseMap();
            configuration.CreateMap<GLBOOKSDto, GLBOOKS>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaxAuthorityDto, TaxAuthority>().ReverseMap();
            configuration.CreateMap<TaxAuthorityDto, TaxAuthority>().ReverseMap();
            configuration.CreateMap<CreateOrEditAccountSubLedgerDto, AccountSubLedger>().ReverseMap();
            configuration.CreateMap<AccountSubLedgerDto, AccountSubLedger>().ReverseMap();
            configuration.CreateMap<CreateOrEditCurrencyRateDto, CurrencyRate>().ReverseMap();
            configuration.CreateMap<CurrencyRateDto, CurrencyRate>().ReverseMap();
            configuration.CreateMap<CreateOrEditChartofControlDto, ChartofControl>().ReverseMap();
            configuration.CreateMap<ChartofControlDto, ChartofControl>().ReverseMap();
            configuration.CreateMap<CreateOrEditSegmentlevel3Dto, Segmentlevel3>().ReverseMap();
            configuration.CreateMap<Segmentlevel3Dto, Segmentlevel3>().ReverseMap();
            configuration.CreateMap<CreateOrEditSubControlDetailDto, SubControlDetail>().ReverseMap();
            configuration.CreateMap<SubControlDetailDto, SubControlDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditControlDetailDto, ControlDetail>().ReverseMap();
            configuration.CreateMap<ControlDetailDto, ControlDetail>().ReverseMap();

            configuration.CreateMap<CreateOrEditGroupCodeDto, GroupCode>().ReverseMap();
            configuration.CreateMap<GroupCodeDto, GroupCode>().ReverseMap();
            configuration.CreateMap<CreateOrEditGroupCategoryDto, GroupCategory>().ReverseMap();
            configuration.CreateMap<GroupCategoryDto, GroupCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditFiscalCalendarDto, FiscalCalendar>();
            configuration.CreateMap<FiscalCalendar, FiscalCalendarDto>();

            configuration.CreateMap<CreateOrEditCompanyProfileDto, CompanyProfile>();
            configuration.CreateMap<CompanyProfile, CompanyProfileDto>();

            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}