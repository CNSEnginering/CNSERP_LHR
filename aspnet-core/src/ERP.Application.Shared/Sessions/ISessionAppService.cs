using System.Threading.Tasks;
using Abp.Application.Services;
using ERP.Sessions.Dto;

namespace ERP.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
