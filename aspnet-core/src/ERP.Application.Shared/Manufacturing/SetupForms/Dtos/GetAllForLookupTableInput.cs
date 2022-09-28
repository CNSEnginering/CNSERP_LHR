using Abp.Application.Services.Dto;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}