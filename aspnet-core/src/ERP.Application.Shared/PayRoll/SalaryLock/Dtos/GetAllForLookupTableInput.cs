using Abp.Application.Services.Dto;

namespace ERP.PayRoll.SalaryLock.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}