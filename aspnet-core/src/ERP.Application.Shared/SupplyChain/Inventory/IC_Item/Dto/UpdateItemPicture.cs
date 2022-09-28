using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class UpdateItemPicture
    {
            [Required]
            [MaxLength(400)]
            public string FileToken { get; set; }

            public string ItemId { get; set; }

             public int X { get; set; }

            public int Y { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }
        
    }
}
