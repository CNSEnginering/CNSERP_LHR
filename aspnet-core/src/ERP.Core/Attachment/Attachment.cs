using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.Attachment
{
    [Table("AppDocs")]
    public class Attachment : Entity, IMustHaveTenant
    {
       
        
            public int TenantId { get; set; }

            public virtual int? APPID { get; set; }
            public virtual string AppName { get; set; }
            public virtual int? DocID { get; set; }
            public virtual Guid? FileID { get; set; }
            public virtual string FileName { get; set; }

    }
}
