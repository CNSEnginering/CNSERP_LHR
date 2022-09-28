using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports
{
    public interface IEducationAppService: IApplicationService
    {
        List<EducationListingDto> GetData(int? TenantId, string fromCode, string toCode, string description);
    }
}
