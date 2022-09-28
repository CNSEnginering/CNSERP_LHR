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
using ERP.SupplyChain.Inventory;
using ERP.Authorization.Users;

namespace ERP.Manufacturing.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_MFAREA)]
    public class MFAREAAppService : ERPAppServiceBase, IMFAREAAppService
    {
        private readonly IRepository<MFAREA> _mfareaRepository;
        private readonly IMFAREAExcelExporter _mfareaExcelExporter;
        private readonly IRepository<ICLocation> _locRepository;
        public MFAREAAppService(IRepository<MFAREA> mfareaRepository, IRepository<ICLocation> locRepository, IMFAREAExcelExporter mfareaExcelExporter)
        {
            _mfareaRepository = mfareaRepository;
            _mfareaExcelExporter = mfareaExcelExporter;
            _locRepository = locRepository;
        }

        public async Task<PagedResultDto<GetMFAREAForViewDto>> GetAll(GetAllMFAREAInput input)
        {

            var filteredMFAREA = _mfareaRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AREAID.Contains(input.Filter) || e.AREADESC.Contains(input.Filter) || e.ADDRESS.Contains(input.Filter) || e.CONTNAME.Contains(input.Filter) || e.CONTPOS.Contains(input.Filter) || e.CONTCELL.Contains(input.Filter) || e.CONTEMAIL.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AREAIDFilter), e => e.AREAID == input.AREAIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AREADESCFilter), e => e.AREADESC == input.AREADESCFilter)
                        .WhereIf(input.MinAREATYFilter != null, e => e.AREATY >= input.MinAREATYFilter)
                        .WhereIf(input.MaxAREATYFilter != null, e => e.AREATY <= input.MaxAREATYFilter)
                        .WhereIf(input.MinSTATUSFilter != null, e => e.STATUS >= input.MinSTATUSFilter)
                        .WhereIf(input.MaxSTATUSFilter != null, e => e.STATUS <= input.MaxSTATUSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDRESSFilter), e => e.ADDRESS == input.ADDRESSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTNAMEFilter), e => e.CONTNAME == input.CONTNAMEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTPOSFilter), e => e.CONTPOS == input.CONTPOSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTCELLFilter), e => e.CONTCELL == input.CONTCELLFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTEMAILFilter), e => e.CONTEMAIL == input.CONTEMAILFilter)
                        .WhereIf(input.MinLOCIDFilter != null, e => e.LOCID >= input.MinLOCIDFilter)
                        .WhereIf(input.MaxLOCIDFilter != null, e => e.LOCID <= input.MaxLOCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredMFAREA = filteredMFAREA
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mfarea = from o in pagedAndFilteredMFAREA
                         join l in _locRepository.GetAll() on new { a = o.TenantId, b = o.LOCID } equals new { a = l.TenantId, b = l.LocID }
                         select new GetMFAREAForViewDto()
                         {
                             MFAREA = new MFAREADto
                             {
                                 AREAID = o.AREAID,
                                 AREADESC = o.AREADESC,
                                 AREATY = o.AREATY,
                                 STATUS = o.STATUS,
                                 ADDRESS = o.ADDRESS,
                                 CONTNAME = o.CONTNAME,
                                 CONTPOS = o.CONTPOS,
                                 CONTCELL = o.CONTCELL,
                                 CONTEMAIL = o.CONTEMAIL,
                                 LOCID = o.LOCID,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id,
                                 LocDesc = l.LocName
                             }
                         };

            var totalCount = await filteredMFAREA.CountAsync();

            return new PagedResultDto<GetMFAREAForViewDto>(
                totalCount,
                await mfarea.ToListAsync()
            );
        }

        public async Task<GetMFAREAForViewDto> GetMFAREAForView(int id)
        {
            var mfarea = await _mfareaRepository.GetAsync(id);

            var output = new GetMFAREAForViewDto { MFAREA = ObjectMapper.Map<MFAREADto>(mfarea) };
            output.MFAREA.LocDesc = _locRepository.GetAll().Where(o => o.LocID == output.MFAREA.LOCID && o.TenantId == AbpSession.TenantId).FirstOrDefault().LocName;
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MFAREA_Edit)]
        public async Task<GetMFAREAForEditOutput> GetMFAREAForEdit(EntityDto input)
        {
            var mfarea = await _mfareaRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMFAREAForEditOutput { MFAREA = ObjectMapper.Map<CreateOrEditMFAREADto>(mfarea) };
            output.MFAREA.LocDesc = _locRepository.GetAll().Where(o => o.LocID == output.MFAREA.LOCID && o.TenantId == AbpSession.TenantId).FirstOrDefault().LocName;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFAREADto input)
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

        [AbpAuthorize(AppPermissions.Pages_MFAREA_Create)]
        protected virtual async Task Create(CreateOrEditMFAREADto input)
        {
            var mfarea = ObjectMapper.Map<MFAREA>(input);

            if (AbpSession.TenantId != null)
            {
                mfarea.TenantId = (int)AbpSession.TenantId;
            }
            mfarea.CreateDate = DateTime.Now;
            mfarea.CreatedBy = GetCurrentUserName().Result.UserName;
            await _mfareaRepository.InsertAsync(mfarea);
        }

        [AbpAuthorize(AppPermissions.Pages_MFAREA_Edit)]
        protected virtual async Task Update(CreateOrEditMFAREADto input)
        {
            var mfarea = await _mfareaRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtDate = DateTime.Now;
            input.AudtUser = GetCurrentUserName().Result.UserName;
            ObjectMapper.Map(input, mfarea);
        }

        [AbpAuthorize(AppPermissions.Pages_MFAREA_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mfareaRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMFAREAToExcel(GetAllMFAREAForExcelInput input)
        {

            var filteredMFAREA = _mfareaRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AREAID.Contains(input.Filter) || e.AREADESC.Contains(input.Filter) || e.ADDRESS.Contains(input.Filter) || e.CONTNAME.Contains(input.Filter) || e.CONTPOS.Contains(input.Filter) || e.CONTCELL.Contains(input.Filter) || e.CONTEMAIL.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AREAIDFilter), e => e.AREAID == input.AREAIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AREADESCFilter), e => e.AREADESC == input.AREADESCFilter)
                        .WhereIf(input.MinAREATYFilter != null, e => e.AREATY >= input.MinAREATYFilter)
                        .WhereIf(input.MaxAREATYFilter != null, e => e.AREATY <= input.MaxAREATYFilter)
                        .WhereIf(input.MinSTATUSFilter != null, e => e.STATUS >= input.MinSTATUSFilter)
                        .WhereIf(input.MaxSTATUSFilter != null, e => e.STATUS <= input.MaxSTATUSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDRESSFilter), e => e.ADDRESS == input.ADDRESSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTNAMEFilter), e => e.CONTNAME == input.CONTNAMEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTPOSFilter), e => e.CONTPOS == input.CONTPOSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTCELLFilter), e => e.CONTCELL == input.CONTCELLFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTEMAILFilter), e => e.CONTEMAIL == input.CONTEMAILFilter)
                        .WhereIf(input.MinLOCIDFilter != null, e => e.LOCID >= input.MinLOCIDFilter)
                        .WhereIf(input.MaxLOCIDFilter != null, e => e.LOCID <= input.MaxLOCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredMFAREA
                         select new GetMFAREAForViewDto()
                         {
                             MFAREA = new MFAREADto
                             {
                                 AREAID = o.AREAID,
                                 AREADESC = o.AREADESC,
                                 AREATY = o.AREATY,
                                 STATUS = o.STATUS,
                                 ADDRESS = o.ADDRESS,
                                 CONTNAME = o.CONTNAME,
                                 CONTPOS = o.CONTPOS,
                                 CONTCELL = o.CONTCELL,
                                 CONTEMAIL = o.CONTEMAIL,
                                 LOCID = o.LOCID,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var mfareaListDtos = await query.ToListAsync();

            return _mfareaExcelExporter.ExportToFile(mfareaListDtos);
        }
        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }
    }

}