
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GroupCategoryDto : EntityDto
    {
        public int GRPCTCODE { get; set; }
        public string GRPCTDESC { get; set; }

    }
}