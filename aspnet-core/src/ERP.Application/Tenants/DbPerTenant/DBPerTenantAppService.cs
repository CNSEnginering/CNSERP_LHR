using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERP.Tenants.DbPerTenant;
using ERP.Tenants.DbPerTenant.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Tenants.DbPerTenant
{
    public class DBPerTenantAppService : ERPAppServiceBase, IDBPerTenantAppService
    {

        private readonly IRepository<ConnectionPerTenant> _ConnectionPerTenantRepository;
        public DBPerTenantAppService(IRepository<ConnectionPerTenant> ConnectionPerTenantRepository)
        {
            _ConnectionPerTenantRepository = ConnectionPerTenantRepository;
        }

        public async Task<ListResultDto<GetAllConnections>> GetAllConnections(int TenantId, string ModuleID)
        {
            var Connections = _ConnectionPerTenantRepository.GetAll().Where(x => x.TenantId == TenantId && x.ModuleID == ModuleID);

            var final = from o in Connections
                        select new GetAllConnections
                        {
                            TenantId = o.TenantId,
                            DatabaseName = o.DatabaseName,
                            ModuleID = o.ModuleID,
                            Password = o.Password,
                            ServerName = o.ServerName,
                            UserID = o.UserID,
                            VConnectionString = o.VConnectionString
                        };

            return new ListResultDto<GetAllConnections>(
                await final.ToListAsync()
                );


        }
    }
}
