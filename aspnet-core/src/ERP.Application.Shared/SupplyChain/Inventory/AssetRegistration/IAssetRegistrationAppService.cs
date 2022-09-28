using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.AssetRegistration.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.AssetRegistration
{
    public interface IAssetRegistrationAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAssetRegistrationForViewDto>> GetAll(GetAllAssetRegistrationInput input);

        Task<GetAssetRegistrationForViewDto> GetAssetRegistrationForView(int id);

		Task<GetAssetRegistrationForEditOutput> GetAssetRegistrationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAssetRegistrationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAssetRegistrationToExcel(GetAllAssetRegistrationForExcelInput input);

		
    }
}