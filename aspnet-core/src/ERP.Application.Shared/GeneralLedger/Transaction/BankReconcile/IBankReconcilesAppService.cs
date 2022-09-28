using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.Transaction.BankReconcile
{
    public interface IBankReconcilesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBankReconcileForViewDto>> GetAll(GetAllBankReconcilesInput input);

		Task<GetBankReconcileForEditOutput> GetBankReconcileForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBankReconcileDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBankReconcilesToExcel(GetAllBankReconcilesForExcelInput input);

		
    }
}