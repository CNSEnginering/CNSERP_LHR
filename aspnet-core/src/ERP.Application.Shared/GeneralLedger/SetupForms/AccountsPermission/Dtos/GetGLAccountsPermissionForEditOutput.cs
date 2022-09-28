using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.AccountsPermission.Dtos
{
    public class GetGLAccountsPermissionForEditOutput
    {
		public CreateOrEditGLAccountsPermissionDto GLAccountsPermission { get; set; }


    }
}