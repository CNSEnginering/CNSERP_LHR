using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllGroupCategoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



		public string GRPCTDESCFilter { get; set; }



    }
}