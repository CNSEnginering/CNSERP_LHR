using System.Collections.Generic;
using Abp.Dependency;
using ERP.AccountPayables.Importing.DirectInvoice.Dto;

namespace ERP.AccountPayables.Importing.DirectInvoice
{
    public interface IDirectInvoicesListExcelDataReader : ITransientDependency
    {
        List<ImportDirectInvoiceDto> GetDirectInvoiceFromExcel(byte[] fileBytes);
    }
}
