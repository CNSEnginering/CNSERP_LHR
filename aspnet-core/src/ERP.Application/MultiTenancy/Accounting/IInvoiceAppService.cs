using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ERP.MultiTenancy.Accounting.Dto;

namespace ERP.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
