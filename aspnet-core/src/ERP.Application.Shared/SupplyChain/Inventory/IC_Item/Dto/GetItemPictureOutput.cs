using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class GetItemPictureOutput
    {
        public string ItemPicture { get; set; }

        public GetItemPictureOutput(string itemPicture)
        {
            ItemPicture = itemPicture;
        }
    }
}
