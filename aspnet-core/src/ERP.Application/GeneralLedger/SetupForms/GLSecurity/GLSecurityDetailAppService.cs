using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.GLSecurity.Exporting;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ERP.GeneralLedger.SetupForms.GLSecurity
{
    [AbpAuthorize(AppPermissions.Pages_GLSecurityDetail)]
    public class GLSecurityDetailAppService : ERPAppServiceBase, IGLSecurityDetailAppService
    {
        private readonly IRepository<GLSecurityDetail> _gLSecurityDetailRepository;
        private readonly IGLSecurityDetailExcelExporter _gLSecurityDetailExcelExporter;


        public GLSecurityDetailAppService(
            IRepository<GLSecurityDetail> gLSecurityDetailRepository,
            IGLSecurityDetailExcelExporter gLSecurityDetailExcelExporter)
        {
            _gLSecurityDetailRepository = gLSecurityDetailRepository;
            _gLSecurityDetailExcelExporter = gLSecurityDetailExcelExporter;

        }

        public async Task<PagedResultDto<GetGLSecurityDetailForViewDto>> GetAll(GetAllGLSecurityDetailInput input)
        {

            var filteredGLSecurityDetail = _gLSecurityDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.UserID.Contains(input.Filter) || e.BeginAcc.Contains(input.Filter) || e.EndAcc.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserIDFilter), e => e.UserID.Trim().ToLower() == input.UserIDFilter.Trim().ToLower())
                        .WhereIf(input.CanSeeFilter > -1, e => System.Convert.ToInt32(e.CanSee) == input.CanSeeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BeginAccFilter), e => e.BeginAcc.Trim().ToLower() == input.BeginAccFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndAccFilter), e => e.EndAcc.Trim().ToLower() == input.EndAccFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var pagedAndFilteredGLSecurityDetail = filteredGLSecurityDetail
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var gLSecurityDetail = from o in pagedAndFilteredGLSecurityDetail
                                   select new GetGLSecurityDetailForViewDto()
                                   {
                                       GLSecurityDetail = new GLSecurityDetailDto
                                       {
                                           DetID = o.DetID,
                                           UserID = o.UserID,
                                           CanSee = o.CanSee,
                                           BeginAcc = o.BeginAcc,
                                           EndAcc = o.EndAcc,
                                           AudtUser = o.AudtUser,
                                           AudtDate = o.AudtDate,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredGLSecurityDetail.CountAsync();

            return new PagedResultDto<GetGLSecurityDetailForViewDto>(
                totalCount,
                await gLSecurityDetail.ToListAsync()
            );
        }


        [AbpAuthorize(AppPermissions.Pages_GLSecurityDetail_Edit)]
        public async Task<GetGLSecurityDetailForEditOutput> GetGLSecurityDetailForEdit(int ID)
        {
            var gLSecurityDetail = await _gLSecurityDetailRepository.GetAllListAsync(x => x.DetID == ID && x.TenantId == AbpSession.TenantId);

            var gLSecurityDetails = from o in gLSecurityDetail
                                    select new CreateOrEditGLSecurityDetailDto
                                    {
                                        DetID = o.DetID,
                                        UserID = o.UserID,
                                        CanSee = o.CanSee,
                                        BeginAcc = o.BeginAcc,
                                        EndAcc = o.EndAcc,
                                        AudtUser = o.AudtUser,
                                        AudtDate = o.AudtDate,
                                        Id = o.Id

                                    };


            var output = new GetGLSecurityDetailForEditOutput { GLSecurityDetail = ObjectMapper.Map<ICollection<CreateOrEditGLSecurityDetailDto>>(gLSecurityDetails) };

            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditGLSecurityDetailDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }
        }

        public async Task<GetGLSecurityDetailForViewDto> GetGLSecurityDetailForView(int id)
        {
            var gLSecurityDetail = await _gLSecurityDetailRepository.GetAsync(id);

            var output = new GetGLSecurityDetailForViewDto { GLSecurityDetail = ObjectMapper.Map<GLSecurityDetailDto>(gLSecurityDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLSecurityDetail_Create)]
        protected virtual async Task Create(CreateOrEditGLSecurityDetailDto input)
        {
            GLSecurityDetail gLSecurityDetail = new GLSecurityDetail();
            gLSecurityDetail.DetID = input.DetID;
            gLSecurityDetail.UserID = input.UserID;
            gLSecurityDetail.CanSee = input.CanSee;
            gLSecurityDetail.BeginAcc = input.BeginAcc;
            gLSecurityDetail.EndAcc = input.EndAcc;
            gLSecurityDetail.AudtUser = input.AudtUser;
            gLSecurityDetail.AudtDate = input.AudtDate;

            //var gLSecurityDetail = ObjectMapper.Map<gLSecurityDetail>(input);

            if (AbpSession.TenantId != null)
            {
                gLSecurityDetail.TenantId = (int)AbpSession.TenantId;
            }


            await _gLSecurityDetailRepository.InsertAsync(gLSecurityDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_GLSecurityDetail_Edit)]
        protected virtual async Task Update(CreateOrEditGLSecurityDetailDto input)
        {
            GLSecurityDetail gLSecurityDetail = new GLSecurityDetail();
            gLSecurityDetail.DetID = input.DetID;
            gLSecurityDetail.DetID = input.DetID;
            gLSecurityDetail.UserID = input.UserID;
            gLSecurityDetail.CanSee = input.CanSee;
            gLSecurityDetail.BeginAcc = input.BeginAcc;
            gLSecurityDetail.EndAcc = input.EndAcc;
            gLSecurityDetail.AudtUser = input.AudtUser;
            gLSecurityDetail.AudtDate = input.AudtDate;

            await _gLSecurityDetailRepository.InsertAsync(gLSecurityDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_GLSecurityDetail_Delete)]
        public async Task Delete(int input)
        {
            await _gLSecurityDetailRepository.DeleteAsync(x => x.DetID == input);
        }
        [AbpAuthorize(AppPermissions.Pages_GLSecurityDetail_Process)]
        public string ProcessGLSecurity(string userID, int detID)
        {
            int TenantId = (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("FillGLSecDtl", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@TenantID", TenantId);
                cmd.Parameters.AddWithValue("@BeginAcc", "");
                cmd.Parameters.AddWithValue("@EndAcc", "");
                cmd.Parameters.AddWithValue("@ROWID", 0);
                cmd.Parameters.AddWithValue("@CanSee", 0);
                cn.Open();
                cmd.ExecuteNonQuery();
             //   // cn.Close();
            }
            return "done";
        }


        public async Task<FileDto> GetGLSecurityDetailToExcel(GetAllGLSecurityDetailForExcelInput input)
        {

            var filteredGLSecurityDetail = _gLSecurityDetailRepository.GetAll()
                         .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.UserID.Contains(input.Filter) || e.BeginAcc.Contains(input.Filter) || e.EndAcc.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserIDFilter), e => e.UserID.Trim().ToLower() == input.UserIDFilter.Trim().ToLower())
                        .WhereIf(input.CanSeeFilter > -1, e => System.Convert.ToInt32(e.CanSee) == input.CanSeeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BeginAccFilter), e => e.BeginAcc.Trim().ToLower() == input.BeginAccFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndAccFilter), e => e.EndAcc.Trim().ToLower() == input.EndAccFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var query = (from o in filteredGLSecurityDetail
                         select new GetGLSecurityDetailForViewDto()
                         {
                             GLSecurityDetail = new GLSecurityDetailDto
                             {
                                 DetID = o.DetID,
                                 UserID = o.UserID,
                                 CanSee = o.CanSee,
                                 BeginAcc = o.BeginAcc,
                                 EndAcc = o.EndAcc,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 Id = o.Id
                             }
                         });


            var gLSecurityDetailListDtos = await query.ToListAsync();

            return _gLSecurityDetailExcelExporter.ExportToFile(gLSecurityDetailListDtos);
        }





    }
}