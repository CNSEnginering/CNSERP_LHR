using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory.IC_Segment1
{
    public interface IICSegment1AppService : IApplicationService
    {
        Task<PagedResultDto<GetICSegment1ForViewDto>> GetAll(GetAllICSegment1Input input);

        Task<GetICSegment1ForViewDto> GetICSegment1ForView(int id);

        Task<GetICSegment1ForEditOutput> GetICSegment1ForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditICSegment1Dto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetICSegment1ToExcel(GetAllICSgment1ForExcelInput input);

        Task<PagedResultDto<NameValueDto>> GetICSegment1ForFinder(FindIcSegment1Input input);

    }
}
