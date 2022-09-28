using System.Collections.Generic;
using ERP.AccountPayables.Importing.DirectInvoice.Dto;
using ERP.Dto;

namespace ERP.AccountPayables.Importing.DirectInvoice
{
    public interface IInvalidDirectInvoiceExporter
    {
        FileDto ExportToFile(List<ImportDirectInvoiceDto> userListDtos);
    }
}
