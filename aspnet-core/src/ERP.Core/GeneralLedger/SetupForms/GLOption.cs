using ERP.GeneralLedger.SetupForms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("GLSETUP")]
    public class GLOption : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        public virtual string STOCKCTRLACC { get; set; }
        public virtual string DEFAULTCLACC { get; set; }

        public virtual string Seg1Name { get; set; }

        public virtual string Seg2Name { get; set; }

        public virtual string Seg3Name { get; set; }

        public virtual bool DirectPost { get; set; }

        public virtual bool AutoSeg3 { get; set; }
        public virtual bool? InstrumentNo { get; set; }

        public virtual string FirstSignature { get; set; }

        public virtual string SecondSignature { get; set; }

        public virtual string ThirdSignature { get; set; }

        public virtual string FourthSignature { get; set; }

        public virtual string FifthSignature { get; set; }

        public virtual string SixthSignature { get; set; }
        public virtual int? DocFrequency { get; set; }

        public virtual DateTime? AUDTDATE { get; set; }

        public virtual string AUDTUSER { get; set; }

        public virtual DateTime? LastPostingDate { get; set; }
        public virtual DateTime? LastPostingDateS { get; set; }
        public virtual DateTime? LastPostingDateSR { get; set; }
        public virtual DateTime? LastPostingDateRR { get; set; }
        public virtual DateTime? LastPostingDateTR { get; set; }
        public virtual DateTime? LastPostingDateCS { get; set; }
        public virtual DateTime? LastPostingDateBT { get; set; }
        public virtual DateTime? LastPostingDateAS { get; set; }
        public virtual DateTime? LastPostingDateCN { get; set; }
        public virtual DateTime? LastPostingDateDN { get; set; }

        public virtual int? FinancePoint { get; set; }
        //public virtual string? ChartofControlId { get; set; }

        //      [ForeignKey("ChartofControlId")]
        //public ChartofControl ChartofControlFk { get; set; }

    }
}