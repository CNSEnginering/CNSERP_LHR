using Abp.Application.Services.Dto;
using System;

namespace ERP.CommonServices.Dtos
{
    public class GetAllBkTransfersForExcelInput
    {
		public string Filter { get; set; }

		public string CMPIDFilter { get; set; }

		public int? MaxDOCIDFilter { get; set; }
		public int? MinDOCIDFilter { get; set; }

		public DateTime? MaxDOCDATEFilter { get; set; }
		public DateTime? MinDOCDATEFilter { get; set; }

		public DateTime? MaxTRANSFERDATEFilter { get; set; }
		public DateTime? MinTRANSFERDATEFilter { get; set; }

		public string DESCRIPTIONFilter { get; set; }

		public int? MaxFROMBANKIDFilter { get; set; }
		public int? MinFROMBANKIDFilter { get; set; }

		public int? MaxFROMCONFIGIDFilter { get; set; }
		public int? MinFROMCONFIGIDFilter { get; set; }

		public int? MaxTOBANKIDFilter { get; set; }
		public int? MinTOBANKIDFilter { get; set; }

		public int? MaxTOCONFIGIDFilter { get; set; }
		public int? MinTOCONFIGIDFilter { get; set; }

		public double? MaxAVAILLIMITFilter { get; set; }
		public double? MinAVAILLIMITFilter { get; set; }

		public double? MaxTRANSFERAMOUNTFilter { get; set; }
		public double? MinTRANSFERAMOUNTFilter { get; set; }

		public DateTime? MaxAUDTDATEFilter { get; set; }
		public DateTime? MinAUDTDATEFilter { get; set; }

		public string AUDTUSERFilter { get; set; }


		 public string BankBANKNAMEFilter { get; set; }

		 
    }
}