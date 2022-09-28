using System.Collections.Generic;
using Abp.Dependency;
using ERP.SupplyChain.Inventory.IC_Segment2.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Importing
{
    public interface IICSegment2ListExcelDataReader : ITransientDependency
    {
        List<ImportICSegment2Dto> GetICSegment2FromExcel(byte[] fileBytes);
    }
}
