using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Opening.Dtos;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.CommonServices;

namespace ERP.SupplyChain.Inventory.Opening
{
    [AbpAuthorize(AppPermissions.Inventory_ICOPNDetails)]
    public class ICOPNDetailsAppService : ERPAppServiceBase, IICOPNDetailsAppService
    {
		 private readonly IRepository<ICOPNDetail> _icopnDetailRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        

        public ICOPNDetailsAppService(IRepository<ICOPNDetail> icopnDetailRepository , IRepository<ICItem> ICItemRepository) 
		  {
			_icopnDetailRepository = icopnDetailRepository;
            _ICItemRepository = ICItemRepository;
            
        }

        public async Task<PagedResultDto<ICOPNDetailDto>> GetICOPNDData(int detId)
        {

            var filteredICOPNDetails = _icopnDetailRepository.GetAll().Where(e => e.DocNo == detId && e.TenantId == AbpSession.TenantId);

            var icopnDetails = from o in filteredICOPNDetails
                               select new ICOPNDetailDto
                               {
                                   DetID = o.DetID,
                                   LocID = o.LocID,
                                   DocNo = o.DocNo,
                                   ItemID = o.ItemID,
                                   ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp,
                                   Unit = o.Unit,
                                   Conver = o.Conver,
                                   Qty = o.Qty,
                                   Rate = o.Rate,
                                   Amount = o.Amount,
                                   Remarks = o.Remarks,
                                   Active = o.Active,
                                   CreatedBy = o.CreatedBy,
                                   CreateDate = o.CreateDate,
                                   AudtUser = o.AudtUser,
                                   AudtDate = o.AudtDate,
                                   Id = o.Id
                               };

            var totalCount = await icopnDetails.CountAsync();

            return new PagedResultDto<ICOPNDetailDto>(
                totalCount,
                await icopnDetails.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Inventory_ICOPNDetails_Edit)]
		 public async Task<GetICOPNDetailForEditOutput> GetICOPNDetailForEdit(EntityDto input)
         {
            var icopnDetail = await _icopnDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetICOPNDetailForEditOutput {ICOPNDetail = ObjectMapper.Map<CreateOrEditICOPNDetailDto>(icopnDetail)};
			
            return output;
         }

    }
}