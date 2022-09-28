using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.CommonServices;
using System;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
    [AbpAuthorize(AppPermissions.Purchase_PORECDetails)]
    public class PORECDetailsAppService : ERPAppServiceBase, IPORECDetailsAppService
    {
		private readonly IRepository<PORECDetail> _porecDetailRepository;
		private readonly IRepository<PORECHeader> _porecHeaderRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;
      
        private readonly IRepository<POSTAT> _PorcDetailForEdit;
        
        
      


        public PORECDetailsAppService(IRepository<PORECDetail> porecDetailRepository, IRepository<PORECHeader> porecHeaderRepository, IRepository<POSTAT> PorcDetailForEdit, IRepository<ICItem> ICItemRepository, IRepository<TaxClass> taxClassRepository) 
		  {
			_porecDetailRepository = porecDetailRepository;
            _ICItemRepository = ICItemRepository;
            _taxClassRepository = taxClassRepository;
            _PorcDetailForEdit = PorcDetailForEdit;
            _porecHeaderRepository = porecHeaderRepository;
        }

		public async Task<PagedResultDto<PORECDetailDto>> GetPORECDData(int detId)
        {
            var PODetail =_porecHeaderRepository.GetAll().Where(x => x.DocNo == detId && x.TenantId == AbpSession.TenantId).FirstOrDefault();

            var filteredPORECDetails = _porecDetailRepository.GetAll().Where(e => e.DocNo == detId && e.TenantId == AbpSession.TenantId);
            var detail = _PorcDetailForEdit.GetAll().Where(x=>x.TenantId==AbpSession.TenantId && x.DocNo== PODetail.PODocNo && x.LocID==PODetail.LocID && (x.QtyP != null ? x.QtyP : 0) > 0);
            var porecDetails = from o in filteredPORECDetails 
                              // join a in detail on o.ItemID equals a.ItemID
                            select new PORECDetailDto
                            {
                                DetID = o.DetID,
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                ItemID = o.ItemID,
                                ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                Unit = o.Unit,
                                Conver = o.Conver,
                                POQty = 0,
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

            var totalCount = await filteredPORECDetails.CountAsync();

            return new PagedResultDto<PORECDetailDto>(
                totalCount,
                await porecDetails.ToListAsync()
            );
         }

        [AbpAuthorize(AppPermissions.Purchase_PORECDetails_Edit)]
		 public async Task<PORECDetailDto> GetPORECDetailForEdit(EntityDto input)
         {
            var porecDetail = await _porecDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<PORECDetailDto>(porecDetail);
			
            return output;
         }

    }
}