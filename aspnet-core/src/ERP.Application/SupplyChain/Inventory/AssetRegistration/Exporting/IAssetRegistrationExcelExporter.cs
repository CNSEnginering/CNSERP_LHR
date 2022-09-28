using System.Collections.Generic;
using ERP.SupplyChain.Inventory.AssetRegistration.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.AssetRegistration.Exporting
{
    public interface IAssetRegistrationExcelExporter
    {
        FileDto ExportToFile(List<GetAssetRegistrationForViewDto> assetRegistration);
    }
}