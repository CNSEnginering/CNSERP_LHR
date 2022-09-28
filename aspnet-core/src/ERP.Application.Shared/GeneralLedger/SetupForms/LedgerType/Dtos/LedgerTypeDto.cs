
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.LedgerType.Dtos
{
    public class LedgerTypeDto : EntityDto
    {
		public int LedgerID { get; set; }

		public string LedgerDesc { get; set; }

		public bool Active { get; set; }



    }
}