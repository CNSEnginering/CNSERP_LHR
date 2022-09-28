using Abp.Dependency;
using ERP.SupplyChain.Inventory.InventoryGLLinks.Importing.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.InventoryGLLinks.Importing
{
   public interface IInventoryGLLinksExcelDataReader : ITransientDependency
    {
            List<ImportInventoryGLLinksDto> GetInventoryGLLinksFromExcel(byte[] fileBytes);
        
    }
}
