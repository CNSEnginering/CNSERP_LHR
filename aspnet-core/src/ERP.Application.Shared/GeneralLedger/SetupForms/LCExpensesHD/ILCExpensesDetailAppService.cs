using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD
{
    public interface ILCExpensesDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetLCExpensesDetailForViewDto>> GetAll(GetAllLCExpensesDetailInput input);

        Task<GetLCExpensesDetailForEditOutput> GetLCExpensesDetailForEdit(int ID);

        Task CreateOrEdit(ICollection<CreateOrEditLCExpensesDetailDto> input);

        Task Delete(int input);

        Task<FileDto> GetLCExpensesDetailToExcel(GetAllLCExpensesDetailForExcelInput input);
    }
}