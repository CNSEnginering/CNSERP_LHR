using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("CSCURHIST")]
   public class CurrencyRateHistory : Entity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual DateTime RateDate { get; set; }
        public virtual string CurID { get; set; }
        public virtual string CurName { get; set; }
        public virtual double CurRate { get; set; }
        public virtual DateTime AudtDate { get; set; }
        public virtual string AudtUser { get; set; }
        public virtual string Symbol { get; set; }     
        public virtual int Decimal { get; set; }
    }
}
