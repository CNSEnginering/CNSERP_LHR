using ERP.Dto;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Purchase.Exporting
{
    public interface IRequisitionsExcelExporter
    {
        FileDto ExportToFile(List<GetRequisitionForViewDto> gatePass);
    }
}
