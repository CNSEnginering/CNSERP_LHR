using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.AccountsPermission.Dtos
{
    public class GetAllGLAccountsPermissionsForExcelInput
    {
		public string Filter { get; set; }

		public string UserIDFilter { get; set; }

		public long? MaxCanSeeFilter { get; set; }
		public long? MinCanSeeFilter { get; set; }

		public string BeginAccFilter { get; set; }

		public string EndAccFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }



    }
}