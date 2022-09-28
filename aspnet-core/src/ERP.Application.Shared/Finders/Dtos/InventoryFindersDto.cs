using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Finders.Dtos
{
    public class InventoryFindersDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Unit { get; set; }

        public decimal? Conver { get; set; }

        public decimal? Rate { get; set; }
        public decimal? Qty { get; set; }

        public string Option5 { get; set; }
        public string ExpiryDate { get; set; }
        public string ManfDate { get; set; }
    }
}
