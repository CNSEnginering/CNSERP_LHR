using Abp.Dependency;
using ERP.SupplyChain.Inventory.Opening.Importing.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.Opening.Importing
{
    public interface IICOPNHDExcelDataReader : ITransientDependency
    {
        List<ImportICOPNHDto> GetTransactionFromExcel(byte[] fileBytes);

    }
}
