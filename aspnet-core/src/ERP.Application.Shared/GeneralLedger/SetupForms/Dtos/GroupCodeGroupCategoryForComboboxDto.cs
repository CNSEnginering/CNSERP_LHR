using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GroupCategoryForComboboxDto: ComboboxItemDto
    {
		public int Id { get; set; }
		public string DisplayName { get; set; }
    }
}