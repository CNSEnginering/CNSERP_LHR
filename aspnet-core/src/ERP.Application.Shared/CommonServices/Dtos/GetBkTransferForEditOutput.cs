using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.Dtos
{
    public class GetBkTransferForEditOutput
    {
		public CreateOrEditBkTransferDto BkTransfer { get; set; }
		public string FromBankName { get; set;}
        public string FromBankAddress { get; set; }
        public string FromBankAccount { get; set; }
        public string ToBankName { get; set; }
        public string ToBankAddress { get; set; }
        public string ToBankAccount { get; set; }


    }
}