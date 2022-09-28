using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.GLDocRev.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace ERP.GeneralLedger.SetupForms.GLDocRev
{
    [AbpAuthorize(AppPermissions.Pages_GLDocRev)]
    public class GLDocRevAppService : ERPAppServiceBase, IGLDocRevAppService
    {
        private readonly IRepository<GLDocRev> _glDocRevRepository;

        public GLDocRevAppService(IRepository<GLDocRev> glDocRevRepository)
        {
            _glDocRevRepository = glDocRevRepository;

        }

        public async Task<PagedResultDto<GetGLDocRevForViewDto>> GetAll(GetAllGLDocRevInput input)
        {

            var filteredGLDocRev = _glDocRevRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.PostedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID == input.BookIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinFmtDocNoFilter != null, e => e.FmtDocNo >= input.MinFmtDocNoFilter)
                        .WhereIf(input.MaxFmtDocNoFilter != null, e => e.FmtDocNo <= input.MaxFmtDocNoFilter)
                        .WhereIf(input.MinDocYearFilter != null, e => e.DocYear >= input.MinDocYearFilter)
                        .WhereIf(input.MaxDocYearFilter != null, e => e.DocYear <= input.MaxDocYearFilter)
                        .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                        .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                        .WhereIf(input.MinNewDocDateFilter != null, e => e.NewDocDate >= input.MinNewDocDateFilter)
                        .WhereIf(input.MaxNewDocDateFilter != null, e => e.NewDocDate <= input.MaxNewDocDateFilter)
                        .WhereIf(input.MinNewDocNoFilter != null, e => e.NewDocNo >= input.MinNewDocNoFilter)
                        .WhereIf(input.MaxNewDocNoFilter != null, e => e.NewDocNo <= input.MaxNewDocNoFilter)
                        .WhereIf(input.MinNewFmtDocNoFilter != null, e => e.NewFmtDocNo >= input.MinNewFmtDocNoFilter)
                        .WhereIf(input.MaxNewFmtDocNoFilter != null, e => e.NewFmtDocNo <= input.MaxNewFmtDocNoFilter)
                        .WhereIf(input.MinNewDocYearFilter != null, e => e.NewDocYear >= input.MinNewDocYearFilter)
                        .WhereIf(input.MaxNewDocYearFilter != null, e => e.NewDocYear <= input.MaxNewDocYearFilter)
                        .WhereIf(input.MinNewDocMonthFilter != null, e => e.NewDocMonth >= input.MinNewDocMonthFilter)
                        .WhereIf(input.MaxNewDocMonthFilter != null, e => e.NewDocMonth <= input.MaxNewDocMonthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(input.PostedFilter.HasValue && input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter), e => e.PostedBy == input.PostedByFilter)
                        .WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
                        .WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter);

            var pagedAndFilteredGLDocRev = filteredGLDocRev
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var glDocRev = from o in pagedAndFilteredGLDocRev
                           select new
                           {

                               o.BookID,
                               o.DocNo,
                               o.DocDate,
                               o.FmtDocNo,
                               o.DocYear,
                               o.DocMonth,
                               o.NewDocDate,
                               o.NewDocNo,
                               o.NewFmtDocNo,
                               o.NewDocYear,
                               o.NewDocMonth,
                               o.Narration,
                               o.Posted,
                               o.PostedBy,
                               o.PostedDate,
                               Id = o.Id
                           };

            var totalCount = await filteredGLDocRev.CountAsync();

            var dbList = await glDocRev.ToListAsync();
            var results = new List<GetGLDocRevForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetGLDocRevForViewDto()
                {
                    GLDocRev = new GLDocRevDto
                    {

                        BookID = o.BookID,
                        DocNo = o.DocNo,
                        DocDate = o.DocDate,
                        FmtDocNo = o.FmtDocNo,
                        DocYear = o.DocYear,
                        DocMonth = o.DocMonth,
                        NewDocDate = o.NewDocDate,
                        NewDocNo = o.NewDocNo,
                        NewFmtDocNo = o.NewFmtDocNo,
                        NewDocYear = o.NewDocYear,
                        NewDocMonth = o.NewDocMonth,
                        Narration = o.Narration,
                        Posted = o.Posted,
                        PostedBy = o.PostedBy,
                        PostedDate = o.PostedDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetGLDocRevForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetGLDocRevForViewDto> GetGLDocRevForView(int id)
        {
            var glDocRev = await _glDocRevRepository.GetAsync(id);

            var output = new GetGLDocRevForViewDto { GLDocRev = ObjectMapper.Map<GLDocRevDto>(glDocRev) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLDocRev_Edit)]
        public async Task<GetGLDocRevForEditOutput> GetGLDocRevForEdit(EntityDto input)
        {
            var glDocRev = await _glDocRevRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGLDocRevForEditOutput { GLDocRev = ObjectMapper.Map<CreateOrEditGLDocRevDto>(glDocRev) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGLDocRevDto input)
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

        [AbpAuthorize(AppPermissions.Pages_GLDocRev_Create)]
        protected virtual async Task Create(CreateOrEditGLDocRevDto input)
        {
            var glDocRev = ObjectMapper.Map<GLDocRev>(input);

            if (AbpSession.TenantId != null)
            {
                glDocRev.TenantId = (int)AbpSession.TenantId;
            }

            await _glDocRevRepository.InsertAsync(glDocRev);

        }

        [AbpAuthorize(AppPermissions.Pages_GLDocRev_Edit)]
        protected virtual async Task Update(CreateOrEditGLDocRevDto input)
        {
            var glDocRev = await _glDocRevRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, glDocRev);

        }

        [AbpAuthorize(AppPermissions.Pages_GLDocRev_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _glDocRevRepository.DeleteAsync(input.Id);
        }
        [HttpGet]
        public int GetDocNo()
        {
            try
            {
                var result = _glDocRevRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId).Max(c => c.MaxDocNo).GetValueOrDefault();
                var docNo = 0;
                if (result == 0)
                {
                    docNo = 1;
                }
                else
                {
                    docNo = result + 1;
                }
                return docNo;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }

        [HttpPost]

        public string Process(CreateOrEditGLDocRevDto input)
        {
            var docrev = _glDocRevRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            string msg = "";
            try
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"];
                using (SqlConnection cn = new SqlConnection(str))
                {

                    SqlCommand cmd;
                    cmd = new SqlCommand("GLvoucherReversal", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OLDDETID", input.DetId);
                    cmd.Parameters.AddWithValue("@BOOKID", input.BookID);
                    cmd.Parameters.AddWithValue("@DOCNO", input.NewDocNo);
                    cmd.Parameters.AddWithValue("@FMTDOCNO", input.NewFmtDocNo);
                    cmd.Parameters.AddWithValue("@DOCMONTH", input.NewDocMonth);
                    cmd.Parameters.AddWithValue("@DOCDATE", input.NewDocDate);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);


                    cn.Open();
                    cmd.ExecuteNonQuery();
                    docrev.Posted = true;
                    var transh = _glDocRevRepository.FirstOrDefault((int)docrev.Id);
                    ObjectMapper.Map(docrev, transh);
                    cn.Close();
                    return msg = "save";
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}