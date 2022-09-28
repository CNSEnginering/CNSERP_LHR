using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD
{
    public interface IGLTRDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLTRDetailForViewDto>> FilterGLTRDData(int input);

		Task<GetGLTRDetailForEditOutput> GetGLTRDetailForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLTRDetailDto input);

		Task Delete(EntityDto input);

		
    }
}