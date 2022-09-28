using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.OEDrivers.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}