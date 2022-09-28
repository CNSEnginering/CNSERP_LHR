using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAllGroupCodesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxGRPCODEFilter { get; set; }
		public int? MinGRPCODEFilter { get; set; }

		public string GRPDESCFilter { get; set; }

        public string GRPCTDESCFilter { get; set; }


    }
}