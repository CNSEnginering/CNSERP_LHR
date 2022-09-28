using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.OERoutes.Dtos
{
    public class GetAllOERoutesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxRoutIDFilter { get; set; }
        public int? MinRoutIDFilter { get; set; }

        public string RoutDescFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

    }
}