using Abp.Application.Services;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports
{
    public interface IItemListingAppService: IApplicationService
    {
        List<ItemListingDto> GetData(string itemtype);
        List<AgeingInvoiceListingDto> GetAgeingInvoice(int? TenantId, string fromDate, string fromAcc, int FromsubAcc, int TosubAcc);
        List<DeleteLogDto> GetDeleteLog(int? TenantId, string FormName);
    }
}
