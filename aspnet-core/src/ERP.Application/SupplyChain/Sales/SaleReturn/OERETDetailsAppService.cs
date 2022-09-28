

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.SaleReturn.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.CommonServices;
using ERP.SupplyChain.Inventory.IC_Item;

namespace ERP.SupplyChain.Sales.SaleReturn
{
	[AbpAuthorize(AppPermissions.Sales_OERETDetails)]
    public class OERETDetailsAppService : ERPAppServiceBase, IOERETDetailsAppService
    {
		 private readonly IRepository<OERETDetail> _oeretDetailRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;

        public OERETDetailsAppService(IRepository<OERETDetail> oeretDetailRepository, IRepository<ICItem> ICItemRepository, IRepository<TaxClass> taxClassRepository) 
		  {
			_oeretDetailRepository = oeretDetailRepository;
            _ICItemRepository = ICItemRepository;
            _taxClassRepository = taxClassRepository;
        }

		 public async Task<PagedResultDto<OERETDetailDto>> GetOERETDData(int detId)
        {
			
			var filteredOERETDetails = _oeretDetailRepository.GetAll().Where(e => e.DocNo == detId && e.TenantId == AbpSession.TenantId);

            var oeretDetails = from o in filteredOERETDetails
                               select new OERETDetailDto
							    {
                                    DetID = o.DetID,
                                    LocID = o.LocID,
                                    DocNo = o.DocNo,
                                    ItemID = o.ItemID,
                                    ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                    Unit = o.Unit,
                                    Conver = o.Conver,
                                    Qty = o.Qty,
                                    Rate = o.Rate,
                                    Amount = o.Amount,
                                    Disc = o.Disc,
                                    TaxAuth = o.TaxAuth,
                                    TaxClass = o.TaxClass,
                                    TaxClassDesc = _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.CLASSID == o.TaxClass).Count() > 0 ? _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.TaxClass).SingleOrDefault().CLASSDESC : "",
                                    TaxRate = o.TaxRate,
                                    TaxAmt = o.TaxAmt,
                                    Cost = o.Cost,
                                    CostAmt = o.CostAmt,
                                    Remarks = o.Remarks,
                                    NetAmount = o.NetAmount,
                                    Id = o.Id
						        };

            var totalCount = await filteredOERETDetails.CountAsync();

            return new PagedResultDto<OERETDetailDto>(
                totalCount,
                await oeretDetails.ToListAsync()
            );
         }

        [AbpAuthorize(AppPermissions.Sales_OERETDetails_Edit)]
		 public async Task<OERETDetailDto> GetOERETDetailForEdit(EntityDto input)
         {
            var oeretDetail = await _oeretDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<OERETDetailDto>(oeretDetail);
			
            return output;
         }
    }
}