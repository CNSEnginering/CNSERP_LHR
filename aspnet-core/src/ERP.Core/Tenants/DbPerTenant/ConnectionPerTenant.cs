using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Tenants.DbPerTenant
{
    [Table("Abp_Connections")]
    public class ConnectionPerTenant : Entity, IMustHaveTenant
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
