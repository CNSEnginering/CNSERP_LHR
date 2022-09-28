using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.Dtos
{
    public class GetAllBanksForExcelInput
    {
		public string Filter { get; set; }

		public string CMPIDFilter { get; set; }

		public string BANKIDFilter { get; set; }

		public string BANKNAMEFilter { get; set; }

		public string ADDR1Filter { get; set; }

		public string ADDR2Filter { get; set; }

		public string ADDR3Filter { get; set; }

		public string ADDR4Filter { get; set; }

		public string CITYFilter { get; set; }

		public string STATEFilter { get; set; }

		public string COUNTRYFilter { get; set; }

		public string POSTALFilter { get; set; }

		public string CONTACTFilter { get; set; }

		public string PHONEFilter { get; set; }

		public string FAXFilter { get; set; }

		public int INACTIVEFilter { get; set; }

        public double? MaxODLIMITFilter { get; set; }
        public double? MinODLIMITFilter { get; set; }

        public DateTime? MaxINACTDATEFilter { get; set; }
		public DateTime? MinINACTDATEFilter { get; set; }

		public string BKACCTNUMBERFilter { get; set; }

		public string IDACCTBANKFilter { get; set; }

		public string IDACCTWOFFFilter { get; set; }

		public string IDACCTCRCARDFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }


		 public string ChartofControlIdFilter { get; set; }

		 
    }
}