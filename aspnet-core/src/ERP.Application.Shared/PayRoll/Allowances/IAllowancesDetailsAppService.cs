using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Allowances.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.PayRoll.Allowances
{
    public interface IAllowancesDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAllowancesDetailForViewDto>> GetAll(GetAllAllowancesDetailsInput input);

        Task<GetAllowancesDetailForViewDto> GetAllowancesDetailForView(int id);

		Task<GetAllowancesDetailForEditOutput> GetAllowancesDetailForEdit(int ID);

		Task CreateOrEdit(ICollection<CreateOrEditAllowancesDetailDto> input);

		Task Delete(int input);

		Task<FileDto> GetAllowancesDetailsToExcel(GetAllAllowancesDetailsForExcelInput input);

    }
}