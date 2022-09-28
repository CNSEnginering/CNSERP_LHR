using Abp.Application.Services;
using ERP.Reports.PayRoll.Dtos;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IAssetRegListingAppService : IApplicationService
    {
        List<AssetRegListingDto> GetData(int? TenantId);
    }
}
