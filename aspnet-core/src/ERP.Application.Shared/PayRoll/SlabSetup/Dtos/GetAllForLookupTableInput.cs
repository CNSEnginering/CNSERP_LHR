using Abp.Application.Services.Dto;

namespace ERP.Payroll.SlabSetup.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}