using System.Collections.Generic;
using ERP.CommonServices.RecurringVoucher.Dtos;
using ERP.Dto;

namespace ERP.CommonServices.RecurringVoucher.Exporting
{
    public interface IRecurringVouchersExcelExporter
    {
        FileDto ExportToFile(List<GetRecurringVoucherForViewDto> recurringVouchers);
    }
}