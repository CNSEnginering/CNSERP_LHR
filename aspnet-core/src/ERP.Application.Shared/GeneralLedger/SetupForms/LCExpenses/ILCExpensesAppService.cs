using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.LCExpenses.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.GeneralLedger.SetupForms.LCExpenses
{
    public interface ILCExpensesAppService : IApplicationService
    {
        Task<PagedResultDto<GetLCExpensesForViewDto>> GetAll(GetAllLCExpensesInput input);

        Task<GetLCExpensesForViewDto> GetLCExpensesForView(int id);

        Task<GetLCExpensesForEditOutput> GetLCExpensesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLCExpensesDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetLCExpensesToExcel(GetAllLCExpensesForExcelInput input);

    }
}
