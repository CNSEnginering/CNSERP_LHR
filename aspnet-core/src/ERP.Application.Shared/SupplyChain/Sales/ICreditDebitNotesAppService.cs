using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.SupplyChain.Sales
{
    public interface ICreditDebitNotesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCreditDebitNoteForViewDto>> GetAll(GetAllCreditDebitNotesInput input);

		Task<GetCreditDebitNoteForEditOutput> GetCreditDebitNoteForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCreditDebitNoteDto input);

        Task Delete(EntityDto input);

        Task CreateOrEdit(List<CreditDebitNoteDetailDto> CreditDebitNoteDetailDtolist);
    }
}