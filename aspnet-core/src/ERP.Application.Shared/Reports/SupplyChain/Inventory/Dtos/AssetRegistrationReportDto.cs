using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory.Dtos
{
    public class AssetRegistrationReportDto
    {
        public string AssetID { get; set; }
        public string AssetName { get; set; }
        public string RegDate { get; set; }
        public string AssetType { get; set; }
        public string LocID { get; set; }  
        public string Location { get; set; }  
        public string Warranty { get; set; }
        public string ExpiryDate { get; set; }
        public string Insurance { get; set; }
        public string Finance { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchasePrice { get; set; }
        public string SerialNo { get; set; }
        public string ItemID { get; set; }
        public string Item { get; set; }
        public string AssetAccID { get; set; }
        public string AssetAccName { get; set; }
        public string AccDeprID { get; set; }
        public string AccDeprName { get; set; }
        public string DepExpAccID { get; set; }
        public string DepExpAccName { get; set; }
        public string DepRate { get; set; }
        public string Narration { get; set; }
        public string DepStartDate { get; set; }
        public string DepMethod { get; set; }
        public string EffectiveLife { get; set; }
        public string BookValue { get; set; }
        public string LastDepAmount { get; set; }
        public string LastDepDate { get; set; }
        public string AccumolatedDep { get; set; }
    }
}
