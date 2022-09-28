using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.DirectInvoice;
using ERP.GeneralLedger.DirectInvoice.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.CommonServices;

namespace ERP.SupplyChain.Pages.DirectInvoice
{
    [AbpAuthorize(AppPermissions.Pages_GLINVHeaders)]
    public class GLINVHeadersAppService : ERPAppServiceBase, IGLINVHeadersAppService
    {
		 private readonly IRepository<GLINVHeader> _glinvHeaderRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;


        public GLINVHeadersAppService(IRepository<GLINVHeader> glinvHeaderRepository, IRepository<GLLocation> glLocationRepository, IRepository<ChartofControl, string> chartofControlRepository,IRepository<TaxAuthority, string> taxAuthorityRepository, IRepository<TaxClass> taxClassRepository) 
		  {
			_glinvHeaderRepository = glinvHeaderRepository;
            _chartofControlRepository = chartofControlRepository;
            _taxAuthorityRepository = taxAuthorityRepository;
            _taxClassRepository = taxClassRepository;
            _glLocationRepository = glLocationRepository;

        }

		 public async Task<PagedResultDto<GLINVHeaderDto>> GetAll(GetAllGLINVHeadersInput input)
         {

            var filteredGLINVHeaders = _glinvHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeID.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.CprNo.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.RefNo.Contains(input.Filter) || e.PartyInvNo.Contains(input.Filter) || e.PostedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter),  e => e.TypeID == input.TypeIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID == input.BankIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID == input.BankIDFilter)
                        //.WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        //.WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
                        //.WhereIf(input.MinCurRateFilter != null, e => e.CurRate >= input.MinCurRateFilter)
                        //.WhereIf(input.MaxCurRateFilter != null, e => e.CurRate <= input.MaxCurRateFilter)
                        .WhereIf(input.MinClosingBalanceFilter != null, e => e.ClosingBalance >= input.MinClosingBalanceFilter)
                        .WhereIf(input.MaxClosingBalanceFilter != null, e => e.ClosingBalance <= input.MaxClosingBalanceFilter)
                        .WhereIf(input.MinCreditLimitFilter != null, e => e.CreditLimit >= input.MinCreditLimitFilter)
                        .WhereIf(input.MaxCreditLimitFilter != null, e => e.CreditLimit <= input.MaxCreditLimitFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter),  e => e.ChequeNo == input.ChequeNoFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.RefNoFilter),  e => e.RefNo == input.RefNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PartyInvNoFilter),  e => e.PartyInvNo == input.PartyInvNoFilter)
						.WhereIf(input.MinPartyInvDateFilter != null, e => e.PartyInvDate >= input.MinPartyInvDateFilter)
						.WhereIf(input.MaxPartyInvDateFilter != null, e => e.PartyInvDate <= input.MaxPartyInvDateFilter)
						.WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted) )
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter),  e => e.PostedBy == input.PostedByFilter)
                        //.WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
                        //.WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter)
                        .WhereIf(input.MinCprIDFilter != null, e => e.CprID >= input.MinCprIDFilter)
                        .WhereIf(input.MaxCprIDFilter != null, e => e.CprID <= input.MaxCprIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CprNoFilter),  e => e.CprNo == input.CprNoFilter)
                        .WhereIf(input.MinCprDateFilter != null, e => e.CprDate >= input.MinCprDateFilter)
                        .WhereIf(input.MaxCprDateFilter != null, e => e.CprDate <= input.MaxCprDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter).Where(o=>o.TenantId==AbpSession.TenantId && o.TypeID==input.TypeIDFilter);

			var pagedAndFilteredGLINVHeaders = filteredGLINVHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var glinvHeaders = from o in pagedAndFilteredGLINVHeaders
                         select new GLINVHeaderDto
							{
                                DocNo = o.DocNo,
                                TypeID = o.TypeID,
                                PaymentOption=o.PaymentOption,
                                BankID = o.BankID,
                                LocID=o.LocID,
                                AccountID=o.AccountID,
                                ConfigID = o.ConfigID,
                                DocDate = o.DocDate,
                                PostDate = o.PostDate,
                                Narration = o.Narration,
                                CurID=o.CurID,
                                CurRate = o.CurRate,
                                ChequeNo = o.ChequeNo,
                                RefNo = o.RefNo,
                                PayReason=o.PayReason,
                                TaxAuth=o.TaxAuth,
                                TaxClass=o.TaxClass,
                                TaxRate=o.TaxRate,
                                TaxAccID=o.TaxAccID,
                                TaxAmount=o.TaxAmount,
                                ClosingBalance=o.ClosingBalance,
                                CreditLimit=o.CreditLimit,
                                PartyInvNo = o.PartyInvNo,
                                PartyInvDate = o.PartyInvDate,
                                PostedStock = o.PostedStock,
                                PostedStockBy = o.PostedStockBy,
                                PostedStockDate = o.PostedStockDate,
                                CprID=o.CprID,
                                CprNo=o.CprNo,
                                CprDate=o.CprDate,
                                Posted = o.Posted,
                                PostedBy = o.PostedBy,
                                PostedDate = o.PostedDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
						};

            var totalCount = await filteredGLINVHeaders.CountAsync();

            return new PagedResultDto<GLINVHeaderDto>(
                totalCount,
                await glinvHeaders.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_GLINVHeaders_Edit)]
		 public async Task<GLINVHeaderDto> GetGLINVHeaderForEdit(EntityDto input)
         {
            var glinvHeader = await _glinvHeaderRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<GLINVHeaderDto>(glinvHeader);

            if (output.TaxAccID != null)
            {
                output.TaxAccDesc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.TaxAccID).Count() > 0 ? 
                    _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.TaxAccID).
                    FirstOrDefault().AccountName : "";
            }
            if (output.LocID != null)
            {
                output.LocDesc = _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == output.LocID).FirstOrDefault().LocDesc;
            }
            if (output.TaxAuth != null)
            {
                output.TaxAuthDesc = _taxAuthorityRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.TaxAuth).FirstOrDefault().TAXAUTHDESC;
            }
            if (output.TaxClass != null && output.TaxClass != 0)
            {
                output.TaxClassDesc = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == output.TaxClass).FirstOrDefault().CLASSDESC;
            }

            return output;
         }
        
    }
}