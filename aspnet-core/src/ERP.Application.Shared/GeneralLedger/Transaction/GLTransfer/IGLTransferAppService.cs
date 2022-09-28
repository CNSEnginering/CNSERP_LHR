using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.Dtos;
using ERP.Dto;
using ERP.GeneralLedger.Transaction.GLTransfer.Dtos;

namespace ERP.GeneralLedger.Transaction.GLTransfer
{
    public interface IGLTransferAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLTransferForViewDto>> GetAll(GetAllGLTransferInput input);

        Task<GetGLTransferForViewDto> GetGLTransferForView(int id);

		Task<GetGLTransferForEditOutput> GetGLTransferForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLTransferDto input);

        Task Delete(EntityDto input);
        Task<FileDto> GetGLTransferToExcel(GetAllGLTransferForExcelInput input);



    }
}