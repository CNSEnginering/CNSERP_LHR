using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.IC_UNIT.Dto
{
    public class GetAllIC_UNITsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string UnitFilter { get; set; }

		public double? ConverFilter { get; set; }

		public short? ActiveFilter { get; set; }
    }
}