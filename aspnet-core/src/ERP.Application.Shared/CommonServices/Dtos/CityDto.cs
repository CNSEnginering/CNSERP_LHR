
using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.Dtos
{
	public class CityDto : EntityDto
	{
		public int CityID { get; set; }

		public string Name { get; set; }

		public int ProvinceID { get; set; }

		public int CountryID { get; set; }

		public string preFix { get; set; }

	}
}