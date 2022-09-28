using System.Collections.Generic;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices.Exporting
{
    public interface ICompanyProfilesExcelExporter
    {
        FileDto ExportToFile(List<GetCompanyProfileForViewDto> companyProfiles);
    }
}