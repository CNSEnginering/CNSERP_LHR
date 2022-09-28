
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditGLOptionDto : EntityDto<int?>
    {

		public string DEFAULTCLACC { get; set; }

        public string STOCKCTRLACC { get; set; }
        public string Seg1Name { get; set; }
		
		
		public string Seg2Name { get; set; }
		
		
		public string Seg3Name { get; set; }
		
		
		public bool DirectPost { get; set; }
		public bool AutoSeg3 { get; set; }
		public bool InstrumentNo { get; set; }


        public  string FirstSignature { get; set; }

        public  string SecondSignature { get; set; }

        public  string ThirdSignature { get; set; }

        public  string FourthSignature { get; set; }

        public  string FifthSignature { get; set; }

        public  string SixthSignature { get; set; }
        public  int? DocFrequency { get; set; }

        public DateTime? AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }

        public int? FinancePoint { get; set; }
        //public string? ChartofControlId { get; set; }


    }
}