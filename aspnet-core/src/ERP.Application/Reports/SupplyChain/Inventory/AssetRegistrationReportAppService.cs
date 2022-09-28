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
    public class AssetRegistrationReportAppService : ERPReportAppServiceBase , IAssetRegistrationReportAppService
    {
        private readonly IRepository<AssetRegistration> _assetRegRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<GLSecChartofControl, string> _glSecChartofControlRepository;
        public AssetRegistrationReportAppService(IRepository<AssetRegistration> assetRegRepository,
            IRepository<ICLocation> icLocationRepository,
            IRepository<ICItem> itemRepository,
            IRepository<GLSecChartofControl, string> glSecChartofControlRepository)
        {
            _assetRegRepository = assetRegRepository;
            _icLocationRepository = icLocationRepository;
            _itemRepository = itemRepository;
            _glSecChartofControlRepository = glSecChartofControlRepository;
        }
 

        public List<AssetRegistrationReportDto> GetData(int? TenantId)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var assetsReg = _assetRegRepository.GetAll().Where(o => o.TenantId == TenantId);
            var locations = _icLocationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var items= _itemRepository.GetAll().Where(o => o.TenantId == TenantId);
            var accounts= _glSecChartofControlRepository.GetAll().Where(o => o.TenantId == TenantId);
            var data = from a in assetsReg
                       join b in locations on a.LocID equals b.LocID
                       join c in items on a.ItemID equals c.ItemId
                       join d in accounts on a.AccAsset equals d.Id
                       join e in accounts on a.AccDepr equals e.Id
                       join f in accounts on a.AccExp equals f.Id
                       select new AssetRegistrationReportDto()
                       {
                           AssetID = a.FmtAssetID,
                           AssetName = a.AssetName,
                           RegDate= a.RegDate== null? a.RegDate.ToString(): a.RegDate.Value.ToString("dd/MM/yyyy"),
                           AssetType = a.AssetType ==1 ? "Computer Equipment": a.AssetType==2 ? "Office Equipment":a.AssetType==3 ?"Plant/Machine":"" ,
                           LocID= a.LocID.ToString(),
                           Location= b.LocName,
                           Warranty=a.Warranty==true?"Yes":"No",
                           ExpiryDate= a.ExpiryDate == null ? a.ExpiryDate.ToString() : a.ExpiryDate.Value.ToString("dd/MM/yyyy"),
                           Insurance=a.Insurance == true ? "Yes" : "No",
                           Finance=a.Finance == true ? "Yes" : "No",
                           PurchaseDate = a.PurchaseDate == null ? a.PurchaseDate.ToString() : a.PurchaseDate.Value.ToString("dd/MM/yyyy"),
                           PurchasePrice=a.PurchasePrice.ToString(),
                           SerialNo=a.SerialNumber,
                           ItemID=a.ItemID,
                           Item=c.Descp,
                           AssetAccID=a.AccAsset,
                           AssetAccName=d.AccountName,
                           AccDeprID=a.AccDepr,
                           AccDeprName=e.AccountName,
                           DepExpAccID=a.AccExp,
                           DepExpAccName=f.AccountName,
                           DepRate=a.DepRate.ToString(),
                           Narration=a.Narration,
                           DepStartDate= a.DepStartDate == null ? a.DepStartDate.ToString() : a.DepStartDate.Value.ToString("dd/MM/yyyy"),
                           DepMethod=a.DepMethod==1 ? "Straight Line Methods":a.DepMethod==2 ? "Declining Methods":"",
                           EffectiveLife=a.AssetLife.ToString(),
                           BookValue=a.BookValue.ToString()+" Years",
                           LastDepAmount=a.LastDepAmount.ToString(),
                           LastDepDate = a.LastDepDate == null ? a.LastDepDate.ToString() : a.LastDepDate.Value.ToString("dd/MM/yyyy"),
                           AccumolatedDep = a.AccumulatedDepAmt.ToString(),
                       };
            return data.ToList();
        }
    }

   
}
