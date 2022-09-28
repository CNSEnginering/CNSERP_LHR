using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.OERoutes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}