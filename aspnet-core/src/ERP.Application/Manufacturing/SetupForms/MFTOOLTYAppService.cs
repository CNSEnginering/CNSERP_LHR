using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Manufacturing.SetupForms.Exporting;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;

namespace ERP.Manufacturing.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_MFTOOLTY)]
    public class MFTOOLTYAppService : ERPAppServiceBase, IMFTOOLTYAppService
    {
        private readonly IRepository<MFTOOLTY> _mftooltyRepository;
        private readonly IMFTOOLTYExcelExporter _mftooltyExcelExporter;

        public MFTOOLTYAppService(IRepository<MFTOOLTY> mftooltyRepository, IMFTOOLTYExcelExporter mftooltyExcelExporter)
        {
            _mftooltyRepository = mftooltyRepository;
            _mftooltyExcelExporter = mftooltyExcelExporter;

        }

        public async Task<PagedResultDto<GetMFTOOLTYForViewDto>> GetAll(GetAllMFTOOLTYInput input)
        {

            var filteredMFTOOLTY = _mftooltyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TOOLTYID.Contains(input.Filter) || e.TOOLTYDESC.Contains(input.Filter) || e.UNIT.Contains(input.Filter) || e.COMMENTS.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYIDFilter), e => e.TOOLTYID == input.TOOLTYIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYDESCFilter), e => e.TOOLTYDESC == input.TOOLTYDESCFilter)
                        
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UNITFilter), e => e.UNIT == input.UNITFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.COMMENTSFilter), e => e.COMMENTS == input.COMMENTSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredMFTOOLTY = filteredMFTOOLTY
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mftoolty = from o in pagedAndFilteredMFTOOLTY
                           select new GetMFTOOLTYForViewDto()
                           {
                               MFTOOLTY = new MFTOOLTYDto
                               {
                                   TOOLTYID = o.TOOLTYID,
                                   TOOLTYDESC = o.TOOLTYDESC,
                                  
                                   UNITCOST = o.UNITCOST,
                                   UNIT = o.UNIT,
                                   COMMENTS = o.COMMENTS,
                                   AudtUser = o.AudtUser,
                                   AudtDate = o.AudtDate,
                                   CreatedBy = o.CreatedBy,
                                   CreateDate = o.CreateDate,
                                   Id = o.Id
                               }
                           };

            var totalCount = await filteredMFTOOLTY.CountAsync();
            var mfToolsetList = await mftoolty.ToListAsync();
            return new PagedResultDto<GetMFTOOLTYForViewDto>(
                totalCount,
               mfToolsetList
            );
        }
    
        public async Task<GetMFTOOLTYForViewDto> GetMFTOOLTYForView(int id)
        {
            var mftoolty = await _mftooltyRepository.GetAsync(id);

            var output = new GetMFTOOLTYForViewDto { MFTOOLTY = ObjectMapper.Map<MFTOOLTYDto>(mftoolty) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MFTOOLTY_Edit)]
        public async Task<GetMFTOOLTYForEditOutput> GetMFTOOLTYForEdit(EntityDto input)
        {
            var mftoolty = await _mftooltyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMFTOOLTYForEditOutput { MFTOOLTY = ObjectMapper.Map<CreateOrEditMFTOOLTYDto>(mftoolty) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFTOOLTYDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MFTOOLTY_Create)]
        protected virtual async Task Create(CreateOrEditMFTOOLTYDto input)
        {
            input.CreateDate = DateTime.Now.Date;
            input.CreatedBy = GetCurrentUserName().Result.UserName;
            var mftoolty = ObjectMapper.Map<MFTOOLTY>(input);


            if (AbpSession.TenantId != null)
            {
                mftoolty.TenantId = (int)AbpSession.TenantId;
                
            }

            await _mftooltyRepository.InsertAsync(mftoolty);
        }

        [AbpAuthorize(AppPermissions.Pages_MFTOOLTY_Edit)]
        protected virtual async Task Update(CreateOrEditMFTOOLTYDto input)
        {
            input.AudtDate = DateTime.Now.Date;
            input.AudtUser = GetCurrentUserName().Result.UserName;
            var mftoolty = await _mftooltyRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, mftoolty);
        }

        [AbpAuthorize(AppPermissions.Pages_MFTOOLTY_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mftooltyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMFTOOLTYToExcel(GetAllMFTOOLTYForExcelInput input)
        {

            var filteredMFTOOLTY = _mftooltyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TOOLTYID.Contains(input.Filter) || e.TOOLTYDESC.Contains(input.Filter) || e.UNIT.Contains(input.Filter) || e.COMMENTS.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYIDFilter), e => e.TOOLTYID == input.TOOLTYIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYDESCFilter), e => e.TOOLTYDESC == input.TOOLTYDESCFilter)
                        //.WhereIf(input.MinSTATUSFilter != null, e => e.STATUS >= input.MinSTATUSFilter)
                        //.WhereIf(input.MaxSTATUSFilter != null, e => e.STATUS <= input.MaxSTATUSFilter)
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UNITFilter), e => e.UNIT == input.UNITFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.COMMENTSFilter), e => e.COMMENTS == input.COMMENTSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredMFTOOLTY
                         select new GetMFTOOLTYForViewDto()
                         {
                             MFTOOLTY = new MFTOOLTYDto
                             {
                                 TOOLTYID = o.TOOLTYID,
                                 TOOLTYDESC = o.TOOLTYDESC,
                                 STATUS = o.STATUS,
                                 UNITCOST = o.UNITCOST,
                                 UNIT = o.UNIT,
                                 COMMENTS = o.COMMENTS,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var mftooltyListDtos = await query.ToListAsync();

            return _mftooltyExcelExporter.ExportToFile(mftooltyListDtos);
        }
        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }
    }
}