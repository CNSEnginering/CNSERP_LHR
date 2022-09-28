using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class GetChequeBookDetailForEditOutput
    {
		public ICollection<CreateOrEditChequeBookDetailDto> ChequeBookDetail { get; set; }


    }
}