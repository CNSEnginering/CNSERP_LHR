using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.AssetRegistration.Dtos;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory.AssetRegistration
{
    [AbpAuthorize(AppPermissions.Pages_AssetRegistrationDetails)]
    public class AssetRegistrationDetailsAppService : ERPAppServiceBase, IAssetRegistrationDetailsAppService
    {
		 private readonly IRepository<AssetRegistrationDetail> _assetRegistrationDetailRepository;
       


        public AssetRegistrationDetailsAppService(IRepository<AssetRegistrationDetail> assetRegistrationDetailRepository)
        {
            _assetRegistrationDetailRepository = assetRegistrationDetailRepository;
        }

		 public async Task<PagedResultDto<GetAssetRegistrationDetailForViewDto>> GetAll(GetAllAssetRegistrationDetailsInput input)
         {
			
			var filteredAssetRegistrationDetails = _assetRegistrationDetailRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinAssetIDFilter != null, e => e.AssetID >= input.MinAssetIDFilter)
						.WhereIf(input.MaxAssetIDFilter != null, e => e.AssetID <= input.MaxAssetIDFilter)
						.WhereIf(input.MinDepStartDateFilter != null, e => e.DepStartDate >= input.MinDepStartDateFilter)
						.WhereIf(input.MaxDepStartDateFilter != null, e => e.DepStartDate <= input.MaxDepStartDateFilter)
						.WhereIf(input.MinDepMethodFilter != null, e => e.DepMethod >= input.MinDepMethodFilter)
						.WhereIf(input.MaxDepMethodFilter != null, e => e.DepMethod <= input.MaxDepMethodFilter)
						.WhereIf(input.MinAssetLifeFilter != null, e => e.AssetLife >= input.MinAssetLifeFilter)
						.WhereIf(input.MaxAssetLifeFilter != null, e => e.AssetLife <= input.MaxAssetLifeFilter)
						.WhereIf(input.MinBookValueFilter != null, e => e.BookValue >= input.MinBookValueFilter)
						.WhereIf(input.MaxBookValueFilter != null, e => e.BookValue <= input.MaxBookValueFilter)
						.WhereIf(input.MinLastDepAmountFilter != null, e => e.LastDepAmount >= input.MinLastDepAmountFilter)
						.WhereIf(input.MaxLastDepAmountFilter != null, e => e.LastDepAmount <= input.MaxLastDepAmountFilter)
						.WhereIf(input.MinLastDepDateFilter != null, e => e.LastDepDate >= input.MinLastDepDateFilter)
						.WhereIf(input.MaxLastDepDateFilter != null, e => e.LastDepDate <= input.MaxLastDepDateFilter)
						.WhereIf(input.MinAccumulatedDepAmtFilter != null, e => e.AccumulatedDepAmt >= input.MinAccumulatedDepAmtFilter)
						.WhereIf(input.MaxAccumulatedDepAmtFilter != null, e => e.AccumulatedDepAmt <= input.MaxAccumulatedDepAmtFilter);

			var pagedAndFilteredAssetRegistrationDetails = filteredAssetRegistrationDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var assetRegistrationDetails = from o in pagedAndFilteredAssetRegistrationDetails
                         select new GetAssetRegistrationDetailForViewDto() {
							AssetRegistrationDetail = new AssetRegistrationDetailDto
							{
                                AssetID = o.AssetID,
                                DepStartDate = o.DepStartDate,
                                DepMethod = o.DepMethod,
                                AssetLife = o.AssetLife,
                                BookValue = o.BookValue,
                                LastDepAmount = o.LastDepAmount,
                                LastDepDate = o.LastDepDate,
                                AccumulatedDepAmt = o.AccumulatedDepAmt,
                                Id = o.Id
							}
						};

            var totalCount = await filteredAssetRegistrationDetails.CountAsync();

            return new PagedResultDto<GetAssetRegistrationDetailForViewDto>(
                totalCount,
                await assetRegistrationDetails.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_AssetRegistrationDetails_Edit)]
		 public async Task<GetAssetRegistrationDetailForEditOutput> GetAssetRegistrationDetailForEdit(EntityDto input)
         {
            var assetRegistrationDetail = await _assetRegistrationDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetAssetRegistrationDetailForEditOutput {AssetRegistrationDetail = ObjectMapper.Map<CreateOrEditAssetRegistrationDetailDto>(assetRegistrationDetail)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAssetRegistrationDetailDto input)
         {
            //if(input.Id == null){
				await Create(input);
			//}
			//else{
			//	await Update(input);
			//}
         }

		 [AbpAuthorize(AppPermissions.Pages_AssetRegistrationDetails_Create)]
		 protected virtual async Task Create(CreateOrEditAssetRegistrationDetailDto input)
         {
            var assetRegistrationDetail = ObjectMapper.Map<AssetRegistrationDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				assetRegistrationDetail.TenantId = (int) AbpSession.TenantId;
			}
            await _assetRegistrationDetailRepository.InsertAsync(assetRegistrationDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_AssetRegistrationDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditAssetRegistrationDetailDto input)
         {
            var assetRegistrationDetail = await _assetRegistrationDetailRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, assetRegistrationDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_AssetRegistrationDetails_Delete)]
         public async Task Delete(int input)
         {
            await _assetRegistrationDetailRepository.DeleteAsync(x=>x.AssetID == input);
         } 
    }
}