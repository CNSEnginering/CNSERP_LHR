using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ERP.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var oeDrivers = pages.CreateChildPermission(AppPermissions.Pages_OEDrivers, L("OEDrivers"), multiTenancySides: MultiTenancySides.Tenant);
            oeDrivers.CreateChildPermission(AppPermissions.Pages_OEDrivers_Create, L("CreateNewOEDrivers"), multiTenancySides: MultiTenancySides.Tenant);
            oeDrivers.CreateChildPermission(AppPermissions.Pages_OEDrivers_Edit, L("EditOEDrivers"), multiTenancySides: MultiTenancySides.Tenant);
            oeDrivers.CreateChildPermission(AppPermissions.Pages_OEDrivers_Delete, L("DeleteOEDrivers"), multiTenancySides: MultiTenancySides.Tenant);

            var arinvd = pages.CreateChildPermission(AppPermissions.Pages_ARINVD, L("ARINVD"), multiTenancySides: MultiTenancySides.Tenant);
            arinvd.CreateChildPermission(AppPermissions.Pages_ARINVD_Create, L("CreateNewARINVD"), multiTenancySides: MultiTenancySides.Tenant);
            arinvd.CreateChildPermission(AppPermissions.Pages_ARINVD_Edit, L("EditARINVD"), multiTenancySides: MultiTenancySides.Tenant);
            arinvd.CreateChildPermission(AppPermissions.Pages_ARINVD_Delete, L("DeleteARINVD"), multiTenancySides: MultiTenancySides.Tenant);

            var arinvh = pages.CreateChildPermission(AppPermissions.Pages_ARINVH, L("ARINVH"), multiTenancySides: MultiTenancySides.Tenant);
            arinvh.CreateChildPermission(AppPermissions.Pages_ARINVH_Create, L("CreateNewARINVH"), multiTenancySides: MultiTenancySides.Tenant);
            arinvh.CreateChildPermission(AppPermissions.Pages_ARINVH_Edit, L("EditARINVH"), multiTenancySides: MultiTenancySides.Tenant);
            arinvh.CreateChildPermission(AppPermissions.Pages_ARINVH_Delete, L("DeleteARINVH"), multiTenancySides: MultiTenancySides.Tenant);

            

            var oecsd = pages.CreateChildPermission(AppPermissions.Pages_OECSD, L("OECSD"), multiTenancySides: MultiTenancySides.Tenant);
            oecsd.CreateChildPermission(AppPermissions.Pages_OECSD_Create, L("CreateNewOECSD"), multiTenancySides: MultiTenancySides.Tenant);
            oecsd.CreateChildPermission(AppPermissions.Pages_OECSD_Edit, L("EditOECSD"), multiTenancySides: MultiTenancySides.Tenant);
            oecsd.CreateChildPermission(AppPermissions.Pages_OECSD_Delete, L("DeleteOECSD"), multiTenancySides: MultiTenancySides.Tenant);

            var oecsh = pages.CreateChildPermission(AppPermissions.Pages_OECSH, L("OECSH"), multiTenancySides: MultiTenancySides.Tenant);
            oecsh.CreateChildPermission(AppPermissions.Pages_OECSH_Create, L("CreateNewOECSH"), multiTenancySides: MultiTenancySides.Tenant);
            oecsh.CreateChildPermission(AppPermissions.Pages_OECSH_Edit, L("EditOECSH"), multiTenancySides: MultiTenancySides.Tenant);
            oecsh.CreateChildPermission(AppPermissions.Pages_OECSH_Delete, L("DeleteOECSH"), multiTenancySides: MultiTenancySides.Tenant);

            //var porcDetailForEdit = pages.CreateChildPermission(AppPermissions.Pages_PorcDetailForEdit, L("PorcDetailForEdit"), multiTenancySides: MultiTenancySides.Tenant);
            //porcDetailForEdit.CreateChildPermission(AppPermissions.Pages_PorcDetailForEdit_Create, L("CreateNewPorcDetailForEdit"), multiTenancySides: MultiTenancySides.Tenant);
            //porcDetailForEdit.CreateChildPermission(AppPermissions.Pages_PorcDetailForEdit_Edit, L("EditPorcDetailForEdit"), multiTenancySides: MultiTenancySides.Tenant);
            //porcDetailForEdit.CreateChildPermission(AppPermissions.Pages_PorcDetailForEdit_Delete, L("DeletePorcDetailForEdit"), multiTenancySides: MultiTenancySides.Tenant);

            var mfwcm = pages.CreateChildPermission(AppPermissions.Pages_MFWCM, L("MFWCM"), multiTenancySides: MultiTenancySides.Tenant);
            mfwcm.CreateChildPermission(AppPermissions.Pages_MFWCM_Create, L("CreateNewMFWCM"), multiTenancySides: MultiTenancySides.Tenant);
            mfwcm.CreateChildPermission(AppPermissions.Pages_MFWCM_Edit, L("EditMFWCM"), multiTenancySides: MultiTenancySides.Tenant);
            mfwcm.CreateChildPermission(AppPermissions.Pages_MFWCM_Delete, L("DeleteMFWCM"), multiTenancySides: MultiTenancySides.Tenant);

            var manufacturing = pages.CreateChildPermission(AppPermissions.Pages_Manufacturing, L("Manufacturing"), multiTenancySides: MultiTenancySides.Tenant);
            var mfacset = manufacturing.CreateChildPermission(AppPermissions.Pages_Manufacturing_Setup, L("Setup"), multiTenancySides: MultiTenancySides.Tenant);
            var mfacTrans = manufacturing.CreateChildPermission(AppPermissions.Pages_Manufacturing_Transaction, L("Transaction"), multiTenancySides: MultiTenancySides.Tenant);

            var mfacAccs = mfacset.CreateChildPermission(AppPermissions.Pages_MFACSET, L("ManufacAccs"), multiTenancySides: MultiTenancySides.Tenant);

            mfacAccs.CreateChildPermission(AppPermissions.Pages_MFACSET_Create, L("CreateNewMFACSET"), multiTenancySides: MultiTenancySides.Tenant);
            mfacAccs.CreateChildPermission(AppPermissions.Pages_MFACSET_Edit, L("EditMFACSET"), multiTenancySides: MultiTenancySides.Tenant);
            mfacAccs.CreateChildPermission(AppPermissions.Pages_MFACSET_Delete, L("DeleteMFACSET"), multiTenancySides: MultiTenancySides.Tenant);

            var mfarea = mfacset.CreateChildPermission(AppPermissions.Pages_MFAREA, L("MFAREA"), multiTenancySides: MultiTenancySides.Tenant);
            mfarea.CreateChildPermission(AppPermissions.Pages_MFAREA_Create, L("CreateNewMFAREA"), multiTenancySides: MultiTenancySides.Tenant);
            mfarea.CreateChildPermission(AppPermissions.Pages_MFAREA_Edit, L("EditMFAREA"), multiTenancySides: MultiTenancySides.Tenant);
            mfarea.CreateChildPermission(AppPermissions.Pages_MFAREA_Delete, L("DeleteMFAREA"), multiTenancySides: MultiTenancySides.Tenant);

            var mfresmas = mfacset.CreateChildPermission(AppPermissions.Pages_MFRESMAS, L("MFRESMAS"), multiTenancySides: MultiTenancySides.Tenant);
            mfresmas.CreateChildPermission(AppPermissions.Pages_MFRESMAS_Create, L("CreateNewMFRESMAS"), multiTenancySides: MultiTenancySides.Tenant);
            mfresmas.CreateChildPermission(AppPermissions.Pages_MFRESMAS_Edit, L("EditMFRESMAS"), multiTenancySides: MultiTenancySides.Tenant);
            mfresmas.CreateChildPermission(AppPermissions.Pages_MFRESMAS_Delete, L("DeleteMFRESMAS"), multiTenancySides: MultiTenancySides.Tenant);

            var mftool = mfacset.CreateChildPermission(AppPermissions.Pages_MFTOOL, L("MFTOOL"), multiTenancySides: MultiTenancySides.Tenant);
            mftool.CreateChildPermission(AppPermissions.Pages_MFTOOL_Create, L("CreateNewMFTOOL"), multiTenancySides: MultiTenancySides.Tenant);
            mftool.CreateChildPermission(AppPermissions.Pages_MFTOOL_Edit, L("EditMFTOOL"), multiTenancySides: MultiTenancySides.Tenant);
            mftool.CreateChildPermission(AppPermissions.Pages_MFTOOL_Delete, L("DeleteMFTOOL"), multiTenancySides: MultiTenancySides.Tenant);

            var mftoolty = mfacset.CreateChildPermission(AppPermissions.Pages_MFTOOLTY, L("MFTOOLTY"), multiTenancySides: MultiTenancySides.Tenant);
            mftoolty.CreateChildPermission(AppPermissions.Pages_MFTOOLTY_Create, L("CreateNewMFTOOLTY"), multiTenancySides: MultiTenancySides.Tenant);
            mftoolty.CreateChildPermission(AppPermissions.Pages_MFTOOLTY_Edit, L("EditMFTOOLTY"), multiTenancySides: MultiTenancySides.Tenant);
            mftoolty.CreateChildPermission(AppPermissions.Pages_MFTOOLTY_Delete, L("DeleteMFTOOLTY"), multiTenancySides: MultiTenancySides.Tenant);

            var mfwctol = mfacset.CreateChildPermission(AppPermissions.Pages_MFWCTOL, L("MFWCTOL"), multiTenancySides: MultiTenancySides.Tenant);
            mfwctol.CreateChildPermission(AppPermissions.Pages_MFWCTOL_Create, L("CreateNewMFWCTOL"), multiTenancySides: MultiTenancySides.Tenant);
            mfwctol.CreateChildPermission(AppPermissions.Pages_MFWCTOL_Edit, L("EditMFWCTOL"), multiTenancySides: MultiTenancySides.Tenant);
            mfwctol.CreateChildPermission(AppPermissions.Pages_MFWCTOL_Delete, L("DeleteMFWCTOL"), multiTenancySides: MultiTenancySides.Tenant);

            var mfwcres = mfacset.CreateChildPermission(AppPermissions.Pages_MFWCRES, L("MFWCRES"), multiTenancySides: MultiTenancySides.Tenant);
            mfwcres.CreateChildPermission(AppPermissions.Pages_MFWCRES_Create, L("CreateNewMFWCRES"), multiTenancySides: MultiTenancySides.Tenant);
            mfwcres.CreateChildPermission(AppPermissions.Pages_MFWCRES_Edit, L("EditMFWCRES"), multiTenancySides: MultiTenancySides.Tenant);
            mfwcres.CreateChildPermission(AppPermissions.Pages_MFWCRES_Delete, L("DeleteMFWCRES"), multiTenancySides: MultiTenancySides.Tenant);

            var glbsCtg = pages.CreateChildPermission(AppPermissions.Pages_GLBSCtg, L("GLBSCtg"), multiTenancySides: MultiTenancySides.Tenant);
            glbsCtg.CreateChildPermission(AppPermissions.Pages_GLBSCtg_Create, L("CreateNewGLBSCtg"), multiTenancySides: MultiTenancySides.Tenant);
            glbsCtg.CreateChildPermission(AppPermissions.Pages_GLBSCtg_Edit, L("EditGLBSCtg"), multiTenancySides: MultiTenancySides.Tenant);
            glbsCtg.CreateChildPermission(AppPermissions.Pages_GLBSCtg_Delete, L("DeleteGLBSCtg"), multiTenancySides: MultiTenancySides.Tenant);

            var auditPostingLogs = pages.CreateChildPermission(AppPermissions.Pages_AuditPostingLogs, L("AuditPostingLogs"), multiTenancySides: MultiTenancySides.Tenant);
            auditPostingLogs.CreateChildPermission(AppPermissions.Pages_AuditPostingLogs_Create, L("CreateNewAuditPostingLogs"), multiTenancySides: MultiTenancySides.Tenant);
            auditPostingLogs.CreateChildPermission(AppPermissions.Pages_AuditPostingLogs_Edit, L("EditAuditPostingLogs"), multiTenancySides: MultiTenancySides.Tenant);
            auditPostingLogs.CreateChildPermission(AppPermissions.Pages_AuditPostingLogs_Delete, L("DeleteAuditPostingLogs"), multiTenancySides: MultiTenancySides.Tenant);

            var arTerms = pages.CreateChildPermission(AppPermissions.Pages_ARTerms, L("ARTerms"), multiTenancySides: MultiTenancySides.Tenant);
            arTerms.CreateChildPermission(AppPermissions.Pages_ARTerms_Create, L("CreateNewARTerm"), multiTenancySides: MultiTenancySides.Tenant);
            arTerms.CreateChildPermission(AppPermissions.Pages_ARTerms_Edit, L("EditARTerm"), multiTenancySides: MultiTenancySides.Tenant);
            arTerms.CreateChildPermission(AppPermissions.Pages_ARTerms_Delete, L("DeleteARTerm"), multiTenancySides: MultiTenancySides.Tenant);

            var plCategories = pages.CreateChildPermission(AppPermissions.Pages_PLCategories, L("PLCategories"), multiTenancySides: MultiTenancySides.Tenant);
            plCategories.CreateChildPermission(AppPermissions.Pages_PLCategories_Create, L("CreateNewPLCategory"), multiTenancySides: MultiTenancySides.Tenant);
            plCategories.CreateChildPermission(AppPermissions.Pages_PLCategories_Edit, L("EditPLCategory"), multiTenancySides: MultiTenancySides.Tenant);
            plCategories.CreateChildPermission(AppPermissions.Pages_PLCategories_Delete, L("DeletePLCategory"), multiTenancySides: MultiTenancySides.Tenant);

            var recurringVouchers = pages.CreateChildPermission(AppPermissions.Pages_RecurringVouchers, L("RecurringVouchers"), multiTenancySides: MultiTenancySides.Tenant);
            recurringVouchers.CreateChildPermission(AppPermissions.Pages_RecurringVouchers_Create, L("CreateNewRecurringVoucher"), multiTenancySides: MultiTenancySides.Tenant);
            recurringVouchers.CreateChildPermission(AppPermissions.Pages_RecurringVouchers_Edit, L("EditRecurringVoucher"), multiTenancySides: MultiTenancySides.Tenant);
            recurringVouchers.CreateChildPermission(AppPermissions.Pages_RecurringVouchers_Delete, L("DeleteRecurringVoucher"), multiTenancySides: MultiTenancySides.Tenant);

            var chequeBookDetails = pages.CreateChildPermission(AppPermissions.Pages_ChequeBookDetails, L("ChequeBookDetails"), multiTenancySides: MultiTenancySides.Tenant);
            chequeBookDetails.CreateChildPermission(AppPermissions.Pages_ChequeBookDetails_Create, L("CreateNewChequeBookDetail"), multiTenancySides: MultiTenancySides.Tenant);
            chequeBookDetails.CreateChildPermission(AppPermissions.Pages_ChequeBookDetails_Edit, L("EditChequeBookDetail"), multiTenancySides: MultiTenancySides.Tenant);
            chequeBookDetails.CreateChildPermission(AppPermissions.Pages_ChequeBookDetails_Delete, L("DeleteChequeBookDetail"), multiTenancySides: MultiTenancySides.Tenant);

            var chequeBooks = pages.CreateChildPermission(AppPermissions.Pages_ChequeBooks, L("ChequeBooks"), multiTenancySides: MultiTenancySides.Tenant);
            chequeBooks.CreateChildPermission(AppPermissions.Pages_ChequeBooks_Create, L("CreateNewChequeBook"), multiTenancySides: MultiTenancySides.Tenant);
            chequeBooks.CreateChildPermission(AppPermissions.Pages_ChequeBooks_Edit, L("EditChequeBook"), multiTenancySides: MultiTenancySides.Tenant);
            chequeBooks.CreateChildPermission(AppPermissions.Pages_ChequeBooks_Delete, L("DeleteChequeBook"), multiTenancySides: MultiTenancySides.Tenant);

            var csAlert = pages.CreateChildPermission(AppPermissions.Pages_CSAlert, L("CSAlert"), multiTenancySides: MultiTenancySides.Tenant);
            csAlert.CreateChildPermission(AppPermissions.Pages_CSAlert_Create, L("CreateNewCSAlert"), multiTenancySides: MultiTenancySides.Tenant);
            csAlert.CreateChildPermission(AppPermissions.Pages_CSAlert_Edit, L("EditCSAlert"), multiTenancySides: MultiTenancySides.Tenant);
            csAlert.CreateChildPermission(AppPermissions.Pages_CSAlert_Delete, L("DeleteCSAlert"), multiTenancySides: MultiTenancySides.Tenant);

            var csAlertLog = pages.CreateChildPermission(AppPermissions.Pages_CSAlertLog, L("CSAlertLog"), multiTenancySides: MultiTenancySides.Tenant);
            csAlertLog.CreateChildPermission(AppPermissions.Pages_CSAlertLog_Create, L("CreateNewCSAlertLog"), multiTenancySides: MultiTenancySides.Tenant);
            csAlertLog.CreateChildPermission(AppPermissions.Pages_CSAlertLog_Edit, L("EditCSAlertLog"), multiTenancySides: MultiTenancySides.Tenant);
            csAlertLog.CreateChildPermission(AppPermissions.Pages_CSAlertLog_Delete, L("DeleteCSAlertLog"), multiTenancySides: MultiTenancySides.Tenant);

            var glCheques = pages.CreateChildPermission(AppPermissions.Pages_GlCheques, L("GlCheques"), multiTenancySides: MultiTenancySides.Tenant);
            glCheques.CreateChildPermission(AppPermissions.Pages_GlCheques_Create, L("CreateNewGlCheque"), multiTenancySides: MultiTenancySides.Tenant);
            glCheques.CreateChildPermission(AppPermissions.Pages_GlCheques_Edit, L("EditGlCheque"), multiTenancySides: MultiTenancySides.Tenant);
            glCheques.CreateChildPermission(AppPermissions.Pages_GlCheques_Delete, L("DeleteGlCheque"), multiTenancySides: MultiTenancySides.Tenant);

            var crDRNote = pages.CreateChildPermission(AppPermissions.Pages_CRDRNote, L("CRDRNote"), multiTenancySides: MultiTenancySides.Tenant);
            crDRNote.CreateChildPermission(AppPermissions.Pages_CRDRNote_Create, L("CreateNewCRDRNote"), multiTenancySides: MultiTenancySides.Tenant);
            crDRNote.CreateChildPermission(AppPermissions.Pages_CRDRNote_Edit, L("EditGlCRDRNote"), multiTenancySides: MultiTenancySides.Tenant);
            crDRNote.CreateChildPermission(AppPermissions.Pages_CRDRNote_Delete, L("DeleteCRDRNote"), multiTenancySides: MultiTenancySides.Tenant);

            var glTransfer = pages.CreateChildPermission(AppPermissions.Pages_GLTransfer, L("GLTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            glTransfer.CreateChildPermission(AppPermissions.Pages_GLTransfer_Create, L("CreateNewGLTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            glTransfer.CreateChildPermission(AppPermissions.Pages_GLTransfer_Edit, L("EditGlGLTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            glTransfer.CreateChildPermission(AppPermissions.Pages_GLTransfer_Delete, L("DeleteGLTransfer"), multiTenancySides: MultiTenancySides.Tenant);

            var glSecurityDetail = pages.CreateChildPermission(AppPermissions.Pages_GLSecurityDetail, L("GLSecurityDetail"), multiTenancySides: MultiTenancySides.Tenant);
            glSecurityDetail.CreateChildPermission(AppPermissions.Pages_GLSecurityDetail_Create, L("CreateNewGLSecurityDetail"), multiTenancySides: MultiTenancySides.Tenant);
            glSecurityDetail.CreateChildPermission(AppPermissions.Pages_GLSecurityDetail_Edit, L("EditGLSecurityDetail"), multiTenancySides: MultiTenancySides.Tenant);
            glSecurityDetail.CreateChildPermission(AppPermissions.Pages_GLSecurityDetail_Delete, L("DeleteGLSecurityDetail"), multiTenancySides: MultiTenancySides.Tenant);
            glSecurityDetail.CreateChildPermission(AppPermissions.Pages_GLSecurityDetail_Process, L("ProcessGLSecurityDetail"), multiTenancySides: MultiTenancySides.Tenant);

            var glSecurityHeader = pages.CreateChildPermission(AppPermissions.Pages_GLSecurityHeader, L("GLSecurityHeader"), multiTenancySides: MultiTenancySides.Tenant);
            glSecurityHeader.CreateChildPermission(AppPermissions.Pages_GLSecurityHeader_Create, L("CreateNewGLSecurityHeader"), multiTenancySides: MultiTenancySides.Tenant);
            glSecurityHeader.CreateChildPermission(AppPermissions.Pages_GLSecurityHeader_Edit, L("EditGLSecurityHeader"), multiTenancySides: MultiTenancySides.Tenant);
            glSecurityHeader.CreateChildPermission(AppPermissions.Pages_GLSecurityHeader_Delete, L("DeleteGLSecurityHeader"), multiTenancySides: MultiTenancySides.Tenant);

            var lcExpensesDetail = pages.CreateChildPermission(AppPermissions.Pages_LCExpensesDetail, L("LCExpensesDetail"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpensesDetail.CreateChildPermission(AppPermissions.Pages_LCExpensesDetail_Create, L("CreateNewLCExpensesDetail"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpensesDetail.CreateChildPermission(AppPermissions.Pages_LCExpensesDetail_Edit, L("EditLCExpensesDetail"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpensesDetail.CreateChildPermission(AppPermissions.Pages_LCExpensesDetail_Delete, L("DeleteLCExpensesDetail"), multiTenancySides: MultiTenancySides.Tenant);

            var lcExpensesHeader = pages.CreateChildPermission(AppPermissions.Pages_LCExpensesHeader, L("LCExpensesHeader"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpensesHeader.CreateChildPermission(AppPermissions.Pages_LCExpensesHeader_Create, L("CreateNewLCExpensesHeader"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpensesHeader.CreateChildPermission(AppPermissions.Pages_LCExpensesHeader_Edit, L("EditLCExpensesHeader"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpensesHeader.CreateChildPermission(AppPermissions.Pages_LCExpensesHeader_Delete, L("DeleteLCExpensesHeader"), multiTenancySides: MultiTenancySides.Tenant);

            var glAccountsPermissions = pages.CreateChildPermission(AppPermissions.Pages_GLAccountsPermissions, L("GLAccountsPermissions"), multiTenancySides: MultiTenancySides.Tenant);
            glAccountsPermissions.CreateChildPermission(AppPermissions.Pages_GLAccountsPermissions_Create, L("CreateNewGLAccountsPermission"), multiTenancySides: MultiTenancySides.Tenant);
            glAccountsPermissions.CreateChildPermission(AppPermissions.Pages_GLAccountsPermissions_Edit, L("EditGLAccountsPermission"), multiTenancySides: MultiTenancySides.Tenant);
            glAccountsPermissions.CreateChildPermission(AppPermissions.Pages_GLAccountsPermissions_Delete, L("DeleteGLAccountsPermission"), multiTenancySides: MultiTenancySides.Tenant);

            var lcExpenses = pages.CreateChildPermission(AppPermissions.Pages_LCExpenses, L("LCExpenses"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpenses.CreateChildPermission(AppPermissions.Pages_LCExpenses_Create, L("CreateNewLCExpense"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpenses.CreateChildPermission(AppPermissions.Pages_LCExpenses_Edit, L("EditLCExpense"), multiTenancySides: MultiTenancySides.Tenant);
            lcExpenses.CreateChildPermission(AppPermissions.Pages_LCExpenses_Delete, L("DeleteLCExpense"), multiTenancySides: MultiTenancySides.Tenant);

            var assetRegistrationDetails = pages.CreateChildPermission(AppPermissions.Pages_AssetRegistrationDetails, L("AssetRegistrationDetails"), multiTenancySides: MultiTenancySides.Tenant);
            assetRegistrationDetails.CreateChildPermission(AppPermissions.Pages_AssetRegistrationDetails_Create, L("CreateNewAssetRegistrationDetail"), multiTenancySides: MultiTenancySides.Tenant);
            assetRegistrationDetails.CreateChildPermission(AppPermissions.Pages_AssetRegistrationDetails_Edit, L("EditAssetRegistrationDetail"), multiTenancySides: MultiTenancySides.Tenant);
            assetRegistrationDetails.CreateChildPermission(AppPermissions.Pages_AssetRegistrationDetails_Delete, L("DeleteAssetRegistrationDetail"), multiTenancySides: MultiTenancySides.Tenant);

            var ledgerTypes = pages.CreateChildPermission(AppPermissions.Pages_LedgerTypes, L("LedgerTypes"), multiTenancySides: MultiTenancySides.Tenant);
            ledgerTypes.CreateChildPermission(AppPermissions.Pages_LedgerTypes_Create, L("CreateNewLedgerType"), multiTenancySides: MultiTenancySides.Tenant);
            ledgerTypes.CreateChildPermission(AppPermissions.Pages_LedgerTypes_Edit, L("EditLedgerType"), multiTenancySides: MultiTenancySides.Tenant);
            ledgerTypes.CreateChildPermission(AppPermissions.Pages_LedgerTypes_Delete, L("DeleteLedgerType"), multiTenancySides: MultiTenancySides.Tenant);

            var vwReqStatus2 = pages.CreateChildPermission(AppPermissions.Pages_VwReqStatus2, L("VwReqStatus2"), multiTenancySides: MultiTenancySides.Tenant);
            vwReqStatus2.CreateChildPermission(AppPermissions.Pages_VwReqStatus2_Create, L("CreateNewVwReqStatus2"), multiTenancySides: MultiTenancySides.Tenant);
            vwReqStatus2.CreateChildPermission(AppPermissions.Pages_VwReqStatus2_Edit, L("EditVwReqStatus2"), multiTenancySides: MultiTenancySides.Tenant);
            vwReqStatus2.CreateChildPermission(AppPermissions.Pages_VwReqStatus2_Delete, L("DeleteVwReqStatus2"), multiTenancySides: MultiTenancySides.Tenant);

            var vwRetQty = pages.CreateChildPermission(AppPermissions.Pages_VwRetQty, L("VwRetQty"), multiTenancySides: MultiTenancySides.Tenant);
            vwRetQty.CreateChildPermission(AppPermissions.Pages_VwRetQty_Create, L("CreateNewVwRetQty"), multiTenancySides: MultiTenancySides.Tenant);
            vwRetQty.CreateChildPermission(AppPermissions.Pages_VwRetQty_Edit, L("EditVwRetQty"), multiTenancySides: MultiTenancySides.Tenant);
            vwRetQty.CreateChildPermission(AppPermissions.Pages_VwRetQty_Delete, L("DeleteVwRetQty"), multiTenancySides: MultiTenancySides.Tenant);

            var vwReqStatus = pages.CreateChildPermission(AppPermissions.Pages_VwReqStatus, L("VwReqStatus"), multiTenancySides: MultiTenancySides.Tenant);
            vwReqStatus.CreateChildPermission(AppPermissions.Pages_VwReqStatus_Create, L("CreateNewVwReqStatus"), multiTenancySides: MultiTenancySides.Tenant);
            vwReqStatus.CreateChildPermission(AppPermissions.Pages_VwReqStatus_Edit, L("EditVwReqStatus"), multiTenancySides: MultiTenancySides.Tenant);
            vwReqStatus.CreateChildPermission(AppPermissions.Pages_VwReqStatus_Delete, L("DeleteVwReqStatus"), multiTenancySides: MultiTenancySides.Tenant);

            var postat = pages.CreateChildPermission(AppPermissions.Pages_POSTAT, L("POSTAT"), multiTenancySides: MultiTenancySides.Tenant);
            postat.CreateChildPermission(AppPermissions.Pages_POSTAT_Create, L("CreateNewPOSTAT"), multiTenancySides: MultiTenancySides.Tenant);
            postat.CreateChildPermission(AppPermissions.Pages_POSTAT_Edit, L("EditPOSTAT"), multiTenancySides: MultiTenancySides.Tenant);
            postat.CreateChildPermission(AppPermissions.Pages_POSTAT_Delete, L("DeletePOSTAT"), multiTenancySides: MultiTenancySides.Tenant);

            var gltrv = pages.CreateChildPermission(AppPermissions.Pages_GLTRV, L("GLTRV"), multiTenancySides: MultiTenancySides.Tenant);
            gltrv.CreateChildPermission(AppPermissions.Pages_GLTRV_Create, L("CreateNewGLTRV"), multiTenancySides: MultiTenancySides.Tenant);
            gltrv.CreateChildPermission(AppPermissions.Pages_GLTRV_Edit, L("EditGLTRV"), multiTenancySides: MultiTenancySides.Tenant);
            gltrv.CreateChildPermission(AppPermissions.Pages_GLTRV_Delete, L("DeleteGLTRV"), multiTenancySides: MultiTenancySides.Tenant);
            var reqStat = pages.CreateChildPermission(AppPermissions.Pages_ReqStat, L("ReqStat"), multiTenancySides: MultiTenancySides.Tenant);
            reqStat.CreateChildPermission(AppPermissions.Pages_ReqStat_Create, L("CreateNewReqStat"), multiTenancySides: MultiTenancySides.Tenant);
            reqStat.CreateChildPermission(AppPermissions.Pages_ReqStat_Edit, L("EditReqStat"), multiTenancySides: MultiTenancySides.Tenant);
            reqStat.CreateChildPermission(AppPermissions.Pages_ReqStat_Delete, L("DeleteReqStat"), multiTenancySides: MultiTenancySides.Tenant);

            var glDocRev = pages.CreateChildPermission(AppPermissions.Pages_GLDocRev, L("GLDocRev"), multiTenancySides: MultiTenancySides.Tenant);
            glDocRev.CreateChildPermission(AppPermissions.Pages_GLDocRev_Create, L("CreateNewGLDocRev"), multiTenancySides: MultiTenancySides.Tenant);
            glDocRev.CreateChildPermission(AppPermissions.Pages_GLDocRev_Edit, L("EditGLDocRev"), multiTenancySides: MultiTenancySides.Tenant);
            glDocRev.CreateChildPermission(AppPermissions.Pages_GLDocRev_Delete, L("DeleteGLDocRev"), multiTenancySides: MultiTenancySides.Tenant);

            var bankReconcileDetails = pages.CreateChildPermission(AppPermissions.Pages_BankReconcileDetails, L("BankReconcileDetails"), multiTenancySides: MultiTenancySides.Tenant);
            bankReconcileDetails.CreateChildPermission(AppPermissions.Pages_BankReconcileDetails_Create, L("CreateNewBankReconcileDetail"), multiTenancySides: MultiTenancySides.Tenant);
            bankReconcileDetails.CreateChildPermission(AppPermissions.Pages_BankReconcileDetails_Edit, L("EditBankReconcileDetail"), multiTenancySides: MultiTenancySides.Tenant);
            bankReconcileDetails.CreateChildPermission(AppPermissions.Pages_BankReconcileDetails_Delete, L("DeleteBankReconcileDetail"), multiTenancySides: MultiTenancySides.Tenant);

            var bankReconciles = pages.CreateChildPermission(AppPermissions.Pages_BankReconciles, L("BankReconciles"), multiTenancySides: MultiTenancySides.Tenant);
            bankReconciles.CreateChildPermission(AppPermissions.Pages_BankReconciles_Create, L("CreateNewBankReconcile"), multiTenancySides: MultiTenancySides.Tenant);
            bankReconciles.CreateChildPermission(AppPermissions.Pages_BankReconciles_Edit, L("EditBankReconcile"), multiTenancySides: MultiTenancySides.Tenant);
            bankReconciles.CreateChildPermission(AppPermissions.Pages_BankReconciles_Delete, L("DeleteBankReconcile"), multiTenancySides: MultiTenancySides.Tenant);

            //=====================Report Permissions=========================
            var reports = pages.CreateChildPermission(AppPermissions.Pages_Reports, L("Reports"));

            var supplyChainReports = reports.CreateChildPermission(AppPermissions.Reports_SupplyChainReports, L("SupplyChainReports"));
            var inventoryReports = supplyChainReports.CreateChildPermission(AppPermissions.SupplyChainReports_InventoryReports, L("InventoryReport"));
            inventoryReports.CreateChildPermission(AppPermissions.InventoryReport_ItemLedger, L("ItemLedger"));
            inventoryReports.CreateChildPermission(AppPermissions.InventoryReport_DocumentPrinting, L("DocumentPrinting"));
            inventoryReports.CreateChildPermission(AppPermissions.InventoryReport_Activity, L("Activity"));
            inventoryReports.CreateChildPermission(AppPermissions.InventoryReport_Consumption, L("Consumption"));
            inventoryReports.CreateChildPermission(AppPermissions.AssetRegistrationReports, L("AssetRegReports"));

            var saleReports = supplyChainReports.CreateChildPermission(AppPermissions.SupplyChainReports_SaleReports, L("SaleReports"));
            saleReports.CreateChildPermission(AppPermissions.SaleReports_DocumentPrinting, L("DocumentPrinting"));

            var financeReports = reports.CreateChildPermission(AppPermissions.Reports_FinanceReports, L("FinanceReports"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_Ledger, L("Ledger"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_TransactionListing, L("TransactionListing"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_BankReconcilation, L("BankReconcilation"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_PartyBalances, L("PartyBalances"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_TrialBalances, L("TrialBalances"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_ChartOfACListing, L("ChartOfACListing"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_VoucherPrinting, L("VoucherPrinting"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_CashBook, L("CashBook"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_BankBook, L("BankBook"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_SubledgerTrail, L("SubledgerTrail"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_TaxCollection, L("TaxCollection"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_LCExpenses, L("LCExpenses"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_ProfitAndLossStatement, L("ProfitAndLossStatement"));
            financeReports.CreateChildPermission(AppPermissions.FinanceReports_Setup, L("Setup"));

            var payRollReports = reports.CreateChildPermission(AppPermissions.Reports_PayRollReports, L("PayRollReports"));
            payRollReports.CreateChildPermission(AppPermissions.PayRollReports_AttendanceReports, L("AttendanceReports"));
            payRollReports.CreateChildPermission(AppPermissions.PayRollReports_EmployeeReports, L("EmployeeReports"));
            payRollReports.CreateChildPermission(AppPermissions.PayRollReports_SalaryReports, L("SalaryReports"));
            payRollReports.CreateChildPermission(AppPermissions.PayRollReports_SetupReports, L("SetupReports"));
            payRollReports.CreateChildPermission(AppPermissions.PayRollReports_AllowanceReports, L("AllowanceReports"));

            //=====================Pay Roll Permissions=========================
            var payRoll = pages.CreateChildPermission(AppPermissions.Pages_PayRoll, L("PayRoll"));
            var payrollSetup = payRoll.CreateChildPermission(AppPermissions.Pages_PayRoll_Setup, L("Setup"));
            var payrollTransactions = payRoll.CreateChildPermission(AppPermissions.Pages_PayRoll_Transactions, L("Transactions"));

            var Desig = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Designations, L("Designations"));
            Desig.CreateChildPermission(AppPermissions.PayRoll_Designations_Create, L("CreateNewDesignation"));
            Desig.CreateChildPermission(AppPermissions.PayRoll_Designations_Edit, L("EditDesignation"));
            Desig.CreateChildPermission(AppPermissions.PayRoll_Designations_Delete, L("DeleteDesignation"));

            var Religion = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Religions, L("Religions"));
            Religion.CreateChildPermission(AppPermissions.PayRoll_Religions_Create, L("CreateNewReligion"));
            Religion.CreateChildPermission(AppPermissions.PayRoll_Religions_Edit, L("EditReligion"));
            Religion.CreateChildPermission(AppPermissions.PayRoll_Religions_Delete, L("DeleteReligion"));

            var salaryLock = payrollSetup.CreateChildPermission(AppPermissions.Pages_SalaryLock, L("SalaryLock"), multiTenancySides: MultiTenancySides.Tenant);
            salaryLock.CreateChildPermission(AppPermissions.Pages_SalaryLock_Create, L("CreateNewSalaryLock"), multiTenancySides: MultiTenancySides.Tenant);
            salaryLock.CreateChildPermission(AppPermissions.Pages_SalaryLock_Edit, L("EditSalaryLock"), multiTenancySides: MultiTenancySides.Tenant);
            salaryLock.CreateChildPermission(AppPermissions.Pages_SalaryLock_Delete, L("DeleteSalaryLock"), multiTenancySides: MultiTenancySides.Tenant);

            var Dept = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Departments, L("Departments"));
            Dept.CreateChildPermission(AppPermissions.PayRoll_Departments_Create, L("CreateNewDepartment"));
            Dept.CreateChildPermission(AppPermissions.PayRoll_Departments_Edit, L("EditDepartment"));
            Dept.CreateChildPermission(AppPermissions.PayRoll_Departments_Delete, L("DeleteDepartment"));

            var cader = payrollSetup.CreateChildPermission(AppPermissions.Pages_Cader, L("Cader"), multiTenancySides: MultiTenancySides.Tenant);
            cader.CreateChildPermission(AppPermissions.Pages_Cader_Create, L("CreateNewCader"), multiTenancySides: MultiTenancySides.Tenant);
            cader.CreateChildPermission(AppPermissions.Pages_Cader_Edit, L("EditCader"), multiTenancySides: MultiTenancySides.Tenant);
            cader.CreateChildPermission(AppPermissions.Pages_Cader_Delete, L("DeleteCader"), multiTenancySides: MultiTenancySides.Tenant);

            var Sec = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Sections, L("Sections"));
            Sec.CreateChildPermission(AppPermissions.PayRoll_Sections_Create, L("CreateNewSection"));
            Sec.CreateChildPermission(AppPermissions.PayRoll_Sections_Edit, L("EditSection"));
            Sec.CreateChildPermission(AppPermissions.PayRoll_Sections_Delete, L("DeleteSection"));

            var Shift = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Shifts, L("Shifts"));
            Shift.CreateChildPermission(AppPermissions.PayRoll_Shifts_Create, L("CreateNewShift"));
            Shift.CreateChildPermission(AppPermissions.PayRoll_Shifts_Edit, L("EditShift"));
            Shift.CreateChildPermission(AppPermissions.PayRoll_Shifts_Delete, L("DeleteShift"));

            var Grades = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Grades, L("Grades"));
            Grades.CreateChildPermission(AppPermissions.PayRoll_Grades_Create, L("CreateNewGrade"));
            Grades.CreateChildPermission(AppPermissions.PayRoll_Grades_Edit, L("EditGrade"));
            Grades.CreateChildPermission(AppPermissions.PayRoll_Grades_Delete, L("DeleteGrade"));

            var Loac = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Locations, L("Locations"));
            Loac.CreateChildPermission(AppPermissions.PayRoll_Locations_Create, L("CreateNewLocation"));
            Loac.CreateChildPermission(AppPermissions.PayRoll_Locations_Edit, L("EditLocation"));
            Loac.CreateChildPermission(AppPermissions.PayRoll_Locations_Delete, L("DeleteLocation"));

            var EmpType = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_EmployeeType, L("EmployeeType"));
            EmpType.CreateChildPermission(AppPermissions.PayRoll_EmployeeType_Create, L("CreateNewEmployeeType"));
            EmpType.CreateChildPermission(AppPermissions.PayRoll_EmployeeType_Edit, L("EditEmployeeType"));
            EmpType.CreateChildPermission(AppPermissions.PayRoll_EmployeeType_Delete, L("DeleteEmployeeType"));

            var EmpEarnings = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_EmployeeEarnings, L("EmployeeEarnings"));
            EmpEarnings.CreateChildPermission(AppPermissions.PayRoll_EmployeeEarnings_Create, L("CreateNewEmployeeEarnings"));
            EmpEarnings.CreateChildPermission(AppPermissions.PayRoll_EmployeeEarnings_Edit, L("EditEmployeeEarnings"));
            EmpEarnings.CreateChildPermission(AppPermissions.PayRoll_EmployeeEarnings_Delete, L("DeleteEmployeeEarnings"));

            var EmpArrear = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_EmployeeArrears, L("EmployeeArrears"));
            EmpArrear.CreateChildPermission(AppPermissions.PayRoll_EmployeeArrears_Create, L("CreateNewEmployeeArrears"));
            EmpArrear.CreateChildPermission(AppPermissions.PayRoll_EmployeeArrears_Edit, L("EditEmployeeArrears"));
            EmpArrear.CreateChildPermission(AppPermissions.PayRoll_EmployeeArrears_Delete, L("DeleteEmployeeArrears"));

            var EmpLeave = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeaves, L("EmployeeLeaves"));
            EmpLeave.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeaves_Create, L("CreateNewEmployeeLeaves"));
            EmpLeave.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeaves_Edit, L("EditEmployeeLeaves"));
            EmpLeave.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeaves_Delete, L("DeleteEmployeeLeaves"));

            var EmpLeaveTotal = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeavesTotal, L("EmployeeLeavesTotal"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLeaveTotal.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeavesTotal_Create, L("CreateNewEmployeeLeavesTotal"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLeaveTotal.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeavesTotal_Edit, L("EditEmployeeLeavesTotal"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLeaveTotal.CreateChildPermission(AppPermissions.PayRoll_EmployeeLeavesTotal_Delete, L("DeleteEmployeeLeavesTotal"), multiTenancySides: MultiTenancySides.Tenant);

            var Emp = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Employees, L("Employees"));
            Emp.CreateChildPermission(AppPermissions.PayRoll_Employees_Create, L("CreateNewEmployees"));
            Emp.CreateChildPermission(AppPermissions.PayRoll_Employees_Edit, L("EditEmployees"));
            Emp.CreateChildPermission(AppPermissions.PayRoll_Employees_Delete, L("DeleteEmployees"));
            Emp.CreateChildPermission(AppPermissions.PayRoll_Employees_ShowLoader, L("ShowLoader"));

            var EmpDeduction = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_EmployeeDeductions, L("EmployeeDeductions"));
            EmpDeduction.CreateChildPermission(AppPermissions.PayRoll_EmployeeDeductions_Create, L("CreateNewEmployeeDeductions"));
            EmpDeduction.CreateChildPermission(AppPermissions.PayRoll_EmployeeDeductions_Edit, L("EditEmployeeDeductions"));
            EmpDeduction.CreateChildPermission(AppPermissions.PayRoll_EmployeeDeductions_Delete, L("DeleteEmployeeDeductions"));

            var EmpAdvance = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_EmployeeAdvances, L("EmployeeAdvances"));
            EmpAdvance.CreateChildPermission(AppPermissions.PayRoll_EmployeeAdvances_Create, L("CreateNewEmployeeAdvances"));
            EmpAdvance.CreateChildPermission(AppPermissions.PayRoll_EmployeeAdvances_Edit, L("EditEmployeeAdvances"));
            EmpAdvance.CreateChildPermission(AppPermissions.PayRoll_EmployeeAdvances_Delete, L("DeleteEmployeeAdvances"));

            var Edu = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Education, L("Education"));
            Edu.CreateChildPermission(AppPermissions.PayRoll_Education_Create, L("CreateNewEducation"));
            Edu.CreateChildPermission(AppPermissions.PayRoll_Education_Edit, L("EditEducation"));
            Edu.CreateChildPermission(AppPermissions.PayRoll_Education_Delete, L("DeleteEducation"));

            var EmpSalary = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_EmployeeSalary, L("EmployeeSalary"));
            EmpSalary.CreateChildPermission(AppPermissions.PayRoll_EmployeeSalary_Create, L("CreateNewEmployeeSalary"));
            EmpSalary.CreateChildPermission(AppPermissions.PayRoll_EmployeeSalary_Edit, L("EditEmployeeSalary"));
            EmpSalary.CreateChildPermission(AppPermissions.PayRoll_EmployeeSalary_Delete, L("DeleteEmployeeSalary"));
            EmpSalary.CreateChildPermission(AppPermissions.PayRoll_EmployeeSalary_ShowLoader, L("ShowLoader"));

            var cader_link_D = payrollSetup.CreateChildPermission(AppPermissions.Pages_Cader_link_D, L("Cader_link_D"), multiTenancySides: MultiTenancySides.Tenant);
            cader_link_D.CreateChildPermission(AppPermissions.Pages_Cader_link_D_Create, L("CreateNewCader_link_D"), multiTenancySides: MultiTenancySides.Tenant);
            cader_link_D.CreateChildPermission(AppPermissions.Pages_Cader_link_D_Edit, L("EditCader_link_D"), multiTenancySides: MultiTenancySides.Tenant);
            cader_link_D.CreateChildPermission(AppPermissions.Pages_Cader_link_D_Delete, L("DeleteCader_link_D"), multiTenancySides: MultiTenancySides.Tenant);

            var cader_link_H = payrollSetup.CreateChildPermission(AppPermissions.Pages_Cader_link_H, L("Cader_link_H"), multiTenancySides: MultiTenancySides.Tenant);
            cader_link_H.CreateChildPermission(AppPermissions.Pages_Cader_link_H_Create, L("CreateNewCader_link_H"), multiTenancySides: MultiTenancySides.Tenant);
            cader_link_H.CreateChildPermission(AppPermissions.Pages_Cader_link_H_Edit, L("EditCader_link_H"), multiTenancySides: MultiTenancySides.Tenant);
            cader_link_H.CreateChildPermission(AppPermissions.Pages_Cader_link_H_Delete, L("DeleteCader_link_H"), multiTenancySides: MultiTenancySides.Tenant);

            var AttendHeader = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_AttendanceHeader, L("AttendanceHeader"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceHeader_Create, L("CreateNewAttendanceHeader"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceHeader_Edit, L("EditAttendanceHeader"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceHeader_Delete, L("DeleteAttendanceHeader"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceHeader_Process, L("ProcessAttendanceHeader"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceDetail, L("AttendanceDetail"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceDetail_Create, L("CreateNewAttendanceDetail"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceDetail_Edit, L("EditAttendanceDetail"));
            AttendHeader.CreateChildPermission(AppPermissions.PayRoll_AttendanceDetail_Delete, L("DeleteAttendanceDetail"));

            var Attendence = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_Attendance, L("Attendance"));
            Attendence.CreateChildPermission(AppPermissions.PayRoll_Attendance_Create, L("CreateNewAttendance"));
            Attendence.CreateChildPermission(AppPermissions.PayRoll_Attendance_Edit, L("EditAttendance"));
            Attendence.CreateChildPermission(AppPermissions.PayRoll_Attendance_Delete, L("DeleteAttendance"));

            var monthlyCPR = payrollTransactions.CreateChildPermission(AppPermissions.Pages_MonthlyCPR, L("MonthlyCPR"), multiTenancySides: MultiTenancySides.Tenant);
            monthlyCPR.CreateChildPermission(AppPermissions.Pages_MonthlyCPR_Create, L("CreateNewMonthlyCPR"), multiTenancySides: MultiTenancySides.Tenant);
            monthlyCPR.CreateChildPermission(AppPermissions.Pages_MonthlyCPR_Edit, L("EditMonthlyCPR"), multiTenancySides: MultiTenancySides.Tenant);
            monthlyCPR.CreateChildPermission(AppPermissions.Pages_MonthlyCPR_Delete, L("DeleteMonthlyCPR"), multiTenancySides: MultiTenancySides.Tenant);

            var SalrySheet = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_SalarySheet, L("SalarySheet"));
            SalrySheet.CreateChildPermission(AppPermissions.PayRoll_SalarySheet_Create, L("CreateNewSalarySheet"));

            var DeductionType = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_DeductionTypes, L("DeductionTypes"));
            DeductionType.CreateChildPermission(AppPermissions.PayRoll_DeductionTypes_Create, L("CreateNewDeductionTypes"));
            DeductionType.CreateChildPermission(AppPermissions.PayRoll_DeductionTypes_Edit, L("EditDeductionTypes"));
            DeductionType.CreateChildPermission(AppPermissions.PayRoll_DeductionTypes_Delete, L("DeleteDeductionTypes"));

            var Earningtype = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_EarningTypes, L("EarningTypes"));
            Earningtype.CreateChildPermission(AppPermissions.PayRoll_EarningTypes_Create, L("CreateNewEarningTypes"));
            Earningtype.CreateChildPermission(AppPermissions.PayRoll_EarningTypes_Edit, L("EditEarningTypes"));
            Earningtype.CreateChildPermission(AppPermissions.PayRoll_EarningTypes_Delete, L("DeleteEarningTypes"));

            var Holiday = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_Holidays, L("Holidays"));
            Holiday.CreateChildPermission(AppPermissions.PayRoll_Holidays_Create, L("CreateNewHolidays"));
            Holiday.CreateChildPermission(AppPermissions.PayRoll_Holidays_Edit, L("EditHolidays"));
            Holiday.CreateChildPermission(AppPermissions.PayRoll_Holidays_Delete, L("DeleteHolidays"));

            var AllowanceSetup = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_AllowanceSetup, L("AllowanceSetup"));
            AllowanceSetup.CreateChildPermission(AppPermissions.PayRoll_AllowanceSetup_Create, L("CreateNewAllowanceSetup"));
            AllowanceSetup.CreateChildPermission(AppPermissions.PayRoll_AllowanceSetup_Edit, L("EditAllowanceSetup"));
            AllowanceSetup.CreateChildPermission(AppPermissions.PayRoll_AllowanceSetup_Delete, L("DeleteAllowanceSetup"));

            var Allowance = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_Allowances, L("Allowances"));
            Allowance.CreateChildPermission(AppPermissions.PayRoll_Allowances_Create, L("CreateNewAllowances"));
            Allowance.CreateChildPermission(AppPermissions.PayRoll_Allowances_Edit, L("EditAllowances"));
            Allowance.CreateChildPermission(AppPermissions.PayRoll_Allowances_Delete, L("DeleteAllowances"));

            var AllowancesDet = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_AllowancesDetails, L("AllowancesDetails"));
            AllowancesDet.CreateChildPermission(AppPermissions.PayRoll_AllowancesDetails_Create, L("CreateNewAllowancesDetail"));
            AllowancesDet.CreateChildPermission(AppPermissions.PayRoll_AllowancesDetails_Edit, L("EditAllowancesDetail"));
            AllowancesDet.CreateChildPermission(AppPermissions.PayRoll_AllowancesDetails_Delete, L("DeleteAllowancesDetail"));

            var SubDes = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_SubDesignations, L("SubDesignations"));
            SubDes.CreateChildPermission(AppPermissions.PayRoll_SubDesignations_Create, L("CreateNewSubDesignations"));
            SubDes.CreateChildPermission(AppPermissions.PayRoll_SubDesignations_Edit, L("EditSubDesignations"));
            SubDes.CreateChildPermission(AppPermissions.PayRoll_SubDesignations_Delete, L("DeleteSubDesignations"));

            var EmpLoan = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoans, L("EmployeeLoans"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLoan.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoans_Create, L("CreateNewEmployeeLoans"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLoan.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoans_Edit, L("EditEmployeeLoans"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLoan.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoans_Delete, L("DeleteEmployeeLoans"), multiTenancySides: MultiTenancySides.Tenant);

            var EmpLoanType = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoansType, L("EmployeeLoansType"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLoanType.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoansType_Create, L("CreateNewEmployeeLoansType"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLoanType.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoansType_Edit, L("EditEmployeeLoansType"), multiTenancySides: MultiTenancySides.Tenant);
            EmpLoanType.CreateChildPermission(AppPermissions.PayRoll_EmployeeLoansType_Delete, L("DeleteEmployeeLoansType"), multiTenancySides: MultiTenancySides.Tenant);

            var StopSalary = payrollTransactions.CreateChildPermission(AppPermissions.PayRoll_StopSalary, L("StopSalary"), multiTenancySides: MultiTenancySides.Tenant);
            StopSalary.CreateChildPermission(AppPermissions.PayRoll_StopSalary_Create, L("CreateNewStopSalary"), multiTenancySides: MultiTenancySides.Tenant);
            StopSalary.CreateChildPermission(AppPermissions.PayRoll_StopSalary_Edit, L("EditStopSalary"), multiTenancySides: MultiTenancySides.Tenant);
            StopSalary.CreateChildPermission(AppPermissions.PayRoll_StopSalary_Delete, L("DeleteStopSalary"), multiTenancySides: MultiTenancySides.Tenant);

            var SalbSetup = payrollSetup.CreateChildPermission(AppPermissions.PayRoll_SlabSetup, L("SlabSetup"), multiTenancySides: MultiTenancySides.Tenant);
            SalbSetup.CreateChildPermission(AppPermissions.PayRoll_SlabSetup_Create, L("CreateSlabSetup"), multiTenancySides: MultiTenancySides.Tenant);
            SalbSetup.CreateChildPermission(AppPermissions.PayRoll_SlabSetup_Edit, L("EditSlaSetup"), multiTenancySides: MultiTenancySides.Tenant);
            SalbSetup.CreateChildPermission(AppPermissions.PayRoll_SlabSetup_Delete, L("DeleteSlabSetup"), multiTenancySides: MultiTenancySides.Tenant);

            var adjH = payrollSetup.CreateChildPermission(AppPermissions.Pages_AdjH, L("AdjH"), multiTenancySides: MultiTenancySides.Tenant);
            adjH.CreateChildPermission(AppPermissions.Pages_AdjH_Create, L("CreateNewAdjH"), multiTenancySides: MultiTenancySides.Tenant);
            adjH.CreateChildPermission(AppPermissions.Pages_AdjH_Edit, L("EditAdjH"), multiTenancySides: MultiTenancySides.Tenant);
            adjH.CreateChildPermission(AppPermissions.Pages_AdjH_Delete, L("DeleteAdjH"), multiTenancySides: MultiTenancySides.Tenant);

            var employerBank = payrollSetup.CreateChildPermission(AppPermissions.Pages_EmployerBank, L("EmployerBank"), multiTenancySides: MultiTenancySides.Tenant);
            employerBank.CreateChildPermission(AppPermissions.Pages_EmployerBank_Create, L("CreateNewEmployerBank"), multiTenancySides: MultiTenancySides.Tenant);
            employerBank.CreateChildPermission(AppPermissions.Pages_EmployerBank_Edit, L("EditEmployerBank"), multiTenancySides: MultiTenancySides.Tenant);
            employerBank.CreateChildPermission(AppPermissions.Pages_EmployerBank_Delete, L("DeleteEmployerBank"), multiTenancySides: MultiTenancySides.Tenant);

            var hrmSetup = payrollSetup.CreateChildPermission(AppPermissions.Pages_HrmSetup, L("HrmSetup"), multiTenancySides: MultiTenancySides.Tenant);
            hrmSetup.CreateChildPermission(AppPermissions.Pages_HrmSetup_Create, L("CreateNewHrmSetup"), multiTenancySides: MultiTenancySides.Tenant);
            hrmSetup.CreateChildPermission(AppPermissions.Pages_HrmSetup_Edit, L("EditHrmSetup"), multiTenancySides: MultiTenancySides.Tenant);
            hrmSetup.CreateChildPermission(AppPermissions.Pages_HrmSetup_Delete, L("DeleteHrmSetup"), multiTenancySides: MultiTenancySides.Tenant);

            //=====================Supply Chain Permissions=========================
            var supplyChain = pages.CreateChildPermission(AppPermissions.Pages_SupplyChain, L("SupplyChain"));

            supplyChain.CreateChildPermission(AppPermissions.Transaction_Inventory_ShowAll, L("ShowAllTRV1"));
            //Purchase Permissions
            var purchase = supplyChain.CreateChildPermission(AppPermissions.SupplyChain_Purchase, L("Purchase"));

            var popoDetails = purchase.CreateChildPermission(AppPermissions.Purchase_POPODetails, L("POPODetails"));
            popoDetails.CreateChildPermission(AppPermissions.Purchase_POPODetails_Edit, L("EditPOPODetail"));

            var popoHeaders = purchase.CreateChildPermission(AppPermissions.Purchase_POPOHeaders, L("POPOHeaders"));
            popoHeaders.CreateChildPermission(AppPermissions.Purchase_POPOHeaders_Edit, L("EditPOPOHeader"));

            var purchaseOrder = purchase.CreateChildPermission(AppPermissions.Purchase_PurchaseOrders, L("PurchaseOrders"));
            purchaseOrder.CreateChildPermission(AppPermissions.Purchase_PurchaseOrders_Create, L("CreateNewPurchaseOrders"));
            purchaseOrder.CreateChildPermission(AppPermissions.Purchase_PurchaseOrders_Edit, L("EditPurchaseOrders"));
            purchaseOrder.CreateChildPermission(AppPermissions.Purchase_PurchaseOrders_Delete, L("DeletePurchaseOrders"));
            purchaseOrder.CreateChildPermission(AppPermissions.Purchase_PurchaseOrders_Print, L("PrintPurchaseOrders"));

            var requisitions = purchase.CreateChildPermission(AppPermissions.Purchase_Requisitions, L("Requisitions"));
            requisitions.CreateChildPermission(AppPermissions.Purchase_Requisitions_Create, L("CreateNewRequisition"));
            requisitions.CreateChildPermission(AppPermissions.Purchase_Requisitions_Edit, L("EditRequisition"));
            requisitions.CreateChildPermission(AppPermissions.Purchase_Requisitions_Delete, L("DeleteRequisition"));
            requisitions.CreateChildPermission(AppPermissions.Purchase_Requisitions_Print, L("PrintRequisition"));

            var poretDetails = purchase.CreateChildPermission(AppPermissions.Purchase_PORETDetails, L("PORETDetails"));
            poretDetails.CreateChildPermission(AppPermissions.Purchase_PORETDetails_Edit, L("EditPORETDetail"));

            var poretHeaders = purchase.CreateChildPermission(AppPermissions.Purchase_PORETHeaders, L("PORETHeaders"));
            poretHeaders.CreateChildPermission(AppPermissions.Purchase_PORETHeaders_Edit, L("EditPORETHeader"));

            var receiptReturn = purchase.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn, L("ReceiptReturn"));
            receiptReturn.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn_Create, L("CreateNewReceiptReturn"));
            receiptReturn.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn_Edit, L("EditReceiptReturn"));
            receiptReturn.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn_Delete, L("DeleteReceiptReturn"));
            receiptReturn.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn_Process, L("ProcessReceiptReturn"));
            receiptReturn.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn_Approve, L("ApproveReceiptReturn"));
            receiptReturn.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn_UnApprove, L("UnApproveReceiptReturn"));
            receiptReturn.CreateChildPermission(AppPermissions.Purchase_ReceiptReturn_Print, L("PrintReceiptReturn"));

            var porecDetails = purchase.CreateChildPermission(AppPermissions.Purchase_PORECDetails, L("PORECDetails"));
            porecDetails.CreateChildPermission(AppPermissions.Purchase_PORECDetails_Edit, L("EditPORECDetail"));

            var porecHeaders = purchase.CreateChildPermission(AppPermissions.Purchase_PORECHeaders, L("PORECHeaders"));
            porecHeaders.CreateChildPermission(AppPermissions.Purchase_PORECHeaders_Edit, L("EditPORECHeader"));

            var icrecaExps = purchase.CreateChildPermission(AppPermissions.Purchase_ICRECAExps, L("ICRECAExps"));
            icrecaExps.CreateChildPermission(AppPermissions.Purchase_ICRECAExps_Edit, L("EditICRECAExp"));

            var receiptEntry = purchase.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry, L("ReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_Create, L("CreateNewReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_Edit, L("EditReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_Delete, L("DeleteReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_Process, L("ProcessReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_Approve, L("ApproveReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_UnApprove, L("UnApproveReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_Print, L("PrintReceiptEntry"));
            receiptEntry.CreateChildPermission(AppPermissions.Purchase_ReceiptEntry_ShowAmounts, L("ShowAmounts"));

            var apinvh = purchase.CreateChildPermission(AppPermissions.Pages_APINVH, L("APINVH"), multiTenancySides: MultiTenancySides.Tenant);
            apinvh.CreateChildPermission(AppPermissions.Pages_APINVH_Create, L("CreateNewAPINVH"), multiTenancySides: MultiTenancySides.Tenant);
            apinvh.CreateChildPermission(AppPermissions.Pages_APINVH_Edit, L("EditAPINVH"), multiTenancySides: MultiTenancySides.Tenant);
            apinvh.CreateChildPermission(AppPermissions.Pages_APINVH_Delete, L("DeleteAPINVH"), multiTenancySides: MultiTenancySides.Tenant);

            //Inventory Permissions
            var inventory = supplyChain.CreateChildPermission(AppPermissions.SupplyChain_Inventory, L("Inventory"));

            supplyChain.CreateChildPermission(AppPermissions.SupplyChain_VoucherPosting, L("VoucherPosting"));
            supplyChain.CreateChildPermission(AppPermissions.SupplyChain_Approval, L("Approval"));

            var icSetups = inventory.CreateChildPermission(AppPermissions.Inventory_ICSetups, L("ICSetups"));
            icSetups.CreateChildPermission(AppPermissions.Inventory_ICSetups_Create, L("CreateNewICSetup"));
            icSetups.CreateChildPermission(AppPermissions.Inventory_ICSetups_Edit, L("EditICSetup"));
            icSetups.CreateChildPermission(AppPermissions.Inventory_ICSetups_Delete, L("DeleteICSetup"));

            var iclot = inventory.CreateChildPermission(AppPermissions.Pages_ICLOT, L("ICLOT"), multiTenancySides: MultiTenancySides.Tenant);
            iclot.CreateChildPermission(AppPermissions.Pages_ICLOT_Create, L("CreateNewICLOT"), multiTenancySides: MultiTenancySides.Tenant);
            iclot.CreateChildPermission(AppPermissions.Pages_ICLOT_Edit, L("EditICLOT"), multiTenancySides: MultiTenancySides.Tenant);
            iclot.CreateChildPermission(AppPermissions.Pages_ICLOT_Delete, L("DeleteICLOT"), multiTenancySides: MultiTenancySides.Tenant);

            var inventoryGlLinks = inventory.CreateChildPermission(AppPermissions.Inventory_InventoryGlLinks, L("InventoryGlLinks"));
            inventoryGlLinks.CreateChildPermission(AppPermissions.Inventory_InventoryGlLinks_Create, L("CreateNewInventoryGlLink"));
            inventoryGlLinks.CreateChildPermission(AppPermissions.Inventory_InventoryGlLinks_Edit, L("EditInventoryGlLink"));
            inventoryGlLinks.CreateChildPermission(AppPermissions.Inventory_InventoryGlLinks_Delete, L("DeleteInventoryGlLink"));

            var priceLists = inventory.CreateChildPermission(AppPermissions.Inventory_PriceLists, L("PriceLists"));
            priceLists.CreateChildPermission(AppPermissions.Inventory_PriceLists_Create, L("CreateNewPriceList"));
            priceLists.CreateChildPermission(AppPermissions.Inventory_PriceLists_Edit, L("EditPriceList"));
            priceLists.CreateChildPermission(AppPermissions.Inventory_PriceLists_Delete, L("DeletePriceList"));

            var costCenters = inventory.CreateChildPermission(AppPermissions.Inventory_CostCenters, L("CostCenters"));
            costCenters.CreateChildPermission(AppPermissions.Inventory_CostCenters_Create, L("CreateNewCostCenter"));
            costCenters.CreateChildPermission(AppPermissions.Inventory_CostCenters_Edit, L("EditCostCenter"));
            costCenters.CreateChildPermission(AppPermissions.Inventory_CostCenters_Delete, L("DeleteCostCenter"));

            var icuoMs = inventory.CreateChildPermission(AppPermissions.Inventory_ICUOMs, L("ICUOMs"));
            icuoMs.CreateChildPermission(AppPermissions.Inventory_ICUOMs_Create, L("CreateNewICUOM"));
            icuoMs.CreateChildPermission(AppPermissions.Inventory_ICUOMs_Edit, L("EditICUOM"));
            icuoMs.CreateChildPermission(AppPermissions.Inventory_ICUOMs_Delete, L("DeleteICUOM"));

            var iceLocation = inventory.CreateChildPermission(AppPermissions.Inventory_ICELocation, L("ICELocation"), multiTenancySides: MultiTenancySides.Tenant);
            inventory.CreateChildPermission(AppPermissions.Inventory_ICELocation_Create, L("CreateNewICELocation"), multiTenancySides: MultiTenancySides.Tenant);
            inventory.CreateChildPermission(AppPermissions.Inventory_ICELocation_Edit, L("EditICELocation"), multiTenancySides: MultiTenancySides.Tenant);
            inventory.CreateChildPermission(AppPermissions.Inventory_ICELocation_Delete, L("DeleteICELocation"), multiTenancySides: MultiTenancySides.Tenant);

            var icLocations = inventory.CreateChildPermission(AppPermissions.Inventory_ICLocations, L("ICLocations"));
            icLocations.CreateChildPermission(AppPermissions.Inventory_ICLocations_Create, L("CreateNewICLocation"));
            icLocations.CreateChildPermission(AppPermissions.Inventory_ICLocations_Edit, L("EditICLocation"));
            icLocations.CreateChildPermission(AppPermissions.Inventory_ICLocations_Delete, L("DeleteICLocation"));

            var reorderLevels = inventory.CreateChildPermission(AppPermissions.Inventory_ReorderLevels, L("ReorderLevels"));
            reorderLevels.CreateChildPermission(AppPermissions.Inventory_ReorderLevels_Create, L("CreateNewReorderLevel"));
            reorderLevels.CreateChildPermission(AppPermissions.Inventory_ReorderLevels_Edit, L("EditReorderLevel"));
            reorderLevels.CreateChildPermission(AppPermissions.Inventory_ReorderLevels_Delete, L("DeleteReorderLevel"));

            var transactionTypes = inventory.CreateChildPermission(AppPermissions.Inventory_TransactionTypes, L("TransactionTypes"));
            transactionTypes.CreateChildPermission(AppPermissions.Inventory_TransactionTypes_Create, L("CreateNewTransactionType"));
            transactionTypes.CreateChildPermission(AppPermissions.Inventory_TransactionTypes_Edit, L("EditTransactionType"));
            transactionTypes.CreateChildPermission(AppPermissions.Inventory_TransactionTypes_Delete, L("DeleteTransactionType"));

            var itemPricings = inventory.CreateChildPermission(AppPermissions.Inventory_ItemPricings, L("ItemPricings"));
            itemPricings.CreateChildPermission(AppPermissions.Inventory_ItemPricings_Create, L("CreateNewItemPricing"));
            itemPricings.CreateChildPermission(AppPermissions.Inventory_ItemPricings_Edit, L("EditItemPricing"));
            itemPricings.CreateChildPermission(AppPermissions.Inventory_ItemPricings_Delete, L("DeleteItemPricing"));

            var ICSegment1 = inventory.CreateChildPermission(AppPermissions.Inventory_ICSegment1, L("ICSegment1"));
            ICSegment1.CreateChildPermission(AppPermissions.Inventory_ICSegment1_Create, L("CreateNewICSegment1"));
            ICSegment1.CreateChildPermission(AppPermissions.Inventory_ICSegment1_Edit, L("EditICSegment1"));
            ICSegment1.CreateChildPermission(AppPermissions.Inventory_ICSegment1_Delete, L("DeleteICSegment1"));

            var ICSegment2 = inventory.CreateChildPermission(AppPermissions.Inventory_ICSegment2, L("ICSegment2"));
            ICSegment2.CreateChildPermission(AppPermissions.Inventory_ICSegment2_Create, L("CreateNewICSegment2"));
            ICSegment2.CreateChildPermission(AppPermissions.Inventory_ICSegment2_Edit, L("EditICSegment2"));
            ICSegment2.CreateChildPermission(AppPermissions.Inventory_ICSegment2_Delete, L("DeleteICSegment2"));

            var ICSegment3 = inventory.CreateChildPermission(AppPermissions.Inventory_ICSegment3, L("ICSegment3"));
            ICSegment3.CreateChildPermission(AppPermissions.Inventory_ICSegment3_Create, L("CreateNewICSegment3"));
            ICSegment3.CreateChildPermission(AppPermissions.Inventory_ICSegment3_Edit, L("EditICSegment3"));
            ICSegment3.CreateChildPermission(AppPermissions.Inventory_ICSegment3_Delete, L("DeleteICSegment3"));

            var ICItem = inventory.CreateChildPermission(AppPermissions.Inventory_ICItem, L("ICItems"));
            ICItem.CreateChildPermission(AppPermissions.Inventory_ICItem_Create, L("CreateNewIcItem"));
            ICItem.CreateChildPermission(AppPermissions.Inventory_ICItem_Edit, L("EditIcItem"));
            ICItem.CreateChildPermission(AppPermissions.Inventory_ICItem_Delete, L("DeleteIcItem"));

            var icopT5 = inventory.CreateChildPermission(AppPermissions.Inventory_ICOPT5, L("ICOPT5"));//, multiTenancySides: MultiTenancySides.Tenant);
            icopT5.CreateChildPermission(AppPermissions.Inventory_ICOPT5_Create, L("CreateNewICOPT5"));//, multiTenancySides: MultiTenancySides.Tenant);
            icopT5.CreateChildPermission(AppPermissions.Inventory_ICOPT5_Edit, L("EditICOPT5"));//, multiTenancySides: MultiTenancySides.Tenant);
            icopT5.CreateChildPermission(AppPermissions.Inventory_ICOPT5_Delete, L("DeleteICOPT5"));//, multiTenancySides: MultiTenancySides.Tenant);

            var icopT4 = inventory.CreateChildPermission(AppPermissions.Inventory_ICOPT4, L("ICOPT4"));//, multiTenancySides: MultiTenancySides.Tenant);
            icopT4.CreateChildPermission(AppPermissions.Inventory_ICOPT4_Create, L("CreateNewICOPT4"));//, multiTenancySides: MultiTenancySides.Tenant);
            icopT4.CreateChildPermission(AppPermissions.Inventory_ICOPT4_Edit, L("EditICOPT4"));//, multiTenancySides: MultiTenancySides.Tenant);
            icopT4.CreateChildPermission(AppPermissions.Inventory_ICOPT4_Delete, L("DeleteICOPT4"));//, multiTenancySides: MultiTenancySides.Tenant);

            var subCostCenters = inventory.CreateChildPermission(AppPermissions.Inventory_SubCostCenters, L("SubCostCenters"));
            subCostCenters.CreateChildPermission(AppPermissions.Inventory_SubCostCenters_Create, L("CreateNewSubCostCenter"));
            subCostCenters.CreateChildPermission(AppPermissions.Inventory_SubCostCenters_Edit, L("EditSubCostCenter"));
            subCostCenters.CreateChildPermission(AppPermissions.Inventory_SubCostCenters_Delete, L("DeleteSubCostCenter"));

            var gatePasses = inventory.CreateChildPermission(AppPermissions.Inventory_GatePasses, L("GatePasses"));
            gatePasses.CreateChildPermission(AppPermissions.Inventory_GatePasses_Create, L("CreateNewGatePass"));
            gatePasses.CreateChildPermission(AppPermissions.Inventory_GatePasses_Edit, L("EditGatePass"));
            gatePasses.CreateChildPermission(AppPermissions.Inventory_GatePasses_Delete, L("DeleteGatePass"));
            gatePasses.CreateChildPermission(AppPermissions.Inventory_InwardGatePasses_Create, L("CreateNewInwardGatePass"));
            gatePasses.CreateChildPermission(AppPermissions.Inventory_OutwardGatePasses_Create, L("CreateNewOutwardGatePass"));
            gatePasses.CreateChildPermission(AppPermissions.Inventory_InwardGatePasses_View, L("ViewInwardGatePass"));
            gatePasses.CreateChildPermission(AppPermissions.Inventory_OutwardGatePasses_View, L("ViewOutwardGatePass"));

            var transfers = inventory.CreateChildPermission(AppPermissions.Inventory_Transfers, L("Transfers"), multiTenancySides: MultiTenancySides.Tenant);
            transfers.CreateChildPermission(AppPermissions.Inventory_Transfers_Create, L("CreateNewTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            transfers.CreateChildPermission(AppPermissions.Inventory_Transfers_Edit, L("EditTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            transfers.CreateChildPermission(AppPermissions.Inventory_Transfers_Delete, L("DeleteTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            transfers.CreateChildPermission(AppPermissions.Inventory_Transfers_Print, L("PrintTransfer"), multiTenancySides: MultiTenancySides.Tenant);
            transfers.CreateChildPermission(AppPermissions.Inventory_Transfers_Process, L("Process"), multiTenancySides: MultiTenancySides.Tenant);
            transfers.CreateChildPermission(AppPermissions.Inventory_Transfers_Approve, L("Approve"), multiTenancySides: MultiTenancySides.Tenant);
            transfers.CreateChildPermission(AppPermissions.Inventory_Transfers_UnApprove, L("UnApprove"), multiTenancySides: MultiTenancySides.Tenant);

            var icadjDetails = inventory.CreateChildPermission(AppPermissions.Inventory_ICADJDetails, L("ICADJDetails"));
            icadjDetails.CreateChildPermission(AppPermissions.Inventory_ICADJDetails_Edit, L("EditICADJDetail"));

            var icadjHeaders = inventory.CreateChildPermission(AppPermissions.Inventory_ICAHeaders, L("ICAHeaders"));
            icadjHeaders.CreateChildPermission(AppPermissions.Inventory_ICAHeaders_Edit, L("EditICAHeaders"));

            var adjustments = inventory.CreateChildPermission(AppPermissions.Inventory_Adjustments, L("Adjustments"));
            adjustments.CreateChildPermission(AppPermissions.Inventory_Adjustments_Edit, L("EditAdjustment"));
            adjustments.CreateChildPermission(AppPermissions.Inventory_Adjustments_Create, L("CreateNewAdjustment"));
            adjustments.CreateChildPermission(AppPermissions.Inventory_Adjustments_Delete, L("DeleteAdjustment"));
            adjustments.CreateChildPermission(AppPermissions.Inventory_Adjustments_Print, L("PrintAdjustment"));
            adjustments.CreateChildPermission(AppPermissions.Inventory_Adjustments_Approve, L("Approve"));
            adjustments.CreateChildPermission(AppPermissions.Inventory_Adjustments_UnApprove, L("Unapprove"));

            var icwoHeaders = inventory.CreateChildPermission(AppPermissions.Inventory_ICWOHeaders, L("ICWOHeaders"), multiTenancySides: MultiTenancySides.Tenant);
            icwoHeaders.CreateChildPermission(AppPermissions.Inventory_ICWOHeaders_Edit, L("EditICWOHeader"), multiTenancySides: MultiTenancySides.Tenant);

            var icwoDetails = inventory.CreateChildPermission(AppPermissions.Inventory_ICWODetails, L("ICWODetails"), multiTenancySides: MultiTenancySides.Tenant);
            icwoDetails.CreateChildPermission(AppPermissions.Inventory_ICWODetails_Edit, L("EditICWODetailr"), multiTenancySides: MultiTenancySides.Tenant);

            var workOrder = inventory.CreateChildPermission(AppPermissions.Inventory_WorkOrder, L("WorkOrder"));
            workOrder.CreateChildPermission(AppPermissions.Inventory_WorkOrder_Edit, L("EditWorkOrder"));
            workOrder.CreateChildPermission(AppPermissions.Inventory_WorkOrder_Create, L("CreateNewWorkOrder"));
            workOrder.CreateChildPermission(AppPermissions.Inventory_WorkOrder_Delete, L("DeleteWorkOrder"));
            workOrder.CreateChildPermission(AppPermissions.Inventory_WorkOrder_Approve, L("Approve"));
            workOrder.CreateChildPermission(AppPermissions.Inventory_WorkOrder_UnApprove, L("Unapprove"));

            var iccnsDetails = inventory.CreateChildPermission(AppPermissions.Inventory_ICCNSDetails, L("ICCNSDetails"));
            iccnsDetails.CreateChildPermission(AppPermissions.Inventory_ICCNSDetails_Edit, L("EditICCNSDetail"));

            var iccnsHeaders = inventory.CreateChildPermission(AppPermissions.Inventory_ICCNSHeaders, L("ICCNSHeaders"));
            iccnsHeaders.CreateChildPermission(AppPermissions.Inventory_ICCNSHeaders_Edit, L("EditICCNSHeader"));

            var consumptions = inventory.CreateChildPermission(AppPermissions.Inventory_Consumptions, L("Consumptions"));
            consumptions.CreateChildPermission(AppPermissions.Inventory_Consumptions_Create, L("CreateNewConsumption"));
            consumptions.CreateChildPermission(AppPermissions.Inventory_Consumptions_Edit, L("EditConsumption"));
            consumptions.CreateChildPermission(AppPermissions.Inventory_Consumptions_Delete, L("DeleteConsumption"));
            consumptions.CreateChildPermission(AppPermissions.Inventory_Consumptions_Print, L("PrintConsumption"));
            consumptions.CreateChildPermission(AppPermissions.Inventory_Consumptions_Process, L("Process"));
            consumptions.CreateChildPermission(AppPermissions.Inventory_Consumptions_Approve, L("Approve"));
            consumptions.CreateChildPermission(AppPermissions.Inventory_Consumptions_UnApprove, L("Unapprove"));

            var icledg = inventory.CreateChildPermission(AppPermissions.Inventory_ICLEDG, L("ICLEDG"), multiTenancySides: MultiTenancySides.Tenant);
            icledg.CreateChildPermission(AppPermissions.Inventory_ICLEDG_Create, L("CreateNewICLEDG"), multiTenancySides: MultiTenancySides.Tenant);
            icledg.CreateChildPermission(AppPermissions.Inventory_ICLEDG_Edit, L("EditICLEDG"), multiTenancySides: MultiTenancySides.Tenant);
            icledg.CreateChildPermission(AppPermissions.Inventory_ICLEDG_Delete, L("DeleteICLEDG"), multiTenancySides: MultiTenancySides.Tenant);

            var icopnDetails = inventory.CreateChildPermission(AppPermissions.Inventory_ICOPNDetails, L("ICOPNDetails"));
            icopnDetails.CreateChildPermission(AppPermissions.Inventory_ICOPNDetails_Edit, L("EditICOPNDetail"));

            var icopnHeaders = inventory.CreateChildPermission(AppPermissions.Inventory_ICOPNHeaders, L("ICOPNHeaders"));
            icopnHeaders.CreateChildPermission(AppPermissions.Inventory_ICOPNHeaders_Edit, L("EditICOPNHeader"));

            var openings = inventory.CreateChildPermission(AppPermissions.Inventory_Openings, L("Openings"));
            openings.CreateChildPermission(AppPermissions.Inventory_Openings_Create, L("CreateNewOpening"));
            openings.CreateChildPermission(AppPermissions.Inventory_Openings_Edit, L("EditOpening"));
            openings.CreateChildPermission(AppPermissions.Inventory_Openings_Delete, L("DeleteOpening"));
            openings.CreateChildPermission(AppPermissions.Inventory_Openings_Print, L("PrintOpening"));
            openings.CreateChildPermission(AppPermissions.Inventory_Openings_Approve, L("Approve"));
            openings.CreateChildPermission(AppPermissions.Inventory_Openings_UnApprove, L("Unapprove"));
            openings.CreateChildPermission(AppPermissions.Inventory_Openings_ShowLoader, L("ShowLoader"));

            var assemblies = inventory.CreateChildPermission(AppPermissions.Inventory_Assemblies, L("Assemblies"), multiTenancySides: MultiTenancySides.Tenant);
            assemblies.CreateChildPermission(AppPermissions.Inventory_Assemblies_Create, L("CreateNewAssembly"), multiTenancySides: MultiTenancySides.Tenant);
            assemblies.CreateChildPermission(AppPermissions.Inventory_Assemblies_Edit, L("EditAssembly"), multiTenancySides: MultiTenancySides.Tenant);
            assemblies.CreateChildPermission(AppPermissions.Inventory_Assemblies_Delete, L("DeleteAssembly"), multiTenancySides: MultiTenancySides.Tenant);

            var iC_UNITs = inventory.CreateChildPermission(AppPermissions.Inventory_IC_UNITs, L("IC_UNITs"), multiTenancySides: MultiTenancySides.Tenant);
            iC_UNITs.CreateChildPermission(AppPermissions.Inventory_IC_UNITs_Create, L("CreateNewIC_UNIT"), multiTenancySides: MultiTenancySides.Tenant);
            iC_UNITs.CreateChildPermission(AppPermissions.Inventory_IC_UNITs_Edit, L("EditIC_UNIT"), multiTenancySides: MultiTenancySides.Tenant);
            iC_UNITs.CreateChildPermission(AppPermissions.Inventory_IC_UNITs_Delete, L("DeleteIC_UNIT"), multiTenancySides: MultiTenancySides.Tenant);

            var assetRegistration = inventory.CreateChildPermission(AppPermissions.Pages_AssetRegistration, L("AssetRegistration"), multiTenancySides: MultiTenancySides.Tenant);
            assetRegistration.CreateChildPermission(AppPermissions.Pages_AssetRegistration_Create, L("CreateNewAssetRegistration"), multiTenancySides: MultiTenancySides.Tenant);
            assetRegistration.CreateChildPermission(AppPermissions.Pages_AssetRegistration_Edit, L("EditAssetRegistration"), multiTenancySides: MultiTenancySides.Tenant);
            assetRegistration.CreateChildPermission(AppPermissions.Pages_AssetRegistration_Delete, L("DeleteAssetRegistration"), multiTenancySides: MultiTenancySides.Tenant);

            //=====================Sale Permissions=========================

            var sales = supplyChain.CreateChildPermission(AppPermissions.SupplyChain_Sales, L("Sales"));

            var oeRoutes = sales.CreateChildPermission(AppPermissions.Sales_OERoutes, L("OERoutes"), multiTenancySides: MultiTenancySides.Tenant);
            oeRoutes.CreateChildPermission(AppPermissions.Sales_OERoutes_Create, L("CreateNewOERoutes"), multiTenancySides: MultiTenancySides.Tenant);
            oeRoutes.CreateChildPermission(AppPermissions.Sales_OERoutes_Edit, L("EditOERoutes"), multiTenancySides: MultiTenancySides.Tenant);
            oeRoutes.CreateChildPermission(AppPermissions.Sales_OERoutes_Delete, L("DeleteOERoutes"), multiTenancySides: MultiTenancySides.Tenant);

            var saleAccounts = sales.CreateChildPermission(AppPermissions.Sales_SaleAccounts, L("SaleAccounts"));
            saleAccounts.CreateChildPermission(AppPermissions.Sales_SaleAccounts_Create, L("CreateNewOECOLL"));
            saleAccounts.CreateChildPermission(AppPermissions.Sales_SaleAccounts_Edit, L("EditOECOLL"));
            saleAccounts.CreateChildPermission(AppPermissions.Sales_SaleAccounts_Delete, L("DeleteOECOLL"));

            var oesaleDetails = sales.CreateChildPermission(AppPermissions.Sales_OESALEDetails, L("OESALEDetails"));
            oesaleDetails.CreateChildPermission(AppPermissions.Sales_OESALEDetails_Edit, L("EditOESALEDetail"));

            var oesaleHeaders = sales.CreateChildPermission(AppPermissions.Sales_OESALEHeaders, L("OESALEHeaders"));
            oesaleHeaders.CreateChildPermission(AppPermissions.Sales_OESALEHeaders_Edit, L("EditOESALEHeader"));

            var saleEntry = sales.CreateChildPermission(AppPermissions.Sales_SaleEntry, L("SaleEntry"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_Create, L("CreateNewSaleEntry"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_Edit, L("EditSaleEntry"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_Delete, L("DeleteSaleEntry"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_Print, L("PrintSaleEntry"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_Process, L("ProcessSaleEntry"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_Approve, L("Approve"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_UnApprove, L("Unapprove"));
            saleEntry.CreateChildPermission(AppPermissions.Sales_SaleEntry_ShowLoader, L("ShowLoader"));

            var oeqh = sales.CreateChildPermission(AppPermissions.Pages_OEQH, L("OEQH"), multiTenancySides: MultiTenancySides.Tenant);
            oeqh.CreateChildPermission(AppPermissions.Pages_OEQH_Create, L("CreateNewOEQH"), multiTenancySides: MultiTenancySides.Tenant);
            oeqh.CreateChildPermission(AppPermissions.Pages_OEQH_Edit, L("EditOEQH"), multiTenancySides: MultiTenancySides.Tenant);
            oeqh.CreateChildPermission(AppPermissions.Pages_OEQH_Delete, L("DeleteOEQH"), multiTenancySides: MultiTenancySides.Tenant);

            var oeqd = sales.CreateChildPermission(AppPermissions.Pages_OEQD, L("OEQD"), multiTenancySides: MultiTenancySides.Tenant);
            oeqd.CreateChildPermission(AppPermissions.Pages_OEQD_Create, L("CreateNewOEQD"), multiTenancySides: MultiTenancySides.Tenant);
            oeqd.CreateChildPermission(AppPermissions.Pages_OEQD_Edit, L("EditOEQD"), multiTenancySides: MultiTenancySides.Tenant);
            oeqd.CreateChildPermission(AppPermissions.Pages_OEQD_Delete, L("DeleteOEQD"), multiTenancySides: MultiTenancySides.Tenant);

            var creditDebitNotes = sales.CreateChildPermission(AppPermissions.Sales_CreditDebitNotes, L("CreditDebitNotes"), multiTenancySides: MultiTenancySides.Tenant);
            creditDebitNotes.CreateChildPermission(AppPermissions.Sales_CreditNotes, L("CreditNote"), multiTenancySides: MultiTenancySides.Tenant);
            creditDebitNotes.CreateChildPermission(AppPermissions.Sales_DebitNotes, L("DebitNote"), multiTenancySides: MultiTenancySides.Tenant);
            creditDebitNotes.CreateChildPermission(AppPermissions.Sales_CreditDebitNotes_Create, L("CreateNewCreditDebitNote"), multiTenancySides: MultiTenancySides.Tenant);
            creditDebitNotes.CreateChildPermission(AppPermissions.Sales_CreditDebitNotes_Edit, L("EditCreditDebitNote"), multiTenancySides: MultiTenancySides.Tenant);
            creditDebitNotes.CreateChildPermission(AppPermissions.Sales_CreditDebitNotes_Delete, L("DeleteCreditDebitNote"), multiTenancySides: MultiTenancySides.Tenant);
            creditDebitNotes.CreateChildPermission(AppPermissions.Sales_CreditDebitNotes_Process, L("Process"), multiTenancySides: MultiTenancySides.Tenant);

            var oeretDetails = sales.CreateChildPermission(AppPermissions.Sales_OERETDetails, L("OERETDetails"));
            oeretDetails.CreateChildPermission(AppPermissions.Sales_OERETDetails_Edit, L("EditOERETDetail"));

            var oeretHeaders = sales.CreateChildPermission(AppPermissions.Sales_OERETHeaders, L("OERETHeaders"));
            oeretHeaders.CreateChildPermission(AppPermissions.Sales_OERETHeaders_Edit, L("EditOERETHeader"));

            var saleReturn = sales.CreateChildPermission(AppPermissions.Sales_SaleReturn, L("SaleReturn"));
            saleReturn.CreateChildPermission(AppPermissions.Sales_SaleReturn_Create, L("CreateNewSaleReturn"));
            saleReturn.CreateChildPermission(AppPermissions.Sales_SaleReturn_Edit, L("EditSaleReturn"));
            saleReturn.CreateChildPermission(AppPermissions.Sales_SaleReturn_Print, L("PrintSaleReturn"));
            saleReturn.CreateChildPermission(AppPermissions.Sales_SaleReturn_Delete, L("DeleteSaleReturn"));
            saleReturn.CreateChildPermission(AppPermissions.Sales_SaleReturn_Process, L("ProcessSaleReturn"));
            saleReturn.CreateChildPermission(AppPermissions.Sales_SaleReturn_Approve, L("ApproveSaleReturn"));
            saleReturn.CreateChildPermission(AppPermissions.Sales_SaleReturn_UnApprove, L("UnApproveSaleReturn"));

            var salesReferences = sales.CreateChildPermission(AppPermissions.Sales_SalesReferences, L("SalesReferences"));
            salesReferences.CreateChildPermission(AppPermissions.Sales_SalesReferences_Create, L("CreateNewSalesReference"));
            salesReferences.CreateChildPermission(AppPermissions.Sales_SalesReferences_Edit, L("EditSalesReference"));
            salesReferences.CreateChildPermission(AppPermissions.Sales_SalesReferences_Delete, L("DeleteSalesReference"));

            var oeinvknockd = pages.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKD, L("OEINVKNOCKD"), multiTenancySides: MultiTenancySides.Tenant);
            oeinvknockd.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKD_Create, L("CreateNewOEINVKNOCKD"), multiTenancySides: MultiTenancySides.Tenant);
            oeinvknockd.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKD_Edit, L("EditOEINVKNOCKD"), multiTenancySides: MultiTenancySides.Tenant);
            oeinvknockd.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKD_Delete, L("DeleteOEINVKNOCKD"), multiTenancySides: MultiTenancySides.Tenant);

            var oeinvknockh = pages.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKH, L("OEINVKNOCKH"));
            oeinvknockh.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKH_Create, L("CreateNewOEINVKNOCKH"));
            oeinvknockh.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKH_Edit, L("EditOEINVKNOCKH"));
            oeinvknockh.CreateChildPermission(AppPermissions.Pages_OEINVKNOCKH_Delete, L("DeleteOEINVKNOCKH"));

            //=====================Supply Chain Permissions=========================

            //Finance Permisiions
            var glOptions = pages.CreateChildPermission(AppPermissions.Pages_GLOptions, L("GLOptions"));
            glOptions.CreateChildPermission(AppPermissions.Pages_GLOptions_Create, L("CreateNewGLOption"));
            glOptions.CreateChildPermission(AppPermissions.Pages_GLOptions_Edit, L("EditGLOption"));
            glOptions.CreateChildPermission(AppPermissions.Pages_GLOptions_Delete, L("DeleteGLOption"));

            pages.CreateChildPermission(AppPermissions.Pages_APTransactionList, L("APTransactionList"));
            pages.CreateChildPermission(AppPermissions.Pages_PartyBalances, L("PartyBalances"));
            pages.CreateChildPermission(AppPermissions.Pages_TrialBalances, L("TrialBalances"));

            // pages.CreateChildPermission(AppPermissions.Pages_ARTransactionList, L("ARTransactionList"));

            reports.CreateChildPermission(AppPermissions.Pages_LedgerReport, L("LedgerReport"));
            reports.CreateChildPermission(AppPermissions.Pages_VoucherPringReport, L("VoucherpringReport"));
            reports.CreateChildPermission(AppPermissions.Pages_ReportView, L("ReportView"));

            var financial = reports.CreateChildPermission(AppPermissions.Reports_Financial, L("FinancialReports"));
            financial.CreateChildPermission(AppPermissions.Financial_ChartOfAccListing, L("ChartOfACListingReport"));
            financial.CreateChildPermission(AppPermissions.Financial_GeneralLedger, L("GeneralLedgerReport"));

            var arOptions = pages.CreateChildPermission(AppPermissions.Pages_AROptions, L("AROptions"));
            arOptions.CreateChildPermission(AppPermissions.Pages_AROptions_Create, L("CreateNewAROption"));
            arOptions.CreateChildPermission(AppPermissions.Pages_AROptions_Edit, L("EditAROption"));
            arOptions.CreateChildPermission(AppPermissions.Pages_AROptions_Delete, L("DeleteAROption"));

            var apOptions = pages.CreateChildPermission(AppPermissions.Pages_APOptions, L("APOptions"));
            apOptions.CreateChildPermission(AppPermissions.Pages_APOptions_Create, L("CreateNewAPOption"));
            apOptions.CreateChildPermission(AppPermissions.Pages_APOptions_Edit, L("EditAPOption"));
            apOptions.CreateChildPermission(AppPermissions.Pages_APOptions_Delete, L("DeleteAPOption"));

            var glinvDetails = pages.CreateChildPermission(AppPermissions.Pages_GLINVDetails, L("GLINVDetails"));
            glinvDetails.CreateChildPermission(AppPermissions.Pages_GLINVDetails_Edit, L("EditGLINVDetails"));
            var glinvHeaders = pages.CreateChildPermission(AppPermissions.Pages_GLINVHeaders, L("GLINVHeaders"));
            glinvHeaders.CreateChildPermission(AppPermissions.Pages_GLINVHeaders_Edit, L("EditGLINVHeaders"));
            var directInvoice = pages.CreateChildPermission(AppPermissions.Pages_DirectInvoice, L("DirectInvoice"));
            directInvoice.CreateChildPermission(AppPermissions.Pages_DirectInvoice_Create, L("CreateNewDirectInvoice"));
            directInvoice.CreateChildPermission(AppPermissions.Pages_DirectInvoice_Edit, L("EditDirectInvoice"));
            directInvoice.CreateChildPermission(AppPermissions.Pages_DirectInvoice_Delete, L("DeleteDirectInvoice"));
            directInvoice.CreateChildPermission(AppPermissions.Pages_DirectInvoice_PaymentProcess, L("ProcessPayment"));
            directInvoice.CreateChildPermission(AppPermissions.Pages_DirectInvoice_StockProcess, L("ProcessStock"));
            directInvoice.CreateChildPermission(AppPermissions.Pages_DirectInvoice_UpdateCpr, L("UpdateCpr"));

            var taxClasses = pages.CreateChildPermission(AppPermissions.Pages_TaxClasses, L("TaxClasses"));
            taxClasses.CreateChildPermission(AppPermissions.Pages_TaxClasses_Create, L("CreateNewTaxClass"));
            taxClasses.CreateChildPermission(AppPermissions.Pages_TaxClasses_Edit, L("EditTaxClass"));
            taxClasses.CreateChildPermission(AppPermissions.Pages_TaxClasses_Delete, L("DeleteTaxClass")/*, multiTenancySides: MultiTenancySides.Tenant*/);

            var gltrHeader = pages.CreateChildPermission(AppPermissions.Pages_GLTRHeaders, L("GLTRHeaders"));
            gltrHeader.CreateChildPermission(AppPermissions.Pages_GLTRHeaders_Create, L("CreateNewGLTRHeaders"));
            gltrHeader.CreateChildPermission(AppPermissions.Pages_GLTRHeaders_Edit, L("EditGLTRHeaders"));
            gltrHeader.CreateChildPermission(AppPermissions.Pages_GLTRHeaders_Delete, L("DeleteGLTRHeaders"));

            var gltrDetails = pages.CreateChildPermission(AppPermissions.Pages_GLTRDetails, L("GLTRDetails"));
            gltrDetails.CreateChildPermission(AppPermissions.Pages_GLTRDetails_Create, L("CreateNewGLTRDetails"));
            gltrDetails.CreateChildPermission(AppPermissions.Pages_GLTRDetails_Edit, L("EditGLTRDetails"));
            gltrDetails.CreateChildPermission(AppPermissions.Pages_GLTRDetails_Delete, L("DeleteGLTRDetails"));

            var fiscalCalenders = pages.CreateChildPermission(AppPermissions.Pages_FiscalCalenders, L("FiscalCalenders"));
            fiscalCalenders.CreateChildPermission(AppPermissions.Pages_FiscalCalenders_Create, L("CreateNewFiscalCalender"));
            fiscalCalenders.CreateChildPermission(AppPermissions.Pages_FiscalCalenders_Edit, L("EditFiscalCalender"));
            fiscalCalenders.CreateChildPermission(AppPermissions.Pages_FiscalCalenders_Delete, L("DeleteFiscalCalender"));
            var csUserLocH = pages.CreateChildPermission(AppPermissions.Pages_CSUserLocH, L("CSUserLocH"), multiTenancySides: MultiTenancySides.Tenant);
            csUserLocH.CreateChildPermission(AppPermissions.Pages_CSUserLocH_Create, L("CreateNewCSUserLocH"), multiTenancySides: MultiTenancySides.Tenant);
            csUserLocH.CreateChildPermission(AppPermissions.Pages_CSUserLocH_Edit, L("EditCSUserLocH"), multiTenancySides: MultiTenancySides.Tenant);
            csUserLocH.CreateChildPermission(AppPermissions.Pages_CSUserLocH_Delete, L("DeleteCSUserLocH"), multiTenancySides: MultiTenancySides.Tenant);

            var csUserLocD = pages.CreateChildPermission(AppPermissions.Pages_CSUserLocD, L("CSUserLocD"), multiTenancySides: MultiTenancySides.Tenant);
            csUserLocD.CreateChildPermission(AppPermissions.Pages_CSUserLocD_Create, L("CreateNewCSUserLocD"), multiTenancySides: MultiTenancySides.Tenant);
            csUserLocD.CreateChildPermission(AppPermissions.Pages_CSUserLocD_Edit, L("EditCSUserLocD"), multiTenancySides: MultiTenancySides.Tenant);
            csUserLocD.CreateChildPermission(AppPermissions.Pages_CSUserLocD_Delete, L("DeleteCSUserLocD"), multiTenancySides: MultiTenancySides.Tenant);

            var accountsPostings = pages.CreateChildPermission(AppPermissions.Pages_AccountsPostings, L("AccountsPostings"));
            accountsPostings.CreateChildPermission(AppPermissions.Pages_AccountsPostings_Create, L("CreateNewAccountsPosting"));
            accountsPostings.CreateChildPermission(AppPermissions.Pages_AccountsPostings_Edit, L("EditAccountsPosting"));
            accountsPostings.CreateChildPermission(AppPermissions.Pages_AccountsPostings_Delete, L("DeleteAccountsPosting"));
            accountsPostings.CreateChildPermission(AppPermissions.Pages_AccountsApproval_Create, L("CreateNewAccountsApproval"));
            accountsPostings.CreateChildPermission(AppPermissions.Pages_AccountsUnApproval_Create, L("CreateNewAccountsUnApproval"));

            var BatchListPreviews = pages.CreateChildPermission(AppPermissions.Pages_BatchListPreviews, L("BatchListPreviews"));
            BatchListPreviews.CreateChildPermission(AppPermissions.Pages_BatchListPreviews_Create, L("CreateNewBatchListPreview"));
            BatchListPreviews.CreateChildPermission(AppPermissions.Pages_BatchListPreviews_Edit, L("EditBatchListPreview"));
            BatchListPreviews.CreateChildPermission(AppPermissions.Pages_BatchListPreviews_Delete, L("DeleteBatchListPreview"));

            var glconfig = pages.CreateChildPermission(AppPermissions.Pages_GLCONFIG, L("GLCONFIG"));
            glconfig.CreateChildPermission(AppPermissions.Pages_GLCONFIG_Create, L("CreateNewGLCONFIG"));
            glconfig.CreateChildPermission(AppPermissions.Pages_GLCONFIG_Edit, L("EditGLCONFIG"));
            glconfig.CreateChildPermission(AppPermissions.Pages_GLCONFIG_Delete, L("DeleteGLCONFIG"));

            var glbooks = pages.CreateChildPermission(AppPermissions.Pages_GLBOOKS, L("GLBOOKS"));
            glbooks.CreateChildPermission(AppPermissions.Pages_GLBOOKS_Create, L("CreateNewGLBOOKS"));
            glbooks.CreateChildPermission(AppPermissions.Pages_GLBOOKS_Edit, L("EditGLBOOKS"));
            glbooks.CreateChildPermission(AppPermissions.Pages_GLBOOKS_Delete, L("DeleteGLBOOKS"));

            var taxAuthorities = pages.CreateChildPermission(AppPermissions.Pages_TaxAuthorities, L("TaxAuthorities"));
            taxAuthorities.CreateChildPermission(AppPermissions.Pages_TaxAuthorities_Create, L("CreateNewTaxAuthority"));
            taxAuthorities.CreateChildPermission(AppPermissions.Pages_TaxAuthorities_Edit, L("EditTaxAuthority"));
            taxAuthorities.CreateChildPermission(AppPermissions.Pages_TaxAuthorities_Delete, L("DeleteTaxAuthority"));

            var bkTransfers = pages.CreateChildPermission(AppPermissions.Pages_BkTransfers, L("BkTransfers"));
            bkTransfers.CreateChildPermission(AppPermissions.Pages_BkTransfers_Create, L("CreateNewBkTransfer"));
            bkTransfers.CreateChildPermission(AppPermissions.Pages_BkTransfers_Edit, L("EditBkTransfer"));
            bkTransfers.CreateChildPermission(AppPermissions.Pages_BkTransfers_Delete, L("DeleteBkTransfer"));

            // bank

            var banks = pages.CreateChildPermission(AppPermissions.Pages_Banks, L("Banks"));
            banks.CreateChildPermission(AppPermissions.Pages_Banks_Create, L("CreateNewBank"));
            banks.CreateChildPermission(AppPermissions.Pages_Banks_Edit, L("EditBank"));
            banks.CreateChildPermission(AppPermissions.Pages_Banks_Delete, L("DeleteBank"));

            //CPR
            var cpr = pages.CreateChildPermission(AppPermissions.Pages_CPR, L("CPR"));
            cpr.CreateChildPermission(AppPermissions.Pages_CPR_Create, L("CreateNewCPR"));
            cpr.CreateChildPermission(AppPermissions.Pages_CPR_Edit, L("EditCPR"));
            cpr.CreateChildPermission(AppPermissions.Pages_CPR_Delete, L("DeleteCPR"));

            //terms

            var apterms = pages.CreateChildPermission(AppPermissions.Pages_APTerms, L("APTerms"));
            apterms.CreateChildPermission(AppPermissions.Pages_APTerms_Create, L("CreateNewAPTerm"));
            apterms.CreateChildPermission(AppPermissions.Pages_APTerms_Edit, L("EditAPTerm"));
            apterms.CreateChildPermission(AppPermissions.Pages_APTerms_Delete, L("DeleteAPTerm"));

            var accountSubLedgers = pages.CreateChildPermission(AppPermissions.SetupForms_AccountSubLedgers, L("AccountSubLedgers"));
            accountSubLedgers.CreateChildPermission(AppPermissions.SetupForms_AccountSubLedgers_Create, L("CreateNewAccountSubLedger"));
            accountSubLedgers.CreateChildPermission(AppPermissions.SetupForms_AccountSubLedgers_Edit, L("EditAccountSubLedger"));
            accountSubLedgers.CreateChildPermission(AppPermissions.SetupForms_AccountSubLedgers_Delete, L("DeleteAccountSubLedger"));

            var GeneralLedger = pages.CreateChildPermission(AppPermissions.Pages_GeneralLedger, L("GeneralLedger"));
            var SetupForms = GeneralLedger.CreateChildPermission(AppPermissions.GeneralLedger_SetupForms, L("SetupForms"));
            var transaction = GeneralLedger.CreateChildPermission(AppPermissions.GeneralLedger_Transaction, L("Transaction"));

            SetupForms.CreateChildPermission(AppPermissions.SetupForms_VendorMaster, L("VendorMaster"));
            SetupForms.CreateChildPermission(AppPermissions.SetupForms_CustomerMaster, L("CustomerMaster"));

            var voucherEntry = transaction.CreateChildPermission(AppPermissions.Transactions_VoucherEntry, L("VoucherEntry"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_Create, L("CreateNewVoucherEntry"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_Edit, L("EditVoucherEntry"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_Print, L("PrintVoucherEntry"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_Delete, L("DeleteVoucherEntry"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_Approve, L("Approve"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_UnApprove, L("Unapprove"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_Post, L("Post"));
            voucherEntry.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_ShowLoader, L("ShowLoader"));

            var voucherPermits = transaction.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_VoucherPermits, L("VoucherPermit"));
            voucherPermits.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_BP, L("BankPayment"));
            voucherPermits.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_BR, L("BankReceipt"));
            voucherPermits.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_CP, L("CashPayment"));
            voucherPermits.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_CR, L("CashReceipt"));
            voucherPermits.CreateChildPermission(AppPermissions.Transaction_VoucherEntry_ShowAllTRV, L("ShowAllTRV"));

            var jvEntry = transaction.CreateChildPermission(AppPermissions.Transactions_JVEntry, L("JVEntry"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_Create, L("CreateNewJVEntry"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_Edit, L("EditJVEntry"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_Print, L("PrintJVEntry"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_Delete, L("DeleteJVEntry"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_Approve, L("Approve"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_UnApprove, L("Unapprove"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_Post, L("Post"));
            jvEntry.CreateChildPermission(AppPermissions.Transaction_JVEntry_ShowLoader, L("ShowLoader"));

            var voucherReversal = SetupForms.CreateChildPermission(AppPermissions.SetupForms_voucher_reversal, L("VoucherReversal"));
            voucherReversal.CreateChildPermission(AppPermissions.SetupForms_voucher_reversal_Create, L("CreateVoucherReversal"));
            voucherReversal.CreateChildPermission(AppPermissions.SetupForms_voucher_reversal_Edit, L("EditVoucherReversal"));
            voucherReversal.CreateChildPermission(AppPermissions.SetupForms_voucher_reversal_Delete, L("DeleteVoucherReversal"));
            voucherReversal.CreateChildPermission(AppPermissions.SetupForms_voucher_reversal_Process, L("ProcessVoucherReversal"));

            var bankReconcile = transaction.CreateChildPermission(AppPermissions.Transactions_BankReconcile, L("BankReconcile"));
            bankReconcile.CreateChildPermission(AppPermissions.Transaction_BankReconcile_Create, L("CreateNewBankReconcile"));
            bankReconcile.CreateChildPermission(AppPermissions.Transaction_BankReconcile_Edit, L("EditBankReconcile"));
            bankReconcile.CreateChildPermission(AppPermissions.Transaction_BankReconcile_Delete, L("DeleteBankReconcile"));
            bankReconcile.CreateChildPermission(AppPermissions.Transaction_BankReconcile_Process, L("ProcessBankReconcile"));

            var glReconHeaders = transaction.CreateChildPermission(AppPermissions.Transaction_GLReconHeaders, L("GLReconH"));
            glReconHeaders.CreateChildPermission(AppPermissions.Transaction_GLReconHeaders_Edit, L("EditGLReconH"));

            var glReconDetails = transaction.CreateChildPermission(AppPermissions.Transaction_GLReconDetails, L("GLReconD"));
            glReconDetails.CreateChildPermission(AppPermissions.Transaction_GLReconDetails_Edit, L("EditGLReconD"));

            var currencyRates = SetupForms.CreateChildPermission(AppPermissions.SetupForms_CurrencyRates, L("CurrencyRates"));
            currencyRates.CreateChildPermission(AppPermissions.SetupForms_CurrencyRates_Create, L("CreateNewCurrencyRate"));
            currencyRates.CreateChildPermission(AppPermissions.SetupForms_CurrencyRates_Edit, L("EditCurrencyRate"));
            currencyRates.CreateChildPermission(AppPermissions.SetupForms_CurrencyRates_Delete, L("DeleteCurrencyRate"));

            var glLocations = SetupForms.CreateChildPermission(AppPermissions.SetupForms_GLLocations, L("GLLocations"));
            glLocations.CreateChildPermission(AppPermissions.SetupForms_GLLocations_Create, L("CreateNewGLLocation"));
            glLocations.CreateChildPermission(AppPermissions.SetupForms_GLLocations_Edit, L("EditGLLocation"));
            glLocations.CreateChildPermission(AppPermissions.SetupForms_GLLocations_Delete, L("DeleteGLLocation"));

            var glslGroups = SetupForms.CreateChildPermission(AppPermissions.Pages_GLSLGroups, L("GLSLGroups"), multiTenancySides: MultiTenancySides.Tenant);
            glslGroups.CreateChildPermission(AppPermissions.Pages_GLSLGroups_Create, L("CreateNewGLSLGroups"), multiTenancySides: MultiTenancySides.Tenant);
            glslGroups.CreateChildPermission(AppPermissions.Pages_GLSLGroups_Edit, L("EditGLSLGroups"), multiTenancySides: MultiTenancySides.Tenant);
            glslGroups.CreateChildPermission(AppPermissions.Pages_GLSLGroups_Delete, L("DeleteGLSLGroups"), multiTenancySides: MultiTenancySides.Tenant);

            var chartofControls = SetupForms.CreateChildPermission(AppPermissions.SetupForms_ChartofControls, L("ChartofControls"));
            chartofControls.CreateChildPermission(AppPermissions.SetupForms_ChartofControls_Create, L("CreateNewChartofControl"));
            chartofControls.CreateChildPermission(AppPermissions.SetupForms_ChartofControls_Edit, L("EditChartofControl"));
            chartofControls.CreateChildPermission(AppPermissions.SetupForms_ChartofControls_Delete, L("DeleteChartofControl"));

            var segmentlevel3s = SetupForms.CreateChildPermission(AppPermissions.SetupForms_Segmentlevel3s, L("Segmentlevel3s"));
            segmentlevel3s.CreateChildPermission(AppPermissions.SetupForms_Segmentlevel3s_Create, L("CreateNewSegmentlevel3"));
            segmentlevel3s.CreateChildPermission(AppPermissions.SetupForms_Segmentlevel3s_Edit, L("EditSegmentlevel3"));
            segmentlevel3s.CreateChildPermission(AppPermissions.SetupForms_Segmentlevel3s_Delete, L("DeleteSegmentlevel3"));

            var subControlDetails = SetupForms.CreateChildPermission(AppPermissions.SetupForms_SubControlDetails, L("SubControlDetails"));
            subControlDetails.CreateChildPermission(AppPermissions.SetupForms_SubControlDetails_Create, L("CreateNewSubControlDetail"));
            subControlDetails.CreateChildPermission(AppPermissions.SetupForms_SubControlDetails_Edit, L("EditSubControlDetail"));
            subControlDetails.CreateChildPermission(AppPermissions.SetupForms_SubControlDetails_Delete, L("DeleteSubControlDetail"));

            var controlDetails = SetupForms.CreateChildPermission(AppPermissions.SetupForms_ControlDetails, L("ControlDetails"));
            controlDetails.CreateChildPermission(AppPermissions.SetupForms_ControlDetails_Create, L("CreateNewControlDetail"));
            controlDetails.CreateChildPermission(AppPermissions.SetupForms_ControlDetails_Edit, L("EditControlDetail"));
            controlDetails.CreateChildPermission(AppPermissions.SetupForms_ControlDetails_Delete, L("DeleteControlDetail"));

            var groupCodes = SetupForms.CreateChildPermission(AppPermissions.SetupForms_GroupCodes, L("GroupCodes"));
            groupCodes.CreateChildPermission(AppPermissions.SetupForms_GroupCodes_Create, L("CreateNewGroupCode"));
            groupCodes.CreateChildPermission(AppPermissions.SetupForms_GroupCodes_Edit, L("EditGroupCode"));
            groupCodes.CreateChildPermission(AppPermissions.SetupForms_GroupCodes_Delete, L("DeleteGroupCode"));

            var groupCategories = SetupForms.CreateChildPermission(AppPermissions.SetupForms_GroupCategories, L("GroupCategories"));
            groupCategories.CreateChildPermission(AppPermissions.SetupForms_GroupCategories_Create, L("CreateNewGroupCategory"));
            groupCategories.CreateChildPermission(AppPermissions.SetupForms_GroupCategories_Edit, L("EditGroupCategory"));
            groupCategories.CreateChildPermission(AppPermissions.SetupForms_GroupCategories_Delete, L("DeleteGroupCategory"));

            var fiscalCalendars = SetupForms.CreateChildPermission(AppPermissions.SetupForms_FiscalCalendars, L("FiscalCalendars"));
            fiscalCalendars.CreateChildPermission(AppPermissions.SetupForms_FiscalCalendars_Create, L("CreateNewFiscalCalendar"));
            fiscalCalendars.CreateChildPermission(AppPermissions.SetupForms_FiscalCalendars_Edit, L("EditFiscalCalendar"));
            fiscalCalendars.CreateChildPermission(AppPermissions.SetupForms_FiscalCalendars_Delete, L("DeleteFiscalCalendar"));

            var csfsc = SetupForms.CreateChildPermission(AppPermissions.SetupForms_CSFSC, L("CSFSC"));
            csfsc.CreateChildPermission(AppPermissions.SetupForms_CSFSC_Create, L("CreateNewCSFSC"));
            csfsc.CreateChildPermission(AppPermissions.SetupForms_CSFSC_Edit, L("EditCSFSC"));
            csfsc.CreateChildPermission(AppPermissions.SetupForms_CSFSC_Delete, L("DeleteCSFSC"));

            var companyProfiles = SetupForms.CreateChildPermission(AppPermissions.SetupForms_CompanyProfiles, L("CompanyProfiles"));
            companyProfiles.CreateChildPermission(AppPermissions.SetupForms_CompanyProfiles_Create, L("CreateNewCompanyProfile"));
            companyProfiles.CreateChildPermission(AppPermissions.SetupForms_CompanyProfiles_Edit, L("EditCompanyProfile"));
            companyProfiles.CreateChildPermission(AppPermissions.SetupForms_CompanyProfiles_Delete, L("DeleteCompanyProfile"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ShowLoader, L("ShowLoader"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            //TENANT-SPECIFIC PERMISSIONS

            var dashboard = pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);
            var statics = dashboard.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_Statics, L("Statics"), multiTenancySides: MultiTenancySides.Tenant);
            var charts = dashboard.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_Chart, L("Chart"), multiTenancySides: MultiTenancySides.Tenant);

            // Statics // 

            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_Cash, L("Cash"));
            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_Bank, L("Bank"));
            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_PostDatedChequeRecieved, L("PostDatedChequeRecieved"));
            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_PostDatedChequeIssue, L("PostDatedChequeIssue"));
            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_BankOverDraft, L("BankOverDraft"));
            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_StockBalances, L("StockBalances"));
            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_RecieveAble, L("RecieveAble"));
            statics.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_PayAble, L("PayAble"));
            // Chart //

            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_CashChart, L("CashChart"));
            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_BankChart, L("BankChart"));
            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_PostDatedChequeRecievedChart, L("PostDatedChequeRecievedChart"));
            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_PostDatedChequeIssueChart, L("PostDatedChequeIssueChart"));
            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_BankOverDraftChart, L("BankOverDraftChart"));
            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_StockBalancesChart, L("StockBalancesChart"));
            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_RecieveAbleChart, L("RecieveAbleChart"));
            charts.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard_PayAbleChart, L("PayAbleChart"));
            // Chart //

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ERPConsts.LocalizationSourceName);
        }
    }
}