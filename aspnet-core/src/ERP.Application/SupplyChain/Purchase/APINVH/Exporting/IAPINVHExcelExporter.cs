using System.Collections.Generic;
using ERP.SupplyChain.Purchase.APINVH.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.APINVH.Exporting
{
    public interface IAPINVHExcelExporter
    {
        FileDto ExportToFile(List<GetAPINVHForViewDto> apinvh);
    }
}