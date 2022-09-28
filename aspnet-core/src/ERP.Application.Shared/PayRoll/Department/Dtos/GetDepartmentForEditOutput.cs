using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Department.Dtos
{
    public class GetDepartmentForEditOutput
    {
		public CreateOrEditDepartmentDto Department { get; set; }


    }
}