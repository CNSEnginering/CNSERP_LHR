using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.LedgerType.Dtos;
using ERP.Dto;


namespace ERP.GeneralLedger.SetupForms.LedgerType
{
    public interface ILedgerTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLedgerTypeForViewDto>> GetAll(GetAllLedgerTypesInput input);

		Task<GetLedgerTypeForEditOutput> GetLedgerTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLedgerTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLedgerTypesToExcel(GetAllLedgerTypesForExcelInput input);

		
    }
}