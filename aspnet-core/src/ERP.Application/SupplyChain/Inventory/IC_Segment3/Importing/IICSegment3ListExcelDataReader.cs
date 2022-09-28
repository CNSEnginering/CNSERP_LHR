using System.Collections.Generic;
using Abp.Dependency;
using ERP.SupplyChain.Inventory.IC_Segment3.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Importing
{
    public interface IICSegment3ListExcelDataReader : ITransientDependency
    {
        List<ImportICSegment3Dto> GetICSegment3FromExcel(byte[] fileBytes);
    }
}
