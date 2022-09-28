using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ERP.CommonServices.UserLoc.CSUserLocD.Dtos;

namespace ERP.CommonServices.UserLoc.CSUserLocH.Dtos
{
    public class CreateOrEditCSUserLocHDto : EntityDto<int?>
    {

        public short? TypeID { get; set; }

        [StringLength(CSUserLocHConsts.MaxCreatedByLength, MinimumLength = CSUserLocHConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
        public virtual string UserId { get; set; }

        [StringLength(CSUserLocHConsts.MaxAudtUserLength, MinimumLength = CSUserLocHConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }
        public ICollection<CreateOrEditCSUserLocDDto> UserLocD { get; set; }

    }
}