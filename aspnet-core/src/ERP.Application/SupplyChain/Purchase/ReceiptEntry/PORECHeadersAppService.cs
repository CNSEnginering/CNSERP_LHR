

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Purchase.PurchaseOrder;
using ERP.SupplyChain.Purchase.PurchaseOrder.Dtos;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
	[AbpAuthorize(AppPermissions.Purchase_PORECHeaders)]
    public class PORECHeadersAppService : ERPAppServiceBase, IPORECHeadersAppService
    {
		 private readonly IRepository<PORECHeader> _porecHeaderRepository;
        private readonly IRepository<POPOHeader> _popoHeaderRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;

        public PORECHeadersAppService(IRepository<PORECHeader> porecHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<User, long> userRepository, IRepository<POPOHeader> popoHeaderRepository, IRepository<ChartofControl, string> chartofControlRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<CostCenter> costCenterRepository) 
		  {
			_porecHeaderRepository = porecHeaderRepository;
            _popoHeaderRepository = popoHeaderRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _costCenterRepository = costCenterRepository;
            _csUserLocDRepository = csUserLocDRepository;
            _userRepository = userRepository;
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
        public async Task<PagedResultDto<PORECHeaderDto>> GetAll(GetAllPORECHeadersInput input)
         {
            var query = UserManager.Users;
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);

            var filteredPORECHeaders = _porecHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.IGPNo.Contains(input.Filter) || e.BillNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter > 0, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter > 0, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        //.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        //.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter),  e => e.AccountID == input.AccountIDFilter)
                        //.WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        //.WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.IGPNoFilter),  e => e.IGPNo == input.IGPNoFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.BillNoFilter),  e => e.BillNo == input.BillNoFilter)
                        //.WhereIf(input.MinBillDateFilter != null, e => e.BillDate >= input.MinBillDateFilter)
                        //.WhereIf(input.MaxBillDateFilter != null, e => e.BillDate <= input.MaxBillDateFilter)
                        //.WhereIf(input.MinBillAmtFilter != null, e => e.BillAmt >= input.MinBillAmtFilter)
                        //.WhereIf(input.MaxBillAmtFilter != null, e => e.BillAmt <= input.MaxBillAmtFilter)
                        //.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
                        //.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
                        //.WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
                        //.WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Approved) || (input.ActiveFilter == 0 && !e.Approved))
                        .WhereIf(input.PostedFilter > 0, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                        //.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                        //.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        //.WhereIf(input.MinPODocNoFilter != null, e => e.PODocNo >= input.MinPODocNoFilter)
                        //.WhereIf(input.MaxPODocNoFilter != null, e => e.PODocNo <= input.MaxPODocNoFilter)
                        //.WhereIf(input.MinOrdNoFilter != null, e => e.OrdNo >= input.MinOrdNoFilter)
                        //.WhereIf(input.MaxOrdNoFilter != null, e => e.OrdNo <= input.MaxOrdNoFilter)
                        //.WhereIf(input.MinRecDocNoFilter != null, e => e.RecDocNo >= input.MinRecDocNoFilter)
                        //.WhereIf(input.MaxRecDocNoFilter != null, e => e.RecDocNo <= input.MaxRecDocNoFilter)
                        //.WhereIf(input.MinFreightFilter != null, e => e.Freight >= input.MinFreightFilter)
                        //.WhereIf(input.MaxFreightFilter != null, e => e.Freight <= input.MaxFreightFilter)
                        //.WhereIf(input.MinAddExpFilter != null, e => e.AddExp >= input.MinAddExpFilter)
                        //.WhereIf(input.MaxAddExpFilter != null, e => e.AddExp <= input.MaxAddExpFilter)
                        //.WhereIf(input.MinCCIDFilter != null, e => e.CCID >= input.MinCCIDFilter)
                        //.WhereIf(input.MaxCCIDFilter != null, e => e.CCID <= input.MaxCCIDFilter)
                        //.WhereIf(input.MinAddDiscFilter != null, e => e.AddDisc >= input.MinAddDiscFilter)
                        //.WhereIf(input.MaxAddDiscFilter != null, e => e.AddDisc <= input.MaxAddDiscFilter)
                        //.WhereIf(input.MinAddLeakFilter != null, e => e.AddLeak >= input.MinAddLeakFilter)
                        //.WhereIf(input.MaxAddLeakFilter != null, e => e.AddLeak <= input.MaxAddLeakFilter)
                        //.WhereIf(input.MinAddFreightFilter != null, e => e.AddFreight >= input.MinAddFreightFilter)
                        //.WhereIf(input.MaxAddFreightFilter != null, e => e.AddFreight <= input.MaxAddFreightFilter)
                        //.WhereIf(input.onHoldFilter > -1,  e => (input.onHoldFilter == 1 && e.onHold) || (input.onHoldFilter == 0 && !e.onHold) )
                        //.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.ApprovedBy == input.AudtUserFilter)
                        //.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        //.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter);
						//.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						//.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            IQueryable<PORECHeader> pagedAndFilteredPORECHeaders = null;

            var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName == "admin")
            {
                 pagedAndFilteredPORECHeaders = filteredPORECHeaders
                  .OrderBy(input.Sorting ?? "id desc")
                  .PageBy(input);
            }
            else
            {
                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status == true).Select(x => x.LocId).ToList();
                
                    pagedAndFilteredPORECHeaders = filteredPORECHeaders.Where(c => c.TenantId == AbpSession.TenantId && Locd.Contains(c.LocID))
            .OrderBy(input.Sorting ?? "id desc")
            .PageBy(input);
                }
                else
                {
                    pagedAndFilteredPORECHeaders = filteredPORECHeaders.Where(c => c.CreatedBy == UserName)
              .OrderBy(input.Sorting ?? "id desc")
              .PageBy(input);
                }

              
            }

          

			var porecHeaders = from o in pagedAndFilteredPORECHeaders
                         select new PORECHeaderDto
							{
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                AccountID = o.AccountID,
                                SubAccID = o.SubAccID,
                                Narration = o.Narration,
                                IGPNo = o.IGPNo,
                                BillNo = o.BillNo,
                                BillDate = o.BillDate,
                                BillAmt = o.BillAmt,
                                TotalQty = o.TotalQty,
                                TotalAmt = o.TotalAmt,
                                Posted = o.Posted,
                                PostedBy = o.PostedBy,
                                PostedDate = o.PostedDate,
                                Approved=o.Approved,
                                ApprovedBy=o.ApprovedBy,
                                ApprovedDate=o.ApprovedDate,
                                LinkDetID = o.LinkDetID,
                                PODocNo = o.PODocNo,
                                OrdNo = o.OrdNo,
                                RecDocNo = o.RecDocNo,
                                Freight = o.Freight,
                                AddExp = o.AddExp,
                                CCID = o.CCID,
                                AddDisc = o.AddDisc,
                                AddLeak = o.AddLeak,
                                AddFreight = o.AddFreight,
                                onHold = o.onHold,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
						};

            var totalCount = await pagedAndFilteredPORECHeaders.CountAsync();

            return new PagedResultDto<PORECHeaderDto>(
                totalCount,
                await porecHeaders.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Purchase_PORECHeaders_Edit)]
		 public async Task<PORECHeaderDto> GetPORECHeaderForEdit(EntityDto input)
         {
            var porecHeader = await _porecHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
           
		    var output = ObjectMapper.Map<PORECHeaderDto>(porecHeader);

            if (output.AccountID != null)
            {
                output.AccDesc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.AccountID).Count()>0?
                    _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.AccountID).SingleOrDefault().AccountName:"";
            }
            if (output.SubAccID != 0 && output.AccountID != null)
            {
                output.SubAccDesc = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.SubAccID && o.AccountID == output.AccountID).Count()>0?
                    _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.SubAccID && o.AccountID == output.AccountID).SingleOrDefault().SubAccName:"";
            }
            if (output.CCID != null)
            {
                output.CCDesc = _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID.Trim().ToUpper()== output.CCID.Trim().ToUpper()).Count()>0?
                    _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID.Trim().ToUpper() == output.CCID.Trim().ToUpper()).SingleOrDefault().CCName:"";
            }

            return output;
         }


        [AbpAuthorize(AppPermissions.Purchase_PORECHeaders_Edit)]
        public POPOHeaderDto GetPONoHeaderData(int locID, int poNo)
        {
            try
            {
                var popoHeader = _popoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == poNo && o.LocID == locID).Count() > 0 ?
                _popoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == poNo && o.LocID == locID).SingleOrDefault() : null;
                popoHeader.Completed = false;
                POPOHeaderDto output = null;
                if (popoHeader != null)
                {
                    //var porecHeader = await _porecHeaderRepository.FirstOrDefaultAsync((int)poDocNo);

                    output = ObjectMapper.Map<POPOHeaderDto>(popoHeader);

                    if (output.AccountID != null)
                    {
                        output.AccDesc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.AccountID).Count() > 0 ?
                            _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.AccountID).SingleOrDefault().AccountName : "";
                    }
                    if (output.SubAccID != 0 && output.AccountID != null)
                    {
                        output.SubAccDesc = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.SubAccID && o.AccountID == output.AccountID).Count() > 0 ?
                            _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.SubAccID && o.AccountID == output.AccountID).SingleOrDefault().SubAccName : "";
                    }
                    if (output.CCID != null)
                    {
                        output.CCDesc = _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID.Trim().ToUpper() == output.CCID.Trim().ToUpper()).Count() > 0 ?
                            _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID.Trim().ToUpper() == output.CCID.Trim().ToUpper()).SingleOrDefault().CCName : "";
                    }
                }
                return output;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

    }
}