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
using ERP.EntityFrameworkCore;
using Abp.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.SetupForms_SubControlDetails)]
    public class SubControlDetailsAppService : ERPAppServiceBase, ISubControlDetailsAppService
    {
        private readonly IRepository<SubControlDetail> _subControlDetailRepository;
        private readonly ISubControlDetailsExcelExporter _subControlDetailsExcelExporter;
        private readonly IRepository<ControlDetail> _lookup_controlDetailRepository;
        private readonly IRepository<Segmentlevel3> _SegmentLevel3Repository;
        private readonly IRepository<GLOption> _glOptionsRepository;


        public SubControlDetailsAppService(IRepository<SubControlDetail> subControlDetailRepository, ISubControlDetailsExcelExporter subControlDetailsExcelExporter, IRepository<ControlDetail> lookup_controlDetailRepository, IRepository<Segmentlevel3> SegmentLevel3Repository, IRepository<GLOption> glOptionsRepository)
        {
            _subControlDetailRepository = subControlDetailRepository;
            _subControlDetailsExcelExporter = subControlDetailsExcelExporter;
            _lookup_controlDetailRepository = lookup_controlDetailRepository;
            _SegmentLevel3Repository = SegmentLevel3Repository;
            _glOptionsRepository = glOptionsRepository;
        }

        public async Task<PagedResultDto<GetSubControlDetailForViewDto>> GetAll(GetAllSubControlDetailsInput input)
        {

            var query = from o in _subControlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        join o1 in _lookup_controlDetailRepository.GetAll() on new { Seg1ID = o.Seg2ID.Substring(0, o.Seg2ID.Length - 4), o.TenantId } equals new { o1.Seg1ID, o1.TenantId }
                        select new
                        {
                            o.Id,
                            o.Seg2ID,
                            o.SegmentName,
                            o.OldCode,
                            o1.Seg1ID,
                            Seg1Name = o1.SegmentName
                        };



            var filteredSubControlDetails = query
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg2ID.Contains(input.Filter) || e.SegmentName.Contains(input.Filter) || e.Seg1Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg2IDFilter), e => e.Seg2ID == input.Seg2IDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SegmentNameFilter), e => e.SegmentName.ToLower() == input.SegmentNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlDetailIdFilter), e => e.Seg1Name.ToLower() == input.ControlDetailIdFilter.ToLower().Trim());

            var pagedAndFilteredSubControlDetails = filteredSubControlDetails
                .OrderBy(input.Sorting ?? "Seg2ID desc")
                .PageBy(input);




            //change id
            var subControlDetails = from o in pagedAndFilteredSubControlDetails
                                    join o1 in _lookup_controlDetailRepository.GetAll() on o.Seg2ID.Substring(0, o.Seg2ID.Length - 4) equals o1.Seg1ID into j1
                                    from s1 in j1.DefaultIfEmpty().Where(x => x.TenantId == AbpSession.TenantId)

                                    select new GetSubControlDetailForViewDto()
                                    {
                                        SubControlDetail = new SubControlDetailDto
                                        {
                                            Seg2ID = o.Seg2ID,
                                            SegmentName = o.SegmentName,
                                            OldCode = o.OldCode,
                                            Id = o.Id
                                        },
                                        ControlDetaildesc = s1 == null ? "" : s1.SegmentName.ToString()
                                    };

            var totalCount = await filteredSubControlDetails.CountAsync();

            return new PagedResultDto<GetSubControlDetailForViewDto>(
                totalCount,
                await subControlDetails.ToListAsync()
            );
        }

        public async Task<GetSubControlDetailForViewDto> GetSubControlDetailForView(int id)
        {
            var subControlDetail = await _subControlDetailRepository.GetAsync(id);

            var output = new GetSubControlDetailForViewDto { SubControlDetail = ObjectMapper.Map<SubControlDetailDto>(subControlDetail) };

            string[] contID = output.SubControlDetail.Seg2ID.Split('-');

            if (contID[0] != null)
            {
                var _lookupControlDetail = await _lookup_controlDetailRepository.FirstOrDefaultAsync(x => x.Seg1ID == (string)contID[0] && x.TenantId == AbpSession.TenantId);
                output.ControlDetailId = _lookupControlDetail.Seg1ID.ToString();
                output.ControlDetaildesc = _lookupControlDetail.SegmentName;
            }
            return output;
        }

        [AbpAuthorize(AppPermissions.SetupForms_SubControlDetails_Edit)]
        public async Task<GetSubControlDetailForEditOutput> GetSubControlDetailForEdit(EntityDto input)
        {
            var subControlDetail = await _subControlDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSubControlDetailForEditOutput { SubControlDetail = ObjectMapper.Map<CreateOrEditSubControlDetailDto>(subControlDetail) };
            string[] contID = output.SubControlDetail.Seg2ID.Split('-');
            if (contID[0] != null)
            {
                var _lookupControlDetail = await _lookup_controlDetailRepository.FirstOrDefaultAsync(x => x.Seg1ID == (string)contID[0] && x.TenantId == AbpSession.TenantId);
                output.ControlDetailId = _lookupControlDetail.Seg1ID.ToString();
                output.ControlDetailDesc = _lookupControlDetail.SegmentName;
            }
            output.SubControlDetail.Seg2ID = contID[1];
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSubControlDetailDto input)
        {


            if (input.Flag != true)
            {
                var subControlDetail = await _subControlDetailRepository.FirstOrDefaultAsync(x => x.Seg2ID == input.SegmantID1.Trim() + '-' + input.Seg2ID.Trim() && x.TenantId == AbpSession.TenantId);

                if (subControlDetail != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Segment ID " + input.Seg2ID + " already taken....");
                }
                await Create(input);
            }
            else
            {
                await Update(input);

               // updateBSPLSettings(input);

            }
        }
        [HttpPost]
        public async Task updateaccount(CreateOrEditSubControlDetailDto input)
        {
            try
            {
                updateSettings(input);
            }
            catch (Exception ex)
            {


            }
        }

        [AbpAuthorize(AppPermissions.SetupForms_SubControlDetails_Create)]
        protected virtual async Task Create(CreateOrEditSubControlDetailDto input)
        {
            var subControlDetail = ObjectMapper.Map<SubControlDetail>(input);


            if (AbpSession.TenantId != null)
            {
                subControlDetail.TenantId = (int)AbpSession.TenantId;
            }
            subControlDetail.Seg2ID = input.SegmantID1 + "-" + input.Seg2ID;
            subControlDetail.Seg1ID = input.SegmantID1;
            //subControlDetail.Seg2ID = GetSubControlID(input.SegmantID1);
            await _subControlDetailRepository.InsertAsync(subControlDetail);

            var isAutoSeg = _glOptionsRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId).AutoSeg3;

            if (isAutoSeg)
            {
                Segmentlevel3 segmentLevel3 = new Segmentlevel3();

                segmentLevel3.TenantId = (int)AbpSession.TenantId;
                segmentLevel3.Seg3ID = subControlDetail.Seg2ID + "-01";
                segmentLevel3.SegmentName = "N/A";
                await _SegmentLevel3Repository.InsertAsync(segmentLevel3);

            }

        }

        [AbpAuthorize(AppPermissions.SetupForms_SubControlDetails_Edit)]
        protected virtual async Task Update(CreateOrEditSubControlDetailDto input)
        {
            input.Seg2ID = input.SegmantID1 + "-" + input.Seg2ID;
            var subControlDetail = await _subControlDetailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, subControlDetail);
        }

        [AbpAuthorize(AppPermissions.SetupForms_SubControlDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            try
            {
                await _subControlDetailRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message, "Error");
            }

        }

        public async Task<FileDto> GetSubControlDetailsToExcel(GetAllSubControlDetailsForExcelInput input)
        {

            var filteredSubControlDetails = _subControlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SegmentName.Contains(input.Filter) || e.OldCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SegmentNameFilter), e => e.SegmentName.ToLower() == input.SegmentNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim());



            // change id here
            var query = (from o in filteredSubControlDetails
                         join o1 in _lookup_controlDetailRepository.GetAll() on o.Seg2ID.Substring(0, o.Seg2ID.Length - 4) equals o1.Seg1ID into j1
                         from s1 in j1.DefaultIfEmpty().Where(x => x.TenantId == AbpSession.TenantId)

                         select new GetSubControlDetailForViewDto()
                         {
                             SubControlDetail = new SubControlDetailDto
                             {
                                 Seg2ID = o.Seg2ID,
                                 SegmentName = o.SegmentName,
                                 OldCode = o.OldCode,
                                 Id = o.Id
                             },
                             ControlDetailId = s1 == null ? "" : s1.Seg1ID.ToString(),
                             ControlDetaildesc = s1 == null ? "" : s1.SegmentName

                         });


            var subControlDetailListDtos = await query.ToListAsync();

            return _subControlDetailsExcelExporter.ExportToFile(subControlDetailListDtos);
        }

        [AbpAuthorize(AppPermissions.SetupForms_SubControlDetails)]
        public async Task<PagedResultDto<SubControlDetailControlDetailLookupTableDto>> GetAllControlDetailForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_controlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter), e => false ||
                   e.Seg1ID.ToString().Contains(input.Filter) || e.SegmentName.ToLower().ToString().Contains(input.Filter.ToLower()));



            var totalCount = await query.CountAsync();

            var controlDetailList = await query
                .OrderBy(input.Sorting ?? "Seg1ID desc")
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<SubControlDetailControlDetailLookupTableDto>();
            foreach (var controlDetail in controlDetailList)
            {
                lookupTableDtoList.Add(new SubControlDetailControlDetailLookupTableDto
                {
                    Id = controlDetail.Seg1ID,
                    DisplayName = controlDetail.SegmentName
                });
            }

            return new PagedResultDto<SubControlDetailControlDetailLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public string GetSubControlID(string id)
        {

            var filteredSubControlDetails = _subControlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //string x = "" +  id + "%";
            string[] xstring;
            string xformat = "";
            string nString = "";
            string finalSting = "";
            var getMaxID = filteredSubControlDetails.Where(c => EF.Functions.Like(c.Seg2ID, $"{id}%") && c.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Seg2ID).Select(x => x.Seg2ID).FirstOrDefault();

            if (getMaxID == null)
            {

                xformat = string.Format("{0:000}", 1);
                finalSting = xformat; //id + "-" + xformat;
            }
            else
            {
                xstring = getMaxID.Split('-');
                nString = xstring[1];
                if (Convert.ToInt32(nString) + 1 > 999)
                {
                    xformat = string.Format("{0:000}", 999);
                    finalSting = xformat; //id + "-" + xformat;
                }
                else
                {
                    xformat = string.Format("{0:000}", Convert.ToInt32(nString) + 1);
                    finalSting = xformat; //id + "-" + xformat;
                }
            }

            return finalSting;
        }
        public void updateSettings(CreateOrEditSubControlDetailDto input)
        {

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("UpDateCtg", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Segment2", input.SegmantID1 + "-" + input.Seg2ID);
                cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                if (input.Acctype == "PL")
                {
                    cmd.Parameters.AddWithValue("@accType", input.AccountType);
                    cmd.Parameters.AddWithValue("@accHead", input.AccountHeader);
                    cmd.Parameters.AddWithValue("@sortOrder", input.SortOrder);
                    cmd.Parameters.AddWithValue("@Type", input.Acctype);
                }
                else if (input.Acctype == "BS")
                {
                    cmd.Parameters.AddWithValue("@accType", input.AccountBSType);
                    cmd.Parameters.AddWithValue("@accHead", input.AccountBSHeader);
                    cmd.Parameters.AddWithValue("@sortOrder", input.SortBSOrder);
                    cmd.Parameters.AddWithValue("@Type", input.Acctype);
                }
                else if (input.Acctype == "CF")
                {
                    cmd.Parameters.AddWithValue("@accType", input.AccountCFType);
                    cmd.Parameters.AddWithValue("@accHead", input.AccountCFHeader);
                    cmd.Parameters.AddWithValue("@sortOrder", input.SortCFOrder);
                    cmd.Parameters.AddWithValue("@Type", input.Acctype);
                }


                cn.Open();
                cmd.ExecuteNonQuery();
                //cn.Close();
            }
        }
        public void updateBSPLSettings(CreateOrEditSubControlDetailDto input)
        {

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("dbo.UpDateBSCtg", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Segment2", input.Seg2ID);
                cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@accBsType", input.AccountBSType);
                cmd.Parameters.AddWithValue("@accBsHead", input.AccountBSHeader);
                cmd.Parameters.AddWithValue("@sortBsOrder", input.SortBSOrder);
                cmd.Parameters.AddWithValue("@accType", input.AccountType);
                cmd.Parameters.AddWithValue("@accHead", input.AccountHeader);
                cmd.Parameters.AddWithValue("@sortOrder", input.SortOrder);

                cn.Open();
                cmd.ExecuteNonQuery();
         //       // cn.Close();
            }
        }



    }
}