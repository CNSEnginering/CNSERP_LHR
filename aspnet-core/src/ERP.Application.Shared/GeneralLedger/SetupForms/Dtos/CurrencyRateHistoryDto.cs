using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
   public class CurrencyRateHistoryDto : EntityDto
    {
        public DateTime RateDate { get; set; }
        public string CurID { get; set; }
        public string CurName { get; set; }
        public double CurRate { get; set; }
        public DateTime AudtDate { get; set; }
        public string AudtUser { get; set; }
        public string Symbol { get; set; }
        public int TenantId { get; set; }
        public int Decimal { get; set; }


    }
}
