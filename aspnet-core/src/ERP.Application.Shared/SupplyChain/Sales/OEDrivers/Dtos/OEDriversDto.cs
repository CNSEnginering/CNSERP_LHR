using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.OEDrivers.Dtos
{
    public class OEDriversDto : EntityDto
    {
        public int DriverID { get; set; }

        public string DriverName { get; set; }

        public bool Active { get; set; }

        public string DriverCtrlAcc { get; set; }

        public int? DriverSubAccID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}