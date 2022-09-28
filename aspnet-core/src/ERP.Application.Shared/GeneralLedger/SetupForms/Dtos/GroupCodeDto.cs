
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GroupCodeDto : EntityDto
    {
        public int GRPCODE { get; set; }
        public string GRPDESC { get; set; }

		public int GRPCTCODE { get; set; }
        
    }
}