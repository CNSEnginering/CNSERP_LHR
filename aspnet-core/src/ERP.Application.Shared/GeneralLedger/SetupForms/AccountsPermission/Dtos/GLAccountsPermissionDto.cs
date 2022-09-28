
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.AccountsPermission.Dtos
{
    public class GLAccountsPermissionDto : EntityDto
    {
		public string UserID { get; set; }

		public long? CanSee { get; set; }

		public string BeginAcc { get; set; }

		public string EndAcc { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }



    }
}