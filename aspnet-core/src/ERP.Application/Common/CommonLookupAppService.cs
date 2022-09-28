using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using ERP.Common.Dto;
using ERP.Editions;
using ERP.Editions.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.SupplyChain.Inventory.IC_Segment3;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;

namespace ERP.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : ERPAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        //private readonly IICSegment1AppService _ICSegment1AppService;
        //private readonly IICSegment2AppService _ICSegment2AppService;
        //private readonly IICSegment3AppService _ICSegment3AppService;


        public CommonLookupAppService(EditionManager editionManager)
        {
            _editionManager = editionManager;
            //_ICSegment1AppService = ICSegment1AppService;
            //_ICSegment2AppService = ICSegment2AppService;
            //_ICSegment3AppService = ICSegment3AppService;
        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    );

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }

        //public async Task<PagedResultDto<NameValueDto>> GetICSegment1ForFinder(FindIcSegment1Input input)
        //{
        //    return await _ICSegment1AppService.GetICSegment1ForFinder(input);
        //}

        //public async Task<PagedResultDto<NameValueDto>> FindICSegment2(GetAllIcSegment2InputForFinder input)
        //{
        //    return await _ICSegment2AppService.GetICSegment2ForFinder(input);
        //}

        //public async Task<PagedResultDto<NameValueDto>> FindICSegment3(FindIcSegment3Input input)
        //{
        //    return await _ICSegment3AppService.GetICSegment3ForFinder(input);
        //}
    }
}
