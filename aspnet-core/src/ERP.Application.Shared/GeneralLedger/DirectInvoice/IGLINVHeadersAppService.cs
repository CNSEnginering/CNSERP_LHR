using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.GeneralLedger.DirectInvoice.Dtos;

namespace ERP.GeneralLedger.DirectInvoice
{
    public interface IGLINVHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<GLINVHeaderDto>> GetAll(GetAllGLINVHeadersInput input);

		Task<GLINVHeaderDto> GetGLINVHeaderForEdit(EntityDto input);

		
    }
}