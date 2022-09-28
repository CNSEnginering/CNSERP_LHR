

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.PurchaseOrder.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms;
using ERP.AccountPayables;
using ERP.CommonServices;
using ERP.SupplyChain.Purchase.Requisition;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;

namespace ERP.SupplyChain.Purchase.PurchaseOrder
{
    [AbpAuthorize(AppPermissions.Purchase_POPOHeaders)]
    public class POPOHeadersAppService : ERPAppServiceBase, IPOPOHeadersAppService
    {
        private readonly IRepository<POPOHeader> _popoHeaderRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<APTerm> _apTermRepository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;
        private readonly IRepository<Requisitions> _requisitionRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;
        private readonly IRepository<POSTAT> _repository;

        private readonly IRepository<User, long> _userRepository;

        public POPOHeadersAppService(IRepository<POPOHeader> popoHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<ChartofControl, string> chartofControlRepository, IRepository<Requisitions> requisitionRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<User, long> userRepository, IRepository<APTerm> apTermRepository, IRepository<TaxAuthority, string> taxAuthorityRepository, IRepository<TaxClass> taxClassRepository,
            IRepository<POSTAT> repository)
        {
            _popoHeaderRepository = popoHeaderRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _apTermRepository = apTermRepository;
            _taxAuthorityRepository = taxAuthorityRepository;
            _taxClassRepository = taxClassRepository;
            _csUserLocDRepository = csUserLocDRepository;
            _userRepository = userRepository;
            _requisitionRepository = requisitionRepository;
            _repository = repository;
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
        public async Task<PagedResultDto<POPOHeaderDto>> GetAll(GetAllPOPOHeadersInput input)
        {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);

            var filteredPOPOHeaders = _popoHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.CCID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.TaxAuth.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter > 0, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter > 0, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinArrivalDateFilter != null, e => e.ArrivalDate >= input.MinArrivalDateFilter)
                        .WhereIf(input.MaxArrivalDateFilter != null, e => e.ArrivalDate <= input.MaxArrivalDateFilter)
                        //.WhereIf(input.MinReqNoFilter != null, e => e.ReqNo >= input.MinReqNoFilter)
                        //.WhereIf(input.MaxReqNoFilter != null, e => e.ReqNo <= input.MaxReqNoFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter),  e => e.AccountID == input.AccountIDFilter)
                        //.WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        //.WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        //.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
                        //.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
                        //.WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
                        //.WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter), e => e.Narration == input.OrdNoFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.CCIDFilter),  e => e.CCID == input.CCIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
                        //.WhereIf(input.MinWHTermIDFilter != null, e => e.WHTermID >= input.MinWHTermIDFilter)
                        //.WhereIf(input.MaxWHTermIDFilter != null, e => e.WHTermID <= input.MaxWHTermIDFilter)
                        //.WhereIf(input.MinWHRateFilter != null, e => e.WHRate >= input.MinWHRateFilter)
                        //.WhereIf(input.MaxWHRateFilter != null, e => e.WHRate <= input.MaxWHRateFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuthFilter),  e => e.TaxAuth == input.TaxAuthFilter)
                        //.WhereIf(input.MinTaxClassFilter != null, e => e.TaxClass >= input.MinTaxClassFilter)
                        //.WhereIf(input.MaxTaxClassFilter != null, e => e.TaxClass <= input.MaxTaxClassFilter)
                        //.WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        //.WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        //.WhereIf(input.MinTaxAmountFilter != null, e => e.TaxAmount >= input.MinTaxAmountFilter)
                        //.WhereIf(input.MaxTaxAmountFilter != null, e => e.TaxAmount <= input.MaxTaxAmountFilter)
                        //.WhereIf(input.onHoldFilter > -1,  e => (input.onHoldFilter == 1 && e.onHold) || (input.onHoldFilter == 0 && !e.onHold) )
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Approved) || (input.ActiveFilter == 0 && !e.Approved))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            IQueryable<POPOHeader> pagedAndFilteredPOPOHeaders = null;

            var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName == "admin")
            {
                pagedAndFilteredPOPOHeaders = filteredPOPOHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            }
            else
            {
                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status == true).Select(x => x.LocId).ToList();
                    pagedAndFilteredPOPOHeaders = filteredPOPOHeaders.Where(c => c.TenantId == AbpSession.TenantId && Locd.Contains(c.LocID))
             .OrderBy(input.Sorting ?? "id desc")
             .PageBy(input);
                }
                else
                {
                    pagedAndFilteredPOPOHeaders = filteredPOPOHeaders.Where(c => c.CreatedBy == UserName)
              .OrderBy(input.Sorting ?? "id desc")
              .PageBy(input);
                }

            }




            var popoHeaders = from o in pagedAndFilteredPOPOHeaders
                              select new POPOHeaderDto()
                              {
                                  Terms = o.Terms,
                                  LocID = o.LocID,
                                  DocNo = o.DocNo,
                                  DocDate = o.DocDate,
                                  ArrivalDate = o.ArrivalDate,
                                  ReqNo = o.ReqNo,
                                  AccountID = o.AccountID,
                                  SubAccID = o.SubAccID,
                                  TotalQty = o.TotalQty,
                                  TotalAmt = o.TotalAmt,
                                  OrdNo = o.OrdNo,
                                  CCID = o.CCID,
                                  Narration = o.Narration,
                                  WHTermID = o.WHTermID,
                                  WHRate = o.WHRate,
                                  TaxAuth = o.TaxAuth,
                                  TaxClass = o.TaxClass,
                                  TaxRate = o.TaxRate,
                                  TaxAmount = o.TaxAmount,
                                  onHold = o.onHold,
                                  Active = o.Active,
                                  AudtUser = o.AudtUser,

                                  AudtDate = o.AudtDate,
                                  CreatedBy = o.CreatedBy,
                                  CreateDate = o.CreateDate,
                                  Id = o.Id,
                                  Approved = o.Approved,
                                  Completed = o.Completed
                                  //ItemPrice=o.ItemPrice
                              };

            var totalCount = await filteredPOPOHeaders.CountAsync();

            return new PagedResultDto<POPOHeaderDto>(
                totalCount,
                await popoHeaders.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Purchase_POPOHeaders_Edit)]
        public async Task<POPOHeaderDto> GetPOPOHeaderForEdit(EntityDto input)
        {
            var popoHeader = await _popoHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);

            var output = ObjectMapper.Map<POPOHeaderDto>(popoHeader);

            if (output.AccountID != null)
            {
                output.AccDesc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.AccountID).FirstOrDefault().AccountName;
            }
            if (output.SubAccID != 0 && output.SubAccID != null && output.AccountID != null)
            {
                output.SubAccDesc = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.SubAccID && o.AccountID == output.AccountID).FirstOrDefault().SubAccName;
            }
            if (output.WHTermID != null)
            {
                output.WHTermDesc = _apTermRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.WHTermID).FirstOrDefault().TERMDESC;
            }
            if (output.TaxAuth != null)
            {
                output.TaxAuthDesc = _taxAuthorityRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.TaxAuth).FirstOrDefault().TAXAUTHDESC;
            }
            if (output.TaxClass != null && output.TaxClass != 0)
            {
                output.TaxClassDesc = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CLASSID == output.TaxClass).FirstOrDefault().CLASSDESC;
            }
            if (output.ReqNo != null && output.TaxClass != 0)
            {
                output.Completed = _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == popoHeader.ReqNo).FirstOrDefault() == null ? false : _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == popoHeader.ReqNo).FirstOrDefault().Completed;
            }

            return output;
        }

    }
}