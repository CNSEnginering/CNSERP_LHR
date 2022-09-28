using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.GLTransfer.Dtos
{
    public class GetAllGLTransferForExcelInput 
    {
		public string Filter { get; set; }
        public int? MaxDOCIDFilter { get; set; }
        public int? MinDOCIDFilter { get; set; }
        public DateTime? MaxDOCDATEFilter { get; set; }
        public DateTime? MinDOCDATEFilter { get; set; }
        public DateTime? MaxTRANSFERDATEFilter { get; set; }
        public DateTime? MinTRANSFERDATEFilter { get; set; }
        public string DESCRIPTIONFilter { get; set; }
        public int? MaxFROMLOCIDFilter { get; set; }
        public int? MinFROMLOCIDFilter { get; set; }
        public string FromBANKIDFilter { get; set; }
        public int? MaxFROMCONFIGIDFilter { get; set; }
        public int? MinFROMCONFIGIDFilter { get; set; }
        public string FROMBANKACCIDFilter { get; set; }
        public string FROMACCIDFilter { get; set; }
        public int? MaxTOLOCIDFilter { get; set; }
        public int? MinTOLOCIDFilter { get; set; }
        public string ToBANKIDFilter { get; set; }
        public int? MaxTOCONFIGIDFilter { get; set; }
        public int? MinTOCONFIGIDFilter { get; set; }
        public string TOBANKACCIDFilter { get; set; }
        public string TOACCIDFilter { get; set; }
        public double? MaxTRANSFERAMOUNTFilter { get; set; }
        public double? MinTRANSFERAMOUNTFilter { get; set; }
        public int STATUSFilter { get; set; }
        public int? MaxGLLINKIDFROMFilter { get; set; }
        public int? MinGLLINKIDFROMFilter { get; set; }
        public int? MaxGLLINKIDTOFilter { get; set; }
        public int? MinGLLINKIDTOFilter { get; set; }
        public int? MaxGLDOCIDFROMFilter { get; set; }
        public int? MinGLDOCIDFROMFilter { get; set; }
        public int? MaxGLDOCIDTOFilter { get; set; }
        public int? MinGLDOCIDTOFilter { get; set; }

        public string AUDTUSERFilter { get; set; }

        public DateTime? MaxAUDTDATEFilter { get; set; }
        public DateTime? MinAUDTDATEFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreatedOnFilter { get; set; }
        public DateTime? MinCreatedOnFilter { get; set; }

    }
}