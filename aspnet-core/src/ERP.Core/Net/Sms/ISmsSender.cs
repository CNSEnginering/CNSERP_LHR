using System.Threading.Tasks;

namespace ERP.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}