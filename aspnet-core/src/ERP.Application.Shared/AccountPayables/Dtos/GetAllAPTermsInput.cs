using Abp.Application.Services.Dto;
using System;

namespace ERP.AccountPayables.Dtos
{
    public class GetAllAPTermsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TERMDESCFilter { get; set; }

		public double? MaxTERMRATEFilter { get; set; }
		public double? MinTERMRATEFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }

		public int INACTIVEFilter { get; set; }

        public Int16 TERMTYPE { get; set; }



    }
}