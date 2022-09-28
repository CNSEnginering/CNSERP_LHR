using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.UserLoc.CSUserLocD.Dtos
{
    public class CSUserLocDDto : EntityDto
    {
        public short? TypeID { get; set; }

        public string UserID { get; set; }

        public bool Status { get; set; }

    }
}