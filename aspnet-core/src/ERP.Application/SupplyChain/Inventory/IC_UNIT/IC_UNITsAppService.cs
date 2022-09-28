

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.IC_UNIT.Dto;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory.IC_UNIT
{
	[AbpAuthorize(AppPermissions.Inventory_IC_UNITs)]
    public class IC_UNITsAppService : ERPAppServiceBase, IIC_UNITsAppService
    {
		 private readonly IRepository<IC_UNIT> _iC_UNITRepository;
		 

		  public IC_UNITsAppService(IRepository<IC_UNIT> iC_UNITRepository ) 
		  {
			_iC_UNITRepository = iC_UNITRepository;
			
		  }

		 public async Task<PagedResultDto<GetIC_UNITForViewDto>> GetAll(GetAllIC_UNITsInput input)
         {

            var filteredIC_UNITs = _iC_UNITRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Unit.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.ItemId.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnitFilter), e => e.Unit == input.UnitFilter)
                        .WhereIf(input.ConverFilter != null, e => e.Conver == input.ConverFilter)
                        .WhereIf(input.ActiveFilter != null, e => e.Active == input.ActiveFilter);

			var pagedAndFilteredIC_UNITs = filteredIC_UNITs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var iC_UNITs = from o in pagedAndFilteredIC_UNITs
                         select new GetIC_UNITForViewDto() {
							IC_UNIT = new IC_UNITDto
							{
                                Unit = o.Unit,
                                Conver = o.Conver,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredIC_UNITs.CountAsync();

            return new PagedResultDto<GetIC_UNITForViewDto>(
                totalCount,
                await iC_UNITs.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_IC_UNITs_Edit)]
		 public async Task<GetIC_UNITForEditOutput> GetIC_UNITForEdit(string ItemID)
         {
            var iC_UNIT = await _iC_UNITRepository.GetAllListAsync(x=> x.ItemId == ItemID && x.TenantId == AbpSession.TenantId);
           
		    var output = new GetIC_UNITForEditOutput {IC_UNIT = ObjectMapper.Map<ICollection<CreateOrEditIC_UNITDto>>(iC_UNIT)};
            return output;
         }

		 public async Task CreateOrEdit(ICollection<CreateOrEditIC_UNITDto> input)
         {

            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }
            
         }

		 [AbpAuthorize(AppPermissions.Inventory_IC_UNITs_Create)]
		 protected virtual async Task Create(CreateOrEditIC_UNITDto input)
         {
            var iC_UNIT = ObjectMapper.Map<IC_UNIT>(input);

			
			if (AbpSession.TenantId != null)
			{
				iC_UNIT.TenantId = (int) AbpSession.TenantId;
			}
		

            await _iC_UNITRepository.InsertAsync(iC_UNIT);
         }

		 [AbpAuthorize(AppPermissions.Inventory_IC_UNITs_Edit)]
		 protected virtual async Task Update(CreateOrEditIC_UNITDto input)
         {
            var iC_UNIT = await _iC_UNITRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, iC_UNIT);
         }

		 [AbpAuthorize(AppPermissions.Inventory_IC_UNITs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _iC_UNITRepository.DeleteAsync(input.Id);
         }
        public async Task<ListResultDto<IC_UNITDto>> GetUnitList()
        {

            var filteredIC_UNITs = _iC_UNITRepository.GetAll();
            var pagedAndFilteredIC_UNITs = filteredIC_UNITs
                .OrderBy("id asc");

            var iC_UNITs = from o in pagedAndFilteredIC_UNITs
                           select new IC_UNITDto
                           {
                               ItemId = o.ItemId,
                               Unit = o.Unit,
                               Conver = o.Conver,
                               Active = o.Active,
                               Id = o.Id

                           };

            return new ListResultDto<IC_UNITDto>(
                await iC_UNITs.ToListAsync()
            );
        }
    }
}