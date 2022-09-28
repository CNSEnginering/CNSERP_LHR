

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.SupplyChain.Sales.OERoutes;
using ERP.SupplyChain.Sales.OEDrivers;

namespace ERP.SupplyChain.Sales.SaleEntry
{
	[AbpAuthorize(AppPermissions.Sales_OESALEHeaders)]
    public class OESALEHeadersAppService : ERPAppServiceBase, IOESALEHeadersAppService
    {
		private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<PriceLists> _priceListRepository;
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<OECOLL> _oecollRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<OERoutes.OERoutes> _oeRoutesRepository;
        private readonly IRepository<OEDrivers.OEDrivers> _oeDriversRepository;

        public OESALEHeadersAppService(
            IRepository<OESALEHeader> oesaleHeaderRepository,
            IRepository<ICLocation> icLocationRepository,
            IRepository<PriceLists> priceListRepository,
            IRepository<TransactionType> transactionTypeRepository,
            IRepository<OECOLL> oecollRepository, IRepository<CSUserLocD> csUserLocDRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<User, long> userRepository,
            IRepository<OERoutes.OERoutes> oeRoutesRepository,
             IRepository<OEDrivers.OEDrivers> oeDriversRepository
            ) 
		  {
			_oesaleHeaderRepository = oesaleHeaderRepository;
            _icLocationRepository = icLocationRepository;
            _priceListRepository = priceListRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _oecollRepository = oecollRepository; _csUserLocDRepository = csUserLocDRepository;
             _userRepository = userRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _oeRoutesRepository = oeRoutesRepository;
            _oeDriversRepository = oeDriversRepository;


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
        public async Task<PagedResultDto<OESALEHeaderDto>> GetAll(GetAllOESALEHeadersInput input)
         {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);


            var filteredOESALEHeaders = _oesaleHeaderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.TypeID.Contains(input.Filter) || e.PriceList.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.OGP.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        //.WhereIf(input.MinPaymentDateFilter != null, e => e.PaymentDate >= input.MinPaymentDateFilter)
                        //.WhereIf(input.MaxPaymentDateFilter != null, e => e.PaymentDate <= input.MaxPaymentDateFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter),  e => e.TypeID == input.TypeIDFilter)
                        //.WhereIf(input.MinCustIDFilter != null, e => e.CustID >= input.MinCustIDFilter)
                        //.WhereIf(input.MaxCustIDFilter != null, e => e.CustID <= input.MaxCustIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.PriceListFilter),  e => e.PriceList == input.PriceListFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.OGPFilter),  e => e.OGP == input.OGPFilter)
                        //.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
                        //.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
                        //.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        //.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        //.WhereIf(input.MinTaxFilter != null, e => e.Tax >= input.MinTaxFilter)
                        //.WhereIf(input.MaxTaxFilter != null, e => e.Tax <= input.MaxTaxFilter)
                        //.WhereIf(input.MinAddTaxFilter != null, e => e.AddTax >= input.MinAddTaxFilter)
                        //.WhereIf(input.MaxAddTaxFilter != null, e => e.AddTax <= input.MaxAddTaxFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.DiscFilter),  e => e.Disc == input.DiscFilter)
                        .WhereIf(input.MinOrdNoFilter != null, e => e.OrdNo >= input.MinOrdNoFilter)
                        .WhereIf(input.MaxOrdNoFilter != null, e => e.OrdNo <= input.MaxOrdNoFilter)
                        //.WhereIf(input.MinTotAmtFilter != null, e => e.TotAmt >= input.MinTotAmtFilter)
                        //.WhereIf(input.MaxTotAmtFilter != null, e => e.TotAmt <= input.MaxTotAmtFilter)
                        .WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted) )
						.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
						.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
						//.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			//var pagedAndFilteredOESALEHeaders = filteredOESALEHeaders
   //             .OrderBy(input.Sorting ?? "id desc")
   //             .PageBy(input);

            IQueryable<OESALEHeader> pagedAndFilteredOESALEHeaders = null;

