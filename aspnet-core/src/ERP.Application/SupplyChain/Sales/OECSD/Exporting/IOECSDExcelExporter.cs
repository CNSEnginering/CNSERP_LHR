using System.Collections.Generic;
using ERP.SupplyChain.Sales.OECSD.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.OECSD.Exporting
{
    public interface IOECSDExcelExporter
    {
        FileDto ExportToFile(List<GetOECSDForViewDto> oecsd);
    }
}