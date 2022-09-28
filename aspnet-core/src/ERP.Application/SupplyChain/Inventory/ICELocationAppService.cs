

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_ICELocation)]
    public class ICELocationAppService : ERPAppServiceBase, IICELocationAppService
    {
        private readonly IRepository<ICELocation> _iceLocationRepository;
        private readonly IRepository<User, long> _userRepository;

        public ICELocationAppService(IRepository<ICELocation> iceLocationRepository,
            IRepository<User, long> userRepository)
        {
            _iceLocationRepository = iceLocationRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetICELocationForViewDto>> GetAll(GetAllICELocationInput input)
        {

            var filteredICELocation = _iceLocationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocationTitle.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.ApprovedBy.Contains(input.Filter))
                        //.WhereIf(input.MinIDFilter != null, e => e.ID >= input.MinIDFilter)
                        //.WhereIf(input.MaxIDFilter != null, e => e.ID <= input.MaxIDFilter)
                        //.WhereIf(input.MinTenantIDFilter != null, e => e.TenantID >= input.MinTenantIDFilter)
                        //.WhereIf(input.MaxTenantIDFilter != null, e => e.TenantID <= input.MaxTenantIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleFilter), e => e.LocationTitle == input.LocationTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApprovedByFilter), e => e.ApprovedBy == input.ApprovedByFilter)
                        .WhereIf(input.MinApprovedDateFilter != null, e => e.ApprovedDate >= input.MinApprovedDateFilter)
                        .WhereIf(input.MaxApprovedDateFilter != null, e => e.ApprovedDate <= input.MaxApprovedDateFilter);

            var pagedAndFilteredICELocation = filteredICELocation
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var iceLocation = from o in pagedAndFilteredICELocation
                              join
                              a in pagedAndFilteredICELocation.Where(o => o.TenantId == AbpSession.TenantId)
                              on  o.ParentId equals  a.Id into parent
                              from parentDesc in parent.DefaultIfEmpty()
                              select new GetICELocationForViewDto()
                              {
                                  ICELocation = new ICELocationDto
                                  {
                                      //ID = o.ID,
                                      ParentId = o.ParentId,
                                      ParentDesc = parentDesc.LocationTitle,
                                      //TenantID = o.TenantID,
                                      LocationTitle = o.LocationTitle,
                                      AudtUser = o.AudtUser,
                                      AudtDate = o.AudtDate,
                                      ApprovedBy = o.ApprovedBy,
                                      ApprovedDate = o.ApprovedDate,
                                      Id = o.Id
                                  }
                              };

            var totalCount = await filteredICELocation.CountAsync();

            return new PagedResultDto<GetICELocationForViewDto>(
                totalCount,
                await iceLocation.ToListAsync()
            );
        }

        public List<ICELocation> GetParentLocationList()
        {
            List<ICELocation> list = new List<ICELocation>();
            list = (from a in _iceLocationRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId)
                    select new ICELocation()
                    {
                        ParentId = a.Id,
                        LocationTitle = a.LocationTitle
                    }).ToList();
            list.Insert(0, new ICELocation() { ParentId = 0, LocationTitle = "Please Select" });
            return list;
        }

        public async Task<GetICELocationForViewDto> GetICELocationForView(int id)
        {
            var iceLocation = await _iceLocationRepository.GetAsync(id);

            var output = new GetICELocationForViewDto { ICELocation = ObjectMapper.Map<ICELocationDto>(iceLocation) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_ICELocation_Edit)]
        public async Task<GetICELocationForEditOutput> GetICELocationForEdit(EntityDto input)
        {
            var iceLocation = await _iceLocationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetICELocationForEditOutput { ICELocation = ObjectMapper.Map<CreateOrEditICELocationDto>(iceLocation) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditICELocationDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_ICELocation_Create)]
        protected virtual async Task Create(CreateOrEditICELocationDto input)
        {
            var iceLocation = ObjectMapper.Map<ICELocation>(input);


            if (AbpSession.TenantId != null)
            {
                iceLocation.TenantId = (int)AbpSession.TenantId;
            }
            iceLocation.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            iceLocation.AudtDate = DateTime.Now;



            await _iceLocationRepository.InsertAsync(iceLocation);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICELocation_Edit)]
        protected virtual async Task Update(CreateOrEditICELocationDto input)
        {

            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;

            var iceLocation = await _iceLocationRepository.FirstOrDefaultAsync((int)input.Id);
            if (AbpSession.TenantId != null)
            {
                input.TenantId = (int)AbpSession.TenantId;
            }
            ObjectMapper.Map(input, iceLocation);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICELocation_Delete)]
        public async Task Delete(int Id)
        {
            await _iceLocationRepository.DeleteAsync(Id);
        }
    }
}