            var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName == "admin")
            {
                pagedAndFilteredOESALEHeaders = filteredOESALEHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            }
            else
            {
                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status == true).Select(x => x.LocId).ToList();
                   
                    pagedAndFilteredOESALEHeaders = filteredOESALEHeaders.Where(c => c.TenantId == AbpSession.TenantId && Locd.Contains(c.LocID))
            .OrderBy(input.Sorting ?? "id desc")
            .PageBy(input);
                }
                else
                {
                    pagedAndFilteredOESALEHeaders = filteredOESALEHeaders.Where(c => c.CreatedBy == UserName)
             .OrderBy(input.Sorting ?? "id desc")
             .PageBy(input);
                }
              
            }



            var oesaleHeaders = from o in pagedAndFilteredOESALEHeaders
                         select new OESALEHeaderDto
                         {
                            LocID = o.LocID,
                            DocNo = o.DocNo,
                            DocDate = o.DocDate,
                            PaymentDate = o.PaymentDate,
                            TypeID = o.TypeID,
                            CustID = o.CustID,
                            PriceList = o.PriceList,
                            Narration = o.Narration,
                            OGP = o.OGP,
                            TotalQty = o.TotalQty,
                            Amount = o.Amount,
                            Tax = o.Tax,
                            AddTax = o.AddTax,
                            Disc = o.Disc,
                            OrdNo = o.OrdNo,
                            TotAmt = o.TotAmt,
                            Posted = o.Posted,
                            LinkDetID = o.LinkDetID,
                            Active = o.Active,
                            AudtUser = o.AudtUser,
                            AudtDate = o.AudtDate,
                            CreatedBy = o.CreatedBy,
                            CreateDate = o.CreateDate,
                            Approved = o.Approved,
                            Id = o.Id,
                            //DriverName=o.DriverName,
                            //DriverCtrlAcc=o.DriverCtrlAcc,
                            //DriverSubAccID=o.DriverSubAccID,
                            //DriverID=o.DriverID
                         };

            var totalCount = await pagedAndFilteredOESALEHeaders.CountAsync();

            return new PagedResultDto<OESALEHeaderDto>(
                totalCount,
                await oesaleHeaders.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Sales_OESALEHeaders_Edit)]
		 public async Task<OESALEHeaderDto> GetOESALEHeaderForEdit(EntityDto input)
         {
            var oesaleHeader = await _oesaleHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            oesaleHeader.Approved = oesaleHeader.Approved == null ? false : oesaleHeader.Approved;
		    var output = ObjectMapper.Map<OESALEHeaderDto>(oesaleHeader);

            if (output.LocID != 0)
            {
                output.LocDesc = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == output.LocID).Count() > 0 ?
                    _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == output.LocID).SingleOrDefault().LocName : "";
            }
            //adding Route DESC
            if (output.RoutID != null)
            {
                output.RoutDesc = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.RoutID == output.RoutID).Count() > 0 ?
                    _oeRoutesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.RoutID == output.RoutID).SingleOrDefault().RoutDesc : "";
            }
            if (output.DriverID != 0)
            {
               output.DriverName = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DriverID == output.DriverID).Count() > 0 ?
                 _oeDriversRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DriverID == output.DriverID).SingleOrDefault().DriverName : "";
                output.DriverCtrlAcc = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DriverID == output.DriverID).Count() > 0 ?
                 _oeDriversRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DriverID == output.DriverID).SingleOrDefault().DriverCtrlAcc : "";
                output.DriverSubAccID = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DriverID == output.DriverID).Count() > 0 ?
                 _oeDriversRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DriverID == output.DriverID).SingleOrDefault().DriverSubAccID : 0;
                

            }

            if (output.TypeID != null)
            {
                output.TypeDesc = _transactionTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeId == output.TypeID).Count() > 0 ?
                    _transactionTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeId == output.TypeID).SingleOrDefault().Description : "";
            }
            if (output.CustID != null)
            {
                var accountID = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == output.TypeID.ToUpper().Trim()).Count() > 0 ?
                    _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == output.TypeID.ToUpper().Trim()).SingleOrDefault().ChAccountID : "";
                if (accountID != "")
                {
                    output.CustomerName = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.CustID && o.AccountID== accountID).Count() > 0 ?
                    _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.CustID && o.AccountID == accountID).SingleOrDefault().SubAccName : "";
                }
            }

            return output;
         }
    }
}