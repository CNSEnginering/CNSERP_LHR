using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.AccountPayables.Dtos
{
   public class APTransactionListViewDto
    {
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string BookId { get; set; }
        public string UserId { get; set; }
        public bool DirectPost { get; set; }
        public string Status { get; set; }
    }
}
