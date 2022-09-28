
using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.Dtos
{
    public class BankDto : EntityDto
    {
		public string CMPID { get; set; }

		public int DocType { get; set; }

		public string BANKID { get; set; }

		public string BANKNAME { get; set; }

		public string BranchName { get; set; }

		public string ADDR1 { get; set; }

		public string ADDR2 { get; set; }

		public string ADDR3 { get; set; }

		public string ADDR4 { get; set; }

		public string CITY { get; set; }

		public string STATE { get; set; }

		public string COUNTRY { get; set; }

		public string POSTAL { get; set; }

		public string CONTACT { get; set; }

		public string PHONE { get; set; }

		public string FAX { get; set; }

        public double? ODLIMIT { get; set; }

        public bool INACTIVE { get; set; }

		public DateTime? INACTDATE { get; set; }

		public string BKACCTNUMBER { get; set; }

		public string IDACCTBANK { get; set; }

		public string IDACCTWOFF { get; set; }

		public string IDACCTCRCARD { get; set; }

		public DateTime? AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }


		
		 
    }
}