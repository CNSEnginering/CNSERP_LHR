

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.Consumption;

namespace ERP.SupplyChain.Inventory.Adjustment
{
	[AbpAuthorize(AppPermissions.Inventory_ICADJDetails)]
    public class ICADJDetailsAppService : ERPAppServiceBase, IICADJDetailsAppService
    {
		 private readonly IRepository<ICADJDetail> _icadjDetailRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<ICCNSHeader> _icCNSHeaderRepository;
        private readonly IRepository<ICCNSDetail> _icCNSDetailRepository;


        public ICADJDetailsAppService(IRepository<ICADJDetail> icadjDetailRepository, IRepository<ICItem> ICItemRepository,
            IRepository<ICCNSHeader> icCNSHeaderRepository,
            IRepository<ICCNSDetail> icCNSDetailRepository) 
		  {
			_icadjDetailRepository = icadjDetailRepository;
            _ICItemRepository = ICItemRepository;
            _icCNSHeaderRepository = icCNSHeaderRepository;
            _icCNSDetailRepository = icCNSDetailRepository;
          }

        public async Task<PagedResultDto<ICADJDetailDto>> GetICADJDData(int detId)
        {

            var filteredICADJDetails = _icadjDetailRepository.GetAll().Where(e => e.DocNo == detId && e.TenantId == AbpSession.TenantId);

            var icadjDetails = from o in filteredICADJDetails
                               select new ICADJDetailDto
                               {
                                   DetID = o.DetID,
                                   DocNo = o.DocNo,
                                   ItemID = o.ItemID,
                                   ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count()>0? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp:"",
                                   Unit = o.Unit,
                                   Conver = o.Conver,
                                   Type = o.Type,
                                   Qty = o.Qty,
                                   Cost = o.Cost,
                                   Amount = o.Amount,
                                   Remarks = o.Remarks,
                                   Id = o.Id
                               };

            var totalCount = await icadjDetails.CountAsync();

            return new PagedResultDto<ICADJDetailDto>(
                totalCount,
                await icadjDetails.ToListAsync()
            );
        }

        public async Task<PagedResultDto<ICADJDetailDto>> GetConsumptionData(int docNo)
        {

            var filteredData = _icCNSDetailRepository.GetAll().Where(e => e.DocNo == docNo);

            var icadjDetails = from o in filteredData
                               select new ICADJDetailDto
                               {
                                   DetID = o.DetID,
                                   DocNo = o.DocNo,
                                   ItemID = o.ItemID,
                                   ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                   Unit = o.Unit,
                                   Conver = o.Conver,
                                   Type = "Both",
                                   Qty = o.Qty,
                                   Cost = o.Cost,
                                   Amount = o.Amount,
                                   Remarks = o.Remarks,
                                 //  Id = o.Id
                               };

            var totalCount = await icadjDetails.CountAsync();

            return new PagedResultDto<ICADJDetailDto>(
                totalCount,
                await icadjDetails.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Inventory_ICADJDetails_Edit)]
		 public async Task<ICADJDetailDto> GetICADJDetailForEdit(EntityDto input)
         {
            var icadjDetail = await _icadjDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<ICADJDetailDto>(icadjDetail);
			
            return output;
         }

    }
}