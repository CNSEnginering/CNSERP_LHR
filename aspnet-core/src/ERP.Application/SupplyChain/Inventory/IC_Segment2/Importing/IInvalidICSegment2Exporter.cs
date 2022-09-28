using System.Collections.Generic;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment2.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Importing
{
    public interface IInvalidICSegment2Exporter
    {
        FileDto ExportToFile(List<ImportICSegment2Dto> userListDtos);
    }
}
