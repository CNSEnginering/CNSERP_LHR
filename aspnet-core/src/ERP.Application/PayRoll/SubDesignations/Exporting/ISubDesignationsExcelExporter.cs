using System.Collections.Generic;
using ERP.PayRoll.SubDesignations.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.SubDesignations.Exporting
{
    public interface ISubDesignationsExcelExporter
    {
        FileDto ExportToFile(List<GetSubDesignationsForViewDto> subDesignations);
    }
}