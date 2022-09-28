using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.SubDesignations.Dtos
{
    public class GetSubDesignationsForEditOutput
    {
		public CreateOrEditSubDesignationsDto SubDesignations { get; set; }


    }
}