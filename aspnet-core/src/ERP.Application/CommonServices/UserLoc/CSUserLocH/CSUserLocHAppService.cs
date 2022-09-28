using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.CommonServices.UserLoc.CSUserLocH.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.CommonServices.UserLoc.CSUserLocD.Dtos;

namespace ERP.CommonServices.UserLoc.CSUserLocH
{
    [AbpAuthorize(AppPermissions.Pages_CSUserLocH)]
    public class CSUserLocHAppService : ERPAppServiceBase, ICSUserLocHAppService
    {
        private readonly IRepository<CSUserLocH> _csUserLocHRepository;
        private readonly IRepository<CSUserLocD.CSUserLocD> _csUserLocDRepository;
        private readonly IRepository<User, long> _userRepository;

        public CSUserLocHAppService(IRepository<CSUserLocH> csUserLocHRepository, IRepository<User, long> userRepository, IRepository<CSUserLocD.CSUserLocD> csUserLocDRepository)
        {
            _csUserLocHRepository = csUserLocHRepository;
            _csUserLocDRepository = csUserLocDRepository;
            _userRepository = userRepository;

        }

        public async Task<PagedResultDto<GetCSUserLocHForViewDto>> GetAll(GetAllCSUserLocHInput input)
        {

            var filteredCSUserLocH = _csUserLocHRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.UserId.Contains(input.Filter) || e.UserId.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var pagedAndFilteredCSUserLocH = filteredCSUserLocH
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var csUserLocH = from o in pagedAndFilteredCSUserLocH
                             select new
                             {

                                 o.TypeID,
                                 o.UserId,
                                 o.CreatedBy,
                                 o.CreateDate,
                                 o.AudtUser,
                                 o.AudtDate,
                                 Id = o.Id
                             };

            var totalCount = await filteredCSUserLocH.CountAsync();

            var dbList = await csUserLocH.ToListAsync();
            var results = new List<GetCSUserLocHForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCSUserLocHForViewDto()
                {
                    CSUserLocH = new CSUserLocHDto
                    {

                        TypeID = o.TypeID,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        AudtUser = o.AudtUser,
                        UserId=o.UserId,
                        AudtDate = o.AudtDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCSUserLocHForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCSUserLocHForViewDto> GetCSUserLocHForView(int id)
        {
            var csUserLocH = await _csUserLocHRepository.GetAsync(id);

            var output = new GetCSUserLocHForViewDto { CSUserLocH = ObjectMapper.Map<CSUserLocHDto>(csUserLocH) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CSUserLocH_Edit)]
        public async Task<GetCSUserLocHForEditOutput> GetCSUserLocHForEdit(EntityDto input)
        {
            var csUserLocH = await _csUserLocHRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCSUserLocHForEditOutput { CSUserLocH = ObjectMapper.Map<CreateOrEditCSUserLocHDto>(csUserLocH) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCSUserLocHDto input)
        {
            if (input.Id == null)
            {
                var CheckId = ExistUserName(input.UserId, input.TypeID);
                if (CheckId==0)
                {
                await Create(input);
                }
                else
                {
                    input.Id = CheckId;
                    await Update(input);
                }
            }
            else
            {
                await Update(input);
            }
        }
        public int? ExistUserName(string userid,short? Typ)
        {
            try
            {
                var id = 0;
                var Existdata = _csUserLocHRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == Typ && c.UserId == userid).FirstOrDefault();
                if (Existdata!=null)
                {
                    id = Existdata.Id;
                }
                return id;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        [AbpAuthorize(AppPermissions.Pages_CSUserLocH_Create)]
        protected virtual async Task Create(CreateOrEditCSUserLocHDto input)
        {
            var csUserLocH = ObjectMapper.Map<CSUserLocH>(input);

            if (AbpSession.TenantId != null)
            {
                csUserLocH.TenantId = (int)AbpSession.TenantId;
                csUserLocH.CreateDate = DateTime.Now;
                csUserLocH.CreatedBy= _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            }

         var GetId=   await _csUserLocHRepository.InsertAndGetIdAsync(csUserLocH);
            if (input.UserLocD != null)
            {
                foreach (var data in input.UserLocD)
                {

                    var Detail = ObjectMapper.Map<CSUserLocD.CSUserLocD>(data);
                    if (AbpSession.TenantId != null)
                    {
                        Detail.TenantId = (int)AbpSession.TenantId;
                    }
                   Detail.DetId = GetId;
                   Detail.LocId = data.LocId;
                   Detail.TypeID = input.TypeID;
                    Detail.UserID = input.UserId;
                    Detail.Status = data.Status;
                    await _csUserLocDRepository.InsertAsync(Detail);


                }
            }

        }

        [AbpAuthorize(AppPermissions.Pages_CSUserLocH_Edit)]
        protected virtual async Task Update(CreateOrEditCSUserLocHDto input)
        {
            input.AudtDate = DateTime.Now;
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var csUserLocH = await _csUserLocHRepository.FirstOrDefaultAsync((int)input.Id);
            input.CreateDate = csUserLocH.CreateDate;
            input.CreatedBy = csUserLocH.CreatedBy;
            ObjectMapper.Map(input, csUserLocH);
            //For  Detail
            var Det = await _csUserLocDRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DetId == input.Id).ToListAsync();
            if (Det != null)
            {
                foreach (var item in Det)
                {
                    await _csUserLocDRepository.DeleteAsync(item);
                }
            }


            if (input.UserLocD != null)
            {
                foreach (var data in input.UserLocD)
                {

                    var Detail = ObjectMapper.Map<CSUserLocD.CSUserLocD>(data);

                    if (AbpSession.TenantId != null)
                    {
                        Detail.TenantId = (int)AbpSession.TenantId;
                    }
                    Detail.DetId = csUserLocH.Id;
                    Detail.UserID = input.UserId;
                    Detail.TypeID = input.TypeID;
                    Detail.LocId = data.LocId;
                    Detail.Status = data.Status;
                    await _csUserLocDRepository.InsertAsync(Detail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CSUserLocH_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _csUserLocHRepository.DeleteAsync(input.Id);
        }
        public CreateOrEditCSUserLocHDto GetUserLoc(string userId,int type)
        {
            CreateOrEditCSUserLocHDto header = new CreateOrEditCSUserLocHDto();
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<CreateOrEditCSUserLocDDto> LocList = new List<CreateOrEditCSUserLocDDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                SqlCommand cmd;
                cmd = new SqlCommand("sp_UserLocation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Type", type);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        CreateOrEditCSUserLocDDto Data = new CreateOrEditCSUserLocDDto()
                        {
                            LocId = rdr["LocId"] is DBNull ? 0 : Convert.ToInt32(rdr["LocId"]),
                            Locdesc = rdr["LocName"] is DBNull ? "" : rdr["LocName"].ToString(),
                            Status = rdr["Cansee"] is DBNull ? false : Convert.ToBoolean(rdr["Cansee"]),
                        };

                        LocList.Add(Data);

                    }

                }
            }
            header.UserLocD = LocList;
            return header;
        }
    }
}