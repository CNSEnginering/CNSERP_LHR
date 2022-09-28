using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IStockReportAppService
    {
        List<ItemStockSegment2> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate);
    }

}
