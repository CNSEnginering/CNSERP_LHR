using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.ICLOT.Dtos
{
    public class CreateOrEditICLOTDto : EntityDto<int?>
    {

        public int? TenantID { get; set; }

        [StringLength(ICLOTConsts.MaxLotNoLength, MinimumLength = ICLOTConsts.MinLotNoLength)]
        public string LotNo { get; set; }

        public DateTime? ManfDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool Active { get; set; }

        [StringLength(ICLOTConsts.MaxAudtUserLength, MinimumLength = ICLOTConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(ICLOTConsts.MaxCreatedByLength, MinimumLength = ICLOTConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}