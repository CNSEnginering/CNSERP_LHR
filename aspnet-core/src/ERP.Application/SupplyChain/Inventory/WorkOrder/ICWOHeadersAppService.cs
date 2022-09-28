

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.WorkOrder.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.SupplyChain.Sales.SaleQutation;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.GeneralLedger.SetupForms;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;

namespace ERP.SupplyChain.Inventory.WorkOrder
{
	[AbpAuthorize(AppPermissions.Inventory_ICWOHeaders)]
    public class ICWOHeadersAppService : ERPAppServiceBase, IICWOHeadersAppService
    {
		 private readonly IRepository<ICWOHeader> _icwoHeaderRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<OEQH> _oeqhRepository;
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IRepository<OECOLL> _oecollRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<OEQD> _oeqDRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;


        public ICWOHeadersAppService(IRepository<ICWOHeader> icwoHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<User, long> userRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<OECOLL> oecollRepository, IRepository<TransactionType> transactionTypeRepository, IRepository<OEQH> oeqhRepository, IRepository<CostCenter> costCenterRepository) 
		  {
			_icwoHeaderRepository = icwoHeaderRepository;
            _costCenterRepository = costCenterRepository;
            _oeqhRepository = oeqhRepository;
            _csUserLocDRepository = csUserLocDRepository;
            _userRepository = userRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _oecollRepository = oecollRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;


        }
        public async Task<OEQHDto> GetOEQHForWorkOrder(string id)
        {
            try
            {
                var DocNo = Convert.ToInt32(id);
                var icwoHeader = await _oeqhRepository.FirstOrDefaultAsync(x => x.DocNo == DocNo);

                var output = ObjectMapper.Map<OEQHDto>(icwoHeader);
                //sale type 

                output.SaleTypeDesc = _transactionTypeRepository.GetAll().Where(x => x.TypeId.ToUpper().Trim() == output.TypeID.ToUpper().Trim()).Count() > 0 ?
                    _transactionTypeRepository.GetAll().Where(x => x.TypeId == output.TypeID).FirstOrDefault().Description : "";

                //Customer 
                var accountID = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == output.TypeID.ToUpper().Trim()).Count() > 0 ?
                  _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == output.TypeID.ToUpper().Trim()).SingleOrDefault().ChAccountID : "";
                if (accountID != "")
                {
                    output.CustomerDesc = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == accountID && x.Id == output.CustID).WhereIf(!string.IsNullOrWhiteSpace(""),
                       e => e.Id.ToString().ToUpper().Contains("") || e.SubAccName.ToString().ToUpper().Contains("")).FirstOrDefault().SubAccName;

                }
                //if (output.CCID != null)
                //{
                //    var ccDesc = _costCenterRepository.GetAll().Where(o => o.CCID == output.CCID && o.TenantId == AbpSession.TenantId).Count() > 0 ? _costCenterRepository.GetAll().Where(o => o.CCID == output.CCID && o.TenantId == AbpSession.TenantId).SingleOrDefault().CCName : "";
                //    output.CCDesc = ccDesc.ToString();
                //}

                return output;
            }
            catch (Exception ex)
            {

                throw;
            }
           
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

        public async Task<PagedResultDto<ICWOHeaderDto>> GetAll(GetAllICWOHeadersInput input)
         {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);

            var filteredICWOHeaders = _icwoHeaderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CCID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.ApprovedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.CCIDFilter),  e => e.CCID == input.CCIDFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
						//.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
						//.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
                        //.WhereIf(input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                        .WhereIf(input.ApprovedFilter > -1,  e => (input.ApprovedFilter == 1 && e.Approved) || (input.ApprovedFilter == 0 && !e.Approved) )
						//.WhereIf(!string.IsNullOrWhiteSpace(input.ApprovedByFilter),  e => e.ApprovedBy == input.ApprovedByFilter)
						//.WhereIf(input.MinApprovedDateFilter != null, e => e.ApprovedDate >= input.MinApprovedDateFilter)
						//.WhereIf(input.MaxApprovedDateFilter != null, e => e.ApprovedDate <= input.MaxApprovedDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter).Where(e =>e.TenantId == AbpSession.TenantId);

			//var pagedAndFilteredICWOHeaders = filteredICWOHeaders
   //             .OrderBy(input.Sorting ?? "id desc")
   //             .PageBy(input);

            IQueryable<ICWOHeader> pagedAndFilteredICWOHeaders = null;

            var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName == "admin")
            {
                pagedAndFilteredICWOHeaders = filteredICWOHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            }
            else
            {
                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status == true).Select(x => x.LocId).ToList();
                    
                    pagedAndFilteredICWOHeaders = filteredICWOHeaders.Where(c => c.TenantId == AbpSession.TenantId && Locd.Contains(c.LocID))
             .OrderBy(input.Sorting ?? "id desc")
             .PageBy(input);
                }
                else
                {
                    pagedAndFilteredICWOHeaders = filteredICWOHeaders.Where(c => c.CreatedBy == UserName)
              .OrderBy(input.Sorting ?? "id desc")
              .PageBy(input);
                }
               
            }



            var icwoHeaders = from o in pagedAndFilteredICWOHeaders
                         select new ICWOHeaderDto
							{
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                CCID = o.CCID,
                                Narration = o.Narration,
                                Refrence = o.Refrence,
                                TotalQty = o.TotalQty,
                                TotalAmt=o.TotalAmt,
                                Active = o.Active,
                                Approved = o.Approved,
                                ApprovedBy = o.ApprovedBy,
                                ApprovedDate = o.ApprovedDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Posted = o.Posted,
                                Id = o.Id
						};

            var totalCount = await pagedAndFilteredICWOHeaders.CountAsync();

            return new PagedResultDto<ICWOHeaderDto>(
                totalCount,
                await icwoHeaders.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ICWOHeaders_Edit)]
		 public async Task<ICWOHeaderDto> GetICWOHeaderForEdit(EntityDto input)
         {
            var icwoHeader = await _icwoHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            //var icwoHeader = await _icwoHeaderRepository.FirstOrDefaultAsync(input.Id);
            var output = ObjectMapper.Map<ICWOHeaderDto>(icwoHeader);

            if (output.CCID != null)
            {
                var ccDesc = _costCenterRepository.GetAll().Where(o => o.CCID == output.CCID && o.TenantId == AbpSession.TenantId).Count()>0? _costCenterRepository.GetAll().Where(o => o.CCID == output.CCID && o.TenantId == AbpSession.TenantId).SingleOrDefault().CCName:"";
                output.CCDesc = ccDesc.ToString();
            }

            return output;
         }
    }
}