
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GLCONFIGDto : EntityDto
    {
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string BookName { get; set; }
        public int SubAccID { get; set; }
        public int ConfigID { get; set; }
        public string BookID { get; set; }
        public bool PostingOn { get; set; }
        public DateTime? AUDTDATE { get; set; }
        public string AUDTUSER { get; set; }
        public int GLBOOKSId { get; set; }
        public string ChartofControlId { get; set; }
        public int AccountSubLedgerId { get; set; }
        public string BANKID { get; set; }
        public string BANKNAME { get; set; }


    }
}