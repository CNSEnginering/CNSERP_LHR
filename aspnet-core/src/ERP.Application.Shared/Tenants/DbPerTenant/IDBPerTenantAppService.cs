using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Tenants.DbPerTenant.Dto;
using System.Threading.Tasks;

namespace ERP.Tenants.DbPerTenant
{
    public interface IDBPerTenantAppService: IApplicationService
    {
        Task<ListResultDto<GetAllConnections>> GetAllConnections(int TenantId, string ModuleID);
    }
}
