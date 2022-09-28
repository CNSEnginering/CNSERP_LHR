using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace ERP.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
