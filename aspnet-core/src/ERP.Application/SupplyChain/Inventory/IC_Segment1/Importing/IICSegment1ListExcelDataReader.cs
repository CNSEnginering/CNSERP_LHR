using System.Collections.Generic;
using Abp.Dependency;
using ERP.SupplyChain.Inventory.IC_Segment1.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Importing
{
    public interface IICSegment1ListExcelDataReader : ITransientDependency
    {
        List<ImportICSegment1Dto> GetICSegment1FromExcel(byte[] fileBytes);
    }
}
