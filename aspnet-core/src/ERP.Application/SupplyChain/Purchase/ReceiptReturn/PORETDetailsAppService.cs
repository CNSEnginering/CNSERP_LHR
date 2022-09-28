

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.ReceiptReturn.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.CommonServices;

namespace ERP.SupplyChain.Purchase.ReceiptReturn
{
	[AbpAuthorize(AppPermissions.Purchase_PORETDetails)]
    public class PORETDetailsAppService : ERPAppServiceBase, IPORETDetailsAppService
    {
		private readonly IRepository<PORETDetail> _poretDetailRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;

        public PORETDetailsAppService(IRepository<PORETDetail> poretDetailRepository, IRepository<ICItem> ICItemRepository, IRepository<TaxClass> taxClassRepository) 
		  {
			_poretDetailRepository = poretDetailRepository;
            _ICItemRepository = ICItemRepository;
            _taxClassRepository = taxClassRepository;
          }

		public async Task<PagedResultDto<PORETDetailDto>> GetPORETDData(int detId)
        {
			
			var filteredPORETDetails = _poretDetailRepository.GetAll().Where(e => e.DocNo == detId && e.TenantId == AbpSession.TenantId);
            var poretDetails = from o in filteredPORETDetails
                            select new PORETDetailDto
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
                                TaxAuth = o.TaxAuth,
                                TaxClass = o.TaxClass,
                                TaxClassDesc = _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.CLASSID == o.TaxClass).Count() > 0 ? _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.TaxClass).SingleOrDefault().CLASSDESC : "",
                                TaxRate = o.TaxRate,
                                TaxAmt = o.TaxAmt,
                                Remarks = o.Remarks,
                                NetAmount = o.NetAmount,
                                Id = o.Id
						    };

            var totalCount = await filteredPORETDetails.CountAsync();

            return new PagedResultDto<PORETDetailDto>(
                totalCount,
                await poretDetails.ToListAsync()
            );
         }

        [AbpAuthorize(AppPermissions.Purchase_PORETDetails_Edit)]
		 public async Task<PORETDetailDto> GetPORETDetailForEdit(EntityDto input)
         {
            var poretDetail = await _poretDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<PORETDetailDto>(poretDetail);
			
            return output;
         }
    }
}