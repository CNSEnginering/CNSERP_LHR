using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory.IC_Segment3
{
    public interface IICSegment3AppService: IApplicationService
    {
        Task<PagedResultDto<GetICSegment3ForViewDto>> GetAll(GetAllICSegment3Input input);

        Task<GetICSegment3ForViewDto> GetICSegment3ForView(int id);

        Task<GetICSegment3ForEditOutput> GetICSegment3ForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditICSegment3Dto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetICSegment3ToExcel(GetAllICSegment3ForExcelInput input);

        Task<PagedResultDto<NameValueDto>> GetICSegment3ForFinder(FindIcSegment3Input input);
    }
}
