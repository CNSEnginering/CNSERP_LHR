using ERP.GeneralLedger.SetupForms;


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
    [AbpAuthorize(AppPermissions.Pages_GLCONFIG)]
    public class GLCONFIGAppService : ERPAppServiceBase, IGLCONFIGAppService
    {
        private readonly IRepository<GLCONFIG> _glconfigRepository;
        private readonly IGLCONFIGExcelExporter _glconfigExcelExporter;
        private readonly IRepository<GLBOOKS> _lookup_glbooksRepository;
        private readonly IRepository<ChartofControl, string> _lookup_chartofControlRepository;
        private readonly IRepository<AccountSubLedger, int> _lookup_accountSubLedgerRepository;
        private readonly IRepository<User, long> _userRepository;

        public GLCONFIGAppService(IRepository<GLCONFIG> glconfigRepository, IGLCONFIGExcelExporter glconfigExcelExporter,
            IRepository<GLBOOKS> lookup_glbooksRepository, IRepository<ChartofControl, string> lookup_chartofControlRepository,
            IRepository<AccountSubLedger, int> lookup_accountSubLedgerRepository,
            IRepository<User, long> userRepository
            )
        {
            _glconfigRepository = glconfigRepository;
            _glconfigExcelExporter = glconfigExcelExporter;
            _lookup_glbooksRepository = lookup_glbooksRepository;
            _lookup_chartofControlRepository = lookup_chartofControlRepository;
            _lookup_accountSubLedgerRepository = lookup_accountSubLedgerRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<GetGLCONFIGForViewDto>> GetAll(GetAllGLCONFIGInput input)
        {

            var filteredGLCONFIG = _glconfigRepository.GetAll()



                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.BookID.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                        .WhereIf(input.PostingOnFilter > -1, e => Convert.ToInt32(e.PostingOn) == input.PostingOnFilter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
            //.OrderByDescending(o => o.AUDTDATE);



            var pagedAndFilteredGLCONFIG = filteredGLCONFIG
                .OrderBy(input.Sorting ?? "AUDTDATE desc")
                .PageBy(input);

            var glconfig = from o in pagedAndFilteredGLCONFIG




                           select new GetGLCONFIGForViewDto()
                           {
                               GLCONFIG = new GLCONFIGDto
                               {
                                   AccountID = o.AccountID,
                                   SubAccID = o.SubAccID,
                                   ConfigID = o.ConfigID,
                                   BookID = o.BookID,
                                   PostingOn = o.PostingOn,
                                   AUDTDATE = o.AUDTDATE,
                                   AUDTUSER = o.AUDTUSER,
                                   Id = o.Id,
                                   BookName = o.BookID != "" ? _lookup_glbooksRepository.GetAll().Where(a => a.BookID == o.BookID && a.TenantId == o.TenantId).SingleOrDefault().BookName : "",
                                   AccountName = o.AccountID != "" ? _lookup_chartofControlRepository.GetAll().Where(a => a.Id == o.AccountID && a.TenantId == o.TenantId).SingleOrDefault().AccountName : "",
                               },


                           };

            var totalCount = await filteredGLCONFIG.CountAsync();

            return new PagedResultDto<GetGLCONFIGForViewDto>(
                totalCount,
                await glconfig.ToListAsync()
            );
        }

        public string GetGlConfigId(string bookId, string accId)
        {
            var data = _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.AccountID == accId).Count() > 0 ?
                   _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.AccountID == accId).FirstOrDefault().ConfigID :
                   0;
            return data.ToString();
        }

        public string GetGlConfigAccount(string bookId, string accId)
        {
            var data = _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.AccountID == accId).Count() > 0 ?
                   _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.AccountID == accId).FirstOrDefault().AccountID :
                   "";
            return data;
        }

        public async Task<GetGLCONFIGForViewDto> GetGLCONFIGForView(int id)
        {
            var glconfig = await _glconfigRepository.GetAsync(id);

            var output = new GetGLCONFIGForViewDto { GLCONFIG = ObjectMapper.Map<GLCONFIGDto>(glconfig) };

            if (output.GLCONFIG.GLBOOKSId != null)
            {
                var _lookupGLBOOKS = await _lookup_glbooksRepository.FirstOrDefaultAsync(output.GLCONFIG.GLBOOKSId);
                output.GLBOOKSBookName = _lookupGLBOOKS.BookName.ToString();
            }

            if (output.GLCONFIG.ChartofControlId != null)
            {
                var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync((string)output.GLCONFIG.ChartofControlId);
                output.ChartofControlAccountName = _lookupChartofControl.AccountName.ToString();
            }

            if (output.GLCONFIG.AccountSubLedgerId != null)
            {
                var _lookupAccountSubLedger = await _lookup_accountSubLedgerRepository.FirstOrDefaultAsync((int)output.GLCONFIG.AccountSubLedgerId);
                output.AccountSubLedgerSubAccName = _lookupAccountSubLedger.SubAccName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLCONFIG_Edit)]
        public async Task<GetGLCONFIGForEditOutput> GetGLCONFIGForEdit(EntityDto input)
        {
            var glconfig = await _glconfigRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGLCONFIGForEditOutput { GLCONFIG = ObjectMapper.Map<CreateOrEditGLCONFIGDto>(glconfig) };

            if (output.GLCONFIG.BookID != null)
            {
                var _lookupGLBOOKS = await _lookup_glbooksRepository.FirstOrDefaultAsync(o => o.BookID == (string)output.GLCONFIG.BookID);
                output.GLBOOKSBookName = _lookupGLBOOKS.BookName.ToString();
            }

            if (output.GLCONFIG.AccountID != null)
            {
                var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync(o => o.Id == output.GLCONFIG.AccountID);
                output.ChartofControlAccountName = _lookupChartofControl.AccountName.ToString();
            }

            if (output.GLCONFIG.SubAccID != 0)
            {
                var _lookupAccountSubLedger = await _lookup_accountSubLedgerRepository.FirstOrDefaultAsync(o => o.Id == (int)output.GLCONFIG.SubAccID);
                output.AccountSubLedgerSubAccName = _lookupAccountSubLedger.SubAccName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGLCONFIGDto input)
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

        [AbpAuthorize(AppPermissions.Pages_GLCONFIG_Create)]
        protected virtual async Task Create(CreateOrEditGLCONFIGDto input)
        {
            var glconfig = ObjectMapper.Map<GLCONFIG>(input);


            if (AbpSession.TenantId != null)
            {
                glconfig.TenantId = (int)AbpSession.TenantId;
                var checkUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault();
                if (checkUser != null)
                    glconfig.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

                input.AUDTUSER = glconfig.AUDTUSER;
            }
            glconfig.AUDTDATE = DateTime.Now;

            //glconfig.AccountID = input.ChartofControlId;
            glconfig.BookID = input.GLBOOKSId;
            if (glconfig.SubAccID != 0)
            {
                glconfig.SubAccID = input.SubAccID;
            }
            else
            {
                glconfig.SubAccID = 0;
            }

            //if (glconfig.Id.IsNullOrWhiteSpace())
            //{
            //    //glconfig.Id = Guid.NewGuid().ToString();
            //    var id = Convert.ToInt32(_glconfigRepository.GetAll().Max(o => o.Id)) + 1;
            //    glconfig.Id = id.ToString();
            //}

            await _glconfigRepository.InsertAsync(glconfig);
        }

        [AbpAuthorize(AppPermissions.Pages_GLCONFIG_Edit)]
        protected virtual async Task Update(CreateOrEditGLCONFIGDto input)
        {
            var glconfig = await _glconfigRepository.FirstOrDefaultAsync(input.Id);
            glconfig.AUDTDATE = DateTime.Now;
            if (AbpSession.TenantId != null)
            {
                glconfig.TenantId = (int)AbpSession.TenantId;
                var checkUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault();
                if (checkUser != null)
                    glconfig.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

                input.AUDTUSER = glconfig.AUDTUSER;
            }
            ObjectMapper.Map(input, glconfig);
        }

        [AbpAuthorize(AppPermissions.Pages_GLCONFIG_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _glconfigRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetGLCONFIGToExcel(GetAllGLCONFIGForExcelInput input)
        {

            var filteredGLCONFIG = _glconfigRepository.GetAll()



                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.BookID.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                        .WhereIf(input.PostingOnFilter > -1, e => Convert.ToInt32(e.PostingOn) == input.PostingOnFilter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);


            var query = (from o in filteredGLCONFIG


                         select new GetGLCONFIGForViewDto()
                         {
                             GLCONFIG = new GLCONFIGDto
                             {
                                 AccountID = o.AccountID,
                                 SubAccID = o.SubAccID,
                                 ConfigID = o.ConfigID,
                                 BookID = o.BookID,
                                 PostingOn = o.PostingOn,
                                 AUDTDATE = o.AUDTDATE,
                                 AUDTUSER = o.AUDTUSER,
                                 Id = o.Id
                             },

                         });


            var glconfigListDtos = await query.ToListAsync();

            return _glconfigExcelExporter.ExportToFile(glconfigListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_GLCONFIG)]
        public async Task<PagedResultDto<GLCONFIGGLBOOKSLookupTableDto>> GetAllGLBOOKSForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_glbooksRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.BookName.ToLower().ToString().Contains(input.Filter.ToLower())
               ).Where(o => o.TenantId == AbpSession.TenantId && o.INACTIVE == false);

            var totalCount = await query.CountAsync();

            var glbooksList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<GLCONFIGGLBOOKSLookupTableDto>();
            foreach (var glbooks in glbooksList)
            {
                lookupTableDtoList.Add(new GLCONFIGGLBOOKSLookupTableDto
                {
                    Id = glbooks.BookID,
                    DisplayName = glbooks.BookName?.ToString()
                });
            }

            return new PagedResultDto<GLCONFIGGLBOOKSLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        public int MaxidConfig(string bookId)
        {

            int maxid = 0;
            if (bookId != "" && bookId != null)
            {
                maxid = ((from tab1 in _glconfigRepository.GetAll().Where(o => o.BookID == bookId && o.TenantId == AbpSession.TenantId) select (int?)tab1.ConfigID).Max() ?? 0) + 1;
            }
            return maxid;
        }
        [AbpAuthorize(AppPermissions.Pages_GLCONFIG)]
        public async Task<PagedResultDto<GLCONFIGChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_chartofControlRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.AccountName.ToLower().ToString().Contains(input.Filter.ToLower())
               ).Where(o => o.TenantId == AbpSession.TenantId && o.Inactive == false);

            var totalCount = await query.CountAsync();

            var chartofControlList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<GLCONFIGChartofControlLookupTableDto>();
            foreach (var chartofControl in chartofControlList)
            {
                lookupTableDtoList.Add(new GLCONFIGChartofControlLookupTableDto
                {
                    Id = chartofControl.Id,
                    DisplayName = chartofControl.AccountName?.ToString()
                });
            }

            return new PagedResultDto<GLCONFIGChartofControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_GLCONFIG)]
        public async Task<PagedResultDto<GLCONFIGAccountSubLedgerLookupTableDto>> GetAllAccountSubLedgerForLookupTable(GetAllForLookupTableInput input, string accountid)
        {
            int totalCount = 0;
            var lookupTableDtoList = new List<GLCONFIGAccountSubLedgerLookupTableDto>();
            if (accountid != "" && accountid != null)
            {
                var checkSubLedger = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == accountid && o.Inactive == false && o.TenantId == AbpSession.TenantId).SingleOrDefault();

                if (checkSubLedger != null)
                {
                    if (checkSubLedger.SubLedger)
                    {
                        var query = _lookup_accountSubLedgerRepository.GetAll().WhereIf(
                                     !string.IsNullOrWhiteSpace(input.Filter),
                                    e => e.SubAccName.ToLower().ToString().Contains(input.Filter.ToLower())
                                 ).Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountid);

                        totalCount = await query.CountAsync();

                        var accountSubLedgerList = await query
                            .PageBy(input)
                            .ToListAsync();


                        foreach (var accountSubLedger in accountSubLedgerList)
                        {
                            lookupTableDtoList.Add(new GLCONFIGAccountSubLedgerLookupTableDto
                            {
                                Id = accountSubLedger.Id,
                                DisplayName = accountSubLedger.SubAccName?.ToString()
                            });
                        }
                    }

                }
            }

            return new PagedResultDto<GLCONFIGAccountSubLedgerLookupTableDto>(
                   totalCount,
                   lookupTableDtoList
               );
        }
    }
}