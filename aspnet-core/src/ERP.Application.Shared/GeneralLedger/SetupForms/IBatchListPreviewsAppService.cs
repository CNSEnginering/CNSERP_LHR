using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IBatchListPreviewsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBatchListPreviewForViewDto>> GetAll(GetAllBatchListPreviewsInput input);

        Task<ICollection<GetBatchListPreviewForViewDto>> GetBatchListPreviewForView(int id, string bookpID, DateTime docDate);


        Task<GetBatchListPreviewForEditOutput> GetBatchListPreviewForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBatchListPreviewDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBatchListPreviewsToExcel(GetAllBatchListPreviewsForExcelInput input);

		
    }
}