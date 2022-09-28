using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.UserLoc.CSUserLocD.Dtos
{
    public class CreateOrEditCSUserLocDDto : EntityDto<int?>
    {

        public short? TypeID { get; set; }

        [StringLength(CSUserLocDConsts.MaxUserIDLength, MinimumLength = CSUserLocDConsts.MinUserIDLength)]
        public string UserID { get; set; }
        public int? DetId { get; set; }
        public int? LocId { get; set; }
        public string Locdesc { get; set; }
        public bool Status { get; set; }

    }
}