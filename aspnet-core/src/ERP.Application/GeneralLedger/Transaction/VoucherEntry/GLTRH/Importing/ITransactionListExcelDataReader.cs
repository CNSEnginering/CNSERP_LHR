using System.Collections.Generic;
using Abp.Dependency;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing
{
    public interface ITransactionListExcelDataReader : ITransientDependency
    {
        List<ImportTransactionDto> GetTransactionFromExcel(byte[] fileBytes);
    }
}
