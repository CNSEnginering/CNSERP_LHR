using System.Collections.Generic;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Importing
{
    public interface IInvalidICSegment3Exporter
    {
        FileDto ExportToFile(List<ImportICSegment3Dto> userListDtos);
    }
}
