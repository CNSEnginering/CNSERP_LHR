using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.GeneralLedger.Transaction
{
    [Table("vwMonthlyConsolidated")]
    public class MonthlyConsolidated : Entity , IMustHaveTenant
    {
        [Column("Id")]
        public override int Id { get => base.Id; set => base.Id = value; }
        public virtual bool Posted { get; set; }
        public virtual bool Approved { get; set; }
        public virtual string AccountId { get; set; }
        public virtual string AccountName { get; set; }
        public virtual int LocId { get; set; }
        public virtual DateTime DocDate { get; set; }
        public virtual double Jan { get; set; }
        public virtual double Feb { get; set; }
        public virtual double Mar { get; set; }
        public virtual double Apr { get; set; }
        public virtual double May { get; set; }
        public virtual double Jun { get; set; }
        public virtual double Jul { get; set; }
        public virtual double Aug { get; set; }
        public virtual double Sep { get; set; }
        public virtual double Oct { get; set; }
        public virtual double Nov { get; set; }
        public virtual double Dec { get; set; }
        public int TenantId { get; set; }
    }
}
