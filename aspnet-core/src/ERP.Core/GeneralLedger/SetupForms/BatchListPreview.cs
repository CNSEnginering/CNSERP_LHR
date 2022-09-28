using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
	[Table("V_GLTRV")]
    public class BatchListPreview : Entity 
    {

        [Column("DocNo")]
        public override int Id { get => base.Id; set => base.Id = value; }

        public virtual DateTime DocDate { get; set; }

        public virtual int DocMonth { get; set; }

        [Column("NARRATION")]
		public virtual string Description { get; set; }
		
        [Column("BookName")]
		public virtual string BookDesc { get; set; }
		
		public virtual double Debit { get; set; }
		
		public virtual double Credit { get; set; }

        public virtual string BookID { get; set; }


    }
}