using System.Collections.Generic;
using ERP.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing
{
    public interface IInvalidTransactionExporter
    {
        FileDto ExportToFile(List<ImportTransactionDto> userListDtos);
    }
}
