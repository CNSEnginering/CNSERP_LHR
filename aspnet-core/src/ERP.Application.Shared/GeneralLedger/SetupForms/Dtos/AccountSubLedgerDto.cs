
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class AccountSubLedgerDto : EntityDto
    {
        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public int SubAccID { get; set; }

        public string SubAccName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string CNICNO { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Contact { get; set; }

        public string RegNo { get; set; }

        public virtual string PAYTERMS { get; set; }

        public string TAXAUTH { get; set; }

        public int? ClassID { get; set; }

        public string STTAXAUTH { get; set; }

        public int? STClassID { get; set; }

        public string OldSL { get; set; }

        public short? LedgerType { get; set; }

        public string Agreement1 { get; set; }

        public string Agreement2 { get; set; }

        public int? PayTerm { get; set; }

        public string OtherCondition { get; set; }

        public string Reference { get; set; }

        public DateTime? AUDTDATE { get; set; }

        public string AUDTUSER { get; set; }

        public int? SLType { get; set; }
        public string SLDesc { get; set; }

        public string LegalName { get; set; }

        public int? CountryID { get; set; }

        public int? ProvinceID { get; set; }

        public int? CityID { get; set; }

        public bool Linked { get; set; }

        public string ParentID { get; set; }
        public string ItemPriceID { get; set; }

        public bool Active { get; set; }
        public int? ParentSubID { get; set; }

        public bool? Parent { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool? CrLimit { get; set; }

    }
}