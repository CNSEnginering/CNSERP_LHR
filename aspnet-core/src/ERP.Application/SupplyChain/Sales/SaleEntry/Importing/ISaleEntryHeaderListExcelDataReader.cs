using Abp.Dependency;
using ERP.SupplyChain.Sales.SaleEntry.Importing.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Sales.SaleEntry.Importing
{
    public interface  ISaleEntryHeaderListExcelDataReader : ITransientDependency
    {
        List<ImportSaleEntryHeaderDto> GetSaleEntryFromExcel(byte[] fileBytes);
    }
}
