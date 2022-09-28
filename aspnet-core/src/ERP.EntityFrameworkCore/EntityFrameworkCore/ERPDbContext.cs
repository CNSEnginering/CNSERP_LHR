using ERP.SupplyChain.Sales.OEDrivers;
using ERP.AccountReceivables.RouteInvoices;
using ERP.SupplyChain.Sales.OERoutes;
using ERP.GeneralLedger.SetupForms.GLDocRev;
using ERP.SupplyChain.Sales.Invoices;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.CommonServices.UserLoc.CSUserLocH;
using ERP.GeneralLedger.SetupForms.GLSLGroups;
//using ERP.PayRoll.CaderMaster.cader_link_D;
//using ERP.PayRoll.CaderMaster.cader_link_H;

using ERP.SupplyChain.Sales.OECSD;
using ERP.SupplyChain.Sales.OECSH;

using ERP.SupplyChain.Sales.SaleQutation;
using ERP.SupplyChain.Inventory.ICLOT;

//using ERP.SupplyChain.Purchase.PorcDetailForEdit;
using ERP.SupplyChain.Purchase.APINVH;
using ERP.Manufacturing.SetupForms;
using ERP.Manufacturing;
using ERP.Payroll.SlabSetup;
using ERP.PayRoll;
using ERP.Payroll.EmployeeLeaveBalance;
using ERP.PayRoll.Adjustments;
using ERP.PayRoll.EmployeeLoansType;
using ERP.PayRoll.EmployeeLoans;
using ERP.Common.AuditPostingLogs;
using ERP.AccountReceivables;
using ERP.CommonServices.RecurringVoucher;
using ERP.CommonServices.ChequeBooks;

using ERP.Common.AlertLog;
using ERP.SupplyChain.Inventory.WorkOrder;
using ERP.SupplyChain.Sales.SalesReference;
using ERP.SupplyChain.Sales.SaleReturn;
using ERP.GeneralLedger.SetupForms.LedgerType;
using ERP.SupplyChain.Sales;
using ERP.SupplyChain.Sales.SaleEntry;
using ERP.Sales.SaleAccounts;
using ERP.SupplyChain.Inventory.AssetRegistration;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using ERP.SupplyChain.Purchase.PurchaseOrder;
using ERP.SupplyChain.Purchase.Requisition;
using ERP.SupplyChain.Inventory.Opening;
using ERP.SupplyChain.Inventory.Adjustment;
using ERP.SupplyChain.Inventory;
using ERP.GeneralLedger.Transaction;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.AccountPayables;
using ERP.GeneralLedger.SetupForms;
using ERP.CommonServices;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Roles;
using ERP.Authorization.Users;
using ERP.Chat;
using ERP.Editions;
using ERP.Friendships;
using ERP.MultiTenancy;
using ERP.MultiTenancy.Accounting;
using ERP.MultiTenancy.Payments;
using ERP.Storage;
using ERP.Tenants.DbPerTenant;
using ERP.GeneralLedger.DirectInvoice;
using ERP.SupplyChain.Inventory.Consumption;
using ERP.SupplyChain.Inventory.ICOPT5;
using ERP.SupplyChain.Inventory.ICOPT4;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.SupplyChain.Inventory.IC_Segment3;
using ERP.SupplyChain.Inventory.IC_UNIT;
using ERP.SupplyChain.Purchase;
using ERP.SupplyChain.Purchase.ReceiptReturn;
using ERP.GeneralLedger.Transaction.BankReconcile;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.SupplyChain.Sales.CreditDebitNote;
using ERP.Common.CSAlertInfo;
using ERP.GeneralLedger.SetupForms.GLPLCategory;
using ERP.GeneralLedger.SetupForms.LCExpenses;
using ERP.GeneralLedger.SetupForms.LCExpensesHD;
using ERP.AccountPayables.CRDRNote;
using ERP.GeneralLedger.Transaction.GLTransfer;
//using ERP.GeneralLedger.SetupForms.AccountsPermission;

namespace ERP.EntityFrameworkCore
{
    public class ERPDbContext : AbpZeroDbContext<Tenant, Role, User, ERPDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<SupplyChain.Sales.OEDrivers.OEDrivers> OEDrivers { get; set; }

        public virtual DbSet<ARINVD> ARINVD { get; set; }

