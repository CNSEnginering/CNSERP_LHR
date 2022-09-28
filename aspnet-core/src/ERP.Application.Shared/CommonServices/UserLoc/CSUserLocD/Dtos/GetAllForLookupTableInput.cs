using Abp.Application.Services.Dto;

namespace ERP.CommonServices.UserLoc.CSUserLocD.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}