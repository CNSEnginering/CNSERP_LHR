using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Finders.Dtos
{
   public class ToolTypeFindersDto
    {
        public string Id { get; set; }
        public string TypeId { get; set; }

        public string DisplayName { get; set; }
        public string UOM{ get; set; }
        public string UnitCost { get; set; }

    }
}
