using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Common.Dto;
using ERP.Editions.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;

namespace ERP.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();

        //Task<PagedResultDto<NameValueDto>> GetICSegment1ForFinder(FindIcSegment1Input input);
        //Task<PagedResultDto<NameValueDto>> FindICSegment2(GetAllIcSegment2InputForFinder input);
        //Task<PagedResultDto<NameValueDto>> FindICSegment3(FindIcSegment3Input input);
    }
}