using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.OEDrivers.Dtos
{
    public class GetAllOEDriversInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDriverIDFilter { get; set; }
        public int? MinDriverIDFilter { get; set; }

        public string DriverNameFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string DriverCtrlAccFilter { get; set; }

        public int? MaxDriverSubAccIDFilter { get; set; }
        public int? MinDriverSubAccIDFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

    }
}