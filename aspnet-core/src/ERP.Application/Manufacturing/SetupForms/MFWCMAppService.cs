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
using Microsoft.AspNetCore.Mvc;

namespace ERP.Manufacturing.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_MFWCM)]
    public class MFWCMAppService : ERPAppServiceBase, IMFWCMAppService
    {
        private readonly IRepository<MFWCM> _mfwcmRepository;
        private readonly IMFWCMExcelExporter _mfwcmExcelExporter;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<MFWCRES> _mfwcresRepository;
        private readonly IRepository<MFWCTOL> _mfwctolRepository;
        public MFWCMAppService(IRepository<MFWCTOL> mfwctolRepository, IRepository<MFWCM> mfwcmRepository, IRepository<MFWCRES> mfwcresRepository, IRepository<User, long> userRepository, IMFWCMExcelExporter mfwcmExcelExporter)
        {
            _mfwcmRepository = mfwcmRepository;
            _mfwcmExcelExporter = mfwcmExcelExporter;
            _userRepository = userRepository;
            _mfwcresRepository = mfwcresRepository;
            _mfwctolRepository = mfwctolRepository;
        }

        public async Task<PagedResultDto<GetMFWCMForViewDto>> GetAll(GetAllMFWCMInput input)
        {

            var filteredMFWCM = _mfwcmRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.WCID.Contains(input.Filter) || e.WCESC.Contains(input.Filter) || e.COMMENTS.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCIDFilter), e => e.WCID == input.WCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCESCFilter), e => e.WCESC == input.WCESCFilter)
                        .WhereIf(input.MinTOTRSCCOSTFilter != null, e => e.TOTRSCCOST >= input.MinTOTRSCCOSTFilter)
                        .WhereIf(input.MaxTOTRSCCOSTFilter != null, e => e.TOTRSCCOST <= input.MaxTOTRSCCOSTFilter)
                        .WhereIf(input.MinTOTTLCOSTFilter != null, e => e.TOTTLCOST >= input.MinTOTTLCOSTFilter)
                        .WhereIf(input.MaxTOTTLCOSTFilter != null, e => e.TOTTLCOST <= input.MaxTOTTLCOSTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.COMMENTSFilter), e => e.COMMENTS == input.COMMENTSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredMFWCM = filteredMFWCM
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mfwcm = from o in pagedAndFilteredMFWCM
                        select new GetMFWCMForViewDto()
                        {
                            MFWCM = new MFWCMDto
                            {
                                WCID = o.WCID,
                                WCESC = o.WCESC,
                                TOTRSCCOST = o.TOTRSCCOST,
                                TOTTLCOST = o.TOTTLCOST,
                                COMMENTS = o.COMMENTS,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
                            }
                        };

            var totalCount = await filteredMFWCM.CountAsync();

            return new PagedResultDto<GetMFWCMForViewDto>(
                totalCount,
                await mfwcm.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCM_Edit)]
        public async Task<GetMFWCMForEditOutput> GetMFWCMForEdit(EntityDto input)
        {
            var mfwcm = await _mfwcmRepository.FirstOrDefaultAsync(input.Id);
            var mfwcres = await _mfwcresRepository.GetAll().Where(o => o.DetID == input.Id && o.TenantId == (int)AbpSession.TenantId).ToListAsync();
            var mfwctol = await _mfwctolRepository.GetAll().Where(o => o.DetID == input.Id && o.TenantId == (int)AbpSession.TenantId).ToListAsync();
            var output = new GetMFWCMForEditOutput { MFWCM = ObjectMapper.Map<CreateOrEditMFWCMDto>(mfwcm) };
            output.MFWCM.ResDetailDto = ObjectMapper.Map<List<CreateOrEditMFWCRESDto>>(mfwcres);
            output.MFWCM.ToolDetailDto = ObjectMapper.Map<List<CreateOrEditMFWCTOLDto>>(mfwctol);

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFWCMDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MFWCM_Create)]
        protected virtual async Task Create(CreateOrEditMFWCMDto input)
        {

            var mfwcm = ObjectMapper.Map<MFWCM>(input);
            mfwcm.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            mfwcm.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            mfwcm.AudtDate = DateTime.Now;
            mfwcm.CreateDate = DateTime.Now;
            try
            {
                if (AbpSession.TenantId != null)
                {
                    mfwcm.TenantId = (int)AbpSession.TenantId;
                }
                var Wcid = GetWCId(input.WCID);
                if (Wcid==0)
                {
                    var getId = await _mfwcmRepository.InsertAndGetIdAsync(mfwcm);

                    if (input.ResDetailDto != null && input.ResDetailDto.Count > 0)
                    {
                        foreach (var data in input.ResDetailDto)
                        {

                            var MfwcresDetail = ObjectMapper.Map<MFWCRES>(data);
                            if (AbpSession.TenantId != null)
                            {
                                MfwcresDetail.TenantId = (int)AbpSession.TenantId;
                            }
                            MfwcresDetail.DetID = getId;

                            MfwcresDetail.TOTALCOST = MfwcresDetail.REQQTY * MfwcresDetail.UNITCOST;

                            await _mfwcresRepository.InsertAsync(MfwcresDetail);


                        }
                    }

                    if (input.ToolDetailDto != null && input.ToolDetailDto.Count > 0)
                    {
                        foreach (var data in input.ToolDetailDto)
                        {

                            var MfwcToolDetail = ObjectMapper.Map<MFWCTOL>(data);
                            if (AbpSession.TenantId != null)
                            {
                                MfwcToolDetail.TenantId = (int)AbpSession.TenantId;
                            }
                            MfwcToolDetail.DetID = getId;

                            MfwcToolDetail.TOTALCOST = MfwcToolDetail.REQQTY * MfwcToolDetail.UNITCOST;

                            await _mfwctolRepository.InsertAsync(MfwcToolDetail);


                        }
                    }
                }
            
            }
            catch (Exception ex)
            {

               
            }
          
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCM_Edit)]
        protected virtual async Task Update(CreateOrEditMFWCMDto input)
        {
            var mfwcm = await _mfwcmRepository.FirstOrDefaultAsync((int)input.Id);
           
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            ObjectMapper.Map(input, mfwcm);
            await _mfwcresRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
            await _mfwctolRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
            if (input.ResDetailDto != null && input.ResDetailDto.Count > 0)
            {
                foreach (var data in input.ResDetailDto)
                {

                    var MfwcresDetail = ObjectMapper.Map<MFWCRES>(data);
                    if (AbpSession.TenantId != null)
                    {
                        MfwcresDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    MfwcresDetail.DetID = input.Id;

                    MfwcresDetail.TOTALCOST = MfwcresDetail.REQQTY * MfwcresDetail.UNITCOST;

                    await _mfwcresRepository.InsertAsync(MfwcresDetail);


                }
            }

            if (input.ToolDetailDto != null && input.ToolDetailDto.Count > 0)
            {
                foreach (var data in input.ToolDetailDto)
                {

                    var MfwcToolDetail = ObjectMapper.Map<MFWCTOL>(data);
                    if (AbpSession.TenantId != null)
                    {
                        MfwcToolDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    MfwcToolDetail.DetID = input.Id;

                    MfwcToolDetail.TOTALCOST = MfwcToolDetail.REQQTY * MfwcToolDetail.UNITCOST;

                    await _mfwctolRepository.InsertAsync(MfwcToolDetail);


                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCM_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mfwcmRepository.DeleteAsync(input.Id);
            await _mfwcresRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
            await _mfwctolRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
        }

        public async Task<FileDto> GetMFWCMToExcel(GetAllMFWCMForExcelInput input)
        {

            var filteredMFWCM = _mfwcmRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.WCID.Contains(input.Filter) || e.WCESC.Contains(input.Filter) || e.COMMENTS.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCIDFilter), e => e.WCID == input.WCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCESCFilter), e => e.WCESC == input.WCESCFilter)
                        .WhereIf(input.MinTOTRSCCOSTFilter != null, e => e.TOTRSCCOST >= input.MinTOTRSCCOSTFilter)
                        .WhereIf(input.MaxTOTRSCCOSTFilter != null, e => e.TOTRSCCOST <= input.MaxTOTRSCCOSTFilter)
                        .WhereIf(input.MinTOTTLCOSTFilter != null, e => e.TOTTLCOST >= input.MinTOTTLCOSTFilter)
                        .WhereIf(input.MaxTOTTLCOSTFilter != null, e => e.TOTTLCOST <= input.MaxTOTTLCOSTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.COMMENTSFilter), e => e.COMMENTS == input.COMMENTSFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredMFWCM
                         select new GetMFWCMForViewDto()
                         {
                             MFWCM = new MFWCMDto
                             {
                                 WCID = o.WCID,
                                 WCESC = o.WCESC,
                                 TOTRSCCOST = o.TOTRSCCOST,
                                 TOTTLCOST = o.TOTTLCOST,
                                 COMMENTS = o.COMMENTS,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var mfwcmListDtos = await query.ToListAsync();

            return _mfwcmExcelExporter.ExportToFile(mfwcmListDtos);
        }

        [HttpGet]
        public int GetWCId(string wcid)
        {
            var result = _mfwcmRepository.GetAll().DefaultIfEmpty().Where(x=>x.WCID==wcid).FirstOrDefault();
            return result==null?0:1;
        }
    }
}