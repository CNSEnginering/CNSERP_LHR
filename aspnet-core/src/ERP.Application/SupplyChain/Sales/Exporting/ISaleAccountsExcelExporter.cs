using ERP.Dto;
using ERP.Sales.SaleAccounts.Dtos;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Purchase.Exporting
{
    public interface ISaleAccountsExcelExporter
    {
        FileDto ExportToFile(List<GetOECOLLForViewDto> gatePass);
    }
}
