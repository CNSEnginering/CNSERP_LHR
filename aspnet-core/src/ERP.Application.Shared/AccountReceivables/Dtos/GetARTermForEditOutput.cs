using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountReceivables.Dtos
{
    public class GetARTermForEditOutput
    {
		public CreateOrEditARTermDto ARTerm { get; set; }


    }
}