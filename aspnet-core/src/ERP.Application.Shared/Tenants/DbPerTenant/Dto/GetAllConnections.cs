using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Tenants.DbPerTenant.Dto
{
    public class GetAllConnections
    {
        public int TenantId { get; set; }
        public virtual string ModuleID { get; set; }
        public virtual string ServerName { get; set; }
        public virtual string DatabaseName { get; set; }
        public virtual string UserID { get; set; }
        public virtual string Password { get; set; }
        public virtual string VConnectionString { get; set; }
    }
}
