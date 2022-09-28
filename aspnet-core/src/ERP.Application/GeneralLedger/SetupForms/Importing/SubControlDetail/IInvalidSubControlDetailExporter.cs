using System.Collections.Generic;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SubControlDetail.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SUbControlDetail
{
    public interface IInvalidSubControlDetailExporter
    {
        FileDto ExportToFile(List<ImportSubControlDetailDto> userListDtos);
    }
}
