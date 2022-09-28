using ERP.GeneralLedger.SetupForms;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Exporting;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Configuration;
using ERP.Authorization.Users;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH
{
    //[AbpAuthorize(AppPermissions.Pages_GLTRHeaders)]
    public class GLTRHeadersAppService : ERPAppServiceBase, IGLTRHeadersAppService
    {
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IGLTRHeadersExcelExporter _gltrHeadersExcelExporter;
        private readonly IRepository<GLCONFIG> _lookup_glconfigRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _lookup_glsubledgerRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<GLBOOKS> _glBooksRepository;
        private readonly IRepository<AccountsPosting> _accountsPostingRepository;
        private readonly VoucherEntryAppService _voucherentryAppservice;

        public GLTRHeadersAppService(IRepository<GLTRHeader> gltrHeaderRepository, IGLTRHeadersExcelExporter gltrHeadersExcelExporter, IRepository<GLCONFIG> lookup_glconfigRepository,
            IRepository<AccountSubLedger> lookup_glsubledgerRepository, IRepository<AccountsPosting> accountsPostingRepository,
            IRepository<GLTRDetail> gltrDetailRepository, IRepository<ChartofControl, string> chartofControlRepository, IRepository<GLLocation> glLocationRepository, IRepository<GLBOOKS> glBooksRepository,
            VoucherEntryAppService voucherentryAppservice)
        {
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrHeadersExcelExporter = gltrHeadersExcelExporter;
            _lookup_glconfigRepository = lookup_glconfigRepository;
            _lookup_glsubledgerRepository = lookup_glsubledgerRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _glLocationRepository = glLocationRepository;
            _glBooksRepository = glBooksRepository;
            _accountsPostingRepository = accountsPostingRepository;
            _voucherentryAppservice = voucherentryAppservice;
        }
        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }
        public async Task<IQueryable<Permission>> GetUserPermissions(User user)
        {
            var permissions = await UserManager.GetGrantedPermissionsAsync(user);

            return permissions.AsQueryable();
        }
        public async Task<PagedResultDto<GetGLTRHeaderForViewDto>> GetAll(GetAllGLTRHeadersInput input)
        {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;

            var usrPermissions = await GetUserPermissions(currUser);

            var filterBooks = "";
            IQueryable<GLTRHeader> filteredGLTRHeaders;
            //var allBooks = _voucherentryAppservice.GetBooksDetails(input.transactionVoucher, "");
            //var allowedBooks = from b in allBooks.AsQueryable()
            //                   join up in usrPermissions on new { a = b.BookID } equals new { a = up.Name.LastIndexOf(".") }
            //                   select new
            //                   {
            //                       b.BookID
            //                   };

            List<string> allowedBooks = new List<string>();

            Dictionary<string, string> permits = new Dictionary<string, string>
            {

                [AppPermissions.Transaction_VoucherEntry_BR] = "BR",
                [AppPermissions.Transaction_VoucherEntry_BP] = "BP",
                [AppPermissions.Transaction_VoucherEntry_CR] = "CR",
                [AppPermissions.Transaction_VoucherEntry_CP] = "CP"
            };

            foreach (var p in permits)
            {
                if (usrPermissions.Any(x => x.Name == p.Key))
                {
                    allowedBooks.Add(p.Value);
                }
            }

            if (userName == "admin" || usrPermissions.Any(x => x.Name == AppPermissions.Transaction_VoucherEntry_ShowAllTRV))
            {

                if (input.BookIDFilter != "JV" && !input.transactionVoucher)
                {
                    filterBooks = "JV";
                    filteredGLTRHeaders = _gltrHeaderRepository.GetAll().Join(_glBooksRepository.GetAll().Where(x => x.Integrated == input.transactionVoucher && x.NormalEntry != 3), header => new { header.BookID, header.TenantId }, book => new { book.BookID, book.TenantId }, (header, book) => header)
                          //.Include(e => e.GLCONFIGFk)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.NARRATION.Contains(input.Filter) ||
                          // e.AccountID.Contains(input.Filter) || e.AccountName.Contains(input.Filter) ||
                          e.AuditUser.Contains(input.Filter) || e.LocId.Equals(input.Filter))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                          .WhereIf(input.LocationFilter != 0, e => e.LocId == input.LocationFilter)
                          .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                          .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                          .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                          .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                          .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                          .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                          .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                          .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.NARRATIONFilter), e => e.NARRATION.ToLower() == input.NARRATIONFilter.ToLower().Trim())
                          .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                          .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                          // .WhereIf(!string.IsNullOrWhiteSpace(input.AccountDescFilter), e => e.AccountName.ToLower() == input.AccountDescFilter.ToLower().Trim())
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                          .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                          .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                          .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
                          .Where(o => o.Posted == false)
                          .Where(o => o.TenantId == AbpSession.TenantId).Where(o => o.BookID != filterBooks && o.BookID.IsIn(allowedBooks.ToArray()));
                    //.WhereIf(!string.IsNullOrWhiteSpace(input.GLCONFIGConfigIDFilter), e => e.GLCONFIGFk != null && e.GLCONFIGFk.ConfigID.ToLower() == input.GLCONFIGConfigIDFilter.ToLower().Trim());
                }
                else if (input.BookIDFilter != "JV" && input.transactionVoucher)
                {
                    filterBooks = "JV";
                    filteredGLTRHeaders = _gltrHeaderRepository.GetAll().Join(_glBooksRepository.GetAll().Where(x => x.Integrated == input.transactionVoucher), header => new { header.BookID, header.TenantId }, book => new { book.BookID, book.TenantId }, (header, book) => header)
                          //.Include(e => e.GLCONFIGFk)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) ||
                          //e.AccountID.Contains(input.Filter) || e.AccountName.Contains(input.Filter) || 
                          e.NARRATION.Contains(input.Filter) || e.AuditUser.Contains(input.Filter) || e.LocId.Equals(input.Filter))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                          .WhereIf(input.LocationFilter != 0, e => e.LocId == input.LocationFilter)
                          .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                          .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                          .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                          .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                          .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                          .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                          .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                          .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.AccountDescFilter), e => e.AccountName.ToLower() == input.AccountDescFilter.ToLower().Trim())
                          // .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                          .WhereIf(!string.IsNullOrWhiteSpace(input.NARRATIONFilter), e => e.NARRATION.ToLower() == input.NARRATIONFilter.ToLower().Trim())
                          .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                          .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                          .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                          .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
                          .Where(o => o.Posted == false)
                          .Where(o => o.TenantId == AbpSession.TenantId).Where(o => o.BookID != filterBooks);
                    //.WhereIf(!string.IsNullOrWhiteSpace(input.GLCONFIGConfigIDFilter), e => e.GLCONFIGFk != null && e.GLCONFIGFk.ConfigID.ToLower() == input.GLCONFIGConfigIDFilter.ToLower().Trim());
                }
                else
                {
                    if (input.BookIDFilter == "JV")
                    {
                        input.BookIDFilter = "";
                    }
                    filteredGLTRHeaders = _gltrHeaderRepository.GetAll().Join(_glBooksRepository.GetAll().Where(x => x.Integrated ==
                    input.transactionVoucher && x.NormalEntry == 3), header => new { header.BookID, header.TenantId }, book => new
                    { book.BookID, book.TenantId }, (header, book) => header)
                               //.Include(e => e.GLCONFIGFk)
                               .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) ||
                               e.NARRATION.Contains(input.Filter) ||
                               //|| e.AccountID.Contains(input.Filter) ||
                               //e.AccountName.Contains(input.Filter) ||
                               e.AuditUser.Contains(input.Filter) || e.LocId.Equals(input.Filter))
                               .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                               .WhereIf(input.LocationFilter != 0, e => e.LocId == input.LocationFilter)
                               .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                               .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                               .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                               .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                               .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                               .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                               .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                               .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                               .WhereIf(!string.IsNullOrWhiteSpace(input.NARRATIONFilter), e => e.NARRATION.ToLower() == input.NARRATIONFilter.ToLower().Trim())
                               .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                               .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                               .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                               .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                               .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                               //.WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
                               .Where(o => o.Posted == false)
                               .Where(o => o.TenantId == AbpSession.TenantId)/*.Where(o => o.BookID != filterBooks)*/;
                    //.WhereIf(!string.IsNullOrWhiteSpace(input.GLCONFIGConfigIDFilter), e => e.GLCONFIGFk != null && e.GLCONFIGFk.ConfigID.ToLower() == input.GLCONFIGConfigIDFilter.ToLower().Trim());
                }
            }
            ///////////////////////////else condition for non-admin user ///////////////////
            else
            {
                if (input.BookIDFilter != "JV" && !input.transactionVoucher)
                {
                    filterBooks = "JV";
                    filteredGLTRHeaders = _gltrHeaderRepository.GetAll().Where(e => e.CreatedBy == userName).Join(_glBooksRepository.GetAll().Where(x => x.Integrated == input.transactionVoucher && x.NormalEntry != 3), header => new { header.BookID, header.TenantId }, book => new { book.BookID, book.TenantId }, (header, book) => header)
                          //.Include(e => e.GLCONFIGFk)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.NARRATION.Contains(input.Filter) ||
                          // e.AccountID.Contains(input.Filter) || e.AccountName.Contains(input.Filter) ||
                          e.AuditUser.Contains(input.Filter) || e.LocId.Equals(input.Filter))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                          .WhereIf(input.LocationFilter != 0, e => e.LocId == input.LocationFilter)
                          .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                          .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                          .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                          .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                          .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                          .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                          .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                          .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.NARRATIONFilter), e => e.NARRATION.ToLower() == input.NARRATIONFilter.ToLower().Trim())
                          .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                          .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                          // .WhereIf(!string.IsNullOrWhiteSpace(input.AccountDescFilter), e => e.AccountName.ToLower() == input.AccountDescFilter.ToLower().Trim())
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                          .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                          .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                          .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
                          .Where(o => o.Posted == false)
                          .Where(o => o.TenantId == AbpSession.TenantId).Where(o => o.BookID != filterBooks);
                    //.WhereIf(!string.IsNullOrWhiteSpace(input.GLCONFIGConfigIDFilter), e => e.GLCONFIGFk != null && e.GLCONFIGFk.ConfigID.ToLower() == input.GLCONFIGConfigIDFilter.ToLower().Trim());
                }
                else if (input.BookIDFilter != "JV" && input.transactionVoucher)
                {
                    filterBooks = "JV";
                    filteredGLTRHeaders = _gltrHeaderRepository.GetAll().Where(e => e.CreatedBy == userName).Join(_glBooksRepository.GetAll().Where(x => x.Integrated == input.transactionVoucher), header => new { header.BookID, header.TenantId }, book => new { book.BookID, book.TenantId }, (header, book) => header)
                          //.Include(e => e.GLCONFIGFk)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) ||
                          //e.AccountID.Contains(input.Filter) || e.AccountName.Contains(input.Filter) || 
                          e.NARRATION.Contains(input.Filter) || e.AuditUser.Contains(input.Filter) || e.LocId.Equals(input.Filter))
                          .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                          .WhereIf(input.LocationFilter != 0, e => e.LocId == input.LocationFilter)
                          .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                          .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                          .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                          .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                          .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                          .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                          .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                          .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.AccountDescFilter), e => e.AccountName.ToLower() == input.AccountDescFilter.ToLower().Trim())
                          // .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                          .WhereIf(!string.IsNullOrWhiteSpace(input.NARRATIONFilter), e => e.NARRATION.ToLower() == input.NARRATIONFilter.ToLower().Trim())
                          .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                          .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                          .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                          .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                          .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                          //.WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
                          .Where(o => o.Posted == false)
                          .Where(o => o.TenantId == AbpSession.TenantId).Where(o => o.BookID != filterBooks);
                    //.WhereIf(!string.IsNullOrWhiteSpace(input.GLCONFIGConfigIDFilter), e => e.GLCONFIGFk != null && e.GLCONFIGFk.ConfigID.ToLower() == input.GLCONFIGConfigIDFilter.ToLower().Trim());
                }
                else
                {
                    if (input.BookIDFilter == "JV")
                    {
                        input.BookIDFilter = "";
                    }
                    filteredGLTRHeaders = _gltrHeaderRepository.GetAll().Where(e => e.CreatedBy == userName).Join(_glBooksRepository.GetAll().Where(x => x.Integrated ==
                    input.transactionVoucher && x.NormalEntry == 3), header => new { header.BookID, header.TenantId }, book => new
                    { book.BookID, book.TenantId }, (header, book) => header)
                               //.Include(e => e.GLCONFIGFk)
                               .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) ||
                               e.NARRATION.Contains(input.Filter) ||
                               //|| e.AccountID.Contains(input.Filter) ||
                               //e.AccountName.Contains(input.Filter) ||
                               e.AuditUser.Contains(input.Filter) || e.LocId.Equals(input.Filter))
                               .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                               .WhereIf(input.LocationFilter != 0, e => e.LocId == input.LocationFilter)
                               .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                               .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                               .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                               .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                               .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                               .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                               .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                               .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                               .WhereIf(!string.IsNullOrWhiteSpace(input.NARRATIONFilter), e => e.NARRATION.ToLower() == input.NARRATIONFilter.ToLower().Trim())
                               .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                               .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                               .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                               .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                               .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                               //.WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
                               .Where(o => o.Posted == false)
                               .Where(o => o.TenantId == AbpSession.TenantId)/*.Where(o => o.BookID != filterBooks)*/;
                    //.WhereIf(!string.IsNullOrWhiteSpace(input.GLCONFIGConfigIDFilter), e => e.GLCONFIGFk != null && e.GLCONFIGFk.ConfigID.ToLower() == input.GLCONFIGConfigIDFilter.ToLower().Trim());
                }

            }



            var pagedAndFilteredGLTRHeaders = filteredGLTRHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);


            IQueryable<GetGLTRHeaderForViewDto> gltrHeaders = null;

            //if (input.transactionVoucher == true)
            //{
            //    gltrHeaders = from o in pagedAndFilteredGLTRHeaders
            //                  join o1 in _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) on o.LocId equals o1.LocId into j1
            //                  from s1 in j1.DefaultIfEmpty()
            //                  select new GetGLTRHeaderForViewDto()
            //                  {
            //                      GLTRHeader = new GLTRHeaderDto
            //                      {
            //                          BookID = o.BookID,
            //                          ConfigID = o.ConfigID,
            //                          //AccountID=o.AccountID,
            //                          //AccountDesc=o.AccountName,
            //                          DocNo = o.DocNo,
            //                          DocMonth = o.DocMonth,
            //                          fmtDocNo = o.FmtDocNo,
            //                          DocDate = o.DocDate,
            //                          NARRATION = o.NARRATION,
            //                          Amount = o.Amount.Value,
            //                          ChType = o.ChType,
            //                          ChNumber = o.ChNumber,
            //                          Approved = o.Approved,
            //                          Posted = o.Posted,
            //                          AuditUser = o.AuditUser,
            //                          AuditTime = o.AuditTime,
            //                          //OldCode = o.OldCode,
            //                          LocId = o.LocId,
            //                          LocDesc = s1 != null ? s1.LocDesc.ToString() : "",
            //                          CreatedBy = o.CreatedBy,
            //                          CreatedOn = o.CreatedOn,
            //                          Id = o.Id
            //                      },
            //                      //GLCONFIGConfigID = s1 == null ? "" : s1.ConfigID.ToString()
            //                  };
            //}
            //else
            //{
            if (input.Sorting == "id DESC")
            {
                gltrHeaders = from o in pagedAndFilteredGLTRHeaders
                              join o1 in _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) on o.LocId equals o1.LocId into j1
                              from s1 in j1.DefaultIfEmpty()
                              join bk in _glBooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) on o.BookID equals bk.BookID into book
                              from book1 in book.DefaultIfEmpty()
                              orderby o.Id descending
                              select new GetGLTRHeaderForViewDto()
                              {
                                  GLTRHeader = new GLTRHeaderDto
                                  {
                                      BookID = o.BookID,
                                      ConfigID = o.ConfigID,
                                      //AccountID=o.AccountID,
                                      //AccountDesc=o.AccountName,
                                      DocNo = o.DocNo,
                                      DocMonth = o.DocMonth,
                                      FmtDocNo = o.FmtDocNo,
                                      DocDate = o.DocDate,
                                      NARRATION = o.NARRATION,
                                      Amount = ((_gltrDetailRepository.GetAll().Where(k => k.TenantId == AbpSession.TenantId && k.Amount > 0
                                      && k.DetID == o.Id
                                      ).Count() > 0) ? (Convert.ToDecimal(_gltrDetailRepository.GetAll().Where(k => k.TenantId == AbpSession.TenantId && k.Amount > 0
                                     && k.DetID == o.Id
                                      ).Select(x => x.Amount).Sum())) : 0),
                                      ChType = o.ChType,
                                      ChNumber = o.ChNumber,
                                      Approved = o.Approved,
                                      Posted = o.Posted,
                                      AuditUser = o.AuditUser,
                                      AuditTime = o.AuditTime,
                                      //OldCode = o.OldCode,
                                      LocId = o.LocId,
                                      LocDesc = s1 != null ? s1.LocDesc.ToString() : "",
                                      CreatedBy = o.CreatedBy,
                                      CreatedOn = o.CreatedOn,
                                      Id = o.Id,
                                      IsIntegrated = (book1.NormalEntry == 3 && book1.Integrated == true) ? true : false
                                  },
                                  //GLCONFIGConfigID = s1 == null ? "" : s1.ConfigID.ToString()
                              };
            }
            else
            {
                gltrHeaders = from o in pagedAndFilteredGLTRHeaders
                              join o1 in _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) on o.LocId equals o1.LocId into j1
                              from s1 in j1.DefaultIfEmpty()
                              join bk in _glBooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) on o.BookID equals bk.BookID into book
                              from book1 in book.DefaultIfEmpty()
                              //orderby o.Id descending
                              select new GetGLTRHeaderForViewDto()
                              {
                                  GLTRHeader = new GLTRHeaderDto
                                  {
                                      BookID = o.BookID,
                                      ConfigID = o.ConfigID,
                                      //AccountID=o.AccountID,
                                      //AccountDesc=o.AccountName,
                                      DocNo = o.DocNo,
                                      DocMonth = o.DocMonth,
                                      FmtDocNo = o.FmtDocNo,
                                      DocDate = o.DocDate,
                                      NARRATION = o.NARRATION,
                                      Amount = ((_gltrDetailRepository.GetAll().Where(k => k.TenantId == AbpSession.TenantId && k.Amount > 0
                                      && k.DetID == o.Id
                                      ).Count() > 0) ? (Convert.ToDecimal(_gltrDetailRepository.GetAll().Where(k => k.TenantId == AbpSession.TenantId && k.Amount > 0
                                     && k.DetID == o.Id
                                      ).Select(x => x.Amount).Sum())) : 0),
                                      ChType = o.ChType,
                                      ChNumber = o.ChNumber,
                                      Approved = o.Approved,
                                      Posted = o.Posted,
                                      AuditUser = o.AuditUser,
                                      AuditTime = o.AuditTime,
                                      //OldCode = o.OldCode,
                                      LocId = o.LocId,
                                      LocDesc = s1 != null ? s1.LocDesc.ToString() : "",
                                      CreatedBy = o.CreatedBy,
                                      CreatedOn = o.CreatedOn,
                                      Id = o.Id,
                                      IsIntegrated = (book1.NormalEntry == 3 && book1.Integrated == true) ? true : false
                                  },
                                  //GLCONFIGConfigID = s1 == null ? "" : s1.ConfigID.ToString()
                              };
            }

           
            //  }


            var totalCount = await filteredGLTRHeaders.CountAsync();

            return new PagedResultDto<GetGLTRHeaderForViewDto>(
                totalCount,
                await gltrHeaders.ToListAsync()
            );
        }

        public async Task<GetGLTRHeaderForViewDto> GetGLTRHeaderForView(int id)
        {
            var gltrHeader = await _gltrHeaderRepository.GetAsync(id);

            var output = new GetGLTRHeaderForViewDto { GLTRHeader = ObjectMapper.Map<GLTRHeaderDto>(gltrHeader) };

            if (output.GLTRHeader.GLCONFIGId != null)
            {
                var _lookupGLCONFIG = await _lookup_glconfigRepository.FirstOrDefaultAsync(output.GLTRHeader.GLCONFIGId);
                output.GLCONFIGConfigID = _lookupGLCONFIG.ConfigID.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLTRHeaders_Edit)]
        public async Task<GetGLTRHeaderForEditOutput> GetGLTRHeaderForEdit(EntityDto input)
        {
            var gltrHeader = await _gltrHeaderRepository.FirstOrDefaultAsync(input.Id);
            var book = _glBooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == gltrHeader.BookID).FirstOrDefault();
            var output = new GetGLTRHeaderForEditOutput { GLTRHeader = ObjectMapper.Map<CreateOrEditGLTRHeaderDto>(gltrHeader) };
            output.GLTRHeader.IsIntegrated = (book.NormalEntry == 3 && book.Integrated == true) ? true : false;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGLTRHeaderDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_GLTRHeaders_Create)]
        protected virtual async Task Create(CreateOrEditGLTRHeaderDto input)
        {
            var gltrHeader = ObjectMapper.Map<GLTRHeader>(input);


            if (AbpSession.TenantId != null)
            {
                gltrHeader.TenantId = (int)AbpSession.TenantId;
            }


            await _gltrHeaderRepository.InsertAsync(gltrHeader);
        }

        [AbpAuthorize(AppPermissions.Pages_GLTRHeaders_Edit)]
        protected virtual async Task Update(CreateOrEditGLTRHeaderDto input)
        {
            var gltrHeader = await _gltrHeaderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, gltrHeader);
        }

        [AbpAuthorize(AppPermissions.Pages_GLTRHeaders_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _gltrHeaderRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetGLTRHeadersToExcel(GetAllGLTRHeadersForExcelInput input)
        {

            var filteredGLTRHeaders = _gltrHeaderRepository.GetAll()
                        // .Include(e => e.GLCONFIGFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.NARRATION.Contains(input.Filter) || e.AuditUser.Contains(input.Filter) || e.OldCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                        .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NARRATIONFilter), e => e.NARRATION.ToLower() == input.NARRATIONFilter.ToLower().Trim())
                        .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                        .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
            //.WhereIf(!string.IsNullOrWhiteSpace(input.GLCONFIGConfigIDFilter), e => e.GLCONFIGFk != null && e.GLCONFIGFk.ConfigID.ToLower() == input.GLCONFIGConfigIDFilter.ToLower().Trim());

            var query = (from o in filteredGLTRHeaders
                             // join o1 in _lookup_glconfigRepository.GetAll() on o.ConfigID equals o1.ConfigID into j1
                             // from s1 in j1.DefaultIfEmpty()

                         select new GetGLTRHeaderForViewDto()
                         {
                             GLTRHeader = new GLTRHeaderDto
                             {
                                 BookID = o.BookID,
                                 ConfigID = o.ConfigID,
                                 DocNo = o.DocNo,
                                 DocMonth = o.DocMonth,
                                 DocDate = o.DocDate,
                                 NARRATION = o.NARRATION,
                                 Amount = o.Amount.Value,
                                 ChType = o.ChType,
                                 ChNumber = o.ChNumber,
                                 Approved = o.Approved,
                                 Posted = o.Posted,
                                 AuditUser = o.AuditUser,
                                 AuditTime = o.AuditTime,
                                 OldCode = o.OldCode,
                                 LocId = o.LocId,
                                 CreatedBy = o.CreatedBy,
                                 CreatedOn = o.CreatedOn,
                                 Id = o.Id
                             },
                             //GLCONFIGConfigID = s1 == null ? "" : s1.ConfigID.ToString()
                         });


            var gltrHeaderListDtos = await query.ToListAsync();

            return _gltrHeadersExcelExporter.ExportToFile(gltrHeaderListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_GLTRHeaders)]
        public async Task<PagedResultDto<GLTRHeaderGLCONFIGLookupTableDto>> GetAllGLCONFIGForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_glconfigRepository.GetAll().Join(_chartofControlRepository.GetAll(), x => x.AccountID, y => y.Id, (x, y) => new { x.ConfigID, x.AccountID, x.TenantId, x.BookID, y.AccountName })
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ConfigID.ToString().Contains(input.Filter.ToLower().Trim()) || e.AccountID.ToLower().Trim().ToString().Contains(input.Filter.ToLower().Trim()) || e.AccountName.ToLower().Trim().ToString().Contains(input.Filter.ToLower().Trim()))
                .Where(o => o.TenantId == AbpSession.TenantId && o.BookID == input.TargetFilter).GroupBy(x => x.AccountID).Select(x => x.First());

            var glconfigList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<GLTRHeaderGLCONFIGLookupTableDto>();
            foreach (var item in glconfigList)
            {
                lookupTableDtoList.Add(new GLTRHeaderGLCONFIGLookupTableDto
                {
                    ConfigId = item.ConfigID.ToString(),
                    AccountId = item.AccountID,
                    AccountDesc = item.AccountName

                });
            }
            var totalCount = glconfigList.Count();

            return new PagedResultDto<GLTRHeaderGLCONFIGLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        [AbpAuthorize(AppPermissions.Pages_GLTRHeaders)]
        public async Task<PagedResultDto<GLTRHeaderGLSubledgerLookupTableDto>> GetAllGLSubledgerForLookupTable(GetAllForLookupTableInput input)
        {
            //var query = _lookup_glsubledgerRepository.GetAll().WhereIf(
            //        !string.IsNullOrWhiteSpace(input.Filter),
            //       e => e.Id.ToString().Contains(input.Filter)
            //    ).Where(e => e.AccountID == input.TargetFilter).Where(o => o.TenantId == AbpSession.TenantId);

            var query = !string.IsNullOrEmpty(input.Filter) ?
                _lookup_glsubledgerRepository.GetAll()
                .Where(o => o.SubAccName.ToLower().ToString().Contains(input.Filter.ToLower()) || o.Id.ToString() == input.Filter)
                .Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == input.TargetFilter)
                : _lookup_glsubledgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == input.TargetFilter);

            var totalCount = await query.CountAsync();

            var glsubledgerList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<GLTRHeaderGLSubledgerLookupTableDto>();
            foreach (var glsubledger in glsubledgerList)
            {
                lookupTableDtoList.Add(new GLTRHeaderGLSubledgerLookupTableDto
                {
                    Id = glsubledger.Id,
                    DisplayName = glsubledger.SubAccName.ToString()
                });
            }

            return new PagedResultDto<GLTRHeaderGLSubledgerLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public double? ClosingBalance(string accountId, DateTime date, int? subAccId)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var balance = 0.0;
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("select  dbo.GetBalance ('" + date + "','" + accountId + "'," + AbpSession.TenantId + ") as Balance", cn))
                {
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            balance = Convert.ToDouble(rdr["Balance"]);
                        }
                    }
                }
            }
            return balance;

            //var header = _gltrHeaderRepository.GetAll();
            //var details = _gltrDetailRepository.GetAll();
            //var query = (from a in header
            //             join
            //             b in details
            //             on new { a.Id, a.TenantId } equals  new { Id = b.DetID, b.TenantId} 
            //             where (b.AccountID == accountId && a.DocDate.Date <= date.Date && a.TenantId == AbpSession.TenantId &&
            //             subAccId != null ? b.SubAccID == subAccId: b.SubAccID == null)
            //             select b.Amount).Sum();
            //return query;
        }
    }
}