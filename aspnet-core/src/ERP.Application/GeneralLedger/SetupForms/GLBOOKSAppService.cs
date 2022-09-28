

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_GLBOOKS)]
    public class GLBOOKSAppService : ERPAppServiceBase, IGLBOOKSAppService
    {
        private readonly IRepository<GLBOOKS> _glbooksRepository;
        private readonly IGLBOOKSExcelExporter _glbooksExcelExporter;
        private readonly IRepository<User, long> _userRepository;

        public GLBOOKSAppService(IRepository<GLBOOKS> glbooksRepository, IGLBOOKSExcelExporter glbooksExcelExporter,IRepository<User, long> userRepository)
        {
            _glbooksRepository = glbooksRepository;
            _glbooksExcelExporter = glbooksExcelExporter;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetGLBOOKSForViewDto>> GetAll(GetAllGLBOOKSInput input)
        {

            var filteredGLBOOKS = _glbooksRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.BookName.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookNameFilter), e => e.BookName.ToLower() == input.BookNameFilter.ToLower().Trim())
                        .WhereIf(input.MinNormalEntryFilter != null, e => e.NormalEntry >= input.MinNormalEntryFilter)
                        .WhereIf(input.MaxNormalEntryFilter != null, e => e.NormalEntry <= input.MaxNormalEntryFilter)
                        .WhereIf(input.IntegratedFilter > -1, e => Convert.ToInt32(e.Integrated) == input.IntegratedFilter)
                        .WhereIf(input.INACTIVEFilter > -1, e => Convert.ToInt32(e.INACTIVE) == input.INACTIVEFilter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
                        .WhereIf(input.MinRestrictedFilter != null, e => e.Restricted >= input.MinRestrictedFilter)
                        .WhereIf(input.MaxRestrictedFilter != null, e => e.Restricted <= input.MaxRestrictedFilter).Where(o => o.TenantId == AbpSession.TenantId && o.TenantId > 0);
                       // .OrderByDescending(o => o.AUDTDATE);

            var pagedAndFilteredGLBOOKS = filteredGLBOOKS
                .OrderBy(input.Sorting ?? "AUDTDATE desc")
                .PageBy(input);

            var glbooks = from o in pagedAndFilteredGLBOOKS
                          select new GetGLBOOKSForViewDto()
                          {
                              GLBOOKS = new GLBOOKSDto
                              {
                                  BookID = o.BookID,
                                  BookName = o.BookName,
                                  NormalEntry = o.NormalEntry,
                                  Integrated = o.Integrated,
                                  INACTIVE = o.INACTIVE,
                                  AUDTDATE = o.AUDTDATE,
                                  AUDTUSER = o.AUDTUSER,
                                  Restricted = o.Restricted,
                                  Id = o.Id
                              }
                          };

            var totalCount = await filteredGLBOOKS.CountAsync();

            return new PagedResultDto<GetGLBOOKSForViewDto>(
                totalCount,
                await glbooks.ToListAsync()
            );
        }

        public async Task<GetGLBOOKSForViewDto> GetGLBOOKSForView(int id)
        {
            var glbooks = await _glbooksRepository.GetAsync(id);

            var output = new GetGLBOOKSForViewDto { GLBOOKS = ObjectMapper.Map<GLBOOKSDto>(glbooks) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLBOOKS_Edit)]
        public async Task<GetGLBOOKSForEditOutput> GetGLBOOKSForEdit(EntityDto input)
        {
            var glbooks = await _glbooksRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGLBOOKSForEditOutput { GLBOOKS = ObjectMapper.Map<CreateOrEditGLBOOKSDto>(glbooks) };

            return output;
        }




        public bool BookIdExists(string bookid)
        {
            var check = _glbooksRepository.GetAll().Where(o => o.BookID == bookid && o.TenantId == AbpSession.TenantId).SingleOrDefault();
            return check == null ? false : true;
        }
        public async Task CreateOrEdit(CreateOrEditGLBOOKSDto input)
        {
            if (input.Id == 0)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_GLBOOKS_Create)]
        protected virtual async Task Create(CreateOrEditGLBOOKSDto input)
        {
            var glbooks = ObjectMapper.Map<GLBOOKS>(input);


            if (AbpSession.TenantId != null)
            {
                glbooks.TenantId = (int)AbpSession.TenantId;
                var checkUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault();
                if(checkUser != null)
                   glbooks.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

                input.AUDTUSER = glbooks.AUDTUSER;
            }
            else
            {
                glbooks.TenantId = 0;
            }

           // if (glbooks.Id.IsNullOrWhiteSpace())
           // {
                // glbooks.Id = Guid.NewGuid().ToString();
                //var id = Convert.ToInt32(_glbooksRepository.GetAll().Max(o => o.Id)) + 1;
               // glbooks.Id = id.ToString();
           // }
            glbooks.AUDTDATE = DateTime.Now;
            //glbooks.AUDTUSER = Convert.ToString(AbpSession.UserId);
            glbooks.Integrated = false;
            glbooks.INACTIVE = false;
            await _glbooksRepository.InsertAsync(glbooks);
        }

        [AbpAuthorize(AppPermissions.Pages_GLBOOKS_Edit)]
        protected virtual async Task Update(CreateOrEditGLBOOKSDto input)
        {
            var glbooks = await _glbooksRepository.FirstOrDefaultAsync(input.Id);
            if (AbpSession.TenantId != null)
            {
                glbooks.TenantId = (int)AbpSession.TenantId;
                var checkUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault();
                if (checkUser != null)
                    glbooks.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

                input.AUDTUSER = glbooks.AUDTUSER;
            }
            glbooks.AUDTDATE = DateTime.Now;
            ObjectMapper.Map(input, glbooks);
        }

        [AbpAuthorize(AppPermissions.Pages_GLBOOKS_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _glbooksRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetGLBOOKSToExcel(GetAllGLBOOKSForExcelInput input)
        {

            var filteredGLBOOKS = _glbooksRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.BookName.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookNameFilter), e => e.BookName.ToLower() == input.BookNameFilter.ToLower().Trim())
                        .WhereIf(input.MinNormalEntryFilter != null, e => e.NormalEntry >= input.MinNormalEntryFilter)
                        .WhereIf(input.MaxNormalEntryFilter != null, e => e.NormalEntry <= input.MaxNormalEntryFilter)
                        .WhereIf(input.IntegratedFilter > -1, e => Convert.ToInt32(e.Integrated) == input.IntegratedFilter)
                        .WhereIf(input.INACTIVEFilter > -1, e => Convert.ToInt32(e.INACTIVE) == input.INACTIVEFilter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
                        .WhereIf(input.MinRestrictedFilter != null, e => e.Restricted >= input.MinRestrictedFilter)
                        .WhereIf(input.MaxRestrictedFilter != null, e => e.Restricted <= input.MaxRestrictedFilter).Where(o => o.TenantId == AbpSession.TenantId);

            var query = (from o in filteredGLBOOKS
                         select new GetGLBOOKSForViewDto()
                         {
                             GLBOOKS = new GLBOOKSDto
                             {
                                 BookID = o.BookID,
                                 BookName = o.BookName,
                                 NormalEntry = o.NormalEntry,
                                 Integrated = o.Integrated,
                                 INACTIVE = o.INACTIVE,
                                 AUDTDATE = o.AUDTDATE,
                                 AUDTUSER = o.AUDTUSER,
                                 Restricted = o.Restricted,
                                 Id = o.Id
                             }
                         });


            var glbooksListDtos = await query.ToListAsync();

            return _glbooksExcelExporter.ExportToFile(glbooksListDtos);
        }


    }
}