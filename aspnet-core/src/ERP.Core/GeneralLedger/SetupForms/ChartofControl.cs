using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using ERP.GeneralLedger.SetupForms.GLPLCategory;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("GLAMF")]
    public class ChartofControl : Entity<string>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Column("AccountID")]
        public override string Id { get => base.Id; set => base.Id = value; }

        [Required]
        public virtual string AccountName { get; set; }

        [Required]
        public virtual bool SubLedger { get; set; }

        public virtual int? OptFld { get; set; }

        public virtual short? SLType { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual DateTime? CreationDate { get; set; }

        public virtual string AuditUser { get; set; }

        public virtual DateTime? AuditTime { get; set; }

        public virtual string OldCode { get; set; }


        [Column("Segment1")]
        public virtual string ControlDetailId { get; set; }


        [Column("Segment2")]
        public virtual string SubControlDetailId { get; set; }


        [Column("Segment3")]
        public virtual string Segmentlevel3Id { get; set; }

        public virtual int GroupCode { get; set; }

        public virtual string AccountType { get; set; }

        public virtual int? AccountHeader { get; set; }

        public virtual int? SortOrder { get; set; }
        public virtual string AccountBSType { get; set; }

        public virtual int? AccountBSHeader { get; set; }

        public virtual int? SortBSOrder { get; set; }
        public virtual string AccountCFType { get; set; }

        public virtual int? AccountCFHeader { get; set; }

        public virtual int? SortCFOrder { get; set; }

        public virtual string AccNature { get; set; }
    }
}