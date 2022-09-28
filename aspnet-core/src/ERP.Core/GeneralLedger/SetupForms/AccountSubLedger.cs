using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("GLAMFSL")]
    public class AccountSubLedger : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Column("SubAccID")]
        public override int Id { get => base.Id; set => base.Id = value; }

        [Required]
        public virtual string AccountID { get; set; }

        public virtual string SubAccName { get; set; }

        public virtual string Address1 { get; set; }

        public virtual string Address2 { get; set; }

        public virtual string City { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Contact { get; set; }

        public virtual string PAYTERMS { get; set; }

        public virtual string RegNo { get; set; }

        public virtual string TAXAUTH { get; set; }

        public virtual int? ClassID { get; set; }

        public virtual string STTAXAUTH { get; set; }

        public virtual int? STClassID { get; set; }

        public virtual string OldSL { get; set; }

        public virtual short? LedgerType { get; set; }

        public virtual string Agreement1 { get; set; }

        public virtual string Agreement2 { get; set; }

        public virtual int? PayTerm { get; set; }

        public virtual string OtherCondition { get; set; }

        public virtual string Reference { get; set; }

        public virtual DateTime? AUDTDATE { get; set; }

        public virtual string AUDTUSER { get; set; }

        public virtual int? SLType { get; set; }


        [ForeignKey("AccountID,TenantId")]
        public ChartofControl ChartofControlFk { get; set; }

        public virtual string LegalName { get; set; }

        public virtual int? CountryID { get; set; }

        public virtual int? ProvinceID { get; set; }

        public virtual int? CityID { get; set; }

        public virtual bool? Linked { get; set; }

        public virtual string ParentID { get; set; }
        public virtual string CNICNO { get; set; }

        public virtual bool Active { get; set; }

        public virtual int? ParentSubID { get; set; }

        public virtual bool? Parent { get; set; }

        public virtual decimal? CreditLimit { get; set; }
        public virtual bool? CrLimit { get; set; }

        public int? SLGrpId { get; set; }
        public string ItemPriceID { get; set; }


    }
    public class AccSubLedger : Entity
    {
        public virtual string ParentID { get; set; }
        public virtual string AccountID { get; set; }
        public virtual string SubAccID { get; set; }
        public virtual string SubAccName { get; set; }
        public virtual int? SLType { get; set; }
    }

}