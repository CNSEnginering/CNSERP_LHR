﻿using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Adjustments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}