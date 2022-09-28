using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.ICOPT4.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.ICOPT4
{
    public interface IICOPT4AppService : IApplicationService 
    {
        Task<PagedResultDto<GetICOPT4ForViewDto>> GetAll(GetAllICOPT4Input input);

        Task<GetICOPT4ForViewDto> GetICOPT4ForView(int id);

		Task<GetICOPT4ForEditOutput> GetICOPT4ForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditICOPT4Dto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetICOPT4ToExcel(GetAllICOPT4ForExcelInput input);

		
    }
}