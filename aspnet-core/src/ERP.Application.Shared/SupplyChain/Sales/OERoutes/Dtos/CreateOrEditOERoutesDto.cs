using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.OERoutes.Dtos
{
    public class CreateOrEditOERoutesDto : EntityDto<int?>
    {

        [Required]
        public int RoutID { get; set; }

        [StringLength(OERoutesConsts.MaxRoutDescLength, MinimumLength = OERoutesConsts.MinRoutDescLength)]
        public string RoutDesc { get; set; }

        public bool Active { get; set; }

        [StringLength(OERoutesConsts.MaxCreatedByLength, MinimumLength = OERoutesConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(OERoutesConsts.MaxAudtUserLength, MinimumLength = OERoutesConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}