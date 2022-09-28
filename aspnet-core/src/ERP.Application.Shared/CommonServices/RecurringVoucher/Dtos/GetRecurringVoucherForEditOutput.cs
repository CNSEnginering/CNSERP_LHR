using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.RecurringVoucher.Dtos
{
    public class GetRecurringVoucherForEditOutput
    {
		public CreateOrEditRecurringVoucherDto RecurringVoucher { get; set; }


    }
}