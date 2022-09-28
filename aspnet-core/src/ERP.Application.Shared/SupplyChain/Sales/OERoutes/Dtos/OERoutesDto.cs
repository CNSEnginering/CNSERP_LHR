using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.OERoutes.Dtos
{
    public class OERoutesDto : EntityDto
    {
        public int RoutID { get; set; }

        public string RoutDesc { get; set; }

        public bool Active { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}