        public virtual DbSet<ARINVH> ARINVH { get; set; }

        public virtual DbSet<SupplyChain.Sales.OERoutes.OERoutes> OERoutes { get; set; }

        public virtual DbSet<GeneralLedger.SetupForms.GLDocRev.GLDocRev> GLDocRev { get; set; }

        public virtual DbSet<OEINVKNOCKD> OEINVKNOCKD { get; set; }

        public virtual DbSet<OEINVKNOCKH> OEINVKNOCKH { get; set; }

        public virtual DbSet<CommonServices.UserLoc.CSUserLocD.CSUserLocD> CSUserLocD { get; set; }

        public virtual DbSet<CommonServices.UserLoc.CSUserLocH.CSUserLocH> CSUserLocH { get; set; }

        public virtual DbSet<GLSLGroups> GLSLGroups { get; set; }

        //public virtual DbSet<Cader_link_D> Cader_link_D { get; set; }

        //public virtual DbSet<Cader_link_H> Cader_link_H { get; set; }

        public virtual DbSet<SupplyChain.Sales.OECSD.OECSD> OECSD { get; set; }

        public virtual DbSet<SupplyChain.Sales.OECSH.OECSH> OECSH { get; set; }

        public virtual DbSet<OEQD> OEQD { get; set; }

        public virtual DbSet<OEQH> OEQH { get; set; }

        public virtual DbSet<ICLOT> ICLOT { get; set; }

        //public virtual DbSet<SupplyChain.Purchase.PorcDetailForEdit.PorcDetailForEdit> PorcDetailForEdit { get; set; }

        public virtual DbSet<APINVH> APINVH { get; set; }

        public virtual DbSet<MFWCM> MFWCM { get; set; }

        public virtual DbSet<MFWCRES> MFWCRES { get; set; }

        public virtual DbSet<MFWCTOL> MFWCTOL { get; set; }
        public virtual DbSet<MFRESMAS> MFRESMAS { get; set; }

        public virtual DbSet<MFAREA> MFAREA { get; set; }

        public virtual DbSet<MFACSET> MFACSET { get; set; }

        public virtual DbSet<CurrencyRateHistory> CurrencyRateHistory { get; set; }
        public virtual DbSet<GLBSCtg> GLBSCtg { get; set; }

        public virtual DbSet<AuditPostingLogs> AuditPostingLogs { get; set; }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<ARTerm> ARTerms { get; set; }

        public virtual DbSet<CommonServices.RecurringVoucher.RecurringVoucher> RecurringVouchers { get; set; }

        public virtual DbSet<PLCategory> PLCategories { get; set; }
        public virtual DbSet<ChequeBookDetail> ChequeBookDetails { get; set; }

        public virtual DbSet<ChequeBook> ChequeBooks { get; set; }

        public virtual DbSet<CSAlert> CSAlert { get; set; }

        public virtual DbSet<CSAlertLog> CSAlertLog { get; set; }

        public virtual DbSet<ICELocation> ICELocation { get; set; }

        public virtual DbSet<Attachment.Attachment> Attachments { get; set; }
        public virtual DbSet<ICWODetail> ICWODetails { get; set; }

        public virtual DbSet<ICWOHeader> ICWOHeaders { get; set; }

        public virtual DbSet<GlCheque> GlCheques { get; set; }
        public virtual DbSet<CRDRNote> CPDRNotes { get; set; }
        public virtual DbSet<GLTransfer> GLTransfers { get; set; }

        //public virtual DbSet<GLAccountsPermission> GLAccountsPermissions { get; set; }
        public virtual DbSet<SalesReference> SalesReferences { get; set; }

        public virtual DbSet<AssetRegistrationDetail> AssetRegistrationDetails { get; set; }

        public virtual DbSet<OERETDetail> OERETDetails { get; set; }

        public virtual DbSet<OERETHeader> OERETHeaders { get; set; }

        public virtual DbSet<GeneralLedger.SetupForms.LedgerType.LedgerType> LedgerTypes { get; set; }

        public virtual DbSet<CreditDebitNoteHeader> CreditDebitNoteHeaders { get; set; }
        public virtual DbSet<CreditDebitNoteDetail> CreditDebitNoteDetails { get; set; }

        public virtual DbSet<OESALEDetail> OESALEDetails { get; set; }

