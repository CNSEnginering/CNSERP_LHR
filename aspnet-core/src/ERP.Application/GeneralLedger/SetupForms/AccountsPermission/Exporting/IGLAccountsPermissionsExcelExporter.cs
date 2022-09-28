using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.AccountsPermission.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.AccountsPermission.Exporting
{
    public interface IGLAccountsPermissionsExcelExporter
    {
        FileDto ExportToFile(List<GetGLAccountsPermissionForViewDto> glAccountsPermissions);
    }
}