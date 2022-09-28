using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.OEDrivers.Dtos
{
    public class CreateOrEditOEDriversDto : EntityDto<int?>
    {

        [Required]
        public int DriverID { get; set; }

        //[StringLength(OEDriversConsts.MaxDriverNameLength, MinimumLength = OEDriversConsts.MinDriverNameLength)]
        public string DriverName { get; set; }
        public string AccountDesc { get; set; }
        public string SubAccountDesc { get; set; }

        public bool Active { get; set; }

        //[StringLength(OEDriversConsts.MaxDriverCtrlAccLength, MinimumLength = OEDriversConsts.MinDriverCtrlAccLength)]
        public string DriverCtrlAcc { get; set; }

        public int? DriverSubAccID { get; set; }

        //[StringLength(OEDriversConsts.MaxCreatedByLength, MinimumLength = OEDriversConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        //[StringLength(OEDriversConsts.MaxAudtUserLength, MinimumLength = OEDriversConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}