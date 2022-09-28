using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.GeneralLedger.Transaction.BankReconcile
{
    public interface IBankReconcileDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBankReconcileDetailForViewDto>> GetAll(GetAllBankReconcileDetailsInput input);

		Task<GetBankReconcileDetailForEditOutput> GetBankReconcileDetailForEdit(int ID);

		Task CreateOrEdit(ICollection<CreateOrEditBankReconcileDetailDto> input);

		Task Delete(int input);

		Task<FileDto> GetBankReconcileDetailsToExcel(GetAllBankReconcileDetailsForExcelInput input);

		
    }
}