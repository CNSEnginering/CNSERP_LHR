using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD
{
    public interface ILCExpensesHeaderAppService : IApplicationService
    {
        Task<PagedResultDto<GetLCExpensesHeaderForViewDto>> GetAll(GetAllLCExpensesHeaderInput input);

        Task<GetLCExpensesHeaderForEditOutput> GetLCExpensesHeaderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditLCExpensesHeaderDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetLCExpensesHeaderToExcel(GetAllLCExpensesHeaderForExcelInput input);

    }
}
