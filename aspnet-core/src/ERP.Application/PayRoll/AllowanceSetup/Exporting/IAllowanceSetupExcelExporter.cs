using System.Collections.Generic;
using ERP.PayRoll.AllowanceSetup.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.AllowanceSetup.Exporting
{
    public interface IAllowanceSetupExcelExporter
    {
        FileDto ExportToFile(List<GetAllowanceSetupForViewDto> allowanceSetup);
    }
}