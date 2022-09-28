using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAccountSubLedgerForEditOutput
    {
		public CreateOrEditAccountSubLedgerDto AccountSubLedger { get; set; }

		public string ChartofControlAccountName { get; set;}

		public string TaxAuthorityTAXAUTHDESC { get; set;}

        public string ParentAccountName { get; set; }

        public string ParentSubAccountName { get; set; }
        public string ItemPriceLIst { get; set; }

    }
}