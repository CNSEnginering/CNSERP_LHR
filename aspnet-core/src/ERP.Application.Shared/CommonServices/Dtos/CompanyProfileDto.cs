
using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.Dtos
{
    public class CompanyProfileDto : EntityDto<string>
    {

        public string CompanyName { get; set; }


        public string Address1 { get; set; }


        public string Address2 { get; set; }

        public string LegalName { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }


        public string City { get; set; }


        public string State { get; set; }


        public string ZipCode { get; set; }


        public string Email { get; set; }


        public string SLRegNo { get; set; }

        public string CONTPERSON { get; set; }

        public string DESIGNATION { get; set; }

        public string CONTPHONE { get; set; }

        public string CONTEMAIL { get; set; }

        public string CONTPERSON1 { get; set; }

        public string DESIGNATION1 { get; set; }

        public string CONTPHONE1 { get; set; }

        public string CONTEMAIL1 { get; set; }

        public string url { get; set; }

        public bool DirectPost { get; set; }

        public string ReportPath { get; set; }

        public string ServerUrl { get; set; }

    }
}