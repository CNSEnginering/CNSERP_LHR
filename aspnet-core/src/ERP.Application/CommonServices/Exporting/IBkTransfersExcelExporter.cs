using System.Collections.Generic;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices.Exporting
{
    public interface IBkTransfersExcelExporter
    {
        FileDto ExportToFile(List<GetBkTransferForViewDto> bkTransfers);
    }
}