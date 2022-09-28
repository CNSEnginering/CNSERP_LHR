using System.Threading.Tasks;
using ERP.Sessions.Dto;

namespace ERP.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
