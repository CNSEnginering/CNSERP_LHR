using System.Collections.Generic;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Importing
{
    public interface IInvalidICSegment1Exporter
    {
        FileDto ExportToFile(List<ImportICSegment1Dto> userListDtos);
    }
}
