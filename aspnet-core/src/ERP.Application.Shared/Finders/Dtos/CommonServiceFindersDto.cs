using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Finders.Dtos
{
    public class CommonServiceFindersDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string AccountID { get; set; }
        public string BKAccountID { get; set; }

        public int DocType { get; set; }

        public double AvailableLimit { get; set; }

        public double CurrRate { get; set; }

        public double TaxRate { get; set; }
        public string Address { get; set; }
        public int DetId { get; set; }
        public string Narration { get; set; }
    }
}
