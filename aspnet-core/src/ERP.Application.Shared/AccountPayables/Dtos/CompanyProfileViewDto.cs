using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.AccountPayables.Dtos
{
  public class CompanyProfileViewDto
    {
        public string CompanyName { get; set; }
        public string CONTPERSON { get; set; }
        public string CONTPHONE { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
