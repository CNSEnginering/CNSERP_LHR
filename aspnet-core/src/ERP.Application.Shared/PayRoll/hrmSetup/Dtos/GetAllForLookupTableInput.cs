using Abp.Application.Services.Dto;

namespace ERP.PayRoll.hrmSetup.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}