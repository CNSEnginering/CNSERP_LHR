using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Finders.Dtos;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Sales.SaleEntry;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Sales.SalesReference;

namespace ERP.Finders.SupplyChain.Sales
{
    public class SalesFindersAppService : ERPAppServiceBase
    {
        private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IRepository<OECOLL> _oecollRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<SalesReference> _salesReferenceRepository;

        public SalesFindersAppService(
            IRepository<OESALEHeader> oesaleHeaderRepository,
            IRepository<ICLocation> icLocationRepository,
            IRepository<TransactionType> transactionTypeRepository,
            IRepository<OECOLL> oecollRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<SalesReference> salesReferenceRepository
        )
        {
            _oesaleHeaderRepository = oesaleHeaderRepository;
            _icLocationRepository = icLocationRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _oecollRepository = oecollRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _salesReferenceRepository = salesReferenceRepository;
        }


        public async Task<PagedResultDto<SalesFindersDto>> GetSalesLookupTable(GetAllForLookupTableInput input)
        {
            if (!string.IsNullOrEmpty(input.ParamFilter))
                input.ParamFilter = input.ParamFilter.Trim();

            var resultDtos = new LookupDto<SalesFindersDto>();
            switch (input.Target)
            {
                case "SaleNo":
                    resultDtos = await GetAllSaleNoForLookupTable(input);
                    break;
                case "Reference":
                    resultDtos = await GetAllSaleReferencesForLookupTable(input);
                    break;
            }
            return new PagedResultDto<SalesFindersDto>(
                    resultDtos.Count,
                    resultDtos.Items
                );
        }
        private async Task<LookupDto<SalesFindersDto>> GetAllSaleReferencesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _salesReferenceRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.RefID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.RefName.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new SalesFindersDto
                                 {
                                     Id = o.RefID.ToString(),
                                     DisplayName = o.RefName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<SalesFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<SalesFindersDto>> GetAllSaleNoForLookupTable(GetAllForLookupTableInput input)
        {
            var oesaleHeader = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID.ToString() == input.ParamFilter);
            var location = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var oecoll = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var subLedgers = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Active == true);

            IQueryable<SalesFindersDto> lookupTableDto;
            if (input.Filter != null)
            {
                lookupTableDto = from o in oesaleHeader
                                 join l in location on o.LocID equals l.LocID
                                 join col in oecoll on o.TypeID.ToLower().Trim() equals col.TypeID.ToLower().Trim() into colj
                                 from coll in colj.DefaultIfEmpty()
                                 join sl in subLedgers on new { A = Convert.ToInt32(o.CustID), B = coll.ChAccountID } equals new { A = sl.Id, B = sl.AccountID } into slj
                                 from sll in slj.DefaultIfEmpty()
                                 where (o.DocNo.ToString().Contains(input.Filter.ToUpper())
                                 || (sll.SubAccName != null ? sll.SubAccName : "").ToUpper().Contains(input.Filter.ToUpper())
                                 || (l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")).ToString().ToUpper().Contains(input.Filter.ToUpper())
                                 )
                                 select new SalesFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = sll.SubAccName != null ? sll.SubAccName : "",
                                     Location = l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")
                                 };
            }
            else
            {
                lookupTableDto = from o in oesaleHeader
                                 join l in location on o.LocID equals l.LocID
                                 join col in oecoll on o.TypeID.ToLower().Trim() equals col.TypeID.ToLower().Trim() into colj
                                 from coll in colj.DefaultIfEmpty()
                                 join sl in subLedgers on new { A = Convert.ToInt32(o.CustID), B = coll.ChAccountID } equals new { A = sl.Id, B = sl.AccountID } into slj
                                 from sll in slj.DefaultIfEmpty()
                                 select new SalesFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = sll.SubAccName != null ? sll.SubAccName : "",
                                     Location = l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")
                                 };
            }
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<SalesFindersDto>(
                lookupTableDto.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        
    }
}
