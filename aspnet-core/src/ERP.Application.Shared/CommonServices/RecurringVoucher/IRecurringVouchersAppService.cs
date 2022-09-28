using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.RecurringVoucher.Dtos;
using ERP.Dto;


namespace ERP.CommonServices.RecurringVoucher
{
    public interface IRecurringVouchersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRecurringVoucherForViewDto>> GetAll(GetAllRecurringVouchersInput input);

        Task<GetRecurringVoucherForViewDto> GetRecurringVoucherForView(int id);

		Task<GetRecurringVoucherForEditOutput> GetRecurringVoucherForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRecurringVoucherDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRecurringVouchersToExcel(GetAllRecurringVouchersForExcelInput input);

		
    }
}