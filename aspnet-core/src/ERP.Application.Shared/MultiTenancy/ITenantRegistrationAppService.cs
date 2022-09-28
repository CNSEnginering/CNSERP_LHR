using System.Threading.Tasks;
using Abp.Application.Services;
using ERP.Editions.Dto;
using ERP.MultiTenancy.Dto;

namespace ERP.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}