using Abp.Domain.Repositories;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public class LedgerReportsAppService : ERPReportAppServiceBase
    {
       // [HttpGet("GetUserDTOs")]
        public List<UserDTO> GetUserDTOs(int ID = 0, string Name = "")
        {

            List<UserDTO> userDTO = new List<UserDTO>()
            {
                new UserDTO { ID = 1, Name = "Name 1" },
                new UserDTO { ID = 2, Name = "Name 2" },
                new UserDTO { ID = 3, Name = "Name 3" },
                new UserDTO { ID = 4, Name = "Name 4" },
            };

            userDTO = userDTO.FindAll(d => (d.ID == ID || ID == 0) && d.Name.Contains(Name));

            return userDTO;
        }



    }

    public class UserDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}
