using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports
{
    public interface IReligionAppService: IApplicationService
    {
        List<ReligionListingDto> GetData(int? TenantId, string fromCode, string toCode, string description);
    }
}
