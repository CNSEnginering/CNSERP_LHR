using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.UserLoc.CSUserLocD.Dtos
{
    public class GetAllCSUserLocDInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public short? MaxTypeIDFilter { get; set; }
        public short? MinTypeIDFilter { get; set; }

        public string UserIDFilter { get; set; }

        public int? StatusFilter { get; set; }

    }
}