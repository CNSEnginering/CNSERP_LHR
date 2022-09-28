using ERP.Dto;
using ERP.PayRoll.Designation.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Designation.Exporting
{
    public interface IDesignationExcelExporter
    {
        FileDto ExportToFile(List<GetDesignationForViewDto> Designation);
    }
}
