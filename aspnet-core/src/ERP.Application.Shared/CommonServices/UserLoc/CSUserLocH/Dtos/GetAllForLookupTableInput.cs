using Abp.Application.Services.Dto;

namespace ERP.CommonServices.UserLoc.CSUserLocH.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}