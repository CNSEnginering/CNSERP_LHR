using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetGLCONFIGForEditOutput
    {
		public CreateOrEditGLCONFIGDto GLCONFIG { get; set; }

		public string GLBOOKSBookName { get; set;}

		public string ChartofControlAccountName { get; set;}

		public string AccountSubLedgerSubAccName { get; set;}

        public string BankName { get; set; }

    }
}