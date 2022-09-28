using System.Collections.Generic;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;

namespace ERP.Authorization.Users.Importing.SubLedger
{
    public interface IInvalidSubLedgerExporter
    {
        FileDto ExportToFile(List<ImportSubLedgerDto> userListDtos);
    }
}
