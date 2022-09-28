

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
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.CommonServices;
using ERP.SupplyChain.Sales.SaleQutation;

namespace ERP.SupplyChain.Sales.SaleEntry
{
	[AbpAuthorize(AppPermissions.Sales_OESALEDetails)]
    public class OESALEDetailsAppService : ERPAppServiceBase, IOESALEDetailsAppService
    {
		 private readonly IRepository<OESALEDetail> _oesaleDetailRepository;
         private readonly IRepository<ICItem> _ICItemRepository;
         private readonly IRepository<TaxClass> _taxClassRepository;

        private readonly IRepository<OEQD> _oeqDRepository;
        public OESALEDetailsAppService(IRepository<OESALEDetail> oesaleDetailRepository, IRepository<OEQD> oeqDRepository, IRepository<ICItem> ICItemRepository, IRepository<TaxClass> taxClassRepository) 
		  {
			_oesaleDetailRepository = oesaleDetailRepository;
            _ICItemRepository = ICItemRepository;
            _taxClassRepository = taxClassRepository;
            _oeqDRepository = oeqDRepository;
        }
        public async Task<PagedResultDto<OESALEDetailDto>> GetOEQHForDetailWorkOrder(string id)
        {
            var DocNo = Convert.ToInt32(id);
            var TaxList = await _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var filteredICADJDetails = _oeqDRepository.GetAll().Where(e => e.DocNo == DocNo && e.TenantId == AbpSession.TenantId);

            var icwoDetails = from o in filteredICADJDetails
                              select new OESALEDetailDto
                              {
                                  DetID = 0,
                                  DocNo = 0,
                                  LocID = o.LocID,
                                  ItemID = o.ItemID,
                                  ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                  Unit = o.Unit,
                                  Conver = Convert.ToDouble(o.Conver),
                                  Qty = Convert.ToDouble(o.Qty),
                                  Rate = Convert.ToDouble(o.Rate),
                                  Amount = Convert.ToDouble(o.Amount),
                                  //ExlTaxAmount = Convert.ToDouble(o.ExlTaxAmount),
                                  Disc = 0,
                                  TaxAuth = o.TaxAuth,
                                  TaxClass = o.TaxClass,
                                  TaxClassDesc = TaxList.Where(x => x.TenantId == AbpSession.TenantId && x.CLASSID == o.TaxClass).Count() > 0 ? TaxList.Where(x => x.TenantId == AbpSession.TenantId && x.CLASSID == o.TaxClass).FirstOrDefault().CLASSDESC : "", //_taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.CLASSID == o.TaxClass).Count() > 0 ? _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.TaxClass).SingleOrDefault().CLASSDESC : "",
                                  TaxRate = o.TaxRate,
                                  TaxAmt = o.TaxAmt,
                                  NetAmount = o.NetAmount,
                                  Remarks = o.Remarks,
                                  Id = 0
                              };

            var totalCount = await icwoDetails.CountAsync();

            return new PagedResultDto<OESALEDetailDto>(
                totalCount,
                await icwoDetails.ToListAsync()
            );
        }

        public async Task<PagedResultDto<OESALEDetailDto>> GetOESALEDData(int detId)
        {

            var filteredOESALEDetails = _oesaleDetailRepository.GetAll().Where(e => e.DocNo == detId && e.TenantId == AbpSession.TenantId);

            var oesaleDetails = from o in filteredOESALEDetails
                                select new OESALEDetailDto
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
                                    ExlTaxAmount=o.ExlTaxAmount,
                                    Disc = o.Disc,
                                    TaxAuth = o.TaxAuth,
                                    TaxClass = o.TaxClass,
                                    TaxClassDesc = _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.CLASSID == o.TaxClass).Count() > 0 ? _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.TaxClass).SingleOrDefault().CLASSDESC : "",
                                    TaxRate = o.TaxRate,
                                    TaxAmt = o.TaxAmt,
                                    Remarks = o.Remarks,
                                    NetAmount = o.NetAmount,
                                    Id = o.Id
                               };

            var totalCount = await filteredOESALEDetails.CountAsync();

            return new PagedResultDto<OESALEDetailDto>(
                totalCount,
                await oesaleDetails.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Sales_OESALEDetails_Edit)]
		 public async Task<OESALEDetailDto> GetOESALEDetailForEdit(EntityDto input)
         {
            var oesaleDetail = await _oesaleDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<OESALEDetailDto>(oesaleDetail);
			
            return output;
         }
    }
}