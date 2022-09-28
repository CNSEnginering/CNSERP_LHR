using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos
{
    public class PLCategoryComboboxItemDto : ComboboxItemDto
    {
       public int Id { get; set; }
       public int? SortOrder { get; set; }
    }
}
