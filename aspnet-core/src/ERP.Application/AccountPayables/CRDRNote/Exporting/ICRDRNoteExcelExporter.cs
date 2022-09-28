using ERP.AccountPayables.CRDRNote.Dtos;
using ERP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.AccountPayables.CRDRNote.Exporting
{
    public interface ICRDRNoteExcelExporter
    {
        FileDto ExportToFile(List<GetCRDRNoteForViewDto> CRDRNote);
    }
}
