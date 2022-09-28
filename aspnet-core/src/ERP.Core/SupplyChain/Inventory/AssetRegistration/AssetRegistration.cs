using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.AssetRegistration
{
	[Table("AMRegister")]
    public class AssetRegistration : Entity , IMustHaveTenant
    {
        [Column(TypeName = "numeric(18, 0)")]
        public override int Id { get => base.Id; set => base.Id = value; }
        public int TenantId { get; set; }

        public virtual int? AssetID { get; set; }
		
		public virtual string FmtAssetID { get; set; }
		
		public virtual string AssetName { get; set; }
		
		public virtual string ItemID { get; set; }
		
		public virtual int? LocID { get; set; }
		
		public virtual DateTime? RegDate { get; set; }
		
		public virtual DateTime? PurchaseDate { get; set; }
		
		public virtual DateTime? ExpiryDate { get; set; }
		
		public virtual bool Warranty { get; set; }
		
		public virtual short? AssetType { get; set; }
		
		public virtual decimal? DepRate { get; set; }
		
		public virtual short? DepMethod { get; set; }
		
		public virtual string SerialNumber { get; set; }
		
		public virtual decimal? PurchasePrice { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual string AccAsset { get; set; }
		
		public virtual string AccDepr { get; set; }
		
		public virtual string AccExp { get; set; }
		
		public virtual DateTime? DepStartDate { get; set; }
		
		public virtual decimal? AssetLife { get; set; }
		
		public virtual decimal? BookValue { get; set; }
		
		public virtual decimal? LastDepAmount { get; set; }
		
		public virtual DateTime? LastDepDate { get; set; }
		
		public virtual bool Disolved { get; set; }
		
		public virtual short? Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
        
        public virtual decimal? AccumulatedDepAmt { get; set; } 

        public virtual bool? Finance { get; set; } 

        public virtual bool? Insurance { get; set; }
        public virtual int? RefID { get; set; }
    }
}