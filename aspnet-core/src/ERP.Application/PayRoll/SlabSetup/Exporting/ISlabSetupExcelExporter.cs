using System.Collections.Generic;
using ERP.Payroll.SlabSetup.Dtos;
using ERP.Dto;

namespace ERP.Payroll.SlabSetup.Exporting
{
    public interface ISlabSetupExcelExporter
    {
        FileDto ExportToFile(List<GetSlabSetupForViewDto> slabSetup);
    }
}