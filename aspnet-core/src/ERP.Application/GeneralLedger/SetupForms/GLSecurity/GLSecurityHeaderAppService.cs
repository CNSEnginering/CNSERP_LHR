

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms.GLSecurity.Exporting;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;
using Abp.Runtime.Session;
using System.Data.SqlClient;
using System.Configuration;

namespace ERP.GeneralLedger.SetupForms.GLSecurity
{
    [AbpAuthorize(AppPermissions.Pages_GLSecurityHeader)]
    public class GLSecurityHeaderAppService : ERPAppServiceBase, IGLSecurityHeaderAppService
    {
        private readonly IRepository<GLSecurityHeader> _gLSecurityHeaderRepository;
        private readonly IGLSecurityHeaderExcelExporter _gLSecurityHeadersExcelExporter;
        private readonly IGLSecurityDetailAppService _gLSecurityDetailRepository;




        public GLSecurityHeaderAppService(
            IRepository<GLSecurityHeader> gLSecurityHeaderRepository, 
            IGLSecurityHeaderExcelExporter gLSecurityHeadersExcelExporter, 
            IGLSecurityDetailAppService gLSecurityDetailRepository)
        {
            _gLSecurityHeaderRepository = gLSecurityHeaderRepository;
            _gLSecurityHeadersExcelExporter = gLSecurityHeadersExcelExporter;
            _gLSecurityDetailRepository = gLSecurityDetailRepository;

        }

        public async Task<PagedResultDto<GetGLSecurityHeaderForViewDto>> GetAll(GetAllGLSecurityHeaderInput input)
        {

            var filteredGLSecurityHeaders = _gLSecurityHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.UserID.Contains(input.Filter) || e.UserName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserIDFilter), e => e.UserID.Trim().ToLower() == input.UserIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.Trim().ToLower() == input.UserNameFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.Trim().ToLower() == input.AudtUserFilter.Trim().ToLower())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.Trim().ToLower() == input.CreatedByFilter.Trim().ToLower())
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.AudtDate <= input.MaxCreatedDateFilter);


            var pagedAndFilteredGLSecurityHeaders = filteredGLSecurityHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var gLSecurityHeaders = from o in pagedAndFilteredGLSecurityHeaders
                                    select new GetGLSecurityHeaderForViewDto()
                                    {
                                        GLSecurityHeader = new GLSecurityHeaderDto
                                        {
                                            UserID = o.UserID,
                                            UserName = o.UserName,
                                            AudtUser = o.AudtUser,
                                            AudtDate = o.AudtDate,
                                            CreatedBy = o.CreatedBy,
                                            CreatedDate = o.CreatedDate,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredGLSecurityHeaders.CountAsync();

            return new PagedResultDto<GetGLSecurityHeaderForViewDto>(
                totalCount,
                await gLSecurityHeaders.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_GLSecurityHeader_Edit)]
        public async Task<GetGLSecurityHeaderForEditOutput> GetGLSecurityHeaderForEdit(EntityDto input)
        {
            var gLSecurityHeader = await _gLSecurityHeaderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGLSecurityHeaderForEditOutput { GLSecurityHeader = ObjectMapper.Map<CreateOrEditGLSecurityHeaderDto>(gLSecurityHeader) };

            var gLSecurityDetail = await _gLSecurityDetailRepository.GetGLSecurityDetailForEdit((int)output.GLSecurityHeader.Id);
            output.GLSecurityHeader.GLSecurityDetail = gLSecurityDetail.GLSecurityDetail;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGLSecurityHeaderDto input)
        {

            if (!input.flag)
            {
                await Create(input);


            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_GLSecurityHeader_Create)]
        protected virtual async Task Create(CreateOrEditGLSecurityHeaderDto input)
        {
            var gLSecurityHeader = ObjectMapper.Map<GLSecurityHeader>(input);


            if (AbpSession.TenantId != null)
            {
                gLSecurityHeader.TenantId = (int)AbpSession.TenantId;
            }


            var Hid = await _gLSecurityHeaderRepository.InsertAndGetIdAsync(gLSecurityHeader);
            foreach (var item in input.GLSecurityDetail)
            {
                item.DetID = Hid;
            }

                await _gLSecurityDetailRepository.CreateOrEdit(input.GLSecurityDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_GLSecurityHeader_Edit)]
        protected virtual async Task Update(CreateOrEditGLSecurityHeaderDto input)
        {
            var gLSecurityHeader = await _gLSecurityHeaderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, gLSecurityHeader);

            foreach (var item in input.GLSecurityDetail)
            {
                item.DetID = (int)input.Id;
            }

            await _gLSecurityDetailRepository.Delete((int)input.Id);
            await _gLSecurityDetailRepository.CreateOrEdit(input.GLSecurityDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_GLSecurityHeader_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _gLSecurityHeaderRepository.DeleteAsync(input.Id);
            await _gLSecurityDetailRepository.Delete(input.Id);
        }

        public async Task<FileDto> GetGLSecurityHeaderToExcel(GetAllGLSecurityHeaderForExcelInput input)
        {

            var filteredGLSecurityHeaders = _gLSecurityHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.UserID.Contains(input.Filter) || e.UserName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserIDFilter), e => e.UserID.Trim().ToLower() == input.UserIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.Trim().ToLower() == input.UserNameFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.Trim().ToLower() == input.AudtUserFilter.Trim().ToLower())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.Trim().ToLower() == input.CreatedByFilter.Trim().ToLower())
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.AudtDate <= input.MaxCreatedDateFilter);

            var query = (from o in filteredGLSecurityHeaders
                         select new GetGLSecurityHeaderForViewDto()
                         {
                             GLSecurityHeader = new GLSecurityHeaderDto
                             {
                                 UserID = o.UserID,
                                 UserName = o.UserName,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreatedDate = o.CreatedDate,
                                 Id = o.Id
                             }
                         });


            var GLSecurityHeaderListDtos = await query.ToListAsync();

            return _gLSecurityHeadersExcelExporter.ExportToFile(GLSecurityHeaderListDtos);
        }
        public string GetUserInfo(string name)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            int TenantId = (int)AbpSession.TenantId;           

            string userName = null;
            using (SqlConnection myConnection = new SqlConnection(str))
            {
                string oString = "Select * from AbpUsers where TenantId=@TenantId and Name=@Name";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                oCmd.Parameters.AddWithValue("@TenantId", TenantId);
                oCmd.Parameters.AddWithValue("@Name", name);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        userName = oReader["UserName"].ToString();
                    }

                    myConnection.Close();
                }
            }
            return userName;
        }

    }

    
}