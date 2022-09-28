using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Finders.Dtos;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ERP.SupplyChain.Purchase.PurchaseOrder;
using ERP.SupplyChain.Inventory;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Purchase.ReceiptEntry;

namespace ERP.Finders.SupplyChain.Purchase
{
    public class PurchaseFindersAppService : ERPAppServiceBase
    {
        private readonly IRepository<POPOHeader> _popoHeaderRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<PORECHeader> _porecHeaderRepository;

        public PurchaseFindersAppService(
            IRepository<POPOHeader> popoHeaderRepository,
            IRepository<ICLocation> icLocationRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<PORECHeader> porecHeaderRepository
        )
        {
            _popoHeaderRepository = popoHeaderRepository;
            _icLocationRepository = icLocationRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _porecHeaderRepository = porecHeaderRepository;
        }


        public async Task<PagedResultDto<PurchaseFindersDto>> GetPurchaseLookupTable(GetAllForLookupTableInput input)
        {
            if (!string.IsNullOrEmpty(input.ParamFilter))
                input.ParamFilter = input.ParamFilter.Trim();

           // var resultDtos = new List<PurchaseFindersDto>();
            var resultDtos = new LookupDto<PurchaseFindersDto>();
            switch (input.Target)
            {
                case "PONo":
                    resultDtos = await GetAllPONoForLookupTable(input);
                    break;
                case "ReceiptNo":
                    resultDtos = await GetAllReceiptNoForLookupTable(input);
                    break;
            }
            return new PagedResultDto<PurchaseFindersDto>(
                    resultDtos.Count,
                    resultDtos.Items
                );
        }

        private async Task<LookupDto<PurchaseFindersDto>> GetAllReceiptNoForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Active==true && o.LocID.ToString() == input.ParamFilter && o.Approved == true);
            var location = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var subledger = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            IQueryable<PurchaseFindersDto> lookupTableDto;
            if (input.Filter != null)
            {
                lookupTableDto = from o in query
                                 join l in location on o.LocID equals l.LocID
                                 join sl in subledger on new { A = Convert.ToInt32(o.SubAccID), B = o.AccountID } equals new { A = sl.Id, B = sl.AccountID }
                                 where (o.DocNo.ToString().Contains(input.Filter.ToUpper())
                                 || (sl.SubAccName != null ? sl.SubAccName : "").ToUpper().Contains(input.Filter.ToUpper())
                                 || (l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")).ToString().ToUpper().Contains(input.Filter.ToUpper())
                                 )
                                 select new PurchaseFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = sl.SubAccName != null ? sl.SubAccName : "",
                                     Location = l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")
                                 };
            }
            else
            {
                lookupTableDto = from o in query
                                 join l in location on o.LocID equals l.LocID
                                 join sl in subledger on new { A = Convert.ToInt32(o.SubAccID), B = o.AccountID } equals new { A = sl.Id, B = sl.AccountID }
                                 select new PurchaseFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = sl.SubAccName != null ? sl.SubAccName : "",
                                     Location = l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")
                                 };
            }
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PurchaseFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PurchaseFindersDto>> GetAllPONoForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _popoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID.ToString() == input.ParamFilter && o.onHold == false && o.Active == true && o.Approved == true);
            var location = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var subledger = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            IQueryable<PurchaseFindersDto> lookupTableDto;
            if (input.Filter != null)
            {
                lookupTableDto = from o in query
                                 join l in location on o.LocID equals l.LocID
                                 join sl in subledger on new { A = Convert.ToInt32(o.SubAccID), B = o.AccountID } equals new { A = sl.Id, B = sl.AccountID }
                                 where (o.DocNo.ToString().Contains(input.Filter.ToUpper())
                                 || (sl.SubAccName != null ? sl.SubAccName : "").ToUpper().Contains(input.Filter.ToUpper())
                                 || (l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")).ToString().ToUpper().Contains(input.Filter.ToUpper())
                                 )
                                 select new PurchaseFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = sl.SubAccName != null ? sl.SubAccName : "",
                                     Location = l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")
                                 };
            }
            else
            {
                lookupTableDto = from o in query
                                 join l in location on o.LocID equals l.LocID
                                 join sl in subledger on new { A = Convert.ToInt32(o.SubAccID), B = o.AccountID } equals new { A = sl.Id, B = sl.AccountID }
                                 select new PurchaseFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = sl.SubAccName != null ? sl.SubAccName : "",
                                     Location = l.LocID.ToString() + " " + (l.LocName != null ? l.LocName : "")
                                 };
            }
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PurchaseFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

    }
}
