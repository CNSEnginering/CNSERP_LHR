using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Exporting
{
    public interface IGLSecurityDetailExcelExporter
    {
        FileDto ExportToFile(List<GetGLSecurityDetailForViewDto> GLSecurityDetail);
    }
}