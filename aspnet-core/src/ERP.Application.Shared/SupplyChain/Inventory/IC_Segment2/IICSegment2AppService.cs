using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory.IC_Segment2
{
    public interface IICSegment2AppService: IApplicationService
    {
        Task<PagedResultDto<GetICSegment2ForViewDto>> GetAll(GetAllICSegment2Input input);

        Task<GetICSegment2ForViewDto> GetICSegment2ForView(int id);

        Task<GetICSegment2ForEditOutput> GetICSegment2ForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditICSegment2Dto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetICSegment2ToExcel(GetAllICSegment2ForExcelInput input);

        Task<PagedResultDto<NameValueDto>> GetICSegment2ForFinder(GetAllIcSegment2InputForFinder input);
    }
}
