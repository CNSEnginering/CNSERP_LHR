using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices
{
    public interface ICompanyProfilesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCompanyProfileForViewDto>> GetAll(GetAllCompanyProfilesInput input);

        Task<GetCompanyProfileForViewDto> GetCompanyProfileForView(string id);

		Task<GetCompanyProfileForEditOutput> GetCompanyProfileForEdit(EntityDto<string> input);

		Task CreateOrEdit(CreateOrEditCompanyProfileDto input);

		Task Delete(EntityDto<string> input);

		Task<FileDto> GetCompanyProfilesToExcel(GetAllCompanyProfilesForExcelInput input);

        List<ReportDataForParams> GetReportDataForParams();



    }
}