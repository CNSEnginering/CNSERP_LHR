using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Finders.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string Target { get; set; }

        public string ParamFilter { get; set; }

        public string Param2Filter { get; set; }
        public int Param3Filter { get; set; }
    }
}