        public virtual DbSet<OESALEHeader> OESALEHeaders { get; set; }

        public virtual DbSet<OECOLL> SaleAccounts { get; set; }

        public virtual DbSet<AssetRegistration> AssetRegistration { get; set; }

        public virtual DbSet<MonthlyConsolidated> MonthlyConsolidated { get; set; }

        public virtual DbSet<VwReqStatus2> VwReqStatus2 { get; set; }

        public virtual DbSet<VwRetQty> VwRetQty { get; set; }

        public virtual DbSet<VwReqStatus> VwReqStatus { get; set; }

        public virtual DbSet<POSTAT> POSTAT { get; set; }

        public virtual DbSet<ICRECAExp> ICRECAExps { get; set; }

        public virtual DbSet<PORECDetail> PORECDetails { get; set; }

        public virtual DbSet<PORECHeader> PORECHeaders { get; set; }

        public virtual DbSet<PORETDetail> PORETDetails { get; set; }

        public virtual DbSet<PORETHeader> PORETHeaders { get; set; }

        public virtual DbSet<GLSecurityDetail> GLSecurityDetails { get; set; }

        public virtual DbSet<GLSecurityHeader> GLSecurityHeaders { get; set; }

        public virtual DbSet<LCExpensesHeader> LCExpensesHeaders { get; set; }

        public virtual DbSet<LCExpensesDetail> LCExpensesDetails { get; set; }

        public virtual DbSet<BankReconcile> BankReconciles { get; set; }

        public virtual DbSet<BankReconcileDetail> BankReconcileDetails { get; set; }
        public virtual DbSet<POPODetail> POPODetails { get; set; }

        public virtual DbSet<POPOHeader> POPOHeaders { get; set; }

        public virtual DbSet<ReqStat> ReqStats { get; set; }

        public virtual DbSet<Requisitions> Requisitions { get; set; }
        public virtual DbSet<RequisitionDetail> RequisitionDetails { get; set; }
        public virtual DbSet<IC_UNIT> IC_UNITs { get; set; }

        public virtual DbSet<ICItem> ICItems { get; set; }
        public virtual DbSet<ICSegment1> ICSegment1s { get; set; }
        public virtual DbSet<ICSegment2> ICSegment2s { get; set; }
        public virtual DbSet<ICSegment3> ICSegment3s { get; set; }
        public virtual DbSet<ICOPT5> ICOPT5 { get; set; }
        public virtual DbSet<ICOPNDetail> ICOPNDetails { get; set; }

        public virtual DbSet<ICOPNHeader> ICOPNHeaders { get; set; }

        public virtual DbSet<Assembly> Assemblies { get; set; }
        public virtual DbSet<AssemblyDetails> AssembliesDetails { get; set; }

        public virtual DbSet<ICADJDetail> ICADJDetails { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }
        public virtual DbSet<TransferDetail> TransfersDetail { get; set; }
        public virtual DbSet<ICLEDG> ICLEDG { get; set; }

        public virtual DbSet<ICCNSDetail> ICCNSDetail { get; set; }

        public virtual DbSet<ICCNSHeader> ICCNSHeader { get; set; }

        public virtual DbSet<ICADJHeader> ICADJHeader { get; set; }

        public virtual DbSet<GLINVDetail> GLINVDetails { get; set; }

        public virtual DbSet<GLINVHeader> GLINVHeaders { get; set; }

        public virtual DbSet<GatePass> GatePasses { get; set; }

        public virtual DbSet<GatePassDetail> GatePassDetails { get; set; }

        public virtual DbSet<SubCostCenter> SubCostCenters { get; set; }

        public virtual DbSet<CostCenter> CostCenters { get; set; }

        public virtual DbSet<ItemPricing> ItemPricings { get; set; }

        public virtual DbSet<ItemsPriceList> ItemsPriceLists { get; set; }
        public virtual DbSet<CharOfACListing> CharOfACListings { get; set; }

        public virtual DbSet<ReorderLevel> ReorderLevels { get; set; }

        public virtual DbSet<TransactionType> TransactionTypes { get; set; }

        public virtual DbSet<PriceLists> PriceLists { get; set; }
        public virtual DbSet<PriceListsM> PriceListsM { get; set; }

        public virtual DbSet<ICUOM> ICUOMs { get; set; }

        public virtual DbSet<ICLocation> ICLocations { get; set; }

