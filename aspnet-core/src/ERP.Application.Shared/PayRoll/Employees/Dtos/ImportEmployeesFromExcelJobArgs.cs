using Abp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Employees.Dto
{
  public  class ImportEmployeesFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
