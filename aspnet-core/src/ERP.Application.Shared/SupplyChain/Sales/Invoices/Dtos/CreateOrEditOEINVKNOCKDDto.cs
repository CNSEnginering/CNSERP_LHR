using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class CreateOrEditOEINVKNOCKDDto : EntityDto<int?>
    {

        public int? DetID { get; set; }

        public int? DocNo { get; set; }

        [Required]
        public int InvNo { get; set; }

        [StringLength(OEINVKNOCKDConsts.MaxInvDateLength, MinimumLength = OEINVKNOCKDConsts.MinInvDateLength)]
        public string InvDate { get; set; }

        public double? Amount { get; set; }

        public double? AlreadyPaid { get; set; }

        public double? Pending { get; set; }

        public double? Adjust { get; set; }

        [StringLength(OEINVKNOCKDConsts.MaxCreatedByLength, MinimumLength = OEINVKNOCKDConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(OEINVKNOCKDConsts.MaxAudtUserLength, MinimumLength = OEINVKNOCKDConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}