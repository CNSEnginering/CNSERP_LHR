using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.CommonServices
{
    [Table("City")]
    public class City : Entity
    {
        public virtual int CityID { get; set; }

        public virtual string Name { get; set; }

        [Required]
        public virtual int ProvinceID { get; set; }

        [Required]
        public virtual int CountryID { get; set; }

        public virtual string preFix { get; set; }

    }
}
