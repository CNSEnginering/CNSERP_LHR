using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.Dtos;
using ERP.Dto;


namespace ERP.GeneralLedger.Transaction
{
    public interface IGlChequesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGlChequeForViewDto>> GetAll(GetAllGlChequesInput input);

        Task<GetGlChequeForViewDto> GetGlChequeForView(int id);

		Task<GetGlChequeForEditOutput> GetGlChequeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGlChequeDto input);

        Task Delete(EntityDto input);



    }
}