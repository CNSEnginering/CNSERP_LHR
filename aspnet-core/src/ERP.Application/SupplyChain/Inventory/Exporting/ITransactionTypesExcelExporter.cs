using System.Collections.Generic;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public interface ITransactionTypesExcelExporter
    {
        FileDto ExportToFile(List<TransactionTypeDto> transactionTypes);
    }
}