        public virtual DbSet<InventoryGlLink> InventoryGlLinks { get; set; }

        public virtual DbSet<ICSetup> ICSetups { get; set; }

        public virtual DbSet<ICOPT4> ICOPT4 { get; set; }
        public virtual DbSet<GLLocation> GLLocations { get; set; }

        public virtual DbSet<GLOption> GLOptions { get; set; }

        public virtual DbSet<ConnectionPerTenant> ConnectionPerTenants { get; set; }

        public virtual DbSet<AROption> AROptions { get; set; }

        public virtual DbSet<APOption> APOptions { get; set; }

        public virtual DbSet<TaxClass> TaxClasses { get; set; }

        public virtual DbSet<FiscalCalender> FiscalCalenders { get; set; }

        public virtual DbSet<AccountsPosting> AccountsPostings { get; set; }
        public virtual DbSet<LCExpenses> LCExpenses { get; set; }

        public virtual DbSet<GLTRDetail> GLTRDetails { get; set; }

        public virtual DbSet<GLTRHeader> GLTRHeaders { get; set; }

        public virtual DbSet<BkTransfer> BkTransfers { get; set; }

        public virtual DbSet<BatchListPreview> BatchListPreviews { get; set; }
        public virtual DbSet<GLCONFIG> GLCONFIG { get; set; }

        public virtual DbSet<GLBOOKS> GLBOOKS { get; set; }
        public virtual DbSet<TaxAuthority> TaxAuthorities { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<CPR> Cprs { get; set; }
        public virtual DbSet<APTerm> APTerms { get; set; }
        public virtual DbSet<AccountSubLedger> AccountSubLedgers { get; set; }

        public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }

        public virtual DbSet<ChartofControl> ChartofControls { get; set; }
        public virtual DbSet<GLSecChartofControl> GLSecChartofControls { get; set; }

        public virtual DbSet<Segmentlevel3> Segmentlevel3s { get; set; }

        public virtual DbSet<SubControlDetail> SubControlDetails { get; set; }

        public virtual DbSet<ControlDetail> ControlDetails { get; set; }

        public virtual DbSet<GroupCode> GroupCodes { get; set; }

        public virtual DbSet<GroupCategory> GroupCategories { get; set; }

        public virtual DbSet<FiscalCalendar> FiscalCalendars { get; set; }

