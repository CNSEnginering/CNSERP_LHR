using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class GetChequeBookForEditOutput
    {
		public CreateOrEditChequeBookDto ChequeBook { get; set; }


    }
}