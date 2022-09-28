using System.Collections.Generic;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3
{
    public interface IInvalidSegmentLevel3Exporter
    {
        FileDto ExportToFile(List<ImportSegmentLevel3Dto> userListDtos);
    }
}
