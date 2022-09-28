using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.AssetRegistration;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class AssetRegListingAppService : ERPReportAppServiceBase , IAssetRegListingAppService
    {
        private readonly IRepository<AssetRegistration> _assetRegRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        public AssetRegListingAppService(IRepository<AssetRegistration> assetRegRepository,
            IRepository<ICLocation> icLocationRepository)
        {
            _assetRegRepository = assetRegRepository;
            _icLocationRepository = icLocationRepository;
        }
 

        public List<AssetRegListingDto> GetData(int? TenantId)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var assetsReg = _assetRegRepository.GetAll().Where(o => o.TenantId == TenantId);
            var locations = _icLocationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var data = from a in assetsReg
                       join b in locations on a.LocID equals b.LocID
                       orderby a.ItemID
                       select new AssetRegListingDto()
                       {
                           AssetID = a.FmtAssetID,
                           AssetName = a.AssetName,
                           RegDate= a.RegDate== null? a.RegDate.ToString(): a.RegDate.Value.ToString("dd/MM/yyyy"),
                           AssetType = a.AssetType ==1 ? "Computer Equipment": a.AssetType==2 ? "Office Equipment":a.AssetType==3 ?"Plant/Machine":"" ,
                           LocID= a.LocID.ToString(),
                           Location= b.LocName,
                       };
            return data.ToList();
        }
    }

   
}
