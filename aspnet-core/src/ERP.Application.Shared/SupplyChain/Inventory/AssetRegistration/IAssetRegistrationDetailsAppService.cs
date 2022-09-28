using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.AssetRegistration.Dtos;
using ERP.Dto;


namespace ERP.SupplyChain.Inventory.AssetRegistration
{
    public interface IAssetRegistrationDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAssetRegistrationDetailForViewDto>> GetAll(GetAllAssetRegistrationDetailsInput input);

		Task<GetAssetRegistrationDetailForEditOutput> GetAssetRegistrationDetailForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAssetRegistrationDetailDto input);

		Task Delete(int AssetId);

		
    }
}