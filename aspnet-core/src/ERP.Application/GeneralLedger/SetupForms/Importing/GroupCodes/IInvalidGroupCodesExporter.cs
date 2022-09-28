using System.Collections.Generic;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.GroupCodes.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCodes
{
    public interface IInvalidGroupCodesExporter
    {
        FileDto ExportToFile(List<ImportGroupCodesDto> userListDtos);
    }
}
