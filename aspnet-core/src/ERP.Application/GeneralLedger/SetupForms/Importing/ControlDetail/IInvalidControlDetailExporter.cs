using System.Collections.Generic;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ControlDetail.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.ControlDetail
{
    public interface IInvalidControlDetailExporter
    {
        FileDto ExportToFile(List<ImportControlDetailDto> userListDtos);
    }
}
