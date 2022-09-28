using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.ICOPT5.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.ICOPT5
{
    public interface IICOPT5AppService : IApplicationService 
    {
        Task<PagedResultDto<GetICOPT5ForViewDto>> GetAll(GetAllICOPT5Input input);

        Task<GetICOPT5ForViewDto> GetICOPT5ForView(int id);

		Task<GetICOPT5ForEditOutput> GetICOPT5ForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditICOPT5Dto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetICOPT5ToExcel(GetAllICOPT5ForExcelInput input);

		
    }
}