using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Cader.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}