        public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public ERPDbContext(DbContextOptions<ERPDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //

            modelBuilder.Entity<OEDrivers>(o =>
            {
                o.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ARINVD>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ARINVH>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OERoutes>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GLDocRev>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OEINVKNOCKD>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OEINVKNOCKH>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CSUserLocD>(c =>
                                  {
                                      c.HasIndex(e => new { e.TenantId });
                                  });
            modelBuilder.Entity<CSUserLocH>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GLSLGroups>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            //modelBuilder.Entity<Cader_link_D>(c =>
            //           {
            //               c.HasIndex(e => new { e.TenantId });
            //           });
            //modelBuilder.Entity<Cader_link_H>(c =>
            //           {
            //               c.HasIndex(e => new { e.TenantId });
            //           });
            modelBuilder.Entity<OECSD>(o =>
                                  {
                                      o.HasIndex(e => new { e.TenantId });
                                  });
            modelBuilder.Entity<OECSH>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OEQD>(o =>
                                             {
                                                 o.HasIndex(e => new { e.TenantId });
                                             });
            modelBuilder.Entity<OEQH>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ICLOT>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });

            // modelBuilder.Entity<PorcDetailForEdit>(p =>
            //{
            //    p.HasIndex(e => new { e.TenantId });
            //});

            modelBuilder.Entity<APINVH>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SlabSetup>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<MFWCTOL>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<MFWCRES>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<MFWCM>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<MFRESMAS>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<MFAREA>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<MFACSET>(m =>
                       {
                           m.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SlabSetup>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<EmployerBank>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GLBSCtg>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<EmployeeLeavesTotal>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<AdjH>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<AuditPostingLogs>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ARTerm>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PLCategory>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });

            var converter = new ValueConverter<int, decimal>(
            v => v,
            v => (int)v,
            new ConverterMappingHints(valueGeneratorFactory: (p, t) => new TemporaryIntValueGenerator()));

            modelBuilder.Entity<RecurringVoucher>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ChequeBookDetail>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ChequeBook>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CSAlert>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CSAlertLog>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ICELocation>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Attachment.Attachment>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GLSecurityDetail>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLSecurityHeader>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LCExpensesHeader>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<LCExpensesDetail>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ICWODetail>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ICWOHeader>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            //modelBuilder.Entity<GLAccountsPermission>(g =>
            //           {
            //               g.HasIndex(e => new { e.TenantId });
            //           });

            modelBuilder.Entity<GlCheque>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<CRDRNote>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<GLTransfer>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<SalesReference>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<AssetRegistrationDetail>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OERETDetail>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<LedgerType>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<OERETHeader>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditDebitNoteHeader>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditDebitNoteHeader>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditDebitNoteHeader>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OESALEDetail>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OESALEHeader>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OECOLL>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<AssetRegistration>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId, e.AssetID }).HasName("IX_AMRegister").IsUnique();
                           a.Property(f => f.Id).ValueGeneratedOnAdd().HasConversion(converter);
                           ;
                       });

            modelBuilder.Entity<VwReqStatus>(v =>
           {
               v.HasIndex(e => new { e.TenantId });
           });
            modelBuilder.Entity<VwRetQty>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VwReqStatus>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<POSTAT>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ICRECAExp>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PORECDetail>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ReqStat>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<PORECHeader>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<PORETDetail>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PORETHeader>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ReqStat>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BankReconcile>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<BankReconcileDetail>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<POPODetail>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<POPOHeader>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Requisitions>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<RequisitionDetail>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<IC_UNIT>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ICOPT5>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ICOPT4>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ICSegment1>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ICSegment2>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ICSegment3>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ICItem>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ICOPNDetail>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<ICOPNHeader>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<Assembly>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Transfer>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            ////////////
            modelBuilder.Entity<Transfer>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Transfer>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ICADJDetail>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLINVDetail>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ICADJHeader>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ICOPT4>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLINVDetail>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLINVHeader>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GatePass>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GatePassDetail>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<SubCostCenter>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CostCenter>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ItemPricing>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ItemsPriceList>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<CharOfACListing>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ReorderLevel>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TransactionType>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PriceLists>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            //modelBuilder.Entity<PriceLists>(i =>
            //           {
            //               i.HasIndex(e => new { e.TenantId });
            //           });
            modelBuilder.Entity<ICUOM>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<InventoryGlLink>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ICLocation>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            //modelBuilder.Entity<InventoryGlLink>(i =>
            //           {
            //               i.HasIndex(e => new { e.TenantId });
            //           });
            modelBuilder.Entity<ICSetup>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLLocation>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GLOption>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<AROption>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<APOption>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<TaxClass>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });

            //modelBuilder.Entity<AccountsPosting>(a =>
            modelBuilder.Entity<FiscalCalender>(f =>
            {
                f.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<LCExpenses>(f =>
            {
                f.HasIndex(e => new { e.TenantId });
            });
            //modelBuilder.Entity<AccountsPosting>(a =>
            //{
            //    a.HasIndex(e => new { e.TenantId });
            //});
            modelBuilder.Entity<BatchListPreview>(entity =>
            {

                entity.HasKey(e => e.Id);
                entity.ToTable("V_GLTRV");

            });

            modelBuilder.Entity<GLTRDetail>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLTRHeader>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BkTransfer>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Bank>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<CPR>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<GLCONFIG>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLBOOKS>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TaxAuthority>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId, e.Id });
                       });
            modelBuilder.Entity<AccountSubLedger>(a =>
                       {
                           a.HasKey(o => new { o.AccountID, o.Id, o.TenantId });
                           //a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<APTerm>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CurrencyRate>(c =>
            {
                c.HasKey(o => new { o.Id, o.TenantId });
            });
            modelBuilder.Entity<ChartofControl>(c =>
            {
                // c.HasIndex(e => new {e.Id , e.TenantId });
                c.HasKey(o => new { o.Id, o.TenantId });
                //c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<GLSecChartofControl>(c =>
            {
                c.HasKey(o => new { o.Id, o.TenantId });
            });
            modelBuilder.Entity<Segmentlevel3>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SubControlDetail>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ControlDetail>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GroupCode>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<GroupCategory>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<FiscalCalendar>(f =>
                       {
                           f.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CompanyProfile>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}